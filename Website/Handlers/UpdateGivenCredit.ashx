<%@ WebHandler Language="C#" Class="UpdateGivenCredit" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Text;
using DNAide.Core;
using IBCAgent;

using System.Web.Security;
public class UpdateGivenCredit : IHttpHandler, IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        try 
        {
            context.Response.ContentType = "text/html";
            
            string errMsg = string.Empty;

            if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount)
            {
                context.Response.Write("<script type='text/javascript'>alert('" + PositionTaking.ReplaceData(Resources.Lang.lblNotAuthorized) + "');</script>");
                return;
            }
            
            string sAmount = context.Request["amount"];
            sAmount = sAmount.Replace(",", "");
            if (String.IsNullOrEmpty(sAmount)) throw new IBCException(Resources.Lang.invalidamount);

            string _custid = context.Request["custid"];
            if (String.IsNullOrEmpty(_custid)) throw new IBCException(Resources.Lang.usernameunavailable);
            
            string _creditType = context.Request["creditType"];
            string _custRoleId = context.Request["custRoleId"];
                        
            string errmsg = String.Empty;            
            bool isproblem = false;
            if (_creditType.ToLower() == "txtgivencredit")
            {
                string siteLevel = "";
                switch (_custRoleId )
                {
                    case "2":
                        siteLevel = "AgentList";
                        break;
                    case "1":
                        siteLevel = "MemberList";
                        break;
                }
                isproblem = MemberInfo.UpdateCreditBalance(_custid, siteLevel, CastAide.AsDouble(sAmount), ref errmsg);                
            }
            else if (_creditType.ToLower() == "txtmemmaxcredit")
                isproblem = MemberInfo.UpdateAgentMaxCredit(CastAide.AsInt32(_custid), CastAide.AsDouble(sAmount), ref errmsg);
            if (isproblem) throw new IBCException(errmsg);
            
            StringBuilder s = new StringBuilder();
            s.Append("parent.$('span" + _creditType + "').innerHTML ='" + DNAide.Utils.FormatHandler.FormatNumber20(CastAide.AsDouble(sAmount), Sunplus.Agent.Model.Entities.UserSession.Current.LangKey) + "';");
            s.Append("parent.changeCredit(false, '" + _creditType + "');");
            if (_creditType.ToLower() == "txtgivencredit")
                s.Append("parent.reloadMenuUsrInfo();");
            context.Response.Write("<script type='text/javascript'>" + s.ToString() + "</script>");            
        }
        catch(Exception ex)
        {
            string str = ex.Message;
            str = Common.NotAuthorized(str);
            context.Response.Write("<script type='text/javascript'>alert('" + str.Replace("'", "\\'") + "');</script>");
        }
        catch
        {
            #if DEBUG
                throw;
            #endif
        }            
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}