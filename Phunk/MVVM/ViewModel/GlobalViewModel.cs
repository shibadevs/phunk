using Phunk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Phunk.MVVM.ViewModel
{
    public class GlobalViewModel : ObservableObject
    {
        public static GlobalViewModel Instance { get; } = new GlobalViewModel();

        /// <summary>
        /// Status text to let the user know what is happening
        /// </summary>
        private string? _statusText;
		public string? StatusText
		{
			get { return _statusText; }
			set { _statusText = value; OnPropertyChanged(); }
		}

        /// <summary>
        /// For the progress bar, displays the current progress
		/// for the executed task
        /// </summary>
        private float _progressValue;
		public float ProgressValue
		{
			get { return _progressValue; }
			set { _progressValue = value; OnPropertyChanged(); }
		}

		/// <summary>
		/// If all requirements are met for the application
		/// </summary>
		private bool _metRequirements;
		public bool MetRequirements
		{
			get { return _metRequirements; }
			set { _metRequirements = value; OnPropertyChanged(); }
		}

        /// <summary>
        /// Stores the logs from the Phunk and displays them
        /// </summary>
        private string? _phunkLogs;
		public string? PhunkLogs
		{
			get { return _phunkLogs; }
			set { _phunkLogs = value; OnPropertyChanged(); }
		}

		private string? _missingRequirements;

		public string? MissingRequirements
		{
			get { return _missingRequirements; }
			set { _missingRequirements = value; OnPropertyChanged(); }
		}

		private bool _canStart;

		public bool CanStart
		{
			get { return _canStart; }
			set { _canStart = value; OnPropertyChanged(); }
		}

		private string _newApkName;

		public string NewApkName
		{
			get { return _newApkName; }
			set { _newApkName = value; OnPropertyChanged(); }
		}

	}
}
