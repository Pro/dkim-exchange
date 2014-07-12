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
        /**********************************************************/
        /*********************** Constants ************************/
        /**********************************************************/


        /**********************************************************/
        /*********************** Variables ************************/
        /**********************************************************/

        private WebClient webClient;
        private string url;
        private string targetLocation;

        /**********************************************************/
        /*********************** Construtor ***********************/
        /**********************************************************/

        public DownloadProgressWindow(string url, string targetLocation)
        {
            InitializeComponent();

            this.webClient = new WebClient();
            this.url = url;
            this.targetLocation = targetLocation;
        }

        /**********************************************************/
        /************************* Events *************************/
        /**********************************************************/

        private void DownloadProgressWindow_Load(object sender, EventArgs e)
        {
            this.lbFile.Text = "Downloading " + this.url;
            this.webClient.Headers.Add("User-Agent", ".NET Framework API Client");
            this.webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(this.Completed);
            this.webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.ProgressChanged);
            this.webClient.DownloadFileAsync(new Uri(this.url), this.targetLocation);
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.pgFile.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.DialogResult = DialogResult.Cancel;
                
                this.pgFile.Value = 0;
                this.lbFile.Text = "Cancelled";

                this.Close();
                return;
            }
            else if (e.Error != null)
            {
                MessageBox.Show("Error downloading file: " + url + "\n" + e.Error.Message);
                
                this.pgFile.Value = 0;
                this.lbFile.Text = "Error: " + e.Error.Message;
                
                return;
            }

            this.pgFile.Value = 100;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /**********************************************************/
        /******************* Internal functions *******************/
        /**********************************************************/


        /**********************************************************/
        /********************** Button click **********************/
        /**********************************************************/

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.webClient.IsBusy)
            {
                this.webClient.CancelAsync();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
