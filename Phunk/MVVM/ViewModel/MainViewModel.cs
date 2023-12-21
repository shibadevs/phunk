using AdonisUI.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;
using Phunk.Core;
using Phunk.MVVM.View.Windows;
using Phunk.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace Phunk.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;

        #region APK
        private string? _filePath;
        public string? FilePath
        {
            get { return _filePath; }
            set { _filePath = value; OnPropertyChanged(); }
        }

        private string? _apkName;

        public string? ApkName
        {
            get { return _apkName; }
            set { _apkName = value; OnPropertyChanged(); }
        }

        private string _originalPackageName;

        public string OriginalPackageName
        {
            get { return _originalPackageName; }
            set { _originalPackageName = value; OnPropertyChanged(); }
        }

        private string _currentPackageName;

        public string CurrentPackageName
        {
            get { return _currentPackageName; }
            set { _currentPackageName = value; OnPropertyChanged(); }
        }



        #endregion

        #region RELAY COMMANDS
        public RelayCommand StartCommand { get; set; }
        public RelayCommand SettingsCommand { get; set; }
        public RelayCommand SelectAPKCommand { get; set; }
        public RelayCommand CreditsCommand { get; set; }
        public RelayCommand ClearLogsCommand { get; set; }
        #endregion

        #region DOWNLOADING
        private string? _currentFilePath;
        public string? CurrentFilePath
        {
            get { return _currentFilePath; }
            set { _currentFilePath = value; OnPropertyChanged(); }
        }
        #endregion

        public MainViewModel()
        {
            Initialize();

            StartCommand = new RelayCommand(async o =>
            {
                GlobalViewModel.StatusText = "";

                try
                {
                    GlobalViewModel.CanStart = false;
                    GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Starting Process";
                    await PerformTaskAsync();

                } catch (Exception ex)
                {
                    GlobalViewModel.StatusText = ex.Message;
                }
            });

            SettingsCommand = new RelayCommand(o => { });
            SelectAPKCommand = new RelayCommand(o => {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "APK files (*.apk)|*.apk";

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    ApkName = Path.GetFileName(selectedFilePath);
                    FilePath = selectedFilePath;
                    GlobalViewModel.PhunkLogs += "[Phunk] ~ Selected " + ApkName;
                    GlobalViewModel.PhunkLogs += "\n[Phunk] ~ " + FilePath;


                    GlobalViewModel.CanStart = true;
                }
            });

            CreditsCommand = new RelayCommand(o => { 
                CreditsWindow window = new CreditsWindow();
                window.Show();
            });

            ClearLogsCommand = new RelayCommand(o => {
                GlobalViewModel.PhunkLogs = "";
            });
        }

        private void Initialize()
        {
            GlobalViewModel.StatusText = "(￢з￢) Waiting for User";
            GlobalViewModel.CanStart = false;
            GlobalViewModel.ProgressValue = 0;


            OriginalPackageName = "n/a";
            FilePath = "n/a";
            ApkName = "n/a";
            if (!Directory.Exists("bin")) Directory.CreateDirectory("bin");
            if (!Directory.Exists("temp")) Directory.CreateDirectory("temp");
        }

        /// <summary>
        /// Performs the Task Asynchronously
        /// </summary>
        /// <returns></returns>
        private async Task PerformTaskAsync()
        {
            GlobalViewModel.StatusText = "((⇀‸↼)) Checking Requirements";
            await Task.Delay(500);
            await CheckDependency();

            GlobalViewModel.ProgressValue = 0;
            if (GlobalViewModel.MetRequirements)
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string binFolderPath = Path.Combine(appDirectory, "bin");

                if (!File.Exists(Path.Combine(binFolderPath, "apktool.jar")) && !File.Exists(Path.Combine(binFolderPath, "apktool.bat")) && !File.Exists(Path.Combine(binFolderPath, "uberapksigner.jar"))) {
                    
                    // Continue with the task
                    GlobalViewModel.StatusText = "((⇀‸↼)) Downloading Required Files";
                    GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Downloading Files";

                    // Download the required files
                    await DownloadFiles();

                    GlobalViewModel.StatusText = "(人´∀`) Success";

                    await Task.Delay(500);
                }

                ApkHandler handler = new ApkHandler();

                GlobalViewModel.ProgressValue = 0;
                GlobalViewModel.StatusText = "((⇀‸↼)) Extracting the APK file using apktool";
                await DecompileApk(binFolderPath, handler);

                GlobalViewModel.ProgressValue = 0;
                GlobalViewModel.StatusText = "((⇀‸↼)) Streaming AndroidManifest.xaml";
                GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Streaming AndroidManifest.xaml and apktool.yaml";
                await StreamApk();

                GlobalViewModel.ProgressValue = 50;
                GlobalViewModel.StatusText = "((⇀‸↼)) Replacing all files & folder names, and inside the files aswell";
                GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Replaced all files & folder names";
                await ReplaceNames();

                GlobalViewModel.ProgressValue = 100;
                GlobalViewModel.StatusText = "(人´∀`) Successfully Replaced";
                GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Success";

                await Task.Delay(500);

                GlobalViewModel.ProgressValue = 0;
                GlobalViewModel.StatusText = "((⇀‸↼)) Building APK using apktool";
                GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Building APK using apktool";
                await BuildApk(binFolderPath, handler);

                GlobalViewModel.ProgressValue = 0;
                GlobalViewModel.StatusText = "((⇀‸↼)) Signing and Zipaligning apk";
                GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Signing and Zipaligning Apk";

                await SignApk(binFolderPath, handler);

                GlobalViewModel.ProgressValue = 100;
                GlobalViewModel.StatusText = "(人´∀`) Success! -> Saved @ Temp -> " + ApkName.Split(".")[0] + "-aligned-debugSigned.apk";
                GlobalViewModel.PhunkLogs += "\n[Phunk] (人´∀`) " + "Saved @ Temp -> " + ApkName.Split(".")[0] + "-aligned-debugSigned.apk"; ;

                GlobalViewModel.CanStart = true;
                Process.Start("explorer.exe", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp"));
                //AdonisUI.Controls.MessageBox.Show("The Package Name has been replaced! You can now sideload the game!", "(ノ^∇^) Success!", AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.None);
            }
            else
            {
                GlobalViewModel.StatusText = "( ´-ω-` ) Can't start the task because you having missing requirements: " + GlobalViewModel.MissingRequirements;
            }
        }

        /// <summary>
        /// Checks if the requirements are met first before performing the actual task
        /// </summary>
        /// <returns></returns>
        private Task CheckDependency()
        {
            try
            {
                var java = ReqChecker.Check("java", checkJava: true);
                
                GlobalViewModel.MissingRequirements += !java ? " Java (JDK 8+) " : "";

                if (GlobalViewModel.MissingRequirements == "")
                {
                    GlobalViewModel.StatusText = "(人´∀`) All Requirements Met";
                    GlobalViewModel.ProgressValue = 100;
                    GlobalViewModel.MetRequirements = true;

                } else
                {
                    GlobalViewModel.StatusText = "（◞‸◟）Missing Requirements - " + GlobalViewModel.MissingRequirements;
                }
            }
            catch (Exception ex)
            {
                
            }
            
            return Task.Delay(1000);
        }

        private async Task DownloadFiles()
        {
            // Download APK Tools
            GlobalViewModel.StatusText = "((⇀‸↼)) Downloading Apktools";
            GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Downloading Apktools";
            await FetchAndDownload("https://api.github.com/repos/iBotPeaches/apktool/releases", 0, "apktool.jar");
            await DownloadAsync("https://raw.githubusercontent.com/iBotPeaches/Apktool/master/scripts/windows/apktool.bat", "apktool.bat");

            // Download Uber Apk Signer
            GlobalViewModel.StatusText = "((⇀‸↼)) Downloading Uber Apk Signer";
            GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Downloading Uber Apk Signer";

            await FetchAndDownload("https://api.github.com/repos/patrickfav/uber-apk-signer/releases", 1, "uberapksigner.jar");

            await Task.Delay(500);
        }

        private async Task DecompileApk(string binFolderPath, ApkHandler handler)
        {
            // Run APK Tool

            var result = await Task.Run(() => handler.DecompileApkTool(Path.Combine(binFolderPath, "apktool.jar"), FilePath, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp/extracted")));

            if (result == 0)
            {
                GlobalViewModel.ProgressValue = 100;
                GlobalViewModel.StatusText = "(人´∀`) Extraction Success";
                GlobalViewModel.PhunkLogs += "\n[Phunk] ~ Extraction Success";
            }
            else
            {
                GlobalViewModel.StatusText = "（◞‸◟） Extraction Failed";
                GlobalViewModel.PhunkLogs += "[Phunk] ! Extraction Failed";
            }

            await Task.Delay(500);

            return;
        }

        private async Task StreamApk()
        {
            string extracted_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp/extracted");
            string packageValue = Util.GetPackageValue(Path.Combine(extracted_path, "AndroidManifest.xml"), "package=");

            GlobalViewModel.StatusText = "((⇀‸↼)) Getting Original Package Name";
            OriginalPackageName = packageValue;

            CurrentPackageName = OriginalPackageName.Split(".")[1].Split(".")[0];
            string final_package_name = OriginalPackageName.Replace(CurrentPackageName, "demo");
            CurrentPackageName = final_package_name;

            await Task.Run(() =>
            {
                Util.ReplaceTextInFile(Path.Combine(extracted_path, "AndroidManifest.xml"), OriginalPackageName, CurrentPackageName);
                Util.ReplaceTextInFile(Path.Combine(extracted_path, "apktool.yml"), OriginalPackageName, CurrentPackageName);
            });

            GlobalViewModel.ProgressValue = 100;

            await Task.Delay(500);
        }

        private Task ReplaceNames()
        {
            string extracted_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp/extracted");
            string folderPath = Path.Combine(extracted_path, "smali/com/" + OriginalPackageName.Split(".")[1].Split(".")[0]);
            string parentDirectory = Path.GetDirectoryName(folderPath);
            string newFolderPath = Path.Combine(parentDirectory, CurrentPackageName.Split(".")[1].Split(".")[0]);
            Util.RenameFolder(folderPath, newFolderPath);

            string mainDirectory = Path.GetDirectoryName(FilePath);

            // If the OBB exists in where the user selected the apk, we will do some certain process to ensure that everything will work.
            if (Directory.Exists(Path.Combine(mainDirectory, OriginalPackageName)))
            {
                // Changes the OBB Folder and File Name to the Set Package Name
                Util.RenameFolder(Path.Combine(mainDirectory, OriginalPackageName), Path.Combine(mainDirectory, CurrentPackageName));
                Util.ReplaceKeywordInFileNames(Path.Combine(mainDirectory, CurrentPackageName), OriginalPackageName, CurrentPackageName);
                Util.MoveFolder(Path.Combine(mainDirectory, CurrentPackageName), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp"));
            }

            Util.ReplaceTextInDirectory(Path.Combine(extracted_path, "smali"), OriginalPackageName.Replace(".", "/"), CurrentPackageName.Replace(".", "/"));

            GlobalViewModel.ProgressValue = 100;

            return Task.Delay(500);
        }

        private async Task BuildApk(string binFolderPath, ApkHandler apkHandler)
        {
            string extracted_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp/extracted");

            await Task.Run(() =>
            {
                apkHandler.BuildApkTool(Path.Combine(binFolderPath, "apktool.jar"), extracted_path, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp/" + ApkName));
            });

            GlobalViewModel.ProgressValue = 100;
            await Task.Delay(500);
        }

        private async Task SignApk(string binFolderPath, ApkHandler apkHandler)
        {
            string extracted_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp/extracted");

            await Task.Run(() =>
            {
                apkHandler.SignApkTool(Path.Combine(binFolderPath, "uberapksigner.jar"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp/" + ApkName), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp"));
            });

            Task.Delay(500);
        }

        #region Downloading Functions


        private async Task FetchAndDownload(string url, int num, string name)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    IWebProxy webProxy = WebRequest.DefaultWebProxy;
                    webProxy.Credentials = CredentialCache.DefaultCredentials;
                    httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                      "Windows NT 5.2; .NET CLR 1.0.3705;)");

                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    response.EnsureSuccessStatusCode(); // Throw an exception if the status code is not successful

                    var jsonResult = await response.Content.ReadAsStringAsync();
                    dynamic dynObj = JsonConvert.DeserializeObject(jsonResult);
                    string browser_url = dynObj[0].assets[num].browser_download_url;

                    await DownloadAsync(browser_url, name);
                }
            }
            catch (HttpRequestException ex)
            {
                
            }
            catch (Exception ex)
            {
                
            }
        }

        private async Task DownloadAsync(string url, string name)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    IWebProxy webProxy = WebRequest.DefaultWebProxy;
                    webProxy.Credentials = CredentialCache.DefaultCredentials;
                    httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                      "Windows NT 5.2; .NET CLR 1.0.3705;)");

                    HttpResponseMessage response = await httpClient.GetAsync(url);

                    response.EnsureSuccessStatusCode(); // Throw an exception if the status code is not successful

                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    // Combine the current directory with "bin"
                    string binFolderPath = Path.Combine(appDirectory, "bin");

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = File.Create(Path.Combine(binFolderPath, name)))
                    {
                        await Dispatcher.CurrentDispatcher.Invoke(async () =>
                        {
                            var buffer = new byte[81920];
                            var bytesRead = 0;
                            var totalBytesRead = 0;
                            var fileSize = response.Content.Headers.ContentLength;

                            var progress = new Progress<int>(percentage =>
                            {
                            
                                    GlobalViewModel.ProgressValue = percentage;
                                    GlobalViewModel.StatusText = $"[{percentage}%] Downloading - " + url;
                            });

                            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fileStream.WriteAsync(buffer, 0, bytesRead);
                                totalBytesRead += bytesRead;

                                var percentage = (int)((double)totalBytesRead / fileSize * 100);
                                ((IProgress<int>)progress).Report(percentage);
                            }
                        });
                    }

                    //ExtractZips(Path.GetFileName(new Uri(url).LocalPath));
                }
            }
            catch (HttpRequestException ex)
            {
                
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion
    }
}
