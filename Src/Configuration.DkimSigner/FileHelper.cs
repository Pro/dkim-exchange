using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace Configuration.DkimSigner
{
    /// <summary>
    /// File Helper functions mainly used during the install process
    /// </summary>
    public class FileHelper
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(string name);

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

        /// <summary>
        /// Permit to copy file from source path to destination path
        /// </summary>
        /// <param name="sSourcePath">Source path</param>
        /// <param name="sDestinationPath">Destination path</param>
        /// <param name="oCompleted">Return exception if any during the process</param>
        public static void CopyFile(string sSourcePath, string sDestinationPath, Action<string, string, Exception> oCompleted)
        {
            Stream oSourceFile = new FileStream(sSourcePath, FileMode.Open, FileAccess.Read);
            Stream oDestinationFile = new FileStream(sDestinationPath, FileMode.Create, FileAccess.Write);
            byte[] oBuffer = new byte[0x1000];
            AsyncOperation oAsyncOper = AsyncOperationManager.CreateOperation(null);

            Action<Exception> oCbCompleted = e =>
            {
                if (oCompleted != null)
                {
                    oAsyncOper.Post(delegate { oSourceFile.Close(); oDestinationFile.Close(); oCompleted(sSourcePath, sDestinationPath, e); }, null);
                }
            };

            AsyncCallback oAsyncCall = null;
            oAsyncCall = readResult =>
            {
                try
                {
                    int iRead = oSourceFile.EndRead(readResult);

                    if (iRead > 0)
                    {
                        oDestinationFile.BeginWrite(oBuffer, 0, iRead, writeResult =>
                        {
                            try
                            {
                                oDestinationFile.EndWrite(writeResult);
                                oSourceFile.BeginRead(oBuffer, 0, oBuffer.Length, oAsyncCall, null);
                            }
                            catch (Exception ex)
                            {
                                oCbCompleted(ex);
                            }
                        }, null);
                    }
                    else
                    {
                        oCbCompleted(null);
                    }
                }
                catch (Exception ex)
                {
                    oCbCompleted(ex);
                }
            };

            oSourceFile.BeginRead(oBuffer, 0, oBuffer.Length, oAsyncCall, null);
        }
    }
}