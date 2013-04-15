// --------------------------------------------------------------------------
// <copyright file="ControllerBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
//   <change who="Anders" date="2013.4.6">Add CurrentSession Property</change>
//   <change who="Lewis" date="2013.4.10">Added RequestIP property</change>
// </history>

namespace Apollo.Age.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common.Extensions;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class BaseController : Controller
    {
        protected ICurrentSession CurrentSession
        {
            get
            {
                ICurrentSession currentSession = (ICurrentSession)Session["Apollo.Age.Web.CurrentSession"];

                return currentSession;
            }
            set
            {
                if (value is ICurrentSession)
                {
                    Session["Apollo.Age.Web.CurrentSession"] = value;
                }
            }
        }

        protected string RequestIP
        {
            get
            {
                string IP = string.Empty;
                string userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;

                foreach (IPAddress itemIPA in Dns.GetHostAddresses(userHostAddress))
                {
                    if (itemIPA.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        IP = itemIPA.ToString();
                        break;
                    }
                }

                if (!IP.IsNullOrTrimEmpty())
                {
                    return IP;
                }

                foreach (IPAddress itemIPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (itemIPA.AddressFamily.ToString().Equals("InterNetwork"))
                    {
                        IP = itemIPA.ToString();
                        break;
                    }
                }

                return IP.Trim();
            }
        }

        protected string GetResourceData( string key)
        {
            return System.Web.HttpContext.GetGlobalResourceObject("Lang", key) as string;
        }
    }
}