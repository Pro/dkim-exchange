namespace Exchange.DkimConfiguration
{
    partial class InstallWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallWindow));
            this.picStopService = new System.Windows.Forms.PictureBox();
            this.lbStopService = new System.Windows.Forms.Label();
            this.lbCopyFiles = new System.Windows.Forms.Label();
            this.picCopyFiles = new System.Windows.Forms.PictureBox();
            this.lbStartService = new System.Windows.Forms.Label();
            this.picStartService = new System.Windows.Forms.PictureBox();
            this.statusImageList = new System.Windows.Forms.ImageList(this.components);
            this.btClose = new System.Windows.Forms.Button();
            this.lbInstallAgent = new System.Windows.Forms.Label();
            this.picInstallAgent = new System.Windows.Forms.PictureBox();
            this.lbDownloadFiles = new System.Windows.Forms.Label();
            this.picDownloadFiles = new System.Windows.Forms.PictureBox();
            this.btInstall = new System.Windows.Forms.Button();
            this.cbxPrereleases = new System.Windows.Forms.CheckBox();
            this.btBrowse = new System.Windows.Forms.Button();
            this.txtVersionFile = new System.Windows.Forms.TextBox();
            this.lbVersionFile = new System.Windows.Forms.Label();
            this.lbOr = new System.Windows.Forms.Label();
            this.cbVersionWeb = new System.Windows.Forms.ComboBox();
            this.lbVersionWeb = new System.Windows.Forms.Label();
            this.lblExchangeVersionWait = new System.Windows.Forms.Label();
            this.gbSelectVersionToInstall = new System.Windows.Forms.GroupBox();
            this.gbInstallStatus = new System.Windows.Forms.GroupBox();
            this.picExtractFiles = new System.Windows.Forms.PictureBox();
            this.lbExtractFiles = new System.Windows.Forms.Label();
            this.progressInstall = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.picStopService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCopyFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInstallAgent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDownloadFiles)).BeginInit();
            this.gbSelectVersionToInstall.SuspendLayout();
            this.gbInstallStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picExtractFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // picStopService
            // 
            this.picStopService.Location = new System.Drawing.Point(11, 119);
            this.picStopService.Name = "picStopService";
            this.picStopService.Size = new System.Drawing.Size(24, 24);
            this.picStopService.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picStopService.TabIndex = 0;
            this.picStopService.TabStop = false;
            // 
            // lbStopService
            // 
            this.lbStopService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStopService.AutoEllipsis = true;
            this.lbStopService.Enabled = false;
            this.lbStopService.Location = new System.Drawing.Point(41, 119);
            this.lbStopService.Name = "lbStopService";
            this.lbStopService.Size = new System.Drawing.Size(451, 24);
            this.lbStopService.TabIndex = 1;
            this.lbStopService.Text = "Stopping Microsoft Exchange Transport service";
            this.lbStopService.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCopyFiles
            // 
            this.lbCopyFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCopyFiles.AutoEllipsis = true;
            this.lbCopyFiles.Enabled = false;
            this.lbCopyFiles.Location = new System.Drawing.Point(41, 149);
            this.lbCopyFiles.Name = "lbCopyFiles";
            this.lbCopyFiles.Size = new System.Drawing.Size(451, 24);
            this.lbCopyFiles.TabIndex = 2;
            this.lbCopyFiles.Text = "Copying new files";
            this.lbCopyFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picCopyFiles
            // 
            this.picCopyFiles.Location = new System.Drawing.Point(11, 149);
            this.picCopyFiles.Name = "picCopyFiles";
            this.picCopyFiles.Size = new System.Drawing.Size(24, 24);
            this.picCopyFiles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picCopyFiles.TabIndex = 4;
            this.picCopyFiles.TabStop = false;
            // 
            // lbStartService
            // 
            this.lbStartService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStartService.AutoEllipsis = true;
            this.lbStartService.Enabled = false;
            this.lbStartService.Location = new System.Drawing.Point(41, 209);
            this.lbStartService.Name = "lbStartService";
            this.lbStartService.Size = new System.Drawing.Size(451, 24);
            this.lbStartService.TabIndex = 4;
            this.lbStartService.Text = "Starting Microsoft Exchange Transport service";
            this.lbStartService.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picStartService
            // 
            this.picStartService.Location = new System.Drawing.Point(11, 209);
            this.picStartService.Name = "picStartService";
            this.picStartService.Size = new System.Drawing.Size(24, 24);
            this.picStartService.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picStartService.TabIndex = 6;
            this.picStartService.TabStop = false;
            // 
            // statusImageList
            // 
            this.statusImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("statusImageList.ImageStream")));
            this.statusImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.statusImageList.Images.SetKeyName(0, "check.png");
            this.statusImageList.Images.SetKeyName(1, "error.png");
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.Location = new System.Drawing.Point(429, 407);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 2;
            this.btClose.Text = "&Close";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // lbInstallAgent
            // 
            this.lbInstallAgent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInstallAgent.AutoEllipsis = true;
            this.lbInstallAgent.Enabled = false;
            this.lbInstallAgent.Location = new System.Drawing.Point(41, 179);
            this.lbInstallAgent.Name = "lbInstallAgent";
            this.lbInstallAgent.Size = new System.Drawing.Size(451, 24);
            this.lbInstallAgent.TabIndex = 3;
            this.lbInstallAgent.Text = "Checking and registering the agent";
            this.lbInstallAgent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picInstallAgent
            // 
            this.picInstallAgent.Location = new System.Drawing.Point(11, 179);
            this.picInstallAgent.Name = "picInstallAgent";
            this.picInstallAgent.Size = new System.Drawing.Size(24, 24);
            this.picInstallAgent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picInstallAgent.TabIndex = 11;
            this.picInstallAgent.TabStop = false;
            // 
            // lbDownloadFiles
            // 
            this.lbDownloadFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbDownloadFiles.AutoEllipsis = true;
            this.lbDownloadFiles.Enabled = false;
            this.lbDownloadFiles.Location = new System.Drawing.Point(41, 59);
            this.lbDownloadFiles.Name = "lbDownloadFiles";
            this.lbDownloadFiles.Size = new System.Drawing.Size(451, 24);
            this.lbDownloadFiles.TabIndex = 0;
            this.lbDownloadFiles.Text = "Download files";
            this.lbDownloadFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picDownloadFiles
            // 
            this.picDownloadFiles.Location = new System.Drawing.Point(11, 59);
            this.picDownloadFiles.Name = "picDownloadFiles";
            this.picDownloadFiles.Size = new System.Drawing.Size(24, 24);
            this.picDownloadFiles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picDownloadFiles.TabIndex = 13;
            this.picDownloadFiles.TabStop = false;
            // 
            // btInstall
            // 
            this.btInstall.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btInstall.Enabled = false;
            this.btInstall.Location = new System.Drawing.Point(417, 93);
            this.btInstall.Name = "btInstall";
            this.btInstall.Size = new System.Drawing.Size(75, 23);
            this.btInstall.TabIndex = 8;
            this.btInstall.Text = "&Install";
            this.btInstall.UseVisualStyleBackColor = true;
            this.btInstall.Click += new System.EventHandler(this.btInstall_Click);
            // 
            // cbxPrereleases
            // 
            this.cbxPrereleases.AutoSize = true;
            this.cbxPrereleases.Enabled = false;
            this.cbxPrereleases.Location = new System.Drawing.Point(239, 24);
            this.cbxPrereleases.Name = "cbxPrereleases";
            this.cbxPrereleases.Size = new System.Drawing.Size(155, 17);
            this.cbxPrereleases.TabIndex = 2;
            this.cbxPrereleases.Text = "Include prerelease versions";
            this.cbxPrereleases.UseVisualStyleBackColor = true;
            this.cbxPrereleases.CheckedChanged += new System.EventHandler(this.cbxPrereleases_CheckedChanged);
            // 
            // btBrowse
            // 
            this.btBrowse.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btBrowse.Location = new System.Drawing.Point(417, 64);
            this.btBrowse.Name = "btBrowse";
            this.btBrowse.Size = new System.Drawing.Size(75, 23);
            this.btBrowse.TabIndex = 6;
            this.btBrowse.Text = "&Browse";
            this.btBrowse.UseVisualStyleBackColor = true;
            this.btBrowse.Click += new System.EventHandler(this.btBrowse_Click);
            // 
            // txtVersionFile
            // 
            this.txtVersionFile.Location = new System.Drawing.Point(101, 66);
            this.txtVersionFile.Name = "txtVersionFile";
            this.txtVersionFile.ReadOnly = true;
            this.txtVersionFile.Size = new System.Drawing.Size(310, 20);
            this.txtVersionFile.TabIndex = 5;
            // 
            // lbVersionFile
            // 
            this.lbVersionFile.AutoSize = true;
            this.lbVersionFile.Location = new System.Drawing.Point(8, 69);
            this.lbVersionFile.Name = "lbVersionFile";
            this.lbVersionFile.Size = new System.Drawing.Size(87, 13);
            this.lbVersionFile.TabIndex = 4;
            this.lbVersionFile.Text = "ZIP file to install :";
            // 
            // lbOr
            // 
            this.lbOr.AutoSize = true;
            this.lbOr.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOr.Location = new System.Drawing.Point(8, 46);
            this.lbOr.Name = "lbOr";
            this.lbOr.Size = new System.Drawing.Size(20, 13);
            this.lbOr.TabIndex = 3;
            this.lbOr.Text = "Or";
            // 
            // cbVersionWeb
            // 
            this.cbVersionWeb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVersionWeb.Enabled = false;
            this.cbVersionWeb.FormattingEnabled = true;
            this.cbVersionWeb.Location = new System.Drawing.Point(101, 22);
            this.cbVersionWeb.Name = "cbVersionWeb";
            this.cbVersionWeb.Size = new System.Drawing.Size(132, 21);
            this.cbVersionWeb.TabIndex = 1;
            this.cbVersionWeb.SelectedIndexChanged += new System.EventHandler(this.cbVersionWeb_SelectedIndexChanged);
            // 
            // lbVersionWeb
            // 
            this.lbVersionWeb.AutoSize = true;
            this.lbVersionWeb.Location = new System.Drawing.Point(8, 25);
            this.lbVersionWeb.Name = "lbVersionWeb";
            this.lbVersionWeb.Size = new System.Drawing.Size(89, 13);
            this.lbVersionWeb.TabIndex = 0;
            this.lbVersionWeb.Text = "Install from Web :";
            // 
            // lblExchangeVersionWait
            // 
            this.lblExchangeVersionWait.AutoSize = true;
            this.lblExchangeVersionWait.Location = new System.Drawing.Point(122, 98);
            this.lblExchangeVersionWait.Name = "lblExchangeVersionWait";
            this.lblExchangeVersionWait.Size = new System.Drawing.Size(213, 13);
            this.lblExchangeVersionWait.TabIndex = 7;
            this.lblExchangeVersionWait.Text = "Checking Exchange Version...  Please Wait";
            // 
            // gbSelectVersionToInstall
            // 
            this.gbSelectVersionToInstall.Controls.Add(this.lblExchangeVersionWait);
            this.gbSelectVersionToInstall.Controls.Add(this.btInstall);
            this.gbSelectVersionToInstall.Controls.Add(this.cbVersionWeb);
            this.gbSelectVersionToInstall.Controls.Add(this.lbVersionWeb);
            this.gbSelectVersionToInstall.Controls.Add(this.cbxPrereleases);
            this.gbSelectVersionToInstall.Controls.Add(this.lbOr);
            this.gbSelectVersionToInstall.Controls.Add(this.btBrowse);
            this.gbSelectVersionToInstall.Controls.Add(this.lbVersionFile);
            this.gbSelectVersionToInstall.Controls.Add(this.txtVersionFile);
            this.gbSelectVersionToInstall.Location = new System.Drawing.Point(12, 11);
            this.gbSelectVersionToInstall.Name = "gbSelectVersionToInstall";
            this.gbSelectVersionToInstall.Size = new System.Drawing.Size(498, 130);
            this.gbSelectVersionToInstall.TabIndex = 0;
            this.gbSelectVersionToInstall.TabStop = false;
            this.gbSelectVersionToInstall.Text = "Select version to install";
            // 
            // gbInstallStatus
            // 
            this.gbInstallStatus.Controls.Add(this.progressInstall);
            this.gbInstallStatus.Controls.Add(this.picExtractFiles);
            this.gbInstallStatus.Controls.Add(this.lbExtractFiles);
            this.gbInstallStatus.Controls.Add(this.lbDownloadFiles);
            this.gbInstallStatus.Controls.Add(this.picStopService);
            this.gbInstallStatus.Controls.Add(this.lbStopService);
            this.gbInstallStatus.Controls.Add(this.picDownloadFiles);
            this.gbInstallStatus.Controls.Add(this.picCopyFiles);
            this.gbInstallStatus.Controls.Add(this.lbInstallAgent);
            this.gbInstallStatus.Controls.Add(this.lbCopyFiles);
            this.gbInstallStatus.Controls.Add(this.picInstallAgent);
            this.gbInstallStatus.Controls.Add(this.picStartService);
            this.gbInstallStatus.Controls.Add(this.lbStartService);
            this.gbInstallStatus.Enabled = false;
            this.gbInstallStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbInstallStatus.Location = new System.Drawing.Point(12, 154);
            this.gbInstallStatus.Name = "gbInstallStatus";
            this.gbInstallStatus.Size = new System.Drawing.Size(498, 247);
            this.gbInstallStatus.TabIndex = 1;
            this.gbInstallStatus.TabStop = false;
            this.gbInstallStatus.Text = "Install Status";
            // 
            // picExtractFiles
            // 
            this.picExtractFiles.Location = new System.Drawing.Point(11, 89);
            this.picExtractFiles.Name = "picExtractFiles";
            this.picExtractFiles.Size = new System.Drawing.Size(24, 24);
            this.picExtractFiles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picExtractFiles.TabIndex = 15;
            this.picExtractFiles.TabStop = false;
            // 
            // lbExtractFiles
            // 
            this.lbExtractFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbExtractFiles.AutoEllipsis = true;
            this.lbExtractFiles.Enabled = false;
            this.lbExtractFiles.Location = new System.Drawing.Point(41, 89);
            this.lbExtractFiles.Name = "lbExtractFiles";
            this.lbExtractFiles.Size = new System.Drawing.Size(451, 24);
            this.lbExtractFiles.TabIndex = 14;
            this.lbExtractFiles.Text = "Extract files";
            this.lbExtractFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressInstall
            // 
            this.progressInstall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressInstall.Location = new System.Drawing.Point(11, 19);
            this.progressInstall.Maximum = 6;
            this.progressInstall.Name = "progressInstall";
            this.progressInstall.Size = new System.Drawing.Size(481, 23);
            this.progressInstall.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressInstall.TabIndex = 3;
            // 
            // InstallWindow
            // 
            this.AcceptButton = this.btClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 442);
            this.ControlBox = false;
            this.Controls.Add(this.gbSelectVersionToInstall);
            this.Controls.Add(this.gbInstallStatus);
            this.Controls.Add(this.btClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Exchange DkimSigner";
            this.Load += new System.EventHandler(this.InstallWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picStopService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCopyFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInstallAgent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDownloadFiles)).EndInit();
            this.gbSelectVersionToInstall.ResumeLayout(false);
            this.gbSelectVersionToInstall.PerformLayout();
            this.gbInstallStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picExtractFiles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picStopService;
        private System.Windows.Forms.Label lbStopService;
        private System.Windows.Forms.Label lbCopyFiles;
        private System.Windows.Forms.PictureBox picCopyFiles;
        private System.Windows.Forms.Label lbStartService;
        private System.Windows.Forms.PictureBox picStartService;
        private System.Windows.Forms.ImageList statusImageList;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Label lbInstallAgent;
        private System.Windows.Forms.PictureBox picInstallAgent;
        private System.Windows.Forms.Label lbDownloadFiles;
        private System.Windows.Forms.PictureBox picDownloadFiles;
        private System.Windows.Forms.Button btInstall;
        private System.Windows.Forms.CheckBox cbxPrereleases;
        private System.Windows.Forms.Button btBrowse;
        public System.Windows.Forms.TextBox txtVersionFile;
        private System.Windows.Forms.Label lbVersionFile;
        private System.Windows.Forms.Label lbOr;
        public System.Windows.Forms.ComboBox cbVersionWeb;
        private System.Windows.Forms.Label lbVersionWeb;
        private System.Windows.Forms.Label lblExchangeVersionWait;
        private System.Windows.Forms.GroupBox gbSelectVersionToInstall;
        private System.Windows.Forms.GroupBox gbInstallStatus;
        private System.Windows.Forms.PictureBox picExtractFiles;
        private System.Windows.Forms.Label lbExtractFiles;
        private System.Windows.Forms.ProgressBar progressInstall;
    }
}