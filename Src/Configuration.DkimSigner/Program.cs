using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

using Configuration.DkimSigner.Exchange;

namespace Configuration.DkimSigner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // *******************************************************************
            // Set some default application configuration
            // *******************************************************************
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // *******************************************************************
            // Variables
            // *******************************************************************
            string[] asArgv = Environment.GetCommandLineArgs();

            // *******************************************************************
            // Load correct Windows form
            // *******************************************************************
            ExchangeServer oExchange = new ExchangeServer();
            Form oForm = null;

            if (Array.IndexOf(asArgv, "--install") >= 0 || Array.IndexOf(asArgv, "--upgrade") >= 0)
            {
                // Start Install/Upgrade process
                oForm = new InstallWindow(oExchange);
                Application.Run(oForm);

                // Start new installed DKIM Signer Configuration GUI
                string sPathExec = Path.Combine(Constants.DKIM_SIGNER_PATH, Constants.DKIM_SIGNER_CONFIGURATION_EXE);
                if (File.Exists(sPathExec))
                {
                    Process.Start(sPathExec);
                }
                else
                {
                    MessageBox.Show("Couldn't find 'Configuration.DkimSigner.exe' in \n" + Constants.DKIM_SIGNER_PATH, "Exec error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                oForm = new MainWindow(oExchange);
                Application.Run(oForm);
            }
        }
    }
}