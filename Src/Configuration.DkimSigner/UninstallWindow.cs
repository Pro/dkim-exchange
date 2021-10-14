using Configuration.DkimSigner.Configuration;
using Configuration.DkimSigner.Exchange;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Configuration.DkimSigner
{
	public partial class UninstallWindow : Form
	{
		public UninstallWindow()
		{
			InitializeComponent();
		}

		private void UninstallWindow_Shown(object sender, EventArgs e)
		{
			if (
				MessageBox.Show("Uninstall Exchange DKIM Signer?", "Uninstall?", MessageBoxButtons.YesNo,
					MessageBoxIcon.Question) != DialogResult.Yes)
			{
				this.Close();
				Application.Exit();
				return;
			}

			try
			{
				lbStep.Text = "Uninstalling Exchange Agent";
				ExchangeServer.UninstallDkimTransportAgent();

				lbStep.Text = "Restarting MS Exchange Transport";
				TransportService ts = new TransportService();
				try
				{
					ts.Do(TransportServiceAction.Restart, delegate (string msg)
					{
						MessageBox.Show(msg, "Service error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					});
				}
				catch (Exception ex)
				{
					MessageBox.Show("Couldn't restart MSExchangeTransport Service. Please restart it manually. \n" + ex.Message, "Error restarting Service", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					ts.Dispose();

				}


				lbStep.Text = "Deleting directory";

				if (
					MessageBox.Show(this,
						"The directory '" + Constants.DkimSignerPath +
						"' will now be deleted. Please make a backup of the keys stored within this directory before continuing.\n Continue deleting?",
						"Delete directory?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					try
					{
						Directory.Delete(Constants.DkimSignerPath, true);
					}
					catch (Exception)
					{
						//ignore exception. Delete directory on reboot
						if (!NativeMethods.MoveFileEx(Constants.DkimSignerPath, null, MoveFileFlags.DelayUntilReboot))
						{
							MessageBox.Show(
								"Unable to schedule '" + Constants.DkimSignerPath + "' for deletion on next reboot.",
								"Delete error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					}
				}

				lbStep.Text = "Removing registry entry";
				CplControl.Unregister();
				UninstallerRegistry.Unregister();

				/*if (MessageBox.Show(this, "Transport Agent removed from Exchange. Would you like me to remove all the settings for Exchange DKIM Signer?'", "Remove settings?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (File.Exists(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml")))
                        File.Delete(Path.Combine(Constants.DKIM_SIGNER_PATH, "settings.xml"));
                }*/
				/*if (MessageBox.Show(this, "Transport Agent removed from Exchange. Would you like me to remove the folder '" + Constants.DKIM_SIGNER_PATH + "' and all its content?\nWARNING: All your settings and keys will be deleted too!", "Remove files?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    var dir = new DirectoryInfo(Constants.DKIM_SIGNER_PATH);
                    dir.Delete(true);
                }*/
				lbStep.Text = "Uninstall complete";
				progressUninstall.Style = ProgressBarStyle.Continuous;
				progressUninstall.Value = 100;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.Message, "Uninstall error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			btClose.Enabled = true;
		}

		private void btClose_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			this.Close();
			Application.Exit();
		}
	}


	/// <summary>
	/// See http://stackoverflow.com/a/6077952/869402
	/// </summary>
	[Flags]
	internal enum MoveFileFlags
	{
		None = 0,
		ReplaceExisting = 1,
		CopyAllowed = 2,
		DelayUntilReboot = 4,
		WriteThrough = 8,
		CreateHardlink = 16,
		FailIfNotTrackable = 32,
	}

	/// <summary>
	/// See http://stackoverflow.com/a/6077952/869402
	/// </summary>
	internal static class NativeMethods
	{
		[DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		public static extern bool MoveFileEx(
			string lpExistingFileName,
			string lpNewFileName,
			MoveFileFlags dwFlags);
	}
}