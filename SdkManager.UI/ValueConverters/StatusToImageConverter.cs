using System;
using SdkManager.Core;
using System.Windows.Data;
using System.Globalization;

namespace SdkManager.UI
{
    [ValueConversion(typeof(Boolean), typeof(Uri))]
    public class StatusToImageConverter : IValueConverter
    {
        public static StatusToImageConverter Instance = new StatusToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var uri = (ClickAction)value;
            switch (uri)
            {
                case ClickAction.Install:
                    return new Uri("pack://application:,,,/Images/download.png");
                case ClickAction.Uninstall:
                    return new Uri("pack://application:,,,/Images/delete.png");
                case ClickAction.Donothing:
                    return new Uri("pack://application:,,,/Images/blank.png");
                default:
                    break;
            }

            return new Uri("pack://application:,,,/Images/blank.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
