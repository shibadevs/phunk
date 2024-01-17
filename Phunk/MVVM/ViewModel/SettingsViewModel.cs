using Phunk.Core;
<<<<<<< HEAD
=======
using Phunk.MVVM.View.Windows;
using Phunk.Ooki;
>>>>>>> 8138598 (Fixed Java Issue and Added Custom Java Path)
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
<<<<<<< HEAD

        public SettingsViewModel()
        {

=======
        public RelayCommand SelectJavaFolderPath { get; set; }
        public SettingsViewModel()
        {
            SelectJavaFolderPath = new RelayCommand((e) =>
            {
                VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
                dialog.Description = "Please select the micro sd folder.";
                dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.
                
                if ((bool)dialog.ShowDialog(new SettingsWindow())) {
                        GlobalViewModel.JavaPathFolderSettingsTxt = dialog.SelectedPath;
                }


            });
>>>>>>> 8138598 (Fixed Java Issue and Added Custom Java Path)
        }
    }
}
