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

namespace Configuration.DkimSigner
{
    public partial class MainWindows : Form
    {
        private const string DKIM_SIGNER_PATH = @"C:\Program Files\Exchange DkimSigner\";
        private const string DKIM_SIGNER_DLL = @"ExchangeDkimSigner.dll";
        private const string DKIM_SIGNER_CONFIG = @"ExchangeDkimSigner.dll.config";
        private const string DKIM_SIGNER_URI = @"https://raw.githubusercontent.com/Pro/dkim-exchange/master/VERSION";

        private static Assembly configurationDefiningAssembly;
        private Dictionary<int, byte[]> _myAttachments;

        public MainWindows()
        {
            InitializeComponent();

            cbLogLevel.SelectedItem = "Information";
            _myAttachments = new Dictionary<int, byte[]>();

            getDkimSignerConfig();

            txtExchangeInstalled.Text = ExchangeHelper.checkExchangeVersionInstalled();
            checkDkimSignerInstalled();
            checkDkimSignerAvailable();
        }

        private static Assembly ConfigResolveEventHandler(object sender, ResolveEventArgs args)
        {
            return configurationDefiningAssembly;
        }

        public static TConfig GetCustomConfig<TConfig>(string assembly, string sectionName) where TConfig : ConfigurationSection
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ConfigResolveEventHandler);
            configurationDefiningAssembly = Assembly.LoadFrom(assembly);
            var customConfig = ConfigurationManager.OpenExeConfiguration(assembly);
            var returnConfig = customConfig.GetSection(sectionName) as TConfig;
            AppDomain.CurrentDomain.AssemblyResolve -= ConfigResolveEventHandler;
            return returnConfig;
        }

        private void getDkimSignerConfig()
        {
            General appSetting = GetCustomConfig<General>(DKIM_SIGNER_PATH + DKIM_SIGNER_DLL, "customSection/general");
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

        private void btUpload_Click(object sender, EventArgs e)
        {
            if (dgvDomainConfiguration.SelectedCells[0].Value != null)
            {
                UploadAttachment(dgvDomainConfiguration.SelectedCells[0]);
            }
            else
            {
                MessageBox.Show("Select a row for upload the private key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btDownload_Click(object sender, EventArgs e)
        {
            if (dgvDomainConfiguration.SelectedCells[0].Value != null)
            {
                DownloadAttachment(dgvDomainConfiguration.SelectedCells[2]);
            }
            else
            {
                MessageBox.Show("Select a row for download the private key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Upload Attachment at the provided DataGridViewCell
        /// </summary>
        /// <param name="dgvCell"></param>
        private void UploadAttachment(DataGridViewCell dgvCell)
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
                    dgvDomainConfiguration.Rows[dgvCell.RowIndex].Cells[2].Value = fileInfo.Name;

                    if (_myAttachments.ContainsKey(dgvCell.RowIndex))
                    {
                        _myAttachments[dgvCell.RowIndex] = binaryData;
                    }
                    else
                    {
                        _myAttachments.Add(dgvCell.RowIndex, binaryData);
                    }
                }
            }
        }

        /// <summary>
        /// Download Attachment from the provided DataGridViewCell
        /// </summary>
        /// <param name="dgvCell"></param>
        private void DownloadAttachment(DataGridViewCell dgvCell)
        {
            string fileName = Convert.ToString(dgvCell.Value);

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
                    byteData = _myAttachments[dgvCell.RowIndex];
                    File.WriteAllBytes(saveFileDialog.FileName, byteData);
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {

        }
    }
}