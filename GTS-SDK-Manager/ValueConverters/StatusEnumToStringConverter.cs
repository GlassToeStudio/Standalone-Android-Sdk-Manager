using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GTS_SDK_Manager
{
    [ValueConversion(typeof(PackageStatus), typeof(string))]
    public class StatusEnumToStringConverter : IValueConverter
    {

        public static StatusEnumToStringConverter Instance = new StatusEnumToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (PackageStatus)value;
            string r = "Not Installed";
            switch (status)
            {
                case PackageStatus.INSTALLED:
                    r = "Installed";
                    break;
                case PackageStatus.UPDATE_AVAILABLE:
                    r = "Update Available";
                    break;
                default:
                    break;
            }
            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
