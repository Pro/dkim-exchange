using System;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
    public partial class DownloadProgressWindow : Form
    {
        // ##########################################################
        // ##################### Variables ##########################
        // ##########################################################

        private WebClient oWebClient;
        private string sUrl;
        private string sTargetLocation;

        // ##########################################################
        // ##################### Construtor #########################
        // ##########################################################

        public DownloadProgressWindow(string sUrl, string sTargetLocation)
        {
            this.InitializeComponent();

            this.oWebClient = new WebClient();
            this.sUrl = sUrl;
            this.sTargetLocation = sTargetLocation;
        }

        // ##########################################################
        // ####################### Events ###########################
        // ##########################################################

        private void DownloadProgressWindow_Load(object sender, EventArgs e)
        {
            this.lbFile.Text = "Downloading " + this.sUrl;
            this.oWebClient.Headers.Add("User-Agent", ".NET Framework API Client");
            this.oWebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.Completed);
            this.oWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.ProgressChanged);
            this.oWebClient.DownloadFileAsync(new Uri(this.sUrl), this.sTargetLocation);
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.pgFile.Value = e.ProgressPercentage;
            this.pgFile.Update();
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.DialogResult = DialogResult.Cancel;
                
                this.pgFile.Value = 0;
                this.lbFile.Text = "Cancelled";

                this.DialogResult = DialogResult.Cancel;
            }
            else if (e.Error != null)
            {
                MessageBox.Show(this, "Error downloading file: " + sUrl + "\n" + e.Error.Message);
                
                this.pgFile.Value = 0;
                this.lbFile.Text = "Error: " + e.Error.Message;

                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.pgFile.Value = 100;

                this.DialogResult = DialogResult.OK;
            }

            this.Close();
        }

        // ##########################################################
        // ################# Internal functions #####################
        // ##########################################################

        // ###########################################################
        // ###################### Button click #######################
        // ###########################################################

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (this.oWebClient.IsBusy)
            {
                this.oWebClient.CancelAsync();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
