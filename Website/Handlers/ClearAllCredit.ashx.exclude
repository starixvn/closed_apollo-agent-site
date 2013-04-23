<%@ WebHandler Language="C#" Class="AGE.Agent.MemberInfo.ClearAllCredit_Handler" %>

using System;
using System.Web;

namespace AGE.Agent.MemberInfo
{
    public class ClearAllCredit_Handler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            string errMsg = string.Empty;
            int errCode = 0;

            if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount)
            {
                context.Response.Write(106);
                return;
            }

            int custId = DNAide.Core.CastAide.AsInt32(context.Request.QueryString["custid"], -1);

            errCode = new Sunplus.Agent.Model.BL.MemberInfo.CreditBalance.CreditBalanceBO(Sunplus.Agent.Model.Entities.UserSession.Current.NetSessionId).ClearCustomerCredit(
                custId,
                Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                out errMsg
                );

            if (errCode == 0)
            {
                errMsg = "0";
            }

            context.Response.Write(errMsg);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}