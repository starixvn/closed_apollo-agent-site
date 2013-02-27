<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.ResetPassword_Handler" %>

namespace Sunplus.Agent.UI._AccountInfo
{
    using System;
    using System.Web;
    using Sunplus.Agent.Utilities;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.BL.AccountInfo;
    using Fanex.Age.UI;
    using CasinoLibrary;
    using CasinoLibrary.SBAgent;

    public class ResetPassword_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        private string newPassword = string.Empty;
        private string custId;
        bool isSynCasino = false;
        string errMsg = string.Empty;
        int errCode = (int)ErrorType.Unknown;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            newPassword = Request["password"];
            custId = Request["custId"];
            bool.TryParse(Request["isSyncCasino"], out isSynCasino);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal ||
                 string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(custId))
            {
                return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            AccountInfoBO bo = new AccountInfoBO(CurrentUserSession.NetSessionId);

            errCode = bo.ResetPassword(custId, newPassword, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());

            if (isSynCasino)
            {
                Customer cust = new Customer();

                try // Casino cheat
                {
                    cust.UpdateCustPwd(int.Parse(custId), Encryption.CfsEnCode(newPassword), Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.UserName : UserSession.Current.SubUserName, 2);
                }
                catch { }
            }

            return new Result(errCode, errMsg);
        }

    }
}