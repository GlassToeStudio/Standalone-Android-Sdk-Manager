using System;
using SdkManager.Core;
using System.Windows.Data;
using System.Globalization;

namespace SdkManager.UI
{
    [ValueConversion(typeof(string), typeof(string))]
    public class StringToColorConverter : IValueConverter
    {
        public static StringToColorConverter Instance = new StringToColorConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string c = "#BBBBBB";
            if (value != null)
            {
                var status = (string)value;

                /* could go on 
                 * [                                       ] // Red
                 * [=                                      ] 
                 * [==                                     ] // ...
                 * [===                                    ]
                 * [=====                                  ] // Yellow
                 * [========                               ]
                 * [===========                            ] // ...
                 * {=====================                  ]
                 * [=======================================] // Green
                 * 
                 * For example
                 */

                if (status.Contains("Downloading"))
                {
                    c = "Yellow";
                }
                if (status.Contains("Unzipping"))
                {
                    c = "Green";
                }
            }
            return c;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
