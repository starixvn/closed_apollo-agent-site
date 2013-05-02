// --------------------------------------------------------------------------
// <copyright file="ControllerBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.27">Created</change>
//   <change who="Anders" date="2013.4.6">Add CurrentSession Property</change>
//   <change who="Lewis" date="2013.4.10">Added RequestIP property</change>
//   <change who="Anders.Le" date="2013.5.2">Modify Current Session, change it to readonly</change>
// </history>

namespace Apollo.Age.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common.Extensions;
    using System;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.IsAuthenticated || CurrentSession == null)
            { 
                // redirect user to a page with corresponding message
                // your session is timeout or the request is not authenticated

                if (Request.IsAjaxRequest())
                {

                }
                else
                { 
                    
                }
            }

            base.OnActionExecuting(filterContext);
        }

        protected ICurrentSession CurrentSession
        {
            get { return SessionManager.Instance.CurrentSession; }            
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

                if (!string.IsNullOrWhiteSpace(IP))
                {
                    return IP;
                }

                foreach (IPAddress itemIPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (itemIPA.AddressFamily.ToString().Equals("InterNetwork", StringComparison.OrdinalIgnoreCase))
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