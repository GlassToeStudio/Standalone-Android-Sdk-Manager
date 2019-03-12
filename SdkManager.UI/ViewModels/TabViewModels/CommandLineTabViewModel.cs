using System.Threading.Tasks;
using System.Windows.Input;

namespace SdkManager.UI
{
    public class CommandLineTabViewModel : TabBaseViewModel
    {
        private string argsList;
        private SdkManagerBatViewModel _sdkManager;
        public ICommand ExecuteCommand { get; set; }

        public string ArgsList
        {
            get => argsList;
            set
            {
                if (argsList != value)
                {
                    argsList = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public CommandLineTabViewModel(MainWindowViewModel main, SdkManagerBatViewModel sdkManager) : base(main)
        {
            _sdkManager = sdkManager;
            ExecuteCommand = new RelayCommand(Execute);
        }

        /// <summary>
        /// Do somethign with the output... Just outputs to the little Label on the MainWindow
        /// </summary>
        private void Execute()
        {
            var t = Task.Run(async () =>
            {
                await _sdkManager.RunCommands(argsList);
            });        
        }
    }
}
