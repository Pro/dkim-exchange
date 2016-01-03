using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Configuration.DkimSigner.Exchange;
using Configuration.DkimSigner.FileIO;
using Configuration.DkimSigner.GitHub;
using Microsoft.Win32;

namespace Configuration.DkimSigner
{
    public partial class InstallWindow : Form
    {
        // ##########################################################
        // ##################### Variables ##########################
        // ##########################################################

        private TransportService transportService;
        private AutoResetEvent transportServiceActionCompleted;
        private string transportServiceSuccessStatus;

        private List<Release> versionAvailable;
        private string exchangeVersion;

        private string zipUrl;
        /// <summary>
        /// If this member is set to true, the files accomanying this .exe should be installed.
        /// </summary>
        private bool performInstall;

        // ##########################################################
        // ##################### Construtor #########################
        // ##########################################################

        public InstallWindow(bool performInstall = false, string zipUrl = null)
        {
            InitializeComponent();

            this.performInstall = performInstall;
            this.zipUrl = zipUrl;
        }

        // ##########################################################
        // ####################### Events ###########################
        // ##########################################################

        private void InstallWindow_Load(object sender, EventArgs e)
        {
            CheckExchangeInstalled();
            try
            {
                transportServiceActionCompleted = new AutoResetEvent(false);
                transportService = new TransportService();
                transportService.StatusChanged += transportService_StatusUptated;
            }
            catch (ExchangeServerException) { }

            if (zipUrl == null && !performInstall)
            {
                CheckDkimSignerAvailable();
            }
            else
            {
                // the dkim signer should be installed directly. Disable selection
                gbSelectVersionToInstall.Enabled = false;
            }
        }

        private void transportService_StatusUptated(object sender, EventArgs e)
        {
            if (transportServiceSuccessStatus != null && transportServiceSuccessStatus == transportService.GetStatus())
            {
                transportServiceActionCompleted.Set();
                transportServiceSuccessStatus = null;
            }
        }

        private void cbVersionWeb_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtVersionFile.Clear();
            btInstall.Enabled = IsBtInstallEnabled();
        }

        private void cbxPrereleases_CheckedChanged(object sender, EventArgs e)
        {
            btInstall.Enabled = false;
            CheckDkimSignerAvailable();
        }

        // ##########################################################
        // ################# Internal functions #####################
        // ##########################################################

        /// <summary>
        /// Thread safe function for the thread DkimSignerAvailable
        /// </summary>
        private async void CheckDkimSignerAvailable()
        {
            cbxPrereleases.Enabled = false;

            await Task.Run(() => versionAvailable = ApiWrapper.GetAllRelease(cbxPrereleases.Checked, new Version("2.0.0")));

            cbxPrereleases.Enabled = true;

            cbVersionWeb.Items.Clear();
            if (versionAvailable != null)
            {
                if (versionAvailable.Count > 0)
                {
                    foreach (Release oVersionAvailable in versionAvailable)
                    {
                        cbVersionWeb.Items.Add(oVersionAvailable.TagName);
                    }

                    cbVersionWeb.Enabled = true;
                }
                else
                {
                    MessageBox.Show(this, "No release information from the Web available.", "Version", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbVersionWeb.Enabled = false;
                }
            }
            else
            {
                MessageBox.Show(this, "Could not obtain release information from the Web. Check your Internet connection or retry later.", "Error fetching version", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbVersionWeb.Enabled = false;
            }

            cbxPrereleases.Enabled = true;
        }

        /// <summary>
        /// Check the Microsoft Exchange Transport Service Status
        /// </summary>
        private async void CheckExchangeInstalled()
        {
            await Task.Run(() => exchangeVersion = ExchangeServer.GetInstalledVersion());

            lblExchangeVersionWait.Hide();
            btInstall.Enabled = IsBtInstallEnabled();

            // If the source for install have been specified in command line
            if (zipUrl != null || performInstall)
            {
                Install();
            }
        }

        private bool IsBtInstallEnabled()
        {
            return exchangeVersion != null && exchangeVersion != "Not installed" && (cbVersionWeb.Text != string.Empty || txtVersionFile.Text != string.Empty);
        }

        private void Install()
        {
            btClose.Enabled = false;
            gbSelectVersionToInstall.Enabled = false;
            gbInstallStatus.Enabled = true;
            Refresh();

            // ###########################################
            // ### IS Exchange version supported?      ###
            // ###########################################

            string agentExchangeVersionPath = "";

            foreach (KeyValuePair<string, string> entry in Constants.DkimSignerVersionDirectory)
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
                lbDownloadFiles.Enabled = true;
            }

            // path which is the base for copying the files. Should be the root of the downloaded .zip file.
            string extractPath;

            if (zipUrl != null)
            {
                // ###########################################
                // ### Download files                      ###
                // ###########################################

                string zipFile = "";

                if (Uri.IsWellFormedUriString(zipUrl, UriKind.RelativeOrAbsolute))
                {
                    zipFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".zip");

                    DownloadProgressWindow oDpw = new DownloadProgressWindow(zipUrl, zipFile);
                    try
                    {
                        if (oDpw.ShowDialog(this) == DialogResult.OK)
                        {
                            lbExtractFiles.Enabled = true;
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
                    if (File.Exists(zipUrl) && Path.GetExtension(zipUrl) == ".zip")
                    {
                        zipFile = zipUrl;
                        lbExtractFiles.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show(this, "The URL or the path to the ZIP file is invalid. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                picDownloadFiles.Image = lbExtractFiles.Enabled ? statusImageList.Images[0] : statusImageList.Images[1];
                Refresh();

                // ###########################################
                // ### Extract files                       ###
                // ###########################################

                string zipDirName = Path.GetDirectoryName(zipFile);
                if (zipDirName == null)
                {
                    MessageBox.Show(this, "Invaild Zip path", "Could not extract directory from zip path: " + zipFile,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                extractPath = Path.Combine(zipDirName, Path.GetFileNameWithoutExtension(zipFile));

                if (lbExtractFiles.Enabled)
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
                            lbStopService.Enabled = true;
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

                picExtractFiles.Image = lbStopService.Enabled ? statusImageList.Images[0] : statusImageList.Images[1];
                Refresh();
            }
            else
            {
                // the files are already downloaded and in the same directory as this .exe file

                // the executable is within: \Src\Configuration.DkimSigner\bin\Release so we need to go up a few directories
                DirectoryInfo dir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory;
                if (dir == null)
                {
                    MessageBox.Show(this, "Could not get directory info for: " + Assembly.GetExecutingAssembly().Location, "Directory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string[] expectedDirs = { "Release", "bin", "Configuration.DkimSigner", "Src" };
                bool sanityFail = false;

                foreach (string str in expectedDirs)
                {
                    if (dir == null || !dir.Name.Equals(str))
                    {
                        sanityFail = true;
                        break;
                    }
                    dir = dir.Parent;
                }
                if (dir == null)
                    sanityFail = true;

                lbDownloadFiles.Enabled = !sanityFail;
                lbExtractFiles.Enabled = !sanityFail;
                lbStopService.Enabled = !sanityFail;

                if (sanityFail)
                {

                    picDownloadFiles.Image = statusImageList.Images[1];
                    picExtractFiles.Image = statusImageList.Images[1];

                    Refresh();
                    MessageBox.Show(this, @"Failed to determine copy root directory.\nThis executable is expected to be in the subpath: \Src\Configuration.DkimSigner\bin\Release", "ZIP Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                else
                {
                    picDownloadFiles.Image = statusImageList.Images[0];
                    picExtractFiles.Image = statusImageList.Images[0];
                    Refresh();
                }

                extractPath = dir.FullName;

            }


            // ###########################################
            // ### Stop Microsoft Transport service    ###
            // ###########################################

            if (lbStopService.Enabled)
            {
                if (transportService.GetStatus() != "Stopped")
                {
                    transportServiceSuccessStatus = "Stopped";
                    transportService.Do(TransportServiceAction.Stop, delegate(string msg)
                    {
                        MessageBox.Show(msg, "Service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    });
                    lbCopyFiles.Enabled = transportServiceActionCompleted.WaitOne();
                }
                else
                {
                    lbCopyFiles.Enabled = true;
                }
            }

            picStopService.Image = lbCopyFiles.Enabled ? statusImageList.Images[0] : statusImageList.Images[1];
            Refresh();

            // ###########################################
            // ### Copy required files                 ###
            // ###########################################

            if (lbCopyFiles.Enabled)
            {
                List<string> filesToCopy = new List<string>();
                filesToCopy.Add(Path.Combine(extractPath, @"Src\Configuration.DkimSigner\bin\Release"));
                filesToCopy.Add(Path.Combine(extractPath, Path.Combine(@"Src\Exchange.DkimSigner\bin\" + agentExchangeVersionPath)));

                // IF the directory "C:\Program Files\Exchange DkimSigner" doesn't exist, create it 
                if (!Directory.Exists(Constants.DkimSignerPath))
                {
                    Directory.CreateDirectory(Constants.DkimSignerPath);
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
                    destinationFiles[i] = Path.Combine(Constants.DkimSignerPath, sFile);
                }

                bool bAnyOperationsAborted;
                bool bReturn = FileOperation.CopyFiles(Handle, sourceFiles, destinationFiles, true, "Copy files", out bAnyOperationsAborted);

                lbInstallAgent.Enabled = bReturn && !bAnyOperationsAborted;
            }

            picCopyFiles.Image = lbInstallAgent.Enabled ? statusImageList.Images[0] : statusImageList.Images[1];
            Refresh();

            // ###########################################
            // ### Install DKIM Signer Excange Agent   ###
            // ###########################################

            if (lbInstallAgent.Enabled)
            {
                try
                {
                    // First make sure the following Registry key exists HKLM:\SYSTEM\CurrentControlSet\Services\EventLog\Application\Exchange DKIM
                    if (EventLog.SourceExists(Constants.DkimSignerEventlogSource))
                    {
                        // Make sure we recreate the event log source to fix messageResourceFile from versions previous to 2.0.0
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(Constants.DkimSignerEventlogRegistry, false);
                        if (key == null || key.GetValue("EventMessageFile") == null)
                        {
                            // Delete the event source for the custom event log 
                            EventLog.DeleteEventSource(Constants.DkimSignerEventlogSource);

                            // Create a new event source for the custom event log 
                            EventSourceCreationData mySourceData = new EventSourceCreationData(Constants.DkimSignerEventlogSource, "Application");
                            mySourceData.MessageResourceFile = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\EventLogMessages.dll";
                            EventLog.CreateEventSource(mySourceData);
                        }
                    }
                    else
                    {
                        // Create a new event source for the custom event log 
                        EventSourceCreationData mySourceData = new EventSourceCreationData(Constants.DkimSignerEventlogSource, "Application");
                        mySourceData.MessageResourceFile = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\EventLogMessages.dll";
                        EventLog.CreateEventSource(mySourceData);
                    }

                    // Install DKIM Transport Agent in Microsoft Exchange 
                    ExchangeServer.InstallDkimTransportAgent();

                    lbStartService.Enabled = true;
                }
                catch (ExchangeServerException ex)
                {
                    MessageBox.Show(this, "Could not install DKIM Agent:\n" + ex.Message, "Error installing agent", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            picInstallAgent.Image = lbStartService.Enabled ? statusImageList.Images[0] : statusImageList.Images[1];
            Refresh();

            // ###########################################
            // ### Start Microsoft Transport service   ###
            // ###########################################

            if (lbStartService.Enabled)
            {
                transportServiceSuccessStatus = "Running";
                transportService.Do(TransportServiceAction.Start, delegate(string msg)
                {
                    MessageBox.Show(msg, "Service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
                picStartService.Image = transportServiceActionCompleted.WaitOne() ? statusImageList.Images[0] : statusImageList.Images[1];
            }
            else
            {
                picStartService.Image = statusImageList.Images[1];
            }

            Refresh();

            btClose.Enabled = true;
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
                    cbVersionWeb.SelectedIndex = -1;
                    txtVersionFile.Text = oFileDialog.FileName;
                    btInstall.Enabled = IsBtInstallEnabled();
                }
            }
        }

        private void btInstall_Click(object sender, EventArgs e)
        {
            zipUrl = txtVersionFile.Text != string.Empty ? txtVersionFile.Text : versionAvailable[cbVersionWeb.SelectedIndex].ZipballUrl;
            Install();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            // Start new installed DKIM Signer Configuration GUI
            Image statusImg = statusImageList.Images[0];
            if (btInstall.Enabled == false && picStopService.Image == statusImg)
            {
                if (MessageBox.Show(this, "Do you want to start DKIM Signer configuration tool?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string sPathExec = Path.Combine(Constants.DkimSignerPath, Constants.DkimSignerConfigurationExe);
                    if (File.Exists(sPathExec))
                    {
                        Process.Start(sPathExec);
                    }
                    else
                    {
                        MessageBox.Show(this, "Couldn't find 'Configuration.DkimSigner.exe' in \n" + Constants.DkimSignerPath, "Exec error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            statusImg.Dispose();

            Close();
        }
    }
}