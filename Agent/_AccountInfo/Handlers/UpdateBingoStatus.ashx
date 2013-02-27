<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.UpdateBingoStatus_Handler" %>

using System;
using System.Web;

using Sunplus.Agent.Model.BL.AccountInfo;
using Fanex.Age.UI;

namespace Sunplus.Agent.UI._AccountInfo
{
    public class UpdateBingoStatus_Handler : UpdateCustomerStatus_Handler, System.Web.SessionState.IRequiresSessionState
    {
        #region Variable
        private bool _isDisabled = false;
        private bool _isSyncBingo = false;
        private bool _isEnableDownline = false;
        #endregion

        public UpdateBingoStatus_Handler()
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
            _isSyncBingo = true;// GetBooleanRequest("issyncbingo", false);
            _isEnableDownline = GetBooleanRequest("enabledownline", false);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.IsBingoSync || CurrentUserSession.IsBingoDisabled)
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
            _errCode = _AccountInfoBO.UpdateBingoStatus(_custId, _isDisabled, _isSyncBingo, _isEnableDownline, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref _errMsg);
            return null;
        }
    }
}