using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

using Configuration.DkimSigner.GitHub;
using Configuration.DkimSigner.Exchange;
using Configuration.DkimSigner.FileIO;

namespace Configuration.DkimSigner
{
    public partial class InstallWindow : Form
    {
        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/

        private ExchangeServer oExchange = null;

        private List<Release> aoVersionAvailable = null;

        private Thread thDkimSignerAvailable = null;
        private Thread thInstallProcess = null;

        delegate void EnablePrereleasesCallback(bool sStatus);
        delegate void RefreshVersionWebItemsCallback();

        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public InstallWindow(ExchangeServer oServer)
        {
            this.InitializeComponent();

            this.picStopService.Image = null;
            this.picCopyFiles.Image = null;
            this.picInstallAgent.Image = null;
            this.picStartService.Image = null;

            // Load variable
            this.oExchange = oServer;
        }

        /**********************************************************/
        /************************* Events *************************/
        /**********************************************************/

        private void InstallWindow_Load(object sender, EventArgs e)
        {
            this.UpdateDkimSignerAvailable();
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

        private void EnablePrereleases(bool sStatus)
        {
            this.cbxPrereleases.Enabled = sStatus;
        }

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

            DownloadProgressWindow oDpw = new DownloadProgressWindow(sZipballUrl, Path.Combine(Path.GetTempPath(), sZipPath));
            return (oDpw.ShowDialog() == DialogResult.OK);
        }

        private bool CopyFiles(string sSourcePath)
        {
            bool bReturn = false;
            bool bAnyOperationsAborted = false;

            // IF the directory "C:\Program Files\Exchange DkimSigner" doesn't exist, create it 
            if (!Directory.Exists(Constants.DKIM_SIGNER_PATH))
            {
                Directory.CreateDirectory(Constants.DKIM_SIGNER_PATH);
            }

            string[] asSourceFiles = Directory.GetFiles(sSourcePath);
            string[] asDestinationFiles = new string[asSourceFiles.Length];


            for (int i = 0; i < asSourceFiles.Length; i++)
            {
                string sFile =  Path.GetFileName(asSourceFiles[i]);
                asDestinationFiles[i] = Path.Combine(Constants.DKIM_SIGNER_PATH,sFile);
            }

            bReturn = FileOperation.CopyFiles(this.Handle, asSourceFiles, asDestinationFiles, true, "Test", out bAnyOperationsAborted);

            return bReturn && !bAnyOperationsAborted;

            //try
            //{
                //FileSystem.CopyDirectory(sSource, Constants.DKIM_SIGNER_PATH, UIOption.AllDialogs, UICancelOption.ThrowException);
            //}
            //catch (Exception)
            //{
            //    bStatus = false;
            //}

            // First copy the configuration executable from Src\Configuration.DkimSigner\bin\Release to the destination:
            //string sourcePath = Path.Combine(sTempPath, @"Src\Configuration.DkimSigner\bin\Release");
            //string destPath = Path.Combine(sInstallPath, "Configuration");
            //string ret = copyAllFiles(sourcePath, destPath);

            //now copy the agent .dll from e.g. \Src\Exchange.DkimSigner\bin\Exchange 2007 SP3 to the destination
            //Get source directory for installed Exchange version:
            //string libDir = directoryFromExchangeVersion();
            //sourcePath = Path.Combine(sTempPath, Path.Combine(@"Src\Exchange.DkimSigner\bin\", libDir));
            //return copyAllFiles(sourcePath, sInstallPath);

            //return bStatus;
        }

        private void StopService()
        {
            try
            {
                this.oExchange.StopTransportService();
            }
            catch (ExchangeServerException) { }
        }

        /*private string directoryFromExchangeVersion()
        {
            string sExchangeVersion = null;

            Version oFullVersion = ExchangeHelper.getExchangeVersion();
            if (oFullVersion != null)
            {
                string sVersion = oFullVersion.ToString();

                if (sVersion.StartsWith("8.3.*"))
                    sExchangeVersion = "Exchange 2007 SP3";
                else if (sVersion.StartsWith("14.0."))
                    sExchangeVersion = "Exchange 2010";
                else if (sVersion.StartsWith("14.1."))
                    sExchangeVersion = "Exchange 2010 SP1";
                else if (sVersion.StartsWith("14.2."))
                    sExchangeVersion = "Exchange 2010 SP2";
                else if (sVersion.StartsWith("14.3."))
                    sExchangeVersion = "Exchange 2010 SP3";
                else if (sVersion.StartsWith("15.0.516.32"))
                    sExchangeVersion = "Exchange 2013";
                else if (sVersion.StartsWith("15.0.620.29"))
                    sExchangeVersion = "Exchange 2013 CU1";
                else if (sVersion.StartsWith("15.0.712.24"))
                    sExchangeVersion = "Exchange 2013 CU2";
                else if (sVersion.StartsWith("15.0.775.38"))
                    sExchangeVersion = "Exchange 2013 CU3";
                else if (sVersion.StartsWith("15.0.847.32"))
                    sExchangeVersion = "Exchange 2013 SP1 CU4";
                else if (sVersion.StartsWith("15.0.913.22"))
                    sExchangeVersion = "Exchange 2013 SP1 CU5";
            }

            return sExchangeVersion;
        }*/

        /// <summary>
        /// 
        /// </summary>
        private void InstallAgent()
        {
            if (this.txtVersionFile.Text != string.Empty || this.cbVersionWeb.SelectedIndex > -1)
            {
                if (!this.oExchange.IsTransportServiceInstalled())
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
                        this.oExchange.InstallDkimTransportAgent();
                    }
                    catch (ExchangeServerException) { }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartService()
        {
            try
            {
                this.oExchange.StartTransportService();
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
            if (this.txtVersionFile.Text != string.Empty || this.cbVersionWeb.SelectedIndex > -1)
            {
                bool bStatus = true;
                string sZipPath = string.Empty;

                this.cbVersionWeb.Enabled = false;
                this.cbxPrereleases.Enabled = false;
                this.btBrowse.Enabled = false;
                this.btInstall.Enabled = false;

                //
                // Download required files
                //
                this.lbDownloadFiles.Enabled = true;

                if (this.cbVersionWeb.SelectedIndex > -1)
                {
                    sZipPath = Guid.NewGuid().ToString() + ".zip";
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

                // ZIPExtract

                if (bStatus)
                {
                    bStatus = this.CopyFiles(@"C:\Users\Alexandre Laroche\Downloads\");
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btClose_Click(object sender, EventArgs e)
        {
            // Start new installed DKIM Signer Configuration GUI
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

            this.Close();
        }
    }
}