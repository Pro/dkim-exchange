namespace Configuration.DkimSigner
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
            this.lbUpgradeStatus = new System.Windows.Forms.Label();
            this.lbStopService = new System.Windows.Forms.Label();
            this.lbCopyFiles = new System.Windows.Forms.Label();
            this.picCopyFiles = new System.Windows.Forms.PictureBox();
            this.lbStartService = new System.Windows.Forms.Label();
            this.picStartService = new System.Windows.Forms.PictureBox();
            this.statusImageList = new System.Windows.Forms.ImageList(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.lbInstallAgent = new System.Windows.Forms.Label();
            this.picInstallAgent = new System.Windows.Forms.PictureBox();
            this.lbDownloadFiles = new System.Windows.Forms.Label();
            this.picDownloadFiles = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picStopService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCopyFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInstallAgent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDownloadFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // picStopService
            // 
            this.picStopService.Location = new System.Drawing.Point(15, 69);
            this.picStopService.Name = "picStopService";
            this.picStopService.Size = new System.Drawing.Size(24, 24);
            this.picStopService.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picStopService.TabIndex = 0;
            this.picStopService.TabStop = false;
            // 
            // lbUpgradeStatus
            // 
            this.lbUpgradeStatus.AutoSize = true;
            this.lbUpgradeStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUpgradeStatus.Location = new System.Drawing.Point(12, 13);
            this.lbUpgradeStatus.Name = "lbUpgradeStatus";
            this.lbUpgradeStatus.Size = new System.Drawing.Size(85, 13);
            this.lbUpgradeStatus.TabIndex = 0;
            this.lbUpgradeStatus.Text = "Install Status:";
            // 
            // lbStopService
            // 
            this.lbStopService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStopService.AutoEllipsis = true;
            this.lbStopService.Location = new System.Drawing.Point(45, 69);
            this.lbStopService.Name = "lbStopService";
            this.lbStopService.Size = new System.Drawing.Size(406, 24);
            this.lbStopService.TabIndex = 1;
            this.lbStopService.Text = "Stopping Exchange Transport service";
            this.lbStopService.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCopyFiles
            // 
            this.lbCopyFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCopyFiles.AutoEllipsis = true;
            this.lbCopyFiles.Location = new System.Drawing.Point(45, 99);
            this.lbCopyFiles.Name = "lbCopyFiles";
            this.lbCopyFiles.Size = new System.Drawing.Size(406, 24);
            this.lbCopyFiles.TabIndex = 3;
            this.lbCopyFiles.Text = "Copying new files";
            this.lbCopyFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picCopyFiles
            // 
            this.picCopyFiles.Location = new System.Drawing.Point(15, 99);
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
            this.lbStartService.Location = new System.Drawing.Point(45, 159);
            this.lbStartService.Name = "lbStartService";
            this.lbStartService.Size = new System.Drawing.Size(406, 24);
            this.lbStartService.TabIndex = 5;
            this.lbStartService.Text = "Starting Exchange Transport service";
            this.lbStartService.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picStartService
            // 
            this.picStartService.Location = new System.Drawing.Point(15, 159);
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
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Enabled = false;
            this.btnClose.Location = new System.Drawing.Point(365, 179);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lbInstallAgent
            // 
            this.lbInstallAgent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbInstallAgent.AutoEllipsis = true;
            this.lbInstallAgent.Location = new System.Drawing.Point(45, 129);
            this.lbInstallAgent.Name = "lbInstallAgent";
            this.lbInstallAgent.Size = new System.Drawing.Size(406, 24);
            this.lbInstallAgent.TabIndex = 4;
            this.lbInstallAgent.Text = "Checking and registering the agent";
            this.lbInstallAgent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picInstallAgent
            // 
            this.picInstallAgent.Location = new System.Drawing.Point(15, 129);
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
            this.lbDownloadFiles.Location = new System.Drawing.Point(45, 39);
            this.lbDownloadFiles.Name = "lbDownloadFiles";
            this.lbDownloadFiles.Size = new System.Drawing.Size(406, 24);
            this.lbDownloadFiles.TabIndex = 2;
            this.lbDownloadFiles.Text = "Download new files";
            this.lbDownloadFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picDownloadFiles
            // 
            this.picDownloadFiles.Location = new System.Drawing.Point(15, 39);
            this.picDownloadFiles.Name = "picDownloadFiles";
            this.picDownloadFiles.Size = new System.Drawing.Size(24, 24);
            this.picDownloadFiles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picDownloadFiles.TabIndex = 13;
            this.picDownloadFiles.TabStop = false;
            // 
            // InstallWindow
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 214);
            this.Controls.Add(this.lbDownloadFiles);
            this.Controls.Add(this.picDownloadFiles);
            this.Controls.Add(this.lbInstallAgent);
            this.Controls.Add(this.picInstallAgent);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lbStartService);
            this.Controls.Add(this.picStartService);
            this.Controls.Add(this.lbCopyFiles);
            this.Controls.Add(this.picCopyFiles);
            this.Controls.Add(this.lbStopService);
            this.Controls.Add(this.lbUpgradeStatus);
            this.Controls.Add(this.picStopService);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InstallWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Exchange DkimSigner - Install";
            this.Load += new System.EventHandler(this.UpgradeWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picStopService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCopyFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInstallAgent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDownloadFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picStopService;
        private System.Windows.Forms.Label lbUpgradeStatus;
        private System.Windows.Forms.Label lbStopService;
        private System.Windows.Forms.Label lbCopyFiles;
        private System.Windows.Forms.PictureBox picCopyFiles;
        private System.Windows.Forms.Label lbStartService;
        private System.Windows.Forms.PictureBox picStartService;
        private System.Windows.Forms.ImageList statusImageList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lbInstallAgent;
        private System.Windows.Forms.PictureBox picInstallAgent;
        private System.Windows.Forms.Label lbDownloadFiles;
        private System.Windows.Forms.PictureBox picDownloadFiles;
    }
}