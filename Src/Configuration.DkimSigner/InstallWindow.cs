using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Configuration.DkimSigner.GitHub;
using Configuration.DkimSigner.Exchange;
using Configuration.DkimSigner.FileIO;
using Ionic.Zip;

namespace Configuration.DkimSigner
{
    public partial class InstallWindow : Form
    {
        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/

        private List<Release> aoVersionAvailable = null;

        private string sExchangeVersion = null;

        private Thread thDkimSignerAvailable = null;
        private Thread thInstallProcess = null;

        delegate void EnablePrereleasesCallback(bool sStatus);
        delegate void RefreshVersionWebItemsCallback();

        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public InstallWindow()
        {
            this.InitializeComponent();

            this.picStopService.Image = null;
            this.picCopyFiles.Image = null;
            this.picInstallAgent.Image = null;
            this.picStartService.Image = null;
        }

        /**********************************************************/
        /************************* Events *************************/
        /**********************************************************/

        private void InstallWindow_Load(object sender, EventArgs e)
        {
            this.UpdateDkimSignerAvailable();

            this.sExchangeVersion = ExchangeServer.GetInstalledVersion();
        }

        private void InstallWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // IF any thread running, we stop them before exit
            if (this.thDkimSignerAvailable != null && this.thDkimSignerAvailable.ThreadState == ThreadState.Running)
            {
                this.thDkimSignerAvailable.Abort();
            }
            
            if (this.thInstallProcess != null && this.thInstallProcess.ThreadState == ThreadState.Running)
            {
                this.thInstallProcess.Abort();
            }
        }

        private void cbVersionWeb_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.txtVersionFile.Clear();
        }

        private void cbxPrereleases_CheckedChanged(object sender, EventArgs e)
        {
            this.EnablePrereleases(false);            
            this.UpdateDkimSignerAvailable();
        }

        /**********************************************************/
        /******************* Internal functions *******************/
        /**********************************************************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sStatus"></param>
        private void EnablePrereleases(bool sStatus)
        {
            this.cbxPrereleases.Enabled = sStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RefreshVersionWebItems()
        {
            this.cbVersionWeb.Items.Clear();

            if (this.aoVersionAvailable != null)
            {
                if (this.aoVersionAvailable.Count > 0)
                {
                    foreach (Release oVersionAvailable in this.aoVersionAvailable)
                    {
                        this.cbVersionWeb.Items.Add(oVersionAvailable.TagName);
                    }

                    MessageBox.Show("The release information from the Web have been load.");

                    this.cbVersionWeb.Enabled = true;
                }
                else
                {
                    MessageBox.Show("No release information from the Web are available.");

                    this.cbVersionWeb.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show("Could not obtain release information from the Web. Check your Internet connexion or please retry later.");

                this.cbVersionWeb.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckDkimSignerAvailableSafe()
        {
            this.aoVersionAvailable = ApiWrapper.GetAllRelease(cbxPrereleases.Checked);
            this.aoVersionAvailable = ApiWrapper.GetAllRelease(cbxPrereleases.Checked, new Version("2.0.0"));

            if (this.cbVersionWeb.InvokeRequired)
            {
                RefreshVersionWebItemsCallback d = new RefreshVersionWebItemsCallback(this.RefreshVersionWebItems);
                this.Invoke(d);
            }
            else
            {
                this.RefreshVersionWebItems();
            }

            if (this.cbxPrereleases.InvokeRequired)
            {
                EnablePrereleasesCallback d = new EnablePrereleasesCallback(this.EnablePrereleases);
                this.Invoke(d, true);
            }
            else
            {
                this.EnablePrereleases(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateDkimSignerAvailable()
        {
            // Get Exchange.DkimSigner version available
            this.thDkimSignerAvailable = new Thread(new ThreadStart(this.CheckDkimSignerAvailableSafe));

            // Start the threads that will do lookup
            try
            {
                this.thDkimSignerAvailable.Start();
            }
            catch (ThreadAbortException) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sZipPath"></param>
        /// <returns></returns>
        private bool DownloadFile(string sZipPath)
        {
            string sZipballUrl = string.Empty;

            foreach (Release oRelease in aoVersionAvailable)
            {
                if (oRelease.TagName == cbVersionWeb.Text)
                {
                    sZipballUrl = oRelease.ZipballUrl;
                    break;
                }
            }

            DownloadProgressWindow oDpw = new DownloadProgressWindow(sZipballUrl, sZipPath);
            return (oDpw.ShowDialog() == DialogResult.OK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sZipPath"></param>
        /// <returns></returns>
        private bool ExtractFiles(string sZipPath)
        {
            bool bStatus = true;

            try
            {
                using (ZipFile zip1 = ZipFile.Read(sZipPath))
                {
                    foreach (ZipEntry e in zip1)
                    {
                        e.Extract(Path.GetDirectoryName(sZipPath), ExtractExistingFileAction.OverwriteSilently);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                bStatus = false;
            }

            return bStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asSourcePath"></param>
        /// <returns></returns>
        private bool CopyFiles(List<string> asSourcePath)
        {
            bool bReturn = false;
            bool bAnyOperationsAborted = false;

            // IF the directory "C:\Program Files\Exchange DkimSigner" doesn't exist, create it 
            if (!Directory.Exists(Constants.DKIM_SIGNER_PATH))
            {
                Directory.CreateDirectory(Constants.DKIM_SIGNER_PATH);
            }

            // Generate list of source files
            string[] asSourceFiles = new string[0];
            foreach (string sSourcePath in asSourcePath)
            {
                string[] asTemp = Directory.GetFiles(sSourcePath);

                Array.Resize(ref asSourceFiles, asSourceFiles.Length + asTemp.Length);
                Array.Copy(asTemp, 0, asSourceFiles,  asSourceFiles.Length - asTemp.Length, asTemp.Length);
            }

            // Generate list of destinations files
            string[] asDestinationFiles = new string[asSourceFiles.Length];
            for (int i = 0; i < asSourceFiles.Length; i++)
            {
                string sFile =  Path.GetFileName(asSourceFiles[i]);
                asDestinationFiles[i] = Path.Combine(Constants.DKIM_SIGNER_PATH,sFile);
            }

            bReturn = FileOperation.CopyFiles(this.Handle, asSourceFiles, asDestinationFiles, true, "Copy files", out bAnyOperationsAborted);

            return bReturn && !bAnyOperationsAborted;
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopService()
        {
            try
            {
                ExchangeServer.StopTransportService();
            }
            catch (ExchangeServerException) { }
        }

        /// <summary>
        /// 
        /// </summary>
        private void InstallAgent()
        {
            if (!ExchangeServer.IsTransportServiceInstalled())
            {
                // First make sure the following Registry key exists
                // HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM
                if (RegistryHelper.Open(@"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM") != null)
                {
                    RegistryHelper.WriteSubKeyTree(@"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM");
                }

                // Install DKIM Transport Agent in Microsoft Exchange 
                try
                {
                    ExchangeServer.InstallDkimTransportAgent();
                }
                catch (ExchangeServerException) { }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartService()
        {
            try
            {
                ExchangeServer.StartTransportService();
            }
            catch (ExchangeServerException) { }
        }

        /**********************************************************/
        /********************** Button click **********************/
        /**********************************************************/

        private void btBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFileDialog = new OpenFileDialog();

            oFileDialog.FileName = "dkim-exchange.zip";
            oFileDialog.Filter = "ZIP files|*.zip";
            oFileDialog.Title = "Select the .zip file downloaded from github.com";

            if (oFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.cbVersionWeb.SelectedIndex = -1;

                this.txtVersionFile.Text = oFileDialog.FileName;
            }
        }

        private void btInstall_Click(object sender, EventArgs e)
        {
            if(this.sExchangeVersion != "Not installed")
            {
                if (this.txtVersionFile.Text != string.Empty || this.cbVersionWeb.SelectedIndex > -1)
                {
                    bool bStatus = true;
                    string sZipPath = string.Empty;

                    this.cbVersionWeb.Enabled = false;
                    this.cbxPrereleases.Enabled = false;
                    this.btBrowse.Enabled = false;
                    this.btInstall.Enabled = false;
                    this.btClose.Enabled = false;

                    //
                    // Download required files
                    //

                    this.lbDownloadFiles.Enabled = true;

                    if (this.cbVersionWeb.SelectedIndex > -1)
                    {
                        sZipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".zip");
                        bStatus = this.DownloadFile(sZipPath);
                    }
                    else
                    {
                        sZipPath = this.txtVersionFile.Text;
                    }

                    this.picDownloadFiles.Image = bStatus ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                    this.Refresh();

                    //
                    // Copy required files
                    //

                    this.lbCopyFiles.Enabled = true;

                    string sTemp = string.Empty;
                    if (bStatus)
                    {
                        if (this.sExchangeVersion.StartsWith("8.3."))
                        {
                            sTemp = "Exchange 2007 SP3";
                        }
                        else if (this.sExchangeVersion.StartsWith("14.0."))
                        {
                            sTemp = "Exchange 2010";
                        }
                        else if (this.sExchangeVersion.StartsWith("14.1."))
                        {
                            sTemp = "Exchange 2010 SP1";
                        }
                        else if (this.sExchangeVersion.StartsWith("14.2."))
                        {
                            sTemp = "Exchange 2010 SP2";
                        }
                        else if (this.sExchangeVersion.StartsWith("14.3."))
                        {
                            sTemp = "Exchange 2010 SP3";
                        }
                        else if (this.sExchangeVersion.StartsWith("15.0.516.32"))
                        {
                            sTemp = "Exchange 2013";
                        }
                        else if (this.sExchangeVersion.StartsWith("15.0.620.29"))
                        {
                            sTemp = "Exchange 2013 CU1";
                        }
                        else if (this.sExchangeVersion.StartsWith("15.0.712.24"))
                        {
                            sTemp = "Exchange 2013 CU2";
                        }
                        else if (this.sExchangeVersion.StartsWith("15.0.775.38"))
                        {
                            sTemp = "Exchange 2013 CU3";
                        }
                        else if (this.sExchangeVersion.StartsWith("15.0.847.32"))
                        {
                            sTemp = "Exchange 2013 SP1 CU4";
                        }
                        else if (this.sExchangeVersion.StartsWith("15.0.913.22"))
                        {
                            sTemp = "Exchange 2013 SP1 CU5";
                        }
                        else
                        {
                            MessageBox.Show("The current Microsoft Exchange version isn't support by DKIM agent.");
                            bStatus = false;
                        }
                    }

                    if (bStatus)
                    { 
                        bStatus = this.ExtractFiles(sZipPath);
                    }

                    if (bStatus)
                    {  
                        List<string> asCopyFilePath = new List<string>();
                        asCopyFilePath.Add(Path.Combine(Path.GetDirectoryName(sZipPath), Path.GetFileNameWithoutExtension(sZipPath), @"Src\Configuration.DkimSigner\bin\Release"));
                        asCopyFilePath.Add(Path.Combine(Path.GetDirectoryName(sZipPath), Path.GetFileNameWithoutExtension(sZipPath), Path.Combine(@"Src\Exchange.DkimSigner\bin\" + sTemp)));
                        bStatus = this.CopyFiles(asCopyFilePath);

                        Directory.Delete(Path.Combine(Path.GetDirectoryName(sZipPath), Path.GetFileNameWithoutExtension(sZipPath)), true);
                    }

                    this.picCopyFiles.Image = bStatus ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                    this.Refresh();

                    //
                    // Stop Microsoft Exchange Transport Service
                    //

                    this.lbStopService.Enabled = true;

                    if (bStatus)
                    {
                        //this.StopService();
                    }

                    this.picStopService.Image = bStatus ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                    this.Refresh();

                    //
                    // Instal DKIM Signer agent in Microsoft Exchange Transport Service
                    //

                    this.lbInstallAgent.Enabled = true;

                    if (bStatus)
                    {
                        //this.InstallAgent();
                    }

                    this.picInstallAgent.Image = bStatus ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                    this.Refresh();

                    //
                    // Start Microsoft Exchange Transport Service
                    //

                    this.lbStartService.Enabled = true;

                    if (bStatus)
                    {
                        //this.StartService();
                    }

                    this.picStartService.Image = bStatus ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                    this.Refresh();

                    //
                    // Done
                    //

                    this.btClose.Enabled = true;
                }
                else
                {
                    MessageBox.Show("You have to select a version to install from the Web or a ZIP file.");
                }
            }
            else
            {
                MessageBox.Show("Microsoft Exchange server must be installed before attempt to install DKIM agent.");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btClose_Click(object sender, EventArgs e)
        {
            // Start new installed DKIM Signer Configuration GUI
            if (this.btInstall.Enabled == false && this.picStopService.Image == this.statusImageList.Images[0])
            {
                if (MessageBox.Show("Do you want to start DKIM Signer configuration tool?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sPathExec = Path.Combine(Constants.DKIM_SIGNER_PATH, Constants.DKIM_SIGNER_CONFIGURATION_EXE);
                    if (File.Exists(sPathExec))
                    {
                        System.Diagnostics.Process.Start(sPathExec);
                    }
                    else
                    {
                        MessageBox.Show("Couldn't find 'Configuration.DkimSigner.exe' in \n" + Constants.DKIM_SIGNER_PATH, "Exec error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.Close();
        }
    }
}