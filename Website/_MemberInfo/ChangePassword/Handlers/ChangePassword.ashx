<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.ChangePassword.ChangePassword_Handler" %>

using System;
using System.Web;

using Fanex.Age.UI;
using Sunplus.Agent.Model.Entities;

using Sunplus.Agent.Model.BL.Authorization;

/*
* Created 20100226@Terry, Lee - Handler for change password
* Revision ?@? - ...
*
*/

namespace Sunplus.Agent.UI._MemberInfo.ChangePassword
{
    public class ChangePassword_Handler : Fanex.Age.UI.TplHandler
    {
        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        string oldPwd, newPwd;
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            newPwd = Request["txtNewPwd"];
            oldPwd = Request["txtOldPwd"];
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!Validation.CheckValidPassword(newPwd) || string.IsNullOrEmpty(oldPwd) || CurrentUserSession.IsInternal)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            //AuthorizationBO bo = new AuthorizationBO(CurrentUserSession.NetSessionId);

            //int errCode = 0;
            //string errMsg = string.Empty;

            //errCode = bo.ChangePassword(newPwd, oldPwd, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), out errMsg);
            //return new Result(errCode, errMsg);
            return null;
        }
    }
}