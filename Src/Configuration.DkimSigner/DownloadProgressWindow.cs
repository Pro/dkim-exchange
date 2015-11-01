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
            InitializeComponent();

            oWebClient = new WebClient();
            this.sUrl = sUrl;
            this.sTargetLocation = sTargetLocation;
        }

        // ##########################################################
        // ####################### Events ###########################
        // ##########################################################

        private void DownloadProgressWindow_Load(object sender, EventArgs e)
        {
            lbFile.Text = "Downloading " + sUrl;
            oWebClient.Headers.Add("User-Agent", ".NET Framework API Client");
            oWebClient.DownloadFileCompleted += Completed;
            oWebClient.DownloadProgressChanged += ProgressChanged;
            oWebClient.DownloadFileAsync(new Uri(sUrl), sTargetLocation);
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            pgFile.Value = e.ProgressPercentage;
            pgFile.Update();
            Refresh();
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                DialogResult = DialogResult.Cancel;
                
                pgFile.Value = 0;
                lbFile.Text = "Cancelled";

                DialogResult = DialogResult.Cancel;
            }
            else if (e.Error != null)
            {
                MessageBox.Show(this, "Error downloading file: " + sUrl + "\n" + e.Error.Message);
                
                pgFile.Value = 0;
                lbFile.Text = "Error: " + e.Error.Message;

                DialogResult = DialogResult.Cancel;
            }
            else
            {
                pgFile.Value = 100;

                DialogResult = DialogResult.OK;
            }

            Close();
        }

        // ##########################################################
        // ################# Internal functions #####################
        // ##########################################################

        // ###########################################################
        // ###################### Button click #######################
        // ###########################################################

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (oWebClient.IsBusy)
            {
                oWebClient.CancelAsync();
            }
            else
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}
