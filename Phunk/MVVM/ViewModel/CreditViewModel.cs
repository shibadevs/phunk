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
                Process.Start("https://github.com/patrickfav/uber-apk-signer");
            });

            ApktoolGithub = new RelayCommand(o => {
                Process.Start("https://github.com/iBotPeaches/Apktool");
            });

            AdonisGithub = new RelayCommand(o => { 
                Process.Start("https://github.com/benruehl/adonis-ui");
            });

            RedditThread = new RelayCommand(o => { 
                Process.Start("https://www.reddit.com/r/QuestPiracy/comments/17ajrof/workaround_for_free_trials_of_full_apps_tested/?share_id=hNEaDQHZDpikCPjnSpxL9&utm_content=2&utm_medium=android_app&utm_name=androidcss&utm_source=share&utm_term=1");
            });
        }
    }
}
