using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTS_SDK_Manager   
{
    /// <summary>
    /// Static class containing all necessary regular expressions. Useful to parse sdkmanger.bat -- list --verbose output.
    /// </summary>
    public static class Patterns
    {
        #region Maybe do this later
        const string _APILEVEL = @"(?<APILevel>[\d]*)(?:[\s\w\-\;]*\n))";
        const string _CAPTURE_DESCRIPTION = @"(?:[\s]*Description[:][\s]*)(?<Description>[\s\w]*\n)";
        const string _CAPTURE_VERSION = @"(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d]*)";
        const string _CAPTURE_LOCATION = @"(?:(?:[\s]*Installed Location[:][\s]*)(?<Installed_Location>[\s]*[\w\:\\\-]*))?";
        const string _platform_pattern = @"(?<Platform>(?:platforms;android-)";

        // This works just fine.
        public const string TestString = _platform_pattern + _APILEVEL + _CAPTURE_DESCRIPTION + _CAPTURE_VERSION + _CAPTURE_LOCATION;
        #endregion

        #region unused
        public const string INSTALLED_BODY_PATTERN = @"(?:^Installed packages:)(?<Installed_Body>(?:.*\n)*)(?:[\s]*\nAvailable Packages:|\n$)";
        public const string AVAILABLE_BODY_PATTERN = @"(?:^Available Packages:)(?<Available_Body>(?:.*\n)*)(?:Available Updates:)";
        public const string UPDATEABLE_BODY_PATTERN = @"(?:^Available Updates:)(?<Available_Body>(?:.*\n)*)";
        #endregion

        #region Platform items
        /// <summary>
        /// Expression to match the pattern of all high-level platforms.
        /// </summary>
        public const string PLATFORM_PATTERN =      @"(?<Platform>(?:platforms;android-)(?<APILevel>[\d]*))(?:[\s]*Description[:][\s]*)(?<Description>Android[\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d]*)(?:(?:[\s]*Installed Location[:][\s]*)(?<Installed_Location>[\s]*[\w\:\\\-]*))?";
        /// <summary>
        /// Expression to match the pattern of all google api packages.
        /// </summary>
        public const string GOOGLE_APIS =           @"(?<Platform>(?:add-ons;addon-google_apis-google-)(?<APILevel>[\d]*))(?:[\s\w]*[:][\s]*)(?<Description>Google[\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d]*)(?:(?:[\s]*Installed Location[:][\s]*)(?<Installed_Location>[\s]*[\w\:\\\-]*))?";
        /// <summary>
        /// Expression to match the pattern of all platform sources.
        /// </summary>
        public const string SOURCES_PATTERN =       @"(?<Platform>(?:sources;android-)(?<APILevel>[\d]*))(?:[\s\w]*[:][\s]*)(?<Description>Sources[\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d]*)(?:(?:[\s]*Installed Location[:][\s]*)(?<Installed_Location>[\s]*[\w\:\\\-]*))?";
        /// <summary>
        /// Expression to match the pattern of all system images.
        /// </summary>
        public const string SYSTEM_IMAGES_PATTERN = @"(?<Platform>(?:system-images;android-)(?<APILevel>[\d]*)[\s\w\-\;]*\n)(?:[\s]*Description[:][\s]*)(?<Description>[\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d]*)(?:(?:[\s]*Installed Location[:][\s]*)(?<Installed_Location>[\s]*[\w\:\\\-]*))?";
        /// <summary>
        /// Expression to match the pattern of google glass packages.
        /// </summary>
        public const string GOOGLE_GLASS_PATTERN =  @"(?<Platform>(?:add-ons;addon-google_gdk-google-)(?<APILevel>[\d]*))(?:[\s\w]*[:][\s]*)(?<Description>Glass[\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d]*)(?:(?:[\s]*Installed Location[:][\s]*)(?<Installed_Location>[\s]*[\w\:\\\-]*))?";
        
        // This requires a different method to parse.
        /// <summary>
        /// Expression to match the pattern of all updatable platforms.
        /// </summary>
        public const string UPDATEABLE_PACKAGES_PATTERN = @"(?<Platform>^[a-zA-Z][\w\s-\;]*\n)(?:[\s]*)(?:Installed Version[:][\s]*)(?<Installed_Version>[\d.]*)(?:[\s]*)(?:Available Version[:][\s*])(?<Available_Version>[\d.]*)";

        #endregion

        #region SDK Tools

        public const string BUILD_TOOLS_PATTERN = @"(?<Platform>(?:build-tools;)(?<APILevel>[\d]*[.][\d]*[.][\d]*[-\w]*))(?:[\s\w]*[:][\s]*)(?<Description>Android[\s\w-.]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d]*[.][\d]*[.][\d]*[\s][\w]*)(?:(?:[\s]*Installed Location[:][\s]*)(?<Installed_Location>[\s]*[\w\:\\\-]*[\d.]*))?"; // Need to get the laterst version
        public const string GPU_DEBUGGING_TOOLS_PATTERN = @"(?<Platform>(?:extras;android;gapid;)(?<APILevel>[\d]*))(?:[\s\w]*[:][\s]*)(?<Description>GPU[\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d]*.[\d]*.[\d]*.)";
        public const string LLDB_PATTERN = @"(?<Platform>(?:lldb;)(?<APILevel>[.\d]*))(?:[\s\w]*[:][\s]*)(?<Description>LLDB[.\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[.\d]*)";
        public const string ANDROID_AUTO_API_SIM_PATTERN = @"(?<Platform>(?:extras;google;simulators))(?:[\s\w]*[:][\s]*)(?<Description>Android[-\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string ANDROID_AUTO_EMULATOR_PATTERN = @"(?<Platform>(?:extras;google;auto))(?:[\s\w]*[:][\s]*)(?<Description>Android[-\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string EMULATOR_PATTERN = @"(?<Platform>(?:emulator))(?:[\s\w]*[:][\s]*)(?<Description>Android[\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d]*.[\d]*.[\d]*)"; // NO API LEVEL
        public const string PLATFORM_TOOLS_PATTERN = @"(?<Platform>(?:platform-tools))(?:[\s]*Description[:][\s]*)(?<Description>Android[-\s\w]*\n)(?:[\s]*[\s]*Version[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string SKD_TOOLS_PATTERN = @"(?<Platform>(?:tools))(?:[\s\w]*[:][\s]*)(?<Description>Android[-\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string DOCS_PATTERN = @"(?<Platform>(?:docs))(?:[\s\w]*[:][\s]*)(?<Description>Documentation[\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d])"; // NO API LEVEL
        public const string GOOGLE_APK_EXT_LIBRARY_PATTERN = @"(?<Platform>(?:extras;google;market_apk_expansion))(?:[\s\w]*[:][\s]*)(?<Description>Google[-\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string GOOLGE_PLAY_INSTANT_SDK_PATTERN = @"(?<Platform>(?:extras;google;instantapps))(?:[\s\w]*[:][\s]*)(?<Description>Google[-\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string GOOGLE_PLAY_LICENSE_LIBRARY_PATTERN = @"(?<Platform>(?:extras;google;market_licensing))(?:[\s\w]*[:][\s]*)(?<Description>Google[-\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string GOOGLE_PLAY_SERVICES_PATTERN = @"(?<Platform>(?:extras;google;google_play_services))(?:[\s\w]*[:][\s]*)(?<Description>Google[-\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string GOOGLE_USB_DRIVER_PATTERN = @"(?<Platform>(?:extras;google;usb_driver))(?:[\s\w]*[:][\s]*)(?<Description>Google[-\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string GOOGLE_WEB_DRIVER_PATTERN = @"(?<Platform>(?:extras;google;webdriver))(?:[\s\w]*[:][\s]*)(?<Description>Google[-\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string INTEL_86_EMULATOR_PATTERN = @"(?<Platform>(?:extras;intel;Hardware_Accelerated_Execution_Manager))(?:[\s\w]*[:][\s]*)(?<Description>Intel[\(\)\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string NDK_PATTERN = @"(?<Platform>(?:ndk-bundle)(?<APILevel>[.\d]*))(?:[\s\w]*[:][\s]*)(?<Description>NDK[\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[.\d]*)";

        #endregion

        #region Google Support

        public const string CONSTRAINT_LAYOUT_PATTERN = @"(?<Platform>(?:extras;m2repository;com;android;support;constraint;constraint-layout;)(?<APILevel>[-\w.]*))(?:[\s\w]*[:][\s]*)(?<Description>ConstraintLayout[-.\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)";
        public const string CONSTRAINT_LAYOUT_CHILDREN_PATTERN = @"(?<Platform>(?:extras;m2repository;com;android;support;constraint;constraint-layout;)(?<APILevel>[-\w.]*))(?:[\s\w]*[:][\s]*)(?<Description>com[-.\w\:]*[\d]$\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)";
        public const string SOLVER_CONSTRAINT_LAYOUT_PATTERN = @"(?<Platform>(?:extras;m2repository;com;android;support;constraint;constraint-layout-solver;)(?<APILevel>[-\w.]*))(?:[\s\w]*[:][\s]*)(?<Description>Solver[-.\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)";
        public const string SOLVER_CONSTRAINT_LAYOUT_CHILDREN_PATTERN = @"(?<Platform>(?:extras;m2repository;com;android;support;constraint;constraint-layout-solver;)(?<APILevel>[-\w.]*))(?:[\s\w]*[:][\s]*)(?<Description>com[;\\:\-.\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)";
        public const string ANDROID_SUPPORT_PATTERN = @"(?<Platform>(?:extras;android;m2repository))(?:[\s\w]*[:][\s]*)(?<Description>Android[-\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL
        public const string GOOGLE_REPOSITORY_PATTERN = @"(?<Platform>(?:extras;google;m2repository))(?:[\s\w]*[:][\s]*)(?<Description>Google[-\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[\d.]*)"; // NO API LEVEL

        public const string SDK_PATHCER_PATTERN = @"(?<Platform>(?:patcher;v)(?<APILevel>[\d]*))(?:[\s\w]*[:][\s]*)(?<Description>SDK[\s\w]*\n)(?:[\s]*[\w\s]*[:][\s]*)(?<Version>[.\d]*)";

        #endregion
    }
}
