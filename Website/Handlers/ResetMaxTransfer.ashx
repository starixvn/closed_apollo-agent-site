<%@ WebHandler Language="C#" Class="ResetMaxTransfer" %>

using System;
using System.Web;
using System.Web.SessionState;
using DAL;
using BLL;
using DNAide.Web;
using DNAide.Core;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using System.Web.Security;
public class ResetMaxTransfer : IHttpHandler, IRequiresSessionState
{
    bool IsMultiple = false;

    public void ProcessRequest(HttpContext context)
    {

        context.Response.ContentType = "text/plain";
        context.Response.Cache.SetNoStore();

        HttpRequest r = context.Request;
        string errmsg = string.Empty;

        if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount)
        {
            context.Response.Write(106);
            return;
        }

        if (Sunplus.Agent.Model.Entities.UserSession.Current.AdminId != 0)
        {
            context.Response.Write(Resources.Lang.lblNotAuthorized);
            return;
        }

        bool bStatus = false;
        IsMultiple = (r["custid"].Contains(","));

        string strErr = string.Empty;
        if (!MemberInfo.CustomerCheckEnable(r["custid"], out strErr))
        {
            context.Response.Write(strErr);
            return;
        }

        CasinoLibrary.SBAgent.Customer csCustomer = new CasinoLibrary.SBAgent.Customer();

        try
        {
            string strUserName = Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.UserName : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName;
            if (!IsMultiple)
            {
                bStatus = csCustomer.ResetMaxTransfer(int.Parse(r["custid"]), Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                        Sunplus.Agent.Model.Entities.UserSession.Current.CustId, strUserName, 2);
            }
            else
            {
                string[] custids = r["custid"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < custids.Length; i++)
                {
                    int custid = int.Parse(custids[i]);
                    bStatus = csCustomer.ResetMaxTransfer(custid, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                            Sunplus.Agent.Model.Entities.UserSession.Current.CustId, strUserName, 2);
                    if (!bStatus) break;
                }
            }
        }
        catch (Exception e)
        {
            context.Response.Write(e.Message.Replace("'", "\\'"));
            return;
        }

        context.Response.Write(bStatus ? "Ok" : Resources.Lang.resetFailed);

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}