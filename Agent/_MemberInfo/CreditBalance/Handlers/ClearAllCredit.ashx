<%@ WebHandler Language="C#" Class="Sunplus.Agent.MemberInfo.ClearAllCredit_Handler" %>

namespace Sunplus.Agent.MemberInfo
{
    using System;
    using System.Web;
    using Fanex.Age.UI;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.BL.MemberInfo.CreditBalance;

    public class ClearAllCredit_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        private int custId = 0;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["custid"], out this.custId);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (this.custId == 0)
            {
                return new Result(ErrorType.Unknown, Resources.Lang.strNotAccessPage);
            }

            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            string errMsg = String.Empty;

            CreditBalanceBO bo = new CreditBalanceBO(CurrentUserSession.NetSessionId);

            int errCode = bo.ClearCustomerCredit(
                custId,
                Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                out errMsg
                );

            return new Result(errCode, errMsg);
        }
    }
}