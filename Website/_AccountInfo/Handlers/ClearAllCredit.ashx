<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.ClearAllCredit_Handler" %>

using System;
using System.Web;

using Sunplus.Agent.Model.BL.AccountInfo;

namespace Sunplus.Agent.UI._AccountInfo
{
    public class ClearAllCredit_Handler : AccountInfoBase_Handler, System.Web.SessionState.IRequiresSessionState
    {
        #region Variable

        private int _custId = 0;
        #endregion

        public ClearAllCredit_Handler()
        {
            IsValidateRequest = false;
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            data.Add("errCode", _errCode);
            data.Add("errMsg", _errMsg);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            _custId = GetIntRequest("custid", 0);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return base.ValidateRequest(p);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            _errCode = new Sunplus.Agent.Model.BL.MemberInfo.CreditBalance.CreditBalanceBO(CurrentUserSession.NetSessionId).ClearCustomerCredit(
                    _custId,
                    Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    out _errMsg
                    );

            if (_errCode == 0) _errMsg = "";

            return null;
        }
    }
}