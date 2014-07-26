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
            string[] args = Environment.GetCommandLineArgs();
            string installPath = "";

            // *******************************************************************
            // IF upgrade process
            // *******************************************************************
            bool isUpgrade = (Array.IndexOf(args, "--upgrade") >= 0);
            if (isUpgrade)
            {
                int idx = Array.IndexOf(args, "--upgrade") + 1;

                if (args.Length <= idx)
                {
                    MessageBox.Show("Missing install path for update parameter.", "Invalid argument count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
                else
                {
                    installPath = args[idx];
                }
            }

            // *******************************************************************
            // IF install process
            // *******************************************************************
            bool isInstall = (Array.IndexOf(args, "--install") >= 0);
            if (isInstall)
            {
                installPath = Constants.DKIM_SIGNER_PATH;
            }

            // *******************************************************************
            // Load correct Windows form
            // *******************************************************************
            Form oForm = null;
            ExchangeServer oExchange = new ExchangeServer();

            if (isInstall || isUpgrade)
            {               
                oForm = new UpgradeWindow(Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\..")), installPath);

                string sPathExec = Path.Combine(installPath, Constants.DKIM_SIGNER_CONFIGURATION_EXE);
                if (File.Exists(sPathExec))
                {
                    Process.Start(sPathExec);
                }
                else
                {
                    MessageBox.Show("Couldn't find 'Configuration.DkimSigner.exe' in \n" + installPath, "Exec error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                oForm = new MainWindow(oExchange);
            }

            Application.Run(oForm);
        }
    }
}