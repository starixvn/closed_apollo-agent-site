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
                return (ICurrentSession)HttpContext.Current.Session["Apollo.Age.Web.CurrentSession"];
            }
            set
            {
                HttpContext.Current.Session["Apollo.Age.Web.CurrentSession"] = value;
            }
        }
    }
}
