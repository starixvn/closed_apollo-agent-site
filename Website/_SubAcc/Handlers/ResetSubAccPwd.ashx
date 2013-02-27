<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._SubAcc.ResetSubAccPwd_Handler" %>

using System;
using System.Web;

using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.SubAcc;
using Sunplus.Agent.Model.Entities;

/*
 * Created 20091224@Lee - Reseting password of sub-account
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._SubAcc
{
    public class ResetSubAccPwd_Handler : Fanex.Age.UI.TplHandler
    {
        private int subAccId = 0;
        private string newPwd = null;
        private string confirmNewPwd = null;
        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            // 'txtNewPwd', 'txtConfirmPwd', 'SubAccId'

            p.AddHttpParameters(new string[] {
                "txtNewPwd", "txtConfirmPwd", "SubAccId"
            }, Request);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            // Sub-account does not have permission to access SubAcc function. Internal agent does not have update permission.
            if (CurrentUserSession.SubAccId > 0)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            // Validate sub-account id in case user try to insert invalid sub-account id
            subAccId = int.Parse(p["SubAccId"] as string);
            if (subAccId <= 0)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.strNotAccessPage);
            }

            // Validate password in case user try to pass client checking
            newPwd = p["txtNewPwd"] as string;
            confirmNewPwd = p["txtConfirmPwd"] as string;

            if (newPwd != confirmNewPwd || !Validation.CheckValidPassword(newPwd))
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.strNotAccessPage);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            SubAccBO bo = new SubAccBO(CurrentUserSession.NetSessionId);

            string errMsg;

            int errCode = bo.UpdateSubAccount(
                subAccId,
                newPwd,
                Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                out errMsg
                );

            return new Result(errCode, errMsg);
        }

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r);
        }
    }
}