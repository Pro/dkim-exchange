using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using ConfigurationSettings;
using DkimSigner.RSA;
using Configuration.DkimSigner.GitHub;
using Ionic.Zip;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.ServiceProcess;
using Heijden.DNS;
using Configuration.DkimSigner.Properties;

namespace Configuration.DkimSigner
{
    public enum UpdateButtonType
    {
        Install,
        Downgrade,
        Upgrade,
        Disabled
    };

    public partial class MainWindow : Form
    {
        private const string DKIM_SIGNER_PATH = @"C:\Program Files\Exchange DkimSigner\";
        private const string DKIM_SIGNER_DLL = @"ExchangeDkimSigner.dll";

        //private Dictionary<int, byte[]> attachments = new Dictionary<int, byte[]>();
        private Release dkimSignerAvailable = null;
        private System.Version dkimSignerInstalled = null;
        private bool dkimSignerEnabled = false;
        private bool dataUpdated = false;
        private UpdateButtonType updateButtonType = UpdateButtonType.Disabled;
        private bool isUpgrade = false;
        private string installPath = "";
        private bool isInstall = false;
        private Thread thDkimSignerInstalled = null;
        private Thread thDkimSignerAvailable = null;
        Settings config = null;

        delegate void SetDkimSignerInstalledCallback();
        delegate void SetDkimSignerAvailableCallback();

        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public MainWindow()
        {
            string[] args = Environment.GetCommandLineArgs();

            isUpgrade = (Array.IndexOf(args, "--upgrade") >= 0);
            if (isUpgrade)
            {
                int idx = Array.IndexOf(args, "--upgrade") + 1;
                if (args.Length <= idx)
                {
                    MessageBox.Show("Missing install path for update parameter.", "Invalid argument count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                installPath = args[idx];
            }
            isInstall = (Array.IndexOf(args, "--install") >= 0);
            if (isInstall)
            {
                installPath = @"C:\Program Files\Exchange DkimSigner";
            }

            if (!isInstall && !isUpgrade)
                installPath = System.IO.Path.GetDirectoryName(System.IO.Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location,".."));

            ExchangeHelper.AGENT_DIR = installPath;

            InitializeComponent();
            this.cbLogLevel.SelectedItem = "Information";
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
            if (isInstall || isUpgrade)
            {

                this.Hide();
                UpgradeWindow upw = new UpgradeWindow(Path.GetFullPath(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\..\..")), installPath);
                upw.ShowDialog();
                string newExec = System.IO.Path.Combine(installPath,@"Configuration\Configuration.DkimSigner.exe");
                if (!System.IO.File.Exists(newExec))
                {
                    MessageBox.Show(this, "Couldn't find 'Configuration.DkimSigner.exe' in \n" + installPath, "Exec error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else
                    Process.Start(newExec);
                this.dataUpdated = false;
                this.Close();
            }
            else
            {
                updateVersions();
                LoadDkimSignerConfig();
            }
        }

        /// <summary>
        /// Update the current available and installed version info
        /// </summary>
        private void updateVersions()
        {
            // Get Exchange.DkimSigner version installed
            txtDkimSignerInstalled.Text = "Loading ...";
            thDkimSignerInstalled = new Thread(new ThreadStart(this.CheckDkimSignerInstalledSafe));
            btnDisable.Enabled = false;

            // Get Exchange.DkimSigner version available
            txtDkimSignerAvailable.Text = "Loading ...";
            thDkimSignerAvailable = new Thread(new ThreadStart(this.CheckDkimSignerAvailableSafe));


            try
            {
                thDkimSignerInstalled.Start();
                thDkimSignerAvailable.Start();
            }
            catch (ThreadAbortException) { }

            // Get Exchange version installed + load the current configuration
            txtExchangeInstalled.Text = ExchangeHelper.checkExchangeVersionInstalled();
        }

        /// <summary>
        /// Confirm the configuration saving before quit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (thDkimSignerAvailable != null && thDkimSignerAvailable.ThreadState == System.Threading.ThreadState.Running)
                thDkimSignerAvailable.Abort();

            if (thDkimSignerInstalled != null && thDkimSignerInstalled.ThreadState == System.Threading.ThreadState.Running)
                thDkimSignerInstalled.Abort();

            if (!checkSaveConfig())
                e.Cancel = true;

        }

        /// <summary>
        /// Asks the user if he wants to save the current config and saves it.
        /// </summary>
        /// <returns>false if the user pressed cancel. true otherwise</returns>
        private bool checkSaveConfig()
        {
            if (this.dataUpdated)
            {
                DialogResult result = MessageBox.Show("Do you want to save your changes?", "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (!this.SaveDkimSignerConfig())
                    {
                        if (MessageBox.Show("Error saving config. Do you wan to close anyways? This will discard all the changes!", "Discard changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }
            return true;
        }

        private void rbRsaSha1_CheckedChanged(object sender, System.EventArgs e)
        {
            this.dataUpdated = true;
        }

        private void rbRsaSha256_CheckedChanged(object sender, System.EventArgs e)
        {
            this.dataUpdated = true;
        }

        private void rbSimpleHeaderCanonicalization_CheckedChanged(object sender, System.EventArgs e)
        {
            this.dataUpdated = true;
        }

        private void rbRelaxedHeaderCanonicalization_CheckedChanged(object sender, System.EventArgs e)
        {
            this.dataUpdated = true;
        }

        private void rbSimpleBodyCanonicalization_CheckedChanged(object sender, System.EventArgs e)
        {
            this.dataUpdated = true;
        }

        private void rbRelaxedBodyCanonicalization_CheckedChanged(object sender, System.EventArgs e)
        {
            this.dataUpdated = true;
        }

        private void cbLogLevel_TextChanged(object sender, System.EventArgs e)
        {
            this.dataUpdated = true;
        }

        private void txtHeaderToSign_TextChanged(object sender, System.EventArgs e)
        {
            this.dataUpdated = true;
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
            if (dkimSignerInstalled != null)
            {
                this.txtDkimSignerInstalled.Text = dkimSignerInstalled.ToString();
                if (this.dkimSignerEnabled)
                {
                    btnDisable.Text = "Disable";
                }
                else
                {
                    btnDisable.Text = "Enable";
                }
                btnDisable.Enabled = true;
            }
            else
            {
                this.txtDkimSignerInstalled.Text = "Not installed";
                btnDisable.Enabled = false;
            }
            this.initUpdateButton();
        }

        /// <summary>
        ///  Set the value of txtDkimSignerAvailable from CheckDkimSignerAvailableSafe (use by thread DkimSignerAvailable)
        /// </summary>
        /// <param name="dkimSignerAvailable"></param>
        private void SetDkimSignerAvailable()
        {
            if (dkimSignerAvailable != null)
            {
                string version = dkimSignerAvailable.Version.ToString();

                Match match = Regex.Match(dkimSignerAvailable.TagName, @"v?((?:\d+\.){0,3}\d+)(?:-(alpha|beta)(?:\.(\d+))?)?", RegexOptions.IgnoreCase);

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
                this.txtChangelog.Text = dkimSignerAvailable.Body;
            }
            else
            {
                this.txtDkimSignerAvailable.Text = "Unknown";
                this.txtChangelog.Text = "Couldn't get current version.\r\nCheck your Internet connection or restart the application.";
                btUpateInstall.Enabled = false;
            }

            this.initUpdateButton();
        }

        /// <summary>
        /// Updates the init button description and functionality.
        /// </summary>
        private void initUpdateButton()
        {
            if (this.dkimSignerInstalled!=null && this.dkimSignerAvailable!=null)
            {
                if (dkimSignerInstalled.CompareTo(dkimSignerAvailable.Version) > 0)
                {
                    updateButtonType = UpdateButtonType.Downgrade;
                    btUpateInstall.Text = "Downgrade";
                    btUpateInstall.Enabled = true;
                }
                else if (dkimSignerInstalled.CompareTo(dkimSignerAvailable.Version) == 0)
                {
                    updateButtonType = UpdateButtonType.Install;
                    btUpateInstall.Text = "Reinstall";
                    btUpateInstall.Enabled = true;
                } else {
                    updateButtonType = UpdateButtonType.Upgrade;
                    btUpateInstall.Text = "Upgrade";
                    btUpateInstall.Enabled = true;
                }
            }
            else if (this.dkimSignerInstalled == null && this.dkimSignerAvailable != null)
            {
                updateButtonType = UpdateButtonType.Install;
                btUpateInstall.Text = "Install";
                btUpateInstall.Enabled = true;
            }
            else
            {
                updateButtonType = UpdateButtonType.Disabled;
                btUpateInstall.Text = "Update";
                btUpateInstall.Enabled = false;
            }

            if (this.dkimSignerInstalled != null)
            {
                btUninstall.Enabled = true;
                btUninstall.Text = "Uninstall";
            }
            else
            {
                btUninstall.Enabled = false;
                btUninstall.Text = "Uninstall";
            }
        }

        /// <summary>
        /// Thread safe function for the thread DkimSignerInstalled
        /// </summary>
        private void CheckDkimSignerInstalledSafe()
        {
            try
            {
                dkimSignerInstalled = System.Version.Parse(FileVersionInfo.GetVersionInfo(System.IO.Path.Combine(DKIM_SIGNER_PATH, DKIM_SIGNER_DLL)).ProductVersion);
            }
            catch (Exception)
            {
               dkimSignerInstalled = null;
            }

            if (dkimSignerInstalled != null)
            {
                try
                {
                    if (!ExchangeHelper.isAgentInstalled(out dkimSignerEnabled))
                    {
                        dkimSignerInstalled = null;
                    }
                }
                catch (Exception)
                {
                    dkimSignerEnabled = false;
                }
            }
            
            if (this.txtDkimSignerInstalled.InvokeRequired)
            {
                SetDkimSignerInstalledCallback d = new SetDkimSignerInstalledCallback(SetDkimSignerInstalled);
                this.Invoke(d);
            }
            else
            {
                SetDkimSignerInstalled();
            }
        }

        /// <summary>
        /// Thread safe function for the thread DkimSignerAvailable
        /// </summary>
        private void CheckDkimSignerAvailableSafe()
        {
            try
            {
                dkimSignerAvailable = ApiWrapper.getNewestRelease(cbxPrereleases.Checked);
            }
            catch (Exception)
            {
                dkimSignerAvailable = null;
            }

            if (this.txtDkimSignerInstalled.InvokeRequired)
            {
                SetDkimSignerAvailableCallback d = new SetDkimSignerAvailableCallback(SetDkimSignerAvailable);
                this.Invoke(d);
            }
            else
            {
                SetDkimSignerAvailable();
            }
        }

        /// <summary>
        /// Load the current configuration for Exchange DkimSigner from the registry
        /// </summary>
        private void LoadDkimSignerConfig()
        {
            try
            {
                config = Settings.LoadOrCreate(Path.Combine(DKIM_SIGNER_PATH, "settings.xml"));
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
                this.lbxHeadersToSign.Items.Add(str);
            this.lbxHeadersToSign.SelectedItem = null;

            reloadDomainsList();
            this.dataUpdated = false;
        }

        private void reloadDomainsList(string selectedDomain = null)
        {
            // Load the list of domains
            DomainElement currSel = null;
            if (lbxDomains.SelectedItem != null)
                currSel = (DomainElement)lbxDomains.SelectedItem;
            lbxDomains.Items.Clear();
            foreach (DomainElement domain in config.Domains)
            {
                lbxDomains.Items.Add(domain);
            }
            if (currSel != null)
                lbxDomains.SelectedItem = currSel;
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
                config.HeadersToSign.Add(str);

            config.Save(Path.Combine(DKIM_SIGNER_PATH, "settings.xml"));

            this.dataUpdated = false;
            return true;
        }

        /**********************************************************/
        /********************** Button click **********************/
        /**********************************************************/

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
        /// Update the current Configuration.ExchangeDkimSigner WindowsForm and the Exchange.DkimSigner transport agent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btUpateInstall_Click(object sender, EventArgs e)
        {
            switch (updateButtonType)
            {
                case UpdateButtonType.Disabled:
                    return;
                case UpdateButtonType.Downgrade:
                    if (MessageBox.Show("Do you really want to DOWNGRADE the DKIM Exchange Agent from Version " + dkimSignerInstalled.ToString() + " to " + dkimSignerAvailable.Version.ToString() + "?", "Downgrade?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        performUpgrade();
                    }
                    return;
                case UpdateButtonType.Upgrade:
                    if (MessageBox.Show("Do you really want to upgrade the DKIM Exchange Agent from Version " + dkimSignerInstalled.ToString() + " to " + dkimSignerAvailable.Version.ToString() + "?", "Upgrade?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        performUpgrade();
                    }
                    return;
                case UpdateButtonType.Install:
                    if (MessageBox.Show("Do you really want to install the DKIM Exchange Agent version " + dkimSignerAvailable.Version.ToString() + "?", "Install?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        performUpgrade();
                    }
                    return;
            }
        }


        private void btUninstall_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to UNINSTALL the DKIM Exchange Agent?\nPlease remove the following folder manually:\n" + ExchangeHelper.AGENT_DIR, "Uninstall?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                performUninstall();
            }
        }

        private void performUninstall()
        {
            try
            {
                ExchangeHelper.uninstallTransportAgent();

                if (MessageBox.Show("Transport Agent removed from Exchange. Would you like me to remove all the settings for Exchange DKIM Signer?'", "Remove settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==System.Windows.Forms.DialogResult.Yes){
                    if (File.Exists(Path.Combine(DKIM_SIGNER_PATH, "settings.xml")))
                        File.Delete(Path.Combine(DKIM_SIGNER_PATH, "settings.xml"));
                }

                if (MessageBox.Show("Transport Agent removed from Exchange. Would you like me to remove the folder '" + ExchangeHelper.AGENT_DIR + "' and all it's content?", "Remove files?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    var dir = new DirectoryInfo(ExchangeHelper.AGENT_DIR);
                    dir.Delete(true);
                }
            } catch (ExchangeHelperException e) {
                MessageBox.Show(e.Message, "Uninstall error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.updateVersions();
        }

        private void performUpgrade()
        {
            if (dkimSignerAvailable == null)
                return;

            if (!checkSaveConfig())
            {
                return;
            }

            btUpateInstall.Enabled = false;

            string tempDir = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString();
            string tempPath = tempDir + ".zip";
            DownloadProgressWindow dpw = new DownloadProgressWindow(dkimSignerAvailable.ZipballUrl, tempPath);

            if (dpw.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                extractAndInstall(tempPath);
            }

            /*
             * 
            //testing:
            MessageBox.Show("Uninstall retval: " + ExchangeHelper.uninstallTransportAgent().ToString());
            MessageBox.Show("Install retval: " + ExchangeHelper.installTransportAgent().ToString());*/


        }

        private void extractAndInstall(string zipPath)
        {
            //TODO: Show extract progress
            string dir = zipPath.Substring(0,zipPath.LastIndexOf('.'));
            try
            {
                System.IO.Directory.CreateDirectory(dir);
            }
            catch (Exception e)
            {
                MessageBox.Show("Couldn't create directory:\n" + dir + "\n" + e.Message, "Directory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btUpateInstall.Enabled = true;
                return;
            }
            using (ZipFile zip1 = ZipFile.Read(zipPath))
            {
                // here, we extract every entry, but we could extract conditionally
                // based on entry name, size, date, checkbox status, etc.  
                foreach (ZipEntry e in zip1)
                {
                    e.Extract(dir, ExtractExistingFileAction.OverwriteSilently);
                }
            }

            string[] contents = Directory.GetDirectories(dir);
            if (contents.Length == 0)
            {
                MessageBox.Show("Downloaded .zip is empty. Please try again.", "Empty download", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btUpateInstall.Enabled = true;
                return;
            }
            string rootDir = contents[0];

            string exePath = System.IO.Path.Combine(rootDir, @"Src\Configuration.DkimSigner\bin\Release\Configuration.DkimSigner.exe");
            if (System.Diagnostics.Debugger.IsAttached)
                // during development execute current exe
                exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            if (!System.IO.File.Exists(exePath))
            {
                MessageBox.Show("Executable not found within downloaded .zip is empty. Please try again.", "Missing .exe", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btUpateInstall.Enabled = true;
                return;
            }
            string args = "";
            if (updateButtonType == UpdateButtonType.Install)
            {
                args = "--install";
            }
            else
            {
                args = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
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
                btUpateInstall.Enabled = true;
                return;
            }
            this.Close();
        }

        private void cbxPrereleases_CheckedChanged(object sender, EventArgs e)
        {
            updateVersions();
        }

        private void btnInstallZip_Click(object sender, EventArgs e)
        {
            if (openZipFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                extractAndInstall(openZipFileDialog.FileName);
            }
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.dkimSignerEnabled)
                {
                    ExchangeHelper.disalbeTransportAgent();
                }
                else
                    ExchangeHelper.enalbeTransportAgent();
                ExchangeHelper.restartTransportService();
                updateVersions();
            }
            catch (ExchangeHelperException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lbxDomains_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbxDomains.SelectedItems.Count == 0)
            {

                tbxDomainName.Text = "";
                tbxDomainSelector.Text = "";
                tbxDomainPrivateKeyFilename.Text = "";
                tbxDomainDNS.Text = "";
                gbxDomainDetails.Enabled = false;
                return;
            }
            DomainElement selected = (DomainElement)lbxDomains.SelectedItem;
            tbxDomainName.Text = selected.Domain;
            tbxDomainSelector.Text = selected.Selector;
            tbxDomainPrivateKeyFilename.Text = selected.PrivateKeyFile;
            this.dataUpdated = false;

            updateSuggestedDNS();
            gbxDomainDetails.Enabled = true;
            btnDomainDelete.Enabled = true;
            btnDomainSave.Enabled = false;

        }

        private void updateDNSRecordInfo()
        {
            string fullDomain = tbxDomainSelector.Text + "._domainkey." + tbxDomainName.Text;
            try
            {
                Resolver resolver = new Resolver();
                resolver.Recursion = true;
                resolver.UseCache = false;

                //first get the name server for the domain to avoid DNS caching
                Response response = resolver.Query(fullDomain, QType.NS, QClass.IN);
                if (response.RecordsRR.GetLength(0) > 0) {
                    //take first NS server
                    RR nsRecord = response.RecordsRR[0];
                    if (nsRecord.RECORD.RR.RECORD.GetType() == typeof(RecordSOA))
                    {
                        RecordSOA soa = (RecordSOA)nsRecord.RECORD.RR.RECORD;
                        resolver.DnsServer = soa.MNAME;
                    }
                }

                response = resolver.Query(fullDomain, QType.TXT, QClass.IN);

                if (response.RecordsTXT.GetLength(0) > 0) {
                    RecordTXT record = response.RecordsTXT[0];
                    if (record.TXT.Count > 0) {
                        tbxDomainDNS.Text = record.TXT[0];
                    } else {
                        tbxDomainDNS.Text = "No record found for " + fullDomain;
                    }
                } else {
                    tbxDomainDNS.Text = "No record found for " + fullDomain;
                }
                                  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Coldn't get DNS record:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbxDomainDNS.Text = "Error getting record.";
            }
        }

        private void btnDomainSave_Click(object sender, EventArgs e)
        {
            if (errorProvider.GetError(tbxDomainName) != "" || errorProvider.GetError(tbxDomainSelector) != "")
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

            elem.Domain = tbxDomainName.Text;
            elem.Selector = tbxDomainSelector.Text;
            elem.PrivateKeyFile = tbxDomainPrivateKeyFilename.Text;

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


        private void btnDomainCheckDNS_Click(object sender, EventArgs e)
        {
            updateDNSRecordInfo();
        }

        private void btnDomainDelete_Click(object sender, EventArgs e)
        {
            if (lbxDomains.SelectedItem != null)
            {
                DomainElement elem = (DomainElement)lbxDomains.SelectedItem;
                config.Domains.Remove(elem);

                lbxDomains.Items.Remove(elem);
                lbxDomains.SelectedItem = null;

            }

            string keyFile = System.IO.Path.Combine(DKIM_SIGNER_PATH, "keys",tbxDomainPrivateKeyFilename.Text);

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

        private void btnAddDomain_Click(object sender, EventArgs e)
        {
            if (this.dataUpdated)
            {
                DialogResult result = MessageBox.Show("Do you want to save the current changes?", "Save changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    if (!SaveDkimSignerConfig())
                        return;
                } if (result == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }
            this.dataUpdated = false;
            lbxDomains.ClearSelected();
            tbxDNSRecord.Text = "";
            tbxDNSName.Text = "";
            tbxDNSRecord.Text = "";
            gbxDomainDetails.Enabled = true;
            btnDomainDelete.Enabled = false;
        }

        private void tbxDomainName_TextChanged(object sender, EventArgs e)
        {
            this.dataUpdated = true;
            btnDomainSave.Enabled = true;
            tbxDNSName.Text = tbxDomainSelector.Text + "._domainkey." + tbxDomainName.Text + ".";
            if (Uri.CheckHostName(tbxDomainName.Text) != UriHostNameType.Dns)
            {
                errorProvider.SetError(tbxDomainName, "Invalid DNS name. Format: 'example.com'");
            }
            else
            {
                errorProvider.SetError(tbxDomainName, null);
            }
        }

        private void tbxDomainSelector_TextChanged(object sender, EventArgs e)
        {
            this.dataUpdated = true;
            btnDomainSave.Enabled = true;
            tbxDNSName.Text = tbxDomainSelector.Text + "._domainkey." + tbxDomainName.Text + ".";
            if (!Regex.IsMatch(tbxDomainSelector.Text, @"^[a-zA-Z0-9_]+$", RegexOptions.None))
            {
                errorProvider.SetError(tbxDomainSelector, "The selector should only contain characters, numbers and underscores.");
            }
            else
            {
                errorProvider.SetError(tbxDomainSelector, null);
            }
        }

        private void setDomainKeyPath(string path)
        {
            string keyDir = System.IO.Path.Combine(DKIM_SIGNER_PATH, "keys");
            if (path.StartsWith(keyDir))
            {
                path = path.Substring(keyDir.Length +1);
            }
            else if (MessageBox.Show("It is strongly recommended to store all the keys in the directory\n" + keyDir + "\nDo you want me to move the key into this directory?", "Move key?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==System.Windows.Forms.DialogResult.Yes)
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
                            System.IO.File.Move(file,newPath);
                            path = newPath.Substring(keyDir.Length - 4);
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show("Couldn't move file:\n" + file + "\nto\n" +  newPath + "\n"+ ex.Message, "Error moving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
            }
            tbxDomainPrivateKeyFilename.Text = path;
            this.dataUpdated = true;
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
                
                string pubKeyPath = tbxDomainPrivateKeyFilename.Text;
                if (!Path.IsPathRooted(pubKeyPath))
                    pubKeyPath = System.IO.Path.Combine(DKIM_SIGNER_PATH,"keys", pubKeyPath);
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
                        tbxDNSRecord.Text = "No valid RSA pub key:\n" + pubKeyPath;
                        return;
                    }
                }
                else
                {
                    tbxDNSRecord.Text = "No RSA pub key found:\n" + pubKeyPath;
                    return;
                }
            }

            tbxDNSRecord.Text = "v=DKIM1; k=rsa; p=" + rsaPublicKeyBase64;
        }

        private void btDomainKeyGenerate_Click(object sender, EventArgs e)
        {
            saveKeyFileDialog.InitialDirectory = System.IO.Path.Combine(DKIM_SIGNER_PATH, "keys");

            if (!System.IO.Directory.Exists(saveKeyFileDialog.InitialDirectory))
            {
                System.IO.Directory.CreateDirectory(saveKeyFileDialog.InitialDirectory);
            }

            if (tbxDomainName.Text.Length > 0)
                saveKeyFileDialog.FileName = tbxDomainName.Text + ".xml";

            if (saveKeyFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                RSACryptoServiceProvider provider = new RSACryptoServiceProvider();


                File.WriteAllBytes(saveKeyFileDialog.FileName, Encoding.ASCII.GetBytes(provider.ToXmlString(true)));

                

                CSInteropKeys.AsnKeyBuilder.AsnMessage publicEncoded = CSInteropKeys.AsnKeyBuilder.PublicKeyToX509(provider.ExportParameters(true));
                updateSuggestedDNS(Convert.ToBase64String(publicEncoded.GetBytes()));
                File.WriteAllText(saveKeyFileDialog.FileName + ".pub", "-----BEGIN PUBLIC KEY-----\r\n" + Convert.ToBase64String(publicEncoded.GetBytes()) + "\r\n-----END PUBLIC KEY-----");
                CSInteropKeys.AsnKeyBuilder.AsnMessage privateEncoded = CSInteropKeys.AsnKeyBuilder.PrivateKeyToPKCS8(provider.ExportParameters(true));
                File.WriteAllText(saveKeyFileDialog.FileName + ".pem", "-----BEGIN RSA PRIVATE KEY-----\r\n" + Convert.ToBase64String(privateEncoded.GetBytes()) + "\r\n-----END RSA PRIVATE KEY-----");
                
                
                setDomainKeyPath(saveKeyFileDialog.FileName);
            }
        }

        private void btnDomainKeySelect_Click(object sender, EventArgs e)
        {
            openKeyFileDialog.InitialDirectory = System.IO.Path.Combine(DKIM_SIGNER_PATH, "keys");

            if (openKeyFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                setDomainKeyPath(openKeyFileDialog.FileName);
                updateSuggestedDNS();
            }

        }

        private void btnRestartTransportService_Click(object sender, EventArgs e)
        {
            lblExchangeStatus.Text = "Restarting ...";
            Application.DoEvents();
            restartTransportService();
        }

        private bool restartTransportService()
        {
            try
            {
                ExchangeHelper.restartTransportService();
            }
            catch (ExchangeHelperException ex)
            {
                MessageBox.Show("Couldn't change MSExchangeTransport service status:\n" + ex.Message, "Service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
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
                timExchangeStatus.Enabled = false;
                lblExchangeStatus.Text = "";
                MessageBox.Show("Couldn't get MSExchangeTransport service status:\n" + ex.Message, "Service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            lblExchangeStatus.Text = status.ToString();
            if (status == ServiceControllerStatus.Running)
            {
                btnRestartTransportService.Text = "Restart MSExchangeTransport";
                btnRestartTransportService.Enabled = true;
            }
            else if (status == ServiceControllerStatus.Stopped)
            {
                btnRestartTransportService.Text = "Start MSExchangeTransport";
                btnRestartTransportService.Enabled = true;
            }
            else
            {
                btnRestartTransportService.Enabled = false;
            }
        }

        private void lbxHeadersToSign_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnHeaderDelete.Enabled = (lbxHeadersToSign.SelectedItem != null);
        }

        private void btnHeaderDelete_Click(object sender, EventArgs e)
        {
            if (lbxHeadersToSign.SelectedItem == null)
                return;
            lbxHeadersToSign.Items.Remove(lbxHeadersToSign.SelectedItem);
        }

        private void btnHeaderAdd_Click(object sender, EventArgs e)
        {
            HeaderInputForm hif = new HeaderInputForm();
            if (hif.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lbxHeadersToSign.Items.Add(hif.header);
                lbxHeadersToSign.SelectedItem = hif.header;
            }
        }


    }
}