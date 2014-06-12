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
using DkimSigner.Properties;
using Configuration.DkimSigner.GitHub;
using Ionic.Zip;

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

        private Dictionary<int, byte[]> attachments = new Dictionary<int, byte[]>();
        private Release dkimSignerAvailable = null;
        private System.Version dkimSignerInstalled = null;
        private bool dataUpdated = false;
        private UpdateButtonType updateButtonType = UpdateButtonType.Disabled;
        private bool isUpgrade = false;
        private string installPath = "";
        private bool isInstall = false;

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
            Thread thDkimSignerInstalled = new Thread(new ThreadStart(this.CheckDkimSignerInstalledSafe));
            thDkimSignerInstalled.Start();

            // Get Exchange.DkimSigner version available
            txtDkimSignerAvailable.Text = "Loading ...";
            Thread thDkimSignerAvailable = new Thread(new ThreadStart(this.CheckDkimSignerAvailableSafe));
            thDkimSignerAvailable.Start();

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
                    this.SaveDkimSignerConfig();
                }
                else if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Add row numbers in the dgvDomainConfiguration DataGridView in the row header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDomainConfiguration_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        /// <summary>
        /// Open private key information by clicking in private key file in the dgvDomainConfiguration DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDomainConfiguration_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            string domain = string.Empty;
            string selector = string.Empty;
            string filename = string.Empty;

            if (dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[0].Value != null)
                domain = dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[0].Value.ToString();

            if (dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[1].Value != null)
                selector = dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[1].Value.ToString();

            if (dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[2].Value != null)
                filename = dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[2].Value.ToString();

            if (e.ColumnIndex == 2 && domain != string.Empty && selector != string.Empty && filename != string.Empty)
            {
                byte[] binaryData = attachments[dgvDomainConfiguration.SelectedCells[2].RowIndex];

                PrivateKeyWindows form = new PrivateKeyWindows(domain, selector, filename);
                form.ShowDialog();

                if (dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[2].Value.ToString() != form.txtFilename.Text)
                {
                    this.dataUpdated = true;

                    dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[2].Value = form.txtFilename.Text;
                }
            }
        }

        /// <summary>
        /// Reconfigure the private key internal structure when a row have been deleted in the dgvDomainConfiguration DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDomainConfiguration_UserDeletingRow(object sender, System.Windows.Forms.DataGridViewRowCancelEventArgs e)
        {
            int total = attachments.Count;
            int rowIndex = dgvDomainConfiguration.SelectedCells[0].RowIndex;

            attachments.Remove(rowIndex);
            for (int i = rowIndex+1; i < total; i++)
            {
                byte[] value = attachments[i];
                attachments.Remove(i);
                attachments[i-1] = value;
            }
        }

        /// <summary>
        /// Disable all domain configuration buttons when a cell begin to be editing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDomainConfiguration_CellBeginEdit(object sender, System.Windows.Forms.DataGridViewCellCancelEventArgs e)
        {
            this.dataUpdated = true;

            this.btGenerate.Enabled = false;
            this.btUpload.Enabled = false;
            this.btDownload.Enabled = false;
        }

        /// <summary>
        /// Enable / Disable the buttons when the current cell changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDomainConfiguration_CurrentCellChanged(object sender, System.EventArgs e)
        {
            string domain = string.Empty;
            string selector = string.Empty;

            if (dgvDomainConfiguration.SelectedRows[0].Cells[0].Value != null)
                domain = dgvDomainConfiguration.SelectedRows[0].Cells[0].Value.ToString();

            if (dgvDomainConfiguration.SelectedRows[0].Cells[1].Value != null)
                selector = dgvDomainConfiguration.SelectedRows[0].Cells[1].Value.ToString();

            if (domain != string.Empty && selector != string.Empty)
            {
                this.btGenerate.Enabled = true;
                this.btUpload.Enabled = true;
                this.btDownload.Enabled = true;
            }
            else
            {
                this.btGenerate.Enabled = false;
                this.btUpload.Enabled = false;
                this.btDownload.Enabled = false;
            }
        }

        /// <summary>
        /// Validate the current row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDomainConfiguration_RowValidating(object sender, System.Windows.Forms.DataGridViewCellCancelEventArgs e)
        {
            string domain = string.Empty;
            string selector = string.Empty;
            string filename = string.Empty;

            // Get the domain
            if (dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[0].Value != null)
                domain = dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[0].Value.ToString();

            // Get the domain
            if (dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[1].Value != null)
                selector = dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[1].Value.ToString();

            // If (the domain or the selector is empty string) and the domain and selector are both empty
            if ((domain == string.Empty || selector == string.Empty) && domain != selector)
                e.Cancel = true;
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
            }
            else
            {
                this.txtDkimSignerInstalled.Text = "Not installed";
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
                if (dkimSignerAvailable.Prerelease)
                {
                    version += " (beta)";
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
                dkimSignerInstalled = System.Version.Parse(FileVersionInfo.GetVersionInfo(DKIM_SIGNER_PATH + DKIM_SIGNER_DLL).ProductVersion);
            }
            catch (Exception)
            {
               dkimSignerInstalled = null;
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
                dkimSignerAvailable = ApiWrapper.getNewestRelease();
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
            if (RegistryHelper.Open(@"Software\Exchange DkimSigner") != null)
            {
                // Load the log level.
                int logLevel = 0;
                try
                {
                    string temp = RegistryHelper.Read("LogLevel", @"Software\Exchange DkimSigner");

                    if (temp != null)
                        logLevel = Convert.ToInt32(RegistryHelper.Read("LogLevel", @"Software\Exchange DkimSigner"));
                }
                catch (FormatException){}
                catch (OverflowException){}

                if(logLevel == 1)
                {
                    this.cbLogLevel.Text = "Error";
                }
                else if(logLevel == 2)
                {
                    this.cbLogLevel.Text = "Warning";
                }
                else if(logLevel == 3)
                {
                    this.cbLogLevel.Text = "Information";
                }
                else
                {
                    this.cbLogLevel.Text = "Information";
                    MessageBox.Show(Resources.MainWindows_BadLogLevel);
                }

                // Load the signing algorithm.
                try
                {
                    DkimAlgorithmKind signingAlgorithm = (DkimAlgorithmKind)Enum.Parse(typeof(DkimAlgorithmKind), RegistryHelper.Read("Algorithm", @"Software\Exchange DkimSigner\DKIM"), true);

                    if (signingAlgorithm == DkimAlgorithmKind.RsaSha1)
                        this.rbRsaSha1.Checked = true;
                    else
                        this.rbRsaSha256.Checked = true;
                }
                catch (Exception)
                {
                    MessageBox.Show(Resources.MainWindows_BadDkimAlgorithmConfig);
                }

                // Load the header canonicalization algorithm.
                try
                {
                    DkimCanonicalizationKind headerCanonicalization = (DkimCanonicalizationKind)Enum.Parse(typeof(DkimCanonicalizationKind), RegistryHelper.Read("HeaderCanonicalization", @"Software\Exchange DkimSigner\DKIM"), true);

                    if (headerCanonicalization == DkimCanonicalizationKind.Simple)
                        this.rbSimpleHeaderCanonicalization.Checked = true;
                    else
                        this.rbRelaxedHeaderCanonicalization.Checked = true;
                }
                catch (Exception)
                {
                    MessageBox.Show(Resources.MainWindows_BadDkimCanonicalizationHeaderConfig);
                }

                // Load the body canonicalization algorithm.
                try
                {
                    DkimCanonicalizationKind bodyCanonicalization = (DkimCanonicalizationKind)Enum.Parse(typeof(DkimCanonicalizationKind), RegistryHelper.Read("BodyCanonicalization", @"Software\Exchange DkimSigner\DKIM"), true);

                    if (bodyCanonicalization == DkimCanonicalizationKind.Simple)
                        this.rbSimpleBodyCanonicalization.Checked = true;
                    else
                        this.rbRelaxedBodyCanonicalization.Checked = true;
                }
                catch (Exception)
                {
                    MessageBox.Show(Resources.MainWindows_BadDkimCanonicalizationBodyConfig);
                }

                // Load the list of headers to sign in each message.
                string unparsedHeaders = RegistryHelper.Read("HeadersToSign", @"Software\Exchange DkimSigner\DKIM");
                if (unparsedHeaders != null)
                {
                    this.txtHeaderToSign.Text = unparsedHeaders;
                }

                // Load the list of domains
                string[] domainNames = RegistryHelper.GetSubKeyName(@"Software\Exchange DkimSigner\Domain");
                if (domainNames != null)
                {
                    int i = 0;
                    foreach (string domainName in domainNames)
                    {
                        string selector = RegistryHelper.Read("Selector", @"Software\Exchange DkimSigner\Domain\" + domainName);
                        string privateKeyFile = RegistryHelper.Read("PrivateKeyFile", @"Software\Exchange DkimSigner\Domain\" + domainName);

                        this.dgvDomainConfiguration.Rows.Add(   domainName,
                                                                selector,
                                                                privateKeyFile);

                        attachments[i++] = File.ReadAllBytes(DKIM_SIGNER_PATH + @"\keys\" + privateKeyFile);
                    }

                    this.dgvDomainConfiguration.Rows[0].Selected = true;
                }

            }
            this.dataUpdated = false;
        }

        /// <summary>
        /// Save the new configuration into registry for Exchange DkimSigner
        /// </summary>
        private void SaveDkimSignerConfig()
        {
            bool status = true;

            status = status && RegistryHelper.Write("LogLevel", this.cbLogLevel.SelectedIndex + 1, @"Software\Exchange DkimSigner");
            if (!status)
                MessageBox.Show("Error! Impossible to change the log level.");

            status = status && RegistryHelper.Write("Algorithm", this.rbRsaSha1.Checked ? this.rbRsaSha1.Text : this.rbRsaSha256.Text, @"Software\Exchange DkimSigner\DKIM");
            if (!status)
                MessageBox.Show("Error! Impossible to change the algorithm.");

            status = status && RegistryHelper.Write("HeaderCanonicalization", this.rbSimpleHeaderCanonicalization.Checked ? this.rbSimpleHeaderCanonicalization.Text : this.rbRelaxedHeaderCanonicalization.Text, @"Software\Exchange DkimSigner\DKIM");
            if (!status)
                MessageBox.Show("Error! Impossible to change the header canonicalization.");

            status = status && RegistryHelper.Write("BodyCanonicalization", this.rbSimpleBodyCanonicalization.Checked ? this.rbSimpleBodyCanonicalization.Text : this.rbRelaxedBodyCanonicalization.Text, @"Software\Exchange DkimSigner\DKIM");
            if (!status)
                MessageBox.Show("Error! Impossible to change the body canonicalization.");

            status = status && RegistryHelper.Write("HeadersToSign", this.txtHeaderToSign.Text, @"Software\Exchange DkimSigner\DKIM");
            if (!status)
                MessageBox.Show("Error! Impossible to change the headers to sign.");

            RegistryHelper.DeleteSubKeyTree("Domain", @"Software\Exchange DkimSigner\");
            Array.ForEach(Directory.GetFiles(DKIM_SIGNER_PATH + @"\keys\"), File.Delete);
            dgvDomainConfiguration.AllowUserToAddRows = false;
            if (dgvDomainConfiguration.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in this.dgvDomainConfiguration.Rows)
                {
                    if (row.Cells[0].Value != null &&
                        row.Cells[0].Value.ToString() != string.Empty &&
                        row.Cells[1].Value != null &&
                        row.Cells[1].Value.ToString() != string.Empty &&
                        row.Cells[2].Value != null &&
                        row.Cells[2].Value.ToString() != string.Empty)
                    {
                        string domainName = row.Cells[0].Value.ToString();
                        string selector = row.Cells[1].Value.ToString();
                        string privateKeyFile = row.Cells[2].Value.ToString();

                        status = status && RegistryHelper.Write("Selector", selector, @"Software\Exchange DkimSigner\Domain\" + domainName);
                        status = status && RegistryHelper.Write("PrivateKeyFile", privateKeyFile, @"Software\Exchange DkimSigner\Domain\" + domainName);

                        byte[] byteData = null;
                        byteData = attachments[row.Index];
                        File.WriteAllBytes(DKIM_SIGNER_PATH + @"\keys\" + privateKeyFile, byteData);
                    }
                    else
                    {
                        MessageBox.Show("Impossible to save the configuration for all the domain configurations. The line number " + (row.Index+1) + " is incomplet.");
                    }
                }
            }
            dgvDomainConfiguration.AllowUserToAddRows = true;

            this.dataUpdated = false;

            if (status)
                MessageBox.Show("The configuration has been updated.");
            else
                MessageBox.Show("One or many errors happened! All specified configurations haven't been updated.");
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
        /// Button "Generate" in domain configuration action - Generate a new private key file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btGenerate_Click(object sender, EventArgs e)
        {
            byte[] binaryData = RSACryptoHelper.GenerateXMLEncodedRsaPrivateKey();
            string domain = dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
            string selector = dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[1].Value.ToString();

            PrivateKeyWindows form = new PrivateKeyWindows(domain, selector);
            form.ShowDialog();

            dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[2].Value = form.txtFilename.Text;

            if (attachments.ContainsKey(dgvDomainConfiguration.SelectedCells[0].RowIndex))
            {
                attachments[dgvDomainConfiguration.SelectedCells[0].RowIndex] = binaryData;
            }
            else
            {
                attachments.Add(dgvDomainConfiguration.SelectedCells[0].RowIndex, binaryData);
            }
        }

        /// <summary>
        /// Button "Upload" in domain configuration action - Upload the selected private key file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btUpload_Click(object sender, EventArgs e)
        {
            if (dgvDomainConfiguration.SelectedCells.Count > 0)
            {
                using (OpenFileDialog fileDialog = new OpenFileDialog())
                {
                    fileDialog.CheckFileExists = true;
                    fileDialog.CheckPathExists = true;
                    fileDialog.Filter = "All Files|*.*";
                    fileDialog.Title = "Select a file";
                    fileDialog.Multiselect = false;

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fileInfo = new FileInfo(fileDialog.FileName);
                        byte[] binaryData = File.ReadAllBytes(fileDialog.FileName);
                        if (RSACryptoHelper.GetFormatFromEncodedRsaPrivateKey(binaryData) != RSACryptoFormat.UNKNOWN)
                        {
                            dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[2].Value = fileInfo.Name;

                            if (attachments.ContainsKey(dgvDomainConfiguration.SelectedCells[0].RowIndex))
                            {
                                attachments[dgvDomainConfiguration.SelectedCells[0].RowIndex] = binaryData;
                            }
                            else
                            {
                                attachments.Add(dgvDomainConfiguration.SelectedCells[0].RowIndex, binaryData);
                            }
                        }
                        else
                        {
                            MessageBox.Show("The format of the private key file you try to import is invalid.");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a row for upload the private key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Button "Download" in domain configuration action - Download the selected private key file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btDownload_Click(object sender, EventArgs e)
        {
            if (dgvDomainConfiguration.SelectedCells.Count > 0)
            {
                string fileName = Convert.ToString(dgvDomainConfiguration.SelectedCells[2].Value);

                if (fileName == string.Empty)
                    return;

                FileInfo fileInfo = new FileInfo(fileName);
                string fileExtension = fileInfo.Extension;

                byte[] byteData = null;

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Files (*" + fileExtension + ")|*" + fileExtension;
                    saveFileDialog.Title = "Save File as";
                    saveFileDialog.CheckPathExists = true;
                    saveFileDialog.FileName = fileName;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        byteData = attachments[dgvDomainConfiguration.SelectedCells[2].RowIndex];
                        File.WriteAllBytes(saveFileDialog.FileName, byteData);
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a row for download the private key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                //TODO: Show extract progress
                try
                {
                    System.IO.Directory.CreateDirectory(tempDir);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Couldn't create directory:\n" + tempDir + "\n" + e.Message, "Directory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btUpateInstall.Enabled = true;
                    return;
                }
                using (ZipFile zip1 = ZipFile.Read(tempPath))
                {
                    // here, we extract every entry, but we could extract conditionally
                    // based on entry name, size, date, checkbox status, etc.  
                    foreach (ZipEntry e in zip1)
                    {
                        e.Extract(tempDir, ExtractExistingFileAction.OverwriteSilently);
                    }
                }

                string[] contents = Directory.GetDirectories(tempDir);
                if (contents.Length == 0) {
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
                    Process.Start(exePath, args );
                }
                catch (Exception e)
                {
                    MessageBox.Show("Couldn't start updater:\n" + e.Message, "Updater error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btUpateInstall.Enabled = true;
                    return;
                }
                this.Close();
            }

            /*
             * 
            //testing:
            MessageBox.Show("Uninstall retval: " + ExchangeHelper.uninstallTransportAgent().ToString());
            MessageBox.Show("Install retval: " + ExchangeHelper.installTransportAgent().ToString());*/


        }

        private void cbxPrereleases_CheckedChanged(object sender, EventArgs e)
        {
            updateVersions();
        }
    }
}