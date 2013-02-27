<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Master.CopyAgent_Handler" %>

using System;
using System.Web;
using System.Data;

using Fanex.Age.UI;
using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.BL.MemberInfo.SearchAndCopy;

/*
 * Created 20120224@Jeff - Copy user
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._MemberInfo.SearchAndCopy.Handlers.Master
{
    public class CopyAgent_Handler : TplHandler, System.Web.SessionState.IReadOnlySessionState
    {

        string userName = string.Empty;
        string userNameDetail = string.Empty;
        string password = string.Empty;
        string firstName = string.Empty;
        string lastName = string.Empty;
        string phone = string.Empty;
        string mobilePhone = string.Empty;
        string fax = string.Empty;
        float credit = 0;
        float maxCredit = 0;
        int custId = 0;
        int custIdExist = 0;
        string errMsg = string.Empty;
        int errCode = 0;

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            userNameDetail = Request.Form["userName"];
            password = HttpUtility.HtmlDecode(Request.Form["password"]);
            firstName = HttpUtility.HtmlDecode(Request.Form["firstName"]);
            lastName = HttpUtility.HtmlDecode(Request.Form["lastName"]);
            phone = HttpUtility.HtmlDecode(Request.Form["phone"]);
            mobilePhone = HttpUtility.HtmlDecode(Request.Form["mobilePhone"]);
            fax = HttpUtility.HtmlDecode(Request.Form["fax"]);
            float.TryParse(Request.Form["credit"], out credit);
            float.TryParse(Request.Form["maxCredit"], out maxCredit);
            int.TryParse(Request.Form["custId"], out custIdExist);

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if ((custIdExist == 0) || string.IsNullOrEmpty(userNameDetail) || string.IsNullOrEmpty(password))
            {
                errCode = 101;
                errMsg = Resources.Lang.lblNotAuthorized;
                return new Result(errCode, errMsg);
            }
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal || !UserSession.Current.SubPageAccess.CreateAccount)
            {
                errCode = (int)ErrorType.AccessDenied;
                errMsg = Resources.Lang.lblNotAuthorized;
                return new Result(errCode, errMsg);
            }

            userName = UserSession.Current.UserName + userNameDetail;
            SearchAndCopyBO bo = new SearchAndCopyBO(UserSession.Current.NetSessionId);
            bo.CloneUser(out custId, custIdExist, userName, DNAide.Utils.Encryption.CfsEnCode(password.Trim()), firstName, lastName, phone, mobilePhone, fax, credit, maxCredit,
                                                Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), out errCode, out errMsg);

            if (custId > 0)
            {
                errCode = 999;
                errMsg = custId.ToString();
            }
            return new Result(errCode, errMsg);
        }

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

    }
}