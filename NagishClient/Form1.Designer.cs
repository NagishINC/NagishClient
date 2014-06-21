namespace NagishClient
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConnectBT = new System.Windows.Forms.Button();
            this.DisconnectBT = new System.Windows.Forms.Button();
            this.remoteDesktop1 = new VncSharp.RemoteDesktop();
            this.SuspendLayout();
            // 
            // ConnectBT
            // 
            this.ConnectBT.Location = new System.Drawing.Point(179, 593);
            this.ConnectBT.Name = "ConnectBT";
            this.ConnectBT.Size = new System.Drawing.Size(75, 23);
            this.ConnectBT.TabIndex = 1;
            this.ConnectBT.Text = "Connect";
            this.ConnectBT.UseVisualStyleBackColor = true;
            this.ConnectBT.Click += new System.EventHandler(this.ConnectBT_Click);
            // 
            // DisconnectBT
            // 
            this.DisconnectBT.Enabled = false;
            this.DisconnectBT.Location = new System.Drawing.Point(543, 593);
            this.DisconnectBT.Name = "DisconnectBT";
            this.DisconnectBT.Size = new System.Drawing.Size(75, 23);
            this.DisconnectBT.TabIndex = 2;
            this.DisconnectBT.Text = "Disconnect";
            this.DisconnectBT.UseVisualStyleBackColor = true;
            this.DisconnectBT.Click += new System.EventHandler(this.DisconnectBT_Click);
            // 
            // remoteDesktop1
            // 
            this.remoteDesktop1.AutoScroll = true;
            this.remoteDesktop1.AutoScrollMinSize = new System.Drawing.Size(608, 427);
            this.remoteDesktop1.Location = new System.Drawing.Point(14, 12);
            this.remoteDesktop1.Name = "remoteDesktop1";
            this.remoteDesktop1.Size = new System.Drawing.Size(786, 565);
            this.remoteDesktop1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 628);
            this.Controls.Add(this.DisconnectBT);
            this.Controls.Add(this.ConnectBT);
            this.Controls.Add(this.remoteDesktop1);
            this.Name = "Form1";
            this.Text = "NagishClient Class Test";
            this.ResumeLayout(false);

        }

        #endregion

        private VncSharp.RemoteDesktop remoteDesktop1;
        private System.Windows.Forms.Button ConnectBT;
        private System.Windows.Forms.Button DisconnectBT;
    }
}

