namespace Configuration.DkimSigner
{
    partial class PrivateKeyWindows
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
            this.btValidate = new System.Windows.Forms.Button();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.lbDnsSelectorRecord = new System.Windows.Forms.Label();
            this.lbFilename = new System.Windows.Forms.Label();
            this.txtDnsSelectorRecord = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btValidate
            // 
            this.btValidate.Location = new System.Drawing.Point(204, 126);
            this.btValidate.Name = "btValidate";
            this.btValidate.Size = new System.Drawing.Size(79, 23);
            this.btValidate.TabIndex = 16;
            this.btValidate.Text = "Valiade";
            this.btValidate.UseVisualStyleBackColor = true;
            this.btValidate.Click += new System.EventHandler(this.btValidate_Click);
            // 
            // txtFilename
            // 
            this.txtFilename.Location = new System.Drawing.Point(85, 11);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(382, 20);
            this.txtFilename.TabIndex = 19;
            // 
            // lbDnsSelectorRecord
            // 
            this.lbDnsSelectorRecord.AutoSize = true;
            this.lbDnsSelectorRecord.Location = new System.Drawing.Point(12, 44);
            this.lbDnsSelectorRecord.Name = "lbDnsSelectorRecord";
            this.lbDnsSelectorRecord.Size = new System.Drawing.Size(109, 13);
            this.lbDnsSelectorRecord.TabIndex = 18;
            this.lbDnsSelectorRecord.Text = "DNS selector record :";
            // 
            // lbFilename
            // 
            this.lbFilename.AutoSize = true;
            this.lbFilename.Location = new System.Drawing.Point(12, 14);
            this.lbFilename.Name = "lbFilename";
            this.lbFilename.Size = new System.Drawing.Size(55, 13);
            this.lbFilename.TabIndex = 17;
            this.lbFilename.Text = "Filename :";
            // 
            // txtDnsSelectorRecord
            // 
            this.txtDnsSelectorRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDnsSelectorRecord.Location = new System.Drawing.Point(15, 60);
            this.txtDnsSelectorRecord.Multiline = true;
            this.txtDnsSelectorRecord.Name = "txtDnsSelectorRecord";
            this.txtDnsSelectorRecord.ReadOnly = true;
            this.txtDnsSelectorRecord.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDnsSelectorRecord.Size = new System.Drawing.Size(452, 60);
            this.txtDnsSelectorRecord.TabIndex = 20;
            // 
            // PrivateKeyWindows
            // 
            this.AcceptButton = this.btValidate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 161);
            this.ControlBox = false;
            this.Controls.Add(this.txtDnsSelectorRecord);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.lbDnsSelectorRecord);
            this.Controls.Add(this.lbFilename);
            this.Controls.Add(this.btValidate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrivateKeyWindows";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RSA Key";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btValidate;
        public System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Label lbDnsSelectorRecord;
        private System.Windows.Forms.Label lbFilename;
        private System.Windows.Forms.TextBox txtDnsSelectorRecord;
    }
}