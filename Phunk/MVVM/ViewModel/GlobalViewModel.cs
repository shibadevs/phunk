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

		private bool _isCustomJavaPath;

		public bool IsCustomJavaPath
		{
			get { return _isCustomJavaPath; }
			set { _isCustomJavaPath = value; OnPropertyChanged(); }
		}


		/// <summary>
		/// Enables/Disables the Settings Window if process has started or not
		/// </summary>
		private bool _isProcessStarting;
		public bool IsProcessStarting
		{
			get { return _isProcessStarting; }
			set { _isProcessStarting = value; OnPropertyChanged(); }
		}

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

		private string? _newApkName;

		public string? NewApkName
		{
			get { return _newApkName; }
			set { _newApkName = value; OnPropertyChanged(); }
		}

		#region Settings
		/// <summary>
		/// Main Settings
		/// </summary>

		/// <summary>
		/// Main Settings
		/// </summary>
		private string? _javaPathFolderSettingsTxt;

		public string? JavaPathFolderSettingsTxt
		{
			get { return _javaPathFolderSettingsTxt; }
			set { _javaPathFolderSettingsTxt = value; OnPropertyChanged(); }
		}
		
		private string? _finalOutputNameSettingsTxt;
		public string? FinalOutputNameSettingsTxt
        {
			get { return _finalOutputNameSettingsTxt; }
			set { _finalOutputNameSettingsTxt = value; OnPropertyChanged(); }
		}

		private string? _decompileAdditionalParamsSettingsTxt;
		public string? DecompileAdditionalParamsSettingsTxt
        {
			get { return _decompileAdditionalParamsSettingsTxt; }
			set { _decompileAdditionalParamsSettingsTxt = value; OnPropertyChanged(); }
		}

		private string? _signingZipaligningParamsSettingsTxt;
		public string? SigningZipaligningParamsSettingsTxt
        {
			get { return _signingZipaligningParamsSettingsTxt; }
			set { _signingZipaligningParamsSettingsTxt = value; OnPropertyChanged(); }
		}

		private string? _customPackageNameSettingsTxt;
		public string? CustomPackageNameSettingsTxt
		{
			get { return _customPackageNameSettingsTxt; }
			set { _customPackageNameSettingsTxt = value; OnPropertyChanged(); }
		}


		/// <summary>
		/// Configs
		/// </summary>
		private bool _autoCleanSettingsBoolean;
		public bool AutoCleanSettingsBoolean
		{
			get { return _autoCleanSettingsBoolean; }
			set { _autoCleanSettingsBoolean = value; OnPropertyChanged(); }
		}

		private bool _useApkToolSettingsBoolean;
		public bool UseApkToolSettingsBoolean
        {
			get { return _useApkToolSettingsBoolean; }
			set { _useApkToolSettingsBoolean = value; OnPropertyChanged(); }
		}

		private bool _autoUpdatePhunkSettingsBoolean;
		public bool AutoUpdatePhunkSettingsBoolean
		{
			get { return _autoUpdatePhunkSettingsBoolean; }
			set { _autoUpdatePhunkSettingsBoolean = value; OnPropertyChanged(); }
		}
		#endregion
	}
}
