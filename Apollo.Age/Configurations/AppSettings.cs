namespace Apollo.Age.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public static class AppSettings
    {
        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string UnderMaintenanceUrl
        {
            get
            {
                return GetValue("UnderMaintenaceUrl");
            }
        }

        public static bool IsDownTime
        {
            get
            {
                return GetValue("IsDownTime") == "1";
            }
        }

        public static string StartTime
        {
            get
            {
                return GetValue("StartTime");
            }
        }

        public static string EndTime
        {
            get
            {
                return GetValue("EndTime");
            }
        }
    }
}