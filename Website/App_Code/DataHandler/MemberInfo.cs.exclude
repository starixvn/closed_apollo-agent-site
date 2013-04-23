using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using DNAide.Core;
using Fanex.Data.DNAide;
using Sunplus.Agent.Utilities;

public static partial class MemberInfo
{
    public static int CustSubUpdPageAccess(
        int custid,
        int subaccid,
        string updPageAccess,
        bool addnew,
        string language,
        int loginSubAccId,
        out string errMsg
        )
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("subaccid", subaccid);
        htb.Add("addnew", addnew);
        htb.Add("custid", custid);
        htb.Add("language", language);
        htb.Add("updpageaccess", updPageAccess);
        htb.Add("loginsubaccid", loginSubAccId);
        htb.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        htb.Add("errcode", 0);
        htb.Add("errmsg", string.Empty);
        htb.Add("loginip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustSubUpdPageAccess", htb);

        errMsg = CastAide.AsString(dbr.Outputs["errmsg"]);

        return CastAide.AsInt32(dbr.Outputs["errcode"]);
    }

    /// <summary>
    /// Get suspend status
    /// </summary>
    /// <param name="custid"></param>
    /// <param name="custType">1: super, 2:master, 3:agent</param>
    /// <returns></returns>
    public static void GetCustStatus(int custid, string siteName, out bool isSuspended, out bool isAllowOutright)
    {
        isSuspended = false;
        DbResult dbr = null;
        Hashtable htb = new Hashtable();
        htb.Add("custid", custid);
        if (siteName == "Super")
            dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelEditSuspend_SAg", htb, 600);
        else if (siteName == "Master")
            dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelEditSuspend_MAg", htb, 600);
        else if (siteName == "Agent")
            dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelEditSuspend_Ag", htb, 600);

        DataTable dt = dbr.ResultSet.Tables[0];
        bool.TryParse(dt.Rows[0]["UplineSuspended"].ToString(), out isSuspended);
        bool.TryParse(dt.Rows[0]["UplineDisableOutright"].ToString(), out isAllowOutright);
    }

    /// <summary>
    /// Update Open/Closed status for customer
    /// sp: Acc_CustomerUpdClosed
    /// @viet hoang
    /// </summary>
    /// <param name="accid"></param>
    /// <param name="custid"></param>
    /// <param name="isClosed"></param>
    public static void CustomerUpdateClosed(int accid, int subAccId, bool isClosed, string Ip)
    {
        Hashtable param = new Hashtable();
        param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        param.Add("custid", accid);
        param.Add("subaccid", subAccId);
        param.Add("closed", isClosed);
        param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        param.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino);
        param.Add("errcode", 0);
        param.Add("errmsg", string.Empty);
        param.Add("ip", Ip);// Vinh add Ip for closed member
        DbResult dr = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdClosed", param);

        int errCode = (int)dr.Outputs["errcode"];

        if (errCode > 0)
        {
            throw new IBCException(dr.Outputs["errmsg"].ToString());
        }
    }

    public static void CustomerUpdateClosedWithSyncCS(int accid, int subAccId, bool isClosed, bool isCustSync, string Ip)
    {
        CustomerUpdateClosed(accid, subAccId, isClosed, Ip);

        // SynCS
        if (isCustSync)
        {
            CasinoLibrary.SBAgent.Customer cust = new CasinoLibrary.SBAgent.Customer();

            try // Casino cheat
            {
                cust.UpdateCustClosed(accid, isClosed, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.Username : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName, 2);
            }
            catch { }
        }
    }

    /// <summary>
    /// Update suspended status for customer
    /// sp: Acc_CustomerUpdClosed
    /// @Vinh
    /// </summary>
    /// <param name="accid"></param>
    /// <param name="custid"></param>
    /// <param name="isClosed"></param>
    public static void CustomerUpdateSuspend(int accid, int subAccId, bool isSuspended, string Ip)
    {
        Hashtable param = new Hashtable();
        param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        param.Add("custid", accid);
        param.Add("subaccid", subAccId);
        param.Add("suspended", isSuspended);
        param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        param.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino);
        param.Add("errcode", 0);
        param.Add("errmsg", string.Empty);
        param.Add("ip", Ip);
        DbResult dr = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdSuspended", param);

        int errCode = (int)dr.Outputs["errcode"];

        if (errCode > 0)
        {
            throw new IBCException(dr.Outputs["errmsg"].ToString());
        }
    }

    public static void CustomerUpdateSuspendWithSyncCS(int accid, int subAccId, bool isSuspended, bool isCustSync, string Ip)
    {
        CustomerUpdateSuspend(accid, subAccId, isSuspended, Ip);

        // SynCS
        if (isCustSync)
        {
            CasinoLibrary.SBAgent.Customer cust = new CasinoLibrary.SBAgent.Customer();
            try // Casino cheat
            {
                cust.UpdateCustSuspend(accid, isSuspended, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.Username : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName, 2);
            }
            catch { }
        }
    }

    public static void CustomerUpdateDisableOutright(int accid, int subAccId, bool isDisableOutright, string Ip)
    {
        Hashtable param = new Hashtable();
        param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        param.Add("custid", accid);
        param.Add("subaccid", subAccId);
        param.Add("disableoutright", isDisableOutright);
        param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        param.Add("errcode", 0);
        param.Add("errmsg", string.Empty);
        param.Add("ip", Ip);
        DbResult dr = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdDisableOutright", param);
        if ((int)dr.Outputs["errcode"] > 0)
            throw new IBCException(dr.Outputs["errmsg"].ToString());
    }

    public static void CustomerUpdateDisableNumber(int accid, int subAccId, bool isAllowNumberGame, bool isDownLineNumberGame, string Ip)
    {
        Hashtable param = new Hashtable();
        param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        param.Add("custid", accid);
        param.Add("subaccid", subAccId);
        param.Add("disablenumber", isAllowNumberGame);
        param.Add("disabledownline", isDownLineNumberGame);
        param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        param.Add("errcode", 0);
        param.Add("errmsg", string.Empty);
        param.Add("ip", Ip);
        DbResult dr = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdDisableNumber", param);
        if ((int)dr.Outputs["errcode"] > 0)
            throw new IBCException(dr.Outputs["errmsg"].ToString());
    }

    public static void AllowAutoMasterPT(int accid, int subAccId, bool allowautomaster, string Ip)
    {
        Hashtable param = new Hashtable();
        param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        param.Add("custid", accid);
        param.Add("subaccid", subAccId);
        param.Add("disableautompt", allowautomaster);
        param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        param.Add("errcode", 0);
        param.Add("errmsg", string.Empty);
        param.Add("ip", Ip);
        DbResult dr = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdMaster_DisableAutoMPT", param);
        if ((int)dr.Outputs["errcode"] > 0)
            throw new IBCException(dr.Outputs["errmsg"].ToString());
    }

    public static void CustomerUpdateDisabledHR(int subAccId,
       int accid, int custid, bool disabledHR, string Ip, bool isUpdDownline, out string errMsg)
    {
        Hashtable param = new Hashtable();
        param.Add("userid", accid);
        param.Add("custid", custid);
        param.Add("disablehorseracing", disabledHR);
        param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        param.Add("errcode", 0);
        param.Add("errmsg", string.Empty);
        param.Add("ip", Ip);
        param.Add("disabledownline", isUpdDownline);
        param.Add("subaccid", subAccId);

        DbResult dr = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdDisableHR", param);

        errMsg = dr.Outputs["errmsg"].ToString();
        if ((int)dr.Outputs["errcode"] > 0)
            throw new IBCException(errMsg);
    }

    public static void CustomerUpdateDisabledOpenTote(int subAccId,
   int accid, int custid, bool disabledOpenTote, string Ip, bool isUpdDownline, out string errMsg)
    {
        Hashtable param = new Hashtable();
        param.Add("userid", accid);
        param.Add("custid", custid);
        param.Add("disableopentote", disabledOpenTote);
        param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        param.Add("errcode", 0);
        param.Add("errmsg", string.Empty);
        param.Add("ip", Ip);
        param.Add("disabledownline", isUpdDownline);
        param.Add("subaccid", subAccId);

        DbResult dr = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdDisableOpenTote", param);

        errMsg = dr.Outputs["errmsg"].ToString();
        if ((int)dr.Outputs["errcode"] > 0)
            throw new IBCException(errMsg);
    }

    public static void CustomerUpdateDisabledFinancial(int accid, int custid, int subAccId, bool disabledFinancial, string Ip, bool isUpdDownline, out string errMsg)
    {
        Hashtable param = new Hashtable();
        param.Add("userid", accid);
        param.Add("custid", custid);
        param.Add("subaccid", subAccId);
        param.Add("disablefinancial", disabledFinancial);
        param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        param.Add("errcode", 0);
        param.Add("errmsg", string.Empty);
        param.Add("ip", Ip);
        param.Add("disabledownlinefi", isUpdDownline);

        DbResult dr = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdDisableFI", param);

        errMsg = dr.Outputs["errmsg"].ToString();

        if ((int)dr.Outputs["errcode"] > 0)
            throw new IBCException(errMsg);
    }

    public static int CustSubPwdUpd(
        int custid,
        int subaccid,
        string userpwd,
        string language,
        int loginSubAccId,
        out string errMsg
        )
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();
        try
        {
            htb.Add("subaccid", subaccid);
            htb.Add("custid", custid);
            htb.Add("language", language);
            htb.Add("userpwd", Encryption.CfsEnCode(userpwd));
            htb.Add("loginsubaccid", loginSubAccId);
            htb.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
            htb.Add("errcode", 0);
            htb.Add("errmsg", string.Empty);
            htb.Add("loginip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustSubPwdUpd", htb);
        }
        catch (Exception ex)
        {
            string str = ex.Message;
            Sunplus.Agent.Utilities.Logs.Write(ex);
            str = Common.NotAuthorized(str);
            throw new Exception(str);
        }

        errMsg = CastAide.AsString(dbr.Outputs["errmsg"]);
        errMsg = Common.NotAuthorized(errMsg);

        return CastAide.AsInt32(dbr.Outputs["errcode"]);
    }

    public static int CustSubUpdInfo(
        int subaccid,
        int custid,
        string firstname,
        string lastname,
        bool closed,
        string language,
        int loginSubAccId,
        out string errMsg)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("subaccid", subaccid);
        htb.Add("custid", custid);
        htb.Add("firstname", firstname);
        htb.Add("lastname", lastname);
        htb.Add("closed", closed);
        htb.Add("success", 0);
        htb.Add("loginsubaccid", loginSubAccId);
        htb.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        htb.Add("errcode", 0);
        htb.Add("errmsg", string.Empty);
        htb.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        htb.Add("loginip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustSubUpdInfo", htb);

        errMsg = CastAide.AsString(dbr.Outputs["errmsg"]);

        return CastAide.AsInt32(dbr.Outputs["errcode"]);
    }

    public static bool CustSubIns(
        out int subaccid,
        int custid,
        string username,
        string userpwd,
        string firstname,
        string lastname,
        string PageAccess,
        string language,
        int loginSubAccID,
        out int errCode,
        out string errMsg)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("subaccid", 0);
        htb.Add("custid", custid);
        htb.Add("username", username);
        htb.Add("userpwd", Encryption.CfsEnCode(userpwd));
        htb.Add("firstname", firstname);
        htb.Add("lastname", lastname);
        htb.Add("pageaccess", PageAccess);
        htb.Add("success", 0);
        htb.Add("loginsubaccid", loginSubAccID);
        htb.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        htb.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        htb.Add("errcode", 0);
        htb.Add("errmsg", string.Empty);
        htb.Add("loginip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustSubIns", htb);

        subaccid = CastAide.AsInt32(dbr.Outputs["subaccid"]);
        errCode = CastAide.AsInt32(dbr.Outputs["errcode"]);
        errMsg = CastAide.AsString(dbr.Outputs["errmsg"]);

        return CastAide.AsBoolean(dbr.Outputs["success"]);
    }

    public static DbResult GetCustSubSelInfo(string custid, string subaccid)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("custid", custid);
        htb.Add("subaccid", subaccid);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustSubSelInfo", htb);

        return dbr;
    }

    public static DbResult GetSeLMaster_MemberComm(string idsup)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        if (string.IsNullOrEmpty(idsup))
            idsup = "0";
        htb.Add("srecommend", idsup);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelMAgentList", htb);

        return dbr;
    }

    public static DbResult GetSeLMaster_MemberCommCasino(string idsup)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        if (string.IsNullOrEmpty(idsup))
            idsup = "0";
        htb.Add("srecommend", idsup);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelMAgentList_Cas", htb);

        return dbr;
    }


    public static DbResult GetCustSubSel(string custid)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("custid", custid);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustSubSel", htb);

        return dbr;
    }

    public static DbResult GetSeLAgent_MemberComm(string idmas)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        if (string.IsNullOrEmpty(idmas))
            idmas = "0";
        htb.Add("mrecommend", idmas);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelAgentList", htb, 600);

        return dbr;
    }

    public static DbResult GetSeLAgent_MemberCommCasino(string idmas)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        if (string.IsNullOrEmpty(idmas))
            idmas = "0";
        htb.Add("mrecommend", idmas);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelAgentList_Cas", htb, 600);

        return dbr;
    }

    public static DbResult GetDataAgent_WithMaster_MemberComm(string idmas)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        if (string.IsNullOrEmpty(idmas))
        {
            idmas = "0";
        }
        htb.Add("mrecommend", idmas);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelComm_MAg", htb, 600);

        return dbr;
    }

    public static DbResult GetDataAgent_WithMaster_MemberCommForRacing(string idmas)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        if (string.IsNullOrEmpty(idmas))
        {
            idmas = "0";
        }
        htb.Add("mrecommend", idmas);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelRBComm_MAg", htb, 600);

        return dbr;
    }

    public static DbResult GetDataAgent_WithAgent_MemberComm(string idage)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("recommend", idage);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelComm_Ag", htb, 600);

        return dbr;
    }

    public static DbResult GetDataAgent_WithAgent_MemberCommForRacing(string idage)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("recommend", idage);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelRBComm_Ag", htb, 600);

        return dbr;
    }

    public static DbResult GetData_PositionTaking(string custid)
    {
        int intCustID = CastAide.AsInt32(custid);
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("recommend", intCustID);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelPositionTakingByAg", htb, 600);
        return dbr;
    }

    public static DbResult GetData_PositionTakingRacing(string custid)
    {
        int intCustID = CastAide.AsInt32(custid);
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("recommend", intCustID);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelRBPositionTakingByAg", htb, 600);
        return dbr;
    }

    public static DbResult GetData_PositionTakingNumberGame(string custid)
    {
        int intCustID = CastAide.AsInt32(custid);
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("recommend", intCustID);
        dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelNumberGamePositionTakingByAg", htb, 600);
        return dbr;
    }

    public static DbResult GetData_Upline_PositionTaking(string roleid, string custid)
    {
        int intCustID = CastAide.AsInt32(custid);
        int intRoleID = CastAide.AsInt16(roleid);
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        if (intRoleID == 4)
        {
            htb.Add("srecommend", intCustID);
            dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelPositionTakingBySAg", htb, 600);
        }
        else
        {
            htb.Add("mrecommend", intCustID);
            dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelPositionTakingByMAg", htb, 600);
        }

        return dbr;
    }

    public static DbResult GetData_Upline_PositionTakingRacing(string roleid, string custid)
    {
        int intCustID = CastAide.AsInt32(custid);
        int intRoleID = CastAide.AsInt16(roleid);
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        if (intRoleID == 4)
        {
            htb.Add("srecommend", intCustID);
            dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelRBPositionTakingBySAg", htb, 600);
        }
        else
        {
            htb.Add("mrecommend", intCustID);
            dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelRBPositionTakingByMAg", htb, 600);
        }

        return dbr;
    }

    public static DbResult GetData_Upline_PositionTakingNumberGame(string roleid, string custid)
    {
        int intCustID = CastAide.AsInt32(custid);
        int intRoleID = CastAide.AsInt16(roleid);
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        if (intRoleID == 4)
        {
            htb.Add("srecommend", intCustID);
            dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelNumberGamePositionTakingBySAg", htb, 600);
        }
        else
        {
            htb.Add("mrecommend", intCustID);
            dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelNumberGamePositionTakingByMAg", htb, 600);
        }

        return dbr;
    }

    public static DbResult GetDataCreditBalance(string custid, string site_list, int pageSize, int pageIndex, ref int totalPages, string username, int status)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("pagesize", AppSettings.PageSize);
        htb.Add("custid", custid);
        htb.Add("pageindex", pageIndex);
        htb.Add("username", username);
        htb.Add("totalpages", totalPages);
        if (status == 1 || status == 3)
        {
            htb.Add("closed", (status == 1 ? 0 : 1));
        }
        else if (status == 2)
        {
            htb.Add("suspended", 1);
        }
        else if (status == 4)
        {
            htb.Add("enabled", 0);
        }

        if (AppSettings.IsAgentSite)
        {
            dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelCreditBalanceList_Ag", htb, 600);
        }
        else if (AppSettings.IsMasterSite)
        {
            if (site_list == "MemberList")
            {
                dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelCreditBalanceList_Ag", htb, 600);
            }
            else
            {
                dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelCreditBalanceList_Mag", htb, 600);
            }
        }
        else
        {
            if (site_list == "AgentList")
            {
                dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelCreditBalanceList_Mag", htb, 600);
            }
            else if (site_list == "MemberList")
            {
                dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelCreditBalanceList_Ag", htb, 600);
            }
            else
            {
                dbr = DNAide.Data.DbHelper.Execute("Age_CustomerSelCreditBalanceList_Sag", htb, 600);
            }
        }

        totalPages = (int)dbr.Outputs["totalpages"];

        return dbr;
    }

    public static DbResult GetOptionTransferCondition(string custid)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("custid", custid);
        htb.Add("transferoption", "");
        dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerSelTransferOption", htb);

        return dbr;
    }

    public static DbResult GetDataCustomerList(string custid, string username, ref int totalPage, int pageindex, int status) // for agent
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("recommend", custid);
        htb.Add("isagentsite", 0);
        htb.Add("pageindex", pageindex);
        htb.Add("totalpages", totalPage);
        htb.Add("username", username);
        htb.Add("pagesize", AppSettings.PageSize);
        if (status == 1 || status == 3)
        {
            htb.Add("closed", (status == 1 ? 0 : 1));
        }
        else if (status == 2)
        {
            htb.Add("suspended", 1);
        }
        else if (status == 4)
        {
            htb.Add("enabled", 0);
        }
        dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerSelByAg", htb, 600);
        totalPage = CastAide.AsInt32(dbr.Outputs["totalpages"], 0);

        return dbr;
    }

    public static DbResult GetDataMagentList(string custid, string username, ref int totalPage, int pageindex, int status) // for master
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("mrecommend", custid);
        htb.Add("isagentsite", 0);
        htb.Add("pageindex", pageindex);
        htb.Add("totalpages", totalPage);
        htb.Add("pagesize", AppSettings.PageSize);
        htb.Add("username", username);
        if (status == 1 || status == 3)
        {
            htb.Add("closed", (status == 1 ? 0 : 1));
        }
        else if (status == 2)
        {
            htb.Add("suspended", 1);
        }
        else if (status == 4)
        {
            htb.Add("enabled", 0);
        }
        dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerSelByMAg", htb, 600);
        totalPage = CastAide.AsInt32(dbr.Outputs["totalpages"], 0);

        return dbr;
    }

    public static DbResult GetDataSagentList(string custid, string username, ref int totalPage, int pageindex, int status) // for super
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("srecommend", custid);
        htb.Add("isagentsite", 0);
        htb.Add("pageindex", pageindex);
        htb.Add("totalpages", totalPage);
        htb.Add("username", username);
        htb.Add("pagesize", AppSettings.PageSize);
        if (status == 1 || status == 3)
        {
            htb.Add("closed", (status == 1 ? 0 : 1));
        }
        else if (status == 2)
        {
            htb.Add("suspended", 1);
        }
        else if (status == 4)
        {
            htb.Add("enabled", 0);
        }
        dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerSelBySAg", htb, 600);
        totalPage = CastAide.AsInt32(dbr.Outputs["totalpages"], 0);

        return dbr;
    }

    public static DbResult ChangePass(string custid, string userpwd)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("custid", custid);
        htb.Add("userpwd", Encryption.CfsEnCode(userpwd));
        htb.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino);
        htb.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        htb.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        htb.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
        htb.Add("loginip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        htb.Add("errcode", 0);
        htb.Add("errmsg", "");
        dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustomerUpdPwd", htb);

        return dbr;
    }

    public static bool UpdateAgentMaxCredit(int custid, double newMaxCredit, ref string errmsg)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();
        htb.Add("custid", custid);
        htb.Add("maxcredit", newMaxCredit);
        htb.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        htb.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
        htb.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        htb.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        htb.Add("errcode", 0);
        htb.Add("errmsg", "");
        dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustomerUpdMaxCredit", htb);

        errmsg = dbr.Outputs["errmsg"].ToString();
        return (!String.IsNullOrEmpty(errmsg)); //ok = 0; error = 1;
    }

    public static bool UpdateCreditBalance(string custid, string site, double amt, ref string errmsg)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("custid", custid);
        htb.Add("creditamt", amt);
        htb.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        htb.Add("creditproblem", 0);
        htb.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        htb.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
        htb.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        htb.Add("errmsg", "");
        if (AppSettings.IsAgentSite)
        {
            dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustomerUpdCredit_Ag", htb);
        }
        else if (AppSettings.IsMasterSite)
        {
            if (site == "MemberList")
            {
                dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustomerUpdCredit_Ag", htb);
            }
            else
            {
                dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustomerUpdCredit_MAg", htb);
            }
        }
        else
        {
            if (site == "AgentList")
            {
                dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustomerUpdCredit_MAg", htb);
            }
            else if (site == "MemberList")
            {
                dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustomerUpdCredit_Ag", htb);
            }
            else
            {
                dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustomerUpdCredit_SAg", htb);
            }
        }

        errmsg = dbr.Outputs["errmsg"].ToString();
        return CastAide.AsBoolean(dbr.Outputs["creditproblem"]);
    }

    public static string GetDataTransferCondition(string custid)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("custid", custid);
        htb.Add("transferoption", "00000000");
        dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerSelTransferOption", htb);

        return dbr.Outputs["transferoption"].ToString();
    }

    public static DbResult UpdateTransferCondition(string custid, string transferoption)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("custid", custid);
        htb.Add("transferoption", transferoption);
        htb.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
        htb.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
        htb.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        htb.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        htb.Add("returnerrcode", 1);
        htb.Add("errcode", 0);
        htb.Add("errmsg", "");
        dbr = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdTransferOption", htb);

        return dbr;
    }

    public static bool CustomerCheckEnable(string custid, out string errmsg)
    {
        DbResult dbr = null;
        Hashtable htb = new Hashtable();

        htb.Add("custid", custid);
        htb.Add("errcode", 0);
        htb.Add("errmsg", "");
        dbr = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerSelCheckEnable", htb);
        errmsg = dbr.Outputs["errmsg"] + "";
        return string.IsNullOrEmpty(errmsg);
    }
}