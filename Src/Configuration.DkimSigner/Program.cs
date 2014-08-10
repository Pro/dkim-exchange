using System;
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

            if (Array.IndexOf(asArgv, "--install") >= 0)
            {
                oForm = new InstallWindow();
            }
            //else if(Array.IndexOf(asArgv, "--upgrade") >= 0)
            //{
            //}
            //else if (Array.IndexOf(asArgv, "--uninstall") >= 0)
            //{
            //    // Delete Itself
            //    ProcessStartInfo Info=new ProcessStartInfo();
            //    Info.Arguments="/C choice /C Y /N /D Y /T 5 & Del "+ Application.ExecutablePath;
            //    Info.WindowStyle=ProcessWindowStyle.Hidden;
            //    Info.CreateNoWindow=true;
            //    Info.FileName="cmd.exe";
            //    Process.Start(Info); 
            //}
            else
            {
                oForm = new MainWindow();    
            }

            Application.Run(oForm);
        }
    }
}