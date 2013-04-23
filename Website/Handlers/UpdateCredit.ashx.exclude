<%@ WebHandler Language="C#" Class="UpdateCredit" %>

using System;
using System.Web;
using DNAide.Web;
using Fanex.Data.DNAide;
using DNAide.Core;
using System.Web.SessionState;

using System.Web.Security;
public class UpdateCredit : IHttpHandler,IRequiresSessionState {

    public void ProcessRequest(HttpContext context)
    {
        context = IbcHandler.InitializeCulture(context);
        string custid = (context.Request["custid"] == null) ? Sunplus.Agent.Model.Entities.UserSession.Current.CustId.ToString() : context.Request["custid"];

        string strErrMsg = string.Empty;
        
        if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount)
        {
            context.Response.Write(106);
            return;
        }     
        
        string sitetype = (context.Request["site"] == null) ? Common.GetLevel(Sunplus.Agent.Model.Entities.UserSession.Current.RoleId) : context.Request["site"];
        
        string amt = (context.Request["amt"] == null) ? "0" : context.Request["amt"];
        string errmsg = "";
        if (IsNumeric(amt))
        {
            try
            {
                bool isproblem = MemberInfo.UpdateCreditBalance(custid, sitetype, CastAide.AsDouble(amt), ref errmsg);
                if (isproblem == false)
                {
                    context.Response.Write("Ok");
                }
                else
                {
                    context.Response.Write(errmsg);
                }
            }
            catch
            {
                context.Response.Write(Resources.Lang.lblNotAuthorized);
            }
        }
        else
        {
            context.Response.Write(Resources.Lang.errorinputnumber);
        }
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }
    internal static bool IsNumeric(string numberString)
    {
        char[] ca = numberString.ToCharArray();
        for (int i = 0; i < ca.Length; i++)
        {
            if (ca[i] > 57 || ca[i] < 48)
                return false;
        }
        return true;
    }
}