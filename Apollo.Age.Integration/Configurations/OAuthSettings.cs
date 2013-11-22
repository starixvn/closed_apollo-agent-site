// --------------------------------------------------------------------------
// <copyright file="OAuthSettings.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.26">Created</change>
//   <change who="Anders.Le" date="2013.5.9">Add default url</change>
// </history>

namespace Apollo.Age.Integration.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Define open authentication section
    /// <see href="http://haacked.com/archive/2007/03/11/custom-configuration-sections-in-3-easy-steps.aspx"/>
    /// </summary>
    public class OAuthSettings : ConfigurationSection
    {
        private static OAuthSettings settings = ConfigurationManager.GetSection("Apollo.Age.OAuthSettings") as OAuthSettings;

        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public static OAuthSettings Settings
        {
            get
            {
                return settings;
            }
        }

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        [ConfigurationProperty("AppId", IsRequired = true)]
        public string AppId
        {
            get { return (string)this["AppId"]; }
            set { this["AppId"] = value; }
        }

        /// <summary>
        /// Gets or sets the application secret.
        /// </summary>
        /// <value>
        /// The application secret.
        /// </value>
        [ConfigurationProperty("AppSecret", IsRequired = true)]
        public string AppSecret
        {
            get { return (string)this["AppSecret"]; }
            set { this["AppSecret"] = value; }
        }

        /// <summary>
        /// Gets or sets Authorize endpoint.
        /// </summary>
        /// <value>
        /// The authorize URL.
        /// </value>
        [ConfigurationProperty("AuthorizeUrl", IsRequired = true)]
        public string AuthorizeUrl
        {
            get { return (string)this["AuthorizeUrl"]; }
            set { this["AuthorizeUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the url client submit to in order to get authentication token.
        /// </summary>
        /// <value>
        /// The token URL.
        /// </value>
        [ConfigurationProperty("TokenUrl", IsRequired = true)]
        public string TokenUrl
        {
            get { return (string)this["TokenUrl"]; }
            set { this["TokenUrl"] = value; }
        }

        /// <summary>
        /// Gets or sets the redirect url required by authorization server.
        /// </summary>
        /// <value>
        /// The redirect URI.
        /// </value>
        [ConfigurationProperty("RedirectUri", IsRequired = true)]
        public string RedirectUri
        {
            get { return (string)this["RedirectUri"]; }
            set { this["RedirectUri"] = value; }
        }

        /// <summary>
        /// Gets or sets path to TGP script which handle the initial step of single sign on.
        /// </summary>
        /// <value>
        /// The TGP script source.
        /// </value>
        [ConfigurationProperty("TgpScriptSrc", IsRequired = true)]
        public string TgpScriptSrc
        {
            get { return (string)this["TgpScriptSrc"]; }
            set { this["TgpScriptSrc"] = value; }
        }

        /// <summary>
        /// Gets or sets the default URL.
        /// </summary>
        /// <value>
        /// The default URL.
        /// </value>
        [ConfigurationProperty("DefaultUrl", IsRequired = true)]
        public string DefaultUrl
        {
            get { return (string)this["DefaultUrl"]; }
            set { this["DefaultUrl"] = value; }
        }
    }
}