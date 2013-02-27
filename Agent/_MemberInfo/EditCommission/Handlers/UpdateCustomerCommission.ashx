<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.EditCommission.UpdateCustomerCommission_Handler" %>

namespace Sunplus.Agent.UI._MemberInfo.EditCommission
{
    using System;
    using System.Web;
    using Newtonsoft.Json;

    using Fanex.PTEngine.BusinessObjects;
    using Fanex.Age.UI;
    using Sunplus.Agent.Model.BL.MemberInfo.EditCommission;

    public class UpdateCustomerCommission_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        private int targetCustId = 0;
        private int targetRoleId = 0;
        private string parameter = string.Empty;

        private ICommissionSetting commissionSetting;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Fanex.Age.UI.Result r = View as Fanex.Age.UI.Result;
            data.AsResult(r, true);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            int.TryParse(Request["targetCustId"], out targetCustId);
            int.TryParse(Request["targetRoleId"], out targetRoleId);
            string jsonCommission = Request.Form[0];
            commissionSetting = JsonConvert.DeserializeObject<CommissionSetting>(jsonCommission);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (commissionSetting != null)
            {
                IRequestContext requestContext = new RequestContext()
                {
                    CustId = CurrentUserSession.CustId,
                    TargetCustId = targetCustId,
                    TargetRoleId = targetRoleId,
                    FormId = FormType.EditPT,
                    SubAccId = CurrentUserSession.SubAccId,
                    AdminId = CurrentUserSession.UserId,
                    IP = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP()
                };

                EditCommissionBO bo = new EditCommissionBO(CurrentUserSession.NetSessionId);
                var result = bo.UpdateCommission(commissionSetting, requestContext);

                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    return new Fanex.Age.UI.Result(ErrorType.Unknown, result.ErrorMessage);
                }
                else
                {
                    return new Fanex.Age.UI.Result(ErrorType.Success, Resources.Lang.updatesuccessfull);
                }
            }

            return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
        }
    }
}