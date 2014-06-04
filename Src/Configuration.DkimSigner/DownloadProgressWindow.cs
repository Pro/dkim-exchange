using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
    public partial class DownloadProgressWindow : Form
    {
        private string url;
        private string targetLocation;
        private WebClient webClient;


        public DownloadProgressWindow(string url, string targetLocation)
        {
            InitializeComponent();
            this.url = url;
            this.targetLocation = targetLocation;
            webClient = new WebClient();
        }

        private void DownloadProgressWindow_Load(object sender, EventArgs e)
        {
            lblFile.Text = "Downloading " + url;
            webClient.Headers.Add("User-Agent", ".NET Framework API Client");
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri(url), targetLocation);
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (webClient.IsBusy)
            {
                webClient.CancelAsync();
            }
            else
            {

                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                progressBar.Value = 0;
                lblFile.Text = "Cancelled";
                this.Close();
                return;
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error downloading file: " + url + "\n" + e.Error.Message);
                progressBar.Value = 0;
                lblFile.Text = "Error: " + e.Error.Message;
                return;
            }
            progressBar.Value = 100;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
