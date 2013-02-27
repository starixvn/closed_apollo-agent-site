<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.UpdateOutrightStatus_Handler" %>

using System;
using System.Web;

using Sunplus.Agent.Model.BL.AccountInfo;

namespace Sunplus.Agent.UI._AccountInfo
{
    public class UpdateOutrightStatus_Handler : UpdateCustomerStatus_Handler, System.Web.SessionState.IRequiresSessionState
    {
        #region Variable
        private bool _isDisabled = false;
        #endregion
        public UpdateOutrightStatus_Handler()
        {
            IsValidateRequest = false;
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            base.RenderResult(ref data);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            base.ProcessRequest(ref p);
            _isDisabled = GetBooleanRequest("isdisabled", false);
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
            _errMsg = "";
            _errCode = _AccountInfoBO.UpdateOutrightStatus(_custId, _isDisabled, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref _errMsg);
            return null;
        }
    }
}