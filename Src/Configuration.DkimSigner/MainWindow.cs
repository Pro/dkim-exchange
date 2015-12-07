using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Configuration.DkimSigner.Exchange;
using Configuration.DkimSigner.GitHub;
using ConfigurationSettings;
using CSInteropKeys;
using Heijden.DNS;

namespace Configuration.DkimSigner
{
    public partial class MainWindow : Form
    {
        // ##########################################################
        // ##################### Variables ##########################
        // ##########################################################

        private delegate DialogResult ShowMessageBoxCallback(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon);

        private Settings oConfig;
        private Version dkimSignerInstalled;
        private Release dkimSignerAvailable;
        private TransportService transportService;
        private bool bDataUpdated;

        // ##########################################################
        // ##################### Construtor #########################
        // ##########################################################

        public MainWindow(bool enableDebugTab)
        {
            InitializeComponent();

            cbLogLevel.SelectedItem = "Information";
            cbKeyLength.SelectedItem = UserPreferences.Default.KeyLength.ToString();

            string version = Version.Parse(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion).ToString().Substring(0, 5);
            txtAbout.Text = "Version " + version + "\r\n\r\n" +
                                    Constants.DKIM_SIGNER_NOTICE + "\r\n\r\n" +
                                    Constants.DKIM_SIGNER_LICENCE + "\r\n\r\n" +
                                    Constants.DKIM_SIGNER_AUTHOR + "\r\n\r\n" +
                                    Constants.DKIM_SIGNER_WEBSITE;
            if (!enableDebugTab)
                tcConfiguration.TabPages["tpDebug"].Hide();
        }

        // ##########################################################
        // ####################### Events ###########################
        // ##########################################################

        /// <summary>
        /// Load information in the Windowform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            CheckExchangeInstalled();
            CheckDkimSignerAvailable();
            CheckDkimSignerInstalled();

            // Check transport service status each second
            try
            {
                transportService = new TransportService();
                transportService.StatusChanged += transportService_StatusUptated;
            }
            catch (ExchangeServerException) { }

            // Load setting from XML file
            LoadDkimSignerConfig();
        }

        /// <summary>
        /// Confirm the configuration saving before quit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the config have been change and haven't been save
            if (!CheckSaveConfig())
            {
                e.Cancel = true;
            }
            else
            {
                Hide();
                transportService.Dispose();
                transportService = null;
            }
        }

        private void transportService_StatusUptated(object sender, EventArgs e)
        {
            string sStatus = transportService.GetStatus();
            txtExchangeStatus.BeginInvoke(new Action(() => txtExchangeStatus.Text = (sStatus != null ? sStatus : "Unknown")));
        }

        private void txtExchangeStatus_TextChanged(object sender, EventArgs e)
        {
            bool IsRunning = txtExchangeStatus.Text == "Running";
            bool IsStopped = txtExchangeStatus.Text == "Stopped";

            btStartTransportService.Enabled = IsStopped;
            btStopTransportService.Enabled = IsRunning;
            btRestartTransportService.Enabled = IsRunning;
        }

        private void cbxPrereleases_CheckedChanged(object sender, EventArgs e)
        {
            txtDkimSignerAvailable.Text = "Loading ...";
            CheckDkimSignerAvailable();
        }

        private void generic_ValueChanged(object sender, EventArgs e)
        {
            bDataUpdated = true;
        }

        private void lbxHeadersToSign_SelectedIndexChanged(object sender, EventArgs e)
        {
            btHeaderDelete.Enabled = (lbxHeadersToSign.SelectedItem != null);
        }

        private void lbxDomains_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxDomains.SelectedItems.Count == 0)
            {
                txtDomainName.Text = "";
                txtDomainSelector.Text = "";
                txtDomainPrivateKeyFilename.Text = "";
                txtDomainDNS.Text = "";
                gbxDomainDetails.Enabled = false;
                cbKeyLength.Text = UserPreferences.Default.KeyLength.ToString();
            }
            else
            {
                DomainElement oSelected = (DomainElement) lbxDomains.SelectedItem;
                txtDomainName.Text = oSelected.Domain;
                txtDomainSelector.Text = oSelected.Selector;
                txtDomainPrivateKeyFilename.Text = oSelected.PrivateKeyFile;

                if (oSelected.CryptoProvider == null)
                {
                    oSelected.InitElement(Constants.DKIM_SIGNER_PATH);
                }
                else
                {
                    cbKeyLength.Text = oSelected.CryptoProvider.KeySize.ToString();
                }

                UpdateSuggestedDNS();
                txtDomainDNS.Text = "";
                gbxDomainDetails.Enabled = true;
                btDomainDelete.Enabled = true;
                btDomainSave.Enabled = false;
                bDataUpdated = false;
            }
        }

        private void txtDomainName_TextChanged(object sender, EventArgs e)
        {
            epvDomainSelector.SetError(txtDomainName, Uri.CheckHostName(txtDomainName.Text) != UriHostNameType.Dns ? "Invalid DNS name. Format: 'example.com'" : null);
            txtDNSName.Text = txtDomainSelector.Text + "._domainkey." + txtDomainName.Text + ".";
            btDomainSave.Enabled = true;
            bDataUpdated = true;
        }

        private void txtDomainSelector_TextChanged(object sender, EventArgs e)
        {
            epvDomainSelector.SetError(txtDomainSelector, !Regex.IsMatch(txtDomainSelector.Text, @"^[a-z0-9_]{1,63}(?:\.[a-z0-9_]{1,63})?$", RegexOptions.None) ? "The selector should only contain characters, numbers and underscores." : null);
            txtDNSName.Text = txtDomainSelector.Text + "._domainkey." + txtDomainName.Text + ".";
            btDomainSave.Enabled = true;
            bDataUpdated = true;
        }

        // ##########################################################
        // ################# Internal functions #####################
        // ##########################################################

        /// <summary>
        /// Check if a string is in base64
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsBase64String(string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        private DialogResult ShowMessageBox(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            DialogResult? result = null;

            if (InvokeRequired)
            {
                ShowMessageBoxCallback c = ShowMessageBox;
                result = Invoke(c, title, message, buttons, icon) as DialogResult?;
            }
            else
            {
                result = MessageBox.Show(this, message, title, buttons, icon);
            }

            if (result == null)
            {
                throw new Exception("Unexpected error from MessageBox.");
            }

            return (DialogResult) result;
        }

        /// <summary>
        /// Check the Microsoft Exchange Transport Service Status
        /// </summary>
        private async void CheckExchangeInstalled()
        {
            string version = "Unknown";

            ExchangeServerException ex = null;
            await Task.Run(() => { try { version = ExchangeServer.GetInstalledVersion(); } catch (ExchangeServerException e) { ex = e; } });
            
            if (ex != null)
            {
                ShowMessageBox("Exchange Version Error", "Couldn't determine installed Exchange Version: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtExchangeInstalled.Text = version;

            // Uptade Microsft Exchange Transport Service stuatus
            btConfigureTransportService.Enabled = (version != null && version != "Not installed");
            if (!btConfigureTransportService.Enabled)
            {
                txtExchangeStatus.Text = "Unavailable";
            }
        }

        /// <summary>
        /// Thread safe function for the thread DkimSignerInstalled
        /// </summary>
        private async void CheckDkimSignerInstalled()
        {
            Version oDkimSignerInstalled = null;

            // Check if DKIM Agent is in C:\Program Files\Exchange DkimSigner and get version of DLL
            await Task.Run(() => { try { oDkimSignerInstalled = Version.Parse(FileVersionInfo.GetVersionInfo(Path.Combine(Constants.DKIM_SIGNER_PATH, Constants.DKIM_SIGNER_AGENT_DLL)).ProductVersion); } catch (Exception) { } });

            // Check if DKIM agent have been load in Exchange
            if (oDkimSignerInstalled != null)
            {
                bool IsDkimAgentTransportInstalled = false;

                await Task.Run(() => { try { IsDkimAgentTransportInstalled = !ExchangeServer.IsDkimAgentTransportInstalled(); } catch (Exception) { } });

                if (IsDkimAgentTransportInstalled)
                {
                    oDkimSignerInstalled = null;
                }
            }

            txtDkimSignerInstalled.Text = (oDkimSignerInstalled != null ? oDkimSignerInstalled.ToString() : "Not installed");
            btConfigureTransportService.Enabled = (oDkimSignerInstalled != null);
            dkimSignerInstalled = oDkimSignerInstalled;

            SetUpgradeButton();
        }

        /// <summary>
        /// Thread safe function for the thread DkimSignerAvailable
        /// </summary>
        private async void CheckDkimSignerAvailable()
        {
            cbxPrereleases.Enabled = false;

            List<Release> aoRelease = null;
            StringBuilder changelog = new StringBuilder("Couldn't get current version.\r\nCheck your Internet connection or restart the application.");

            // Check the lastest Release
            Exception ex = null;
            await Task.Run(() => { try { aoRelease = ApiWrapper.GetAllRelease(cbxPrereleases.Checked); } catch (Exception e) { ex = e; } });

            if(ex != null)
            {
                changelog.Append("\r\nError: " + ex.Message);
            }

            dkimSignerAvailable = null;

            if (aoRelease != null)
            {
                changelog.Clear();
                
                dkimSignerAvailable = aoRelease[0];
                changelog.AppendLine(aoRelease[0].TagName + " (" + aoRelease[0].CreatedAt.Substring(0, 10) + ")\r\n\t" + aoRelease[0].Body.Replace("\r\n", "\r\n\t") + "\r\n");
                
                for(int i = 1; i < aoRelease.Count; i++)
                {
                    if (dkimSignerAvailable.Version < aoRelease[i].Version)
                    {
                        dkimSignerAvailable = aoRelease[i];
                    }

                    // TAG (DATE)\r\nIndented Text
                    changelog.AppendLine(aoRelease[i].TagName + " (" + aoRelease[i].CreatedAt.Substring(0, 10) + ")\r\n\t" + aoRelease[i].Body.Replace("\r\n", "\r\n\t") + "\r\n");
                }
            }

            txtDkimSignerAvailable.Text = dkimSignerAvailable != null ? dkimSignerAvailable.Version.ToString() : "Unknown";
            txtChangelog.Text = changelog.ToString();
            SetUpgradeButton();

            cbxPrereleases.Enabled = true;
        }

        private void SetUpgradeButton()
        {
            string text = string.Empty;

            bool IsExchangeInstalled = (txtExchangeInstalled.Text != "" && txtExchangeInstalled.Text != "Unknown" && txtExchangeInstalled.Text != "Loading...");

            if (dkimSignerInstalled != null && dkimSignerAvailable != null)
            {
                btUpgrade.Text = (dkimSignerInstalled != null ? (dkimSignerInstalled < dkimSignerAvailable.Version ? "&Upgrade" : "&Reinstall") : "&Install");
            }

            btUpgrade.Enabled = dkimSignerAvailable != null && IsExchangeInstalled;
        }

        /// <summary>
        /// Asks the user if he wants to save the current config and saves it.
        /// </summary>
        /// <returns>false if the user pressed cancel. true otherwise</returns>
        private bool CheckSaveConfig()
        {
            bool bStatus = true;

            // IF the configuration have changed
            if (bDataUpdated)
            {
                DialogResult result = ShowMessageBox("Save changes?", "Do you want to save your changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel ||
                   (result == DialogResult.Yes &&
                   !SaveDkimSignerConfig() &&
                   ShowMessageBox("Discard changes?", "Error saving config. Do you wan to close anyways? This will discard all the changes!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
                {
                    bStatus = false;
                }
            }

            return bStatus;
        }

        /// <summary>
        /// Load the current configuration for Exchange DkimSigner from the registry
        /// </summary>
        private void LoadDkimSignerConfig()
        {
            oConfig = new Settings();
            oConfig.InitHeadersToSign();

            if (!oConfig.Load(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml")))
            {
                ShowMessageBox("Settings error", "Couldn't load the settings file.\n Setting it to default values.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //
            // Log level
            //
            switch (oConfig.Loglevel)
            {
                case 1:
                    cbLogLevel.Text = "Error";
                    break;
                case 2:
                    cbLogLevel.Text = "Warning";
                    break;
                case 3:
                    cbLogLevel.Text = "Information";
                    break;
                case 4:
                    cbLogLevel.Text = "Debug";
                    break;
                default:
                    cbLogLevel.Text = "Information";
                    ShowMessageBox("Information", "The log level is invalid. Set to default: Information.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            //
            // Algorithm and Canonicalization
            //
            rbRsaSha1.Checked = (oConfig.SigningAlgorithm == DkimAlgorithmKind.RsaSha1);
            rbRsaSha256.Checked = (oConfig.SigningAlgorithm == DkimAlgorithmKind.RsaSha256);
            rbSimpleHeaderCanonicalization.Checked = (oConfig.HeaderCanonicalization == DkimCanonicalizationKind.Simple);
            rbRelaxedHeaderCanonicalization.Checked = (oConfig.HeaderCanonicalization == DkimCanonicalizationKind.Relaxed);
            rbSimpleBodyCanonicalization.Checked = (oConfig.BodyCanonicalization == DkimCanonicalizationKind.Simple);
            rbRelaxedBodyCanonicalization.Checked = (oConfig.BodyCanonicalization == DkimCanonicalizationKind.Relaxed);

            //
            // Headers to sign
            //
            lbxHeadersToSign.Items.Clear();
            foreach (string sItem in oConfig.HeadersToSign)
            {
                lbxHeadersToSign.Items.Add(sItem);
            }

            //
            // Domain
            //
            DomainElement oCurrentDomain = null;
            if (lbxDomains.SelectedItem != null)
            {
                oCurrentDomain = (DomainElement) lbxDomains.SelectedItem;
            }

            lbxDomains.Items.Clear();
            foreach (DomainElement oConfigDomain in oConfig.Domains)
            {
                lbxDomains.Items.Add(oConfigDomain);
            }

            if (oCurrentDomain != null)
            {
                lbxDomains.SelectedItem = oCurrentDomain;
            }

            bDataUpdated = false;
        }

        /// <summary>
        /// Save the new configuration into registry for Exchange DkimSigner
        /// </summary>
        private bool SaveDkimSignerConfig()
        {
            oConfig.Loglevel = cbLogLevel.SelectedIndex + 1;
            oConfig.SigningAlgorithm = (rbRsaSha1.Checked ? DkimAlgorithmKind.RsaSha1 : DkimAlgorithmKind.RsaSha256);
            oConfig.BodyCanonicalization = (rbSimpleBodyCanonicalization.Checked ? DkimCanonicalizationKind.Simple : DkimCanonicalizationKind.Relaxed);
            oConfig.HeaderCanonicalization = (rbSimpleHeaderCanonicalization.Checked ? DkimCanonicalizationKind.Simple : DkimCanonicalizationKind.Relaxed);

            oConfig.HeadersToSign.Clear();
            foreach (string sItem in lbxHeadersToSign.Items)
            {
                oConfig.HeadersToSign.Add(sItem);
            }

            oConfig.Save(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml"));
            bDataUpdated = false;

            return true;
        }

        private void UpdateSuggestedDNS(string sRsaPublicKeyBase64 = "")
        {
            string sDNSRecord = "";
            if (sRsaPublicKeyBase64 == string.Empty)
            {
                string sPubKeyPath = txtDomainPrivateKeyFilename.Text;

                if (!Path.IsPathRooted(sPubKeyPath))
                {
                    sPubKeyPath = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys", sPubKeyPath);
                }
                
                if (File.Exists(Path.ChangeExtension(sPubKeyPath, ".pub")))
                    sPubKeyPath = Path.ChangeExtension(sPubKeyPath, ".pub");
                else
                    sPubKeyPath += ".pub";

                if (File.Exists(sPubKeyPath))
                {
                    string[] asContents = File.ReadAllLines(sPubKeyPath);

                    if (asContents.Length > 2 && asContents[0].Equals("-----BEGIN PUBLIC KEY-----") && IsBase64String(asContents[1]))
                    {
                        sRsaPublicKeyBase64 = asContents[1];
                    }
                    else
                    {
                        sDNSRecord = "No valid RSA pub key:\n" + sPubKeyPath;
                    }
                }
                else
                {
                    sDNSRecord = "No RSA pub key found:\n" + sPubKeyPath;
                }
            }

            if (sRsaPublicKeyBase64 != null && sRsaPublicKeyBase64 != string.Empty)
            {
                sDNSRecord = "v=DKIM1; k=rsa; p=" + sRsaPublicKeyBase64;
            }

            txtDNSRecord.Text = sDNSRecord;
        }

        /// <summary>
        /// Set the domain key path for the keys
        /// </summary>
        /// <param name="sPath"></param>
        private void SetDomainKeyPath(string sPath)
        {
            string sKeyDir = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys");

            if (sPath.StartsWith(sKeyDir))
            {
                sPath = sPath.Substring(sKeyDir.Length + 1);
            }
            else if (ShowMessageBox("Move key?", "It is strongly recommended to store all the keys in the directory\n" + sKeyDir + "\nDo you want me to move the key into this directory?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                List<string> asFile = new List<string>();
                asFile.Add(sPath);
                asFile.Add(sPath + ".pub");
                asFile.Add(sPath + ".pem");

                foreach (string sFile in asFile)
                {
                    if (File.Exists(sFile))
                    {
                        string sFilename = Path.GetFileName(sFile);
                        string sNewPath = Path.Combine(sKeyDir, sFilename);

                        try
                        {
                            File.Move(sFile, sNewPath);
                            sPath = sNewPath.Substring(sKeyDir.Length - 4);
                        }
                        catch (IOException ex)
                        {
                            ShowMessageBox("Error moving file", "Couldn't move file:\n" + sFile + "\nto\n" + sNewPath + "\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            txtDomainPrivateKeyFilename.Text = sPath;
            btDomainSave.Enabled = true;
            bDataUpdated = true;
        }

        private void downloadAndInstall()
        {

            string zipFile = null;

            // ###########################################
            // ### Download files                      ###
            // ###########################################

            if (Uri.IsWellFormedUriString(dkimSignerAvailable.ZipballUrl, UriKind.RelativeOrAbsolute))
            {
                zipFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".zip");

                DownloadProgressWindow oDpw = new DownloadProgressWindow(dkimSignerAvailable.ZipballUrl, zipFile);
                try
                {
                    if (oDpw.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Couldn't initialize download progress window:\n" + ex.Message, "Error showing download progress", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                finally
                {
                    oDpw.Dispose();
                }

            }
            else
            {
                if (File.Exists(dkimSignerAvailable.ZipballUrl) && Path.GetExtension(dkimSignerAvailable.ZipballUrl) == ".zip")
                {
                    zipFile = dkimSignerAvailable.ZipballUrl;
                }
                else
                {
                    MessageBox.Show(this, "The URL or the path to the ZIP file is invalid. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // ###########################################
            // ### Extract files                       ###
            // ###########################################

            string extractPath = Path.Combine(Path.GetDirectoryName(zipFile), Path.GetFileNameWithoutExtension(zipFile));

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
                }
                else
                {
                    MessageBox.Show(this, "Downloaded .zip is invalid. Please try again.", "Invalid download", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "ZIP Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //now execute the downloaded .exe file
 
            string exePath = Path.Combine(extractPath, @"Src\Configuration.DkimSigner\bin\Release\Configuration.DkimSigner.exe");
            if (!File.Exists(exePath))
            {
                MessageBox.Show(this, "File not found:\n" + exePath, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Process.Start(exePath, "--upgrade-inplace");
                Close();
            }
            catch (Exception ex)
            {
                ShowMessageBox("Updater error", "Couldn't start the process :\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ###########################################################
        // ###################### Button click #######################
        // ###########################################################

        /// <summary>
        /// Button "start" Microsoft Exchange Transport Service have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void genericTransportService_Click(object sender, EventArgs e)
        {
            switch(((Button)sender).Name)
            {
                case "btStartTransportService":
                    transportService.Do(TransportServiceAction.Start);
                    break;
                case "btStopTransportService":
                    transportService.Do(TransportServiceAction.Stop);
                    break;
                case "btRestartTransportService":
                    transportService.Do(TransportServiceAction.Restart);
                    break;
            }
        }

        private void btUpgrade_Click(object sender, EventArgs e)
        {
            if (btUpgrade.Text == "Reinstall" ? MessageBox.Show(this, "Do you really want to " + btUpgrade.Text.ToUpper() + " the DKIM Exchange Agent (new Version: " + txtDkimSignerAvailable.Text + ")?\n", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes : true)
            {
                downloadAndInstall();
            }
        }

        /// <summary>
        /// Button "configure" Microsoft Exchange Transport Service have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btConfigureTransportService_Click(object sender, EventArgs e)
        {
            ExchangeTransportServiceWindow oEtsw = new ExchangeTransportServiceWindow();

            oEtsw.ShowDialog();
            oEtsw.Dispose();
        }

        /// <summary>
        /// Button "add header" have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btHeaderAdd_Click(object sender, EventArgs e)
        {
            HeaderInputWindow oHiw = new HeaderInputWindow();

            if (oHiw.ShowDialog() == DialogResult.OK)
            {
                lbxHeadersToSign.Items.Add(oHiw.txtHeader.Text);
                lbxHeadersToSign.SelectedItem = oHiw.txtHeader;
                bDataUpdated = true;
            }

            oHiw.Dispose();
        }

        /// <summary>
        /// Button "delete header" have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btHeaderDelete_Click(object sender, EventArgs e)
        {
            if (lbxHeadersToSign.SelectedItem != null)
            {
                lbxHeadersToSign.Items.Remove(lbxHeadersToSign.SelectedItem);
                bDataUpdated = true;
            }
        }

        /// <summary>
        /// Button "Save configuration" have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveConfiguration_Click(object sender, EventArgs e)
        {
            SaveDkimSignerConfig();
        }

        /// <summary>
        /// Button "add domain" have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddDomain_Click(object sender, EventArgs e)
        {
            if (bDataUpdated)
            {
                DialogResult result = ShowMessageBox("Save changes?", "Do you want to save the current changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if ((result == DialogResult.Yes && !SaveDkimSignerConfig()) || result == DialogResult.Cancel)
                {
                    return;
                }
            }

            lbxDomains.ClearSelected();
            txtDNSRecord.Text = "";
            txtDNSName.Text = "";
            txtDNSRecord.Text = "";
            gbxDomainDetails.Enabled = true;
            btDomainDelete.Enabled = false;
            bDataUpdated = false;
        }

        /// <summary>
        /// Button "delete domain" have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDomainDelete_Click(object sender, EventArgs e)
        {
            if (lbxDomains.SelectedItem != null)
            {
                DomainElement oCurrentDomain = (DomainElement)lbxDomains.SelectedItem;
                oConfig.Domains.Remove(oCurrentDomain);
                lbxDomains.Items.Remove(oCurrentDomain);
                lbxDomains.SelectedItem = null;
            }

            string keyFile = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys", txtDomainPrivateKeyFilename.Text);

            List<string> asFile = new List<string>();
            asFile.Add(keyFile);
            asFile.Add(keyFile + ".pub");
            asFile.Add(keyFile + ".pem");

            foreach (string sFile in asFile)
            {
                if (File.Exists(sFile) && ShowMessageBox("Delete key?", "Do you want me to delete the key file?\n" + sFile, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(sFile);
                    }
                    catch (IOException ex)
                    {
                        ShowMessageBox("Error deleting file", "Couldn't delete file:\n" + sFile + "\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            SaveDkimSignerConfig();
        }

        /// <summary>
        /// Button "generate key" in domain configuration have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDomainKeyGenerate_Click(object sender, EventArgs e)
        {
            UserPreferences.Default.KeyLength = Convert.ToInt32(cbKeyLength.Text, 10);
            UserPreferences.Default.Save(); 
            
            using (SaveFileDialog oFileDialog = new SaveFileDialog())
            {
                oFileDialog.DefaultExt = "xml";
                oFileDialog.Filter = "All files|*.*";
                oFileDialog.Title = "Select a location for the new key file";
                oFileDialog.InitialDirectory = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys");

                if (!Directory.Exists(oFileDialog.InitialDirectory))
                {
                    Directory.CreateDirectory(oFileDialog.InitialDirectory);
                }

                if (txtDomainName.Text.Length > 0)
                {
                    oFileDialog.FileName = txtDomainName.Text + ".xml";
                }

                if (oFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (RSACryptoServiceProvider oProvider = new RSACryptoServiceProvider(Convert.ToInt32(cbKeyLength.Text, 10))) {
                        AsnKeyBuilder.AsnMessage oPublicEncoded = AsnKeyBuilder.PublicKeyToX509(oProvider.ExportParameters(true));
                        AsnKeyBuilder.AsnMessage oPrivateEncoded = AsnKeyBuilder.PrivateKeyToPKCS8(oProvider.ExportParameters(true));

                        File.WriteAllBytes(oFileDialog.FileName, Encoding.ASCII.GetBytes(oProvider.ToXmlString(true)));
                        File.WriteAllText(oFileDialog.FileName + ".pub", "-----BEGIN PUBLIC KEY-----\r\n" + Convert.ToBase64String(oPublicEncoded.GetBytes()) + "\r\n-----END PUBLIC KEY-----");
                        File.WriteAllText(oFileDialog.FileName + ".pem", "-----BEGIN PRIVATE KEY-----\r\n" + Convert.ToBase64String(oPrivateEncoded.GetBytes()) + "\r\n-----END PRIVATE KEY-----");

                        UpdateSuggestedDNS(Convert.ToBase64String(oPublicEncoded.GetBytes()));
                        SetDomainKeyPath(oFileDialog.FileName);
                    }
                }
            }
        }

        /// <summary>
        /// Button "select key" in domain configuration have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDomainKeySelect_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog oFileDialog = new OpenFileDialog())
            {
                oFileDialog.FileName = "key";
                oFileDialog.Filter = "Key files|*.xml;*.pem|All files|*.*";
                oFileDialog.Title = "Select a private key for signing";
                oFileDialog.InitialDirectory = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys");

                if (oFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SetDomainKeyPath(oFileDialog.FileName);
                    UpdateSuggestedDNS();
                }
            }
        }

        /// <summary>
        /// Button "check DNS" in domain configuration have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDomainCheckDNS_Click(object sender, EventArgs e)
        {
            string sFullDomain = txtDomainSelector.Text + "._domainkey." + txtDomainName.Text;

            try
            {
                Resolver oResolver = new Resolver();
                oResolver.Recursion = true;
                oResolver.UseCache = false;

                // Get the name server for the domain to avoid DNS caching
                Response oResponse = oResolver.Query(sFullDomain, QType.NS, QClass.IN);
                if (oResponse.RecordsRR.GetLength(0) > 0)
                {
                    RR oNsRecord = oResponse.RecordsRR[0];
                    if (oNsRecord.RECORD.RR.RECORD.GetType() == typeof(RecordSOA))
                    {
                        RecordSOA oSoaRecord = (RecordSOA)oNsRecord.RECORD.RR.RECORD;
                        oResolver.DnsServer = oSoaRecord.MNAME;
                    }
                }

                // Get the TXT record for DKIM
                oResponse = oResolver.Query(sFullDomain, QType.TXT, QClass.IN);
                if (oResponse.RecordsTXT.GetLength(0) > 0)
                {
                    RecordTXT oTxtRecord = oResponse.RecordsTXT[0];
                    txtDomainDNS.Text = oTxtRecord.TXT.Count > 0 ? string.Join(string.Empty, oTxtRecord.TXT) : "No record found for " + sFullDomain;
                }
                else
                {
                    txtDomainDNS.Text = "No record found for " + sFullDomain;
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox("Error", "Coldn't get DNS record:\n" + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDomainDNS.Text = "Error getting record.";
            }
        }

        /// <summary>
        /// Button "save" in domain configuration have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDomainSave_Click(object sender, EventArgs e)
        {
            if (epvDomainSelector.GetError(txtDomainName) == "" && epvDomainSelector.GetError(txtDomainSelector) == "")
            {
                DomainElement oCurrentDomain;
                bool bAddToList = false;

                if (lbxDomains.SelectedItem != null)
                {
                    oCurrentDomain = (DomainElement)lbxDomains.SelectedItem;
                }
                else
                {
                    oCurrentDomain = new DomainElement();
                    bAddToList = true;
                }

                oCurrentDomain.Domain = txtDomainName.Text;
                oCurrentDomain.Selector = txtDomainSelector.Text;
                oCurrentDomain.PrivateKeyFile = txtDomainPrivateKeyFilename.Text;

                if (bAddToList)
                {
                    oConfig.Domains.Add(oCurrentDomain);
                    lbxDomains.Items.Add(oCurrentDomain);
                    lbxDomains.SelectedItem = oCurrentDomain;
                }

                if (SaveDkimSignerConfig())
                {
                    btDomainSave.Enabled = false;
                    btDomainDelete.Enabled = true;
                }
            }
            else
            {
                ShowMessageBox("Config error", "You first need to fix the errors in your domain configuration before saving.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Button "Refresh" on EventLog TabPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btEventLogRefresh_Click(object sender, EventArgs e)
        {
            btEventLogRefresh.Enabled = false;
            
            await Task.Run(() =>
            {
                dgEventLog.Rows.Clear();
                if (EventLog.SourceExists(Constants.DKIM_SIGNER_EVENTLOG_SOURCE))
                {
                    EventLog oLogger = new EventLog();

                    try {
                        oLogger.Log = EventLog.LogNameFromSourceName(Constants.DKIM_SIGNER_EVENTLOG_SOURCE, ".");

                    }
                    catch (Exception ex)
                    {
                        oLogger.Dispose();
                        MessageBox.Show(this, "Couldn't get EventLog source:\n" + ex.Message, "Error getting EventLog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btEventLogRefresh.Enabled = true;
                        return;
                    }

                    for (int i = oLogger.Entries.Count - 1; i > 0; i--)
                    {
                        EventLogEntry oEntry;
                        try {
                            oEntry = oLogger.Entries[i];
                        }
                        catch (Exception ex)
                        {
                            oLogger.Dispose();
                            MessageBox.Show(this, "Couldn't get EventLog entry:\n" + ex.Message, "Error getting EventLog", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btEventLogRefresh.Enabled = true;
                            return;
                        }

                        if (oEntry.Source != Constants.DKIM_SIGNER_EVENTLOG_SOURCE)
                        {
                            continue;
                        }

                        Image oImg = null;
                        switch (oEntry.EntryType)
                        {
                            case EventLogEntryType.Information:
                                oImg = SystemIcons.Information.ToBitmap();
                                break;
                            case EventLogEntryType.Warning:
                                oImg = SystemIcons.Warning.ToBitmap();
                                break;
                            case EventLogEntryType.Error:
                                oImg = SystemIcons.Error.ToBitmap();
                                break;
                            case EventLogEntryType.FailureAudit:
                                oImg = SystemIcons.Error.ToBitmap();
                                break;
                            case EventLogEntryType.SuccessAudit:
                                oImg = SystemIcons.Question.ToBitmap();
                                break;
                        }

                        dgEventLog.BeginInvoke(new Action(() => dgEventLog.Rows.Add(oImg, oEntry.TimeGenerated.ToString("yyyy-MM-ddTHH:mm:ss.fff"), oEntry.Message)));
                    }

                    oLogger.Dispose();
                }
            });

            btEventLogRefresh.Enabled = true;
        }

        private async void btExchangeVersion_Click(object sender, EventArgs e)
        {
            string version = "Unknown";

            btExchangeVersion.Enabled = false;
            ExchangeServerException ex = null;
            await Task.Run(() => { try { version = ExchangeServer.GetInstalledVersion(); } catch (ExchangeServerException exe) { ex = exe; } });

            btExchangeVersion.Enabled = true;
            if (ex != null)
            {
                ShowMessageBox("Exchange Version Error", "Couldn't determine installed Exchange Version: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            char[] values = version.ToCharArray();
            string result = "";
            foreach (char letter in values)
            {
                // Get the integral value of the character. 
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form. 
                string hexOutput = String.Format("{0:X}", value);
                result += "'" + letter + "'" + " -> " + hexOutput + "\n";
            }

            string ConfigVersion = Version.Parse(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion).ToString().Substring(0, 5);


            result = "My version: " + ConfigVersion + "\nExchange\n" + result;

            ShowMessageBox("Exchange Version Debug", result, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
