using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS_SDK_Manager
{
    public class SdkManagerBatViewModel : BaseViewModel
    {
        private string _pathName;

        public string PathName
        {
            get => _pathName;
            set
            {
                if (_pathName != value)
                {
                    _pathName = value;
                    SdkManagerBat.PathName = value;
                    Console.WriteLine("Me : " + _pathName);
                    NotifyPropertyChanged();
                    Console.WriteLine("It : " + SdkManagerBat.PathName);
                }
            }
        }

        private string _consoleOutput = "Status";
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

        public SdkManagerBatViewModel()
        {
            SdkManagerBat.SendOutput += GetSKDManagerOutput;
            PathName = SdkManagerBat.PathName;
        }

        public async Task<string> InstallOrUpdatePackages(string args)
        {
            var t = await Task.Run(() => SdkManagerBat.InstallPackagesAsync(args));
            return t;
        }

        public async Task<string> UninstallPackages(string args)
        {
            var t = await Task.Run(() => SdkManagerBat.UninstallPackagesAsync(args));
            return t;
        }

        private void GetSKDManagerOutput(string output)
        {
            ConsoleOutput = output?.Trim();
        }

        public void Reset()
        {
            SdkManagerBat.ClearCache();
        }
    }
}
