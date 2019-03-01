namespace GTS_SDK_Manager
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
