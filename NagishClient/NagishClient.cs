/*
 * To-do list:
 * 
 * Add a constructor which receives the calling form as a parameter in order to enable resizing of the form when activating the VNC client. (Or at least look into it, to see if that's a viable option that makes sense.)
 * 
 * Add something which resizes the VNC client when it's activated (again, see if that's a proper viable option, and if not, then what should be done to take care of that.)
 * 
 * Write the TCP client and the messages it sends and all that.
 * 
 * More that I can't remember. There's gotta be more. There's always more.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace NagishClient
{
    class NagishClient : IDisposable
    {
        //This class will establish two unrelated connections: a simple TCP connection used to exchange information and a VNC connection.

        /* EXCEPTION CODES:
         * 404 - Server seems to have already been closed.
         * 567 - Cannot connect to the server.
        */

        /* CONNCETION CODES (i.e, the status codes sent through the TCP client)
         * 100 - Continue (as in, the client's up and running and everything's okay)
         * 404 - Stop (as in, the client's closing and sends a message to tell the server to stop its operations)
        */
        
        private VncSharp.RemoteDesktop rd { get; set; }
        public string hostIP { private set; get; }
        private TcpClient tcpClient { set; get; }
        private bool disconnected { set; get; }
        private int retries { set; get; } //If trying to connect and either the TCP client or the VNC client is already connected, will attempt to disconnect them and then reconnect for 3 times. This is of times it was already tried.
        private Thread messageThread { get; set; }
        private readonly int PAUSETIME = 4;
        private readonly string CONNECTEDCODE = "111";
        private readonly string DISCONNECTCODE = "666";
        private readonly char MESSAGEPADDING = '~'; //Will symbolize the beggining and end of a message.
        private string codeToSend { get; set; }
        private bool sentDisconnectCode { get; set; }

        public NagishClient(
            VncSharp.RemoteDesktop rdControl, string hostIP)
        {
            //rdControl is the RemoteDesktop control instance that will be used with this class.
            //hostIP is the... well, the IP of the host. :D
            this.disconnected = true;
            this.rd = rdControl;
            this.hostIP = hostIP;
            this.retries = 0;
            this.codeToSend = this.CONNECTEDCODE;
            this.rd.ConnectionLost += new EventHandler(this.ConnectionLost);
        }

        void ConnectionLost(object sender, EventArgs e)
        {
        }

        public void Connect() //Will initialize the clients and the connection. Think of this as what'd happen when you press the "Connect" button.
        {
            this.initializeClients();
            this.disconnected = false;
            if (!this.rd.IsConnected && !this.tcpClient.Connected)
            {
                this.ConnectVNC();
                this.ConnectTCP();
                this.sentDisconnectCode = false;
                this.codeToSend = this.CONNECTEDCODE;
                this.retries = 0;
            }
            else
            {
                if (retries == 3)
                {
                    throw new Exception("Error with closing and restarting the client.");
                }
                else
                {
                    if (this.tcpClient.Connected)
                    {
                        this.tcpClient.Client.Close();
                        this.tcpClient.Close();
                    }
                    if (this.rd.IsConnected)
                    {
                        this.rd.Disconnect();
                    }
                    this.retries++;
                    this.Connect();
                }
            }
        }

        private void initializeClients() //Will call the methods which initialize the TCP and VNC clients.
        {
            this.initializeVNCClient();
            this.initializeTCPClient();
            //Timing issues sometimes occur here, so I'll add a timer running for a second.
            Stopwatch timer = new Stopwatch();
            timer.Start();
            while (timer.Elapsed < TimeSpan.FromSeconds(1)) ;
            timer.Stop();
        }

        private void initializeTCPClient()
        {
            this.tcpClient = new TcpClient();
        }

        private void initializeVNCClient()
        {
            //Do stuff to this.rd - dunno which stuff yet.
        }

        private void ConnectTCP() //Connect to the remote TCP listener.
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(this.hostIP), 6161);
            try
            {
                this.tcpClient.Connect(localEndPoint);
                //MessageBox.Show("Connected to server.");
                //stream = client.GetStream();

                this.messageThread = new Thread(() => this.messageThreadWork());
                messageThread.Start();
            }
            catch (SocketException exception)
            {
                //MessageBox.Show(exception.ToString());
                //Again, leaving here in case I decide to handle this in the future; will probably be erased, I guess.
                throw exception;
            }
        }

        private void messageThreadWork()
        {
            NetworkStream stream = this.tcpClient.GetStream();
            bool breakLoop = false;
            Stopwatch timer = new Stopwatch();
            Byte[] bytesToSend = new Byte[Math.Max(Encoding.UTF8.GetBytes(this.CONNECTEDCODE).Length, Encoding.UTF8.GetBytes(this.DISCONNECTCODE).Length)];
            while (!breakLoop)
            {
                string toSend = this.codeToSend; //Moving to a local variable to make sure it doesn't change midway, and then quits without sending the disconnection code.
                bytesToSend = Encoding.UTF8.GetBytes(toSend + this.MESSAGEPADDING);
                try
                {
                    stream.Write(bytesToSend, 0, bytesToSend.Length);
                }
                catch (System.IO.IOException)
                {
                    toSend = this.DISCONNECTCODE; //To break the loop.
                    System.Windows.Forms.Form form = this.rd.FindForm();
                    Action disconnect = new Action(() => this.Disconnect());
                    form.Invoke(disconnect);
                }
                if (toSend == this.DISCONNECTCODE)
                {
                    this.sentDisconnectCode = true;
                    breakLoop = true;
                }
                else if (toSend == this.CONNECTEDCODE)
                {
                    timer.Reset();
                    timer.Start();
                    while (timer.Elapsed < TimeSpan.FromSeconds(this.PAUSETIME)) ;
                }
                else
                {
                    throw new Exception("Something weird seems to have happened to the codeToSend variable. Wth. " + toSend.ToString());
                }
                timer.Stop();
            }
            stream.Close();
        }

        private void ConnectVNC() //Connect to the remote VNC server.
        {
            try
            {
                this.rd.Connect(this.hostIP);
            }
            catch (VncSharp.VncProtocolException exc)
            {
                //I originally planned to handle this; currently, I will do nothing and this catch block is likely to be erased in the future.
                throw exc;
            }
            catch (System.IO.IOException exc)
            {
                if (exc.Message == "Unable to read next byte(s).")
                {
                    throw new Exception("404:\nThe connection seems to have already been closed.\nHost must restart the server in order to initiate a new connection.");
                }
                else
                {
                    //throw e;
                    throw new Exception("567:\nCan't connect to the server.\nIs it running?");
                }
            }
        }

        public void Disconnect() //Will disconnect the VNC and TCP clients. Think of it as the action of the Disconnect button.
        {
            if (!disconnected)
            {
                this.codeToSend = this.DISCONNECTCODE;
                if (this.rd.IsConnected)
                {
                    this.rd.Disconnect();
                }
                if (this.tcpClient.Connected)
                {
                    Action thingy = new Action(() => {
                    while (!this.sentDisconnectCode) ;
                    this.tcpClient.Client.Close();
                    this.tcpClient.Close();
                    });
                    Task task = new Task(thingy);
                    task.Start();
                }
                this.disconnected = true;
            }
        }

        public void Dispose()
        {

        }

        ~NagishClient()
        {
            this.Disconnect();
        }
    }
}
