/*
 * Created 20091229@Lee - User session entity, for UI using. This file is temporary to support old functions. Please use TplPage.CurrentUserSession in each page.
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.Entities
{
    using System;
    using System.Runtime.Serialization;
    using System.Web;
    using Sunplus.Agent.Model.BL;

    [Serializable]
    public class UserSession : UserSessionBase
    {
        public UserSession(string netSessionId) : base(netSessionId) 
        {
        }

        public UserSession(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { 
        }

        public static IUserSession Current
        {
            get 
            {
                return UserSessionService.GetUserSessionInstance(HttpContext.Current.Session.SessionID);
            }
        }
    }
}