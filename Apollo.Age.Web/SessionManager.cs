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

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/ff650316.aspx
    /// </summary>
    public sealed class SessionManager
    {
        private const string CacheKeyCurrentSession = "Apollo.Age.Web.CurrentSession";
        private const string CacheKeySuncityLanguage = "Apollo.Age.Web.SuncityLanguage";
        private const string CacheKeySuncityUsername = "Apollo.Age.Web.SuncityUsername";


        private static volatile SessionManager instance;
        private static object syncRoot = new Object();

        private SessionManager() { }

        public static SessionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new SessionManager();
                        }
                    }
                }

                return instance;
            }
        }

        public ICurrentSession CurrentSession
        {
            get
            {
                return (ICurrentSession)HttpContext.Current.Session[CacheKeyCurrentSession];
            }
            set
            {
                HttpContext.Current.Session[CacheKeyCurrentSession] = value;
            }
        }

        public string SuncityLanguage
        {
            get
            {
                return (string)HttpContext.Current.Session[CacheKeySuncityLanguage];
            }
            set
            {
                HttpContext.Current.Session[CacheKeySuncityLanguage] = value;
            }
        }

        public string SuncityUsername
        {
            get
            {
                return (string)HttpContext.Current.Session[CacheKeySuncityUsername];
            }
            set
            {
                HttpContext.Current.Session[CacheKeySuncityUsername] = value;
            }
        }
    }
}
