using SdkManager.Core;
using System.Threading.Tasks;

namespace SdkManager.UI
{
    public class SdkManagerBatViewModel : BaseViewModel
    {
        #region Pivate Backing Fields 

        private string _pathName;
        private string _consoleOutput = "Status";

        #endregion

        #region Public Properties

        /// <summary>
        /// The path to sdkmanager.bat
        /// </summary>
        public string PathName
        {
            get => SdkManagerBat.PathName;
            set
            {
                _pathName = value;
                SdkManagerBat.PathName = value;
            }
        }

        public string ConsoleOutput
        {
            get => _consoleOutput;
            set
            {
                if(_consoleOutput != value)
                {
                    _consoleOutput = value;
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor

        public SdkManagerBatViewModel()
        {
            SdkManagerBat.CommandLineOutputReceived += OnCommandLineOutputReceived;
            PathName = SdkManagerBat.PathName;
        }

        #endregion

        #region Public Methods

        public async Task InstallOrUpdatePackages(string args)
        {
            var t = await Task.Run(() => SdkManagerBat.InstallPackagesAsync(args));
        }

        public async Task UninstallPackages(string args)
        {
            var t = await Task.Run(() => SdkManagerBat.UninstallPackagesAsync(args));
        }

        public void ClearCache()
        {
            SdkManagerBat.ClearCache();
        }

        #endregion

        #region Private Methods

        private void OnCommandLineOutputReceived(string output)
        {
            ConsoleOutput = output?.Trim();
        }
        
        #endregion
    }
}
