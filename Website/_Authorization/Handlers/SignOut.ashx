<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._Authorization.SignOut_Handler" %>

using System;
using System.Web;
using System.Web.Security;

//using Sunplus.Agent.Model.BL.Authorization;

/*
 * Created 20100126@Lee - Sign-out handler. Clear all session keys, cookies, check-out On_User database server.
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._Authorization
{
    public class SignOut_Handler : Fanex.Age.UI.TplHandler
    {
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            // No requested parameters
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            // Check-out

            //AuthorizationBO bo = new AuthorizationBO(CurrentUserSession.NetSessionId);
            //bo.SignOut();

            return null;
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Session.Abandon();
            Response.Cookies.Clear();
            // Clear session ticket
            FormsAuthentication.SignOut(); // Use this command will make the logout function log out all loged in users in the same machine and browser
            //           FormsAuthentication.RedirectToLoginPage();
            Response.Redirect(FormsAuthentication.LoginUrl);

        }
    }
}