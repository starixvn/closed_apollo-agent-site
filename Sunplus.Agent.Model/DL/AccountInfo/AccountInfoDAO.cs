namespace Sunplus.Agent.Model.DL.AccountInfo
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Web;
    using CasinoLibrary.SBAgent;
    using Fanex.Data.DNAide;
    using Sunplus.Agent.Model.DL.Base;
    using Sunplus.Agent.Utilities;

    public class AccountInfoDAO : DAOBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="query">UserName, First Name, Last Name</param>
        /// <returns></returns>
        public DataTable Age_Customer_GetUserSearch(int userId, string userName, bool isMember)
        {
            Hashtable param = new Hashtable();
            param.Add("agentid", userId);
            param.Add("username", userName);
            param.Add("ismember", isMember);
            
            return ExecSpTable("Age_Customer_GetUserSearch", param);
        }

        public DataTable Acc_CustomerSelList(int custId, int pageSize, int pageIndex, ref int totalPages)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            param.Add("pagesize", pageSize);
            param.Add("pageindex", pageIndex);
            param.Add("totalpages", totalPages);
            DbResult rs = ExecSp("Acc_CustomerSelList", param);
            totalPages = (int)rs.Outputs["totalpages"];
            return rs.ResultSet.Tables[0];
        }

        public DataSet Age_Customer_GetAgentDownLine(int agentID)
        {
            Hashtable param = new Hashtable();
            param.Add("agentid", agentID);
            return ExecSpDataset("Age_Customer_GetAgentDownLine", param);
        }

        public DataSet Age_Customer_GetMembersDownLine(int agentID)
        {
            Hashtable param = new Hashtable();
            param.Add("agentid", agentID);
            return ExecSpDataset("Age_Customer_GetMembersDownLine", param);
        }

        public DataSet Age_Customer_GetAgentDownLinePath(int custID, int agentID)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custID);
            param.Add("agentid", agentID);
            return ExecSpDataset("Age_Customer_GetAgentDownLinePath", param);
        }

        public DataSet Age_CustomerSelAllInfo(int currentAgentId, string userName, bool isMember)
        {
            Hashtable param = new Hashtable();
            param.Add("currentagentid", currentAgentId);
            param.Add("username", userName);
            param.Add("ismember", isMember);

            return ExecSpDataset("Age_Customer_GetAllInfo", param);
        }

        public DataTable Acc_CustomerSelOnline(int custId)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            return ExecSpTable("Acc_CustomerSelOnline", param);
        }

        public DataTable Acc_CustomerSelAgentInfo(int custId)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            return ExecSpTable("Acc_CustomerSelAgentInfo", param, 600);
        }

        public DataTable Acc_CustomerSelMAgentInfo(int custId)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            return ExecSpTable("Acc_CustomerSelMAgentInfo", param, 600);
        }

        public DataTable Acc_CustomerSelSAgentInfo(int custId)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            return ExecSpTable("Acc_CustomerSelSAgentInfo", param, 600);
        }

        public Hashtable Acc_CustomerUpdUserSearch(int userId, string search, string firstName, string lastName)
        {
            Hashtable param = new Hashtable();
            param.Add("userid", userId);
            param.Add("search", search);
            param.Add("firstname", firstName);
            param.Add("lastname", lastName);
            ExecSp("Acc_CustomerUpdUserSearch", param);
            return null;
        }

        /// <summary>
        /// Update Agent credit balance
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="custId"></param>
        /// <param name="amount"></param>
        /// <param name="ip"></param>
        /// <param name="language"></param>
        /// <param name="errorMessage"></param>
        /// <returns>
        ///     Error : 1
        ///     Success : 0
        /// </returns>
        public int Age_CustomerUpdCredit_Ag(int userId, int subAccId, int custId, double amount, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("custid", custId);
            parameters.Add("creditamt", amount);
            parameters.Add("userid", userId);
            parameters.Add("creditproblem", 0);
            parameters.Add("language", languageKey);
            parameters.Add("subaccid", subAccId);
            parameters.Add("ip", ip);
            parameters.Add("errmsg", string.Empty);
            DbResult result = ExecSp("Age_CustomerUpdCredit_Ag", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToBoolean(result.Outputs["creditproblem"]) ? 1 : 0;
        }

        /// <summary>
        /// Update Master credit balance
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="custId"></param>
        /// <param name="amount"></param>
        /// <param name="ip"></param>
        /// <param name="language"></param>
        /// <param name="errorMessage"></param>
        /// <returns>
        ///     Error : 1
        ///     Success : 0
        /// </returns>
        public int Age_CustomerUpdCredit_MAg(int userId, int subAccId, int custId, double amount, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("custid", custId);
            parameters.Add("creditamt", amount);
            parameters.Add("userid", userId);
            parameters.Add("creditproblem", 0);
            parameters.Add("language", languageKey);
            parameters.Add("subaccid", subAccId);
            parameters.Add("ip", ip);
            parameters.Add("errmsg", string.Empty);
            DbResult result = ExecSp("Age_CustomerUpdCredit_MAg", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToBoolean(result.Outputs["creditproblem"]) ? 1 : 0;
        }

        /// <summary>
        /// Update Super credit balance
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="custId"></param>
        /// <param name="amount"></param>
        /// <param name="ip"></param>
        /// <param name="language"></param>
        /// <param name="errorMessage"></param>
        /// <returns>
        ///     Error : 1
        ///     Success : 0
        /// </returns>
        public int Age_CustomerUpdCredit_SAg(int userId, int subAccId, int custId, double amount, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("custid", custId);
            parameters.Add("creditamt", amount);
            parameters.Add("userid", userId);
            parameters.Add("creditproblem", 0);
            parameters.Add("language", languageKey);
            parameters.Add("subaccid", subAccId);
            parameters.Add("ip", ip);
            parameters.Add("errmsg", string.Empty);
            DbResult result = ExecSp("Age_CustomerUpdCredit_SAg", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToBoolean(result.Outputs["creditproblem"]) ? 1 : 0;
        }

        /// <summary>
        /// Update max credit
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="subAccountId"></param>
        /// <param name="custid"></param>
        /// <param name="newMaxCredit"></param>
        /// <param name="ip"></param>
        /// <param name="languageKey"></param>
        /// <param name="errorMessage"></param>
        /// <returns>
        /// </returns>
        public int Age_CustomerUpdMaxCredit(int userId, int subAccountId, int custId, double newMaxCredit, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("maxcredit", newMaxCredit);
            parameters.Add("userid", userId);
            parameters.Add("subaccid", subAccountId);
            parameters.Add("language", languageKey);
            parameters.Add("ip", ip);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            DbResult result = ExecSp("Age_CustomerUpdMaxCredit", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update Open/Closed status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="isClosed"></param>
        /// <param name="disabledCasino"></param>
        /// <param name="ip"></param>
        /// <param name="languageKey"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdClosed(int custId, int userId, int subAccId, bool isClosed, bool disabledCasino, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("userid", userId);
            parameters.Add("custid", custId);
            parameters.Add("subaccid", subAccId);
            parameters.Add("closed", isClosed);
            parameters.Add("language", languageKey);
            parameters.Add("discasino", disabledCasino);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("ip", ip);
            DbResult result = ExecSp("Acc_CustomerUpdClosed", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update suspended status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="isSuspended"></param>
        /// <param name="disabledCasino"></param>
        /// <param name="ip"></param>
        /// <param name="languageKey"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdSuspended(int custId, int userId, int subAccId, bool isSuspended, bool disabledCasino, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("userid", userId);
            parameters.Add("custid", custId);
            parameters.Add("subaccid", subAccId);
            parameters.Add("suspended", isSuspended);
            parameters.Add("language", languageKey);
            parameters.Add("discasino", disabledCasino);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("ip", ip);

            DbResult result = ExecSp("Acc_CustomerUpdSuspended", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update disable outright status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="isDisableOutright"></param>
        /// <param name="ip"></param>
        /// <param name="languageKey"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdDisableOutright(int custId, int userId, int subAccId, bool isDisableOutright, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("userid", userId);
            parameters.Add("custid", custId);
            parameters.Add("subaccid", subAccId);
            parameters.Add("disableoutright", isDisableOutright);
            parameters.Add("language", languageKey);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("ip", ip);

            DbResult result = ExecSp("Acc_CustomerUpdDisableOutright", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update disabled HR status for customer
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="disabledHR"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="languageKey"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdDisable151_31(int custId, int userId, int subAccId, bool disabledHR, bool updateDownline, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("userid", userId);
            parameters.Add("custid", custId);
            parameters.Add("disable151_31", disabledHR);
            parameters.Add("language", languageKey);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("ip", ip);
            parameters.Add("disabledownlines151_31", updateDownline);
            parameters.Add("subaccid", subAccId);

            DbResult result = ExecSp("Acc_CustomerUpdDisable151_31", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update open tote status for customer
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="disabledOpenTote"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="laguageKey"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdDisable151_41(int custId, int userId, int subAccId, bool disabledOpenTote, bool updateDownline, string ip, string laguageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("userid", userId);
            parameters.Add("custid", custId);
            parameters.Add("disable151_41", disabledOpenTote);
            parameters.Add("language", laguageKey);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("ip", ip);
            parameters.Add("disabledownlines151_41", updateDownline);
            parameters.Add("subaccid", subAccId);
            DbResult result = ExecSp("Acc_CustomerUpdDisable151_41", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update AU Tote status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="disabledAUTote"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="laguageKey"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdDisable151_51(int custId, int userId, int subAccId, bool disabledAUTote, bool updateDownline, string ip, string laguageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("disable151_51", disabledAUTote);
            parameters.Add("disabledownlines151_51", updateDownline);
            parameters.Add("subaccid", subAccId);
            parameters.Add("userid", userId);
            parameters.Add("language", laguageKey);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("ip", ip);
            DbResult result = ExecSp("Acc_CustomerUpdDisable151_51", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update GreyHoundUK status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="disabledGreyHoundUK"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="laguageKey"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdDisable152_41(int custId, int userId, int subAccId, bool disabledGreyHoundUK, bool updateDownline, string ip, string laguageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("disable152_41", disabledGreyHoundUK);
            parameters.Add("disabledownlines152_41", updateDownline);
            parameters.Add("subaccid", subAccId);
            parameters.Add("userid", userId);
            parameters.Add("language", laguageKey);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("ip", ip);
            DbResult result = ExecSp("Acc_CustomerUpdDisable152_41", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update GreyHoundAU status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="disabledGreyHoundAU"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="laguageKey"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdDisable152_51(int custId, int userId, int subAccId, bool disabledGreyHoundAU, bool updateDownline, string ip, string laguageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("disable152_51", disabledGreyHoundAU);
            parameters.Add("disabledownlines152_51", updateDownline);
            parameters.Add("subaccid", subAccId);
            parameters.Add("userid", userId);
            parameters.Add("language", laguageKey);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("ip", ip);
            DbResult result = ExecSp("Acc_CustomerUpdDisable152_51", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update HarnessAU status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="disabledHarnessAU"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="laguageKey"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdDisable153_51(int custId, int userId, int subAccId, bool disabledHarnessAU, bool updateDownline, string ip, string laguageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("disable153_51", disabledHarnessAU);
            parameters.Add("disabledownlines153_51", updateDownline);
            parameters.Add("subaccid", subAccId);
            parameters.Add("userid", userId);
            parameters.Add("language", laguageKey);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("ip", ip);
            DbResult result = ExecSp("Acc_CustomerUpdDisable153_51", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update disabled financial status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="disabledFinancial"></param>
        /// <param name="updateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="languageKey"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdDisableFI(int custId, int userId, int subAccId, bool disabledFinancial, bool updateDownline, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("userid", userId);
            parameters.Add("custid", custId);
            parameters.Add("subaccid", subAccId);
            parameters.Add("disablefinancial", disabledFinancial);
            parameters.Add("language", languageKey);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("ip", ip);
            parameters.Add("disabledownlinefi", updateDownline);

            DbResult result = ExecSp("Acc_CustomerUpdDisableFI", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update disabled auto master position taking status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="allowautomaster"></param>
        /// <param name="ip"></param>
        /// <param name="languageKey"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdMaster_DisableAutoMPT(int custId, int userId, int subAccId, bool allowautomaster, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("userid", userId);
            parameters.Add("custid", custId);
            parameters.Add("subaccid", subAccId);
            parameters.Add("disableautompt", allowautomaster);
            parameters.Add("language", languageKey);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("ip", ip);

            DbResult result = ExecSp("Acc_CustomerUpdMaster_DisableAutoMPT", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update disabled casino status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="isSync"></param>
        /// <param name="disabledCS"></param>
        /// <param name="isUpdateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="languageKey"></param>
        /// <returns></returns>
        public bool Acc_UpdCSStatus(int custId, int userId, int subAccId, bool isSync, bool disabledCS, bool isUpdateDownline, string ip, string languageKey)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("disabledcs", disabledCS);
            parameters.Add("synccs", isSync);
            parameters.Add("enabledownline", isUpdateDownline);

            parameters.Add("userid", userId);
            parameters.Add("subaccid", subAccId);
            parameters.Add("loginip", ip);
            parameters.Add("language", languageKey);

            DbResult result = ExecSp("Acc_UpdCSStatus", parameters);

            return Convert.ToBoolean(result.Outputs["status"]);
        }

        /// <summary>
        /// Update disabled casino status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="isSync"></param>
        /// <param name="disabledCS"></param>
        /// <param name="isUpdateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="languageKey"></param>
        /// <returns></returns>
        public int Acc_UpdCSStatus(int custId, int userId, int subAccId, bool isSync, bool disabledCS, bool isUpdateDownline, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("disabledcs", disabledCS);
            parameters.Add("synccs", isSync);
            parameters.Add("enabledownline", isUpdateDownline);

            parameters.Add("userid", userId);
            parameters.Add("subaccid", subAccId);
            parameters.Add("loginip", ip);
            parameters.Add("language", languageKey);

            DbResult result = ExecSp("Acc_UpdCSStatus", parameters);

            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update disabled bingo status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="isSynBingo"></param>
        /// <param name="disabledBingo"></param>
        /// <param name="isEnableDownline"></param>
        /// <param name="ip"></param>
        /// <param name="languageKey"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Acc_UpdBingoStatus(int custId, int userId, int subAccId, bool isSynBingo, bool disabledBingo, bool isEnableDownline, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("custid", custId);
            parameters.Add("disabledbingo", disabledBingo);
            parameters.Add("syncbg", isSynBingo);
            parameters.Add("enabledownline", isEnableDownline);
            parameters.Add("userid", userId);
            parameters.Add("subaccid", subAccId);
            parameters.Add("loginip", ip);
            parameters.Add("language", languageKey);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);

            DbResult result = ExecSp("Acc_UpdBingoStatus", parameters);
            errMsg = result.Outputs["errmsg"].ToString();

            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        /// <summary>
        /// Update disabled number game status for customer
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="subAccId"></param>
        /// <param name="disabledNumber"></param>
        /// <param name="isUpdateDownline"></param>
        /// <param name="ip"></param>
        /// <param name="languageKey"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Acc_CustomerUpdDisableNumber(int custId, int userId, int subAccId, bool disabledNumber, bool isUpdateDownline, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("disablenumber", disabledNumber);
            parameters.Add("disabledownline", isUpdateDownline);
            parameters.Add("subaccid", subAccId);
            parameters.Add("userid", userId);
            parameters.Add("language", languageKey);
            parameters.Add("ip", ip);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            DbResult result = ExecSp("Acc_CustomerUpdDisableNumber", parameters);
            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        public int Acc_CustomerUpdDisableVirtualSports(int custId, int userId, int subAccId, bool disable, bool isUpdateDownline, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("disable", disable);
            parameters.Add("disabledownlines", isUpdateDownline);
            parameters.Add("subaccid", subAccId);
            parameters.Add("userid", userId);
            parameters.Add("language", languageKey);
            parameters.Add("ip", ip);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            DbResult result = ExecSp("Acc_CustomerUpdDisableVS", parameters);
            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        public int Acc_CustomerUpdDoubleCommLosing(int custId, int userId, int subAccId, bool doubleCommLosing, string ip, string languageKey, ref string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("doublecommlosing", doubleCommLosing);
            parameters.Add("subaccid", subAccId);
            parameters.Add("userid", userId);
            parameters.Add("language", languageKey);
            parameters.Add("ip", ip);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            DbResult result = ExecSp("Acc_CustomerUpdDoubleCommLosing", parameters);
            errMsg = result.Outputs["errmsg"].ToString();
            return Convert.ToInt32(result.Outputs["errcode"]);
        }

        public DataTable Age_CustomerSelUserInfo(int custId)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            return ExecSpTable("Age_CustomerSelUserInfo", param);
        }

        public int Age_CustomerUpdUserInfo(int custId, string firstName, string lastName, string phone, string mobilePhone, string email, string fax, int isSync, string ip, int userId, int subAccId, ref int errcode)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            param.Add("firstname", firstName);
            param.Add("lastname", lastName);
            param.Add("phone", phone);
            param.Add("mobilephone", mobilePhone);
            param.Add("email", email);
            param.Add("fax", fax);
            param.Add("userid", userId);
            param.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino);
            param.Add("subaccid", subAccId);
            param.Add("ip", ip);
            param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
            param.Add("errorcode", 0);
            param.Add("errormsg", string.Empty);

            DbResult result = ExecSp("Age_CustomerUpdUserInfo", param);
            errcode = Convert.ToInt32(result.Outputs["errcode"]);
            if (errcode <= 0)
            {
                if (isSync == 1)
                {
                    Customer cust = new Customer();
                    try
                    {
                        cust.UpdateCustInfoSimpleNoStatus(custId, firstName, lastName, phone, mobilePhone, fax, email, Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(), Sunplus.Agent.Model.Entities.UserSession.Current.CustId, Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId == 0 ? Sunplus.Agent.Model.Entities.UserSession.Current.UserName : Sunplus.Agent.Model.Entities.UserSession.Current.SubUserName, 2);
                    }
                    catch
                    {
                    }
                }
            }

            return errcode;
        }

        public Hashtable Age_CustomerUpdPwd(string custId, string newPassword, string ip)
        {
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            param.Add("userpwd", Encryption.CfsEnCode(newPassword));
            param.Add("discasino", Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino);
            param.Add("userid", Sunplus.Agent.Model.Entities.UserSession.Current.UserId);
            param.Add("language", Sunplus.Agent.Model.Entities.UserSession.Current.LangKey);
            param.Add("subaccid", Sunplus.Agent.Model.Entities.UserSession.Current.SubAccId);
            param.Add("loginip", ip);
            param.Add("errcode", 0);
            param.Add("errmsg", "");

            return ExecSpHashtable("Age_CustomerUpdPwd", param);
        }
    }
}