using System.IO;
using System.Windows.Controls;

namespace SdkManager.UI
{
    public class FileExistsValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (File.Exists((string)value + @"\tools\bin\sdkmanager.bat"))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Path is not a valid path to the android sdk.");
            }
        }
    }
}
