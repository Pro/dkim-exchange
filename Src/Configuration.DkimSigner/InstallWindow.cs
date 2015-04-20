using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;

using Configuration.DkimSigner.GitHub;
using Configuration.DkimSigner.Exchange;
using Configuration.DkimSigner.FileIO;
using System.Diagnostics;
using Microsoft.Win32;

namespace Configuration.DkimSigner
{
    public partial class InstallWindow : Form
    {
        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/

        private List<Release> aoVersionAvailable = null;
        private string sExchangeVersion = null;

        private enum ThreadIdentifier { ExchangeInstalled, DkimSignerAvailable };
        private IDictionary<ThreadIdentifier, Thread> athRunning = null;

        delegate void EnablePrereleasesCallback(bool sStatus);
        delegate void RefreshVersionWebItemsCallback();
        delegate void RefreshInstallButtonCallback();

        private string upgradeZipUrl = null;
        private bool isInstall;

        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public InstallWindow(bool isInstall, string upgradeZipUrl)
        {
            this.InitializeComponent();

            this.athRunning = new Dictionary<ThreadIdentifier, Thread>();

            this.picStopService.Image = null;
            this.picCopyFiles.Image = null;
            this.picInstallAgent.Image = null;
            this.picStartService.Image = null;
            this.upgradeZipUrl = upgradeZipUrl;
            this.isInstall = isInstall;
        }

        /**********************************************************/
        /************************* Events *************************/
        /**********************************************************/

        private void InstallWindow_Load(object sender, EventArgs e)
        {
            if (!isInstall)
            {
                this.Text = "Exchange DkimSigner - Upgrade";
                pnlInstall.Hide();
            } else if (isInstall && upgradeZipUrl != null)
            {
                this.Text = "Exchange DkimSigner - Install";
                pnlInstall.Hide();
            }
            else
            {
                this.UpdateDkimSignerAvailable();
                lblWait.Hide();
            }

            this.UpdateExchangeInstalled();
        }

        private void InstallWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // IF any thread running, we stop them before exit
            foreach (KeyValuePair<ThreadIdentifier, Thread> oTemp in this.athRunning)
            {
                Thread oTh = oTemp.Value;
                if (oTh != null && oTh.ThreadState == System.Threading.ThreadState.Running)
                {
                    oTh.Join();
                }
            }
            this.athRunning.Clear();
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
            RefreshInstallButton();
            if (this.aoVersionAvailable != null)
            {
                if (this.aoVersionAvailable.Count > 0)
                {
                    foreach (Release oVersionAvailable in this.aoVersionAvailable)
                    {
                        this.cbVersionWeb.Items.Add(oVersionAvailable.TagName);
                    }
                    
                    this.cbVersionWeb.Enabled = true;
                }
                else
                {
                    MessageBox.Show(this, "No release information from the Web available.", "Version", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.cbVersionWeb.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show(this, "Could not obtain release information from the Web. Check your Internet connection or retry later.", "Error fetching version", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                this.cbVersionWeb.Enabled = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateDkimSignerAvailable()
        {
            if (this.athRunning.ContainsKey(ThreadIdentifier.DkimSignerAvailable))
                return; // thread already started

            Thread oTh3 = new Thread(() => {
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
                
                this.athRunning.Remove(ThreadIdentifier.DkimSignerAvailable); 
            
            });
            this.athRunning.Add(ThreadIdentifier.DkimSignerAvailable, oTh3);
            try { oTh3.Start(); }
            catch (ThreadAbortException) { }

            
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateExchangeInstalled()
        {

            if (this.athRunning.ContainsKey(ThreadIdentifier.ExchangeInstalled))
                return; // thread already started
            Thread oTh3 = new Thread(() => {
                this.sExchangeVersion = ExchangeServer.GetInstalledVersion();

                if (this.btInstall.InvokeRequired)
                {
                    RefreshInstallButtonCallback d = new RefreshInstallButtonCallback(this.RefreshInstallButton);
                    this.Invoke(d);
                }
                else
                {
                    this.RefreshInstallButton();
                }
                this.athRunning.Remove(ThreadIdentifier.ExchangeInstalled);
            });
            this.athRunning.Add(ThreadIdentifier.ExchangeInstalled, oTh3);
            try { oTh3.Start(); }
            catch (ThreadAbortException) { }

        }

        private void RefreshInstallButton()
        {
            btInstall.Enabled = (this.sExchangeVersion != null && (this.aoVersionAvailable != null || txtVersionFile.Text.Length > 0));
            if (upgradeZipUrl != null || !isInstall)
            {
                onButtonInstall();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sZipPath"></param>
        /// <returns></returns>
        public static bool DownloadFile(string url, string dest)
        {
            DownloadProgressWindow oDpw = new DownloadProgressWindow(url, dest);
            bool success = (oDpw.ShowDialog() == DialogResult.OK);
            oDpw.Dispose();
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sZipPath"></param>
        /// <returns></returns>
        public static bool ExtractFiles(string sZipPath, string sExtractPath)
        {
            bool bStatus = true;
            if (!Directory.Exists(sExtractPath))
                Directory.CreateDirectory(sExtractPath);
            try
            {
                ZipFile.ExtractToDirectory(sZipPath, sExtractPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ZIP Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bStatus = false;
            }

            return bStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="asSourcePath"></param>
        /// <returns></returns>
        public bool CopyFiles(List<string> asSourcePath)
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
        private static bool StopService()
        {
            try
            {
                ExchangeServer.StopTransportService();
                return true;
            }
            catch (ExchangeServerException ex)
            {
                MessageBox.Show("Could not stop MSExchangeTransport Service:\n" + ex.Message, "Error stopping service", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool InstallAgent()
        {
            if (ExchangeServer.IsTransportServiceInstalled())
            {
                // First make sure the following Registry key exists
                // HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM

                bool createEventLogSource;
                if (EventLog.SourceExists(Constants.DKIM_SIGNER_EVENTLOG_SOURCE))
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM", false);
                    if (key == null || key.GetValue("EventMessageFile") == null)
                    {
                        // make sure we recreate the event log source to fix messageResourceFile from versions previous to 2.0.0
                        EventLog.DeleteEventSource(Constants.DKIM_SIGNER_EVENTLOG_SOURCE);
                        createEventLogSource = true;
                    }
                    else
                    {
                        createEventLogSource = false;
                    }
                }
                else
                {
                    createEventLogSource = true;
                }

                // Create the event source if it does not exist.
                if (createEventLogSource)
                {
                    // Create a new event source for the custom event log 

                    EventSourceCreationData mySourceData = new EventSourceCreationData(Constants.DKIM_SIGNER_EVENTLOG_SOURCE, "Application");

                    // Set the specified file as the resource
                    // file for message text, category text, and 
                    // message parameter strings.  

                    // set dummy file. We don't have our own message files. See http://www.codeproject.com/Articles/4166/Using-MC-exe-message-resources-and-the-NT-event-lo if own file needed.
                    mySourceData.MessageResourceFile = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\EventLogMessages.dll";
                    //mySourceData.CategoryResourceFile = messageFile;
                    //mySourceData.CategoryCount = CategoryCount;
                    //mySourceData.ParameterResourceFile = messageFile;


                    EventLog.CreateEventSource(mySourceData);
                }

                // Install DKIM Transport Agent in Microsoft Exchange 
                try
                {
                    ExchangeServer.InstallDkimTransportAgent();
                    return true;
                }
                catch (ExchangeServerException ex)
                {
                    MessageBox.Show("Could not install DKIM Agent:\n" + ex.Message, "Error installing agent", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("MSExchangeTransport Service not found on this machine. Couldn't install DKIM Agent.", "Error installing agent", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static bool StartService()
        {
            try
            {
                ExchangeServer.StartTransportService();
                return true;
            }
            catch (ExchangeServerException ex) {
                MessageBox.Show("Could not start MSExchangeTransport Service:\n" + ex.Message, "Error starting service", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;            
            }
        }

        public static string getSourceDirectoryForVersion(string exchangeVersion)
        {
            foreach(KeyValuePair<string, string> entry in Constants.DKIM_SIGNER_VERSION_DIRECTORY)
            {
                if (exchangeVersion.StartsWith(entry.Key))
                {
                    return entry.Value;
                }
            }

            MessageBox.Show("Your Microsoft Exchange version isn't supported by the DKIM agent: " + exchangeVersion, "Version not supported", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }


        private void onButtonInstall()
        {
            lblWait.Hide();
            if (this.sExchangeVersion != null && this.sExchangeVersion != "Not installed")
            {
                if (this.txtVersionFile.Text != string.Empty || this.cbVersionWeb.SelectedIndex > -1 || !isInstall || (isInstall && upgradeZipUrl != null))
                {
                    bool bStatus = true;

                    this.cbVersionWeb.Enabled = false;
                    this.cbxPrereleases.Enabled = false;
                    this.btBrowse.Enabled = false;
                    this.btInstall.Enabled = false;
                    this.btClose.Enabled = false;

                    string downloadRootDir = null;

                    if (isInstall || (!isInstall && upgradeZipUrl != null))
                    {
                        // either the user selected a specific version to install or the .exe was called with '--upgrade http://url_to_download.zip'
                        // thus we first need to download and extract the files.

                        string sZipPath = string.Empty;

                        string extractPath = null;
                        //
                        // Download required files
                        //

                        this.lbDownloadFiles.Enabled = true;

                        if (upgradeZipUrl != null)
                        {
                            //check if the provided zip url is a local file or if it first needs to be downloaded.
                            if (!Uri.IsWellFormedUriString(upgradeZipUrl, UriKind.RelativeOrAbsolute))
                            {
                                //upgradeZipUrl contains a local path, thus no download needed
                                sZipPath = upgradeZipUrl;
                                extractPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                            }
                            else
                            {
                                sZipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".zip");
                                extractPath = Path.Combine(Path.GetDirectoryName(sZipPath), Path.GetFileNameWithoutExtension(sZipPath));

                                bStatus = DownloadFile(upgradeZipUrl, sZipPath);
                            }
                        }
                        else if (this.cbVersionWeb.SelectedIndex > -1)
                        {
                            sZipPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".zip");
                            extractPath = Path.Combine(Path.GetDirectoryName(sZipPath), Path.GetFileNameWithoutExtension(sZipPath));
                            string sZipballUrl = string.Empty;

                            foreach (Release oRelease in aoVersionAvailable)
                            {
                                if (oRelease.TagName == cbVersionWeb.Text)
                                {
                                    sZipballUrl = oRelease.ZipballUrl;
                                    break;
                                }
                            }
                            bStatus = DownloadFile(sZipballUrl, sZipPath);
                        }
                        else
                        {
                            sZipPath = this.txtVersionFile.Text;
                            extractPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                        }

                        this.picDownloadFiles.Image = bStatus ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                        this.Refresh();


                        if (bStatus)
                        {
                            bStatus = ExtractFiles(sZipPath, extractPath);
                        }

                        // copy root directory is one directory below extracted zip:
                        if (bStatus)
                        {
                            string[] contents = Directory.GetDirectories(extractPath);
                            if (contents.Length == 0)
                            {
                                MessageBox.Show(this, "Downloaded .zip is empty. Please try again.", "Empty download", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                bStatus = false;
                            }
                            downloadRootDir = contents[0];
                        }

                    }
                    else
                    {
                        // the download root dir is one of the parent folders of this executable.
                        // we are currently in Src\Configuration.DkimSigner\bin\Release\ thus go up 4 folders
                        downloadRootDir = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\..\..\"));
                    }

                    //
                    // Copy required files
                    //

                    string exchangeSourceDir = null;
                    if (bStatus){
                        exchangeSourceDir = getSourceDirectoryForVersion(this.sExchangeVersion);
                        bStatus = (exchangeSourceDir != null);
                    }




                    //
                    // Stop Microsoft Exchange Transport Service
                    //

                    this.lbStopService.Enabled = true;

                    if (bStatus)
                    {
                        bStatus = StopService();
                    }

                    this.picStopService.Image = bStatus ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                    this.Refresh();

                    this.lbCopyFiles.Enabled = true;
                    if (bStatus)
                    {
                        List<string> asCopyFilePath = new List<string>();
                        asCopyFilePath.Add(Path.Combine(downloadRootDir, @"Src\Configuration.DkimSigner\bin\Release"));
                        asCopyFilePath.Add(Path.Combine(downloadRootDir, Path.Combine(@"Src\Exchange.DkimSigner\bin\" + exchangeSourceDir)));
                        bStatus = this.CopyFiles(asCopyFilePath);

                    }

                    this.picCopyFiles.Image = bStatus ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                    this.Refresh();

                    //
                    // Instal DKIM Signer agent in Microsoft Exchange Transport Service
                    //

                    this.lbInstallAgent.Enabled = true;

                    if (bStatus)
                    {
                        bStatus = InstallAgent();
                    }

                    this.picInstallAgent.Image = bStatus ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                    this.Refresh();

                    //
                    // Start Microsoft Exchange Transport Service
                    //

                    this.lbStartService.Enabled = true;

                    if (bStatus)
                    {
                        bStatus = StartService();
                    }

                    this.picStartService.Image = bStatus ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                    this.Refresh();

                    //
                    // Done
                    //

                    this.btClose.Enabled = true;
                    if (bStatus)
                    {
                        MessageBox.Show(this, "Successfully installed/upgraded DKIM Signer. You can now close this window.", "Installed/Upgraded", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(this, "You have to select a version to install from the Web or a ZIP file.", "Select version", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show(this, "Microsoft Exchange server must be installed before attempt to install DKIM agent.", "Exchange not installed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /**********************************************************/
        /********************** Button click **********************/
        /**********************************************************/

        private void btBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog oFileDialog = new OpenFileDialog())
            {

                oFileDialog.FileName = "dkim-exchange.zip";
                oFileDialog.Filter = "ZIP files|*.zip";
                oFileDialog.Title = "Select the .zip file downloaded from github.com";

                if (oFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.cbVersionWeb.SelectedIndex = -1;

                    this.txtVersionFile.Text = oFileDialog.FileName;
                    RefreshInstallButton();
                }
            }
        }

        private void btInstall_Click(object sender, EventArgs e)
        {
            onButtonInstall();
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
                if (MessageBox.Show(this, "Do you want to start DKIM Signer configuration tool?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sPathExec = Path.Combine(Constants.DKIM_SIGNER_PATH, Constants.DKIM_SIGNER_CONFIGURATION_EXE);
                    if (File.Exists(sPathExec))
                    {
                        System.Diagnostics.Process.Start(sPathExec);
                    }
                    else
                    {
                        MessageBox.Show(this, "Couldn't find 'Configuration.DkimSigner.exe' in \n" + Constants.DKIM_SIGNER_PATH, "Exec error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.Close();
        }
    }
}