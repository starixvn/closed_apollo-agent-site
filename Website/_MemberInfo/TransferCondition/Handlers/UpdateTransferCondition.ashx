<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.TransferCondition.UpdateTransferCondition_Handler" %>
namespace Sunplus.Agent.UI._MemberInfo.TransferCondition
{
    using System;
    using System.Web;
    using Fanex.Age.UI;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.BL.MemberInfo.TransferCondition;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Collections;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;
    using System.Data;

    public class UpdateTransferCondition_Handler : Fanex.Age.UI.TplHandler, System.Web.SessionState.IReadOnlySessionState
    {
        private string transferOption = string.Empty;
        private string targetCustId = string.Empty;
        string errMsg = string.Empty;
        int errCode = (int)ErrorType.Unknown;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r, true);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            if (Request["transferOption"] != null)
            {
                transferOption = Request["transferOption"];
            }

            if (Request["targetCustId"] != null)
            {
                targetCustId = Request["targetCustId"];
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.UpdateAccount || CurrentUserSession.IsInternal ||
                string.IsNullOrEmpty(transferOption) || string.IsNullOrEmpty(targetCustId))
            {
                return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (!string.IsNullOrEmpty(transferOption))
            {
                TransferConditionBO bo = new TransferConditionBO(UserSession.Current.NetSessionId);
                Hashtable outputs = bo.UpdateTransferCondition(targetCustId, transferOption, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                errCode = (int)outputs["errcode"];
                if (errCode == 0)
                {
                    errMsg = Resources.Lang.updatesuccessfull;
                    return new Fanex.Age.UI.Result(errCode, errMsg);
                }

                errMsg = outputs["errmsg"].ToString();
            }

            return new Fanex.Age.UI.Result(errCode, errMsg);
        }

    }
}