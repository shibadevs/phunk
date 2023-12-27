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
        public RelayCommand? RedditThread { get; set; }

        public CreditViewModel()
        {
            UberGithub = new RelayCommand(o =>
            {
                Process.Start(new ProcessStartInfo("https://github.com/patrickfav/uber-apk-signer") { UseShellExecute = true });
            });

            ApktoolGithub = new RelayCommand(o => {
                Process.Start(new ProcessStartInfo("https://github.com/iBotPeaches/Apktool") { UseShellExecute = true });
            });

            AdonisGithub = new RelayCommand(o => { 
                Process.Start(new ProcessStartInfo("https://github.com/benruehl/adonis-ui") { UseShellExecute = true });
            });

            RedditThread = new RelayCommand(o => { 
                Process.Start(new ProcessStartInfo("https://www.reddit.com/r/QuestPiracy/comments/17ajrof/workaround_for_free_trials_of_full_apps_tested/?share_id=hNEaDQHZDpikCPjnSpxL9&utm_content=2&utm_medium=android_app&utm_name=androidcss&utm_source=share&utm_term=1") { UseShellExecute = true });
            });
        }
    }
}
