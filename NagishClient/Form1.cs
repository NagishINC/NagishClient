using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NagishClient
{
    public partial class Form1 : Form
    {
        private NagishClient nc;
        public Form1()
        {
            InitializeComponent();
            nc = new NagishClient(this.remoteDesktop1, "127.0.0.1");
            this.remoteDesktop1.ConnectionLost += new EventHandler(remoteDesktop1_ConnectionLost);
        }

        void remoteDesktop1_ConnectionLost(object sender, EventArgs e)
        {
            this.ConnectBT.Enabled = !this.ConnectBT.Enabled;
            this.DisconnectBT.Enabled = !this.DisconnectBT.Enabled;
            this.nc.Disconnect();
        }

        private void ConnectBT_Click(object sender, EventArgs e)
        {
            this.nc.Connect();
            this.ConnectBT.Enabled = !this.ConnectBT.Enabled;
            this.DisconnectBT.Enabled = !this.DisconnectBT.Enabled;
        }

        private void DisconnectBT_Click(object sender, EventArgs e)
        {
            this.nc.Disconnect();
            this.ConnectBT.Enabled = !this.ConnectBT.Enabled;
            this.DisconnectBT.Enabled = !this.DisconnectBT.Enabled;
        }

    }
}
