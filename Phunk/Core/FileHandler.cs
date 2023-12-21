using Phunk.MVVM.ViewModel;
using Phunk.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Threading;

namespace Phunk.Core
{
    public class FileHandler
    {
        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;
        public void ExtractZip(string zipFilePath, string extractToDirectory)
        {
            try
            {
                if (File.Exists(zipFilePath))
                {
                    // Ensure the destination directory exists
                    Directory.CreateDirectory(extractToDirectory);

                    // Extract the zip file
                    ZipFile.ExtractToDirectory(zipFilePath, extractToDirectory);

                    GlobalViewModel.StatusText = $"Zip file '{Path.GetFileName(zipFilePath)}' successfully extracted to '{extractToDirectory}'.";
                }
                else
                {
                    GlobalViewModel.PhunkLogs += "\n[Phunk] ! Zip Failed";
                }
            }
            catch (Exception ex)
            {
                GlobalViewModel.PhunkLogs += "\n[Phunk] ! Zip Failed";
            }
        }
    }
}