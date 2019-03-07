using System;
using SdkManager.Core;
using System.Windows.Data;
using System.Globalization;

namespace SdkManager.UI
{
    [ValueConversion(typeof(PackageStatus), typeof(string))]
    public class StatusEnumToStringConverter : IValueConverter
    {
        public static StatusEnumToStringConverter Instance = new StatusEnumToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string r = "";
            if (value != null)
            {
                var status = (PackageStatus)value;
                switch (status)
                {
                    case PackageStatus.INSTALLED:
                        r = "Installed";
                        break;
                    case PackageStatus.UPDATE_AVAILABLE:
                        r = "Update Available";
                        break;
                    case PackageStatus.NOT_INSTALLED:
                        r = "Not Installed";
                        break;
                    default:
                        break;
                }
            }
            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
