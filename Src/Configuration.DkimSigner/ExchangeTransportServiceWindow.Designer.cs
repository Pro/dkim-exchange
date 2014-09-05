namespace Configuration.DkimSigner
{
    partial class ExchangeTransportServiceWindow
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
            this.btRefresh = new System.Windows.Forms.Button();
            this.btUninstall = new System.Windows.Forms.Button();
            this.btDisable = new System.Windows.Forms.Button();
            this.btMoveUp = new System.Windows.Forms.Button();
            this.btMoveDown = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.dgvTransportServiceAgents = new System.Windows.Forms.DataGridView();
            this.dgvcPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lbHint = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransportServiceAgents)).BeginInit();
            this.SuspendLayout();
            // 
            // btRefresh
            // 
            this.btRefresh.Location = new System.Drawing.Point(403, 12);
            this.btRefresh.Name = "btRefresh";
            this.btRefresh.Size = new System.Drawing.Size(94, 23);
            this.btRefresh.TabIndex = 1;
            this.btRefresh.Text = "&Refresh";
            this.btRefresh.UseVisualStyleBackColor = true;
            this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
            // 
            // btUninstall
            // 
            this.btUninstall.Location = new System.Drawing.Point(403, 69);
            this.btUninstall.Name = "btUninstall";
            this.btUninstall.Size = new System.Drawing.Size(94, 23);
            this.btUninstall.TabIndex = 3;
            this.btUninstall.Text = "U&ninstall";
            this.btUninstall.UseVisualStyleBackColor = true;
            this.btUninstall.Click += new System.EventHandler(this.btUninstall_Click);
            // 
            // btDisable
            // 
            this.btDisable.Location = new System.Drawing.Point(403, 98);
            this.btDisable.Name = "btDisable";
            this.btDisable.Size = new System.Drawing.Size(94, 23);
            this.btDisable.TabIndex = 4;
            this.btDisable.Text = "Disa&ble";
            this.btDisable.UseVisualStyleBackColor = true;
            this.btDisable.Click += new System.EventHandler(this.btDisable_Click);
            // 
            // btMoveUp
            // 
            this.btMoveUp.Enabled = false;
            this.btMoveUp.Location = new System.Drawing.Point(403, 186);
            this.btMoveUp.Name = "btMoveUp";
            this.btMoveUp.Size = new System.Drawing.Size(94, 23);
            this.btMoveUp.TabIndex = 5;
            this.btMoveUp.Text = "Move &Up";
            this.btMoveUp.UseVisualStyleBackColor = true;
            this.btMoveUp.Click += new System.EventHandler(this.btMoveUp_Click);
            // 
            // btMoveDown
            // 
            this.btMoveDown.Enabled = false;
            this.btMoveDown.Location = new System.Drawing.Point(403, 215);
            this.btMoveDown.Name = "btMoveDown";
            this.btMoveDown.Size = new System.Drawing.Size(94, 23);
            this.btMoveDown.TabIndex = 6;
            this.btMoveDown.Text = "Move  &Down";
            this.btMoveDown.UseVisualStyleBackColor = true;
            this.btMoveDown.Click += new System.EventHandler(this.btMoveDown_Click);
            // 
            // btClose
            // 
            this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btClose.Location = new System.Drawing.Point(403, 269);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(94, 23);
            this.btClose.TabIndex = 7;
            this.btClose.Text = "&Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // dgvTransportServiceAgents
            // 
            this.dgvTransportServiceAgents.AllowUserToAddRows = false;
            this.dgvTransportServiceAgents.AllowUserToDeleteRows = false;
            this.dgvTransportServiceAgents.AllowUserToResizeColumns = false;
            this.dgvTransportServiceAgents.AllowUserToResizeRows = false;
            this.dgvTransportServiceAgents.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvTransportServiceAgents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransportServiceAgents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcPriority,
            this.dgvcName,
            this.dgvcEnabled});
            this.dgvTransportServiceAgents.Location = new System.Drawing.Point(13, 12);
            this.dgvTransportServiceAgents.Name = "dgvTransportServiceAgents";
            this.dgvTransportServiceAgents.ReadOnly = true;
            this.dgvTransportServiceAgents.RowHeadersVisible = false;
            this.dgvTransportServiceAgents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTransportServiceAgents.Size = new System.Drawing.Size(372, 259);
            this.dgvTransportServiceAgents.TabIndex = 0;
            this.dgvTransportServiceAgents.SelectionChanged += new System.EventHandler(this.dgvTransportServiceAgents_SelectionChanged);
            // 
            // dgvcPriority
            // 
            this.dgvcPriority.HeaderText = "Priority";
            this.dgvcPriority.Name = "dgvcPriority";
            this.dgvcPriority.ReadOnly = true;
            this.dgvcPriority.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvcPriority.Width = 50;
            // 
            // dgvcName
            // 
            this.dgvcName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcName.HeaderText = "Name";
            this.dgvcName.Name = "dgvcName";
            this.dgvcName.ReadOnly = true;
            this.dgvcName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dgvcEnabled
            // 
            this.dgvcEnabled.HeaderText = "Enabled";
            this.dgvcEnabled.Name = "dgvcEnabled";
            this.dgvcEnabled.ReadOnly = true;
            this.dgvcEnabled.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvcEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgvcEnabled.Width = 60;
            // 
            // lbHint
            // 
            this.lbHint.AutoSize = true;
            this.lbHint.Location = new System.Drawing.Point(12, 274);
            this.lbHint.Name = "lbHint";
            this.lbHint.Size = new System.Drawing.Size(292, 13);
            this.lbHint.TabIndex = 9;
            this.lbHint.Text = "Hint: The DKIM signing agent should have the lowest priority";
            // 
            // ExchangeTransportServiceWindow
            // 
            this.AcceptButton = this.btClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btClose;
            this.ClientSize = new System.Drawing.Size(509, 304);
            this.Controls.Add(this.lbHint);
            this.Controls.Add(this.dgvTransportServiceAgents);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btMoveDown);
            this.Controls.Add(this.btMoveUp);
            this.Controls.Add(this.btDisable);
            this.Controls.Add(this.btUninstall);
            this.Controls.Add(this.btRefresh);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExchangeTransportServiceWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exchange Transport Agents";
            this.Load += new System.EventHandler(this.ExchangeTransportServiceWindows_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransportServiceAgents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btRefresh;
        private System.Windows.Forms.Button btUninstall;
        private System.Windows.Forms.Button btDisable;
        private System.Windows.Forms.Button btMoveUp;
        private System.Windows.Forms.Button btMoveDown;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.DataGridView dgvTransportServiceAgents;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvcEnabled;
        private System.Windows.Forms.Label lbHint;

    }
}