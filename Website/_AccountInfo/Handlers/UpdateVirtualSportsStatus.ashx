<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.UpdateVirtualSportsStatus_Handler" %>

using System;
using System.Web;

using Sunplus.Agent.Model.BL.AccountInfo;
using Fanex.Age.UI;

namespace Sunplus.Agent.UI._AccountInfo
{
    public class UpdateVirtualSportsStatus_Handler : UpdateCustomerStatus_Handler, System.Web.SessionState.IRequiresSessionState
    {
        #region Variable
        private bool _isDisabled = false;
        private bool _isDisableDownline = false;
        #endregion

        public UpdateVirtualSportsStatus_Handler()
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
            _isDisableDownline = GetBooleanRequest("disabledownline", false);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (CurrentUserSession.IsDisabledVirtualSports || CurrentUserSession.IsHideVirtualSports)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return base.ValidateRequest(p);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            _errCode = _AccountInfoBO.UpdateVirtualSportsStatus(_custId, _isDisabled, _isDisableDownline, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref _errMsg);
            return null;
        }
    }
}