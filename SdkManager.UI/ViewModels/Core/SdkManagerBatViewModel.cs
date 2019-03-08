using SdkManager.Core;
using System.Threading.Tasks;

namespace SdkManager.UI
{
    public class SdkManagerBatViewModel : BaseViewModel
    {
        #region Pivate Backing Fields 
        /// <summary>
        /// PathName backing field
        /// </summary>
        private string _pathName;

        /// <summary>
        /// ConsoleOutput backing field.
        /// </summary>
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

        /// <summary>
        /// Output recieved from console window.
        /// </summary>
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

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SdkManagerBatViewModel()
        {
            SdkManagerBat.CommandLineOutputReceived += OnCommandLineOutputReceived;
            PathName = SdkManagerBat.PathName;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Will attempt to install teh passed on package;name arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task InstallOrUpdatePackages(string args)
        {
            var t = await Task.Run(() => SdkManagerBat.InstallPackagesAsync(args));
        }

        /// <summary>
        /// Will attempt to uninstall the passed in package;name arguments
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task UninstallPackages(string args)
        {
            var t = await Task.Run(() => SdkManagerBat.UninstallPackagesAsync(args));
        }

        /// <summary>
        /// Clear the saved data in Sdkmanager to prepare to fetch a new set of data.
        /// </summary>
        public void ClearCache()
        {
            SdkManagerBat.ClearCache();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Called when text is written tot he console window.
        /// </summary>
        /// <param name="output"></param>
        private void OnCommandLineOutputReceived(string output)
        {
            ConsoleOutput = output?.Trim();
        }
        
        #endregion
    }
}
