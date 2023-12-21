using Phunk.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phunk.MVVM.ViewModel
{
    public class CreditViewModel : ObservableObject
    {
        public RelayCommand? UberGithub { get; set; }
        public RelayCommand? ApktoolGithub { get; set; }

        public RelayCommand? AdonisGithub { get; set; }

        public CreditViewModel()
        {
            UberGithub = new RelayCommand(o =>
            {
                Process.Start("https://github.com/patrickfav/uber-apk-signer");
            });

            ApktoolGithub = new RelayCommand(o => {
                Process.Start("https://github.com/iBotPeaches/Apktool");
            });

            AdonisGithub = new RelayCommand(o => { 
                Process.Start("https://github.com/benruehl/adonis-ui");
            });
        }
    }
}
