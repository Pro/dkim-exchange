using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using ConfigurationSettings;
using Configuration.DkimSigner.Exchange;
using Configuration.DkimSigner.GitHub;
using Heijden.DNS;

namespace Configuration.DkimSigner
{
    public partial class MainWindow : Form
    {
        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/

        private Settings oConfig = null;

        private bool bDataUpdated = false;
        
        private Thread thDkimSignerInstalled = null;
        private Thread thDkimSignerAvailable = null;
        private Thread thTransportServiceOperation = null;
        private Thread thExchangeInstalled = null;
        private System.Threading.Timer tiTransportServiceStatus = null;
        private Version dkimSignerInstalled = null;
        private Release dkimSignerAvailable = null;

        delegate void SetDkimSignerInstalledCallback(Version oDkimSignerInstalled);
        delegate void SetDkimSignerAvailableCallback(Release oDkimSignerAvailable);
        delegate void SetExchangeTransportServiceStatusCallback(string sStatus);
        delegate void SetExchangeInstalledCallback(string sStatus);

        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public MainWindow()
        {           
            this.InitializeComponent();

            this.cbLogLevel.SelectedItem = "Information";
            this.cbKeyLength.SelectedItem = "1024";
            this.txtAbout.Text =    Constants.DKIM_SIGNER_VERSION + "\r\n\r\n" +
                                    Constants.DKIM_SIGNER_NOTICE + "\r\n\r\n" +
                                    Constants.DKIM_SIGNER_LICENCE + "\r\n\r\n" +
                                    Constants.DKIM_SIGNER_AUTHOR + "\r\n\r\n" +
                                    Constants.DKIM_SIGNER_WEBSITE;
        }

        /**********************************************************/
        /************************* Events *************************/
        /**********************************************************/

        /// <summary>
        /// Load information in the Windowform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Load(object sender, EventArgs e)
        {
            // Get Exchange version installed + load the current configuration
            // Get Exchange.DkimSigner version installed
            this.txtExchangeInstalled.Text = "Loading ...";
            this.thExchangeInstalled = new Thread(new ThreadStart(this.CheckExchangeInstalledSafe));
            
            // Start the threads that will do lookup
            try
            {
                this.thExchangeInstalled.Start();
            }
            catch (ThreadAbortException) { }

            // update transport service status each second
            this.tiTransportServiceStatus = new System.Threading.Timer(new TimerCallback(this.CheckExchangeTransportServiceStatusSafe), null, 0, 1000);

            // Update Exchange and DKIM Signer version
            this.UpdateVersions();

            // Load setting from XML file
            this.LoadDkimSignerConfig();
        }

        /// <summary>
        /// Confirm the configuration saving before quit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the config have been change and haven't been save
            if (!this.CheckSaveConfig())
            {
                e.Cancel = true;
            }
            else
            {
                //stop timer
                if (this.tiTransportServiceStatus != null)
                    this.tiTransportServiceStatus.Change(Timeout.Infinite, Timeout.Infinite);

                // IF any thread running, we stop them before exit
                if (this.thDkimSignerAvailable != null && this.thDkimSignerAvailable.ThreadState == System.Threading.ThreadState.Running)
                {
                    this.thDkimSignerAvailable.Abort();
                }

                if (this.thDkimSignerInstalled != null && this.thDkimSignerInstalled.ThreadState == System.Threading.ThreadState.Running)
                {
                    this.thDkimSignerInstalled.Abort();
                }

                if (this.thExchangeInstalled != null && this.thExchangeInstalled.ThreadState == System.Threading.ThreadState.Running)
                {
                    this.thExchangeInstalled.Abort();
                }


                if (this.thTransportServiceOperation != null && this.thTransportServiceOperation.ThreadState == System.Threading.ThreadState.Running)
                {
                    this.thTransportServiceOperation.Join();
                }
            }
        }

        private void txtExchangeStatus_TextChanged(object sender, System.EventArgs e)
        {
            switch (this.txtExchangeStatus.Text)
            {
                case "Running":
                    this.btStartTransportService.Enabled = false;
                    this.btStopTransportService.Enabled = true;
                    this.btRestartTransportService.Enabled = true;
                    break;

                case "Stopped":
                    this.btStartTransportService.Enabled = true;
                    this.btStopTransportService.Enabled = false;
                    this.btRestartTransportService.Enabled = false;
                    break;
                default:
                    this.btStartTransportService.Enabled = false;
                    this.btStopTransportService.Enabled = false;
                    this.btRestartTransportService.Enabled = false;
                    break;
            }
        }

        private void cbxPrereleases_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateVersions();
        }

        private void rbRsaSha1_CheckedChanged(object sender, System.EventArgs e)
        {
            this.bDataUpdated = true;
        }

        private void rbRsaSha256_CheckedChanged(object sender, System.EventArgs e)
        {
            this.bDataUpdated = true;
        }

        private void rbSimpleHeaderCanonicalization_CheckedChanged(object sender, System.EventArgs e)
        {
            this.bDataUpdated = true;
        }

        private void rbRelaxedHeaderCanonicalization_CheckedChanged(object sender, System.EventArgs e)
        {
            this.bDataUpdated = true;
        }

        private void rbSimpleBodyCanonicalization_CheckedChanged(object sender, System.EventArgs e)
        {
            this.bDataUpdated = true;
        }

        private void rbRelaxedBodyCanonicalization_CheckedChanged(object sender, System.EventArgs e)
        {
            this.bDataUpdated = true;
        }

        private void cbLogLevel_TextChanged(object sender, System.EventArgs e)
        {
            this.bDataUpdated = true;
        }

        private void lbxHeadersToSign_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btHeaderDelete.Enabled = (this.lbxHeadersToSign.SelectedItem != null);
        }

        private void lbxDomains_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lbxDomains.SelectedItems.Count == 0)
            {
                this.txtDomainName.Text = "";
                this.txtDomainSelector.Text = "";
                this.txtDomainPrivateKeyFilename.Text = "";
                this.txtDomainDNS.Text = "";
                this.gbxDomainDetails.Enabled = false;
            }
            else
            {
                DomainElement oSelected = (DomainElement) this.lbxDomains.SelectedItem;
                this.txtDomainName.Text = oSelected.Domain;
                this.txtDomainSelector.Text = oSelected.Selector;
                this.txtDomainPrivateKeyFilename.Text = oSelected.PrivateKeyFile;

                this.UpdateSuggestedDNS();
                this.txtDomainDNS.Text = "";
                this.gbxDomainDetails.Enabled = true;
                this.btDomainDelete.Enabled = true;
                this.btDomainSave.Enabled = false;

                this.bDataUpdated = false;
            }
        }

        private void txtDomainName_TextChanged(object sender, EventArgs e)
        {           
            if (Uri.CheckHostName(this.txtDomainName.Text) != UriHostNameType.Dns)
            {
                this.epvDomainSelector.SetError(this.txtDomainName, "Invalid DNS name. Format: 'example.com'");
            }
            else
            {
                this.epvDomainSelector.SetError(this.txtDomainName, null);
            }

            this.btDomainSave.Enabled = true;
            this.txtDNSName.Text = this.txtDomainSelector.Text + "._domainkey." + this.txtDomainName.Text + ".";

            this.bDataUpdated = true;
        }

        private void txtDomainSelector_TextChanged(object sender, EventArgs e)
        {
            if (!Regex.IsMatch(this.txtDomainSelector.Text, @"^[a-zA-Z0-9_]+$", RegexOptions.None))
            {
                this.epvDomainSelector.SetError(this.txtDomainSelector, "The selector should only contain characters, numbers and underscores.");
            }
            else
            {
                this.epvDomainSelector.SetError(this.txtDomainSelector, null);
            }

            this.btDomainSave.Enabled = true;
            this.txtDNSName.Text = this.txtDomainSelector.Text + "._domainkey." + this.txtDomainName.Text + ".";

            this.bDataUpdated = true;
        }

        /**********************************************************/
        /******************* Internal functions *******************/
        /**********************************************************/

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

        /// <summary>
        /// 
        /// </summary>
        private void SetExchangeTransportServiceStatus(string sStatus)
        {
            this.txtExchangeStatus.Text = (sStatus != null ? sStatus : "Unknown");
        }

        /// <summary>
        /// Set the value of txtDkimSignerInstalled from CheckDkimSignerInstalledSafe (use by thread DkimSignerInstalled)
        /// </summary>
        /// <param name="dkimSignerInstalled"></param>
        private void SetDkimSignerInstalled(Version oDkimSignerInstalled)
        {
            this.txtDkimSignerInstalled.Text = (oDkimSignerInstalled != null ? oDkimSignerInstalled.ToString() : "Not installed");
            this.btConfigureTransportService.Enabled = (oDkimSignerInstalled != null);
            dkimSignerInstalled = oDkimSignerInstalled;
            setUpgradeButton();
        }

        /// <summary>
        ///  Set the value of txtDkimSignerAvailable from CheckDkimSignerAvailableSafe (use by thread DkimSignerAvailable)
        /// </summary>
        /// <param name="dkimSignerAvailable"></param>
        private void SetDkimSignerAvailable(Release oDkimSignerAvailable)
        {
            dkimSignerAvailable = oDkimSignerAvailable;
            if (oDkimSignerAvailable != null)
            {
                string version = oDkimSignerAvailable.Version.ToString();

                Match match = Regex.Match(oDkimSignerAvailable.TagName, @"v?((?:\d+\.){0,3}\d+)(?:-(alpha|beta|rc)(?:\.(\d+))?)?", RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    if (match.Groups.Count > 2 && match.Groups[2].Value.Length > 0)
                    {
                        version += " (" + match.Groups[2].Value;
                        if (match.Groups.Count > 3 && match.Groups[3].Value.Length > 0)
                        {
                            version += "." + match.Groups[3].Value; 
                        }
                        version += ")";
                    }
                }

                this.txtDkimSignerAvailable.Text = version;
                this.txtChangelog.Text = oDkimSignerAvailable.Body;
            }
            else
            {
                this.txtDkimSignerAvailable.Text = "Unknown";
                this.txtChangelog.Text = "Couldn't get current version.\r\nCheck your Internet connection or restart the application.";
            }
            setUpgradeButton();
        }

        /// <summary>
        /// Check the Microsoft Exchange Transport Service Status
        /// </summary>
        /// <param name="state"></param>
        private void CheckExchangeTransportServiceStatusSafe(object state)
        {
            string sStatus = null;

            try
            {
                sStatus = ExchangeServer.GetTransportServiceStatus().ToString();
            }
            catch (ExchangeServerException)
            {
                // stop timer if we couldn't get service status
                this.tiTransportServiceStatus.Change(Timeout.Infinite, Timeout.Infinite);
            }
            
            if (this.txtExchangeStatus.InvokeRequired)
            {
                SetExchangeTransportServiceStatusCallback d = new SetExchangeTransportServiceStatusCallback(this.SetExchangeTransportServiceStatus);
                this.Invoke(d, sStatus);
            }
            else
            {
                this.SetExchangeTransportServiceStatus(sStatus);
            }
        }

        /// <summary>
        /// Set the value of txtExchangeInstalled from CheckExchangeInstalledSafe (use by thread in Load)
        /// </summary>
        /// <param name="dkimSignerInstalled"></param>
        private void SetExchangeInstalled(string exchangeInstalled)
        {
            this.txtExchangeInstalled.Text = exchangeInstalled;

            // Uptade Microsft Exchange Transport Service stuatus
            if (exchangeInstalled != null && exchangeInstalled != "Not installed")
            {
                this.btConfigureTransportService.Enabled = true;
            }
            else
            {
                this.SetExchangeTransportServiceStatus("Unavailable");
                this.btConfigureTransportService.Enabled = false;
            }
        }

        /// <summary>
        /// Check the Microsoft Exchange Transport Service Status
        /// </summary>
        /// <param name="state"></param>
        private void CheckExchangeInstalledSafe()
        {
            string version = "Unknown";

            try
            {
                version = ExchangeServer.GetInstalledVersion();
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't determine installed Exchange Version: " + e.Message, "Exchange Version Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (this.txtExchangeStatus.InvokeRequired)
            {
                SetExchangeInstalledCallback d = new SetExchangeInstalledCallback(this.SetExchangeInstalled);
                this.Invoke(d, version);
            }
            else
            {
                this.SetExchangeInstalled(version);
            }
        }

        /// <summary>
        /// Thread safe function for the thread DkimSignerInstalled
        /// </summary>
        private void CheckDkimSignerInstalledSafe()
        {
            Version oDkimSignerInstalled = null;
            
            // Check if DKIM Agent is in C:\Program Files\Exchange DkimSigner and get version of DLL
            try
            {
                oDkimSignerInstalled = Version.Parse(System.Diagnostics.FileVersionInfo.GetVersionInfo(Path.Combine(Constants.DKIM_SIGNER_PATH, Constants.DKIM_SIGNER_AGENT_DLL)).ProductVersion);
            }
            catch (Exception) {}

            // Check if DKIM agent have been load in Exchange
            if (oDkimSignerInstalled != null)
            {
                try
                {
                    if (!ExchangeServer.IsDkimAgentTransportInstalled())
                    {
                        oDkimSignerInstalled = null;
                    }
                }
                catch (Exception) { }
            }

            // Set the result in the textbox
            if (this.txtDkimSignerInstalled.InvokeRequired)
            {
                SetDkimSignerInstalledCallback d = new SetDkimSignerInstalledCallback(SetDkimSignerInstalled);
                this.Invoke(d, oDkimSignerInstalled);
            }
            else
            {
                this.SetDkimSignerInstalled(oDkimSignerInstalled);
            }
        }

        /// <summary>
        /// Thread safe function for the thread DkimSignerAvailable
        /// </summary>
        private void CheckDkimSignerAvailableSafe()
        {
            Release oDkimSignerAvailable = null;

            // Check the lastest Release
            try
            {
                oDkimSignerAvailable = ApiWrapper.GetNewestRelease(cbxPrereleases.Checked);
            }
            catch (Exception) {}

            // Set the result in the textbox
            if (this.txtDkimSignerInstalled.InvokeRequired)
            {
                SetDkimSignerAvailableCallback d = new SetDkimSignerAvailableCallback(SetDkimSignerAvailable);
                this.Invoke(d, oDkimSignerAvailable);
            }
            else
            {
                this.SetDkimSignerAvailable(oDkimSignerAvailable);
            }
        }

        /// <summary>
        /// Thread safe function for the thread to start Microsoft Exchange Transport Service
        /// </summary>
        private void StartTransportServiceSafe()
        {
            try
            {
                ExchangeServer.StartTransportService();

                MessageBox.Show("MSExchangeTransport service successfully started.\n", "Service information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ExchangeServerException)
            {
                MessageBox.Show("Couldn't change MSExchangeTransport service status.\n", "Service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Thread safe function for the thread to stop Microsoft Exchange Transport Service
        /// </summary>
        private void StopTransportServiceSafe()
        {
            try
            {
                ExchangeServer.StopTransportService();

                MessageBox.Show("MSExchangeTransport service successfully stopped.\n", "Service information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ExchangeServerException)
            {
                MessageBox.Show("Couldn't change MSExchangeTransport service status.\n", "Service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Thread safe function for the thread to restart Microsoft Exchange Transport Service
        /// </summary>
        private void RestartTransportServiceSafe()
        {
            try
            {
                ExchangeServer.RestartTransportService();

                MessageBox.Show("MSExchangeTransport service successfully restarted.\n", "Service information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (ExchangeServerException)
            {
                MessageBox.Show("Couldn't change MSExchangeTransport service status.\n", "Service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Update the current available and installed version info
        /// </summary>
        private void UpdateVersions()
        {
            // Get Exchange.DkimSigner version installed
            this.txtDkimSignerInstalled.Text = "Loading ...";
            this.thDkimSignerInstalled = new Thread(new ThreadStart(this.CheckDkimSignerInstalledSafe));

            // Get Exchange.DkimSigner version available
            this.txtDkimSignerAvailable.Text = "Loading ...";
            this.thDkimSignerAvailable = new Thread(new ThreadStart(this.CheckDkimSignerAvailableSafe));

            // Start the threads that will do lookup
            try
            {
                this.thDkimSignerInstalled.Start();
                this.thDkimSignerAvailable.Start();
            }
            catch (ThreadAbortException) { }
        }

        /// <summary>
        /// Asks the user if he wants to save the current config and saves it.
        /// </summary>
        /// <returns>false if the user pressed cancel. true otherwise</returns>
        private bool CheckSaveConfig()
        {
            bool bStatus = true;

            // IF the configuration have changed
            if (this.bDataUpdated)
            {
                DialogResult result = MessageBox.Show("Do you want to save your changes?", "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                // IF we want to save the change
                if (result == DialogResult.Yes)
                {
                    // IF we can't save the changes
                    if (!this.SaveDkimSignerConfig())
                    {
                        if (MessageBox.Show("Error saving config. Do you wan to close anyways? This will discard all the changes!", "Discard changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            bStatus = false;
                        }
                    }
                }
                // IF we cancel the save of our changes
                else if (result == DialogResult.Cancel)
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
            this.oConfig = new Settings();

            if (!this.oConfig.Load(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml")))
            {
                MessageBox.Show("Couldn't load the settings file.\n Setting it to default values.", "Settings error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //
            // Log level
            //
            switch (this.oConfig.Loglevel)
            {
                case 1:
                    this.cbLogLevel.Text = "Error";
                    break;
                case 2:
                    this.cbLogLevel.Text = "Warning";
                    break;
                case 3:
                    this.cbLogLevel.Text = "Information";
                    break;
                default:
                    this.cbLogLevel.Text = "Information";
                    MessageBox.Show("The log level is invalid. The log level have been set to Information.");
                    break;
            }

            //
            // Algorithm and Canonicalization
            //
            this.rbRsaSha1.Checked = (oConfig.SigningAlgorithm == DkimAlgorithmKind.RsaSha1);
            this.rbSimpleHeaderCanonicalization.Checked = (this.oConfig.HeaderCanonicalization == DkimCanonicalizationKind.Simple);
            this.rbRelaxedHeaderCanonicalization.Checked = (this.oConfig.HeaderCanonicalization == DkimCanonicalizationKind.Relaxed);
            this.rbSimpleBodyCanonicalization.Checked = (this.oConfig.BodyCanonicalization == DkimCanonicalizationKind.Simple);
            this.rbRelaxedBodyCanonicalization.Checked = (this.oConfig.BodyCanonicalization == DkimCanonicalizationKind.Relaxed);

            //
            // Headers to sign
            //
            this.lbxHeadersToSign.Items.Clear();

            foreach (string sItem in this.oConfig.HeadersToSign)
            {
                this.lbxHeadersToSign.Items.Add(sItem);
            }

            //
            // Domain
            //
            DomainElement oCurrentDomain = null;

            if (this.lbxDomains.SelectedItem != null)
            {
                oCurrentDomain = (DomainElement) this.lbxDomains.SelectedItem;
            }

            this.lbxDomains.Items.Clear();
            
            foreach (DomainElement oConfigDomain in this.oConfig.Domains)
            {
                this.lbxDomains.Items.Add(oConfigDomain);
            }

            if (oCurrentDomain != null)
            {
                this.lbxDomains.SelectedItem = oCurrentDomain;
            }

            this.bDataUpdated = false;
        }

        /// <summary>
        /// Save the new configuration into registry for Exchange DkimSigner
        /// </summary>
        private bool SaveDkimSignerConfig()
        {
            this.oConfig.Loglevel = this.cbLogLevel.SelectedIndex + 1;

            this.oConfig.SigningAlgorithm = (this.rbRsaSha1.Checked ? DkimAlgorithmKind.RsaSha1 : DkimAlgorithmKind.RsaSha256);
            this.oConfig.BodyCanonicalization = (this.rbSimpleBodyCanonicalization.Checked ? DkimCanonicalizationKind.Simple : DkimCanonicalizationKind.Relaxed);
            this.oConfig.HeaderCanonicalization = (this.rbSimpleHeaderCanonicalization.Checked ? DkimCanonicalizationKind.Simple : DkimCanonicalizationKind.Relaxed);

            this.oConfig.HeadersToSign.Clear();

            foreach (string sItem in this.lbxHeadersToSign.Items)
            {
                this.oConfig.HeadersToSign.Add(sItem);
            }

            this.oConfig.Save(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml"));

            this.bDataUpdated = false;

            return true;
        }

                //
                // TODO : Problem when sRsaPublicKeyBase64 is empty
                //
        private void UpdateSuggestedDNS(string sRsaPublicKeyBase64 = "")
        {
            string sDNSRecord = "";
            if (sRsaPublicKeyBase64 == string.Empty)
            {
                string sPubKeyPath = this.txtDomainPrivateKeyFilename.Text;

                if (!Path.IsPathRooted(sPubKeyPath))
                {
                    sPubKeyPath = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys", sPubKeyPath);
                }

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
                sDNSRecord = "v=DKIM1; k=rsa; p=" + sRsaPublicKeyBase64;

            this.txtDNSRecord.Text = sDNSRecord;
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
            else if (MessageBox.Show("It is strongly recommended to store all the keys in the directory\n" + sKeyDir + "\nDo you want me to move the key into this directory?", "Move key?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                            MessageBox.Show("Couldn't move file:\n" + sFile + "\nto\n" + sNewPath + "\n" + ex.Message, "Error moving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            this.txtDomainPrivateKeyFilename.Text = sPath;
            this.btDomainSave.Enabled = true;

            this.bDataUpdated = true;
        }

        /// <summary>
        /// Reloads all the entries from event log and shows them on the EventLog Tab page.
        /// </summary>
        private void refreshEventLog()
        {
            int count = 0;
            dgEventLog.Rows.Clear();
            if (EventLog.SourceExists(Constants.DKIM_SIGNER_EVENTLOG_SOURCE))
            {

                EventLog logger = new EventLog();
                logger.Log = EventLog.LogNameFromSourceName(Constants.DKIM_SIGNER_EVENTLOG_SOURCE, ".");
                var reversed = logger.Entries.Cast<EventLogEntry>().Reverse<EventLogEntry>();

                foreach (System.Diagnostics.EventLogEntry entry in reversed)
                {
                    if (entry.Source != Constants.DKIM_SIGNER_EVENTLOG_SOURCE)
                        continue;
                    count++;
                    if (count > 100)
                    {
                        dgEventLog.Rows.Add(SystemIcons.Information.ToBitmap(), "-----", "Maximum number of 100 Log entries shown");
                        break;
                    }
                    Image img = null;
                    switch (entry.EntryType)
                    {
                        case EventLogEntryType.Information:
                            img = SystemIcons.Information.ToBitmap();
                            break;
                        case EventLogEntryType.Warning:
                            img = SystemIcons.Warning.ToBitmap();
                            break;
                        case EventLogEntryType.Error:
                            img = SystemIcons.Error.ToBitmap();
                            break;
                        case EventLogEntryType.FailureAudit:
                            img = SystemIcons.Error.ToBitmap();
                            break;
                        case EventLogEntryType.SuccessAudit:
                            img = SystemIcons.Question.ToBitmap();
                            break;
                    }
                    dgEventLog.Rows.Add(img, entry.TimeGenerated.ToString("yyyy-MM-ddTHH:mm:ss.fff"), entry.Message);
                }
            }
        }

        private void setUpgradeButton()
        {
            if (dkimSignerAvailable == null)
            {
                btUpgrade.Text = "Install from .zip";
                btUpgrade.Enabled = true;
                return;
            }
            if (dkimSignerInstalled == null)
            {
                btUpgrade.Text = "Install";
                btUpgrade.Enabled = true;
            }
            else
            {
                if (dkimSignerInstalled < dkimSignerAvailable.Version)
                {
                    btUpgrade.Text = "Upgrade";
                    btUpgrade.Enabled = true;
                }
                else if (dkimSignerInstalled == dkimSignerAvailable.Version)
                {
                    btUpgrade.Text = "Reinstall";
                    btUpgrade.Enabled = true;
                }
                else
                {
                    btUpgrade.Text = "Upgrade";
                    btUpgrade.Enabled = false;
                }
            }

        }

        /**********************************************************/
        /********************** Button click **********************/
        /**********************************************************/

        /// <summary>
        /// Button "start" Microsoft Exchange Transport Service have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btStartTransportService_Click(object sender, EventArgs e)
        {
            this.thTransportServiceOperation = new Thread(new ThreadStart(this.StartTransportServiceSafe));

            try
            {
                this.thTransportServiceOperation.Start();
            }
            catch (ThreadAbortException) { }
        }

        /// <summary>
        /// Button "stop" Microsoft Exchange Transport Service have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btStopTransportService_Click(object sender, EventArgs e)
        {
            this.thTransportServiceOperation = new Thread(new ThreadStart(this.StopTransportServiceSafe));

            try
            {
                this.thTransportServiceOperation.Start();
            }
            catch (ThreadAbortException) { }
        }

        /// <summary>
        /// Button "restart" Microsoft Exchange Transport Service have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRestartTransportService_Click(object sender, EventArgs e)
        {
            this.thTransportServiceOperation = new Thread(new ThreadStart(this.RestartTransportServiceSafe));

            try
            {
                this.thTransportServiceOperation.Start();
            }
            catch (ThreadAbortException) { }
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
                this.lbxHeadersToSign.Items.Add(oHiw.txtHeader.Text);
                this.lbxHeadersToSign.SelectedItem = oHiw.txtHeader;

                this.bDataUpdated = true;
            }
        }

        /// <summary>
        /// Button "delete header" have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btHeaderDelete_Click(object sender, EventArgs e)
        {
            if (this.lbxHeadersToSign.SelectedItem != null)
            {
                this.lbxHeadersToSign.Items.Remove(lbxHeadersToSign.SelectedItem);

                this.bDataUpdated = true;
            }
        }

        /// <summary>
        /// Button "Save configuration" have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSaveConfiguration_Click(object sender, EventArgs e)
        {
            this.SaveDkimSignerConfig();
        }

        /// <summary>
        /// Button "add domain" have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btAddDomain_Click(object sender, EventArgs e)
        {
            if (this.bDataUpdated)
            {
                DialogResult result = MessageBox.Show("Do you want to save the current changes?", "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (!SaveDkimSignerConfig())
                        return;
                }

                if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            this.lbxDomains.ClearSelected();
            this.txtDNSRecord.Text = "";
            this.txtDNSName.Text = "";
            this.txtDNSRecord.Text = "";
            this.gbxDomainDetails.Enabled = true;
            this.btDomainDelete.Enabled = false;

            this.bDataUpdated = false;
        }

        /// <summary>
        /// Button "delete domain" have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDomainDelete_Click(object sender, EventArgs e)
        {
            if (this.lbxDomains.SelectedItem != null)
            {
                DomainElement oCurrentDomain = (DomainElement)this.lbxDomains.SelectedItem;
                this.oConfig.Domains.Remove(oCurrentDomain);

                this.lbxDomains.Items.Remove(oCurrentDomain);
                this.lbxDomains.SelectedItem = null;
            }

            string keyFile = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys", this.txtDomainPrivateKeyFilename.Text);

            List<string> asFile = new List<string>();
            asFile.Add(keyFile);
            asFile.Add(keyFile + ".pub");
            asFile.Add(keyFile + ".pem");

            foreach (string sFile in asFile)
            {
                if (File.Exists(sFile) && MessageBox.Show("Do you want me to delete the key file?\n" + sFile, "Delete key?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(sFile);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Couldn't delete file:\n" + sFile + "\n" + ex.Message, "Error deleting file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.SaveDkimSignerConfig();
        }

        /// <summary>
        /// Button "generate key" in domain configuration have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDomainKeyGenerate_Click(object sender, EventArgs e)
        {
            SaveFileDialog oFileDialog = new SaveFileDialog();

            oFileDialog.DefaultExt = "xml";
            oFileDialog.Filter = "All files|*.*";
            oFileDialog.Title = "Select a location for the new key file";

            oFileDialog.InitialDirectory = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys");

            if (!Directory.Exists(oFileDialog.InitialDirectory))
            {
                Directory.CreateDirectory(oFileDialog.InitialDirectory);
            }

            if (this.txtDomainName.Text.Length > 0)
            {
                oFileDialog.FileName = this.txtDomainName.Text + ".xml";
            }

            if (oFileDialog.ShowDialog() == DialogResult.OK)
            {
                RSACryptoServiceProvider oProvider = new RSACryptoServiceProvider(Convert.ToInt32(this.cbKeyLength.Text, 10));
                CSInteropKeys.AsnKeyBuilder.AsnMessage oPublicEncoded = CSInteropKeys.AsnKeyBuilder.PublicKeyToX509(oProvider.ExportParameters(true));
                CSInteropKeys.AsnKeyBuilder.AsnMessage oPrivateEncoded = CSInteropKeys.AsnKeyBuilder.PrivateKeyToPKCS8(oProvider.ExportParameters(true));

                File.WriteAllBytes(oFileDialog.FileName, Encoding.ASCII.GetBytes(oProvider.ToXmlString(true)));
                File.WriteAllText(Path.GetFileNameWithoutExtension(oFileDialog.FileName) + ".pub", "-----BEGIN PUBLIC KEY-----\r\n" + Convert.ToBase64String(oPublicEncoded.GetBytes()) + "\r\n-----END PUBLIC KEY-----");
                File.WriteAllText(Path.GetFileNameWithoutExtension(oFileDialog.FileName) + ".pem", "-----BEGIN RSA PRIVATE KEY-----\r\n" + Convert.ToBase64String(oPrivateEncoded.GetBytes()) + "\r\n-----END RSA PRIVATE KEY-----");

                this.UpdateSuggestedDNS(Convert.ToBase64String(oPublicEncoded.GetBytes()));
                this.SetDomainKeyPath(oFileDialog.FileName);
            }
        }

        /// <summary>
        /// Button "select key" in domain configuration have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDomainKeySelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFileDialog = new OpenFileDialog();

            oFileDialog.FileName = "key";
            oFileDialog.Filter = "Key files|*.xml;*.pem|All files|*.*";
            oFileDialog.Title = "Select a private key for signing";
            oFileDialog.InitialDirectory = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys");

            if (oFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.SetDomainKeyPath(oFileDialog.FileName);
                this.UpdateSuggestedDNS();
            }
        }

        /// <summary>
        /// Button "check DNS" in domain configuration have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDomainCheckDNS_Click(object sender, EventArgs e)
        {
            string sFullDomain = this.txtDomainSelector.Text + "._domainkey." + this.txtDomainName.Text;

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
                    this.txtDomainDNS.Text = oTxtRecord.TXT.Count > 0 ? oTxtRecord.TXT[0] : "No record found for " + sFullDomain;
                }
                else
                {
                    this.txtDomainDNS.Text = "No record found for " + sFullDomain;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Coldn't get DNS record:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtDomainDNS.Text = "Error getting record.";
            }
        }

        /// <summary>
        /// Button "save" in domain configuration have been click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDomainSave_Click(object sender, EventArgs e)
        {
            if (this.epvDomainSelector.GetError(txtDomainName) == "" && this.epvDomainSelector.GetError(txtDomainSelector) == "")
            {
                DomainElement oCurrentDomain;
                bool bAddToList = false;

                if (this.lbxDomains.SelectedItem != null)
                {
                    oCurrentDomain = (DomainElement)this.lbxDomains.SelectedItem;
                }
                else
                {
                    oCurrentDomain = new DomainElement();
                    bAddToList = true;
                }

                oCurrentDomain.Domain = this.txtDomainName.Text;
                oCurrentDomain.Selector = this.txtDomainSelector.Text;
                oCurrentDomain.PrivateKeyFile = this.txtDomainPrivateKeyFilename.Text;

                if (bAddToList)
                {
                    this.oConfig.Domains.Add(oCurrentDomain);
                    this.lbxDomains.Items.Add(oCurrentDomain);
                    this.lbxDomains.SelectedItem = oCurrentDomain;
                }

                if (this.SaveDkimSignerConfig())
                {
                    this.btDomainSave.Enabled = false;
                    this.btDomainDelete.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("You first need to fix the errors in your domain configuration before saving.", "Config error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// Button "Refresh" on EventLog TabPage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btEventLogRefresh_Click(object sender, EventArgs e)
        {
            refreshEventLog();
        }

        private void btUpgrade_Click(object sender, EventArgs e)
        {
            if (this.btUpgrade.Text == "Upgrade" || this.btUpgrade.Text == "Reinstall" ? MessageBox.Show("Do you really want to " + this.btUpgrade.Text.ToUpper() + " the DKIM Exchange Agent (new Version: " + txtDkimSignerAvailable.Text + ")?\n", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes : true)
            {
                try
                {
                    System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location, this.btUpgrade.Text == "Install" ? "--install" : ("--upgrade \"" + dkimSignerAvailable.ZipballUrl + "\""));

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Couldn't start the process :\n" + ex.Message, "Updater error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}