using System;
using SdkManager.Core;
using System.Windows.Data;
using System.Globalization;
using System.Linq;

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
                int count = status.Count(f => f == '=');

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

                if(count <= 3)
                {
                    c = "#FF7D0B";
                }
                else if(count <= 7)
                {
                    c = "#FFA60B";
                }
                else if(count <= 11)
                {
                    c = "#FFD70B";
                }
                else if(count <= 15)
                {
                    c = "#B6FF0B";
                }
                else if(count <= 19)
                {
                    c = "#6DFF0B";
                }
                else if(count <= 40)
                {
                    c = "#44FF0B";
                }

                //if (status.Contains("Downloading"))
                //{
                //    c = "Yellow";
                //}
                //if (status.Contains("Unzipping"))
                //{
                //    c = "Green";
                //}
            }
            return c;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
