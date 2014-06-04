using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Configuration.DkimSigner
{
    /// <summary>
    /// File Helper functions mainly used during the install process
    /// </summary>
    class FileHelper
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteFile(string name);

        /// <summary>
        /// Removes the Zone identifier from a file (protects files downloaded through IE).
        /// Only supported on NTFS file system. If file is located on another FS type, this method will most probably return false.
        /// See: http://stackoverflow.com/questions/6374673/unblock-file-from-within-net-4-c-sharp
        /// </summary>
        /// <param name="fileName">The file to unlock</param>
        /// <returns>True if successfully unlocked.</returns>
        public bool Unblock(string fileName)
        {
            return DeleteFile(fileName + ":Zone.Identifier");
        }


        public static void CopyFile(String sourcePath, String destinationPath, Action<String, String, Exception> completed)
        {
            Stream source = new FileStream(sourcePath, FileMode.Open, FileAccess.Read);
            Stream destination = new FileStream(destinationPath, FileMode.Create, FileAccess.Write);
            byte[] buffer = new byte[0x1000];
            AsyncOperation asyncOp = AsyncOperationManager.CreateOperation(null);

            Action<Exception> cbCompleted = e =>
            {
                if (completed != null) asyncOp.Post(delegate
                     {
                         source.Close();
                         destination.Close();
                         completed(sourcePath, destinationPath, e);
                     }, null);
            };

            AsyncCallback rc = null;
            rc = readResult =>
            {
                try
                {
                    int read = source.EndRead(readResult);
                    if (read > 0)
                    {
                        destination.BeginWrite(buffer, 0, read, writeResult =>
                        {
                            try
                            {
                                destination.EndWrite(writeResult);
                                source.BeginRead(
                                    buffer, 0, buffer.Length, rc, null);
                            }
                            catch (Exception exc) { cbCompleted(exc); }
                        }, null);
                    }
                    else cbCompleted(null);
                }
                catch (Exception exc) { cbCompleted(exc); }
            };

            source.BeginRead(buffer, 0, buffer.Length, rc, null);
        }
    }
}
