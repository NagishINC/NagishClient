namespace RemoteDesktopTest
{
    partial class NagishINC
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
            this.button1 = new System.Windows.Forms.Button();
            this.IPtb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Statustb = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(92, 138);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 39);
            this.button1.TabIndex = 0;
            this.button1.Text = "Launch Remote Dekstop";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // IPtb
            // 
            this.IPtb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPtb.ForeColor = System.Drawing.SystemColors.InfoText;
            this.IPtb.Location = new System.Drawing.Point(79, 72);
            this.IPtb.Name = "IPtb";
            this.IPtb.Size = new System.Drawing.Size(125, 20);
            this.IPtb.TabIndex = 1;
            this.IPtb.TextChanged += new System.EventHandler(this.IPtb_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Host\'s IP address:";
            // 
            // Statustb
            // 
            this.Statustb.AutoSize = true;
            this.Statustb.Location = new System.Drawing.Point(114, 213);
            this.Statustb.Name = "Statustb";
            this.Statustb.Size = new System.Drawing.Size(0, 13);
            this.Statustb.TabIndex = 3;
            // 
            // NagishINC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.Statustb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IPtb);
            this.Controls.Add(this.button1);
            this.Name = "NagishINC";
            this.Text = "Nagish Stuff";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox IPtb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Statustb;
    }
}

