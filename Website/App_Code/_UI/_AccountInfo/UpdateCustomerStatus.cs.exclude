using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sunplus.Agent.Model.BL.AccountInfo;
using Sunplus.Agent.Model.Entities;
/// <summary>
/// Summary description for UpdateCustomerStatus
/// </summary>
namespace Sunplus.Agent.UI._AccountInfo
{
    public class UpdateCustomerStatus_Handler : AccountInfoBase_Handler
    {
        #region Variable
        protected int _custId = 0;
        protected AccountInfoBO _accountInfoBO = null;
        protected AccountInfoBO _AccountInfoBO
        {
            get
            {
                if (_accountInfoBO == null)
                {
                    _accountInfoBO = new AccountInfoBO(CurrentUserSession.NetSessionId);
                }
                return _accountInfoBO;
            }

        }
        #endregion

        public UpdateCustomerStatus_Handler()
        {
            IsValidateRequest = false;
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            base.RenderResult(ref data);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            _custId = GetIntRequest("custid", 0);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            Sunplus.Agent.Model.BL.Authorization.AuthorizationBO authorizationBO = new Sunplus.Agent.Model.BL.Authorization.AuthorizationBO(CurrentUserSession.NetSessionId);
            bool canAccess = authorizationBO.CheckPageAccess(Sunplus.Agent.Model.Entities.PageAccessType.UpdateAccount, out _errCode, out _errMsg);
            if (!canAccess)
            {
                return new Fanex.Age.UI.Result(_errCode, _errMsg);
            }
            else if (_custId <= 0)
            {
                return new Fanex.Age.UI.Result(1, Resources.Lang.usernameunavailable);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return null;
        }
    }
}
