<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.UpdateMemberInfo_Handler" %>

namespace Sunplus.Agent.UI._AccountInfo
{
    using System;
    using System.Web;
    using System.Collections;
    using System.Data;

    using Fanex.Age.UI;
    using Sunplus.Agent.Model.BL.SubAcc;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.BL.AccountInfo;

    public class UpdateMemberInfo_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IRequiresSessionState
    {

        #region Variable
        public int custid = 0;
        public string firstName = string.Empty;
        public string lastName = string.Empty;
        public string phone = string.Empty;
        public string mobile = string.Empty;
        public string email = string.Empty;
        public string fax = string.Empty;
        public string ip = string.Empty;
        private int isSyncCS;
        #endregion

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);

            int errCode = 0;
            string errMsg = "";
            p.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            ip = p["ip"].ToString();

            firstName = HttpUtility.HtmlDecode(p["txtFName"].ToString());
            lastName = HttpUtility.HtmlDecode(p["txtLName"].ToString());
            phone = HttpUtility.HtmlDecode(p["txtPhone"].ToString());
            mobile = HttpUtility.HtmlDecode(p["txtMobile"].ToString());
            email = HttpUtility.HtmlDecode(p["txtEmail"].ToString());
            fax = HttpUtility.HtmlDecode(p["txtFax"].ToString());
            isSyncCS = Convert.ToInt32(p["isSyncCS"]);
            errCode = bo.UpdateCustomerUserInfo(custid, firstName, lastName, phone, mobile, email, fax, isSyncCS, ip, ref errCode);

            return new Result(errCode, errMsg);

        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {

            p.AddHttpParameters(new string[]{
                "txtFName",
                "txtLName",
                "txtPhone",
                "txtMobile",
                "txtEmail",
                "txtFax",
                "custId",
                "isSyncCS"
            }, Request);

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            custid = Convert.ToInt32(p["custId"]);
            if (custid <= 0)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return null;
        }

    }
}