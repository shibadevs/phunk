using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Phunk.Core
{
    public class ReqChecker
    {
        public static bool Check(string value, bool checkJava = false)
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

        static bool IsJavaVersionValid(string command)
        {
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = command;
                    process.StartInfo.Arguments = "-version";
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();
                    string output = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    // Check if the command execution was successful
                    if (process.ExitCode == 0)
                    {
                        // Extract the version information from the output
                        Match match = Regex.Match(output, @"version ""(\d+\.\d+)");
                        if (match.Success)
                        {
                            // Parse the version and check if it's JDK 8 or higher
                            string versionString = match.Groups[1].Value;
                            if (double.TryParse(versionString, out double version) && version >= 1.8)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Handle exceptions if needed
            }

            return false;
        }
    }
}
