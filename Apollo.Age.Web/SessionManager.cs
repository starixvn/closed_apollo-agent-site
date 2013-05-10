// --------------------------------------------------------------------------
// <copyright file="SessionManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.5.1">Created</change>
// </history>


namespace Apollo.Age.Web
{
    using System;
    using System.Web;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Core;

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/ff650316.aspx
    /// </summary>
    public static class SessionManager
    {
        private const string CacheKeySuncityLanguage = "Apollo.Age.Web.SuncityLanguage";
        private const string CacheKeySuncityUsername = "Apollo.Age.Web.SuncityUsername";

        //private static volatile SessionManager instance;
        //private static object syncRoot = new Object();

        //private SessionManager() { }

        //public static SessionManager Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            lock (syncRoot)
        //            {
        //                if (instance == null)
        //                {
        //                    instance = new SessionManager();
        //                }
        //            }
        //        }

        //        return instance;
        //    }
        //}

        public static ICurrentSession CurrentSession
        {
            get
            {
                return (ICurrentSession)HttpContext.Current.Session[Constants.APOLLO_CURRENT_SESSION_KEY];
            }
            set
            {
                HttpContext.Current.Session[Constants.APOLLO_CURRENT_SESSION_KEY] = value;
            }
        }

        public static void ClearAll()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
                HttpContext.Current.Session.Abandon();
                HttpContext.Current.Response.Cookies.Clear();
            }
        }
    }
}