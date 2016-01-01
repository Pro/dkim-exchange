namespace Configuration.DkimSigner
{
    partial class HeaderInputWindow
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
            this.lbHeader = new System.Windows.Forms.Label();
            this.btAdd = new System.Windows.Forms.Button();
            this.cbxHeader = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lbHeader
            // 
            this.lbHeader.AutoSize = true;
            this.lbHeader.Location = new System.Drawing.Point(12, 9);
            this.lbHeader.Name = "lbHeader";
            this.lbHeader.Size = new System.Drawing.Size(74, 13);
            this.lbHeader.TabIndex = 0;
            this.lbHeader.Text = "Header name:";
            // 
            // btAdd
            // 
            this.btAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btAdd.Location = new System.Drawing.Point(197, 51);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(75, 23);
            this.btAdd.TabIndex = 2;
            this.btAdd.Text = "&Add";
            this.btAdd.UseVisualStyleBackColor = true;
            // 
            // cbxHeader
            // 
            this.cbxHeader.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbxHeader.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cbxHeader.FormattingEnabled = true;
            this.cbxHeader.Location = new System.Drawing.Point(15, 25);
            this.cbxHeader.Name = "cbxHeader";
            this.cbxHeader.Size = new System.Drawing.Size(257, 21);
            this.cbxHeader.TabIndex = 3;
            // 
            // HeaderInputWindow
            // 
            this.AcceptButton = this.btAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 87);
            this.Controls.Add(this.cbxHeader);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.lbHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HeaderInputWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add a new header";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HeaderInputForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbHeader;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.ComboBox cbxHeader;
    }
}