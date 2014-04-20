using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ConfigurationSettings;
using DkimSigner.RSA;
using DkimSigner.Properties;

namespace Configuration.DkimSigner
{
    public partial class MainWindows : Form
    {
        private const string DKIM_SIGNER_PATH = @"C:\Program Files\Exchange DkimSigner\";
        private const string DKIM_SIGNER_DLL = @"ExchangeDkimSigner.dll";
        private const string DKIM_SIGNER_URI = @"https://raw.githubusercontent.com/Pro/dkim-exchange/master/VERSION";

        private Dictionary<int, byte[]> attachments;

        public MainWindows()
        {
            InitializeComponent();

            attachments = new Dictionary<int, byte[]>();
            cbLogLevel.SelectedItem = "Information";
            txtExchangeInstalled.Text = ExchangeHelper.checkExchangeVersionInstalled();

            checkDkimSignerInstalled();
            checkDkimSignerAvailable();
            loadDkimSignerConfig();
        }

        private void loadDkimSignerConfig()
        {
            if (RegistryHelper.Open(@"Exchange DkimSigner") != null)
            {
                // Load the log level.
                int logLevel = 0;
                try
                {
                    string temp = RegistryHelper.Read("LogLevel", @"Exchange DkimSigner");

                    if (temp != null)
                        logLevel = Convert.ToInt32(RegistryHelper.Read("LogLevel", @"Exchange DkimSigner"));
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
                    //MessageBox.Show("Impossible to load ");
                }

                // Load the signing algorithm.
                try
                {
                    DkimAlgorithmKind signingAlgorithm = (DkimAlgorithmKind)Enum.Parse(typeof(DkimAlgorithmKind), RegistryHelper.Read("Algorithm", @"Exchange DkimSigner\DKIM"), true);

                    if (signingAlgorithm == DkimAlgorithmKind.RsaSha1)
                        this.rbRsaSha1.Checked = true;
                    else
                        this.rbRsaSha256.Checked = true;
                }
                catch (Exception ex)
                {
                    throw new ConfigurationErrorsException(Resources.MainWindows_BadDkimAlgorithmConfig, ex);
                }

                // Load the header canonicalization algorithm.
                try
                {
                    DkimCanonicalizationKind headerCanonicalization = (DkimCanonicalizationKind)Enum.Parse(typeof(DkimCanonicalizationKind), RegistryHelper.Read("HeaderCanonicalization", @"Exchange DkimSigner\DKIM"), true);

                    if (headerCanonicalization == DkimCanonicalizationKind.Simple)
                        this.rbSimpleHeaderCanonicalization.Checked = true;
                    else
                        this.rbRelaxedHeaderCanonicalization.Checked = true;
                }
                catch (Exception ex)
                {
                    throw new ConfigurationErrorsException(Resources.MainWindows_BadDkimCanonicalizationHeaderConfig, ex);
                }

                // Load the body canonicalization algorithm.
                try
                {
                    DkimCanonicalizationKind bodyCanonicalization = (DkimCanonicalizationKind)Enum.Parse(typeof(DkimCanonicalizationKind), RegistryHelper.Read("BodyCanonicalization", @"Exchange DkimSigner\DKIM"), true);

                    if (bodyCanonicalization == DkimCanonicalizationKind.Simple)
                        this.rbSimpleBodyCanonicalization.Checked = true;
                    else
                        this.rbRelaxedBodyCanonicalization.Checked = true;
                }
                catch (Exception ex)
                {
                    throw new ConfigurationErrorsException(Resources.MainWindows_BadDkimCanonicalizationBodyConfig, ex);
                }

                // Load the list of headers to sign in each message.
                string unparsedHeaders = RegistryHelper.Read("HeadersToSign", @"Exchange DkimSigner\DKIM");
                if (unparsedHeaders != null)
                {
                    this.txtHeaderToSign.Text = unparsedHeaders;
                }

                // Load the list of domains
                string[] domainNames = RegistryHelper.GetSubKeyName(@"Exchange DkimSigner\Domain");
                foreach (string domainName in domainNames)
                {
                    string selector = RegistryHelper.Read("Selector", @"Exchange DkimSigner\Domain\" + domainName);
                    string privateKeyFile = RegistryHelper.Read("PrivateKeyFile", @"Exchange DkimSigner\Domain\" + domainName);
                    string recipientRule = RegistryHelper.Read("RecipientRule", @"Exchange DkimSigner\Domain\" + domainName);
                    string senderRule = RegistryHelper.Read("SenderRule", @"Exchange DkimSigner\Domain\" + domainName);

                    this.dgvDomainConfiguration.Rows.Add(   domainName,
                                                            selector,
                                                            privateKeyFile,
                                                            recipientRule != null ? recipientRule: string.Empty,
                                                            senderRule != null ? senderRule : string.Empty);
                }
            }
        }

        private void checkDkimSignerInstalled()
        {
            try
            {
                txtDkimSignerInstalled.Text = FileVersionInfo.GetVersionInfo(DKIM_SIGNER_PATH + DKIM_SIGNER_DLL).ProductVersion;
            }
            catch (Exception)
            {
                txtDkimSignerInstalled.Text = "Not installed";
            }
        }

        private void checkDkimSignerAvailable()
        {
            try
            {
                WebClient client = new WebClient();
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(downloadFileCompleted);
                client.DownloadFileAsync(new Uri(DKIM_SIGNER_URI), Path.GetTempPath() + "VERSION_DkimSigner");
            }
            catch (Exception)
            {
                txtDkimSignerAvailable.Text = "Unknown";
            }
        }

        private void downloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                using (StreamReader sr = new StreamReader(Path.GetTempPath() + "VERSION_DkimSigner"))
                {
                    txtDkimSignerAvailable.Text = sr.ReadToEnd();
                }
            }
            catch (Exception)
            {
                txtDkimSignerAvailable.Text = "Unknown";
            }
        }

        void dgvDomainConfiguration_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string recipientRule = this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[3].Value != null ? this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[3].Value.ToString() : string.Empty;
            string senderRule = this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[4].Value != null ? this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[4].Value.ToString() : string.Empty;

            RuleWindows form = new RuleWindows(recipientRule, senderRule);
            form.ShowDialog();

            this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[3].Value = form.txtRecipientRule.Text;
            this.dgvDomainConfiguration.Rows[e.RowIndex].Cells[4].Value = form.txtSenderRule.Text;
        }

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
                }
            }
            else
            {
                MessageBox.Show("Select a row for upload the private key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        private void btSave_Click(object sender, EventArgs e)
        {
            bool status = true;

            status = status && RegistryHelper.Write("LogLevel", this.cbLogLevel.SelectedIndex + 1, @"Exchange DkimSigner");
            if(!status)
                MessageBox.Show("Error! Impossible to change the log level.");

            status = status && RegistryHelper.Write("Algorithm", this.rbRsaSha1.Checked ? this.rbRsaSha1.Text : this.rbRsaSha256.Text, @"Exchange DkimSigner\DKIM");
            if (!status)
                MessageBox.Show("Error! Impossible to change the algorithm.");

            status = status && RegistryHelper.Write("HeaderCanonicalization", this.rbSimpleHeaderCanonicalization.Checked ? this.rbSimpleHeaderCanonicalization.Text : this.rbRelaxedHeaderCanonicalization.Text, @"Exchange DkimSigner\DKIM");
            if (!status)
                MessageBox.Show("Error! Impossible to change the header canonicalization.");
            
            status = status && RegistryHelper.Write("BodyCanonicalization", this.rbSimpleBodyCanonicalization.Checked ? this.rbSimpleBodyCanonicalization.Text : this.rbRelaxedBodyCanonicalization.Text, @"Exchange DkimSigner\DKIM");
            if (!status)
                MessageBox.Show("Error! Impossible to change the body canonicalization.");

            status = status && RegistryHelper.Write("HeadersToSign", this.txtHeaderToSign.Text, @"Exchange DkimSigner\DKIM");
            if (!status)
                MessageBox.Show("Error! Impossible to change the headers to sign.");

            dgvDomainConfiguration.AllowUserToAddRows = false;
            if (dgvDomainConfiguration.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in this.dgvDomainConfiguration.Rows)
                {
                    string domainName = row.Cells[0].Value.ToString();
                    string selector = row.Cells[1].Value.ToString();
                    string privateKeyFile = row.Cells[2].Value.ToString();
                    string recipientRule = row.Cells[3].Value != null ? row.Cells[3].Value.ToString() : string.Empty;
                    string senderRule = row.Cells[4].Value != null ? row.Cells[4].Value.ToString() : string.Empty;

                    status = status && RegistryHelper.Write("Selector", selector, @"Exchange DkimSigner\Domain\" + domainName);
                    status = status && RegistryHelper.Write("PrivateKeyFile", "keys/" + privateKeyFile, @"Exchange DkimSigner\Domain\" + domainName);

                    if (recipientRule != string.Empty)
                        status = status && RegistryHelper.Write("RecipientRule", recipientRule, @"Exchange DkimSigner\Domain\" + domainName);

                    if (senderRule != string.Empty)
                        status = status && RegistryHelper.Write("SenderRule", senderRule, @"Exchange DkimSigner\Domain\" + domainName);
                }
            }
            dgvDomainConfiguration.AllowUserToAddRows = true;

            if (status)
                MessageBox.Show("The configuration has been updated.");
            else
                MessageBox.Show("One or many errors happened! All specified configurations haven't been updated.");
        }
    }
}