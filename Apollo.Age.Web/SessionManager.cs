// --------------------------------------------------------------------------
// <copyright file="SessionManager.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.5.1">Created</change>
//   <change who="Anders.Le" date="2013.5.10">Change to static class</change>
// </history>


namespace Apollo.Age.Web
{
    using System.Web;

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/ff650316.aspx
    /// </summary>
    public static class SessionManager
    {        

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
<<<<<<< HEAD
                return (ICurrentSession)HttpContext.Current.Session[Constants.ApolloCurrentSessionKey];
            }
            set
            {
                HttpContext.Current.Session[Constants.ApolloCurrentSessionKey] = value;
=======
                return (ICurrentSession)HttpContext.Current.Session[WebConstants.APOLLO_CURRENT_SESSION_KEY];
            }
            set
            {
                HttpContext.Current.Session[WebConstants.APOLLO_CURRENT_SESSION_KEY] = value;
>>>>>>> 13e5a07cb18cbbf93c0dda834ae10a0eb54c0301
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