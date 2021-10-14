using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Configuration.DkimSigner.FileIO
{
	public class FileOperation
	{
		/// <summary>
		/// Create a '\0'-delimited list of file names for a file operation.
		/// The list is terminated with a final '\0'
		/// </summary>
		/// <param name="names"></param>
		/// <returns></returns>
		private static string MakeFileNameList(string[] names)
		{
			StringBuilder sb = new StringBuilder();
			foreach (string s in names)
			{
				sb.Append(s);
				sb.Append('\0');
			}
			sb.Append('\0');
			return sb.ToString();
		}

		/// <summary>
		/// Copy a list of files.
		/// </summary>
		/// <param name="hwnd">The parent window for progress display etc.</param>
		/// <param name="fromNames">An array of source file names</param>
		/// <param name="toNames">An array of destination file names</param>
		/// <param name="overwrite">Overwrite already existing files</param>
		/// <param name="title">The title to display</param>
		/// <param name="anyOperationsAborted">Indicates if any of the operations were aborted</param>
		/// <returns>true on success, false on error</returns>
		public static bool CopyFiles(IntPtr hwnd, string[] fromNames, string[] toNames, bool overwrite, string title, out bool anyOperationsAborted)
		{
			if (fromNames == null)
				throw new ArgumentNullException("fromNames");
			if (toNames == null)
				throw new ArgumentNullException("toNames");
			if (fromNames.Length != toNames.Length)
				throw new ApplicationException("Source and destination file name arrays must have same length");

			ShellFileOperation sfo = new ShellFileOperation();
			sfo.wFunc = ShellFileOperation.FO_Func.FO_COPY;
			sfo.hwnd = hwnd;
			sfo.lpszProgressTitle = title;
			sfo.fFlags.FOF_MULTIDESTFILES = true;
			sfo.fFlags.FOF_NOCONFIRMATION = overwrite;
			sfo.pFrom = MakeFileNameList(fromNames);
			sfo.pTo = MakeFileNameList(toNames);

			bool returnValue = sfo.Execute();

			anyOperationsAborted = sfo.fAnyOperationsAborted;

			return returnValue;
		}

		/// <summary>
		/// Delete a list of files.
		/// </summary>
		/// <param name="hwnd">The parent window for progress display etc.</param>
		/// <param name="names">An array of source file names</param>
		/// <param name="title">The title to display</param>
		/// <param name="anyOperationsAborted">Indicates if any of the operations were aborted</param>
		/// <returns>true on success, false on error</returns>
		public static bool DeleteFiles(IntPtr hwnd, string[] names, string title, out bool anyOperationsAborted)
		{
			if (names == null)
				throw new ArgumentNullException("names");

			ShellFileOperation sfo = new ShellFileOperation();
			sfo.wFunc = ShellFileOperation.FO_Func.FO_DELETE;
			sfo.hwnd = hwnd;
			sfo.lpszProgressTitle = title;
			sfo.pFrom = MakeFileNameList(names);
			sfo.pTo = "";

			bool returnValue = sfo.Execute();

			anyOperationsAborted = sfo.fAnyOperationsAborted;

			return returnValue;
		}

		/// <summary>
		/// Removes the Zone identifier from a file (protects files downloaded through IE).
		/// Only supported on NTFS file system. If file is located on another FS type, this method will most probably return false.
		/// See: http://stackoverflow.com/questions/6374673/unblock-file-from-within-net-4-c-sharp
		/// </summary>
		/// <param name="sFileName">The file to unlock</param>
		/// <returns>True if successfully unlocked.</returns>
		public bool Unblock(string sFileName)
		{
			return DeleteFile(sFileName + ":Zone.Identifier");
		}

		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteFile(string name);
	}
}