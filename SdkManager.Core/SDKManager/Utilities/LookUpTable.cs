using System.Collections.Generic;

namespace SdkManager.Core
{
    /// <summary>
    /// static class for converting from standard description to alias description.
    /// </summary>
    public static class LookUpTable
    {
        /// <summary>
        /// Convert from default platform desction to the alias name of a platform.
        /// <para>Android SDK Platform 7 => Android 2.1 (Eclair)</para>
        /// </summary>
        private static Dictionary<string, string> Aliases = new Dictionary<string, string>
        {
            { "Android SDK Platform 7", "Android 2.1 (Eclair)"},
            { "Android SDK Platform 8", "Android 2.2 (Froyo)"},
            { "Android SDK Platform 9", "Android 2.3 (Gingerbread)"},
            { "Android SDK Platform 10", "Android 2.3.3 (Gingerbread)"},
            { "Android SDK Platform 11", "Android 3.0 (Honeycomb)"},
            { "Android SDK Platform 12", "Android 3.1 (Honeycomb)"},
            { "Android SDK Platform 13", "Android 3.2 (Honeycomb)"},
            { "Android SDK Platform 14", "Android 4.0 (IceCreamsandwich)"},
            { "Android SDK Platform 15", "Android 4.0.3 (IceCreamsandwich)"},
            { "Android SDK Platform 16", "Android 4.1 (Jelly Bean)"},
            { "Android SDK Platform 17", "Android 4.2 (Jelly Bean)"},
            { "Android SDK Platform 18", "Android 4.3 (Jelly Bean)"},
            { "Android SDK Platform 19", "Android 4.4 (KitKat)"},
            { "Android SDK Platform 20", "Android 4.4W (KitKat Wear)"},
            { "Android SDK Platform 21", "Android 5.0 (Lillipop)"},
            { "Android SDK Platform 22", "Android 5.1 (Lillipop)"},
            { "Android SDK Platform 23", "Android 6.0 (Marshmallow)"},
            { "Android SDK Platform 24", "Android 7.0 (Nougat)"},
            { "Android SDK Platform 25", "Android 7.1.1 (Nougat)"},
            { "Android SDK Platform 26", "Android 8.0 (Oreo)"},
            { "Android SDK Platform 27", "Android 8.1 (Oreo)"},
            { "Android SDK Platform 28", "Android 28 (Pie)"},
            { "Android SDK Platform Q", "Android SDK Platform Q"},
            { "android-28", "Android SDK Platform 28"},
        };

        /// <summary>
        /// Will return an alternate version of a Platform name if found, else return the platform name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetDescription(string name)
        {
            string r = name;
            Aliases.TryGetValue(name, out r);
            if(string.IsNullOrEmpty(r))
            {
                r = name;
            }
            return r;
        }
    }
}
