using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;

namespace RemoteDesktopTest
{
    public partial class NagishINC : Form
    {
        public NagishINC()
        {
            InitializeComponent();
        }

        private bool validateIP(string IP)
        {
            IPAddress IPAddr;
            return IPAddress.TryParse(IP, out IPAddr);
        }

        private void IPtb_TextChanged(object sender, EventArgs e)
        {
            if (this.validateIP(this.IPtb.Text))
            {
                this.button1.Enabled = true;
            }
            else
            {
                this.button1.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 RDConnectionForm = new Form2(this.IPtb.Text);
            DialogResult res = RDConnectionForm.showForm();
            if (res == System.Windows.Forms.DialogResult.Abort)
            {
                this.Statustb.Text = "";
                MessageBox.Show("Connection failed.");
            }
            else
            {
                this.Statustb.Text = "Connection closed.";
            }
        }
    }
}
