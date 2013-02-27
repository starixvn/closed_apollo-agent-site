<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.UpdateCasinoStatus_Handler" %>

using System;
using System.Web;

using Sunplus.Agent.Model.BL.AccountInfo;

namespace Sunplus.Agent.UI._AccountInfo
{
    public class UpdateCasinoStatus_Handler : UpdateCustomerStatus_Handler, System.Web.SessionState.IRequiresSessionState
    {
        #region Variable
        private bool _isDisabled = false;
        private bool _isSynCasino = false;
        private bool _isUpdateDownline = false;
        #endregion

        public UpdateCasinoStatus_Handler()
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
            _isSynCasino = true; //GetBooleanRequest("issynccs", false);
            _isUpdateDownline = GetBooleanRequest("isupdatedownline", false);
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
            _errMsg = string.Empty;
            _errCode = _AccountInfoBO.UpdateCasinoStatus(_custId, _isDisabled, _isSynCasino, _isUpdateDownline, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), ref _errMsg);
            
            return null;
        }
    }
}