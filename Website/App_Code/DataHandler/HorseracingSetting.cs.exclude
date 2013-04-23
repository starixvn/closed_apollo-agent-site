using System;
using System.Collections;
using System.Data;
using Fanex.Data.DNAide;

using Sunplus.Agent.Utilities;

public class HorseracingSetting
{
    public HorseracingSetting()
    {

    }

    /// <summary>
    /// call store procedure Acc_CustomerSelDisableHR
    /// </summary>
    /// <param name="custId">customer id</param>
    /// <returns>Disabled HR status, true, false</returns>
    public static bool IsDisabledHR(int custId)
    {
 
        Hashtable param = new Hashtable();
        param.Add("custid", custId);
        DbResult ds = DNAide.Data.DbHelper.Execute("Acc_CustomerSelDisableHR", param);
        DataRow row1 = ds.ResultSet.Tables[0].Rows[0];
        bool parentDisabledHR = Convert.IsDBNull(row1["DisableHorseracing"])
                                    ? false
                                    : Convert.ToBoolean(row1["DisableHorseracing"]);
        return parentDisabledHR;

    }

    public static DbResult GetAllHRSetting(int custid, int roleid)
    {
        DbResult dsHRBetSetting = null;

        string spName = string.Empty;
        Hashtable param = new Hashtable();
        param.Add("custid", custid);
        switch (roleid)
        {
            case 1:            
                dsHRBetSetting = DNAide.Data.DbHelper.Execute("Acc_CustomerSelHRMember_All", param);
                break;
            case 2:                    
                dsHRBetSetting = DNAide.Data.DbHelper.Execute("Acc_CustomerSelHRAgent_All", param);
                break;
            case 3:                    
                dsHRBetSetting = DNAide.Data.DbHelper.Execute("Acc_CustomerSelHRMaster_All", param);
                break;
            case 4:                    
                dsHRBetSetting = DNAide.Data.DbHelper.Execute("Acc_CustomerSelHRSuper_All", param);
                break;
        }          


        return dsHRBetSetting;
    }

    public static DbResult GetDataSuperViewAgent(int custid)
    {
        Hashtable htb = new Hashtable();
        htb.Add("custid", custid);
        DbResult dbr = null;
        dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerSelHRAgent_SAg", htb);
        return dbr;
    }

    public static Hashtable UpdateCustomerHRSetting(Hashtable hParams)
    {
        Hashtable hResult = new Hashtable();
     
        DbResult ds = null;
        ds = DNAide.Data.DbHelper.ExecuteCmd("Acc_CustomerUpdAllSettingHR", hParams);
        if (ds != null)
        {
            hResult.Add("errCode", ds.Outputs["errcode"]);
            hResult.Add("errMsg", ds.Outputs["errmsg"]);
        }

        return hResult;
    }

    private static bool IsNullDB(DbResult ds)
    {
        if (ds != null && ds.ResultSet.Tables[0].Rows.Count > 0)
            return false;
        return true;
    }
    public static DbResult SuperUpdateMasterHR(Hashtable htb) // Super Update Master
    {
        DbResult dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerUpdMasterHR_All", htb);
        return dbr;
    }
    public static DbResult MasterUpdateAgentHR(Hashtable htb) // Master update Agent
    {
        DbResult dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerUpdAgentHR_All", htb);
        return dbr;
    }
    public static DbResult AgentUpdateMemberHR(Hashtable htb) // Agent update Member
    {
        DbResult dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerUpdMemberHR_All", htb);
        return dbr;
    }
    public static DbResult SuperUpdateMemberHR(Hashtable htb) // Super edit member
    {
        DbResult dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerUpdMemberHR_SAg", htb);
        return dbr;
    }
    public static DbResult MasterUpdateMemberHR(Hashtable htb) // Master edit member
    {
        DbResult dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerUpdMemberHR_MAg", htb);
        return dbr;
    }
    public static DbResult GetDataSuperUpdateMember(int custid, int agentid) // Get data for Super edit member
    {
        Hashtable htb = new Hashtable();
        htb.Add("custid", custid );
        htb.Add("agentid", agentid);
        DbResult dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerSelHREditMember_SAg", htb);
        return dbr;
    }
    public static DbResult GetDataMasterUpdateMember(int custid, int agentid) // Get data for Master edit member
    {
        Hashtable htb = new Hashtable();
        htb.Add("custid", custid);
        htb.Add("agentid", agentid);
        DbResult dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerSelHREditMember_MAg", htb);
        return dbr;
    }
    public static DbResult GetDataSuperEditMinPTHR(int custid) // Get data for Super edit min pt 
    {
        Hashtable htb = new Hashtable();
        htb.Add("custid", custid);
        DbResult dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerSelHRComEditOverride_MAg", htb);
        return dbr;
    }
    public static DbResult GetDataMasterEditMinPTHR(int custid) // Get data for Master edit min pt
    {
        Hashtable htb = new Hashtable();
        htb.Add("custid", custid);
        DbResult dbr = DNAide.Data.DbHelper.Execute("Acc_CustomerSelHRComEditOverride_Ag", htb);
        return dbr;
    }
    public static DbResult UpdateMinPTHR(Hashtable htb) // Update min max for HR
    {
        DbResult dbr = DNAide.Data.DbHelper.Execute("Acc_CustOverrideUpdHR", htb);
        return dbr;
    }
}
