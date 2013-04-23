<%@ WebHandler Language="C#" Class="ChangePassMemberInfo" %>

using System;
using System.Web;
using System.Data;
using System.Web.Security;
using System.Web.SessionState;

using DAL;
using DNAide.Web;
using DNAide.Utils;
using Fanex.Data.DNAide;

using CasinoLibrary;
using CasinoLibrary.SBAgent;

using Sunplus.Agent.Model.Entities;

public class ChangePassMemberInfo : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string errMsg = string.Empty;

        if (UserSession.Current.IsInternal)
        {
            context.Response.Write(Resources.Lang.lblNotAuthorized);
            return;
        }

        if (!UserSession.Current.SubPageAccess.UpdateAccount)
        {
            context.Response.Write(106);
            return;
        }

        string userpass = (context.Request.Form["upw"] == null) ? "" : context.Request.Form["upw"];
        string custid = (context.Request.Form["custid"] == null) ? "" : context.Request.Form["custid"];
        bool isSync = context.Request.Form["issync"] == "1";

        DbResult dbr = MemberInfo.ChangePass(custid, userpass);

        // SynCS
        if (isSync)
        {
            Customer cust = new Customer();

            try // Casino cheat
            {
                cust.UpdateCustPwd(int.Parse(custid), Encryption.CfsEnCode(userpass), Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), UserSession.Current.CustId, UserSession.Current.SubAccId == 0 ? UserSession.Current.Username : UserSession.Current.SubUserName, 2);
            }
            catch { }
        }

        int errCode = DNAide.Core.CastAide.AsInt32(dbr.Outputs["errcode"]);
        errMsg = DNAide.Core.CastAide.AsString(dbr.Outputs["errmsg"]);

        if (errCode == 0)
        {
            context.Response.Write("Ok");
        }
        else
        {
            context.Response.Write(errMsg);
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}