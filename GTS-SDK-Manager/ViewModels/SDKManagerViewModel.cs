using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS_SDK_Manager
{
    public class SDKManagerViewModel : BaseViewModel
    {
        private string _pathName;

        public string PathName { get => _pathName;
            set
            {
                if(_pathName != value)
                {
                    _pathName = value;
                    SDKManagerBat.PathName = value;
                    Console.WriteLine("Me : " + _pathName);
                    NotifyPropertyChanged();
                    Console.WriteLine("It : " + SDKManagerBat.PathName);
                }
            }
        }

        public SDKManagerViewModel()
        {
            PathName = SDKManagerBat.PathName;

        }

        public void Reset()
        {
            SDKManagerBat.Reset();
        }
    }
}
