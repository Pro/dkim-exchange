namespace Configuration.DkimSigner
{
    partial class ConfigureWindow
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
            this.btClose = new System.Windows.Forms.Button();
            this.dgvExchangeVersion = new System.Windows.Forms.DataGridView();
            this.dgvcExchangeVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcExchangeDirectory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvExchangeVersion)).BeginInit();
            this.SuspendLayout();
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.Location = new System.Drawing.Point(353, 226);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 1;
            this.btClose.Text = "&Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // dgvExchangeVersion
            // 
            this.dgvExchangeVersion.AllowUserToAddRows = false;
            this.dgvExchangeVersion.AllowUserToDeleteRows = false;
            this.dgvExchangeVersion.AllowUserToResizeColumns = false;
            this.dgvExchangeVersion.AllowUserToResizeRows = false;
            this.dgvExchangeVersion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvExchangeVersion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcExchangeVersion,
            this.dgvcExchangeDirectory});
            this.dgvExchangeVersion.Location = new System.Drawing.Point(12, 12);
            this.dgvExchangeVersion.MultiSelect = false;
            this.dgvExchangeVersion.Name = "dgvExchangeVersion";
            this.dgvExchangeVersion.ReadOnly = true;
            this.dgvExchangeVersion.RowHeadersVisible = false;
            this.dgvExchangeVersion.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvExchangeVersion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvExchangeVersion.ShowCellErrors = false;
            this.dgvExchangeVersion.ShowCellToolTips = false;
            this.dgvExchangeVersion.ShowEditingIcon = false;
            this.dgvExchangeVersion.ShowRowErrors = false;
            this.dgvExchangeVersion.Size = new System.Drawing.Size(416, 208);
            this.dgvExchangeVersion.TabIndex = 0;
            // 
            // dgvcExchangeVersion
            // 
            this.dgvcExchangeVersion.HeaderText = "Version";
            this.dgvcExchangeVersion.Name = "dgvcExchangeVersion";
            this.dgvcExchangeVersion.ReadOnly = true;
            // 
            // dgvcExchangeDirectory
            // 
            this.dgvcExchangeDirectory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcExchangeDirectory.HeaderText = "Directory";
            this.dgvcExchangeDirectory.Name = "dgvcExchangeDirectory";
            this.dgvcExchangeDirectory.ReadOnly = true;
            // 
            // ConfigureWindow
            // 
            this.AcceptButton = this.btClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 261);
            this.ControlBox = false;
            this.Controls.Add(this.dgvExchangeVersion);
            this.Controls.Add(this.btClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigureWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exchange DkimSigner";
            this.Load += new System.EventHandler(this.ConfigureWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvExchangeVersion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.DataGridView dgvExchangeVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcExchangeVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcExchangeDirectory;
    }
}