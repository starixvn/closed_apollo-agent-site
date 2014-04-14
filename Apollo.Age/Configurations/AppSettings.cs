// --------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.12">Created</change>
//   <change who="Anders.Le" date="2013.6.12">Change IsMultipleSignOn to IsAllowMultipleSignIn</change>
// </history>

namespace Apollo.Age.Configurations
{
    using System;
    using System.Configuration;
    using Fanex.Common;
    using Fanex.Logging;

    /// <summary>
    /// Provide static accessing function for setting key from app setting.config.
    /// </summary>
    public static class AppSettings
    {
        public static string UnderMaintenanceUrl
        {
            get
            {
                return GetValue("UnderMaintenaceUrl");
            }
        }

        public static bool IsApolloDownTime
        {
            get
            {
                return GetValue("IsApolloDownTime") == "1";
            }
        }

        public static string ApolloDownTime
        {
            get
            {
                return GetValue("ApolloDownTime");
            }
        }

        public static string StartTime
        {
            get
            {
                return ApolloDownTime.Split("-".ToCharArray())[0];
            }
        }

        public static string EndTime
        {
            get
            {
                return ApolloDownTime.Split("-".ToCharArray())[1];
            }
        }

        public static bool AllowInternalUpdate
        {
            get
            {
                return GetValue("AllowInternalUpdate") == "1";
            }
        }

        public static bool AllowMultipleSignIn
        {
            get
            {
                return GetValue("AllowMultipleSignIn") == "1";
            }
        }

        public static string SuperSafeCode
        {
            get
            {
                return GetValue("SuperSafeCode");
            }
        }

        public static bool DisabledUnderMaintenanceAPI
        {
            get
            {
                return GetValue("DisabledUnderMaintenanceAPI") == "1";
            }
        }

        public static DateTime ResultDateConfig
        {
            get
            {
                var resultDate = GetValue("ResultDateConfig");
                return Converter.ToDateTime(resultDate, DateTime.Now);
            }
        }

        public static int CurrencyTesting
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(GetValue("CurrencyTesting")))
                {
                    return Convert.ToInt32(GetValue("CurrencyTesting"));
                }

                return -1;
            }
        }

        public static bool DisableEditOnMember
        {
            get
            {
                return GetValue("DisableEditOnMember") == "1";
            }
        }

        public static int ShowEstimateTimeWhenGreaterThan
        {
            get
            {
                return Converter.ToInt(GetValue("ShowEstimateTimeWhenGreaterThan"));
            }
        }

        public static string PeakHours
        {
            get
            {
                return GetValue("PeakHours");
            }
        }

        public static DateTime PeriodDateRange
        {
            get
            {
                string periodDate = GetValue("PeriodDateRange");
                if (string.IsNullOrWhiteSpace(periodDate))
                {
                    return DateTime.Now;
                }

                return Convert.ToDateTime(GetValue("PeriodDateRange"));
            }
        }

        public static int MessageSiteId
        {
            get
            {
                return Convert.ToInt32(GetValue("MessageSiteId"));
            }
        }

        public static int SSOSiteId
        {
            get
            {
                return Convert.ToInt32(GetValue("SSOSiteId"));
            }
        }

        public static int SSOTimeOut
        {
            get
            {
                return Convert.ToInt32(GetValue("SSOTimeout"));
            }
        }

        public static string ReportPeakHour
        {
            get
            {
                return GetValue("HistoryReportPeakHours");
            }
        }

        /// <summary>
        /// Gets value from a key.
        /// </summary>
        /// <param name="key">Input a key.</param>
        /// <returns>Return value.</returns>
        public static string GetValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                return string.Empty;
            }
        }
    }
}