<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._AccountInfo.UpdateDoubleCommStatus_Handler" %>

using System;
using System.Web;
using System.Collections;
using Sunplus.Agent.Model.BL.Authorization;

using Sunplus.Agent.Model.BL.AccountInfo;
using Fanex.Age.UI;

namespace Sunplus.Agent.UI._AccountInfo
{
    public class UpdateDoubleCommStatus_Handler : UpdateCustomerStatus_Handler, System.Web.SessionState.IRequiresSessionState
    {
        #region Variable
        private bool doubleCommLosing = false;
        #endregion

        public UpdateDoubleCommStatus_Handler()
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
            doubleCommLosing = GetBooleanRequest("doubleCommLosing", false);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowDoubleCommission)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblDComNotAuthorized);
            }

            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(Fanex.Age.UI.ErrorType.AccessDenied, Resources.Lang.lblDComNotAuthorized);
            }

            // Get Status Current user. Check data concurrent update
            Hashtable outputs = new Hashtable();
            AuthorizationBO authorizationBO = new AuthorizationBO(CurrentUserSession.NetSessionId);
            authorizationBO.GetCustomerStatus(CurrentUserSession.CustId, 0, ref outputs);

            if (!(outputs.Count > 0 && Convert.ToBoolean(outputs["doublecommlosing"])))
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblDComNotAuthorized);
            }

            return base.ValidateRequest(p);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            _errCode = _AccountInfoBO.UpdateDoubleCommStatus(_custId, doubleCommLosing, Request.UserHostAddress, ref _errMsg);
            return null;
        }
    }
}