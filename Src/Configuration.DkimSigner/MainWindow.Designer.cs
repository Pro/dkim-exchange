﻿namespace Configuration.DkimSigner
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.tcConfiguration = new System.Windows.Forms.TabControl();
            this.tbInformation = new System.Windows.Forms.TabPage();
            this.gbDkimSigner = new System.Windows.Forms.GroupBox();
            this.btUpgrade = new System.Windows.Forms.Button();
            this.cbxPrereleases = new System.Windows.Forms.CheckBox();
            this.lblChangelog = new System.Windows.Forms.Label();
            this.txtChangelog = new System.Windows.Forms.TextBox();
            this.txtDkimSignerAvailable = new System.Windows.Forms.TextBox();
            this.btConfigureTransportService = new System.Windows.Forms.Button();
            this.lbDkimSignerAvailable = new System.Windows.Forms.Label();
            this.txtDkimSignerInstalled = new System.Windows.Forms.TextBox();
            this.lbDkimSignerInstalled = new System.Windows.Forms.Label();
            this.gbExchange = new System.Windows.Forms.GroupBox();
            this.txtExchangeStatus = new System.Windows.Forms.TextBox();
            this.btRestartTransportService = new System.Windows.Forms.Button();
            this.btStopTransportService = new System.Windows.Forms.Button();
            this.btStartTransportService = new System.Windows.Forms.Button();
            this.lbExchangeStatus = new System.Windows.Forms.Label();
            this.lbExchangeInstalled = new System.Windows.Forms.Label();
            this.txtExchangeInstalled = new System.Windows.Forms.TextBox();
            this.tpDKIM = new System.Windows.Forms.TabPage();
            this.gbAllowedSigners = new System.Windows.Forms.GroupBox();
            this.lbPermittedSignersNote = new System.Windows.Forms.Label();
            this.btSignersDel = new System.Windows.Forms.Button();
            this.btSignersAdd = new System.Windows.Forms.Button();
            this.lbxPermittedSigners = new System.Windows.Forms.ListBox();
            this.btSaveConfiguration = new System.Windows.Forms.Button();
            this.gbHeaderToSign = new System.Windows.Forms.GroupBox();
            this.btHeaderAdd = new System.Windows.Forms.Button();
            this.btHeaderDelete = new System.Windows.Forms.Button();
            this.lbxHeadersToSign = new System.Windows.Forms.ListBox();
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
            this.gbDomain = new System.Windows.Forms.GroupBox();
            this.lbxDomains = new System.Windows.Forms.ListBox();
            this.btAddDomain = new System.Windows.Forms.Button();
            this.btDomainDelete = new System.Windows.Forms.Button();
            this.gbxDomainDetails = new System.Windows.Forms.GroupBox();
            this.cbKeyLength = new System.Windows.Forms.ComboBox();
            this.lbKeyLength = new System.Windows.Forms.Label();
            this.txtDNSName = new System.Windows.Forms.TextBox();
            this.lbSuggestedDnsName = new System.Windows.Forms.Label();
            this.lbExistingDns = new System.Windows.Forms.Label();
            this.lbSuggestedDnsRecord = new System.Windows.Forms.Label();
            this.txtDNSRecord = new System.Windows.Forms.TextBox();
            this.btDomainCheckDNS = new System.Windows.Forms.Button();
            this.txtDomainDNS = new System.Windows.Forms.TextBox();
            this.btDomainSave = new System.Windows.Forms.Button();
            this.txtDomainPrivateKeyFilename = new System.Windows.Forms.TextBox();
            this.lbPrivateKey = new System.Windows.Forms.Label();
            this.txtDomainSelector = new System.Windows.Forms.TextBox();
            this.lbSelector = new System.Windows.Forms.Label();
            this.txtDomainName = new System.Windows.Forms.TextBox();
            this.lbDomainName = new System.Windows.Forms.Label();
            this.btDomainKeySelect = new System.Windows.Forms.Button();
            this.btDomainKeyGenerate = new System.Windows.Forms.Button();
            this.tpLog = new System.Windows.Forms.TabPage();
            this.btEventLogRefresh = new System.Windows.Forms.Button();
            this.dgEventLog = new System.Windows.Forms.DataGridView();
            this.icon = new System.Windows.Forms.DataGridViewImageColumn();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.tpAbout = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAbout = new Configuration.DkimSigner.CustomTextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.epvDomainSelector = new System.Windows.Forms.ErrorProvider(this.components);
            this.tpDebug = new System.Windows.Forms.TabPage();
            this.btExchangeVersion = new System.Windows.Forms.Button();
            this.tcConfiguration.SuspendLayout();
            this.tbInformation.SuspendLayout();
            this.gbDkimSigner.SuspendLayout();
            this.gbExchange.SuspendLayout();
            this.tpDKIM.SuspendLayout();
            this.gbAllowedSigners.SuspendLayout();
            this.gbHeaderToSign.SuspendLayout();
            this.gbLogLevel.SuspendLayout();
            this.gbBodyCanonicalization.SuspendLayout();
            this.gbHeaderCanonicalization.SuspendLayout();
            this.gbAlgorithm.SuspendLayout();
            this.tpDomain.SuspendLayout();
            this.gbDomain.SuspendLayout();
            this.gbxDomainDetails.SuspendLayout();
            this.tpLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEventLog)).BeginInit();
            this.tpAbout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epvDomainSelector)).BeginInit();
            this.tpDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcConfiguration
            // 
            this.tcConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcConfiguration.Controls.Add(this.tbInformation);
            this.tcConfiguration.Controls.Add(this.tpDKIM);
            this.tcConfiguration.Controls.Add(this.tpDomain);
            this.tcConfiguration.Controls.Add(this.tpLog);
            this.tcConfiguration.Controls.Add(this.tpAbout);
            this.tcConfiguration.Controls.Add(this.tpDebug);
            this.tcConfiguration.ItemSize = new System.Drawing.Size(150, 25);
            this.tcConfiguration.Location = new System.Drawing.Point(12, 12);
            this.tcConfiguration.Name = "tcConfiguration";
            this.tcConfiguration.SelectedIndex = 0;
            this.tcConfiguration.Size = new System.Drawing.Size(715, 476);
            this.tcConfiguration.TabIndex = 0;
            // 
            // tbInformation
            // 
            this.tbInformation.Controls.Add(this.gbDkimSigner);
            this.tbInformation.Controls.Add(this.gbExchange);
            this.tbInformation.Location = new System.Drawing.Point(4, 29);
            this.tbInformation.Name = "tbInformation";
            this.tbInformation.Size = new System.Drawing.Size(707, 443);
            this.tbInformation.TabIndex = 2;
            this.tbInformation.Text = "Information";
            this.tbInformation.UseVisualStyleBackColor = true;
            // 
            // gbDkimSigner
            // 
            this.gbDkimSigner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDkimSigner.Controls.Add(this.btUpgrade);
            this.gbDkimSigner.Controls.Add(this.cbxPrereleases);
            this.gbDkimSigner.Controls.Add(this.lblChangelog);
            this.gbDkimSigner.Controls.Add(this.txtChangelog);
            this.gbDkimSigner.Controls.Add(this.txtDkimSignerAvailable);
            this.gbDkimSigner.Controls.Add(this.btConfigureTransportService);
            this.gbDkimSigner.Controls.Add(this.lbDkimSignerAvailable);
            this.gbDkimSigner.Controls.Add(this.txtDkimSignerInstalled);
            this.gbDkimSigner.Controls.Add(this.lbDkimSignerInstalled);
            this.gbDkimSigner.Location = new System.Drawing.Point(3, 93);
            this.gbDkimSigner.Name = "gbDkimSigner";
            this.gbDkimSigner.Size = new System.Drawing.Size(695, 347);
            this.gbDkimSigner.TabIndex = 1;
            this.gbDkimSigner.TabStop = false;
            this.gbDkimSigner.Text = "DKIM Signer";
            // 
            // btUpgrade
            // 
            this.btUpgrade.Enabled = false;
            this.btUpgrade.Location = new System.Drawing.Point(255, 47);
            this.btUpgrade.Name = "btUpgrade";
            this.btUpgrade.Size = new System.Drawing.Size(180, 23);
            this.btUpgrade.TabIndex = 8;
            this.btUpgrade.Text = "&Upgrade";
            this.btUpgrade.UseVisualStyleBackColor = true;
            this.btUpgrade.Click += new System.EventHandler(this.btUpgrade_Click);
            // 
            // cbxPrereleases
            // 
            this.cbxPrereleases.AutoSize = true;
            this.cbxPrereleases.Location = new System.Drawing.Point(441, 51);
            this.cbxPrereleases.Name = "cbxPrereleases";
            this.cbxPrereleases.Size = new System.Drawing.Size(155, 17);
            this.cbxPrereleases.TabIndex = 5;
            this.cbxPrereleases.Text = "Include prerelease versions";
            this.cbxPrereleases.UseVisualStyleBackColor = true;
            this.cbxPrereleases.CheckedChanged += new System.EventHandler(this.cbxPrereleases_CheckedChanged);
            // 
            // lblChangelog
            // 
            this.lblChangelog.AutoSize = true;
            this.lblChangelog.Location = new System.Drawing.Point(6, 76);
            this.lblChangelog.Name = "lblChangelog";
            this.lblChangelog.Size = new System.Drawing.Size(61, 13);
            this.lblChangelog.TabIndex = 6;
            this.lblChangelog.Text = "Changelog:";
            // 
            // txtChangelog
            // 
            this.txtChangelog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChangelog.Location = new System.Drawing.Point(9, 92);
            this.txtChangelog.Multiline = true;
            this.txtChangelog.Name = "txtChangelog";
            this.txtChangelog.ReadOnly = true;
            this.txtChangelog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtChangelog.Size = new System.Drawing.Size(680, 249);
            this.txtChangelog.TabIndex = 7;
            // 
            // txtDkimSignerAvailable
            // 
            this.txtDkimSignerAvailable.Location = new System.Drawing.Point(82, 49);
            this.txtDkimSignerAvailable.Name = "txtDkimSignerAvailable";
            this.txtDkimSignerAvailable.ReadOnly = true;
            this.txtDkimSignerAvailable.Size = new System.Drawing.Size(138, 20);
            this.txtDkimSignerAvailable.TabIndex = 4;
            this.txtDkimSignerAvailable.Text = "Loading...";
            // 
            // btConfigureTransportService
            // 
            this.btConfigureTransportService.Enabled = false;
            this.btConfigureTransportService.Location = new System.Drawing.Point(255, 21);
            this.btConfigureTransportService.Name = "btConfigureTransportService";
            this.btConfigureTransportService.Size = new System.Drawing.Size(180, 23);
            this.btConfigureTransportService.TabIndex = 2;
            this.btConfigureTransportService.Text = "&Configure";
            this.btConfigureTransportService.UseVisualStyleBackColor = true;
            this.btConfigureTransportService.Click += new System.EventHandler(this.btConfigureTransportService_Click);
            // 
            // lbDkimSignerAvailable
            // 
            this.lbDkimSignerAvailable.AutoSize = true;
            this.lbDkimSignerAvailable.Location = new System.Drawing.Point(6, 52);
            this.lbDkimSignerAvailable.Name = "lbDkimSignerAvailable";
            this.lbDkimSignerAvailable.Size = new System.Drawing.Size(56, 13);
            this.lbDkimSignerAvailable.TabIndex = 3;
            this.lbDkimSignerAvailable.Text = "Available :";
            // 
            // txtDkimSignerInstalled
            // 
            this.txtDkimSignerInstalled.Location = new System.Drawing.Point(82, 23);
            this.txtDkimSignerInstalled.Name = "txtDkimSignerInstalled";
            this.txtDkimSignerInstalled.ReadOnly = true;
            this.txtDkimSignerInstalled.Size = new System.Drawing.Size(138, 20);
            this.txtDkimSignerInstalled.TabIndex = 1;
            this.txtDkimSignerInstalled.Text = "Loading...";
            // 
            // lbDkimSignerInstalled
            // 
            this.lbDkimSignerInstalled.AutoSize = true;
            this.lbDkimSignerInstalled.Location = new System.Drawing.Point(6, 26);
            this.lbDkimSignerInstalled.Name = "lbDkimSignerInstalled";
            this.lbDkimSignerInstalled.Size = new System.Drawing.Size(52, 13);
            this.lbDkimSignerInstalled.TabIndex = 0;
            this.lbDkimSignerInstalled.Text = "Installed :";
            // 
            // gbExchange
            // 
            this.gbExchange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbExchange.Controls.Add(this.txtExchangeStatus);
            this.gbExchange.Controls.Add(this.btRestartTransportService);
            this.gbExchange.Controls.Add(this.btStopTransportService);
            this.gbExchange.Controls.Add(this.btStartTransportService);
            this.gbExchange.Controls.Add(this.lbExchangeStatus);
            this.gbExchange.Controls.Add(this.lbExchangeInstalled);
            this.gbExchange.Controls.Add(this.txtExchangeInstalled);
            this.gbExchange.Location = new System.Drawing.Point(3, 3);
            this.gbExchange.Name = "gbExchange";
            this.gbExchange.Size = new System.Drawing.Size(695, 84);
            this.gbExchange.TabIndex = 0;
            this.gbExchange.TabStop = false;
            this.gbExchange.Text = "Micosoft Exchange";
            // 
            // txtExchangeStatus
            // 
            this.txtExchangeStatus.Location = new System.Drawing.Point(388, 17);
            this.txtExchangeStatus.Name = "txtExchangeStatus";
            this.txtExchangeStatus.ReadOnly = true;
            this.txtExchangeStatus.Size = new System.Drawing.Size(138, 20);
            this.txtExchangeStatus.TabIndex = 3;
            this.txtExchangeStatus.Text = "Loading...";
            this.txtExchangeStatus.TextChanged += new System.EventHandler(this.txtExchangeStatus_TextChanged);
            // 
            // btRestartTransportService
            // 
            this.btRestartTransportService.Enabled = false;
            this.btRestartTransportService.Location = new System.Drawing.Point(441, 47);
            this.btRestartTransportService.Name = "btRestartTransportService";
            this.btRestartTransportService.Size = new System.Drawing.Size(87, 23);
            this.btRestartTransportService.TabIndex = 6;
            this.btRestartTransportService.Text = "Re&start";
            this.btRestartTransportService.UseVisualStyleBackColor = true;
            this.btRestartTransportService.Click += new System.EventHandler(this.genericTransportService_Click);
            // 
            // btStopTransportService
            // 
            this.btStopTransportService.Enabled = false;
            this.btStopTransportService.Location = new System.Drawing.Point(348, 47);
            this.btStopTransportService.Name = "btStopTransportService";
            this.btStopTransportService.Size = new System.Drawing.Size(87, 23);
            this.btStopTransportService.TabIndex = 5;
            this.btStopTransportService.Text = "St&op";
            this.btStopTransportService.UseVisualStyleBackColor = true;
            this.btStopTransportService.Click += new System.EventHandler(this.genericTransportService_Click);
            // 
            // btStartTransportService
            // 
            this.btStartTransportService.Enabled = false;
            this.btStartTransportService.Location = new System.Drawing.Point(255, 47);
            this.btStartTransportService.Name = "btStartTransportService";
            this.btStartTransportService.Size = new System.Drawing.Size(87, 23);
            this.btStartTransportService.TabIndex = 4;
            this.btStartTransportService.Text = "St&art";
            this.btStartTransportService.UseVisualStyleBackColor = true;
            this.btStartTransportService.Click += new System.EventHandler(this.genericTransportService_Click);
            // 
            // lbExchangeStatus
            // 
            this.lbExchangeStatus.AutoSize = true;
            this.lbExchangeStatus.Location = new System.Drawing.Point(252, 20);
            this.lbExchangeStatus.Name = "lbExchangeStatus";
            this.lbExchangeStatus.Size = new System.Drawing.Size(130, 13);
            this.lbExchangeStatus.TabIndex = 2;
            this.lbExchangeStatus.Text = "Transport Service Status :";
            // 
            // lbExchangeInstalled
            // 
            this.lbExchangeInstalled.AutoSize = true;
            this.lbExchangeInstalled.Location = new System.Drawing.Point(6, 20);
            this.lbExchangeInstalled.Name = "lbExchangeInstalled";
            this.lbExchangeInstalled.Size = new System.Drawing.Size(52, 13);
            this.lbExchangeInstalled.TabIndex = 0;
            this.lbExchangeInstalled.Text = "Installed :";
            // 
            // txtExchangeInstalled
            // 
            this.txtExchangeInstalled.Location = new System.Drawing.Point(82, 17);
            this.txtExchangeInstalled.Name = "txtExchangeInstalled";
            this.txtExchangeInstalled.ReadOnly = true;
            this.txtExchangeInstalled.Size = new System.Drawing.Size(138, 20);
            this.txtExchangeInstalled.TabIndex = 1;
            this.txtExchangeInstalled.Text = "Loading...";
            // 
            // tpDKIM
            // 
            this.tpDKIM.Controls.Add(this.gbAllowedSigners);
            this.tpDKIM.Controls.Add(this.btSaveConfiguration);
            this.tpDKIM.Controls.Add(this.gbHeaderToSign);
            this.tpDKIM.Controls.Add(this.gbLogLevel);
            this.tpDKIM.Controls.Add(this.gbBodyCanonicalization);
            this.tpDKIM.Controls.Add(this.gbHeaderCanonicalization);
            this.tpDKIM.Controls.Add(this.gbAlgorithm);
            this.tpDKIM.Location = new System.Drawing.Point(4, 29);
            this.tpDKIM.Name = "tpDKIM";
            this.tpDKIM.Padding = new System.Windows.Forms.Padding(3);
            this.tpDKIM.Size = new System.Drawing.Size(707, 443);
            this.tpDKIM.TabIndex = 0;
            this.tpDKIM.Text = "DKIM Settings";
            this.tpDKIM.UseVisualStyleBackColor = true;
            // 
            // gbAllowedSigners
            // 
            this.gbAllowedSigners.Controls.Add(this.lbPermittedSignersNote);
            this.gbAllowedSigners.Controls.Add(this.btSignersDel);
            this.gbAllowedSigners.Controls.Add(this.btSignersAdd);
            this.gbAllowedSigners.Controls.Add(this.lbxPermittedSigners);
            this.gbAllowedSigners.Location = new System.Drawing.Point(243, 132);
            this.gbAllowedSigners.Name = "gbAllowedSigners";
            this.gbAllowedSigners.Size = new System.Drawing.Size(426, 258);
            this.gbAllowedSigners.TabIndex = 6;
            this.gbAllowedSigners.TabStop = false;
            this.gbAllowedSigners.Text = "Allowed signers";
            // 
            // lbPermittedSignersNote
            // 
            this.lbPermittedSignersNote.Location = new System.Drawing.Point(18, 207);
            this.lbPermittedSignersNote.Name = "lbPermittedSignersNote";
            this.lbPermittedSignersNote.Size = new System.Drawing.Size(380, 48);
            this.lbPermittedSignersNote.TabIndex = 4;
            this.lbPermittedSignersNote.Text = "These is a list of permitted signers allowed to sign email messages with DKIM sig" +
    "nature. If this list is empty all email messages will be signed no matter of dir" +
    "ection.";
            // 
            // btSignersDel
            // 
            this.btSignersDel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btSignersDel.Location = new System.Drawing.Point(336, 69);
            this.btSignersDel.Name = "btSignersDel";
            this.btSignersDel.Size = new System.Drawing.Size(65, 23);
            this.btSignersDel.TabIndex = 3;
            this.btSignersDel.Text = "&Delete";
            this.btSignersDel.UseVisualStyleBackColor = true;
            this.btSignersDel.Click += new System.EventHandler(this.btSignersDel_Click);
            // 
            // btSignersAdd
            // 
            this.btSignersAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btSignersAdd.Location = new System.Drawing.Point(336, 31);
            this.btSignersAdd.Name = "btSignersAdd";
            this.btSignersAdd.Size = new System.Drawing.Size(65, 23);
            this.btSignersAdd.TabIndex = 2;
            this.btSignersAdd.Text = "&Add";
            this.btSignersAdd.UseVisualStyleBackColor = true;
            this.btSignersAdd.Click += new System.EventHandler(this.btSignersAdd_Click);
            // 
            // lbxPermittedSigners
            // 
            this.lbxPermittedSigners.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbxPermittedSigners.FormattingEnabled = true;
            this.lbxPermittedSigners.IntegralHeight = false;
            this.lbxPermittedSigners.Location = new System.Drawing.Point(21, 28);
            this.lbxPermittedSigners.Name = "lbxPermittedSigners";
            this.lbxPermittedSigners.Size = new System.Drawing.Size(296, 169);
            this.lbxPermittedSigners.TabIndex = 1;
            // 
            // btSaveConfiguration
            // 
            this.btSaveConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btSaveConfiguration.Location = new System.Drawing.Point(243, 408);
            this.btSaveConfiguration.Name = "btSaveConfiguration";
            this.btSaveConfiguration.Size = new System.Drawing.Size(426, 23);
            this.btSaveConfiguration.TabIndex = 5;
            this.btSaveConfiguration.Text = "&Save configuration";
            this.btSaveConfiguration.UseVisualStyleBackColor = true;
            this.btSaveConfiguration.Click += new System.EventHandler(this.btSaveConfiguration_Click);
            // 
            // gbHeaderToSign
            // 
            this.gbHeaderToSign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbHeaderToSign.Controls.Add(this.btHeaderAdd);
            this.gbHeaderToSign.Controls.Add(this.btHeaderDelete);
            this.gbHeaderToSign.Controls.Add(this.lbxHeadersToSign);
            this.gbHeaderToSign.Location = new System.Drawing.Point(6, 6);
            this.gbHeaderToSign.Name = "gbHeaderToSign";
            this.gbHeaderToSign.Size = new System.Drawing.Size(216, 431);
            this.gbHeaderToSign.TabIndex = 0;
            this.gbHeaderToSign.TabStop = false;
            this.gbHeaderToSign.Text = "Header to sign";
            // 
            // btHeaderAdd
            // 
            this.btHeaderAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btHeaderAdd.Location = new System.Drawing.Point(39, 402);
            this.btHeaderAdd.Name = "btHeaderAdd";
            this.btHeaderAdd.Size = new System.Drawing.Size(65, 23);
            this.btHeaderAdd.TabIndex = 1;
            this.btHeaderAdd.Text = "&Add";
            this.btHeaderAdd.UseVisualStyleBackColor = true;
            this.btHeaderAdd.Click += new System.EventHandler(this.btHeaderAdd_Click);
            // 
            // btHeaderDelete
            // 
            this.btHeaderDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btHeaderDelete.Location = new System.Drawing.Point(110, 402);
            this.btHeaderDelete.Name = "btHeaderDelete";
            this.btHeaderDelete.Size = new System.Drawing.Size(65, 23);
            this.btHeaderDelete.TabIndex = 2;
            this.btHeaderDelete.Text = "&Delete";
            this.btHeaderDelete.UseVisualStyleBackColor = true;
            this.btHeaderDelete.Click += new System.EventHandler(this.btHeaderDelete_Click);
            // 
            // lbxHeadersToSign
            // 
            this.lbxHeadersToSign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbxHeadersToSign.FormattingEnabled = true;
            this.lbxHeadersToSign.IntegralHeight = false;
            this.lbxHeadersToSign.Location = new System.Drawing.Point(6, 19);
            this.lbxHeadersToSign.Name = "lbxHeadersToSign";
            this.lbxHeadersToSign.Size = new System.Drawing.Size(202, 377);
            this.lbxHeadersToSign.TabIndex = 0;
            this.lbxHeadersToSign.SelectedIndexChanged += new System.EventHandler(this.lbxHeadersToSign_SelectedIndexChanged);
            // 
            // gbLogLevel
            // 
            this.gbLogLevel.Controls.Add(this.cbLogLevel);
            this.gbLogLevel.Location = new System.Drawing.Point(243, 64);
            this.gbLogLevel.Name = "gbLogLevel";
            this.gbLogLevel.Size = new System.Drawing.Size(203, 52);
            this.gbLogLevel.TabIndex = 2;
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
            "Information",
            "Debug"});
            this.cbLogLevel.Location = new System.Drawing.Point(6, 22);
            this.cbLogLevel.Name = "cbLogLevel";
            this.cbLogLevel.Size = new System.Drawing.Size(191, 21);
            this.cbLogLevel.TabIndex = 0;
            this.cbLogLevel.TextChanged += new System.EventHandler(this.generic_ValueChanged);
            // 
            // gbBodyCanonicalization
            // 
            this.gbBodyCanonicalization.Controls.Add(this.rbRelaxedBodyCanonicalization);
            this.gbBodyCanonicalization.Controls.Add(this.rbSimpleBodyCanonicalization);
            this.gbBodyCanonicalization.Location = new System.Drawing.Point(452, 64);
            this.gbBodyCanonicalization.Name = "gbBodyCanonicalization";
            this.gbBodyCanonicalization.Size = new System.Drawing.Size(217, 52);
            this.gbBodyCanonicalization.TabIndex = 4;
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
            this.rbRelaxedBodyCanonicalization.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
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
            this.rbSimpleBodyCanonicalization.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
            // 
            // gbHeaderCanonicalization
            // 
            this.gbHeaderCanonicalization.Controls.Add(this.rbRelaxedHeaderCanonicalization);
            this.gbHeaderCanonicalization.Controls.Add(this.rbSimpleHeaderCanonicalization);
            this.gbHeaderCanonicalization.Location = new System.Drawing.Point(452, 6);
            this.gbHeaderCanonicalization.Name = "gbHeaderCanonicalization";
            this.gbHeaderCanonicalization.Size = new System.Drawing.Size(217, 52);
            this.gbHeaderCanonicalization.TabIndex = 3;
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
            this.rbRelaxedHeaderCanonicalization.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
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
            this.rbSimpleHeaderCanonicalization.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
            // 
            // gbAlgorithm
            // 
            this.gbAlgorithm.Controls.Add(this.rbRsaSha256);
            this.gbAlgorithm.Controls.Add(this.rbRsaSha1);
            this.gbAlgorithm.Location = new System.Drawing.Point(243, 6);
            this.gbAlgorithm.Name = "gbAlgorithm";
            this.gbAlgorithm.Size = new System.Drawing.Size(203, 52);
            this.gbAlgorithm.TabIndex = 1;
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
            this.rbRsaSha256.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
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
            this.rbRsaSha1.CheckedChanged += new System.EventHandler(this.generic_ValueChanged);
            // 
            // tpDomain
            // 
            this.tpDomain.Controls.Add(this.gbDomain);
            this.tpDomain.Controls.Add(this.gbxDomainDetails);
            this.tpDomain.Location = new System.Drawing.Point(4, 29);
            this.tpDomain.Name = "tpDomain";
            this.tpDomain.Padding = new System.Windows.Forms.Padding(3);
            this.tpDomain.Size = new System.Drawing.Size(707, 443);
            this.tpDomain.TabIndex = 1;
            this.tpDomain.Text = "Domain Settings";
            this.tpDomain.UseVisualStyleBackColor = true;
            // 
            // gbDomain
            // 
            this.gbDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbDomain.Controls.Add(this.lbxDomains);
            this.gbDomain.Controls.Add(this.btAddDomain);
            this.gbDomain.Controls.Add(this.btDomainDelete);
            this.gbDomain.Location = new System.Drawing.Point(6, 9);
            this.gbDomain.Name = "gbDomain";
            this.gbDomain.Size = new System.Drawing.Size(151, 431);
            this.gbDomain.TabIndex = 0;
            this.gbDomain.TabStop = false;
            this.gbDomain.Text = "Domains";
            // 
            // lbxDomains
            // 
            this.lbxDomains.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbxDomains.FormattingEnabled = true;
            this.lbxDomains.IntegralHeight = false;
            this.lbxDomains.Location = new System.Drawing.Point(6, 19);
            this.lbxDomains.Name = "lbxDomains";
            this.lbxDomains.Size = new System.Drawing.Size(139, 377);
            this.lbxDomains.TabIndex = 0;
            this.lbxDomains.SelectedIndexChanged += new System.EventHandler(this.lbxDomains_SelectedIndexChanged);
            // 
            // btAddDomain
            // 
            this.btAddDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btAddDomain.Location = new System.Drawing.Point(6, 402);
            this.btAddDomain.Name = "btAddDomain";
            this.btAddDomain.Size = new System.Drawing.Size(65, 23);
            this.btAddDomain.TabIndex = 1;
            this.btAddDomain.Text = "&Add";
            this.btAddDomain.UseVisualStyleBackColor = true;
            this.btAddDomain.Click += new System.EventHandler(this.btAddDomain_Click);
            // 
            // btDomainDelete
            // 
            this.btDomainDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDomainDelete.Location = new System.Drawing.Point(80, 402);
            this.btDomainDelete.Name = "btDomainDelete";
            this.btDomainDelete.Size = new System.Drawing.Size(65, 23);
            this.btDomainDelete.TabIndex = 2;
            this.btDomainDelete.Text = "&Delete";
            this.btDomainDelete.UseVisualStyleBackColor = true;
            this.btDomainDelete.Click += new System.EventHandler(this.btDomainDelete_Click);
            // 
            // gbxDomainDetails
            // 
            this.gbxDomainDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDomainDetails.Controls.Add(this.cbKeyLength);
            this.gbxDomainDetails.Controls.Add(this.lbKeyLength);
            this.gbxDomainDetails.Controls.Add(this.txtDNSName);
            this.gbxDomainDetails.Controls.Add(this.lbSuggestedDnsName);
            this.gbxDomainDetails.Controls.Add(this.lbExistingDns);
            this.gbxDomainDetails.Controls.Add(this.lbSuggestedDnsRecord);
            this.gbxDomainDetails.Controls.Add(this.txtDNSRecord);
            this.gbxDomainDetails.Controls.Add(this.btDomainCheckDNS);
            this.gbxDomainDetails.Controls.Add(this.txtDomainDNS);
            this.gbxDomainDetails.Controls.Add(this.btDomainSave);
            this.gbxDomainDetails.Controls.Add(this.txtDomainPrivateKeyFilename);
            this.gbxDomainDetails.Controls.Add(this.lbPrivateKey);
            this.gbxDomainDetails.Controls.Add(this.txtDomainSelector);
            this.gbxDomainDetails.Controls.Add(this.lbSelector);
            this.gbxDomainDetails.Controls.Add(this.txtDomainName);
            this.gbxDomainDetails.Controls.Add(this.lbDomainName);
            this.gbxDomainDetails.Controls.Add(this.btDomainKeySelect);
            this.gbxDomainDetails.Controls.Add(this.btDomainKeyGenerate);
            this.gbxDomainDetails.Enabled = false;
            this.gbxDomainDetails.Location = new System.Drawing.Point(163, 9);
            this.gbxDomainDetails.Name = "gbxDomainDetails";
            this.gbxDomainDetails.Size = new System.Drawing.Size(538, 431);
            this.gbxDomainDetails.TabIndex = 1;
            this.gbxDomainDetails.TabStop = false;
            this.gbxDomainDetails.Text = "Domain details";
            // 
            // cbKeyLength
            // 
            this.cbKeyLength.FormattingEnabled = true;
            this.cbKeyLength.Items.AddRange(new object[] {
            "1024",
            "2048"});
            this.cbKeyLength.Location = new System.Drawing.Point(137, 72);
            this.cbKeyLength.Name = "cbKeyLength";
            this.cbKeyLength.Size = new System.Drawing.Size(87, 21);
            this.cbKeyLength.TabIndex = 7;
            // 
            // lbKeyLength
            // 
            this.lbKeyLength.AutoSize = true;
            this.lbKeyLength.Location = new System.Drawing.Point(6, 76);
            this.lbKeyLength.Name = "lbKeyLength";
            this.lbKeyLength.Size = new System.Drawing.Size(131, 13);
            this.lbKeyLength.TabIndex = 6;
            this.lbKeyLength.Text = "Key length for generation :";
            // 
            // txtDNSName
            // 
            this.txtDNSName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDNSName.Location = new System.Drawing.Point(137, 97);
            this.txtDNSName.Name = "txtDNSName";
            this.txtDNSName.ReadOnly = true;
            this.txtDNSName.Size = new System.Drawing.Size(385, 20);
            this.txtDNSName.TabIndex = 11;
            // 
            // lbSuggestedDnsName
            // 
            this.lbSuggestedDnsName.AutoSize = true;
            this.lbSuggestedDnsName.Location = new System.Drawing.Point(6, 100);
            this.lbSuggestedDnsName.Name = "lbSuggestedDnsName";
            this.lbSuggestedDnsName.Size = new System.Drawing.Size(121, 13);
            this.lbSuggestedDnsName.TabIndex = 10;
            this.lbSuggestedDnsName.Text = "Suggested DNS Name :";
            // 
            // lbExistingDns
            // 
            this.lbExistingDns.AutoSize = true;
            this.lbExistingDns.Location = new System.Drawing.Point(6, 184);
            this.lbExistingDns.Name = "lbExistingDns";
            this.lbExistingDns.Size = new System.Drawing.Size(75, 13);
            this.lbExistingDns.TabIndex = 14;
            this.lbExistingDns.Text = "Existing DNS :";
            // 
            // lbSuggestedDnsRecord
            // 
            this.lbSuggestedDnsRecord.AutoSize = true;
            this.lbSuggestedDnsRecord.Location = new System.Drawing.Point(6, 126);
            this.lbSuggestedDnsRecord.Name = "lbSuggestedDnsRecord";
            this.lbSuggestedDnsRecord.Size = new System.Drawing.Size(128, 13);
            this.lbSuggestedDnsRecord.TabIndex = 12;
            this.lbSuggestedDnsRecord.Text = "Suggested DNS Record :";
            // 
            // txtDNSRecord
            // 
            this.txtDNSRecord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDNSRecord.Location = new System.Drawing.Point(137, 123);
            this.txtDNSRecord.Multiline = true;
            this.txtDNSRecord.Name = "txtDNSRecord";
            this.txtDNSRecord.ReadOnly = true;
            this.txtDNSRecord.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDNSRecord.Size = new System.Drawing.Size(385, 52);
            this.txtDNSRecord.TabIndex = 13;
            // 
            // btDomainCheckDNS
            // 
            this.btDomainCheckDNS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btDomainCheckDNS.Location = new System.Drawing.Point(9, 210);
            this.btDomainCheckDNS.Name = "btDomainCheckDNS";
            this.btDomainCheckDNS.Size = new System.Drawing.Size(69, 23);
            this.btDomainCheckDNS.TabIndex = 16;
            this.btDomainCheckDNS.Text = "&Check";
            this.btDomainCheckDNS.UseVisualStyleBackColor = true;
            this.btDomainCheckDNS.Click += new System.EventHandler(this.btDomainCheckDNS_Click);
            // 
            // txtDomainDNS
            // 
            this.txtDomainDNS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomainDNS.Location = new System.Drawing.Point(137, 181);
            this.txtDomainDNS.Multiline = true;
            this.txtDomainDNS.Name = "txtDomainDNS";
            this.txtDomainDNS.ReadOnly = true;
            this.txtDomainDNS.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDomainDNS.Size = new System.Drawing.Size(385, 52);
            this.txtDomainDNS.TabIndex = 15;
            // 
            // btDomainSave
            // 
            this.btDomainSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btDomainSave.Location = new System.Drawing.Point(9, 399);
            this.btDomainSave.Name = "btDomainSave";
            this.btDomainSave.Size = new System.Drawing.Size(513, 23);
            this.btDomainSave.TabIndex = 17;
            this.btDomainSave.Text = "&Save domain";
            this.btDomainSave.UseVisualStyleBackColor = true;
            this.btDomainSave.Click += new System.EventHandler(this.btDomainSave_Click);
            // 
            // txtDomainPrivateKeyFilename
            // 
            this.txtDomainPrivateKeyFilename.Location = new System.Drawing.Point(137, 48);
            this.txtDomainPrivateKeyFilename.Name = "txtDomainPrivateKeyFilename";
            this.txtDomainPrivateKeyFilename.ReadOnly = true;
            this.txtDomainPrivateKeyFilename.Size = new System.Drawing.Size(385, 20);
            this.txtDomainPrivateKeyFilename.TabIndex = 5;
            // 
            // lbPrivateKey
            // 
            this.lbPrivateKey.AutoSize = true;
            this.lbPrivateKey.Location = new System.Drawing.Point(6, 48);
            this.lbPrivateKey.Name = "lbPrivateKey";
            this.lbPrivateKey.Size = new System.Drawing.Size(108, 13);
            this.lbPrivateKey.TabIndex = 4;
            this.lbPrivateKey.Text = "Private key filename :";
            // 
            // txtDomainSelector
            // 
            this.txtDomainSelector.Location = new System.Drawing.Point(377, 15);
            this.txtDomainSelector.Name = "txtDomainSelector";
            this.txtDomainSelector.Size = new System.Drawing.Size(145, 20);
            this.txtDomainSelector.TabIndex = 3;
            this.txtDomainSelector.TextChanged += new System.EventHandler(this.txtDomainSelector_TextChanged);
            // 
            // lbSelector
            // 
            this.lbSelector.AutoSize = true;
            this.lbSelector.Location = new System.Drawing.Point(313, 18);
            this.lbSelector.Name = "lbSelector";
            this.lbSelector.Size = new System.Drawing.Size(49, 13);
            this.lbSelector.TabIndex = 2;
            this.lbSelector.Text = "Selector:";
            // 
            // txtDomainName
            // 
            this.txtDomainName.Location = new System.Drawing.Point(137, 19);
            this.txtDomainName.Name = "txtDomainName";
            this.txtDomainName.Size = new System.Drawing.Size(145, 20);
            this.txtDomainName.TabIndex = 1;
            this.txtDomainName.TextChanged += new System.EventHandler(this.txtDomainName_TextChanged);
            // 
            // lbDomainName
            // 
            this.lbDomainName.AutoSize = true;
            this.lbDomainName.Location = new System.Drawing.Point(6, 22);
            this.lbDomainName.Name = "lbDomainName";
            this.lbDomainName.Size = new System.Drawing.Size(78, 13);
            this.lbDomainName.TabIndex = 0;
            this.lbDomainName.Text = "Domain name :";
            // 
            // btDomainKeySelect
            // 
            this.btDomainKeySelect.AutoSize = true;
            this.btDomainKeySelect.Location = new System.Drawing.Point(418, 71);
            this.btDomainKeySelect.Name = "btDomainKeySelect";
            this.btDomainKeySelect.Size = new System.Drawing.Size(104, 23);
            this.btDomainKeySelect.TabIndex = 9;
            this.btDomainKeySelect.Text = "S&elect key file";
            this.btDomainKeySelect.UseVisualStyleBackColor = true;
            this.btDomainKeySelect.Click += new System.EventHandler(this.btDomainKeySelect_Click);
            // 
            // btDomainKeyGenerate
            // 
            this.btDomainKeyGenerate.AutoSize = true;
            this.btDomainKeyGenerate.Location = new System.Drawing.Point(308, 71);
            this.btDomainKeyGenerate.Name = "btDomainKeyGenerate";
            this.btDomainKeyGenerate.Size = new System.Drawing.Size(104, 23);
            this.btDomainKeyGenerate.TabIndex = 8;
            this.btDomainKeyGenerate.Text = "&Generate new key";
            this.btDomainKeyGenerate.UseVisualStyleBackColor = true;
            this.btDomainKeyGenerate.Click += new System.EventHandler(this.btDomainKeyGenerate_Click);
            // 
            // tpLog
            // 
            this.tpLog.Controls.Add(this.btEventLogRefresh);
            this.tpLog.Controls.Add(this.dgEventLog);
            this.tpLog.Controls.Add(this.label2);
            this.tpLog.Location = new System.Drawing.Point(4, 29);
            this.tpLog.Name = "tpLog";
            this.tpLog.Padding = new System.Windows.Forms.Padding(3);
            this.tpLog.Size = new System.Drawing.Size(707, 443);
            this.tpLog.TabIndex = 4;
            this.tpLog.Text = "EventLog Viewer";
            this.tpLog.UseVisualStyleBackColor = true;
            // 
            // btEventLogRefresh
            // 
            this.btEventLogRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btEventLogRefresh.Location = new System.Drawing.Point(618, 414);
            this.btEventLogRefresh.Name = "btEventLogRefresh";
            this.btEventLogRefresh.Size = new System.Drawing.Size(75, 23);
            this.btEventLogRefresh.TabIndex = 2;
            this.btEventLogRefresh.Text = "Refresh";
            this.btEventLogRefresh.UseVisualStyleBackColor = true;
            this.btEventLogRefresh.Click += new System.EventHandler(this.btEventLogRefresh_Click);
            // 
            // dgEventLog
            // 
            this.dgEventLog.AllowUserToAddRows = false;
            this.dgEventLog.AllowUserToDeleteRows = false;
            this.dgEventLog.AllowUserToResizeColumns = false;
            this.dgEventLog.AllowUserToResizeRows = false;
            this.dgEventLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgEventLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgEventLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgEventLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.icon,
            this.time,
            this.message});
            this.dgEventLog.Location = new System.Drawing.Point(11, 37);
            this.dgEventLog.MultiSelect = false;
            this.dgEventLog.Name = "dgEventLog";
            this.dgEventLog.ReadOnly = true;
            this.dgEventLog.RowHeadersVisible = false;
            this.dgEventLog.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgEventLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgEventLog.Size = new System.Drawing.Size(682, 371);
            this.dgEventLog.TabIndex = 1;
            // 
            // icon
            // 
            this.icon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.icon.HeaderText = "";
            this.icon.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.icon.MinimumWidth = 24;
            this.icon.Name = "icon";
            this.icon.ReadOnly = true;
            this.icon.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.icon.Width = 24;
            // 
            // time
            // 
            this.time.HeaderText = "Time";
            this.time.Name = "time";
            this.time.ReadOnly = true;
            this.time.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.time.Width = 55;
            // 
            // message
            // 
            this.message.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.message.HeaderText = "Message";
            this.message.Name = "message";
            this.message.ReadOnly = true;
            this.message.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(206, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "EventLog entries for DKIM Signing agent :";
            // 
            // tpAbout
            // 
            this.tpAbout.BackColor = System.Drawing.Color.Transparent;
            this.tpAbout.Controls.Add(this.label1);
            this.tpAbout.Controls.Add(this.txtAbout);
            this.tpAbout.Controls.Add(this.picLogo);
            this.tpAbout.Location = new System.Drawing.Point(4, 29);
            this.tpAbout.Name = "tpAbout";
            this.tpAbout.Size = new System.Drawing.Size(707, 443);
            this.tpAbout.TabIndex = 3;
            this.tpAbout.Text = "About";
            this.tpAbout.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(155, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(441, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "DKIM Signer for Microsoft Exchanger Server";
            // 
            // txtAbout
            // 
            this.txtAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAbout.BackColor = System.Drawing.Color.Transparent;
            this.txtAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAbout.Location = new System.Drawing.Point(158, 59);
            this.txtAbout.Multiline = true;
            this.txtAbout.Name = "txtAbout";
            this.txtAbout.Size = new System.Drawing.Size(531, 369);
            this.txtAbout.TabIndex = 1;
            // 
            // picLogo
            // 
            this.picLogo.Image = ((System.Drawing.Image)(resources.GetObject("picLogo.Image")));
            this.picLogo.Location = new System.Drawing.Point(14, 18);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(128, 128);
            this.picLogo.TabIndex = 0;
            this.picLogo.TabStop = false;
            // 
            // epvDomainSelector
            // 
            this.epvDomainSelector.ContainerControl = this;
            // 
            // tpDebug
            // 
            this.tpDebug.Controls.Add(this.btExchangeVersion);
            this.tpDebug.Location = new System.Drawing.Point(4, 29);
            this.tpDebug.Name = "tpDebug";
            this.tpDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tpDebug.Size = new System.Drawing.Size(707, 443);
            this.tpDebug.TabIndex = 5;
            this.tpDebug.Text = "Debug Helper";
            this.tpDebug.UseVisualStyleBackColor = true;
            // 
            // btExchangeVersion
            // 
            this.btExchangeVersion.Location = new System.Drawing.Point(6, 6);
            this.btExchangeVersion.Name = "btExchangeVersion";
            this.btExchangeVersion.Size = new System.Drawing.Size(199, 23);
            this.btExchangeVersion.TabIndex = 0;
            this.btExchangeVersion.Text = "Show Exchange Version as HEX";
            this.btExchangeVersion.UseVisualStyleBackColor = true;
            this.btExchangeVersion.Click += new System.EventHandler(this.btExchangeVersion_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 512);
            this.Controls.Add(this.tcConfiguration);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(730, 380);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exchange DKIM Signer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.tcConfiguration.ResumeLayout(false);
            this.tbInformation.ResumeLayout(false);
            this.gbDkimSigner.ResumeLayout(false);
            this.gbDkimSigner.PerformLayout();
            this.gbExchange.ResumeLayout(false);
            this.gbExchange.PerformLayout();
            this.tpDKIM.ResumeLayout(false);
            this.gbAllowedSigners.ResumeLayout(false);
            this.gbHeaderToSign.ResumeLayout(false);
            this.gbLogLevel.ResumeLayout(false);
            this.gbBodyCanonicalization.ResumeLayout(false);
            this.gbBodyCanonicalization.PerformLayout();
            this.gbHeaderCanonicalization.ResumeLayout(false);
            this.gbHeaderCanonicalization.PerformLayout();
            this.gbAlgorithm.ResumeLayout(false);
            this.gbAlgorithm.PerformLayout();
            this.tpDomain.ResumeLayout(false);
            this.gbDomain.ResumeLayout(false);
            this.gbxDomainDetails.ResumeLayout(false);
            this.gbxDomainDetails.PerformLayout();
            this.tpLog.ResumeLayout(false);
            this.tpLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEventLog)).EndInit();
            this.tpAbout.ResumeLayout(false);
            this.tpAbout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epvDomainSelector)).EndInit();
            this.tpDebug.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcConfiguration;
        private System.Windows.Forms.TabPage tpDKIM;
        private System.Windows.Forms.TabPage tpDomain;
        private System.Windows.Forms.TabPage tbInformation;
        private System.Windows.Forms.GroupBox gbDkimSigner;
        private System.Windows.Forms.TextBox txtDkimSignerAvailable;
        private System.Windows.Forms.Label lbDkimSignerAvailable;
        private System.Windows.Forms.GroupBox gbExchange;
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
        private System.Windows.Forms.Button btDomainKeySelect;
        private System.Windows.Forms.Button btSaveConfiguration;
        private System.Windows.Forms.Label lblChangelog;
        private System.Windows.Forms.TextBox txtChangelog;
        private System.Windows.Forms.Button btDomainKeyGenerate;
        private System.Windows.Forms.CheckBox cbxPrereleases;
        private System.Windows.Forms.GroupBox gbxDomainDetails;
        private System.Windows.Forms.Button btDomainDelete;
        private System.Windows.Forms.Button btDomainSave;
        private System.Windows.Forms.TextBox txtDomainPrivateKeyFilename;
        private System.Windows.Forms.Label lbPrivateKey;
        private System.Windows.Forms.TextBox txtDomainSelector;
        private System.Windows.Forms.Label lbSelector;
        private System.Windows.Forms.TextBox txtDomainName;
        private System.Windows.Forms.Label lbDomainName;
        private System.Windows.Forms.Button btAddDomain;
        private System.Windows.Forms.ListBox lbxDomains;
        private System.Windows.Forms.TextBox txtDomainDNS;
        private System.Windows.Forms.Button btDomainCheckDNS;
        private System.Windows.Forms.Label lbExistingDns;
        private System.Windows.Forms.Label lbSuggestedDnsRecord;
        private System.Windows.Forms.TextBox txtDNSRecord;
        private System.Windows.Forms.TextBox txtDNSName;
        private System.Windows.Forms.Label lbSuggestedDnsName;
        private System.Windows.Forms.ErrorProvider epvDomainSelector;
        private System.Windows.Forms.Label lbExchangeStatus;
        private System.Windows.Forms.Button btHeaderAdd;
        private System.Windows.Forms.Button btHeaderDelete;
        private System.Windows.Forms.ListBox lbxHeadersToSign;
        private System.Windows.Forms.GroupBox gbDomain;
        private System.Windows.Forms.ComboBox cbKeyLength;
        private System.Windows.Forms.Label lbKeyLength;
        private System.Windows.Forms.Button btConfigureTransportService;
        private System.Windows.Forms.TextBox txtExchangeStatus;
        private System.Windows.Forms.Button btRestartTransportService;
        private System.Windows.Forms.Button btStopTransportService;
        private System.Windows.Forms.Button btStartTransportService;
        private System.Windows.Forms.TabPage tpAbout;
        private System.Windows.Forms.PictureBox picLogo;
        private Configuration.DkimSigner.CustomTextBox txtAbout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tpLog;
        private System.Windows.Forms.Button btEventLogRefresh;
        private System.Windows.Forms.DataGridView dgEventLog;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewImageColumn icon;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewTextBoxColumn message;
        private System.Windows.Forms.Button btUpgrade;
        private System.Windows.Forms.TabPage tpDebug;
        private System.Windows.Forms.Button btExchangeVersion;
        private System.Windows.Forms.GroupBox gbAllowedSigners;
        private System.Windows.Forms.Button btSignersDel;
        private System.Windows.Forms.Button btSignersAdd;
        private System.Windows.Forms.ListBox lbxPermittedSigners;
        private System.Windows.Forms.Label lbPermittedSignersNote;
    }
}

