using Configuration.DkimSigner.GitHub;
using Configuration.DkimSigner.Exchange;
using Configuration.DkimSigner.FileIO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Configuration.DkimSigner
{
    public partial class InstallWindow : Form
    {
        // ##########################################################
        // ##################### Variables ##########################
        // ##########################################################

        private TransportService transportService = null;
        private AutoResetEvent transportServiceActionCompleted = null;
        private string transportServiceSuccessStatus = null;

        private List<Release> versionAvailable = null;
        private string exchangeVersion = null;

        private string zipUrl = null;
        /// <summary>
        /// If this member is set to true, the files accomanying this .exe should be installed.
        /// </summary>
        private bool performInstall;

        // ##########################################################
        // ##################### Construtor #########################
        // ##########################################################

        public InstallWindow(bool performInstall = false, string zipUrl = null)
        {
            this.InitializeComponent();

            this.performInstall = performInstall;
            this.zipUrl = zipUrl;
        }

        // ##########################################################
        // ####################### Events ###########################
        // ##########################################################

        private void InstallWindow_Load(object sender, EventArgs e)
        {
            this.CheckExchangeInstalled();       
            try
            {
                this.transportServiceActionCompleted = new AutoResetEvent(false);
                this.transportService = new TransportService();
                this.transportService.StatusChanged += new EventHandler(this.transportService_StatusUptated);
            }
            catch (ExchangeServerException) { }

            if (this.zipUrl == null && !this.performInstall)
            {
                this.CheckDkimSignerAvailable();
            }
            else
            {
                // the dkim signer should be installed directly. Disable selection
                this.gbSelectVersionToInstall.Enabled = false;
            }
        }

        private void transportService_StatusUptated(object sender, EventArgs e)
        {
            if (this.transportServiceSuccessStatus != null && this.transportServiceSuccessStatus == this.transportService.GetStatus())
            {
                this.transportServiceActionCompleted.Set();
                this.transportServiceSuccessStatus = null;
            }
        }

        private void cbVersionWeb_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.txtVersionFile.Clear();
            this.btInstall.Enabled = this.btInstallRefresh();
        }

        private void cbxPrereleases_CheckedChanged(object sender, EventArgs e)
        {
            this.btInstall.Enabled = false;
            this.CheckDkimSignerAvailable();
        }

        // ##########################################################
        // ################# Internal functions #####################
        // ##########################################################

        /// <summary>
        /// Thread safe function for the thread DkimSignerAvailable
        /// </summary>
        private async void CheckDkimSignerAvailable()
        {
            this.cbxPrereleases.Enabled = false;
            
            await Task.Run(() => this.versionAvailable = ApiWrapper.GetAllRelease(cbxPrereleases.Checked, new Version("2.0.0")));
            
            this.cbxPrereleases.Enabled = true;

            this.cbVersionWeb.Items.Clear();
            if (this.versionAvailable != null)
            {
                if (this.versionAvailable.Count > 0)
                {
                    foreach (Release oVersionAvailable in this.versionAvailable)
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

            this.cbxPrereleases.Enabled = true;
        }

        /// <summary>
        /// Check the Microsoft Exchange Transport Service Status
        /// </summary>
        private async void CheckExchangeInstalled()
        {
            await Task.Run(() => this.exchangeVersion = ExchangeServer.GetInstalledVersion());

            this.lblExchangeVersionWait.Hide();
            this.btInstall.Enabled = this.btInstallRefresh();

            // If the source for install have been specified in command line
            if (this.zipUrl != null || this.performInstall)
            {
                this.Install();
            }
        }

        private bool btInstallRefresh()
        {
            return this.exchangeVersion != null && this.exchangeVersion != "Not installed" && (this.cbVersionWeb.Text != string.Empty || this.txtVersionFile.Text != string.Empty);
        }

        private void Install()
        {
            this.btClose.Enabled = false;
            this.gbSelectVersionToInstall.Enabled = false;
            this.gbInstallStatus.Enabled = true;
            this.Refresh();

            // ###########################################
            // ### IS Exchange version supported?      ###
            // ###########################################

            string agentExchangeVersionPath = "";

            foreach (KeyValuePair<string, string> entry in Constants.DKIM_SIGNER_VERSION_DIRECTORY)
            {
                if (exchangeVersion.StartsWith(entry.Key))
                {
                    agentExchangeVersionPath = entry.Value;
                    break;
                }
            }

            if (agentExchangeVersionPath == "")
            {
                MessageBox.Show(this, "Your Microsoft Exchange version isn't supported by the DKIM agent: " + exchangeVersion, "Version not supported", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.lbDownloadFiles.Enabled = true;
            }

            // path which is the base for copying the files. Should be the root of the downloaded .zip file.
            string extractPath;

            if (this.zipUrl != null) {
                // ###########################################
                // ### Download files                      ###
                // ###########################################

                string zipFile = "";

                if (Uri.IsWellFormedUriString(this.zipUrl, UriKind.RelativeOrAbsolute))
                {
                    zipFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".zip");

                    DownloadProgressWindow oDpw = new DownloadProgressWindow(this.zipUrl, zipFile);
                    try {
                        if (oDpw.ShowDialog(this) == DialogResult.OK)
                        {
                            this.lbExtractFiles.Enabled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "Couldn't initialize download progress window:\n" + ex.Message, "Error showing download progress", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        oDpw.Dispose();
                    }
                
                }
                else
                {
                    if (File.Exists(this.zipUrl) && Path.GetExtension(this.zipUrl) == ".zip")
                    {
                        zipFile = this.zipUrl;
                        this.lbExtractFiles.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show(this, "The URL or the path to the ZIP file is invalid. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                this.picDownloadFiles.Image = this.lbExtractFiles.Enabled ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                this.Refresh();

                // ###########################################
                // ### Extract files                       ###
                // ###########################################

                extractPath = Path.Combine(Path.GetDirectoryName(zipFile), Path.GetFileNameWithoutExtension(zipFile));

                if (this.lbExtractFiles.Enabled)
                {
                    if (!Directory.Exists(extractPath))
                    {
                        Directory.CreateDirectory(extractPath);
                    }

                    try
                    {
                        ZipFile.ExtractToDirectory(zipFile, extractPath);

                        // copy root directory is one directory below extracted zip:
                        string[] contents = Directory.GetDirectories(extractPath);
                        if (contents.Length == 1)
                        {
                            extractPath = Path.Combine(extractPath, contents[0]);
                            this.lbStopService.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show(this, "Downloaded .zip is invalid. Please try again.", "Invalid download", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }                   
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message, "ZIP Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                this.picExtractFiles.Image = this.lbStopService.Enabled ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
                this.Refresh();
            } else {
                // the files are already downloaded and in the same directory as this .exe file

                // the executable is within: \Src\Configuration.DkimSigner\bin\Release so we need to go up a few directories
                DirectoryInfo dir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
                string[] expectedDirs = {"Release", "bin", "Configuration.DkimSigner", "Src"};
                bool sanityFail = false;

                foreach (string str in expectedDirs) {
                    if (!dir.Name.Equals(str)) {
                        sanityFail = true;
                        break;
                    } else {
                        dir = dir.Parent;
                    }
                }

                this.lbDownloadFiles.Enabled = !sanityFail;
                this.lbExtractFiles.Enabled = !sanityFail;
                this.lbStopService.Enabled = !sanityFail;

                if (sanityFail) {
                                        
                    this.picDownloadFiles.Image = this.statusImageList.Images[1];
                    this.picExtractFiles.Image = this.statusImageList.Images[1];

                    this.Refresh();
                    MessageBox.Show(this, @"Failed to determine copy root directory.\nThis executable is expected to be in the subpath: \Src\Configuration.DkimSigner\bin\Release", "ZIP Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                } else {                    
                    this.picDownloadFiles.Image = this.statusImageList.Images[0];
                    this.picExtractFiles.Image = this.statusImageList.Images[0];
                    this.Refresh();
                }

                extractPath = dir.FullName;

            }
                        

            // ###########################################
            // ### Stop Microsoft Transport service    ###
            // ###########################################

            if (this.lbStopService.Enabled)
            {
                if (this.transportService.GetStatus() != "Stopped")
                {
                    this.transportServiceSuccessStatus = "Stopped";
                    this.transportService.Do(TransportServiceAction.Stop);
                    this.lbCopyFiles.Enabled = this.transportServiceActionCompleted.WaitOne();
                }
                else
                {
                    this.lbCopyFiles.Enabled = true;
                }
            }

            this.picStopService.Image = this.lbCopyFiles.Enabled ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
            this.Refresh();

            // ###########################################
            // ### Copy required files                 ###
            // ###########################################

            if (this.lbCopyFiles.Enabled)
            {
                List<string> filesToCopy = new List<string>();
                filesToCopy.Add(Path.Combine(extractPath, @"Src\Configuration.DkimSigner\bin\Release"));
                filesToCopy.Add(Path.Combine(extractPath, Path.Combine(@"Src\Exchange.DkimSigner\bin\" + agentExchangeVersionPath)));

                // IF the directory "C:\Program Files\Exchange DkimSigner" doesn't exist, create it 
                if (!Directory.Exists(Constants.DKIM_SIGNER_PATH))
                {
                    Directory.CreateDirectory(Constants.DKIM_SIGNER_PATH);
                }

                // Generate list of source files
                string[] sourceFiles = new string[0];
                foreach (string sourcePath in filesToCopy)
                {
                    string[] asTemp = Directory.GetFiles(sourcePath);

                    Array.Resize(ref sourceFiles, sourceFiles.Length + asTemp.Length);
                    Array.Copy(asTemp, 0, sourceFiles, sourceFiles.Length - asTemp.Length, asTemp.Length);
                }

                // Generate list of destinations files
                string[] destinationFiles = new string[sourceFiles.Length];
                for (int i = 0; i < sourceFiles.Length; i++)
                {
                    string sFile = Path.GetFileName(sourceFiles[i]);
                    destinationFiles[i] = Path.Combine(Constants.DKIM_SIGNER_PATH, sFile);
                }

                bool bAnyOperationsAborted = false;
                bool bReturn = FileOperation.CopyFiles(this.Handle, sourceFiles, destinationFiles, true, "Copy files", out bAnyOperationsAborted);

                this.lbInstallAgent.Enabled = bReturn && !bAnyOperationsAborted;
            }

            this.picCopyFiles.Image = this.lbInstallAgent.Enabled ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
            this.Refresh();

            // ###########################################
            // ### Install DKIM Signer Excange Agent   ###
            // ###########################################

            if (this.lbInstallAgent.Enabled)
            {
                try
                {
                    // First make sure the following Registry key exists HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM
                    if (EventLog.SourceExists(Constants.DKIM_SIGNER_EVENTLOG_SOURCE))
                    {
                        // Make sure we recreate the event log source to fix messageResourceFile from versions previous to 2.0.0
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(Constants.DKIM_SIGNER_EVENTLOG_REGISTRY, false);
                        if (key == null || key.GetValue("EventMessageFile") == null)
                        {
                            // Delete the event source for the custom event log 
                            EventLog.DeleteEventSource(Constants.DKIM_SIGNER_EVENTLOG_SOURCE);

                            // Create a new event source for the custom event log 
                            EventSourceCreationData mySourceData = new EventSourceCreationData(Constants.DKIM_SIGNER_EVENTLOG_SOURCE, "Application");
                            mySourceData.MessageResourceFile = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\EventLogMessages.dll";
                            EventLog.CreateEventSource(mySourceData);
                        }
                    }
                    else
                    {
                        // Create a new event source for the custom event log 
                        EventSourceCreationData mySourceData = new EventSourceCreationData(Constants.DKIM_SIGNER_EVENTLOG_SOURCE, "Application");
                        mySourceData.MessageResourceFile = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\EventLogMessages.dll";
                        EventLog.CreateEventSource(mySourceData);
                    }

                    // Install DKIM Transport Agent in Microsoft Exchange 
                    ExchangeServer.InstallDkimTransportAgent();

                    this.lbStartService.Enabled = true;
                }
                catch (ExchangeServerException ex)
                {
                    MessageBox.Show(this, "Could not install DKIM Agent:\n" + ex.Message, "Error installing agent", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            this.picInstallAgent.Image = this.lbStartService.Enabled ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
            this.Refresh();
            
            // ###########################################
            // ### Start Microsoft Transport service   ###
            // ###########################################

            if (this.lbStartService.Enabled)
            {
                this.transportServiceSuccessStatus = "Running";
                this.transportService.Do(TransportServiceAction.Start);
                this.picStartService.Image = this.transportServiceActionCompleted.WaitOne() ? this.statusImageList.Images[0] : this.statusImageList.Images[1];
            }
            else
            {
                this.picStartService.Image = this.statusImageList.Images[1];
            }

            this.Refresh();

            this.btClose.Enabled = true;
        }

        // ###########################################################
        // ###################### Button click #######################
        // ###########################################################

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
                    this.btInstall.Enabled = this.btInstallRefresh();
                }
            }
        }

        private void btInstall_Click(object sender, EventArgs e)
        {
            this.zipUrl = this.txtVersionFile.Text != string.Empty ? this.txtVersionFile.Text : this.versionAvailable[this.cbVersionWeb.SelectedIndex].ZipballUrl;
            this.Install();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            // Start new installed DKIM Signer Configuration GUI
            System.Drawing.Image statusImg = this.statusImageList.Images[0];
            if (this.btInstall.Enabled == false && this.picStopService.Image == statusImg)
            {
                if (MessageBox.Show(this, "Do you want to start DKIM Signer configuration tool?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sPathExec = Path.Combine(Constants.DKIM_SIGNER_PATH, Constants.DKIM_SIGNER_CONFIGURATION_EXE);
                    if (File.Exists(sPathExec))
                    {
                        Process.Start(sPathExec);
                    }
                    else
                    {
                        MessageBox.Show(this, "Couldn't find 'Configuration.DkimSigner.exe' in \n" + Constants.DKIM_SIGNER_PATH, "Exec error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            statusImg.Dispose();

            this.Close();
        }
    }
}