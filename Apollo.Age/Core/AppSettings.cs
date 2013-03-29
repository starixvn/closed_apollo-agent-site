// --------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.29">Created</change>
// </history>


namespace Apollo.Age.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Configuration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettings
    {
        static AppSettings()
        {
            IsLocal = ConfigurationManager.AppSettings["IsLocal"] == "1";
        }

        public static bool IsLocal { get; private set; } 
    }
}
