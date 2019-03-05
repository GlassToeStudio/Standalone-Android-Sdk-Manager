namespace SdkManger.UI
{
    public class CommandLineTabViewModel : TabBaseViewModel
    {
        private string argsList;

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

        public CommandLineTabViewModel() : base()
        {

        }
    }
}
