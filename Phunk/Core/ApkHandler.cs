using Phunk.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Phunk.Core
{
    public class ApkHandler
    {
        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;
        public int DecompileApkTool(string apktoolPath, string apkPath, string outputApkPath, string additionalParams = "")
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = !GlobalViewModel.IsCustomJavaPath && GlobalViewModel.JavaPathFolderSettingsTxt.Length == 0
                        ? "java"
                        : Path.Combine(GlobalViewModel.JavaPathFolderSettingsTxt + "/bin/java.exe"),
                    Arguments = $"-jar \"{apktoolPath}\" -f d \"{apkPath}\" -o \"{outputApkPath}\"" + " " + additionalParams,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();
                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();


                    if (process.ExitCode == 0)
                    {
                        return 0;
                        //Console.WriteLine("Output: " + output);
                    }
                    else
                    {
                        MessageBox.Show(process.StandardError.ReadToEnd());
                        GlobalViewModel.PhunkLogs += "[Phunk] ! " + error;
                        return 1;
                        //Console.WriteLine("Apktool build failed.");
                    }
                }

            }
            catch (Exception ex)
            {
                GlobalViewModel.PhunkLogs += "[Phunk] ! " + ex.Message;
                MessageBox.Show(ex.Message);
                return 1;
            }
        }

        public void BuildApkTool(string apktoolPath, string directoryPath, string outputApkPath)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = !GlobalViewModel.IsCustomJavaPath && GlobalViewModel.JavaPathFolderSettingsTxt.Length == 0
                        ? "java"
                        : Path.Combine(GlobalViewModel.JavaPathFolderSettingsTxt + "/bin/java.exe"),
                    Arguments = $"-jar \"{apktoolPath}\" -f b \"{directoryPath}\" -o \"{outputApkPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();
                    process.WaitForExit();
                   

                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    if (process.ExitCode == 0)
                    {
                        GlobalViewModel.StatusText = "(人´∀`) Building Success";
                        GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Building Success";


                        //Console.WriteLine("Output: " + output);
                    }
                    else
                    {
                        GlobalViewModel.StatusText = "（◞‸◟） Building Failed";
                        GlobalViewModel.PhunkLogs += "\n[Phunk] ! Extraction Failed";

                        //Console.WriteLine("Apktool build failed.");
                        MessageBox.Show(error);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
                GlobalViewModel.PhunkLogs += "\n[Phunk] ! " + ex.Message;


            }
        }

        public void SignApkTool(string ubersignerPath, string apkPath, string outputApkPath, string additionalParams = "")
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = !GlobalViewModel.IsCustomJavaPath && GlobalViewModel.JavaPathFolderSettingsTxt.Length == 0
                        ? "java"
                        : Path.Combine(GlobalViewModel.JavaPathFolderSettingsTxt + "/bin/java.exe"),
                    Arguments = $"-jar \"{ubersignerPath}\" -a \"{apkPath}\" -o \"{outputApkPath}\" --allowResign" + " " + additionalParams,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (Process process = new Process { StartInfo = psi })
                    {
                        process.Start();
                        process.WaitForExit();

                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();

                        if (process.ExitCode == 0)
                        {
                            GlobalViewModel.StatusText = "(人´∀`) Signing and Zipaligning Success!";
                            GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Signing and Zipaligning Success!";

                            //Console.WriteLine("Output: " + output);
                        }
                        else
                        {
                            GlobalViewModel.StatusText = "（◞‸◟） Signing and Zipaligning Failed!";
                            GlobalViewModel.PhunkLogs += "\n[Phunk] ! Signing and Zipaligning Failed";
                            GlobalViewModel.PhunkLogs += "\n[Phunk] ! " + error;

                            //Console.WriteLine("Apktool build failed.");
                            MessageBox.Show(error);
                        }
                    }
            }
            catch (Exception ex)
            {
                GlobalViewModel.PhunkLogs += "\n[Phunk] ! " + ex.Message;
                //Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
