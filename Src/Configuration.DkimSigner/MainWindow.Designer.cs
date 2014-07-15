namespace Configuration.DkimSigner
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
            this.gbAvailable = new System.Windows.Forms.GroupBox();
            this.btInstallZip = new System.Windows.Forms.Button();
            this.cbxPrereleases = new System.Windows.Forms.CheckBox();
            this.btInstallUpate = new System.Windows.Forms.Button();
            this.lblChangelog = new System.Windows.Forms.Label();
            this.txtChangelog = new System.Windows.Forms.TextBox();
            this.txtDkimSignerAvailable = new System.Windows.Forms.TextBox();
            this.lbDkimSignerAvailable = new System.Windows.Forms.Label();
            this.gbInstalled = new System.Windows.Forms.GroupBox();
            this.lblExchangeStatus = new System.Windows.Forms.Label();
            this.btRestartTransportService = new System.Windows.Forms.Button();
            this.btDisable = new System.Windows.Forms.Button();
            this.btUninstall = new System.Windows.Forms.Button();
            this.lbExchangeInstalled = new System.Windows.Forms.Label();
            this.txtDkimSignerInstalled = new System.Windows.Forms.TextBox();
            this.txtExchangeInstalled = new System.Windows.Forms.TextBox();
            this.lbDkimSignerInstalled = new System.Windows.Forms.Label();
            this.tpDKIM = new System.Windows.Forms.TabPage();
            this.btSave = new System.Windows.Forms.Button();
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
            this.btnAddDomain = new System.Windows.Forms.Button();
            this.btnDomainDelete = new System.Windows.Forms.Button();
            this.gbxDomainDetails = new System.Windows.Forms.GroupBox();
            this.txtDNSName = new System.Windows.Forms.TextBox();
            this.lbSuggestedDnsName = new System.Windows.Forms.Label();
            this.lbExistingDns = new System.Windows.Forms.Label();
            this.lbSuggestedDnsRecord = new System.Windows.Forms.Label();
            this.txtDNSRecord = new System.Windows.Forms.TextBox();
            this.btnDomainCheckDNS = new System.Windows.Forms.Button();
            this.txtDomainDNS = new System.Windows.Forms.TextBox();
            this.btnDomainSave = new System.Windows.Forms.Button();
            this.txtDomainPrivateKeyFilename = new System.Windows.Forms.TextBox();
            this.lbPrivateKey = new System.Windows.Forms.Label();
            this.txtDomainSelector = new System.Windows.Forms.TextBox();
            this.lbSelector = new System.Windows.Forms.Label();
            this.txtDomainName = new System.Windows.Forms.TextBox();
            this.lbDomainName = new System.Windows.Forms.Label();
            this.btnDomainKeySelect = new System.Windows.Forms.Button();
            this.btDomainKeyGenerate = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.timExchangeStatus = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cbKeyLength = new System.Windows.Forms.ComboBox();
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
            this.gbDomain.SuspendLayout();
            this.gbxDomainDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
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
            this.tcConfiguration.ItemSize = new System.Drawing.Size(150, 25);
            this.tcConfiguration.Location = new System.Drawing.Point(12, 12);
            this.tcConfiguration.Name = "tcConfiguration";
            this.tcConfiguration.SelectedIndex = 0;
            this.tcConfiguration.Size = new System.Drawing.Size(715, 317);
            this.tcConfiguration.TabIndex = 0;
            // 
            // tbInformation
            // 
            this.tbInformation.Controls.Add(this.gbAvailable);
            this.tbInformation.Controls.Add(this.gbInstalled);
            this.tbInformation.Location = new System.Drawing.Point(4, 29);
            this.tbInformation.Name = "tbInformation";
            this.tbInformation.Size = new System.Drawing.Size(707, 284);
            this.tbInformation.TabIndex = 2;
            this.tbInformation.Text = "Information";
            this.tbInformation.UseVisualStyleBackColor = true;
            // 
            // gbAvailable
            // 
            this.gbAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAvailable.Controls.Add(this.btInstallZip);
            this.gbAvailable.Controls.Add(this.cbxPrereleases);
            this.gbAvailable.Controls.Add(this.btInstallUpate);
            this.gbAvailable.Controls.Add(this.lblChangelog);
            this.gbAvailable.Controls.Add(this.txtChangelog);
            this.gbAvailable.Controls.Add(this.txtDkimSignerAvailable);
            this.gbAvailable.Controls.Add(this.lbDkimSignerAvailable);
            this.gbAvailable.Location = new System.Drawing.Point(3, 93);
            this.gbAvailable.Name = "gbAvailable";
            this.gbAvailable.Size = new System.Drawing.Size(695, 188);
            this.gbAvailable.TabIndex = 6;
            this.gbAvailable.TabStop = false;
            this.gbAvailable.Text = "Available";
            // 
            // btInstallZip
            // 
            this.btInstallZip.Location = new System.Drawing.Point(446, 48);
            this.btInstallZip.Name = "btInstallZip";
            this.btInstallZip.Size = new System.Drawing.Size(94, 23);
            this.btInstallZip.TabIndex = 13;
            this.btInstallZip.Text = "Install from .zip";
            this.btInstallZip.UseVisualStyleBackColor = true;
            this.btInstallZip.Click += new System.EventHandler(this.btInstallZip_Click);
            // 
            // cbxPrereleases
            // 
            this.cbxPrereleases.AutoSize = true;
            this.cbxPrereleases.Location = new System.Drawing.Point(238, 25);
            this.cbxPrereleases.Name = "cbxPrereleases";
            this.cbxPrereleases.Size = new System.Drawing.Size(155, 17);
            this.cbxPrereleases.TabIndex = 12;
            this.cbxPrereleases.Text = "Include prerelease versions";
            this.cbxPrereleases.UseVisualStyleBackColor = true;
            this.cbxPrereleases.CheckedChanged += new System.EventHandler(this.cbxPrereleases_CheckedChanged);
            // 
            // btInstallUpate
            // 
            this.btInstallUpate.Enabled = false;
            this.btInstallUpate.Location = new System.Drawing.Point(446, 19);
            this.btInstallUpate.Name = "btInstallUpate";
            this.btInstallUpate.Size = new System.Drawing.Size(94, 23);
            this.btInstallUpate.TabIndex = 9;
            this.btInstallUpate.Text = "Update";
            this.btInstallUpate.UseVisualStyleBackColor = true;
            this.btInstallUpate.Click += new System.EventHandler(this.btInstallUpate_Click);
            // 
            // lblChangelog
            // 
            this.lblChangelog.AutoSize = true;
            this.lblChangelog.Location = new System.Drawing.Point(6, 58);
            this.lblChangelog.Name = "lblChangelog";
            this.lblChangelog.Size = new System.Drawing.Size(61, 13);
            this.lblChangelog.TabIndex = 10;
            this.lblChangelog.Text = "Changelog:";
            // 
            // txtChangelog
            // 
            this.txtChangelog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChangelog.Location = new System.Drawing.Point(9, 77);
            this.txtChangelog.Multiline = true;
            this.txtChangelog.Name = "txtChangelog";
            this.txtChangelog.ReadOnly = true;
            this.txtChangelog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtChangelog.Size = new System.Drawing.Size(680, 105);
            this.txtChangelog.TabIndex = 11;
            // 
            // txtDkimSignerAvailable
            // 
            this.txtDkimSignerAvailable.Location = new System.Drawing.Point(82, 21);
            this.txtDkimSignerAvailable.Name = "txtDkimSignerAvailable";
            this.txtDkimSignerAvailable.ReadOnly = true;
            this.txtDkimSignerAvailable.Size = new System.Drawing.Size(138, 20);
            this.txtDkimSignerAvailable.TabIndex = 8;
            this.txtDkimSignerAvailable.Text = "Loading...";
            // 
            // lbDkimSignerAvailable
            // 
            this.lbDkimSignerAvailable.AutoSize = true;
            this.lbDkimSignerAvailable.Location = new System.Drawing.Point(6, 24);
            this.lbDkimSignerAvailable.Name = "lbDkimSignerAvailable";
            this.lbDkimSignerAvailable.Size = new System.Drawing.Size(70, 13);
            this.lbDkimSignerAvailable.TabIndex = 7;
            this.lbDkimSignerAvailable.Text = "Dkim Signer :";
            // 
            // gbInstalled
            // 
            this.gbInstalled.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbInstalled.Controls.Add(this.lblExchangeStatus);
            this.gbInstalled.Controls.Add(this.btRestartTransportService);
            this.gbInstalled.Controls.Add(this.btDisable);
            this.gbInstalled.Controls.Add(this.btUninstall);
            this.gbInstalled.Controls.Add(this.lbExchangeInstalled);
            this.gbInstalled.Controls.Add(this.txtDkimSignerInstalled);
            this.gbInstalled.Controls.Add(this.txtExchangeInstalled);
            this.gbInstalled.Controls.Add(this.lbDkimSignerInstalled);
            this.gbInstalled.Location = new System.Drawing.Point(3, 3);
            this.gbInstalled.Name = "gbInstalled";
            this.gbInstalled.Size = new System.Drawing.Size(695, 84);
            this.gbInstalled.TabIndex = 1;
            this.gbInstalled.TabStop = false;
            this.gbInstalled.Text = "Installed";
            // 
            // lblExchangeStatus
            // 
            this.lblExchangeStatus.AutoSize = true;
            this.lblExchangeStatus.Location = new System.Drawing.Point(226, 53);
            this.lblExchangeStatus.Name = "lblExchangeStatus";
            this.lblExchangeStatus.Size = new System.Drawing.Size(37, 13);
            this.lblExchangeStatus.TabIndex = 15;
            this.lblExchangeStatus.Text = "Status";
            // 
            // btRestartTransportService
            // 
            this.btRestartTransportService.Enabled = false;
            this.btRestartTransportService.Location = new System.Drawing.Point(346, 48);
            this.btRestartTransportService.Name = "btRestartTransportService";
            this.btRestartTransportService.Size = new System.Drawing.Size(194, 23);
            this.btRestartTransportService.TabIndex = 14;
            this.btRestartTransportService.Text = "Restart MSExchangeTransport";
            this.btRestartTransportService.UseVisualStyleBackColor = true;
            this.btRestartTransportService.Click += new System.EventHandler(this.btRestartTransportService_Click);
            // 
            // btDisable
            // 
            this.btDisable.Location = new System.Drawing.Point(346, 19);
            this.btDisable.Name = "btDisable";
            this.btDisable.Size = new System.Drawing.Size(94, 23);
            this.btDisable.TabIndex = 13;
            this.btDisable.Text = "Disable";
            this.btDisable.UseVisualStyleBackColor = true;
            this.btDisable.Click += new System.EventHandler(this.btDisable_Click);
            // 
            // btUninstall
            // 
            this.btUninstall.Location = new System.Drawing.Point(446, 19);
            this.btUninstall.Name = "btUninstall";
            this.btUninstall.Size = new System.Drawing.Size(94, 23);
            this.btUninstall.TabIndex = 10;
            this.btUninstall.Text = "Uninstall";
            this.btUninstall.UseVisualStyleBackColor = true;
            this.btUninstall.Click += new System.EventHandler(this.btUninstall_Click);
            // 
            // lbExchangeInstalled
            // 
            this.lbExchangeInstalled.AutoSize = true;
            this.lbExchangeInstalled.Location = new System.Drawing.Point(6, 53);
            this.lbExchangeInstalled.Name = "lbExchangeInstalled";
            this.lbExchangeInstalled.Size = new System.Drawing.Size(61, 13);
            this.lbExchangeInstalled.TabIndex = 4;
            this.lbExchangeInstalled.Text = "Exchange :";
            // 
            // txtDkimSignerInstalled
            // 
            this.txtDkimSignerInstalled.Location = new System.Drawing.Point(82, 21);
            this.txtDkimSignerInstalled.Name = "txtDkimSignerInstalled";
            this.txtDkimSignerInstalled.ReadOnly = true;
            this.txtDkimSignerInstalled.Size = new System.Drawing.Size(138, 20);
            this.txtDkimSignerInstalled.TabIndex = 3;
            this.txtDkimSignerInstalled.Text = "Loading...";
            // 
            // txtExchangeInstalled
            // 
            this.txtExchangeInstalled.Location = new System.Drawing.Point(82, 50);
            this.txtExchangeInstalled.Name = "txtExchangeInstalled";
            this.txtExchangeInstalled.ReadOnly = true;
            this.txtExchangeInstalled.Size = new System.Drawing.Size(138, 20);
            this.txtExchangeInstalled.TabIndex = 5;
            this.txtExchangeInstalled.Text = "Loading...";
            // 
            // lbDkimSignerInstalled
            // 
            this.lbDkimSignerInstalled.AutoSize = true;
            this.lbDkimSignerInstalled.Location = new System.Drawing.Point(6, 24);
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
            this.tpDKIM.Size = new System.Drawing.Size(707, 284);
            this.tpDKIM.TabIndex = 0;
            this.tpDKIM.Text = "DKIM";
            this.tpDKIM.UseVisualStyleBackColor = true;
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(393, 220);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(125, 23);
            this.btSave.TabIndex = 14;
            this.btSave.Text = "Save configuration";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
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
            this.gbHeaderToSign.Size = new System.Drawing.Size(216, 272);
            this.gbHeaderToSign.TabIndex = 1;
            this.gbHeaderToSign.TabStop = false;
            this.gbHeaderToSign.Text = "Header to sign";
            // 
            // btHeaderAdd
            // 
            this.btHeaderAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btHeaderAdd.Location = new System.Drawing.Point(39, 243);
            this.btHeaderAdd.Name = "btHeaderAdd";
            this.btHeaderAdd.Size = new System.Drawing.Size(65, 23);
            this.btHeaderAdd.TabIndex = 2;
            this.btHeaderAdd.Text = "Add";
            this.btHeaderAdd.UseVisualStyleBackColor = true;
            this.btHeaderAdd.Click += new System.EventHandler(this.btHeaderAdd_Click);
            // 
            // btHeaderDelete
            // 
            this.btHeaderDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btHeaderDelete.Location = new System.Drawing.Point(110, 243);
            this.btHeaderDelete.Name = "btHeaderDelete";
            this.btHeaderDelete.Size = new System.Drawing.Size(65, 23);
            this.btHeaderDelete.TabIndex = 1;
            this.btHeaderDelete.Text = "Delete";
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
            this.lbxHeadersToSign.Size = new System.Drawing.Size(202, 218);
            this.lbxHeadersToSign.TabIndex = 0;
            this.lbxHeadersToSign.SelectedIndexChanged += new System.EventHandler(this.lbxHeadersToSign_SelectedIndexChanged);
            // 
            // gbLogLevel
            // 
            this.gbLogLevel.Controls.Add(this.cbLogLevel);
            this.gbLogLevel.Location = new System.Drawing.Point(243, 64);
            this.gbLogLevel.Name = "gbLogLevel";
            this.gbLogLevel.Size = new System.Drawing.Size(203, 52);
            this.gbLogLevel.TabIndex = 6;
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
            this.cbLogLevel.TabIndex = 7;
            this.cbLogLevel.TextChanged += new System.EventHandler(this.cbLogLevel_TextChanged);
            // 
            // gbBodyCanonicalization
            // 
            this.gbBodyCanonicalization.Controls.Add(this.rbRelaxedBodyCanonicalization);
            this.gbBodyCanonicalization.Controls.Add(this.rbSimpleBodyCanonicalization);
            this.gbBodyCanonicalization.Location = new System.Drawing.Point(452, 64);
            this.gbBodyCanonicalization.Name = "gbBodyCanonicalization";
            this.gbBodyCanonicalization.Size = new System.Drawing.Size(217, 52);
            this.gbBodyCanonicalization.TabIndex = 11;
            this.gbBodyCanonicalization.TabStop = false;
            this.gbBodyCanonicalization.Text = "Body Canonicalization";
            // 
            // rbRelaxedBodyCanonicalization
            // 
            this.rbRelaxedBodyCanonicalization.AutoSize = true;
            this.rbRelaxedBodyCanonicalization.Location = new System.Drawing.Point(105, 20);
            this.rbRelaxedBodyCanonicalization.Name = "rbRelaxedBodyCanonicalization";
            this.rbRelaxedBodyCanonicalization.Size = new System.Drawing.Size(64, 17);
            this.rbRelaxedBodyCanonicalization.TabIndex = 13;
            this.rbRelaxedBodyCanonicalization.Text = "Relaxed";
            this.rbRelaxedBodyCanonicalization.UseVisualStyleBackColor = true;
            this.rbRelaxedBodyCanonicalization.CheckedChanged += new System.EventHandler(this.rbRelaxedBodyCanonicalization_CheckedChanged);
            // 
            // rbSimpleBodyCanonicalization
            // 
            this.rbSimpleBodyCanonicalization.AutoSize = true;
            this.rbSimpleBodyCanonicalization.Checked = true;
            this.rbSimpleBodyCanonicalization.Location = new System.Drawing.Point(21, 20);
            this.rbSimpleBodyCanonicalization.Name = "rbSimpleBodyCanonicalization";
            this.rbSimpleBodyCanonicalization.Size = new System.Drawing.Size(56, 17);
            this.rbSimpleBodyCanonicalization.TabIndex = 12;
            this.rbSimpleBodyCanonicalization.TabStop = true;
            this.rbSimpleBodyCanonicalization.Text = "Simple";
            this.rbSimpleBodyCanonicalization.UseVisualStyleBackColor = true;
            this.rbSimpleBodyCanonicalization.CheckedChanged += new System.EventHandler(this.rbSimpleBodyCanonicalization_CheckedChanged);
            // 
            // gbHeaderCanonicalization
            // 
            this.gbHeaderCanonicalization.Controls.Add(this.rbRelaxedHeaderCanonicalization);
            this.gbHeaderCanonicalization.Controls.Add(this.rbSimpleHeaderCanonicalization);
            this.gbHeaderCanonicalization.Location = new System.Drawing.Point(452, 6);
            this.gbHeaderCanonicalization.Name = "gbHeaderCanonicalization";
            this.gbHeaderCanonicalization.Size = new System.Drawing.Size(217, 52);
            this.gbHeaderCanonicalization.TabIndex = 8;
            this.gbHeaderCanonicalization.TabStop = false;
            this.gbHeaderCanonicalization.Text = "Header Canonicalization";
            // 
            // rbRelaxedHeaderCanonicalization
            // 
            this.rbRelaxedHeaderCanonicalization.AutoSize = true;
            this.rbRelaxedHeaderCanonicalization.Location = new System.Drawing.Point(105, 20);
            this.rbRelaxedHeaderCanonicalization.Name = "rbRelaxedHeaderCanonicalization";
            this.rbRelaxedHeaderCanonicalization.Size = new System.Drawing.Size(64, 17);
            this.rbRelaxedHeaderCanonicalization.TabIndex = 10;
            this.rbRelaxedHeaderCanonicalization.Text = "Relaxed";
            this.rbRelaxedHeaderCanonicalization.UseVisualStyleBackColor = true;
            this.rbRelaxedHeaderCanonicalization.CheckedChanged += new System.EventHandler(this.rbRelaxedHeaderCanonicalization_CheckedChanged);
            // 
            // rbSimpleHeaderCanonicalization
            // 
            this.rbSimpleHeaderCanonicalization.AutoSize = true;
            this.rbSimpleHeaderCanonicalization.Checked = true;
            this.rbSimpleHeaderCanonicalization.Location = new System.Drawing.Point(21, 20);
            this.rbSimpleHeaderCanonicalization.Name = "rbSimpleHeaderCanonicalization";
            this.rbSimpleHeaderCanonicalization.Size = new System.Drawing.Size(56, 17);
            this.rbSimpleHeaderCanonicalization.TabIndex = 9;
            this.rbSimpleHeaderCanonicalization.TabStop = true;
            this.rbSimpleHeaderCanonicalization.Text = "Simple";
            this.rbSimpleHeaderCanonicalization.UseVisualStyleBackColor = true;
            this.rbSimpleHeaderCanonicalization.CheckedChanged += new System.EventHandler(this.rbSimpleHeaderCanonicalization_CheckedChanged);
            // 
            // gbAlgorithm
            // 
            this.gbAlgorithm.Controls.Add(this.rbRsaSha256);
            this.gbAlgorithm.Controls.Add(this.rbRsaSha1);
            this.gbAlgorithm.Location = new System.Drawing.Point(243, 6);
            this.gbAlgorithm.Name = "gbAlgorithm";
            this.gbAlgorithm.Size = new System.Drawing.Size(203, 52);
            this.gbAlgorithm.TabIndex = 3;
            this.gbAlgorithm.TabStop = false;
            this.gbAlgorithm.Text = "Algorithm";
            // 
            // rbRsaSha256
            // 
            this.rbRsaSha256.AutoSize = true;
            this.rbRsaSha256.Location = new System.Drawing.Point(105, 20);
            this.rbRsaSha256.Name = "rbRsaSha256";
            this.rbRsaSha256.Size = new System.Drawing.Size(81, 17);
            this.rbRsaSha256.TabIndex = 5;
            this.rbRsaSha256.Text = "RsaSha256";
            this.rbRsaSha256.UseVisualStyleBackColor = true;
            this.rbRsaSha256.CheckedChanged += new System.EventHandler(this.rbRsaSha256_CheckedChanged);
            // 
            // rbRsaSha1
            // 
            this.rbRsaSha1.AutoSize = true;
            this.rbRsaSha1.Checked = true;
            this.rbRsaSha1.Location = new System.Drawing.Point(21, 20);
            this.rbRsaSha1.Name = "rbRsaSha1";
            this.rbRsaSha1.Size = new System.Drawing.Size(69, 17);
            this.rbRsaSha1.TabIndex = 4;
            this.rbRsaSha1.TabStop = true;
            this.rbRsaSha1.Text = "RsaSha1";
            this.rbRsaSha1.UseVisualStyleBackColor = true;
            this.rbRsaSha1.CheckedChanged += new System.EventHandler(this.rbRsaSha1_CheckedChanged);
            // 
            // tpDomain
            // 
            this.tpDomain.Controls.Add(this.gbDomain);
            this.tpDomain.Controls.Add(this.gbxDomainDetails);
            this.tpDomain.Location = new System.Drawing.Point(4, 29);
            this.tpDomain.Name = "tpDomain";
            this.tpDomain.Padding = new System.Windows.Forms.Padding(3);
            this.tpDomain.Size = new System.Drawing.Size(707, 284);
            this.tpDomain.TabIndex = 1;
            this.tpDomain.Text = "Domains";
            this.tpDomain.UseVisualStyleBackColor = true;
            // 
            // gbDomain
            // 
            this.gbDomain.Controls.Add(this.lbxDomains);
            this.gbDomain.Controls.Add(this.btnAddDomain);
            this.gbDomain.Controls.Add(this.btnDomainDelete);
            this.gbDomain.Location = new System.Drawing.Point(6, 6);
            this.gbDomain.Name = "gbDomain";
            this.gbDomain.Size = new System.Drawing.Size(151, 266);
            this.gbDomain.TabIndex = 20;
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
            this.lbxDomains.Size = new System.Drawing.Size(139, 212);
            this.lbxDomains.TabIndex = 5;
            this.lbxDomains.SelectedIndexChanged += new System.EventHandler(this.lbxDomains_SelectedIndexChanged);
            // 
            // btnAddDomain
            // 
            this.btnAddDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddDomain.Location = new System.Drawing.Point(6, 237);
            this.btnAddDomain.Name = "btnAddDomain";
            this.btnAddDomain.Size = new System.Drawing.Size(65, 23);
            this.btnAddDomain.TabIndex = 6;
            this.btnAddDomain.Text = "Add";
            this.btnAddDomain.UseVisualStyleBackColor = true;
            this.btnAddDomain.Click += new System.EventHandler(this.btAddDomain_Click);
            // 
            // btnDomainDelete
            // 
            this.btnDomainDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDomainDelete.Location = new System.Drawing.Point(80, 237);
            this.btnDomainDelete.Name = "btnDomainDelete";
            this.btnDomainDelete.Size = new System.Drawing.Size(65, 23);
            this.btnDomainDelete.TabIndex = 12;
            this.btnDomainDelete.Text = "Delete";
            this.btnDomainDelete.UseVisualStyleBackColor = true;
            this.btnDomainDelete.Click += new System.EventHandler(this.btDomainDelete_Click);
            // 
            // gbxDomainDetails
            // 
            this.gbxDomainDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDomainDetails.Controls.Add(this.cbKeyLength);
            this.gbxDomainDetails.Controls.Add(this.label1);
            this.gbxDomainDetails.Controls.Add(this.txtDNSName);
            this.gbxDomainDetails.Controls.Add(this.lbSuggestedDnsName);
            this.gbxDomainDetails.Controls.Add(this.lbExistingDns);
            this.gbxDomainDetails.Controls.Add(this.lbSuggestedDnsRecord);
            this.gbxDomainDetails.Controls.Add(this.txtDNSRecord);
            this.gbxDomainDetails.Controls.Add(this.btnDomainCheckDNS);
            this.gbxDomainDetails.Controls.Add(this.txtDomainDNS);
            this.gbxDomainDetails.Controls.Add(this.btnDomainSave);
            this.gbxDomainDetails.Controls.Add(this.txtDomainPrivateKeyFilename);
            this.gbxDomainDetails.Controls.Add(this.lbPrivateKey);
            this.gbxDomainDetails.Controls.Add(this.txtDomainSelector);
            this.gbxDomainDetails.Controls.Add(this.lbSelector);
            this.gbxDomainDetails.Controls.Add(this.txtDomainName);
            this.gbxDomainDetails.Controls.Add(this.lbDomainName);
            this.gbxDomainDetails.Controls.Add(this.btnDomainKeySelect);
            this.gbxDomainDetails.Controls.Add(this.btDomainKeyGenerate);
            this.gbxDomainDetails.Enabled = false;
            this.gbxDomainDetails.Location = new System.Drawing.Point(163, 9);
            this.gbxDomainDetails.Name = "gbxDomainDetails";
            this.gbxDomainDetails.Size = new System.Drawing.Size(538, 272);
            this.gbxDomainDetails.TabIndex = 7;
            this.gbxDomainDetails.TabStop = false;
            this.gbxDomainDetails.Text = "Domain details";
            // 
            // txtDNSName
            // 
            this.txtDNSName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDNSName.Location = new System.Drawing.Point(137, 97);
            this.txtDNSName.Name = "txtDNSName";
            this.txtDNSName.ReadOnly = true;
            this.txtDNSName.Size = new System.Drawing.Size(385, 20);
            this.txtDNSName.TabIndex = 19;
            // 
            // lbSuggestedDnsName
            // 
            this.lbSuggestedDnsName.AutoSize = true;
            this.lbSuggestedDnsName.Location = new System.Drawing.Point(6, 100);
            this.lbSuggestedDnsName.Name = "lbSuggestedDnsName";
            this.lbSuggestedDnsName.Size = new System.Drawing.Size(121, 13);
            this.lbSuggestedDnsName.TabIndex = 18;
            this.lbSuggestedDnsName.Text = "Suggested DNS Name :";
            // 
            // lbExistingDns
            // 
            this.lbExistingDns.AutoSize = true;
            this.lbExistingDns.Location = new System.Drawing.Point(6, 184);
            this.lbExistingDns.Name = "lbExistingDns";
            this.lbExistingDns.Size = new System.Drawing.Size(75, 13);
            this.lbExistingDns.TabIndex = 17;
            this.lbExistingDns.Text = "Existing DNS :";
            // 
            // lbSuggestedDnsRecord
            // 
            this.lbSuggestedDnsRecord.AutoSize = true;
            this.lbSuggestedDnsRecord.Location = new System.Drawing.Point(6, 126);
            this.lbSuggestedDnsRecord.Name = "lbSuggestedDnsRecord";
            this.lbSuggestedDnsRecord.Size = new System.Drawing.Size(128, 13);
            this.lbSuggestedDnsRecord.TabIndex = 16;
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
            this.txtDNSRecord.Size = new System.Drawing.Size(385, 52);
            this.txtDNSRecord.TabIndex = 15;
            // 
            // btnDomainCheckDNS
            // 
            this.btnDomainCheckDNS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDomainCheckDNS.Location = new System.Drawing.Point(9, 210);
            this.btnDomainCheckDNS.Name = "btnDomainCheckDNS";
            this.btnDomainCheckDNS.Size = new System.Drawing.Size(69, 23);
            this.btnDomainCheckDNS.TabIndex = 14;
            this.btnDomainCheckDNS.Text = "Check";
            this.btnDomainCheckDNS.UseVisualStyleBackColor = true;
            this.btnDomainCheckDNS.Click += new System.EventHandler(this.btDomainCheckDNS_Click);
            // 
            // txtDomainDNS
            // 
            this.txtDomainDNS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomainDNS.Location = new System.Drawing.Point(137, 181);
            this.txtDomainDNS.Multiline = true;
            this.txtDomainDNS.Name = "txtDomainDNS";
            this.txtDomainDNS.ReadOnly = true;
            this.txtDomainDNS.Size = new System.Drawing.Size(385, 52);
            this.txtDomainDNS.TabIndex = 13;
            // 
            // btnDomainSave
            // 
            this.btnDomainSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDomainSave.Location = new System.Drawing.Point(447, 240);
            this.btnDomainSave.Name = "btnDomainSave";
            this.btnDomainSave.Size = new System.Drawing.Size(75, 23);
            this.btnDomainSave.TabIndex = 11;
            this.btnDomainSave.Text = "Save";
            this.btnDomainSave.UseVisualStyleBackColor = true;
            this.btnDomainSave.Click += new System.EventHandler(this.btDomainSave_Click);
            // 
            // txtDomainPrivateKeyFilename
            // 
            this.txtDomainPrivateKeyFilename.Location = new System.Drawing.Point(137, 48);
            this.txtDomainPrivateKeyFilename.Name = "txtDomainPrivateKeyFilename";
            this.txtDomainPrivateKeyFilename.ReadOnly = true;
            this.txtDomainPrivateKeyFilename.Size = new System.Drawing.Size(385, 20);
            this.txtDomainPrivateKeyFilename.TabIndex = 10;
            // 
            // lbPrivateKey
            // 
            this.lbPrivateKey.AutoSize = true;
            this.lbPrivateKey.Location = new System.Drawing.Point(6, 48);
            this.lbPrivateKey.Name = "lbPrivateKey";
            this.lbPrivateKey.Size = new System.Drawing.Size(108, 13);
            this.lbPrivateKey.TabIndex = 9;
            this.lbPrivateKey.Text = "Private key filename :";
            // 
            // txtDomainSelector
            // 
            this.txtDomainSelector.Location = new System.Drawing.Point(377, 15);
            this.txtDomainSelector.Name = "txtDomainSelector";
            this.txtDomainSelector.Size = new System.Drawing.Size(145, 20);
            this.txtDomainSelector.TabIndex = 8;
            this.txtDomainSelector.TextChanged += new System.EventHandler(this.txtDomainSelector_TextChanged);
            // 
            // lbSelector
            // 
            this.lbSelector.AutoSize = true;
            this.lbSelector.Location = new System.Drawing.Point(313, 18);
            this.lbSelector.Name = "lbSelector";
            this.lbSelector.Size = new System.Drawing.Size(49, 13);
            this.lbSelector.TabIndex = 7;
            this.lbSelector.Text = "Selector:";
            // 
            // txtDomainName
            // 
            this.txtDomainName.Location = new System.Drawing.Point(137, 19);
            this.txtDomainName.Name = "txtDomainName";
            this.txtDomainName.Size = new System.Drawing.Size(145, 20);
            this.txtDomainName.TabIndex = 6;
            this.txtDomainName.TextChanged += new System.EventHandler(this.txtDomainName_TextChanged);
            // 
            // lbDomainName
            // 
            this.lbDomainName.AutoSize = true;
            this.lbDomainName.Location = new System.Drawing.Point(6, 22);
            this.lbDomainName.Name = "lbDomainName";
            this.lbDomainName.Size = new System.Drawing.Size(78, 13);
            this.lbDomainName.TabIndex = 5;
            this.lbDomainName.Text = "Domain name :";
            // 
            // btnDomainKeySelect
            // 
            this.btnDomainKeySelect.AutoSize = true;
            this.btnDomainKeySelect.Location = new System.Drawing.Point(418, 71);
            this.btnDomainKeySelect.Name = "btnDomainKeySelect";
            this.btnDomainKeySelect.Size = new System.Drawing.Size(104, 23);
            this.btnDomainKeySelect.TabIndex = 3;
            this.btnDomainKeySelect.Text = "Select key file";
            this.btnDomainKeySelect.UseVisualStyleBackColor = true;
            this.btnDomainKeySelect.Click += new System.EventHandler(this.btDomainKeySelect_Click);
            // 
            // btDomainKeyGenerate
            // 
            this.btDomainKeyGenerate.AutoSize = true;
            this.btDomainKeyGenerate.Location = new System.Drawing.Point(308, 71);
            this.btDomainKeyGenerate.Name = "btDomainKeyGenerate";
            this.btDomainKeyGenerate.Size = new System.Drawing.Size(104, 23);
            this.btDomainKeyGenerate.TabIndex = 2;
            this.btDomainKeyGenerate.Text = "Generate new key";
            this.btDomainKeyGenerate.UseVisualStyleBackColor = true;
            this.btDomainKeyGenerate.Click += new System.EventHandler(this.btDomainKeyGenerate_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // timExchangeStatus
            // 
            this.timExchangeStatus.Enabled = true;
            this.timExchangeStatus.Interval = 200;
            this.timExchangeStatus.Tick += new System.EventHandler(this.timExchangeStatus_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Key length for generation :";
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
            this.cbKeyLength.TabIndex = 21;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 342);
            this.Controls.Add(this.tcConfiguration);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(730, 380);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exchange DkimSigner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.tcConfiguration.ResumeLayout(false);
            this.tbInformation.ResumeLayout(false);
            this.gbAvailable.ResumeLayout(false);
            this.gbAvailable.PerformLayout();
            this.gbInstalled.ResumeLayout(false);
            this.gbInstalled.PerformLayout();
            this.tpDKIM.ResumeLayout(false);
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
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
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
        private System.Windows.Forms.Button btnDomainKeySelect;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Label lblChangelog;
        private System.Windows.Forms.TextBox txtChangelog;
        private System.Windows.Forms.Button btInstallUpate;
        private System.Windows.Forms.Button btDomainKeyGenerate;
        private System.Windows.Forms.Button btUninstall;
        private System.Windows.Forms.CheckBox cbxPrereleases;
        private System.Windows.Forms.Button btInstallZip;
        private System.Windows.Forms.Button btDisable;
        private System.Windows.Forms.GroupBox gbxDomainDetails;
        private System.Windows.Forms.Button btnDomainDelete;
        private System.Windows.Forms.Button btnDomainSave;
        private System.Windows.Forms.TextBox txtDomainPrivateKeyFilename;
        private System.Windows.Forms.Label lbPrivateKey;
        private System.Windows.Forms.TextBox txtDomainSelector;
        private System.Windows.Forms.Label lbSelector;
        private System.Windows.Forms.TextBox txtDomainName;
        private System.Windows.Forms.Label lbDomainName;
        private System.Windows.Forms.Button btnAddDomain;
        private System.Windows.Forms.ListBox lbxDomains;
        private System.Windows.Forms.TextBox txtDomainDNS;
        private System.Windows.Forms.Button btnDomainCheckDNS;
        private System.Windows.Forms.Label lbExistingDns;
        private System.Windows.Forms.Label lbSuggestedDnsRecord;
        private System.Windows.Forms.TextBox txtDNSRecord;
        private System.Windows.Forms.TextBox txtDNSName;
        private System.Windows.Forms.Label lbSuggestedDnsName;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button btRestartTransportService;
        private System.Windows.Forms.Timer timExchangeStatus;
        private System.Windows.Forms.Label lblExchangeStatus;
        private System.Windows.Forms.Button btHeaderAdd;
        private System.Windows.Forms.Button btHeaderDelete;
        private System.Windows.Forms.ListBox lbxHeadersToSign;
        private System.Windows.Forms.GroupBox gbDomain;
        private System.Windows.Forms.ComboBox cbKeyLength;
        private System.Windows.Forms.Label label1;
    }
}

