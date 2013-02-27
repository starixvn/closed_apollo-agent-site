<%@ WebHandler Language="C#" Class="UpdateTransferCondition" %>

using System;
using System.Web;
using DNAide.Web;
using DNAide.Utils;
using DNAide.Core;
using Fanex.Data.DNAide;
using System.Web.SessionState;

using System.Web.Security;
public class UpdateTransferCondition : IHttpHandler,IRequiresSessionState  {
    
    public void ProcessRequest (HttpContext context) 
    {
        try
        {
            context.Response.ContentType = "text/html";

            string errMsg = string.Empty;

            if (Sunplus.Agent.Model.Entities.UserSession.Current.IsInternal)
            {
                context.Response.Write(Resources.Lang.lblNotAuthorized);
                return;
            }
            
            if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount)
            {
                context.Response.Write(106);
                return;
            }
            
            DbResult dbr = null;
            string custid = (context.Request.Form["custid"] == null) ? "" : context.Request.Form["custid"];
            string transferoption = (context.Request.Form["transop"] == null) ? "" : context.Request.Form["transop"];
            dbr = MemberInfo.UpdateTransferCondition(custid, transferoption);
            if (CastAide.AsInt32(dbr.Outputs["errcode"]) == 0)
            {
                context.Response.Write("Ok");
            }
            else
            {
                context.Response.Write(dbr.Outputs["errcode"].ToString());              
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message;
            Sunplus.Agent.Utilities.Logs.Write(ex);
            str = Common.NotAuthorized(str);
            context.Response.Write(str);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}