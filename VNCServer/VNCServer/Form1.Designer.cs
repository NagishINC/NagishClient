namespace VNCServer
{
    partial class StartingWindow
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
            this.startServerBt = new System.Windows.Forms.Button();
            this.stopServerBt = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startServerBt
            // 
            this.startServerBt.Location = new System.Drawing.Point(106, 79);
            this.startServerBt.Name = "startServerBt";
            this.startServerBt.Size = new System.Drawing.Size(75, 23);
            this.startServerBt.TabIndex = 0;
            this.startServerBt.Text = "Start Server";
            this.startServerBt.UseVisualStyleBackColor = true;
            this.startServerBt.Click += new System.EventHandler(this.startServerBt_Click);
            // 
            // stopServerBt
            // 
            this.stopServerBt.Enabled = false;
            this.stopServerBt.Location = new System.Drawing.Point(105, 120);
            this.stopServerBt.Name = "stopServerBt";
            this.stopServerBt.Size = new System.Drawing.Size(75, 23);
            this.stopServerBt.TabIndex = 1;
            this.stopServerBt.Text = "Stop Server";
            this.stopServerBt.UseVisualStyleBackColor = true;
            this.stopServerBt.Click += new System.EventHandler(this.stopServerBt_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(105, 160);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 44);
            this.button1.TabIndex = 2;
            this.button1.Text = "Don\'t press it :(";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // StartingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.stopServerBt);
            this.Controls.Add(this.startServerBt);
            this.Name = "StartingWindow";
            this.Text = "Nagish INC. VNC Server";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startServerBt;
        private System.Windows.Forms.Button stopServerBt;
        private System.Windows.Forms.Button button1;
    }
}

