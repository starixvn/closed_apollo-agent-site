// --------------------------------------------------------------------------
// <copyright file="DeviceDetection.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry" date="2013.05.17">Created</change>
//
// </history>

namespace Apollo.Age.Web
{
    using System;
    using System.Net;
    using System.Web;

    /// <summary>
    /// Device detection class.
    /// </summary>
    public class DeviceDetection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceDetection"/> class.
        /// </summary>
        public DeviceDetection()
        {
            var request = HttpContext.Current.Request;

            DomainName = request.ServerVariables["SERVER_NAME"] != null ? request.ServerVariables["SERVER_NAME"].ToString() : string.Empty;
            if (request.Browser.IsMobileDevice || request.UserAgent.ToLower().Contains("android"))
            {
                BrowserName = request.Browser.Browser + " " + request.Browser.MajorVersion;
                Platform = request.UserAgent.ToLower().Contains("android") ? "andorid" : request.Browser.MobileDeviceModel;
            }
            else
            {
                BrowserName = request.Browser.Browser + " " + request.Browser.Version;
                Platform = request.Browser.Platform;
            }

            CountryCode = string.Empty;
            ClientIp = RequestIP();
        }
        
        public string DomainName { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public string ClientIp { get; set; }

        /// <summary>
        /// Requests the IP.
        /// </summary>
        /// <returns>String IP.</returns>
        protected string RequestIP()
        {
            string ip = string.Empty;
            string userHostAddress = HttpContext.Current.Request.UserHostAddress;

            foreach (IPAddress itemIPA in Dns.GetHostAddresses(userHostAddress))
            {
                if (itemIPA.AddressFamily.ToString().Equals("InterNetwork"))
                {
                    ip = itemIPA.ToString();
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(ip))
            {
                return ip;
            }

            foreach (IPAddress itemIPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (itemIPA.AddressFamily.ToString().Equals("InterNetwork", StringComparison.OrdinalIgnoreCase))
                {
                    ip = itemIPA.ToString();
                    break;
                }
            }

            return ip.Trim();
        }
    }
}