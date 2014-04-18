namespace Configuration.DkimSigner
{
    partial class MainWindows
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
            this.tcConfiguration = new System.Windows.Forms.TabControl();
            this.tbInformation = new System.Windows.Forms.TabPage();
            this.gbAvailable = new System.Windows.Forms.GroupBox();
            this.txtDkimSignerAvailable = new System.Windows.Forms.TextBox();
            this.lbDkimSignerAvailable = new System.Windows.Forms.Label();
            this.gbInstalled = new System.Windows.Forms.GroupBox();
            this.lbExchangeInstalled = new System.Windows.Forms.Label();
            this.txtDkimSignerInstalled = new System.Windows.Forms.TextBox();
            this.txtExchangeInstalled = new System.Windows.Forms.TextBox();
            this.lbDkimSignerInstalled = new System.Windows.Forms.Label();
            this.tpDKIM = new System.Windows.Forms.TabPage();
            this.btSave = new System.Windows.Forms.Button();
            this.gbHeaderToSign = new System.Windows.Forms.GroupBox();
            this.txtHeaderToSign = new System.Windows.Forms.TextBox();
            this.gbLogLevel = new System.Windows.Forms.GroupBox();
            this.cbLogLevel = new System.Windows.Forms.ComboBox();
            this.gbBodyCanonicalization = new System.Windows.Forms.GroupBox();
            this.rbRelaxedBodyCanonicalization = new System.Windows.Forms.RadioButton();
            this.rbSimpleBodyCanonicalization = new System.Windows.Forms.RadioButton();
            this.gbHeaderCanonicalization = new System.Windows.Forms.GroupBox();
            this.rbRelaxedHeaderCanonicalization = new System.Windows.Forms.RadioButton();
            this.rbSimpleHeaderCanonicalization = new System.Windows.Forms.RadioButton();
            this.gbAlgorithm = new System.Windows.Forms.GroupBox();
            this.rbRsaSha256 = new System.Windows.Forms.RadioButton();
            this.rbRsaSha1 = new System.Windows.Forms.RadioButton();
            this.tpDomain = new System.Windows.Forms.TabPage();
            this.btDownload = new System.Windows.Forms.Button();
            this.btUpload = new System.Windows.Forms.Button();
            this.dgvDomainConfiguration = new System.Windows.Forms.DataGridView();
            this.dgvcDomain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcSelector = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcPrivateKeyFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcSenderRule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcRecipientRule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tcConfiguration.SuspendLayout();
            this.tbInformation.SuspendLayout();
            this.gbAvailable.SuspendLayout();
            this.gbInstalled.SuspendLayout();
            this.tpDKIM.SuspendLayout();
            this.gbHeaderToSign.SuspendLayout();
            this.gbLogLevel.SuspendLayout();
            this.gbBodyCanonicalization.SuspendLayout();
            this.gbHeaderCanonicalization.SuspendLayout();
            this.gbAlgorithm.SuspendLayout();
            this.tpDomain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDomainConfiguration)).BeginInit();
            this.SuspendLayout();
            // 
            // tcConfiguration
            // 
            this.tcConfiguration.Controls.Add(this.tbInformation);
            this.tcConfiguration.Controls.Add(this.tpDKIM);
            this.tcConfiguration.Controls.Add(this.tpDomain);
            this.tcConfiguration.ItemSize = new System.Drawing.Size(150, 25);
            this.tcConfiguration.Location = new System.Drawing.Point(12, 12);
            this.tcConfiguration.Name = "tcConfiguration";
            this.tcConfiguration.SelectedIndex = 0;
            this.tcConfiguration.Size = new System.Drawing.Size(560, 357);
            this.tcConfiguration.TabIndex = 0;
            // 
            // tbInformation
            // 
            this.tbInformation.Controls.Add(this.gbAvailable);
            this.tbInformation.Controls.Add(this.gbInstalled);
            this.tbInformation.Location = new System.Drawing.Point(4, 29);
            this.tbInformation.Name = "tbInformation";
            this.tbInformation.Size = new System.Drawing.Size(552, 324);
            this.tbInformation.TabIndex = 2;
            this.tbInformation.Text = "Information";
            this.tbInformation.UseVisualStyleBackColor = true;
            // 
            // gbAvailable
            // 
            this.gbAvailable.Controls.Add(this.txtDkimSignerAvailable);
            this.gbAvailable.Controls.Add(this.lbDkimSignerAvailable);
            this.gbAvailable.Location = new System.Drawing.Point(131, 162);
            this.gbAvailable.Name = "gbAvailable";
            this.gbAvailable.Size = new System.Drawing.Size(275, 54);
            this.gbAvailable.TabIndex = 5;
            this.gbAvailable.TabStop = false;
            this.gbAvailable.Text = "Available";
            // 
            // txtDkimSignerAvailable
            // 
            this.txtDkimSignerAvailable.Enabled = false;
            this.txtDkimSignerAvailable.Location = new System.Drawing.Point(82, 21);
            this.txtDkimSignerAvailable.Name = "txtDkimSignerAvailable";
            this.txtDkimSignerAvailable.Size = new System.Drawing.Size(168, 20);
            this.txtDkimSignerAvailable.TabIndex = 5;
            // 
            // lbDkimSignerAvailable
            // 
            this.lbDkimSignerAvailable.AutoSize = true;
            this.lbDkimSignerAvailable.Location = new System.Drawing.Point(6, 24);
            this.lbDkimSignerAvailable.Name = "lbDkimSignerAvailable";
            this.lbDkimSignerAvailable.Size = new System.Drawing.Size(70, 13);
            this.lbDkimSignerAvailable.TabIndex = 4;
            this.lbDkimSignerAvailable.Text = "Dkim Signer :";
            // 
            // gbInstalled
            // 
            this.gbInstalled.Controls.Add(this.lbExchangeInstalled);
            this.gbInstalled.Controls.Add(this.txtDkimSignerInstalled);
            this.gbInstalled.Controls.Add(this.txtExchangeInstalled);
            this.gbInstalled.Controls.Add(this.lbDkimSignerInstalled);
            this.gbInstalled.Location = new System.Drawing.Point(131, 34);
            this.gbInstalled.Name = "gbInstalled";
            this.gbInstalled.Size = new System.Drawing.Size(275, 100);
            this.gbInstalled.TabIndex = 4;
            this.gbInstalled.TabStop = false;
            this.gbInstalled.Text = "Installed";
            // 
            // lbExchangeInstalled
            // 
            this.lbExchangeInstalled.AutoSize = true;
            this.lbExchangeInstalled.Location = new System.Drawing.Point(6, 29);
            this.lbExchangeInstalled.Name = "lbExchangeInstalled";
            this.lbExchangeInstalled.Size = new System.Drawing.Size(61, 13);
            this.lbExchangeInstalled.TabIndex = 0;
            this.lbExchangeInstalled.Text = "Exchange :";
            // 
            // txtDkimSignerInstalled
            // 
            this.txtDkimSignerInstalled.Enabled = false;
            this.txtDkimSignerInstalled.Location = new System.Drawing.Point(82, 61);
            this.txtDkimSignerInstalled.Name = "txtDkimSignerInstalled";
            this.txtDkimSignerInstalled.Size = new System.Drawing.Size(168, 20);
            this.txtDkimSignerInstalled.TabIndex = 3;
            // 
            // txtExchangeInstalled
            // 
            this.txtExchangeInstalled.Enabled = false;
            this.txtExchangeInstalled.Location = new System.Drawing.Point(82, 26);
            this.txtExchangeInstalled.Name = "txtExchangeInstalled";
            this.txtExchangeInstalled.Size = new System.Drawing.Size(168, 20);
            this.txtExchangeInstalled.TabIndex = 1;
            // 
            // lbDkimSignerInstalled
            // 
            this.lbDkimSignerInstalled.AutoSize = true;
            this.lbDkimSignerInstalled.Location = new System.Drawing.Point(6, 64);
            this.lbDkimSignerInstalled.Name = "lbDkimSignerInstalled";
            this.lbDkimSignerInstalled.Size = new System.Drawing.Size(70, 13);
            this.lbDkimSignerInstalled.TabIndex = 2;
            this.lbDkimSignerInstalled.Text = "Dkim Signer :";
            // 
            // tpDKIM
            // 
            this.tpDKIM.Controls.Add(this.btSave);
            this.tpDKIM.Controls.Add(this.gbHeaderToSign);
            this.tpDKIM.Controls.Add(this.gbLogLevel);
            this.tpDKIM.Controls.Add(this.gbBodyCanonicalization);
            this.tpDKIM.Controls.Add(this.gbHeaderCanonicalization);
            this.tpDKIM.Controls.Add(this.gbAlgorithm);
            this.tpDKIM.Location = new System.Drawing.Point(4, 29);
            this.tpDKIM.Name = "tpDKIM";
            this.tpDKIM.Padding = new System.Windows.Forms.Padding(3);
            this.tpDKIM.Size = new System.Drawing.Size(552, 324);
            this.tpDKIM.TabIndex = 0;
            this.tpDKIM.Text = "DKIM";
            this.tpDKIM.UseVisualStyleBackColor = true;
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(211, 262);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(125, 23);
            this.btSave.TabIndex = 14;
            this.btSave.Text = "Save configuration";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // gbHeaderToSign
            // 
            this.gbHeaderToSign.Controls.Add(this.txtHeaderToSign);
            this.gbHeaderToSign.Location = new System.Drawing.Point(62, 42);
            this.gbHeaderToSign.Name = "gbHeaderToSign";
            this.gbHeaderToSign.Size = new System.Drawing.Size(426, 52);
            this.gbHeaderToSign.TabIndex = 6;
            this.gbHeaderToSign.TabStop = false;
            this.gbHeaderToSign.Text = "Header to sign";
            // 
            // txtHeaderToSign
            // 
            this.txtHeaderToSign.Location = new System.Drawing.Point(6, 22);
            this.txtHeaderToSign.Name = "txtHeaderToSign";
            this.txtHeaderToSign.Size = new System.Drawing.Size(414, 20);
            this.txtHeaderToSign.TabIndex = 0;
            this.txtHeaderToSign.Text = "From; Subject; To; Date; Message-ID;";
            // 
            // gbLogLevel
            // 
            this.gbLogLevel.Controls.Add(this.cbLogLevel);
            this.gbLogLevel.Location = new System.Drawing.Point(62, 170);
            this.gbLogLevel.Name = "gbLogLevel";
            this.gbLogLevel.Size = new System.Drawing.Size(203, 52);
            this.gbLogLevel.TabIndex = 4;
            this.gbLogLevel.TabStop = false;
            this.gbLogLevel.Text = "Log Level";
            // 
            // cbLogLevel
            // 
            this.cbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLogLevel.FormattingEnabled = true;
            this.cbLogLevel.Items.AddRange(new object[] {
            "Error",
            "Warning",
            "Information"});
            this.cbLogLevel.Location = new System.Drawing.Point(6, 22);
            this.cbLogLevel.Name = "cbLogLevel";
            this.cbLogLevel.Size = new System.Drawing.Size(191, 21);
            this.cbLogLevel.TabIndex = 0;
            // 
            // gbBodyCanonicalization
            // 
            this.gbBodyCanonicalization.Controls.Add(this.rbRelaxedBodyCanonicalization);
            this.gbBodyCanonicalization.Controls.Add(this.rbSimpleBodyCanonicalization);
            this.gbBodyCanonicalization.Location = new System.Drawing.Point(285, 171);
            this.gbBodyCanonicalization.Name = "gbBodyCanonicalization";
            this.gbBodyCanonicalization.Size = new System.Drawing.Size(203, 52);
            this.gbBodyCanonicalization.TabIndex = 2;
            this.gbBodyCanonicalization.TabStop = false;
            this.gbBodyCanonicalization.Text = "Body Canonicalization";
            // 
            // rbRelaxedBodyCanonicalization
            // 
            this.rbRelaxedBodyCanonicalization.AutoSize = true;
            this.rbRelaxedBodyCanonicalization.Location = new System.Drawing.Point(105, 20);
            this.rbRelaxedBodyCanonicalization.Name = "rbRelaxedBodyCanonicalization";
            this.rbRelaxedBodyCanonicalization.Size = new System.Drawing.Size(64, 17);
            this.rbRelaxedBodyCanonicalization.TabIndex = 1;
            this.rbRelaxedBodyCanonicalization.Text = "Relaxed";
            this.rbRelaxedBodyCanonicalization.UseVisualStyleBackColor = true;
            // 
            // rbSimpleBodyCanonicalization
            // 
            this.rbSimpleBodyCanonicalization.AutoSize = true;
            this.rbSimpleBodyCanonicalization.Checked = true;
            this.rbSimpleBodyCanonicalization.Location = new System.Drawing.Point(21, 20);
            this.rbSimpleBodyCanonicalization.Name = "rbSimpleBodyCanonicalization";
            this.rbSimpleBodyCanonicalization.Size = new System.Drawing.Size(56, 17);
            this.rbSimpleBodyCanonicalization.TabIndex = 0;
            this.rbSimpleBodyCanonicalization.TabStop = true;
            this.rbSimpleBodyCanonicalization.Text = "Simple";
            this.rbSimpleBodyCanonicalization.UseVisualStyleBackColor = true;
            // 
            // gbHeaderCanonicalization
            // 
            this.gbHeaderCanonicalization.Controls.Add(this.rbRelaxedHeaderCanonicalization);
            this.gbHeaderCanonicalization.Controls.Add(this.rbSimpleHeaderCanonicalization);
            this.gbHeaderCanonicalization.Location = new System.Drawing.Point(284, 103);
            this.gbHeaderCanonicalization.Name = "gbHeaderCanonicalization";
            this.gbHeaderCanonicalization.Size = new System.Drawing.Size(203, 52);
            this.gbHeaderCanonicalization.TabIndex = 1;
            this.gbHeaderCanonicalization.TabStop = false;
            this.gbHeaderCanonicalization.Text = "Header Canonicalization";
            // 
            // rbRelaxedHeaderCanonicalization
            // 
            this.rbRelaxedHeaderCanonicalization.AutoSize = true;
            this.rbRelaxedHeaderCanonicalization.Location = new System.Drawing.Point(105, 20);
            this.rbRelaxedHeaderCanonicalization.Name = "rbRelaxedHeaderCanonicalization";
            this.rbRelaxedHeaderCanonicalization.Size = new System.Drawing.Size(64, 17);
            this.rbRelaxedHeaderCanonicalization.TabIndex = 1;
            this.rbRelaxedHeaderCanonicalization.Text = "Relaxed";
            this.rbRelaxedHeaderCanonicalization.UseVisualStyleBackColor = true;
            // 
            // rbSimpleHeaderCanonicalization
            // 
            this.rbSimpleHeaderCanonicalization.AutoSize = true;
            this.rbSimpleHeaderCanonicalization.Checked = true;
            this.rbSimpleHeaderCanonicalization.Location = new System.Drawing.Point(21, 20);
            this.rbSimpleHeaderCanonicalization.Name = "rbSimpleHeaderCanonicalization";
            this.rbSimpleHeaderCanonicalization.Size = new System.Drawing.Size(56, 17);
            this.rbSimpleHeaderCanonicalization.TabIndex = 0;
            this.rbSimpleHeaderCanonicalization.TabStop = true;
            this.rbSimpleHeaderCanonicalization.Text = "Simple";
            this.rbSimpleHeaderCanonicalization.UseVisualStyleBackColor = true;
            // 
            // gbAlgorithm
            // 
            this.gbAlgorithm.Controls.Add(this.rbRsaSha256);
            this.gbAlgorithm.Controls.Add(this.rbRsaSha1);
            this.gbAlgorithm.Location = new System.Drawing.Point(62, 104);
            this.gbAlgorithm.Name = "gbAlgorithm";
            this.gbAlgorithm.Size = new System.Drawing.Size(203, 52);
            this.gbAlgorithm.TabIndex = 0;
            this.gbAlgorithm.TabStop = false;
            this.gbAlgorithm.Text = "Algorithm";
            // 
            // rbRsaSha256
            // 
            this.rbRsaSha256.AutoSize = true;
            this.rbRsaSha256.Location = new System.Drawing.Point(105, 20);
            this.rbRsaSha256.Name = "rbRsaSha256";
            this.rbRsaSha256.Size = new System.Drawing.Size(81, 17);
            this.rbRsaSha256.TabIndex = 1;
            this.rbRsaSha256.Text = "RsaSha256";
            this.rbRsaSha256.UseVisualStyleBackColor = true;
            // 
            // rbRsaSha1
            // 
            this.rbRsaSha1.AutoSize = true;
            this.rbRsaSha1.Checked = true;
            this.rbRsaSha1.Location = new System.Drawing.Point(21, 20);
            this.rbRsaSha1.Name = "rbRsaSha1";
            this.rbRsaSha1.Size = new System.Drawing.Size(69, 17);
            this.rbRsaSha1.TabIndex = 0;
            this.rbRsaSha1.TabStop = true;
            this.rbRsaSha1.Text = "RsaSha1";
            this.rbRsaSha1.UseVisualStyleBackColor = true;
            // 
            // tpDomain
            // 
            this.tpDomain.Controls.Add(this.btDownload);
            this.tpDomain.Controls.Add(this.btUpload);
            this.tpDomain.Controls.Add(this.dgvDomainConfiguration);
            this.tpDomain.Location = new System.Drawing.Point(4, 29);
            this.tpDomain.Name = "tpDomain";
            this.tpDomain.Padding = new System.Windows.Forms.Padding(3);
            this.tpDomain.Size = new System.Drawing.Size(552, 324);
            this.tpDomain.TabIndex = 1;
            this.tpDomain.Text = "Domain";
            this.tpDomain.UseVisualStyleBackColor = true;
            // 
            // btDownload
            // 
            this.btDownload.Location = new System.Drawing.Point(469, 291);
            this.btDownload.Name = "btDownload";
            this.btDownload.Size = new System.Drawing.Size(75, 23);
            this.btDownload.TabIndex = 4;
            this.btDownload.Text = "Download";
            this.btDownload.UseVisualStyleBackColor = true;
            // 
            // btUpload
            // 
            this.btUpload.Location = new System.Drawing.Point(381, 291);
            this.btUpload.Name = "btUpload";
            this.btUpload.Size = new System.Drawing.Size(75, 23);
            this.btUpload.TabIndex = 3;
            this.btUpload.Text = "Upload";
            this.btUpload.UseVisualStyleBackColor = true;
            // 
            // dgvDomainConfiguration
            // 
            this.dgvDomainConfiguration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDomainConfiguration.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcDomain,
            this.dgvcSelector,
            this.dgvcPrivateKeyFile,
            this.dgvcSenderRule,
            this.dgvcRecipientRule});
            this.dgvDomainConfiguration.Location = new System.Drawing.Point(3, 3);
            this.dgvDomainConfiguration.Name = "dgvDomainConfiguration";
            this.dgvDomainConfiguration.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDomainConfiguration.Size = new System.Drawing.Size(546, 281);
            this.dgvDomainConfiguration.TabIndex = 0;
            // 
            // dgvcDomain
            // 
            this.dgvcDomain.HeaderText = "Domain";
            this.dgvcDomain.Name = "dgvcDomain";
            // 
            // dgvcSelector
            // 
            this.dgvcSelector.HeaderText = "Selector";
            this.dgvcSelector.Name = "dgvcSelector";
            // 
            // dgvcPrivateKeyFile
            // 
            this.dgvcPrivateKeyFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcPrivateKeyFile.HeaderText = "PrivateKeyFile";
            this.dgvcPrivateKeyFile.Name = "dgvcPrivateKeyFile";
            this.dgvcPrivateKeyFile.ReadOnly = true;
            this.dgvcPrivateKeyFile.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dgvcSenderRule
            // 
            this.dgvcSenderRule.HeaderText = "SenderRule";
            this.dgvcSenderRule.Name = "dgvcSenderRule";
            this.dgvcSenderRule.Visible = false;
            // 
            // dgvcRecipientRule
            // 
            this.dgvcRecipientRule.HeaderText = "RecipientRule";
            this.dgvcRecipientRule.Name = "dgvcRecipientRule";
            this.dgvcRecipientRule.Visible = false;
            // 
            // MainWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 381);
            this.Controls.Add(this.tcConfiguration);
            this.Name = "MainWindows";
            this.ShowIcon = false;
            this.Text = "Exchange DkimSigner";
            this.tcConfiguration.ResumeLayout(false);
            this.tbInformation.ResumeLayout(false);
            this.gbAvailable.ResumeLayout(false);
            this.gbAvailable.PerformLayout();
            this.gbInstalled.ResumeLayout(false);
            this.gbInstalled.PerformLayout();
            this.tpDKIM.ResumeLayout(false);
            this.gbHeaderToSign.ResumeLayout(false);
            this.gbHeaderToSign.PerformLayout();
            this.gbLogLevel.ResumeLayout(false);
            this.gbBodyCanonicalization.ResumeLayout(false);
            this.gbBodyCanonicalization.PerformLayout();
            this.gbHeaderCanonicalization.ResumeLayout(false);
            this.gbHeaderCanonicalization.PerformLayout();
            this.gbAlgorithm.ResumeLayout(false);
            this.gbAlgorithm.PerformLayout();
            this.tpDomain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDomainConfiguration)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcConfiguration;
        private System.Windows.Forms.TabPage tpDKIM;
        private System.Windows.Forms.TabPage tpDomain;
        private System.Windows.Forms.TabPage tbInformation;
        private System.Windows.Forms.GroupBox gbAvailable;
        private System.Windows.Forms.TextBox txtDkimSignerAvailable;
        private System.Windows.Forms.Label lbDkimSignerAvailable;
        private System.Windows.Forms.GroupBox gbInstalled;
        private System.Windows.Forms.Label lbExchangeInstalled;
        private System.Windows.Forms.TextBox txtDkimSignerInstalled;
        private System.Windows.Forms.TextBox txtExchangeInstalled;
        private System.Windows.Forms.Label lbDkimSignerInstalled;
        private System.Windows.Forms.GroupBox gbAlgorithm;
        private System.Windows.Forms.RadioButton rbRsaSha1;
        private System.Windows.Forms.RadioButton rbRsaSha256;
        private System.Windows.Forms.GroupBox gbHeaderCanonicalization;
        private System.Windows.Forms.RadioButton rbRelaxedHeaderCanonicalization;
        private System.Windows.Forms.RadioButton rbSimpleHeaderCanonicalization;
        private System.Windows.Forms.GroupBox gbBodyCanonicalization;
        private System.Windows.Forms.RadioButton rbRelaxedBodyCanonicalization;
        private System.Windows.Forms.RadioButton rbSimpleBodyCanonicalization;
        private System.Windows.Forms.GroupBox gbLogLevel;
        private System.Windows.Forms.ComboBox cbLogLevel;
        private System.Windows.Forms.GroupBox gbHeaderToSign;
        private System.Windows.Forms.TextBox txtHeaderToSign;
        private System.Windows.Forms.DataGridView dgvDomainConfiguration;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcDomain;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcSelector;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcPrivateKeyFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcSenderRule;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcRecipientRule;
        private System.Windows.Forms.Button btDownload;
        private System.Windows.Forms.Button btUpload;
        private System.Windows.Forms.Button btSave;
    }
}

