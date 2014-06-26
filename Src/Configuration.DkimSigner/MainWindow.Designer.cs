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
            this.btnInstallZip = new System.Windows.Forms.Button();
            this.cbxPrereleases = new System.Windows.Forms.CheckBox();
            this.btUpateInstall = new System.Windows.Forms.Button();
            this.lblChangelog = new System.Windows.Forms.Label();
            this.txtChangelog = new System.Windows.Forms.TextBox();
            this.txtDkimSignerAvailable = new System.Windows.Forms.TextBox();
            this.lbDkimSignerAvailable = new System.Windows.Forms.Label();
            this.gbInstalled = new System.Windows.Forms.GroupBox();
            this.btnDisable = new System.Windows.Forms.Button();
            this.btUninstall = new System.Windows.Forms.Button();
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
            this.gbxDomainDetails = new System.Windows.Forms.GroupBox();
            this.btnDomainCheckDNS = new System.Windows.Forms.Button();
            this.tbxDomainDNS = new System.Windows.Forms.TextBox();
            this.btnDomainDelete = new System.Windows.Forms.Button();
            this.btnDomainSave = new System.Windows.Forms.Button();
            this.tbxDomainPrivateKeyFilename = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxDomainSelector = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxDomainName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDomainKeySelect = new System.Windows.Forms.Button();
            this.btDomainKeyGenerate = new System.Windows.Forms.Button();
            this.btnAddDomain = new System.Windows.Forms.Button();
            this.lbxDomains = new System.Windows.Forms.ListBox();
            this.openZipFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveKeyFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openKeyFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tbxDNSRecord = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxDNSName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
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
            this.tcConfiguration.Size = new System.Drawing.Size(773, 317);
            this.tcConfiguration.TabIndex = 0;
            // 
            // tbInformation
            // 
            this.tbInformation.Controls.Add(this.gbAvailable);
            this.tbInformation.Controls.Add(this.gbInstalled);
            this.tbInformation.Location = new System.Drawing.Point(4, 29);
            this.tbInformation.Name = "tbInformation";
            this.tbInformation.Size = new System.Drawing.Size(765, 284);
            this.tbInformation.TabIndex = 2;
            this.tbInformation.Text = "Information";
            this.tbInformation.UseVisualStyleBackColor = true;
            // 
            // gbAvailable
            // 
            this.gbAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbAvailable.Controls.Add(this.btnInstallZip);
            this.gbAvailable.Controls.Add(this.cbxPrereleases);
            this.gbAvailable.Controls.Add(this.btUpateInstall);
            this.gbAvailable.Controls.Add(this.lblChangelog);
            this.gbAvailable.Controls.Add(this.txtChangelog);
            this.gbAvailable.Controls.Add(this.txtDkimSignerAvailable);
            this.gbAvailable.Controls.Add(this.lbDkimSignerAvailable);
            this.gbAvailable.Location = new System.Drawing.Point(3, 93);
            this.gbAvailable.Name = "gbAvailable";
            this.gbAvailable.Size = new System.Drawing.Size(759, 188);
            this.gbAvailable.TabIndex = 6;
            this.gbAvailable.TabStop = false;
            this.gbAvailable.Text = "Available";
            // 
            // btnInstallZip
            // 
            this.btnInstallZip.Location = new System.Drawing.Point(446, 48);
            this.btnInstallZip.Name = "btnInstallZip";
            this.btnInstallZip.Size = new System.Drawing.Size(94, 23);
            this.btnInstallZip.TabIndex = 13;
            this.btnInstallZip.Text = "Install from .zip";
            this.btnInstallZip.UseVisualStyleBackColor = true;
            this.btnInstallZip.Click += new System.EventHandler(this.btnInstallZip_Click);
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
            // btUpateInstall
            // 
            this.btUpateInstall.Enabled = false;
            this.btUpateInstall.Location = new System.Drawing.Point(446, 19);
            this.btUpateInstall.Name = "btUpateInstall";
            this.btUpateInstall.Size = new System.Drawing.Size(94, 23);
            this.btUpateInstall.TabIndex = 9;
            this.btUpateInstall.Text = "Update";
            this.btUpateInstall.UseVisualStyleBackColor = true;
            this.btUpateInstall.Click += new System.EventHandler(this.btUpateInstall_Click);
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
            this.txtChangelog.Size = new System.Drawing.Size(744, 105);
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
            this.gbInstalled.Controls.Add(this.btnDisable);
            this.gbInstalled.Controls.Add(this.btUninstall);
            this.gbInstalled.Controls.Add(this.lbExchangeInstalled);
            this.gbInstalled.Controls.Add(this.txtDkimSignerInstalled);
            this.gbInstalled.Controls.Add(this.txtExchangeInstalled);
            this.gbInstalled.Controls.Add(this.lbDkimSignerInstalled);
            this.gbInstalled.Location = new System.Drawing.Point(3, 3);
            this.gbInstalled.Name = "gbInstalled";
            this.gbInstalled.Size = new System.Drawing.Size(759, 84);
            this.gbInstalled.TabIndex = 1;
            this.gbInstalled.TabStop = false;
            this.gbInstalled.Text = "Installed";
            // 
            // btnDisable
            // 
            this.btnDisable.Location = new System.Drawing.Point(226, 26);
            this.btnDisable.Name = "btnDisable";
            this.btnDisable.Size = new System.Drawing.Size(58, 23);
            this.btnDisable.TabIndex = 13;
            this.btnDisable.Text = "Disable";
            this.btnDisable.UseVisualStyleBackColor = true;
            this.btnDisable.Click += new System.EventHandler(this.btnDisable_Click);
            // 
            // btUninstall
            // 
            this.btUninstall.Location = new System.Drawing.Point(446, 26);
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
            this.lbExchangeInstalled.Location = new System.Drawing.Point(6, 55);
            this.lbExchangeInstalled.Name = "lbExchangeInstalled";
            this.lbExchangeInstalled.Size = new System.Drawing.Size(61, 13);
            this.lbExchangeInstalled.TabIndex = 4;
            this.lbExchangeInstalled.Text = "Exchange :";
            // 
            // txtDkimSignerInstalled
            // 
            this.txtDkimSignerInstalled.Location = new System.Drawing.Point(82, 26);
            this.txtDkimSignerInstalled.Name = "txtDkimSignerInstalled";
            this.txtDkimSignerInstalled.ReadOnly = true;
            this.txtDkimSignerInstalled.Size = new System.Drawing.Size(138, 20);
            this.txtDkimSignerInstalled.TabIndex = 3;
            this.txtDkimSignerInstalled.Text = "Loading...";
            // 
            // txtExchangeInstalled
            // 
            this.txtExchangeInstalled.Location = new System.Drawing.Point(82, 52);
            this.txtExchangeInstalled.Name = "txtExchangeInstalled";
            this.txtExchangeInstalled.ReadOnly = true;
            this.txtExchangeInstalled.Size = new System.Drawing.Size(138, 20);
            this.txtExchangeInstalled.TabIndex = 5;
            this.txtExchangeInstalled.Text = "Loading...";
            // 
            // lbDkimSignerInstalled
            // 
            this.lbDkimSignerInstalled.AutoSize = true;
            this.lbDkimSignerInstalled.Location = new System.Drawing.Point(6, 29);
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
            this.tpDKIM.Size = new System.Drawing.Size(765, 284);
            this.tpDKIM.TabIndex = 0;
            this.tpDKIM.Text = "DKIM";
            this.tpDKIM.UseVisualStyleBackColor = true;
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(307, 180);
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
            this.gbHeaderToSign.Location = new System.Drawing.Point(6, 6);
            this.gbHeaderToSign.Name = "gbHeaderToSign";
            this.gbHeaderToSign.Size = new System.Drawing.Size(426, 52);
            this.gbHeaderToSign.TabIndex = 1;
            this.gbHeaderToSign.TabStop = false;
            this.gbHeaderToSign.Text = "Header to sign";
            // 
            // txtHeaderToSign
            // 
            this.txtHeaderToSign.Location = new System.Drawing.Point(6, 22);
            this.txtHeaderToSign.Name = "txtHeaderToSign";
            this.txtHeaderToSign.Size = new System.Drawing.Size(414, 20);
            this.txtHeaderToSign.TabIndex = 2;
            this.txtHeaderToSign.Text = "From; Subject; To; Date; Message-ID;";
            this.txtHeaderToSign.TextChanged += new System.EventHandler(this.txtHeaderToSign_TextChanged);
            // 
            // gbLogLevel
            // 
            this.gbLogLevel.Controls.Add(this.cbLogLevel);
            this.gbLogLevel.Location = new System.Drawing.Point(6, 122);
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
            this.gbBodyCanonicalization.Location = new System.Drawing.Point(215, 122);
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
            this.gbHeaderCanonicalization.Location = new System.Drawing.Point(215, 64);
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
            this.gbAlgorithm.Location = new System.Drawing.Point(6, 64);
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
            this.tpDomain.Controls.Add(this.gbxDomainDetails);
            this.tpDomain.Controls.Add(this.btnAddDomain);
            this.tpDomain.Controls.Add(this.lbxDomains);
            this.tpDomain.Location = new System.Drawing.Point(4, 29);
            this.tpDomain.Name = "tpDomain";
            this.tpDomain.Padding = new System.Windows.Forms.Padding(3);
            this.tpDomain.Size = new System.Drawing.Size(765, 284);
            this.tpDomain.TabIndex = 1;
            this.tpDomain.Text = "Domains";
            this.tpDomain.UseVisualStyleBackColor = true;
            // 
            // gbxDomainDetails
            // 
            this.gbxDomainDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxDomainDetails.Controls.Add(this.tbxDNSName);
            this.gbxDomainDetails.Controls.Add(this.label6);
            this.gbxDomainDetails.Controls.Add(this.label5);
            this.gbxDomainDetails.Controls.Add(this.label4);
            this.gbxDomainDetails.Controls.Add(this.tbxDNSRecord);
            this.gbxDomainDetails.Controls.Add(this.btnDomainCheckDNS);
            this.gbxDomainDetails.Controls.Add(this.tbxDomainDNS);
            this.gbxDomainDetails.Controls.Add(this.btnDomainDelete);
            this.gbxDomainDetails.Controls.Add(this.btnDomainSave);
            this.gbxDomainDetails.Controls.Add(this.tbxDomainPrivateKeyFilename);
            this.gbxDomainDetails.Controls.Add(this.label3);
            this.gbxDomainDetails.Controls.Add(this.tbxDomainSelector);
            this.gbxDomainDetails.Controls.Add(this.label2);
            this.gbxDomainDetails.Controls.Add(this.tbxDomainName);
            this.gbxDomainDetails.Controls.Add(this.label1);
            this.gbxDomainDetails.Controls.Add(this.btnDomainKeySelect);
            this.gbxDomainDetails.Controls.Add(this.btDomainKeyGenerate);
            this.gbxDomainDetails.Enabled = false;
            this.gbxDomainDetails.Location = new System.Drawing.Point(163, 6);
            this.gbxDomainDetails.Name = "gbxDomainDetails";
            this.gbxDomainDetails.Size = new System.Drawing.Size(596, 272);
            this.gbxDomainDetails.TabIndex = 7;
            this.gbxDomainDetails.TabStop = false;
            this.gbxDomainDetails.Text = "Domain details";
            // 
            // btnDomainCheckDNS
            // 
            this.btnDomainCheckDNS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDomainCheckDNS.Location = new System.Drawing.Point(515, 181);
            this.btnDomainCheckDNS.Name = "btnDomainCheckDNS";
            this.btnDomainCheckDNS.Size = new System.Drawing.Size(75, 52);
            this.btnDomainCheckDNS.TabIndex = 14;
            this.btnDomainCheckDNS.Text = "Check DNS";
            this.btnDomainCheckDNS.UseVisualStyleBackColor = true;
            this.btnDomainCheckDNS.Click += new System.EventHandler(this.btnDomainCheckDNS_Click);
            // 
            // tbxDomainDNS
            // 
            this.tbxDomainDNS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxDomainDNS.Location = new System.Drawing.Point(137, 181);
            this.tbxDomainDNS.Multiline = true;
            this.tbxDomainDNS.Name = "tbxDomainDNS";
            this.tbxDomainDNS.ReadOnly = true;
            this.tbxDomainDNS.Size = new System.Drawing.Size(372, 52);
            this.tbxDomainDNS.TabIndex = 13;
            // 
            // btnDomainDelete
            // 
            this.btnDomainDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDomainDelete.Location = new System.Drawing.Point(434, 243);
            this.btnDomainDelete.Name = "btnDomainDelete";
            this.btnDomainDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDomainDelete.TabIndex = 12;
            this.btnDomainDelete.Text = "Delete";
            this.btnDomainDelete.UseVisualStyleBackColor = true;
            this.btnDomainDelete.Click += new System.EventHandler(this.btnDomainDelete_Click);
            // 
            // btnDomainSave
            // 
            this.btnDomainSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDomainSave.Location = new System.Drawing.Point(515, 243);
            this.btnDomainSave.Name = "btnDomainSave";
            this.btnDomainSave.Size = new System.Drawing.Size(75, 23);
            this.btnDomainSave.TabIndex = 11;
            this.btnDomainSave.Text = "Save";
            this.btnDomainSave.UseVisualStyleBackColor = true;
            this.btnDomainSave.Click += new System.EventHandler(this.btnDomainSave_Click);
            // 
            // tbxDomainPrivateKeyFilename
            // 
            this.tbxDomainPrivateKeyFilename.Location = new System.Drawing.Point(137, 71);
            this.tbxDomainPrivateKeyFilename.Name = "tbxDomainPrivateKeyFilename";
            this.tbxDomainPrivateKeyFilename.ReadOnly = true;
            this.tbxDomainPrivateKeyFilename.Size = new System.Drawing.Size(145, 20);
            this.tbxDomainPrivateKeyFilename.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Private key filename:";
            // 
            // tbxDomainSelector
            // 
            this.tbxDomainSelector.Location = new System.Drawing.Point(137, 45);
            this.tbxDomainSelector.Name = "tbxDomainSelector";
            this.tbxDomainSelector.Size = new System.Drawing.Size(145, 20);
            this.tbxDomainSelector.TabIndex = 8;
            this.tbxDomainSelector.TextChanged += new System.EventHandler(this.tbxDomainSelector_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Selector:";
            // 
            // tbxDomainName
            // 
            this.tbxDomainName.Location = new System.Drawing.Point(137, 19);
            this.tbxDomainName.Name = "tbxDomainName";
            this.tbxDomainName.Size = new System.Drawing.Size(145, 20);
            this.tbxDomainName.TabIndex = 6;
            this.tbxDomainName.TextChanged += new System.EventHandler(this.tbxDomainName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Domain name:";
            // 
            // btnDomainKeySelect
            // 
            this.btnDomainKeySelect.AutoSize = true;
            this.btnDomainKeySelect.Location = new System.Drawing.Point(400, 69);
            this.btnDomainKeySelect.Name = "btnDomainKeySelect";
            this.btnDomainKeySelect.Size = new System.Drawing.Size(109, 23);
            this.btnDomainKeySelect.TabIndex = 3;
            this.btnDomainKeySelect.Text = "Select key file";
            this.btnDomainKeySelect.UseVisualStyleBackColor = true;
            this.btnDomainKeySelect.Click += new System.EventHandler(this.btnDomainKeySelect_Click);
            // 
            // btDomainKeyGenerate
            // 
            this.btDomainKeyGenerate.AutoSize = true;
            this.btDomainKeyGenerate.Location = new System.Drawing.Point(288, 69);
            this.btDomainKeyGenerate.Name = "btDomainKeyGenerate";
            this.btDomainKeyGenerate.Size = new System.Drawing.Size(109, 23);
            this.btDomainKeyGenerate.TabIndex = 2;
            this.btDomainKeyGenerate.Text = "Generate new key";
            this.btDomainKeyGenerate.UseVisualStyleBackColor = true;
            this.btDomainKeyGenerate.Click += new System.EventHandler(this.btDomainKeyGenerate_Click);
            // 
            // btnAddDomain
            // 
            this.btnAddDomain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddDomain.Location = new System.Drawing.Point(6, 255);
            this.btnAddDomain.Name = "btnAddDomain";
            this.btnAddDomain.Size = new System.Drawing.Size(151, 23);
            this.btnAddDomain.TabIndex = 6;
            this.btnAddDomain.Text = "Add new domain";
            this.btnAddDomain.UseVisualStyleBackColor = true;
            this.btnAddDomain.Click += new System.EventHandler(this.btnAddDomain_Click);
            // 
            // lbxDomains
            // 
            this.lbxDomains.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbxDomains.FormattingEnabled = true;
            this.lbxDomains.IntegralHeight = false;
            this.lbxDomains.Location = new System.Drawing.Point(6, 6);
            this.lbxDomains.Name = "lbxDomains";
            this.lbxDomains.Size = new System.Drawing.Size(151, 243);
            this.lbxDomains.TabIndex = 5;
            this.lbxDomains.SelectedIndexChanged += new System.EventHandler(this.lbxDomains_SelectedIndexChanged);
            // 
            // openZipFileDialog
            // 
            this.openZipFileDialog.FileName = "dkim-exchange.zip";
            this.openZipFileDialog.Filter = "ZIP files|*.zip";
            this.openZipFileDialog.Title = "Select the .zip file downloaded from github.com";
            // 
            // saveKeyFileDialog
            // 
            this.saveKeyFileDialog.DefaultExt = "xml";
            this.saveKeyFileDialog.Filter = "All files|*.*";
            this.saveKeyFileDialog.Title = "Select a location for the new key file";
            // 
            // openKeyFileDialog
            // 
            this.openKeyFileDialog.FileName = "key";
            this.openKeyFileDialog.Filter = "Key files|*.xml;*.pem|All files|*.*";
            this.openKeyFileDialog.Title = "Select a private key for signing";
            // 
            // tbxDNSRecord
            // 
            this.tbxDNSRecord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxDNSRecord.Location = new System.Drawing.Point(137, 123);
            this.tbxDNSRecord.Multiline = true;
            this.tbxDNSRecord.Name = "tbxDNSRecord";
            this.tbxDNSRecord.ReadOnly = true;
            this.tbxDNSRecord.Size = new System.Drawing.Size(372, 52);
            this.tbxDNSRecord.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Suggested DNS Record:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 184);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Existing DNS:";
            // 
            // tbxDNSName
            // 
            this.tbxDNSName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxDNSName.Location = new System.Drawing.Point(137, 97);
            this.tbxDNSName.Name = "tbxDNSName";
            this.tbxDNSName.ReadOnly = true;
            this.tbxDNSName.Size = new System.Drawing.Size(372, 20);
            this.tbxDNSName.TabIndex = 19;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Suggested DNS Name:";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 341);
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
            this.gbHeaderToSign.PerformLayout();
            this.gbLogLevel.ResumeLayout(false);
            this.gbBodyCanonicalization.ResumeLayout(false);
            this.gbBodyCanonicalization.PerformLayout();
            this.gbHeaderCanonicalization.ResumeLayout(false);
            this.gbHeaderCanonicalization.PerformLayout();
            this.gbAlgorithm.ResumeLayout(false);
            this.gbAlgorithm.PerformLayout();
            this.tpDomain.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox txtHeaderToSign;
        private System.Windows.Forms.Button btnDomainKeySelect;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Label lblChangelog;
        private System.Windows.Forms.TextBox txtChangelog;
        private System.Windows.Forms.Button btUpateInstall;
        private System.Windows.Forms.Button btDomainKeyGenerate;
        private System.Windows.Forms.Button btUninstall;
        private System.Windows.Forms.CheckBox cbxPrereleases;
        private System.Windows.Forms.Button btnInstallZip;
        private System.Windows.Forms.OpenFileDialog openZipFileDialog;
        private System.Windows.Forms.Button btnDisable;
        private System.Windows.Forms.GroupBox gbxDomainDetails;
        private System.Windows.Forms.Button btnDomainDelete;
        private System.Windows.Forms.Button btnDomainSave;
        private System.Windows.Forms.TextBox tbxDomainPrivateKeyFilename;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxDomainSelector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxDomainName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddDomain;
        private System.Windows.Forms.ListBox lbxDomains;
        private System.Windows.Forms.TextBox tbxDomainDNS;
        private System.Windows.Forms.Button btnDomainCheckDNS;
        private System.Windows.Forms.SaveFileDialog saveKeyFileDialog;
        private System.Windows.Forms.OpenFileDialog openKeyFileDialog;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxDNSRecord;
        private System.Windows.Forms.TextBox tbxDNSName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}

