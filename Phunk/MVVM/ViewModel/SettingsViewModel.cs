using Phunk.Core;
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

        public SettingsViewModel()
        {

        }
    }
}
