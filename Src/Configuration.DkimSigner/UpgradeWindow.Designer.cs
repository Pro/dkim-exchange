namespace Configuration.DkimSigner
{
    partial class UpgradeWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpgradeWindow));
            this.picStopService = new System.Windows.Forms.PictureBox();
            this.lblUpgradeStatus = new System.Windows.Forms.Label();
            this.lblStopService = new System.Windows.Forms.Label();
            this.lblCopyFiles = new System.Windows.Forms.Label();
            this.picCopyFiles = new System.Windows.Forms.PictureBox();
            this.lblStartService = new System.Windows.Forms.Label();
            this.picStartService = new System.Windows.Forms.PictureBox();
            this.lblDone = new System.Windows.Forms.Label();
            this.picDone = new System.Windows.Forms.PictureBox();
            this.statusImageList = new System.Windows.Forms.ImageList(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.lblInstallAgent = new System.Windows.Forms.Label();
            this.picInstallAgent = new System.Windows.Forms.PictureBox();
            this.timUpgrade = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picStopService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCopyFiles)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartService)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInstallAgent)).BeginInit();
            this.SuspendLayout();
            // 
            // picStopService
            // 
            this.picStopService.Location = new System.Drawing.Point(15, 39);
            this.picStopService.Name = "picStopService";
            this.picStopService.Size = new System.Drawing.Size(24, 24);
            this.picStopService.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picStopService.TabIndex = 0;
            this.picStopService.TabStop = false;
            // 
            // lblUpgradeStatus
            // 
            this.lblUpgradeStatus.AutoSize = true;
            this.lblUpgradeStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpgradeStatus.Location = new System.Drawing.Point(12, 13);
            this.lblUpgradeStatus.Name = "lblUpgradeStatus";
            this.lblUpgradeStatus.Size = new System.Drawing.Size(99, 13);
            this.lblUpgradeStatus.TabIndex = 1;
            this.lblUpgradeStatus.Text = "Upgrade Status:";
            // 
            // lblStopService
            // 
            this.lblStopService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStopService.AutoEllipsis = true;
            this.lblStopService.Location = new System.Drawing.Point(45, 39);
            this.lblStopService.Name = "lblStopService";
            this.lblStopService.Size = new System.Drawing.Size(392, 24);
            this.lblStopService.TabIndex = 3;
            this.lblStopService.Text = "Stopping Exchange Transport service";
            this.lblStopService.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCopyFiles
            // 
            this.lblCopyFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCopyFiles.AutoEllipsis = true;
            this.lblCopyFiles.Location = new System.Drawing.Point(45, 69);
            this.lblCopyFiles.Name = "lblCopyFiles";
            this.lblCopyFiles.Size = new System.Drawing.Size(392, 24);
            this.lblCopyFiles.TabIndex = 5;
            this.lblCopyFiles.Text = "Copying new files";
            this.lblCopyFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picCopyFiles
            // 
            this.picCopyFiles.Location = new System.Drawing.Point(15, 69);
            this.picCopyFiles.Name = "picCopyFiles";
            this.picCopyFiles.Size = new System.Drawing.Size(24, 24);
            this.picCopyFiles.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picCopyFiles.TabIndex = 4;
            this.picCopyFiles.TabStop = false;
            // 
            // lblStartService
            // 
            this.lblStartService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStartService.AutoEllipsis = true;
            this.lblStartService.Location = new System.Drawing.Point(45, 129);
            this.lblStartService.Name = "lblStartService";
            this.lblStartService.Size = new System.Drawing.Size(392, 24);
            this.lblStartService.TabIndex = 7;
            this.lblStartService.Text = "Starting Exchange Transport service";
            this.lblStartService.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picStartService
            // 
            this.picStartService.Location = new System.Drawing.Point(15, 129);
            this.picStartService.Name = "picStartService";
            this.picStartService.Size = new System.Drawing.Size(24, 24);
            this.picStartService.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picStartService.TabIndex = 6;
            this.picStartService.TabStop = false;
            // 
            // lblDone
            // 
            this.lblDone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDone.AutoEllipsis = true;
            this.lblDone.Location = new System.Drawing.Point(45, 159);
            this.lblDone.Name = "lblDone";
            this.lblDone.Size = new System.Drawing.Size(392, 24);
            this.lblDone.TabIndex = 9;
            this.lblDone.Text = "Done";
            this.lblDone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picDone
            // 
            this.picDone.Location = new System.Drawing.Point(15, 159);
            this.picDone.Name = "picDone";
            this.picDone.Size = new System.Drawing.Size(24, 24);
            this.picDone.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picDone.TabIndex = 8;
            this.picDone.TabStop = false;
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
            this.btnClose.Location = new System.Drawing.Point(362, 232);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblInstallAgent
            // 
            this.lblInstallAgent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInstallAgent.AutoEllipsis = true;
            this.lblInstallAgent.Location = new System.Drawing.Point(45, 99);
            this.lblInstallAgent.Name = "lblInstallAgent";
            this.lblInstallAgent.Size = new System.Drawing.Size(392, 24);
            this.lblInstallAgent.TabIndex = 12;
            this.lblInstallAgent.Text = "Checking and registering the agent";
            this.lblInstallAgent.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picInstallAgent
            // 
            this.picInstallAgent.Location = new System.Drawing.Point(15, 99);
            this.picInstallAgent.Name = "picInstallAgent";
            this.picInstallAgent.Size = new System.Drawing.Size(24, 24);
            this.picInstallAgent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picInstallAgent.TabIndex = 11;
            this.picInstallAgent.TabStop = false;
            // 
            // timUpgrade
            // 
            this.timUpgrade.Enabled = true;
            this.timUpgrade.Tick += new System.EventHandler(this.timUpgrade_Tick);
            // 
            // UpgradeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 267);
            this.Controls.Add(this.lblInstallAgent);
            this.Controls.Add(this.picInstallAgent);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblDone);
            this.Controls.Add(this.picDone);
            this.Controls.Add(this.lblStartService);
            this.Controls.Add(this.picStartService);
            this.Controls.Add(this.lblCopyFiles);
            this.Controls.Add(this.picCopyFiles);
            this.Controls.Add(this.lblStopService);
            this.Controls.Add(this.lblUpgradeStatus);
            this.Controls.Add(this.picStopService);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpgradeWindow";
            this.Text = "Exchange DkimSigner - Upgrade";
            this.Shown += new System.EventHandler(this.UpgradeWindow_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.picStopService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCopyFiles)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartService)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picDone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picInstallAgent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picStopService;
        private System.Windows.Forms.Label lblUpgradeStatus;
        private System.Windows.Forms.Label lblStopService;
        private System.Windows.Forms.Label lblCopyFiles;
        private System.Windows.Forms.PictureBox picCopyFiles;
        private System.Windows.Forms.Label lblStartService;
        private System.Windows.Forms.PictureBox picStartService;
        private System.Windows.Forms.Label lblDone;
        private System.Windows.Forms.PictureBox picDone;
        private System.Windows.Forms.ImageList statusImageList;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblInstallAgent;
        private System.Windows.Forms.PictureBox picInstallAgent;
        private System.Windows.Forms.Timer timUpgrade;
    }
}