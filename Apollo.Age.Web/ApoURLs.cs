// --------------------------------------------------------------------------
// <copyright file="ApoURLs.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.5.13">Created</change>
// </history>

namespace Apollo.Age.Web
{
    using System.Configuration;

    /// <summary>
    /// Custom URLs for Apollo.Agent
    /// <example>http://haacked.com/archive/2007/03/11/custom-configuration-sections-in-3-easy-steps.aspx</example>
    /// </summary>
    public class ApoURLs : ConfigurationSection
    {
        private static ApoURLs settings = ConfigurationManager.GetSection("Apollo.Age.ApoURLs") as ApoURLs;

        public static ApoURLs URLs
        {
            get
            {
                if (settings == null)
                {
                    throw new ConfigurationErrorsException("Cannot find section Apollo.Age.ApoURLs");
                }

                return settings;
            }
        }

        [ConfigurationProperty("AccessDenied", IsRequired = true)]
        public string AccessDenied
        {
            get { return (string)this["AccessDenied"]; }
            set { this["AccessDenied"] = value; }
        }

        [ConfigurationProperty("AccountStopped", IsRequired = true)]
        public string AccountStopped
        {
            get { return (string)this["AccountStopped"]; }
            set { this["AccountStopped"] = value; }
        }

        [ConfigurationProperty("Error", IsRequired = true)]
        public string Error
        {
            get { return (string)this["Error"]; }
            set { this["Error"] = value; }
        }

        [ConfigurationProperty("InternalSignIn", IsRequired = true)]
        public string InternalSignIn
        {
            get { return (string)this["InternalSignIn"]; }
            set { this["InternalSignIn"] = value; }
        }

        [ConfigurationProperty("Unauthorized", IsRequired = true)]
        public string Unauthorized
        {
            get { return (string)this["Unauthorized"]; }
            set { this["Unauthorized"] = value; }
        }

        [ConfigurationProperty("UnderMaintenance", IsRequired = true)]
        public string UnderMaintenance
        {
            get { return (string)this["UnderMaintenance"]; }
            set { this["UnderMaintenance"] = value; }
        }

        [ConfigurationProperty("RedirectToUM", IsRequired = true)]
        public string RedirectToUM
        {
            get { return (string)this["RedirectToUM"]; }
            set { this["RedirectToUM"] = value; }
        }
    }
}