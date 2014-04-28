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

namespace Configuration.DkimSigner
{
    public partial class MainWindow : Form
    {
        private const string DKIM_SIGNER_PATH = @"C:\Program Files\Exchange DkimSigner\";
        private const string DKIM_SIGNER_DLL = @"ExchangeDkimSigner.dll";

        private Dictionary<int, byte[]> attachments;
        private Release currentRelease;

        delegate void SetDkimSignerInstalledCallback(string dkimSignerInstalled);
        delegate void SetDkimSignerAvailableCallback(string dkimSignerAvailable);
        delegate void SetChangelogCallback(string changelog);

        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public MainWindow()
        {
            InitializeComponent();

            attachments = new Dictionary<int, byte[]>();
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
            this.cbLogLevel.SelectedItem = "Information";

            // Get Exchange.DkimSigner version installed
            Thread thDkimSignerInstalled = new Thread(new ThreadStart(this.CheckDkimSignerInstalledSafe));
            thDkimSignerInstalled.Start();

            // Get Exchange.DkimSigner version available
            Thread thDkimSignerAvailable = new Thread(new ThreadStart(this.CheckDkimSignerAvailableSafe));
            thDkimSignerAvailable.Start();

            // Get Exchange version installed + load the current configuration
            txtExchangeInstalled.Text = ExchangeHelper.checkExchangeVersionInstalled();
            LoadDkimSignerConfig();
        }

        /// <summary>
        /// Confirm the configuration saving before quit the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Want to save your changes?", "Confirmation", MessageBoxButtons.YesNoCancel);
            
            if (result == DialogResult.Yes)
            {
                this.SaveDkimSignerConfig();
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
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
        /// Open RuleWindows when the row header have been clicked in the dgvDomainConfiguration DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgvDomainConfiguration_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string recipientRule = this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[3].Value != null ? this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[3].Value.ToString() : string.Empty;
            string senderRule = this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[4].Value != null ? this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[4].Value.ToString() : string.Empty;

            RuleWindow form = new RuleWindow(recipientRule, senderRule);
            form.ShowDialog();

            this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[3].Value = form.txtRecipientRule.Text;
            this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[4].Value = form.txtSenderRule.Text;
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

                dgvDomainConfiguration.Rows[dgvDomainConfiguration.SelectedCells[0].RowIndex].Cells[2].Value = form.txtFilename.Text;
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

        /**********************************************************/
        /******************* Internal functions *******************/
        /**********************************************************/

        /// <summary>
        /// Set the value of txtDkimSignerInstalled from CheckDkimSignerInstalledSafe (use by thread DkimSignerInstalled)
        /// </summary>
        /// <param name="dkimSignerInstalled"></param>
        private void SetDkimSignerInstalled(string dkimSignerInstalled)
        {
            this.txtDkimSignerInstalled.Text = dkimSignerInstalled;
        }

        /// <summary>
        ///  Set the value of txtDkimSignerAvailable from CheckDkimSignerAvailableSafe (use by thread DkimSignerAvailable)
        /// </summary>
        /// <param name="dkimSignerAvailable"></param>
        private void SetDkimSignerAvailable(string dkimSignerAvailable)
        {
            this.txtDkimSignerAvailable.Text = dkimSignerAvailable;
        }

        /// <summary>
        ///  Set the value of txtChangelog from CheckDkimSignerAvailableSafe (use by thread DkimSignerAvailable)
        /// </summary>
        /// <param name="changelog"></param>
        private void SetChangelog(string changelog)
        {
            this.txtChangelog.Text = changelog;
        }

        /// <summary>
        /// Thread safe function for the thread DkimSignerInstalled
        /// </summary>
        private void CheckDkimSignerInstalledSafe()
        {
            string dkimSignerInstalled = string.Empty;

            try
            {
                dkimSignerInstalled = FileVersionInfo.GetVersionInfo(DKIM_SIGNER_PATH + DKIM_SIGNER_DLL).ProductVersion;
            }
            catch (Exception)
            {
               dkimSignerInstalled = "Not installed";
            }
            
            if (this.txtDkimSignerInstalled.InvokeRequired)
            {
                SetDkimSignerInstalledCallback d = new SetDkimSignerInstalledCallback(SetDkimSignerInstalled);
                this.Invoke(d, new object[] { dkimSignerInstalled});
            }
            else
            {
                this.txtDkimSignerInstalled.Text = dkimSignerInstalled;
            }
        }

        /// <summary>
        /// Thread safe function for the thread DkimSignerAvailable
        /// </summary>
        private void CheckDkimSignerAvailableSafe()
        {
            string dkimSignerAvailable = string.Empty;
            string changelog = string.Empty;

            try
            {
                currentRelease = ApiWrapper.getNewestRelease();
                if (currentRelease != null)
                {
                    dkimSignerAvailable = currentRelease.Version.ToString();
                    changelog = currentRelease.Body;
                }
                else
                {
                    dkimSignerAvailable = "Unknown";
                }
            }
            catch (Exception)
            {
                dkimSignerAvailable = "Unknown";
                changelog = "Couldn't get current version.\r\nCheck your Internet connexion and restart the application.";
            }

            if (this.txtDkimSignerInstalled.InvokeRequired)
            {
                SetDkimSignerAvailableCallback d = new SetDkimSignerAvailableCallback(SetDkimSignerAvailable);
                this.Invoke(d, new object[] { dkimSignerAvailable});
            }
            else
            {
                this.txtDkimSignerInstalled.Text = dkimSignerAvailable;
            }

            if (this.txtChangelog.InvokeRequired)
            {
                SetChangelogCallback d = new SetChangelogCallback(SetChangelog);
                this.Invoke(d, new object[] { changelog });
            }
            else
            {
                this.txtChangelog.Text = changelog;
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
                        string recipientRule = RegistryHelper.Read("RecipientRule", @"Software\Exchange DkimSigner\Domain\" + domainName);
                        string senderRule = RegistryHelper.Read("SenderRule", @"Software\Exchange DkimSigner\Domain\" + domainName);

                        this.dgvDomainConfiguration.Rows.Add(   domainName,
                                                                selector,
                                                                privateKeyFile,
                                                                recipientRule != null ? recipientRule : string.Empty,
                                                                senderRule != null ? senderRule : string.Empty);

                        attachments[i++] = File.ReadAllBytes(DKIM_SIGNER_PATH + @"\keys\" + privateKeyFile);
                    }

                    this.dgvDomainConfiguration.Rows[0].Selected = true;
                }
            }
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
                        string recipientRule = row.Cells[3].Value != null ? row.Cells[3].Value.ToString() : string.Empty;
                        string senderRule = row.Cells[4].Value != null ? row.Cells[4].Value.ToString() : string.Empty;

                        status = status && RegistryHelper.Write("Selector", selector, @"Software\Exchange DkimSigner\Domain\" + domainName);
                        status = status && RegistryHelper.Write("PrivateKeyFile", privateKeyFile, @"Software\Exchange DkimSigner\Domain\" + domainName);

                        if (recipientRule != string.Empty)
                            status = status && RegistryHelper.Write("RecipientRule", recipientRule, @"Software\Exchange DkimSigner\Domain\" + domainName);

                        if (senderRule != string.Empty)
                            status = status && RegistryHelper.Write("SenderRule", senderRule, @"Software\Exchange DkimSigner\Domain\" + domainName);

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
            //testing:
            MessageBox.Show("Uninstall retval: " + ExchangeHelper.uninstallTransportAgent().ToString());
            MessageBox.Show("Install retval: " + ExchangeHelper.installTransportAgent().ToString());
        }
    }
}