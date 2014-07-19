using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Serialization;
using System.Windows.Forms;

using ConfigurationSettings;
using Configuration.DkimSigner.GitHub;
using Configuration.DkimSigner.Properties;
using DkimSigner.RSA;
using Ionic.Zip;
using Heijden.DNS;

namespace Configuration.DkimSigner
{
    public partial class MainWindow : Form
    {
        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/

        private Settings config = null;

        private Release dkimSignerAvailable = null;
        private Version dkimSignerInstalled = null;

        private bool bDkimSignerEnabled = false;
        private bool bDataUpdated = false;
        
        private Thread thDkimSignerInstalled = null;
        private Thread thDkimSignerAvailable = null;

        delegate void SetDkimSignerInstalledCallback();
        delegate void SetDkimSignerAvailableCallback();

        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public MainWindow()
        {           
            this.InitializeComponent();

            this.cbLogLevel.SelectedItem = "Information";
            this.cbKeyLength.SelectedItem = "1024";
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
            this.UpdateVersions();
            this.LoadDkimSignerConfig();
        }

        /// <summary>
        /// Confirm the configuration saving before quit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //
            // IF any thread running, we stop them before exit
            //
            if (this.thDkimSignerAvailable != null && this.thDkimSignerAvailable.ThreadState == System.Threading.ThreadState.Running)
            {
                this.thDkimSignerAvailable.Abort();
            }

            if (this.thDkimSignerInstalled != null && this.thDkimSignerInstalled.ThreadState == System.Threading.ThreadState.Running)
            {
                this.thDkimSignerInstalled.Abort();
            }

            //
            // Check if the config have been change and haven't been save
            //
            if (!this.CheckSaveConfig())
            {
                e.Cancel = true;
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
                DomainElement selected = (DomainElement) this.lbxDomains.SelectedItem;
                this.txtDomainName.Text = selected.Domain;
                this.txtDomainSelector.Text = selected.Selector;
                this.txtDomainPrivateKeyFilename.Text = selected.PrivateKeyFile;
            
                this.bDataUpdated = false;

                this.updateSuggestedDNS();
                this.txtDomainDNS.Text = "";
                this.gbxDomainDetails.Enabled = true;
                this.btnDomainDelete.Enabled = true;
                this.btnDomainSave.Enabled = false;
            }
        }

        private void txtDomainName_TextChanged(object sender, EventArgs e)
        {
            this.bDataUpdated = true;
            this.btnDomainSave.Enabled = true;
            this.txtDNSName.Text = this.txtDomainSelector.Text + "._domainkey." + this.txtDomainName.Text + ".";
            
            if (Uri.CheckHostName(this.txtDomainName.Text) != UriHostNameType.Dns)
            {
                this.errorProvider.SetError(this.txtDomainName, "Invalid DNS name. Format: 'example.com'");
            }
            else
            {
                this.errorProvider.SetError(this.txtDomainName, null);
            }
        }

        private void txtDomainSelector_TextChanged(object sender, EventArgs e)
        {
            this.bDataUpdated = true;
            this.btnDomainSave.Enabled = true;
            this.txtDNSName.Text = this.txtDomainSelector.Text + "._domainkey." + this.txtDomainName.Text + ".";

            if (!Regex.IsMatch(this.txtDomainSelector.Text, @"^[a-zA-Z0-9_]+$", RegexOptions.None))
            {
                this.errorProvider.SetError(this.txtDomainSelector, "The selector should only contain characters, numbers and underscores.");
            }
            else
            {
                this.errorProvider.SetError(this.txtDomainSelector, null);
            }
        }

        private void timExchangeStatus_Tick(object sender, EventArgs e)
        {
            ServiceControllerStatus status;

            try
            {
                status = ExchangeHelper.getTransportServiceStatus();
            }
            catch (ExchangeHelperException ex)
            {
                this.timExchangeStatus.Enabled = false;
                this.lblExchangeStatus.Text = "";

                MessageBox.Show("Couldn't get MSExchangeTransport service status:\n" + ex.Message, "Service error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            this.lblExchangeStatus.Text = status.ToString();

            if (status == ServiceControllerStatus.Running)
            {
                this.btRestartTransportService.Text = "Restart MSExchangeTransport";
                this.btRestartTransportService.Enabled = true;
            }
            else if (status == ServiceControllerStatus.Stopped)
            {
                this.btRestartTransportService.Text = "Start MSExchangeTransport";
                this.btRestartTransportService.Enabled = true;
            }
            else
            {
                this.btRestartTransportService.Enabled = false;
            }
        }

        /**********************************************************/
        /******************* Internal functions *******************/
        /**********************************************************/

        /// <summary>
        /// Set the value of txtDkimSignerInstalled from CheckDkimSignerInstalledSafe (use by thread DkimSignerInstalled)
        /// </summary>
        /// <param name="dkimSignerInstalled"></param>
        private void SetDkimSignerInstalled()
        {
            if (this.dkimSignerInstalled != null)
            {
                this.txtDkimSignerInstalled.Text = this.dkimSignerInstalled.ToString();

                if (this.bDkimSignerEnabled)
                {
                    this.btDisable.Text = "Disable";
                }
                else
                {
                    this.btDisable.Text = "Enable";
                }

                this.btDisable.Enabled = true;
            }
            else
            {
                this.txtDkimSignerInstalled.Text = "Not installed";
                this.btDisable.Enabled = false;
            }

            this.InitUpdateButton();
        }

        /// <summary>
        ///  Set the value of txtDkimSignerAvailable from CheckDkimSignerAvailableSafe (use by thread DkimSignerAvailable)
        /// </summary>
        /// <param name="dkimSignerAvailable"></param>
        private void SetDkimSignerAvailable()
        {
            if (this.dkimSignerAvailable != null)
            {
                string version = this.dkimSignerAvailable.Version.ToString();

                Match match = Regex.Match(this.dkimSignerAvailable.TagName, @"v?((?:\d+\.){0,3}\d+)(?:-(alpha|beta)(?:\.(\d+))?)?", RegexOptions.IgnoreCase);

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
                this.txtChangelog.Text = this.dkimSignerAvailable.Body;
            }
            else
            {
                this.txtDkimSignerAvailable.Text = "Unknown";
                this.txtChangelog.Text = "Couldn't get current version.\r\nCheck your Internet connection or restart the application.";
                btInstallUpate.Enabled = false;
            }

            this.InitUpdateButton();
        }

        /// <summary>
        /// Updates the init button description and functionality.
        /// </summary>
        private void InitUpdateButton()
        {
            if (this.dkimSignerInstalled != null && this.dkimSignerAvailable != null)
            {
                if (this.dkimSignerInstalled.CompareTo(this.dkimSignerAvailable.Version) > 0)
                {
                    this.btInstallUpate.Text = "Downgrade";
                    this.btInstallUpate.Enabled = true;
                }
                else if (this.dkimSignerInstalled.CompareTo(this.dkimSignerAvailable.Version) == 0)
                {
                    this.btInstallUpate.Text = "Reinstall";
                    this.btInstallUpate.Enabled = true;
                }
                else
                {
                    this.btInstallUpate.Text = "Upgrade";
                    this.btInstallUpate.Enabled = true;
                }
            }
            else if (this.dkimSignerInstalled == null && this.dkimSignerAvailable != null)
            {
                this.btInstallUpate.Text = "Install";
                this.btInstallUpate.Enabled = true;
            }
            else
            {
                this.btInstallUpate.Text = "Update";
                this.btInstallUpate.Enabled = false;
            }

            if (this.dkimSignerInstalled != null)
            {
                this.btUninstall.Enabled = true;
                this.btUninstall.Text = "Uninstall";
            }
            else
            {
                this.btUninstall.Enabled = false;
                this.btUninstall.Text = "Uninstall";
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
            this.btDisable.Enabled = false;

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

            // Get Exchange version installed + load the current configuration
            this.txtExchangeInstalled.Text = ExchangeHelper.checkExchangeVersionInstalled();
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
        /// Thread safe function for the thread DkimSignerInstalled
        /// </summary>
        private void CheckDkimSignerInstalledSafe()
        {
            try
            {
                this.dkimSignerInstalled = Version.Parse(FileVersionInfo.GetVersionInfo(Path.Combine(Constants.DKIM_SIGNER_PATH, Constants.DKIM_SIGNER_AGENT_DLL)).ProductVersion);
            }
            catch (Exception)
            {
               this.dkimSignerInstalled = null;
            }

            if (this.dkimSignerInstalled != null)
            {
                try
                {
                    // ARGGGG!!! No a good way to implement this! Need to redesign Exchange class 
                    if (!ExchangeHelper.isAgentInstalled(out bDkimSignerEnabled))
                    {
                        this.dkimSignerInstalled = null;
                    }
                }
                catch (Exception)
                {
                    this.bDkimSignerEnabled = false;
                }
            }
            
            if (this.txtDkimSignerInstalled.InvokeRequired)
            {
                SetDkimSignerInstalledCallback d = new SetDkimSignerInstalledCallback(SetDkimSignerInstalled);
                this.Invoke(d);
            }
            else
            {
                this.SetDkimSignerInstalled();
            }
        }

        /// <summary>
        /// Thread safe function for the thread DkimSignerAvailable
        /// </summary>
        private void CheckDkimSignerAvailableSafe()
        {
            try
            {
                this.dkimSignerAvailable = ApiWrapper.getNewestRelease(cbxPrereleases.Checked);
            }
            catch (Exception)
            {
                this.dkimSignerAvailable = null;
            }

            if (this.txtDkimSignerInstalled.InvokeRequired)
            {
                SetDkimSignerAvailableCallback d = new SetDkimSignerAvailableCallback(SetDkimSignerAvailable);
                this.Invoke(d);
            }
            else
            {
                this.SetDkimSignerAvailable();
            }
        }

        /// <summary>
        /// Load the current configuration for Exchange DkimSigner from the registry
        /// </summary>
        private void LoadDkimSignerConfig()
        {
            try
            {
                this.config = Settings.LoadOrCreate(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml"));
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't load the settings file:\n" + e.Message + "\nSetting it to default values.", "Settings error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } 

            switch (config.Loglevel)
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
                    MessageBox.Show(Resources.MainWindows_BadLogLevel);
                    break;
            }

            this.rbRsaSha1.Checked = (config.SigningAlgorithm == DkimAlgorithmKind.RsaSha1);

            this.rbSimpleHeaderCanonicalization.Checked = (config.HeaderCanonicalization == DkimCanonicalizationKind.Simple);
            this.rbRelaxedHeaderCanonicalization.Checked = (config.HeaderCanonicalization == DkimCanonicalizationKind.Relaxed);
            this.rbSimpleBodyCanonicalization.Checked = (config.BodyCanonicalization == DkimCanonicalizationKind.Simple);
            this.rbRelaxedBodyCanonicalization.Checked = (config.BodyCanonicalization == DkimCanonicalizationKind.Relaxed);

            this.lbxHeadersToSign.Items.Clear();
            
            foreach (string str in config.HeadersToSign)
            {
                this.lbxHeadersToSign.Items.Add(str);
            }

            this.lbxHeadersToSign.SelectedItem = null;

            this.ReloadDomainsList();
            this.bDataUpdated = false;
        }

        /// <summary>
        /// Save the new configuration into registry for Exchange DkimSigner
        /// </summary>
        private bool SaveDkimSignerConfig()
        {
            config.Loglevel = this.cbLogLevel.SelectedIndex + 1;

            config.SigningAlgorithm = (this.rbRsaSha1.Checked ? DkimAlgorithmKind.RsaSha1 : DkimAlgorithmKind.RsaSha256);
            config.BodyCanonicalization = (this.rbSimpleBodyCanonicalization.Checked ? DkimCanonicalizationKind.Simple : DkimCanonicalizationKind.Relaxed);
            config.HeaderCanonicalization = (this.rbSimpleHeaderCanonicalization.Checked ? DkimCanonicalizationKind.Simple : DkimCanonicalizationKind.Relaxed);

            config.HeadersToSign.Clear();
            foreach (string str in lbxHeadersToSign.Items)
            {
                config.HeadersToSign.Add(str);
            }

            config.Save(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml"));

            this.bDataUpdated = false;
            return true;
        }

        private void ReloadDomainsList(string selectedDomain = null)
        {
            // Load the list of domains
            DomainElement currSel = null;

            if (lbxDomains.SelectedItem != null)
            {
                currSel = (DomainElement)lbxDomains.SelectedItem;
            }

            lbxDomains.Items.Clear();
            
            foreach (DomainElement domain in config.Domains)
            {
                lbxDomains.Items.Add(domain);
            }
            
            if (currSel != null)
            {
                lbxDomains.SelectedItem = currSel;
            }
        }

        /**********************************************************/
        /********************** Button click **********************/
        /**********************************************************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDisable_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.bDkimSignerEnabled)
                {
                    ExchangeHelper.disalbeTransportAgent();
                }
                else
                {
                    ExchangeHelper.enalbeTransportAgent();
                }

                ExchangeHelper.restartTransportService();
                
                this.UpdateVersions();
            }
            catch (ExchangeHelperException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btUninstall_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to UNINSTALL the DKIM Exchange Agent?\nPlease remove the following folder manually:\n" + Constants.DKIM_SIGNER_PATH, "Uninstall?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.performUninstall();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btRestartTransportService_Click(object sender, EventArgs e)
        {
            this.lblExchangeStatus.Text = "Restarting ...";
            Application.DoEvents();
            this.restartTransportService();
        }

        /// <summary>
        /// Update the current Configuration.ExchangeDkimSigner WindowsForm and the Exchange.DkimSigner transport agent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btInstallUpate_Click(object sender, EventArgs e)
        {
            if(this.btInstallUpate.Enabled == true)
            {
                switch (this.btInstallUpate.Text)
                {
                    case "Downgrade":
                        if (MessageBox.Show("Do you really want to downgrade the DKIM Exchange Agent from Version " + dkimSignerInstalled.ToString() + " to " + dkimSignerAvailable.Version.ToString() + "?", "Downgrade?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.performUpgrade();
                        }
                        break;
                    case "Upgrade":
                        if (MessageBox.Show("Do you really want to upgrade the DKIM Exchange Agent from Version " + dkimSignerInstalled.ToString() + " to " + dkimSignerAvailable.Version.ToString() + "?", "Upgrade?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.performUpgrade();
                        }
                        break;
                    case "Install":
                        if (MessageBox.Show("Do you really want to install the DKIM Exchange Agent version " + dkimSignerAvailable.Version.ToString() + "?", "Install?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            this.performUpgrade();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btInstallZip_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFileDialog = new OpenFileDialog();

            oFileDialog.FileName = "dkim-exchange.zip";
            oFileDialog.Filter = "ZIP files|*.zip";
            oFileDialog.Title = "Select the .zip file downloaded from github.com";

            if (oFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.extractAndInstall(oFileDialog.FileName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btHeaderAdd_Click(object sender, EventArgs e)
        {
            HeaderInputForm hif = new HeaderInputForm();

            if (hif.ShowDialog() == DialogResult.OK)
            {
                this.lbxHeadersToSign.Items.Add(hif.txtHeader.Text);
                this.lbxHeadersToSign.SelectedItem = hif.txtHeader;

                this.bDataUpdated = true;
            }
        }

        /// <summary>
        /// 
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
        /// Button "Save configuration" action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btSave_Click(object sender, EventArgs e)
        {
            this.SaveDkimSignerConfig();
        }

        /// <summary>
        /// 
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

                if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
            }

            this.bDataUpdated = false;
            lbxDomains.ClearSelected();
            txtDNSRecord.Text = "";
            txtDNSName.Text = "";
            txtDNSRecord.Text = "";
            gbxDomainDetails.Enabled = true;
            btnDomainDelete.Enabled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDomainDelete_Click(object sender, EventArgs e)
        {
            if (lbxDomains.SelectedItem != null)
            {
                DomainElement elem = (DomainElement)lbxDomains.SelectedItem;
                config.Domains.Remove(elem);

                lbxDomains.Items.Remove(elem);
                lbxDomains.SelectedItem = null;

            }

            string keyFile = System.IO.Path.Combine(Constants.DKIM_SIGNER_PATH, "keys", txtDomainPrivateKeyFilename.Text);

            List<string> files = new List<string>();
            files.Add(keyFile);
            files.Add(keyFile + ".pub");
            files.Add(keyFile + ".pem");

            foreach (string file in files)
            {
                if (System.IO.File.Exists(file) && MessageBox.Show("Do you want me to delete the key file?\n" + file, "Delete key?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("Couldn't delete file:\n" + file + "\n" + ex.Message, "Error deleting file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }

            this.SaveDkimSignerConfig();
        }

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
                oFileDialog.FileName = txtDomainName.Text + ".xml";
            }

            if (oFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider(Convert.ToInt32(this.cbKeyLength.Text, 10));
                CSInteropKeys.AsnKeyBuilder.AsnMessage publicEncoded = CSInteropKeys.AsnKeyBuilder.PublicKeyToX509(provider.ExportParameters(true));
                CSInteropKeys.AsnKeyBuilder.AsnMessage privateEncoded = CSInteropKeys.AsnKeyBuilder.PrivateKeyToPKCS8(provider.ExportParameters(true));

                File.WriteAllBytes(oFileDialog.FileName, Encoding.ASCII.GetBytes(provider.ToXmlString(true)));
                File.WriteAllText(oFileDialog.FileName + ".pub", "-----BEGIN PUBLIC KEY-----\r\n" + Convert.ToBase64String(publicEncoded.GetBytes()) + "\r\n-----END PUBLIC KEY-----");
                File.WriteAllText(oFileDialog.FileName + ".pem", "-----BEGIN RSA PRIVATE KEY-----\r\n" + Convert.ToBase64String(privateEncoded.GetBytes()) + "\r\n-----END RSA PRIVATE KEY-----");

                this.updateSuggestedDNS(Convert.ToBase64String(publicEncoded.GetBytes()));
                this.setDomainKeyPath(oFileDialog.FileName);
            }
        }

        private void btDomainKeySelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog oFileDialog = new OpenFileDialog();

            oFileDialog.FileName = "key";
            oFileDialog.Filter = "Key files|*.xml;*.pem|All files|*.*";
            oFileDialog.Title = "Select a private key for signing";
            oFileDialog.InitialDirectory = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys");

            if (oFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.setDomainKeyPath(oFileDialog.FileName);
                this.updateSuggestedDNS();
            }
        }

        private void btDomainCheckDNS_Click(object sender, EventArgs e)
        {
            string fullDomain = txtDomainSelector.Text + "._domainkey." + txtDomainName.Text;

            try
            {
                Resolver resolver = new Resolver();
                resolver.Recursion = true;
                resolver.UseCache = false;

                //first get the name server for the domain to avoid DNS caching
                Response response = resolver.Query(fullDomain, QType.NS, QClass.IN);
                if (response.RecordsRR.GetLength(0) > 0)
                {
                    //take first NS server
                    RR nsRecord = response.RecordsRR[0];
                    if (nsRecord.RECORD.RR.RECORD.GetType() == typeof(RecordSOA))
                    {
                        RecordSOA soa = (RecordSOA)nsRecord.RECORD.RR.RECORD;
                        resolver.DnsServer = soa.MNAME;
                    }
                }

                response = resolver.Query(fullDomain, QType.TXT, QClass.IN);

                if (response.RecordsTXT.GetLength(0) > 0)
                {
                    RecordTXT record = response.RecordsTXT[0];
                    if (record.TXT.Count > 0)
                    {
                        txtDomainDNS.Text = record.TXT[0];
                    }
                    else
                    {
                        txtDomainDNS.Text = "No record found for " + fullDomain;
                    }
                }
                else
                {
                    txtDomainDNS.Text = "No record found for " + fullDomain;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Coldn't get DNS record:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDomainDNS.Text = "Error getting record.";
            }
        }

        private void btDomainSave_Click(object sender, EventArgs e)
        {
            if (errorProvider.GetError(txtDomainName) != "" || errorProvider.GetError(txtDomainSelector) != "")
            {
                MessageBox.Show("You first need to fix the errors in your domain configuration before saving.", "Config error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DomainElement elem;
            bool addToList = false;

            if (lbxDomains.SelectedItem != null)
            {
                elem = (DomainElement)lbxDomains.SelectedItem;
            }
            else
            {
                elem = new DomainElement();
                addToList = true;
            }

            elem.Domain = txtDomainName.Text;
            elem.Selector = txtDomainSelector.Text;
            elem.PrivateKeyFile = txtDomainPrivateKeyFilename.Text;

            if (addToList)
            {
                config.Domains.Add(elem);
                lbxDomains.Items.Add(elem);
                lbxDomains.SelectedItem = elem;
            }

            if (this.SaveDkimSignerConfig())
            {
                btnDomainSave.Enabled = false;
                btnDomainDelete.Enabled = true;
            }
        }

        /*************************************************************************************************************/

        private void performUninstall()
        {
            try
            {
                ExchangeHelper.uninstallTransportAgent();

                if (MessageBox.Show("Transport Agent removed from Exchange. Would you like me to remove all the settings for Exchange DKIM Signer?'", "Remove settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (File.Exists(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml")))
                        File.Delete(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml"));
                }

                if (MessageBox.Show("Transport Agent removed from Exchange. Would you like me to remove the folder '" + Constants.DKIM_SIGNER_PATH + "' and all it's content?", "Remove files?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    var dir = new DirectoryInfo(Constants.DKIM_SIGNER_PATH);
                    dir.Delete(true);
                }
            }
            catch (ExchangeHelperException e)
            {
                MessageBox.Show(e.Message, "Uninstall error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.UpdateVersions();
        }

        private void performUpgrade()
        {
            if (dkimSignerAvailable == null)
                return;

            if (!CheckSaveConfig())
            {
                return;
            }

            btInstallUpate.Enabled = false;

            string tempDir = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString();
            string tempPath = tempDir + ".zip";
            DownloadProgressWindow dpw = new DownloadProgressWindow(dkimSignerAvailable.ZipballUrl, tempPath);

            if (dpw.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                extractAndInstall(tempPath);
            }

            /*** DEBUG ***/
            /*MessageBox.Show("Uninstall retval: " + ExchangeHelper.uninstallTransportAgent().ToString());
            MessageBox.Show("Install retval: " + ExchangeHelper.installTransportAgent().ToString());*/
        }

        private void extractAndInstall(string zipPath)
        {
            //TODO: Show extract progress
            string dir = zipPath.Substring(0, zipPath.LastIndexOf('.'));

            try
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't create directory:\n" + dir + "\n" + e.Message, "Directory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btInstallUpate.Enabled = true;
                return;
            }
            using (ZipFile zip1 = ZipFile.Read(zipPath))
            {
                foreach (ZipEntry e in zip1)
                {
                    e.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                }
            }

            string[] contents = Directory.GetDirectories(dir);
            if (contents.Length == 0)
            {
                MessageBox.Show("Downloaded .zip is empty. Please try again.", "Empty download", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btInstallUpate.Enabled = true;
                return;
            }
            string rootDir = contents[0];

            string exePath = Path.Combine(rootDir, @"Src\Configuration.DkimSigner\bin\Release\Configuration.DkimSigner.exe");
            if (System.Diagnostics.Debugger.IsAttached)
                // during development execute current exe
                exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            if (!System.IO.File.Exists(exePath))
            {
                MessageBox.Show("Executable not found within downloaded .zip is empty. Please try again.", "Missing .exe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btInstallUpate.Enabled = true;
                return;
            }
            string args = "";
            if (this.btInstallUpate.Enabled == true && this.btInstallUpate.Text == "Install")
            {
                args = "--install";
            }
            else
            {
                args = Constants.DKIM_SIGNER_PATH;

                if (System.Diagnostics.Debugger.IsAttached)
                    // during development install into updated subfolder
                    args = System.IO.Path.Combine(args, "Updated");
                args = "--upgrade \"" + args + "\"";
            }

            try
            {
                Process.Start(exePath, args);
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't start updater:\n" + e.Message, "Updater error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btInstallUpate.Enabled = true;
                return;
            }

            this.Close();
        }

        private void setDomainKeyPath(string path)
        {
            string keyDir = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys");
            if (path.StartsWith(keyDir))
            {
                path = path.Substring(keyDir.Length + 1);
            }
            else if (MessageBox.Show("It is strongly recommended to store all the keys in the directory\n" + keyDir + "\nDo you want me to move the key into this directory?", "Move key?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                List<string> files = new List<string>();
                files.Add(path);
                files.Add(path + ".pub");
                files.Add(path + ".pem");

                foreach (string file in files)
                {
                    if (System.IO.File.Exists(file))
                    {
                        string name = System.IO.Path.GetFileName(file);
                        string newPath = Path.Combine(keyDir, name);
                        try
                        {
                            System.IO.File.Move(file, newPath);
                            path = newPath.Substring(keyDir.Length - 4);
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show("Couldn't move file:\n" + file + "\nto\n" + newPath + "\n" + ex.Message, "Error moving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }
            txtDomainPrivateKeyFilename.Text = path;
            this.bDataUpdated = true;
            btnDomainSave.Enabled = true;
        }

        public static bool IsBase64String(string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        private void updateSuggestedDNS(string rsaPublicKeyBase64 = null)
        {
            if (rsaPublicKeyBase64 == null)
            {

                string pubKeyPath = txtDomainPrivateKeyFilename.Text;
                if (!Path.IsPathRooted(pubKeyPath))
                {
                    pubKeyPath = Path.Combine(Constants.DKIM_SIGNER_PATH, "keys", pubKeyPath);
                }

                pubKeyPath += ".pub";
                if (File.Exists(pubKeyPath))
                {
                    string[] contents = File.ReadAllLines(pubKeyPath);

                    if (contents.Length > 2 && contents[0].Equals("-----BEGIN PUBLIC KEY-----") && IsBase64String(contents[1]))
                    {
                        rsaPublicKeyBase64 = contents[1];
                    }
                    else
                    {
                        txtDNSRecord.Text = "No valid RSA pub key:\n" + pubKeyPath;
                        return;
                    }
                }
                else
                {
                    txtDNSRecord.Text = "No RSA pub key found:\n" + pubKeyPath;
                    return;
                }
            }

            txtDNSRecord.Text = "v=DKIM1; k=rsa; p=" + rsaPublicKeyBase64;
        }

        private bool restartTransportService()
        {
            bool status = false;

            try
            {
                ExchangeHelper.restartTransportService();
                status = true;
            }
            catch (ExchangeHelperException ex)
            {
                MessageBox.Show("Couldn't change MSExchangeTransport service status:\n" + ex.Message, "Service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return status;
        }
    }
}