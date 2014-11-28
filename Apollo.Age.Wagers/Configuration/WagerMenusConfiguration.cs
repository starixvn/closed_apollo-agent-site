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
    using System.Configuration;
    using System.Web;
    using System.Xml.Linq;

    /// <summary>
    /// Wager menu configuration.
    /// </summary>
    public static class WagerMenusConfiguration
    {
        /// <summary>
        /// Initializes static members of the <see cref="WagerMenusConfiguration" /> class.
        /// </summary>
        static WagerMenusConfiguration()
        {
            var strPath = ConfigurationManager.AppSettings["WagerMenuConfig"].ToString();
            MenuConfig = XDocument.Load(HttpContext.Current.Server.MapPath(strPath));
        }

        public static XDocument MenuConfig { get; set; }
    }
}