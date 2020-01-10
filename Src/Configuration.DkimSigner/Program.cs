using Configuration.DkimSigner.Exchange;
using System;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

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

            /*while (!System.Diagnostics.Debugger.IsAttached)
                System.Threading.Thread.Sleep(100);*/

            // *******************************************************************
            // Load correct Windows form
            // *******************************************************************
            Form oForm = null;
            string[] asArgv = Environment.GetCommandLineArgs();

            int parIdx = Math.Max(Math.Max(Math.Max(Array.IndexOf(asArgv, "--install"),
                Array.IndexOf(asArgv, "--upgrade-inplace")),
                Array.IndexOf(asArgv, "--configure")),
                Array.IndexOf(asArgv, "--uninstall"));

            if (parIdx >= 0)
            {
                if (asArgv[parIdx] == "--configure")
                {
                    oForm = new ConfigureWindow();
                }
                else if (asArgv[parIdx] == "--upgrade-inplace")
                {
                    oForm = new InstallWindow(true);
                }
                else if (asArgv[parIdx] == "--install")
                {
                    try
                    {
                        if (!Exchange.ExchangeServer.IsDkimAgentTransportInstalled())
                        {
                            oForm = new InstallWindow();
                        }
                    }
                    catch
                    {
                        int debugIdx = Array.IndexOf(asArgv, "--debug");
                        MessageBox.Show(
                            "The check to see whether DKIM Signer is installed crashed!" + Environment.NewLine + Environment.NewLine +
                            "This probably means you are running the application on a machine" + Environment.NewLine +
                            "which does not have Exchange installed. The program will still" + Environment.NewLine +
                            "open but it won't be of much use.", Application.ProductName, MessageBoxButtons.OK);
                        oForm = new MainWindow(debugIdx >= 0);
                    }
                }
                else if (asArgv[parIdx] == "--uninstall")
                {
                    oForm = new UninstallWindow();
                }
            }
            else
            {
                int debugIdx = Array.IndexOf(asArgv, "--debug");
                    // Quick check to see if binary and DLL exist - ensures UI opens quickly if program is installed
                    if(File.Exists("" + Constants.DkimSignerPath + "\\" + Constants.DkimSignerAgentDll) && File.Exists("" + Constants.DkimSignerPath + "\\" + Constants.DkimSignerConfigurationExe))
                    { 
                    oForm = new MainWindow(debugIdx >= 0); 
                } else {
                    try
                    {
                        // If the quick check fails, perform a thorough check before launching the installer. This also
                        // ensures the installer doesn't crash when Exhange isn't installed (useless but user friendly).
                        if(!Exchange.ExchangeServer.IsDkimAgentTransportInstalled())
                        {
                            oForm = new InstallWindow();
                        }
                    }
                    catch
                    {
                        MessageBox.Show(
                            "The check to see whether DKIM Signer is installed crashed!" + Environment.NewLine + Environment.NewLine +
                            "This probably means you are running the application on a machine" + Environment.NewLine +
                            "which does not have Exchange installed. The program will still" + Environment.NewLine +
                            "open but it won't be of much use.", Application.ProductName, MessageBoxButtons.OK);
                        oForm = new MainWindow(debugIdx >= 0);
                    }
                }
            }

            if (oForm != null)
            {
                Application.Run(oForm);
            }
        }
    }
}