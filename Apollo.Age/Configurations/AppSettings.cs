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
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

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

        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key].ToLower();
        }
    }
}