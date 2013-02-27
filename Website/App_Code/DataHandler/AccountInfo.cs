using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using DNAide.Core;
using DNAide.Utils;
using Fanex.Data.DNAide;
using Sunplus.Agent.Utilities;

/// <summary>
/// Summary description for AccountInfo
/// </summary>
public class AccountInfo
{
    public static bool IsSimple(int custid)
    {
        bool isSimple = false;
        Hashtable htb = new Hashtable();
        htb.Add("custid", custid);
        htb.Add("issimple", 0);
        DbResult dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerSelMember_IsSimple", htb);
        isSimple = CastAide.AsBoolean(dbr.Outputs["issimple"]);
        return isSimple;
    }

    public static DbResult MessageSel(byte RoleID)
    {
        Hashtable htb = new Hashtable();
        htb.Add("roleid", RoleID);
        return DNAide.Data.DbHelper.Execute("Age_MessageSelOne", htb);
    }

    public static DbResult MessageHistorySel(byte RoleID)
    {
        Hashtable htb = new Hashtable();
        htb.Add("roleid", RoleID);
        return DNAide.Data.DbHelper.Execute("Age_MessageSelAll", htb);
    }

    public static DbResult MessageHistorySelByType(byte RoleID, DateTime Fdate, DateTime Tdate, string filteringType, ref string first)
    {
        Hashtable htb = new Hashtable();
        htb.Add("roleid", RoleID);
        htb.Add("fdate", Fdate);
        htb.Add("tdate", Tdate);
        htb.Add("type", filteringType);
        htb.Add("first", string.Empty);
        DbResult result = DNAide.Data.DbHelper.Execute("Age_MessageSelByType", htb);
        first = (string)result.Outputs["first"];
        return result;
    }

    public static string UpdatePassword(string OldPassword, string NewPassword)
    {
        string strResult = "";
        try
        {
            Hashtable p = new Hashtable();
            DbResult dbr = null;
            p.Add("custid", Sunplus.Agent.Model.Entities.UserSession.Current.CustId);
            p.Add("userpwd", NewPassword);
            p.Add("oldpwd", OldPassword);
            p.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
            p.Add("loginip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
            p.Add("errcode", 0);
            p.Add("errmsg", "");
            if (CastAide.AsInt32(Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId) == 0)
            {
                p.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino);
                p.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
                dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustomerPwdUpd2", p);
            }
            else
            {
                p.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
                dbr = DNAide.Data.DbHelper.ExecuteCmd("Age_CustSubPwdUpd2", p);
            }
            if (CastAide.AsInt32(dbr.Outputs["errcode"]) == 0)
            {
                strResult = "Ok";
                if (!Sunplus.Agent.Model.Entities.UserSession.Current.IsLogged)
                {
                    Sunplus.Agent.Model.Entities.UserSession.Current.IsLogged = true;
                    strResult = "changed";
                }
            }
            else
            {
                strResult = dbr.Outputs["errmsg"].ToString();
            }
        }
        catch (Exception ex)
        {
            strResult = ex.Message;
        }
        return strResult;
    }

    public static DbResult Acc_CustomerSelSAgentInfo(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelSAgentInfo", hash);
    }

    public static DbResult Acc_CustomerInsMember_SAg(Hashtable hash)
    {
        DbResult dbResult = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerInsMember_SAg", hash, 600);
        return dbResult;
    }

    public static DbResult Acc_CustomerSelSEditCustomer(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelSEditCustomer", hash, 600);
    }

    public static DbResult Acc_CustomerUpdEditCustomer_SAg(Hashtable hash)
    {
        DbResult dbResult = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdMaster_All", hash, 600);
        return dbResult;
    }

    public static DbResult Acc_CustomerSelComEditOverride_MAg(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelComEditOverride_MAg", hash, 600);
    }

    public static DbResult Acc_CustomerSelComEditOverride_Ag(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelComEditOverride_Ag", hash, 600);
    }

    public static DbResult Acc_CustOverrideUpd(Hashtable hash)
    {
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustOverrideUpd", hash, 600);
    }

    public static DbResult Acc_CustomerSelMAgentInfo(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelMAgentInfo", hash, 600);
    }

    public static DbResult Acc_CustomerSelTransferOption(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelTransferOption", hash, 600);
    }

    public static DbResult Acc_CustomerInsMember_MAg(Hashtable hash)
    {
        DbResult dbResult = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerInsMember_MAg", hash, 600);
        return dbResult;
    }

    public static DbResult Acc_CustomerSelMEditCustomer(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelMEditCustomer", hash, 600);
    }

    public static DbResult Acc_CustomerUpdEditCustomer_MAg(Hashtable hash)
    {
        DbResult dbResult = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdAgent_All", hash, 600);
        return dbResult;
    }

    public static DbResult Acc_CustomerSelAgentInfo(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelAgentInfo", hash, 600);
    }

    public static DbResult Acc_CustomerSelCustomerInfo(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelCustomerInfo", hash, 600);
    }

    public static DbResult Acc_CustomerInsMember_Ag(Hashtable hash)
    {
        DbResult dbResult = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerInsMember_Ag", hash, 600);
        return dbResult;
    }

    public static DbResult Acc_CustomerUpdEditCustomer_Ag(Hashtable hash)
    {
        DbResult dbResult = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdMember_All", hash, 600);
        return dbResult;
    }

    public static DbResult Acc_CustomerSelEditCustomer(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelEditCustomer", hash, 600);
    }

    public static DbResult Acc_CustomerSelEditCustomer_SAg(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelEditCustomer_SAg", hash, 600);
    }

    public static DbResult Acc_CustomerSelSAgentInfo_Multiple(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelSAgentInfo_Multiple", hash, 600);
    }

    public static DbResult Acc_CustomerUpdSuper_Member(Hashtable hash)
    {
        DbResult dbResult = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdMember_Super", hash, 600);
        return dbResult;
    }

    public static DbResult Acc_CustomerSelEditCustomer_MAg(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelEditCustomer_MAg", hash, 600);
    }

    public static DbResult Acc_CustomerSelMAgentInfo_Multiple(Hashtable hash)
    {
        return DNAide.Data.DbHelper.Execute("Acc_CustomerSelMAgentInfo_Multiple", hash, 600);
    }

    public static DbResult Acc_CustomerUpdMaster_Member(Hashtable hash)
    {
        DbResult dbResult = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdMember_Master", hash, 600);
        return dbResult;
    }

    public static DbResult Acc_CustomerUpdUserInfo(Hashtable hash)
    {
        hash.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);// 'int
        hash.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);//'int
        hash.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino);
        hash.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        hash.Add("errcode", 0);//'int
        hash.Add("errmsg", "");//'nvarchar
        hash.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdUserInfo", hash, 600);
    }

    public static DbResult Acc_CustomerUpdCredit(Hashtable hash)
    {
        hash.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);// 'int
        hash.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);//'int
        hash.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        hash.Add("errcode", 0);//'int
        hash.Add("errmsg", "");//'nvarchar
        hash.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdCredit", hash, 600);
    }

    public static DbResult Acc_CustomerUpdSuspended(Hashtable hash)
    {
        hash.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);// 'int
        hash.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);//'int
        hash.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        hash.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino);
        hash.Add("errcode", 0);//'int
        hash.Add("errmsg", "");//'nvarchar
        hash.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdSuspended", hash, 600);
    }

    public static DbResult Acc_CustomerUpdClosed(Hashtable hash)
    {
        hash.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);// 'int
        hash.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);//'int
        hash.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        hash.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino);
        hash.Add("errcode", 0);//'int
        hash.Add("errmsg", "");//'nvarchar
        hash.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdClosed", hash, 600);
    }

    public static DbResult Acc_CustomerUpdMaster_Commission(Hashtable hash)
    {
        hash.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);// 'int
        hash.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);//'int
        hash.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        hash.Add("errcode", 0);//'int
        hash.Add("errmsg", "");//'nvarchar
        hash.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdMaster_Commission", hash, 600);
    }

    public static DbResult Acc_CustomerUpdAgent_Commission(Hashtable hash)
    {
        hash.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);// 'int
        hash.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);//'int
        hash.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        hash.Add("errcode", 0);//'int
        hash.Add("errmsg", "");//'nvarchar
        hash.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdAgent_Commission", hash, 600);
    }

    public static DbResult Acc_CustomerUpdMember_Commission(Hashtable hash)
    {
        hash.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);// 'int
        hash.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);//'int
        hash.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        hash.Add("errcode", 0);//'int
        hash.Add("errmsg", "");//'nvarchar
        hash.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdMember_Commission", hash, 600);
    }

    public static DbResult Acc_CustomerUpdMaster_MinMaxBet(Hashtable hash)
    {
        hash.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);// 'int
        hash.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);//'int
        hash.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        hash.Add("errcode", 0);//'int
        hash.Add("errmsg", "");//'nvarchar
        hash.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdMaster_MinMaxBet", hash, 600);
    }

    public static DbResult Acc_CustomerUpdAgent_MinMaxBet(Hashtable hash)
    {
        hash.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);// 'int
        hash.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);//'int
        hash.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
        hash.Add("errcode", 0);//'int
        hash.Add("errmsg", "");//'nvarchar
        hash.Add("ip", Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP());
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdAgent_MinMaxBet", hash, 600);
    }

    public static DbResult Acc_CustomerUpdMaster_PositionTaking(Hashtable hash)
    {
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdMaster_PositionTaking", hash, 600);
    }

    public static DbResult Acc_CustomerUpdAgent_PositionTaking(Hashtable hash)
    {
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdAgent_PositionTaking", hash, 600);
    }

    public static DbResult Acc_CustomerUpdMaster_All(Hashtable hash)
    {
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdMaster_All", hash, 600);
    }

    public static DbResult Acc_CustomerUpdAgent_All(Hashtable hash)
    {
        return DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdAgent_All", hash, 600);
    }

    public static DbResult GET_MessageNoticeSelByCustID(int custID)
    {
        Hashtable hash = new Hashtable();
        hash.Add("custid", custID);
        return DNAide.Data.DbHelper.Execute("Age_MessageNoticeSelByCustID", hash);
    }

    public static DbResult SET_MessageNoticeUpdStatus(int custID, bool isViewed)
    {
        Hashtable hash = new Hashtable();
        hash.Add("custid", custID);
        hash.Add("isviewed", isViewed);
        return DNAide.Data.DbHelper.ExecuteCmd("Age_MessageNoticeUpdStatus", hash);
    }

    public static DbResult Age_CustomerSelStatus(int custId)
    {
        Hashtable param = new Hashtable();
        param.Add("custid", custId);
        DbResult ds = DNAide.Data.DbHelper.Execute("Acc_CustomerSelStatus", param);
        return ds;
    }

    public static DbResult GetCustList(int custId, int pageSize, int pageIndex, ref int totalPages)
    {
        Hashtable param = new Hashtable();
        param.Add("custid", custId);
        param.Add("pagesize", pageSize);
        param.Add("pageindex", pageIndex);
        param.Add("totalpages", totalPages);
        DbResult rs = DNAide.Data.DbHelper.Execute("Acc_CustomerSelList", param);
        totalPages = CastAide.AsInt32(rs.Outputs["totalpages"]);
        return rs;
    }

    /// <summary>
    /// SP: Agw_SelCustomerGivenCredit
    /// </summary>
    /// <param name="custid"></param>
    /// <returns></returns>
    public static decimal GetCustomerGivenCredit(string custid/*, out string roleid*/)
    {
        Hashtable param = new Hashtable();
        param.Add("custid", custid);
        DbResult dr = DNAide.Data.DbHelper.Execute("Acc_SelCustomerGivenCredit", param);
        //roleid = dr.ResultSet.Tables[0].Rows[0]["roleid"].ToString();
        return Math.Round(decimal.Parse(dr.ResultSet.Tables[0].Rows[0]["givencredit"].ToString()));
    }

    public static int ClearCreditByCustID(int custID, int subAccID, int userID, string IP, string languageCode, out string errorMsg)
    {
        Hashtable parameters = new Hashtable();
        parameters.Add("custid", custID);
        parameters.Add("subaccid", subAccID);
        parameters.Add("userid", userID);
        parameters.Add("ip", IP);
        parameters.Add("language", languageCode);
        parameters.Add("errcode", 0);
        parameters.Add("errmsg", string.Empty);

        DbResult dbResult = DNAide.Data.DbHelper.Execute("Age_CustomerUpdClearCredit", parameters);
        Hashtable outputs = dbResult.Outputs;
        int errorCode = (int)(outputs["errcode"]);
        errorMsg = outputs["errmsg"] as string;
        return errorCode;
    }
}