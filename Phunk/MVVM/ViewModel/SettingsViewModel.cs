using Phunk.Core;
using Phunk.MVVM.View.Windows;
using Phunk.Ooki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phunk.MVVM.ViewModel
{
    public class SettingsViewModel : ObservableObject
    {
        public GlobalViewModel GlobalViewModel { get; } = GlobalViewModel.Instance;
        public RelayCommand SelectJavaFolderPath { get; set; }

        public SettingsViewModel()
        {

            SelectJavaFolderPath = new RelayCommand((e) =>
            {
                VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
                dialog.Description = "Please select the jre folder.";
                dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.
                
                if ((bool)dialog.ShowDialog(new SettingsWindow())) {
                        GlobalViewModel.JavaPathFolderSettingsTxt = dialog.SelectedPath;
                }


            });
        }
    }
}
