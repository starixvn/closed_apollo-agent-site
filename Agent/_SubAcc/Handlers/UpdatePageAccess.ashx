<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._SubAcc.UpdatePageAccess_Handler" %>

using System;
using System.Web;

using Fanex.Age.UI;
using Sunplus.Agent.Model.BL.SubAcc;
using Sunplus.Agent.Model.Entities;

/*
 * Created 20091224@Lee - Updating page access of sub-account
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.UI._SubAcc
{
    public class UpdatePageAccess_Handler : Fanex.Age.UI.TplHandler
    {
        int subAccId = 0;
        string updPageAccess = string.Empty;

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            p.AddHttpParameters(new string[] {
                "SubAccId",
                "updPageAccess" // Value of page access [A][B]....
            }, Request);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            // Sub-account does not have permission to access SubAcc function. Internal agent does not have update permission.
            if (CurrentUserSession.SubAccId > 0)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            // Validate sub-account id in case user try to put an invalid sub-account id
            subAccId = int.Parse(p["SubAccId"] as string);
            if (subAccId <= 0)
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.strNotAccessPage);
            }

            // Value of page access [A][B]....
            updPageAccess = p["updPageAccess"] as string;
            string[] arrPageAccess = updPageAccess.Split(']');
            for (int i = 0; i < arrPageAccess.Length - 1; i++)
            {
                if (!PageAccessType.IsValidType(arrPageAccess[i] + "]"))
                {
                    return new Result(ErrorType.AccessDenied, Resources.Lang.strNotAccessPage);
                }
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            SubAccBO bo = new SubAccBO(CurrentUserSession.NetSessionId);

            string errMsg;
            int errCode = bo.UpdateSubAccountPermission(
                subAccId,
                updPageAccess,
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