using Phunk.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Phunk.Core
{
    public class ReqChecker
    {
        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;
        public bool Check(string value, bool checkJava = false)
        {
            return checkJava == false ? IsCommandAvailable(value) : IsJavaVersionValid(value);
        }

        static bool IsCommandAvailable(string command)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = "where";
                    process.StartInfo.Arguments = command;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    return !string.IsNullOrWhiteSpace(output);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsJavaVersionValid(string command)
        {
            string versionstr = "";

            try
            {
                using (Process process = new Process())
                {
                    if (GlobalViewModel.JavaPathFolderSettingsTxt.Length != 0)
                    {
                        GlobalViewModel.PhunkLogs += "\n[Phunk] using JAVA from " + GlobalViewModel.JavaPathFolderSettingsTxt;
                    }
                    process.StartInfo.FileName = !GlobalViewModel.IsCustomJavaPath && GlobalViewModel.JavaPathFolderSettingsTxt.Length == 0
                        ? "java"
                        : Path.Combine(GlobalViewModel.JavaPathFolderSettingsTxt + "/bin/java.exe");
                    process.StartInfo.Arguments = "-version";
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    string output = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        Match match = Regex.Match(output, @"version ""(\d+(\.\d+(_\d+)?)?)");

                        if (match.Success)
                        {
                            string versionString = match.Groups[1].Value.Trim();
                            versionstr = versionString;

                            if (Version.TryParse(versionString, out Version version) && version.Major == 1 && version.Minor >= 8)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalViewModel.PhunkLogs += "\n[Phunk] There was an error checking for the requirements. " + ex.ToString();
                GlobalViewModel.CanStart = true;
            }

            GlobalViewModel.PhunkLogs += "\n[Phunk] Version is not up to date: " + versionstr;
            return false;
        }
    }
}
