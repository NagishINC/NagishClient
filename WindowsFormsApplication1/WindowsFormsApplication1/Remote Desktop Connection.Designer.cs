namespace RemoteDesktopTest
{
    partial class Form2
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
            this.DCbutton = new System.Windows.Forms.Button();
            this.remoteDesktop1 = new VncSharp.RemoteDesktop();
            this.CONButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DCbutton
            // 
            this.DCbutton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.DCbutton.Enabled = false;
            this.DCbutton.Location = new System.Drawing.Point(0, 580);
            this.DCbutton.Name = "DCbutton";
            this.DCbutton.Size = new System.Drawing.Size(1113, 28);
            this.DCbutton.TabIndex = 0;
            this.DCbutton.Text = "Disconnect";
            this.DCbutton.UseVisualStyleBackColor = true;
            this.DCbutton.Click += new System.EventHandler(this.DCbutton_Click);
            // 
            // remoteDesktop1
            // 
            this.remoteDesktop1.AutoScroll = true;
            this.remoteDesktop1.AutoScrollMinSize = new System.Drawing.Size(608, 427);
            this.remoteDesktop1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.remoteDesktop1.Location = new System.Drawing.Point(0, 0);
            this.remoteDesktop1.Name = "remoteDesktop1";
            this.remoteDesktop1.Size = new System.Drawing.Size(1113, 580);
            this.remoteDesktop1.TabIndex = 1;
            this.remoteDesktop1.ConnectComplete += new VncSharp.ConnectCompleteHandler(this.remoteDesktop1_ConnectComplete);
            this.remoteDesktop1.ConnectionLost += new System.EventHandler(this.remoteDesktop1_ConnectionLost);
            // 
            // CONButton
            // 
            this.CONButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CONButton.Location = new System.Drawing.Point(0, 552);
            this.CONButton.Name = "CONButton";
            this.CONButton.Size = new System.Drawing.Size(1113, 28);
            this.CONButton.TabIndex = 2;
            this.CONButton.Text = "Connect";
            this.CONButton.UseVisualStyleBackColor = true;
            this.CONButton.Click += new System.EventHandler(this.CONButton_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 608);
            this.Controls.Add(this.CONButton);
            this.Controls.Add(this.remoteDesktop1);
            this.Controls.Add(this.DCbutton);
            this.Name = "Form2";
            this.Text = "Nagish INC. Test Application";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button DCbutton;
        private VncSharp.RemoteDesktop remoteDesktop1;
        private System.Windows.Forms.Button CONButton;
    }
}