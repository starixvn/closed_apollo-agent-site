// --------------------------------------------------------------------------
// <copyright file="OAuthSection.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.26">Created</change>
//   <change who="Anders.Le" date="2013.5.9">Add default url</change>
// </history>

namespace Apollo.Age.Integration.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Configuration;

    /// <summary>
    /// Define open authentication section
    /// <example>http://haacked.com/archive/2007/03/11/custom-configuration-sections-in-3-easy-steps.aspx</example>
    /// </summary>
    public class OAuthSettings : ConfigurationSection
    {
        //private const string UrlPattern = @"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$";
        private static OAuthSettings settings = ConfigurationManager.GetSection("Apollo.Age.OAuthSettings") as OAuthSettings;

        public static OAuthSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    throw new ConfigurationErrorsException("Cannot find section Apollo.Age.OAuthSettings");
                }

                return settings;
            }
        }        

        [ConfigurationProperty("AppId", IsRequired = true)]
        public string AppId
        {
            get { return (string)this["AppId"]; }
            set { this["AppId"] = value; }
        }

        [ConfigurationProperty("AppSecret", IsRequired = true)]
        public string AppSecret
        {
            get { return (string)this["AppSecret"]; }
            set { this["AppSecret"] = value; }
        }

        /// <summary>
        /// Authorize endpoint
        /// </summary>
        [ConfigurationProperty("AuthorizeUrl", IsRequired = true)]
        //[RegexStringValidator(UrlPattern)]
        public string AuthorizeUrl
        {
            get { return (string)this["AuthorizeUrl"]; }
            set { this["AuthorizeUrl"] = value; }
        }

        /// <summary>
        /// The url client submit to in order to get authentication token
        /// </summary>
        [ConfigurationProperty("TokenUrl", IsRequired = true)]
        public string TokenUrl
        {
            get { return (string)this["TokenUrl"]; }
            set { this["TokenUrl"] = value; }
        }

        /// <summary>
        /// The redirect url required by authorization server
        /// </summary>
        [ConfigurationProperty("RedirectUri", IsRequired = true)]
        public string RedirectUri
        {
            get { return (string)this["RedirectUri"]; }
            set { this["RedirectUri"] = value; }
        }

        /// <summary>
        /// Path to TGP script which handle the initial step of single sign on
        /// </summary>
        [ConfigurationProperty("TgpScriptSrc", IsRequired = true)]
        public string TgpScriptSrc
        {
            get { return (string)this["TgpScriptSrc"]; }
            set { this["TgpScriptSrc"] = value; }
        }

        /// <summary>
        /// The url user will be redirected to after single sign on sucessfull login
        /// </summary>
        [ConfigurationProperty("DefaultUrl", IsRequired = true)]
        public string DefaultUrl
        {
            get { return (string)this["DefaultUrl"]; }
            set { this["DefaultUrl"] = value; }
        }
    }
}
