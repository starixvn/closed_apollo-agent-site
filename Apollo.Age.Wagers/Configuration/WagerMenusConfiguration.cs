// --------------------------------------------------------------------------
// <copyright file="WagerMenusConfiguration.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.29">Created</change>
// </history>

namespace Apollo.Age.Wagers.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using System.Configuration;
    using Fanex.Logging;
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class WagerMenusConfiguration
    {
        /// <summary>
        /// Constructor, for Lazy loading
        /// </summary>
        static WagerMenusConfiguration()
        {
            try
            {
                var strPath = ConfigurationManager.AppSettings["WagerMenuConfig"].ToString();
                MenuConfig = XDocument.Load(HttpContext.Current.Server.MapPath(strPath));
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
                throw new Exception("Could not load Wagers Menu configruation file.", ex);
            }
        }

        public static XDocument MenuConfig { get; set; }
    }
}
