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


namespace RemoteDesktopTest
{
    public partial class Form2 : Form
    {
        //TCP client attributes:
        TcpClient client;
        NetworkStream stream;
        Thread clientThread;
        //System.Timers.Timer messageTimer;
        //private ServerTestAssistant isTheserverRunning;

        //Attributes related to status message sending:
        private readonly int STILLCONNECTEDCODE = 100; //A readonly 32 bits integer symbolizing the status code that means the connection is still running.
        private readonly int DISCONNECTCODE = 404; //A readonly 32 bits integer symbolizing the status code that means the connection is stopping.
        private int statusCodeToSend; //The status code that will be sent to the server.
        private double timePerMessage; //The time (in seconds) that will pass before every status message is sent.
        //private Thread statusMessagesThread;

        private string IP;
        private string hostName;
        public Form2(string IP)
        {
            InitializeComponent();
            this.IP = IP;
            this.client = null;
            this.stream = null;
            this.statusCodeToSend = this.STILLCONNECTEDCODE;
            this.timePerMessage = 5;
        }

        private void TCPClientInstantiate()
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(this.IP), 6161);
            client = new TcpClient();
            //this.clientThread = new Thread(() => this.client.Connect(localEndPoint));
            try
            {
                client.Connect(localEndPoint);
                MessageBox.Show("Connected to server.");
                stream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                //int bytesRead;
                //this.timerThread = new Thread(() => this.timerThreadWork());
                //bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
                //this.setTimer();
                //this.messageTimer.Start();
                /*while (true)
                {
                    bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
                    this.messageTimer.Stop();
                    this.setTimer();
                    this.messageTimer.Start();
                }*/
                //byte[] buffer = new byte[client.ReceiveBufferSize];
                //int bytesRead = stream.Read(buffer, 0, client.ReceiveBufferSize);
                //MessageBox.Show(Encoding.ASCII.GetString(buffer, 0, bytesRead));

                //@@@STUFF:
                //byte[] dataToSend = new byte[client.SendBufferSize];
                //dataToSend = System.Text.Encoding.UTF8.GetBytes(this.statusCodeToSend.ToString());
                //stream.Write(dataToSend, 0, dataToSend.Length);
                //@@@ENDOFSTUFF
                /*statusMessagesThread = new Thread(() => this.messageLooper());
                statusMessagesThread.Start();*/
                this.messageLooper();
                

                
            }
            catch (SocketException exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        public void messageLooper() //Loops and sends a message to the server every this.timePerMessage seconds.
        {
            byte[] dataToSend = new byte[client.SendBufferSize];
            Stopwatch timer = new Stopwatch();
            while (this.statusCodeToSend != this.DISCONNECTCODE) //As long as the server isn't stopping...
            {
                timer.Reset(); //Reset timer.
                dataToSend = System.Text.Encoding.UTF8.GetBytes(this.statusCodeToSend.ToString()); //Convert the status code into bytes.
                try
                {
                    this.stream.Write(dataToSend, 0, dataToSend.Length); //Send status code.
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Connection with the client seems to be lost.");
                    break;
                }
                timer.Start(); //Start timer.
                while (timer.Elapsed < TimeSpan.FromSeconds(this.timePerMessage)) { } //Freezing until the timePerMessage passes.
                MessageBox.Show("Message sent.");
            }
            MessageBox.Show("Got here!");
            dataToSend = System.Text.Encoding.UTF8.GetBytes(this.statusCodeToSend.ToString()); //Sending the disconnection code.
        }

        public void startConnection() //Initiating connection attempt(s).
        {
            this.CONButton.Enabled = false;
            this.CONButton.Text = "Connecting...";
            try
            {
                remoteDesktop1.Connect(this.IP);
            }
            catch (VncSharp.VncProtocolException e)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                MessageBox.Show(e.Message); //Showing the error to the user.
            }
            catch (System.IO.IOException e)
            {
                if (e.Message == "Unable to read next byte(s).")
                {
                    MessageBox.Show("The connection was already closed.\nHost must restart server in order to initiate a new connection.");
                    this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                }
                else
                {
                    //throw e;
                    MessageBox.Show("Can't connect to the server.\nIs it running?");
                    this.CONButton.Enabled = true;
                    this.DCbutton.Enabled = false;
                    this.CONButton.Text = "Connect";
                }
            }
            //bool success = false;
            //int attempts = 0;
            //while (!success && attempts < 3)
            //{
            //    try
            //    {
            //        remoteDesktop.Connect(this.IP);
            //        success = true;
            //        this.CONButton.Text = "Connect";
            //        this.CONButton.Enabled = false;
            //        this.DCbutton.Enabled = true;
            //    }
            //    catch (VncSharp.VncProtocolException e)
            //    {
            //        MessageBox.Show("Connection attempt failed. Error message:\n" + e.Message + "\nRetrying (" + (3 - attempts).ToString() + " more times...)");
            //        attempts++;
            //    }
            //}
            //if (!success)
            //{
            //    this.DialogResult = System.Windows.Forms.DialogResult.Abort;
            //}
        }

        public void disconnect()
        {
            /*if (this.stream != null)
            {
                byte[] dataToSend = new byte[client.SendBufferSize];
                dataToSend = System.Text.Encoding.UTF8.GetBytes(this.DISCONNECTCODE.ToString());
                this.stream.Write(dataToSend, 0, dataToSend.Length);
                this.stream.Close();
                this.client.Close();
            }*/
            this.statusCodeToSend = this.DISCONNECTCODE;
            if (this.remoteDesktop1.IsConnected)
            {
                this.remoteDesktop1.Disconnect();
            }

        }

        public DialogResult showForm()
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.Abort)
            {
                return System.Windows.Forms.DialogResult.Abort;
            }
            return this.ShowDialog();
        }

        private void CONButton_Click(object sender, EventArgs e)
        {
            this.startConnection(); //Start the VNC connection.
            //Start the TCP connection:
            this.clientThread = new Thread(() => this.TCPClientInstantiate());
            this.clientThread.Start();
        }

        private void DCbutton_Click(object sender, EventArgs e)
        {
            this.disconnect();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.DCbutton.Enabled)
            {
                this.disconnect();
            }
            if (this.stream != null)
            {
                this.stream.Close();
            }
            if (this.client != null)
            {
                this.client.Close();
            }
            if (this.remoteDesktop1.IsConnected)
            {
                this.disconnect();
            }
            if (this.clientThread != null)
            {
                if (this.clientThread.IsAlive)
                {
                    this.clientThread.Join();
                }
            }
            /*if (this.statusMessagesThread != null)
            {
                if (this.statusMessagesThread.IsAlive)
                {
                    this.statusMessagesThread.Join();
                }
            }*/
            base.OnClosing(e);
        }

        private void remoteDesktop1_ConnectComplete(object sender, VncSharp.ConnectEventArgs e)
        {
            this.Location = new Point(0,0);
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            //this.ClientSize = new Size(e.DesktopWidth, e.DesktopHeight);
            this.remoteDesktop1.Size = new System.Drawing.Size(e.DesktopWidth, e.DesktopHeight);
            this.CONButton.Text = "Connect";
            this.CONButton.Enabled = false;
            this.DCbutton.Enabled = true;
            this.hostName = e.DesktopName;
            
        }

        private void remoteDesktop1_ConnectionLost(object sender, EventArgs e)
        {
            MessageBox.Show("Connection with " + this.hostName + " stopped.");
            this.ClientSize = new System.Drawing.Size(1123, 603);
            this.remoteDesktop1.Size = new System.Drawing.Size(1099, 545);
            this.CONButton.Enabled = true;
            this.DCbutton.Enabled = false;
            this.disconnect();
        }
    }
    /*public class ServerTestAssistant
    {
        public bool serverRunning;
        public string IP;
        public ServerTestAssistant()
        {
            this.serverRunning = false;
            this.IP = string.Empty;
        }
    }*/
}
