using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Timers;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace VNCServer
{
    public partial class StartingWindow : Form
    {
        //TCP-server attributes:
        private IPEndPoint localEndPoint;
        private TcpListener TCPServer;
        private TcpClient client;
        private Thread TCPServerThread;
        private ServerTestAssistant isTheserverRunning;
        //VNC server attributes:
        //private NVNC.VncServer server;
        //private Thread serverThread;
        private string applicationDirectory;
        private Process serverProcess;
        private bool connectionBroken = false;

        private readonly int STILLCONNECTEDCODE = 111;
        private readonly int DISCONNECTCODE = 666;
        private readonly char FRAME = '+';
        private int statusCodeToSend;

        public StartingWindow()
        {
            InitializeComponent();
            this.applicationDirectory = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            //server.Name = "127.0.0.1";
            //server.Password = "";
            /*
            this.server.Port = 5900;
            //this.server.Name = "192.168.0.101";
            this.server.Name = "";
            this.server.Password = "";
             */
        }

        /*private NVNC.VncServer instantiateServer() //Instantiates a VNC server using the NVNC library.
        {
            NVNC.VncServer returnedServer = new NVNC.VncServer();
            returnedServer.Port = 5900;
            returnedServer.Name = "Servah";
            returnedServer.Password = "";
            return returnedServer;
        }*/

        private void startServerBt_Click(object sender, EventArgs e)
        {
            /*
            this.server = new NVNC.VncServer();
            this.server.Port = 5900;
            this.server.Name = "Servah";
            this.server.Password = "";
            */
            //this.server = instantiateServer();
            /*this.serverThread = new Thread(() => this.server.Start());
            this.serverThread.Start();*/
            this.serverProcess = new Process();
            this.TCPServerInstantiate();
            this.serverProcess = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo(this.applicationDirectory + "\\NagishVNCServerExecutable");
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            this.serverProcess.StartInfo = startInfo;
            this.serverProcess.Start();
            this.stopServerBt.Enabled = true;
            this.startServerBt.Enabled = false;
        }

        private void stopServerBt_Click(object sender, EventArgs e)
        {
            this.stopServer(); //Stops the VNC server's process.
            this.TCPServer.Server.Close();
            this.TCPServer.Stop();
            this.stopServerBt.Enabled = false;
            this.startServerBt.Enabled = true;
        }

        private void stopServer()
        {
            this.statusCodeToSend = this.DISCONNECTCODE;
            /*try
            {
                this.server.Stop(); //This method isn't, as of yet, properly implemented in NVNC. Instead, we'll open a server on a seperate process and kill it.
            }
            catch (NullReferenceException)
            {
                //Meaning no one had connected to the server, so no connection can be stopped.
                //Nothing to do about that, basically, so just catching the exception.
            }*/
            foreach (Process serverProc in Process.GetProcessesByName("NagishVNCServerExecutable"))
            {
                serverProc.Kill(); //Killing all the NagishServerExecutable processes. If some other guy uses this name for his process, then we'll also close his process. He stole our name. He deserves that.
            }
            if (!this.serverProcess.HasExited) //As a way to, umm... secure the kill? :P
            {
                if (!this.serverProcess.WaitForExit(200))
                {
                    //try
                    //{
                    this.serverProcess.Kill();
                    if (!this.serverProcess.HasExited)
                    {
                        if (Process.GetProcessesByName("NagishServerExecutable").Length > 0)
                        {
                            MessageBox.Show("There seems to have been an error with closing the server.\nTry, uhh, taskmgr => processes => end task for NagishServerExecutable.\nWe're so very professional.");
                        }
                    }
                    //}
                    //catch (
                }
            }
        }

        private void disconnectStatusCodeReceived()
        {
            Action disconnect = () =>
            {
                this.stopServer();
                this.TCPServer.Server.Close();
                this.TCPServer.Stop();
                this.stopServerBt.Enabled = false;
                this.startServerBt.Enabled = true;
            };
            this.Invoke(disconnect);
        }

        private void TCPServerInstantiate()
        {
            localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6161);
            TCPServer = new TcpListener(localEndPoint);
            isTheserverRunning = new ServerTestAssistant(); //This will be used as a way to share information between the threads. After isTheserverRunning's serverRunning property will be set to "true", this.Disconnect will be enabled. This is done to prevent calling this.server.Start() after the server has been disposed in this.stopServer().
            this.TCPServerThread = new Thread(() => TCPServerThreadWork(isTheserverRunning));
            this.TCPServerThread.Start();
        }

        private void TCPServerThreadWork(ServerTestAssistant serverRunning)
        {
            try
            {
                TCPServer.Start();
                serverRunning.serverRunning = true;
                MessageBox.Show("Server is running...");
                bool serverStopped = false;
                try
                {
                    this.client = TCPServer.AcceptTcpClient();
                }
                catch (SocketException) { serverStopped = true; }//In case the server was closed before a connection could be made.
                if (!serverStopped)
                {
                    MessageBox.Show("Connected to client.");
                    IPAddress clientIP = ((IPEndPoint)(client.Client.RemoteEndPoint)).Address;
                    //serverRunning.IP = clientIP.ToString();
                    NetworkStream stream = client.GetStream();
                    byte[] dataToReceive = new byte[client.ReceiveBufferSize];
                    int bytesRead, statusCode;
                    if (client.Connected)
                    {
                        System.Timers.Timer timer = new System.Timers.Timer();
                        timer.Stop();
                        timer.Elapsed += new ElapsedEventHandler((object caller, ElapsedEventArgs args) => 
                        {
                            this.connectionBroken = true;
                            this.disconnectStatusCodeReceived();
                            throw new ApplicationException("Connection broken.");
                        });
                        timer.Interval = 600;
                        try
                        {
                            timer.Stop();
                            //timer.Start();
                            this.statusCodeToSend = this.STILLCONNECTEDCODE;
                            while (!this.connectionBroken)
                            {
                                dataToReceive = new byte[client.ReceiveBufferSize];
                                bytesRead = stream.Read(dataToReceive, 0, dataToReceive.Length);
                                timer.Stop();
                                string allDataRead = Encoding.UTF8.GetString(dataToReceive, 0, bytesRead);
                                foreach (string uneditedDataRead in allDataRead.Split('~'))
                                {
                                    if (uneditedDataRead == "") continue;
                                    string dataRead = uneditedDataRead.Replace(this.FRAME.ToString(), "");
                                    if (int.TryParse(dataRead, out statusCode))
                                    {
                                        if (statusCode == this.DISCONNECTCODE)
                                        {
                                            MessageBox.Show("Client had disconnected.");
                                            this.disconnectStatusCodeReceived();
                                            this.connectionBroken = true;
                                            break;
                                        }
                                        else if (statusCode != this.STILLCONNECTEDCODE)
                                        {
                                            MessageBox.Show("Connection broken (unidentified status code " + dataRead + " received from client).\nClosing server.");
                                            this.disconnectStatusCodeReceived();
                                            this.connectionBroken = true;
                                            break;
                                        }
                                        else
                                        {
                                            timer.Start();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid characters received.\nMessage was: " + dataRead + "\nBreaking the program's run.");
                                        this.disconnectStatusCodeReceived();
                                        this.connectionBroken = true;
                                        break;
                                    }
                                }
                            }
                            //client.Close();
                        }
                        catch (ApplicationException exc)
                        {
                            MessageBox.Show(exc.Message);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                if (!(exception is ThreadAbortException))
                {
                    MessageBox.Show(exception.ToString());
                }
                //else the thread was aborted, i.e, the user pressed "Disconnect".
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("I asked you not to :(");
            //this.server.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.server.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.stopServerBt.Enabled)
            {
                this.stopServer(); //Stops the VNC server's process.
                this.client.Close();
                this.TCPServer.Server.Close();
                this.TCPServer.Stop();
            }
            base.OnClosing(e);
        }
    }
    public class ServerTestAssistant
    {
        public bool serverRunning;
        public string IP;
        public ServerTestAssistant()
        {
            this.serverRunning = false;
            this.IP = string.Empty;
        }
    }
}
