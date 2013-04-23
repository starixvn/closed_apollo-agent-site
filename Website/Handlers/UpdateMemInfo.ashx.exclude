<%@ WebHandler Language="C#" Class="UpdateMemInfo" %>

using System;
using System.Web;
using DNAide.Web;
using Fanex.Data.DNAide;
using System.Web.SessionState;
using DNAide.Core;
using CasinoLibrary;
using CasinoLibrary.SBAgent;

using System.Web.Security;

public class UpdateMemInfo : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/html";

        Common.UpdateCulture();

        string errMsg = string.Empty;

        if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount)
        {
            context.Response.Write("<script type='text/javascript'>parent.showInfoMsg('" + PositionTaking.ReplaceData(Resources.Lang.lblNotAuthorized) + "',0);</script>");
            return;
        }

        int custid = CastAide.AsInt32(context.Request.QueryString["custid"], -1);
        int isCustSync = CastAide.AsInt32(context.Request.QueryString["isCustSync"], 1);
        if (custid > 0)
        {
            string firstName = CastAide.AsString(context.Request.Form["txtFName"], "");
            string lastName = CastAide.AsString(context.Request.Form["txtLName"], "");
            string phone = CastAide.AsString(context.Request.Form["txtPhone"], "");
            string mobilePhone = CastAide.AsString(context.Request.Form["txtMobile"], "");
            string email = CastAide.AsString(context.Request.Form["txtEmail"], "");
            string fax = CastAide.AsString(context.Request.Form["txtFax"], "");
            int errorCode = 0;
            string errorMsg = String.Empty;

            try
            {
                errorCode = IBCAgent.UserInfo.UpdateCustomerByInfo(custid, firstName, lastName, phone, mobilePhone, email, fax,
                    Sunplus.Agent.Model.Entities.UserSession.Current.UserId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, Sunplus.Agent.Model.Entities.UserSession.Current.LangKey, ref errorMsg);
                if (errorCode > 0)
                    WriteMsg(context, errorMsg, 0);
                else
                {
                    if (isCustSync == 1)
                    {
                        // Sync to casino
                        Customer cust = new Customer();
                        try // Casino cheat
                        {
                            cust.UpdateCustInfoSimpleNoStatus(custid, firstName, lastName, phone, mobilePhone, fax, email, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.Username : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName, 2);
                        }
                        catch { }
                    }

                    WriteMsg(context, Resources.Lang.updatesuccessfull, 1);
                }

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
        string jsReturn = String.Format("<script type='text/javascript'>parent.showInfoMsg('{0}', {1}); top.menu.reloadMenuUsrInfo();</script>", msg.Replace("'", "\\'"), msgType);
        context.Response.Write(jsReturn);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}