<%@ WebHandler Language="C#" Class="UpdateAgentCommission" %>

using System;
using System.Web;
using DNAide.Web;
using Fanex.Data.DNAide;
using System.Web.SessionState;
using DNAide.Core;

using System.Web.Security;
using Sunplus.Agent.Utilities;

public class UpdateAgentCommission : IHttpHandler, IRequiresSessionState
{    
    public void ProcessRequest (HttpContext context) {
        
        context.Response.ContentType = "text/html";
        Common.UpdateCulture();
        string errMsg = string.Empty;

        if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount)
        {
            context.Response.Write("<script type='text/javascript'>parent.showInfoMsg('" + PositionTaking.ReplaceData(Resources.Lang.lblNotAuthorized) + "',0);</script>");
            return;
        }

        int custid = CastAide.AsInt32(context.Request.QueryString["custid"], -1);
        int roleId = CastAide.AsInt32(context.Request.QueryString["roleId"], -1);
        if (custid > 0)
        {            
            double groupA = CastAide.AsDouble(context.Request["ddlgroupa"]);
            double groupB = CastAide.AsDouble(context.Request["ddlgroupb"]);
            double groupC = CastAide.AsDouble(context.Request["ddlgroupc"]);
            double groupD = CastAide.AsDouble(context.Request["ddlgroupd"]);
            double discount1x2 = CastAide.AsDouble(context.Request["ddlgroup1x2"]);
            double discountcs = CastAide.AsDouble(context.Request["ddlgroupother"]);
            double discountnumber = CastAide.AsDouble(context.Request["ddlgroupnumber"],0);
            
            int errorCode = 0;
            string errorMsg = String.Empty;

            try
            {                
                errorCode = IBCAgent.UserInfo.UpdateCommissionByAgent(custid, roleId,
                    groupA, groupB, groupC, groupD, discount1x2, discountcs, discountnumber,
                    Sunplus.Agent.Model.Entities.UserSession.Current.UserId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey, ref errorMsg);                                
                if (errorCode > 0)
                    WriteMsg(context, errorMsg, 0);
                else WriteMsg(context, Resources.Lang.updatesuccessfull, 1);
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                Sunplus.Agent.Utilities.Logs.Write(ex);
                str = Common.NotAuthorized(str);
                WriteMsg(context, str, 0);
            }
        }          
    }

    protected void WriteMsg(HttpContext context, string msg, int msgType)
    {
        string jsReturn = String.Format("<script type='text/javascript'>parent.showInfoMsg('{0}', {1});</script>", msg.Replace("'", "\\'"), msgType);
        context.Response.Write(jsReturn);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
