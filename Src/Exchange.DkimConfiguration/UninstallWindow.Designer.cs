namespace Exchange.DkimConfiguration
{
    partial class UninstallWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UninstallWindow));
            this.btClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressUninstall = new System.Windows.Forms.ProgressBar();
            this.lbStep = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.Enabled = false;
            this.btClose.Location = new System.Drawing.Point(437, 61);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 4;
            this.btClose.Text = "&Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Uninstalling DKIM Signer for Microsoft Exchange:";
            // 
            // progressUninstall
            // 
            this.progressUninstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressUninstall.Location = new System.Drawing.Point(15, 25);
            this.progressUninstall.Name = "progressUninstall";
            this.progressUninstall.Size = new System.Drawing.Size(497, 23);
            this.progressUninstall.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressUninstall.TabIndex = 6;
            this.progressUninstall.UseWaitCursor = true;
            // 
            // lbStep
            // 
            this.lbStep.AutoSize = true;
            this.lbStep.Location = new System.Drawing.Point(12, 51);
            this.lbStep.Name = "lbStep";
            this.lbStep.Size = new System.Drawing.Size(73, 13);
            this.lbStep.TabIndex = 7;
            this.lbStep.Text = "Please wait ...";
            // 
            // UninstallWindow
            // 
            this.AcceptButton = this.btClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 96);
            this.Controls.Add(this.lbStep);
            this.Controls.Add(this.progressUninstall);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UninstallWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DKIM Signer for Exchange - Uninstall";
            this.Shown += new System.EventHandler(this.UninstallWindow_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressUninstall;
        private System.Windows.Forms.Label lbStep;

    }
}