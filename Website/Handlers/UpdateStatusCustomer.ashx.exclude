<%@ WebHandler Language="C#" Class="UpdateStatusCustomer" %>

//nqminh - 06Feb09 - Update status customer

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Web;
using DNAide.Core;

using System.Web.Security;
public class UpdateStatusCustomer : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {

        context.Response.Cache.SetNoStore();
        context.Response.ContentType = "text/html";
        string errMsg = string.Empty;

        if (Sunplus.Agent.Model.Entities.UserSession.Current.CustId <= 0) return; // Invalid login

        if (!Sunplus.Agent.Model.Entities.UserSession.Current.SubPageAccess.UpdateAccount)
        {
            context.Response.Write(GetHtmlErrorMsg(Resources.Lang.lblNotAuthorized));
            return;
        }

        int custid = 0;
        int.TryParse("0" + context.Request["custid"], out custid);
        if (custid > 0)
        {
            bool status = false, isCustSync = true;

            string type = context.Request["type"];
            bool.TryParse(context.Request["status"], out status);
            isCustSync = CastAide.AsInt32(context.Request["issyncs"], 1) == 1;

            if (!String.IsNullOrEmpty(type))
            {
                bool isUpdateDownline = false;
                bool isClearAllCredit = false;
                string chkId = String.Empty;

                try
                {
                    switch (type)
                    {
                        case "suspended":
                            chkId = "chkSuspend";
                            MemberInfo.CustomerUpdateSuspendWithSyncCS(custid, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, status, isCustSync, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                            break;

                        case "closed":
                            chkId = "chkClosed";
                            isClearAllCredit = false;
                            Boolean.TryParse(context.Request["isupddownline"] as string ?? "false", out isClearAllCredit);
                            MemberInfo.CustomerUpdateClosedWithSyncCS(custid, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, status, isCustSync, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                            if (isClearAllCredit)
                            {
                                new Sunplus.Agent.Model.BL.MemberInfo.CreditBalance.CreditBalanceBO(Sunplus.Agent.Model.Entities.UserSession.Current.NetSessionId).ClearCustomerCredit(custid, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), out errMsg);
                            }
                            break;

                        case "disableor":
                            chkId = "chkOR";
                            MemberInfo.CustomerUpdateDisableOutright(custid, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, !status, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                            break;

                        case "disablehr":
                            chkId = "chkHR";
                            isUpdateDownline = false;
                            if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdateDownline))
                                throw new IBCException(Resources.Lang.invalidupdatedownlineparam);
                            MemberInfo.CustomerUpdateDisabledHR(Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, Sunplus.Agent.Model.Entities.UserSession.Current.UserId, custid, !status, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), isUpdateDownline, out errMsg);
                            break;
                        case "disableopentote":
                            chkId = "chkOpenTote";
                            isUpdateDownline = false;
                            if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdateDownline))
                                throw new IBCException(Resources.Lang.invalidupdatedownlineparam);
                            MemberInfo.CustomerUpdateDisabledOpenTote(Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, Sunplus.Agent.Model.Entities.UserSession.Current.UserId, custid, !status, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), isUpdateDownline, out errMsg);
                            break;
                        case "disablefi":
                            chkId = "chkFinancial";
                            isUpdateDownline = false;
                            if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdateDownline))
                                throw new IBCException(Resources.Lang.invalidupdatedownlineparam);
                            MemberInfo.CustomerUpdateDisabledFinancial(Sunplus.Agent.Model.Entities.UserSession.Current.UserId, custid, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, !status, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), isUpdateDownline, out errMsg);
                            break;

                        case "disablecs":
                            chkId = "chkCS";
                            isUpdateDownline = false;
                            if (!Boolean.TryParse(context.Request["isupddownline"] as string ?? string.Empty, out isUpdateDownline))
                                throw new IBCException(Resources.Lang.invalidupdatedownlineparam);
                            //MemberInfo.CustomerUpdateDisabledCS(Sunplus.Agent.Model.Entities.UserSession.Current.UserId, custid, !status, true, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), isUpdateDownline, out errmsg);
                            bool result = DAL.CasinoProvider.Instance.UpdateCSStatus(custid, true, !status, !isUpdateDownline, out errMsg);
                            if (!String.IsNullOrEmpty(errMsg)) throw new IBCException(errMsg);
                            break;

                        case "disablemautopt":
                            chkId = "chkMAutoPT";
                            MemberInfo.AllowAutoMasterPT(custid, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId, !status, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
                            break;

                        default: throw new IBCException(Resources.Lang.invalidsession);
                    }

                    if (!String.IsNullOrEmpty(errMsg)) //show message information if any (not error msg)
                        context.Response.Write(GetHtmlErrorMsg(errMsg));

                    context.Response.Write(GetJsChangeStatus(chkId, status ? "1" : "0", isClearAllCredit)); //change checkbox status
                }
                catch (Exception ex)
                {
                    //for firefox 2: reset checkbox status
                    //if (IsFireFox2(context))
                    //{
                    //    if (!String.IsNullOrEmpty(chkId))
                    //        context.Response.Write(GetJsChangeStatus(chkId));
                    //}

                    string str = ex.Message;
                    Sunplus.Agent.Utilities.Logs.Write(ex);
                    str = Common.NotAuthorized(str);
                    context.Response.Write(GetHtmlErrorMsg(str));
                }
            }

            return;
        }
    }

    private bool IsFireFox2(HttpContext context)
    {
        return (context.Request.Browser.Browser.Equals("firefox", StringComparison.OrdinalIgnoreCase)
            && context.Request.Browser.Version.StartsWith("2."));
    }

    private string GetCurrentIDChkBoxStatus(HttpContext context)
    {
        string type = context.Request["type"];
        string chkId = String.Empty;
        switch (type)
        {
            case "suspended":
                chkId = "chkSuspend";
                break;
            case "closed":
                chkId = "chkClosed";
                break;
            case "disablehr":
                chkId = "chkHR";
                break;
            case "disablefi":
                chkId = "chkFinancial";
                break;
        }
        return chkId;
    }

    protected string GetHtmlErrorMsg(string msg)
    {
        return String.Format("<script type='text/javascript'>parent.ShowErrMsg('{0}');</script>", Common.RemoveSpecialChars(msg));
    }

    protected string GetJsChangeStatus(string chkId, string status, bool isClearAllCredit)
    {
        return String.Format("<script type='text/javascript'>parent.setCheckBoxStatus('{0}', {1}); {2}</script>",
                              chkId, status,
                              isClearAllCredit ? " parent.reloadMenuUsrInfo();" : String.Empty);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}