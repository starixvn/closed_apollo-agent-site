<%@ WebHandler Language="C#" Class="DownlineList" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Web;
using DNAide.Core;
using CasinoLibrary;
using CasinoLibrary.SBAgent;
using System.Data;
using DAL;

using System.Web.Security;

public class DownlineList : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {

        context.Response.Cache.SetNoStore();
        context.Response.ContentType = "text/html";
        string errMsg = string.Empty;
        if (Sunplus.Agent.Model.Entities.UserSession.Current.CustId <= 0) return; // Invalid login

        if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount)
        {
            context.Response.Write("<script type='text/javascript'>parent.ShowErrMsg('" + PositionTaking.ReplaceData(Resources.Lang.lblNotAuthorized) + "');</script>");
            return;
        }

        int custid = 0;
        int.TryParse("0" + context.Request["custid"], out custid);
        if (custid > 0)
        {
            // do save new status here
            bool isSuspended = false;
            bool isClosed = false;
            bool isAllowAutoMPT = false;
            bool isAllowOutright = false;
            bool isClearAllCredit = false;
            bool isSync = false;
            bool isAllowNumberGame = false;
            bool isDownLineNumberGame = false;

            bool.TryParse(context.Request["isSuspended"], out isSuspended);
            bool.TryParse(context.Request["isClosed"], out isClosed);
            bool.TryParse(context.Request["isAllowAutoMPT"], out isAllowAutoMPT);
            bool.TryParse(context.Request["isOutright"], out isAllowOutright);

            bool.TryParse(context.Request["isAllowNumberGame"], out isAllowNumberGame);
            bool.TryParse(context.Request["isDownLineNumberGame"], out isDownLineNumberGame);

            isSync = context.Request["issync"] == "1";

            try
            {
                if (context.Request["isClosed"] + "" != "-1")
                {

                    MemberInfo.CustomerUpdateClosedWithSyncCS(custid, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, isClosed, isSync, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                    if (isClearAllCredit)
                    {
                        new Sunplus.Agent.Model.BL.MemberInfo.CreditBalance.CreditBalanceBO(Sunplus.Agent.Model.Entities.UserSession.Current.NetSessionId).ClearCustomerCredit(custid, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), out errMsg);
                    }
                }
                if (context.Request["isSuspended"] + "" != "-1")
                {
                    MemberInfo.CustomerUpdateSuspendWithSyncCS(custid, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, isSuspended, isSync, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                }
                if (context.Request["isAllowAutoMPT"] + "" != "-1")
                {
                    MemberInfo.AllowAutoMasterPT(custid, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, (isAllowAutoMPT) ? false : true, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                }
                if (context.Request["isOutright"] + "" != "-1")
                {
                    MemberInfo.CustomerUpdateDisableOutright(custid, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, !isAllowOutright, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                }
                if (context.Request["isAllowNumberGame"] + "" != "-1")
                {
                    MemberInfo.CustomerUpdateDisableNumber(custid, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, !isAllowNumberGame, !isDownLineNumberGame, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                }
                context.Response.Write("<script type='text/javascript'>parent.DelayReloadPage(); </script>");
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                Sunplus.Agent.Utilities.Logs.Write(ex);
                str = Common.NotAuthorized(str);
                context.Response.Write("<script type='text/javascript'>parent.ShowErrMsg('" + PositionTaking.ReplaceData(str) + "');</script>");
            }

            return;
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