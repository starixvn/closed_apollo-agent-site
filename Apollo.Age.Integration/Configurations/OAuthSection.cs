// --------------------------------------------------------------------------
// <copyright file="OAuthSection.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.26">Created</change>
// </history>

namespace Apollo.Age.Integration.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Configuration;

    /// <summary>
    /// Define OAuth section
    /// <example>http://msdn.microsoft.com/en-us/library/system.configuration.configurationsection%28v=vs.100%29.aspx</example>
    /// </summary>
    public class OAuthSection : ConfigurationSection
    {        
        private const string UrlPattern = @"/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/";

        private static ConfigurationPropertyCollection _Properties;

        private static readonly ConfigurationProperty _AppId = new ConfigurationProperty(ConfigName.AppId,
            typeof(string),
            "sdyhrtdbHf73FdewK9",
            ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty _AppSecret = new ConfigurationProperty(ConfigName.AppSecret,
          typeof(string),
          "hryfheb7Bjdwd9253Ndyu2NbBhd",
          ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty _AuthorizeUrl = new ConfigurationProperty(ConfigName.AuthorizeUrl,
         typeof(string),
         @"http://oauth.acsdev.net/scoauth/oauth/authorize",
         ConfigurationPropertyOptions.IsRequired);

        private static readonly ConfigurationProperty _TokenUrl = new ConfigurationProperty(ConfigName.TokenUrl,
         typeof(string),
         @"http://oauth.acsdev.net/scoauth/oauth/token",
         ConfigurationPropertyOptions.IsRequired);

        public OAuthSection()
        {
            _Properties = new ConfigurationPropertyCollection();

            _Properties.Add(_AppId);
            _Properties.Add(_AppSecret);
            _Properties.Add(_AuthorizeUrl);
            _Properties.Add(_TokenUrl);
        }

        public string AppId
        {
            get { return (string)this[ConfigName.AppId]; }
            set { this[ConfigName.AppId] = value; }
        }

        public string AppSecret
        {
            get { return (string)this[ConfigName.AppSecret]; }
            set { this[ConfigName.AppSecret] = value; }
        }

        [RegexStringValidator(UrlPattern)]
        public string AuthorizeUrl
        {
            get { return (string)this[ConfigName.AuthorizeUrl]; }
            set { this[ConfigName.AuthorizeUrl] = value; }
        }

        [RegexStringValidator(UrlPattern)]
        public string TokenUrl
        {
            get { return (string)this[ConfigName.TokenUrl]; }
            set { this[ConfigName.TokenUrl] = value; }
        }

        public string RedirectUri
        {
            get { return (string)this[ConfigName.RedirectUri]; }
            set { this[ConfigName.RedirectUri] = value; }
        }
    }
}
