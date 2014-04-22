namespace Configuration.DkimSigner
{
    partial class RuleWindow
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
            this.lbSenderRule = new System.Windows.Forms.Label();
            this.lbRecipientRule = new System.Windows.Forms.Label();
            this.txtSenderRule = new System.Windows.Forms.TextBox();
            this.txtRecipientRule = new System.Windows.Forms.TextBox();
            this.btValidate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbSenderRule
            // 
            this.lbSenderRule.AutoSize = true;
            this.lbSenderRule.Location = new System.Drawing.Point(27, 22);
            this.lbSenderRule.Name = "lbSenderRule";
            this.lbSenderRule.Size = new System.Drawing.Size(67, 13);
            this.lbSenderRule.TabIndex = 0;
            this.lbSenderRule.Text = "Sender rule :";
            // 
            // lbRecipientRule
            // 
            this.lbRecipientRule.AutoSize = true;
            this.lbRecipientRule.Location = new System.Drawing.Point(26, 46);
            this.lbRecipientRule.Name = "lbRecipientRule";
            this.lbRecipientRule.Size = new System.Drawing.Size(78, 13);
            this.lbRecipientRule.TabIndex = 1;
            this.lbRecipientRule.Text = "Recipient rule :";
            // 
            // txtSenderRule
            // 
            this.txtSenderRule.Location = new System.Drawing.Point(107, 19);
            this.txtSenderRule.Name = "txtSenderRule";
            this.txtSenderRule.Size = new System.Drawing.Size(210, 20);
            this.txtSenderRule.TabIndex = 2;
            // 
            // txtRecipientRule
            // 
            this.txtRecipientRule.Location = new System.Drawing.Point(107, 43);
            this.txtRecipientRule.Name = "txtRecipientRule";
            this.txtRecipientRule.Size = new System.Drawing.Size(210, 20);
            this.txtRecipientRule.TabIndex = 3;
            // 
            // btValidate
            // 
            this.btValidate.Location = new System.Drawing.Point(128, 70);
            this.btValidate.Name = "btValidate";
            this.btValidate.Size = new System.Drawing.Size(79, 23);
            this.btValidate.TabIndex = 15;
            this.btValidate.Text = "Valiade";
            this.btValidate.UseVisualStyleBackColor = true;
            this.btValidate.Click += new System.EventHandler(this.btValidate_Click);
            // 
            // RuleWindows
            // 
            this.AcceptButton = this.btValidate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 105);
            this.ControlBox = false;
            this.Controls.Add(this.btValidate);
            this.Controls.Add(this.txtRecipientRule);
            this.Controls.Add(this.txtSenderRule);
            this.Controls.Add(this.lbRecipientRule);
            this.Controls.Add(this.lbSenderRule);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RuleWindows";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Domain rules";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbSenderRule;
        private System.Windows.Forms.Label lbRecipientRule;
        public System.Windows.Forms.TextBox txtSenderRule;
        public System.Windows.Forms.TextBox txtRecipientRule;
        private System.Windows.Forms.Button btValidate;
    }
}