/*
 * Created 20091224@Lee - Authorization data access
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.Model.DL.Authorization
{
    using System;
    using System.Collections;
    using System.Data;
    using Fanex.Data.DNAide;
    using Fanex.Age.Utility;

    public class AuthorizationDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        /// <summary>
        /// SMA after login.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="password"></param>
        /// <param name="subAccId"></param>
        /// <param name="loginTime"></param>
        /// <param name="language"></param>
        /// <param name="ip"></param>
        /// <param name="password"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public Hashtable Age_LoginClaimDownline(
            int custId,
            string password,
            int subAccId,
            string nickname,
            bool isNickname,
            DateTime loginTime,
            string language,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("custid", custId);
            //parameters.Add("userpwd", Encryption.CfsEnCode(password));
            parameters.Add("username", string.Empty);
            parameters.Add("nickname", nickname);
            parameters.Add("isnickname", isNickname);
            parameters.Add("subaccid", subAccId);
            parameters.Add("logintime", loginTime);
            parameters.Add("language", language);

            parameters.Add("roleid", 0);
            parameters.Add("on_userid", 0);
            InsertRequestInfo(requestInfo, parameters);

            Hashtable outputs = ExecSpHashtable("Age_LoginClaimDownline", parameters);

            errCode = (int)outputs["errcode"];
            errMsg = null;
            if (errCode != 0)
            {
                errMsg = outputs["errmsg"] as string;
            }

            return outputs;
        }

        /// <summary>
        /// Override SMA after login.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="password"></param>
        /// <param name="subAccId"></param>
        /// <param name="loginTime"></param>
        /// <param name="language"></param>
        /// <param name="ip"></param>
        /// <param name="password"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public Hashtable Age_LoginClaimDownline(
            int custId,
            string password,
            int subAccId,
            DateTime loginTime,
            string language,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg)
        {
            return this.Age_LoginClaimDownline(custId, password, subAccId, null, false, loginTime, language, requestInfo, out errCode, out errMsg);
        }

        /// <summary>
        /// Admin login.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="adminId"></param>
        /// <param name="loginTime"></param>
        /// <param name="language"></param>
        /// <param name="ip"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public Hashtable Age_LoginDownline_Admin(
            string userName,
            int adminId,
            DateTime loginTime,
            string language,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("username", userName);
            parameters.Add("accuserid", adminId);
            parameters.Add("logintime", loginTime);
            parameters.Add("language", language);

            parameters.Add("roleid", 0);
            parameters.Add("custid", 0);
            parameters.Add("on_userid", 0);
            InsertRequestInfo(requestInfo, parameters);

            Hashtable outputs = ExecSpHashtable("Age_LoginDownline_Admin", parameters);

            errCode = (int)outputs["errcode"];
            errMsg = null;
            if (errCode != 0)
            {
                errMsg = outputs["errmsg"] as string;
            }

            return outputs;
        }

        /// <summary>
        /// Login Verify Downline
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="ip"></param>
        /// <param name="domain"></param>
        /// <param name="custId"></param>
        /// <param name="subAccId"></param>
        /// <param name="loginTime"></param>
        /// <param name="IsLogNickname"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        public void Age_LoginVerifyDownline(
        string userName,
        string password,
        Hashtable requestInfo,
        string language,
        out int custId,
        out int subAccId,
        out DateTime loginTime,
        out string nickName,
        out bool isNickname,
        out int errCode,
        out string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("username", userName);
            //parameters.Add("userpwd", Encryption.CfsEnCode(password));
            parameters.Add("custid", 0);
            parameters.Add("subaccid", 0);
            parameters.Add("serverid", 0);
            parameters.Add("logintime", DateTime.Now);
            parameters.Add("language", language);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);
            InsertRequestInfo(requestInfo, parameters);

            DbResult dr = ExecSp("Age_LoginVerifyDownline", parameters);
            isNickname = dr.Outputs.Contains("isnickname") ? (bool)dr.Outputs["isnickname"] : false;
            nickName = dr.Outputs.Contains("nickname") ? Convert.ToString(dr.Outputs["nickname"]) : string.Empty;
            errCode = (int)dr.Outputs["errcode"];
            errMsg = (string)dr.Outputs["errmsg"];
            custId = (int)dr.Outputs["custid"];
            subAccId = (int)dr.Outputs["subaccid"];
            loginTime = (DateTime)dr.Outputs["logintime"];
        }

        /// <summary>
        /// Override Login Verify Downline
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="ip"></param>
        /// <param name="domain"></param>
        /// <param name="custId"></param>
        /// <param name="subAccId"></param>
        /// <param name="loginTime"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        public void Age_LoginVerifyDownline(
        string userName,
        string password,
        Hashtable requestInfo,
        string language,
        out int custId,
        out int subAccId,
        out DateTime loginTime,
        out int errCode,
        out string errMsg)
        {
            string nickName = string.Empty;
            bool isNickName = false;
            this.Age_LoginVerifyDownline(userName, password, requestInfo, language, out custId, out subAccId, out loginTime, out nickName, out isNickName, out errCode, out errMsg);
        }

        /// <summary>
        /// Customer/Sub-account login.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="language"></param>
        /// <param name="netSessionId"></param>
        /// <param name="ip"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <param name="outputValues"></param>
        /// <param name="isSubAcc"></param>
        /// <returns></returns>
        public bool SignIn(
            string userName,
            string password,
            string language,
            string netSessionId,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg,
            ref Hashtable outputValues,
            out bool isSubAcc)
        {
            //Hashtable parameters = new Hashtable();

            //parameters.Add("isexist", 0);
            //parameters.Add("subusername", userName);

            //Hashtable outputs = ExecSpHashtable("Age_LoginDownline_CheckSubUsername", parameters);

            //if ((bool)outputs["isexisted"])
            //{
            //    isSubAcc = true;
            //    outputValues = this.Age_LoginDownline_Sub(userName, password, language, requestInfo, out errCode, out errMsg);
            //}
            //else
            //{
            //    isSubAcc = false;
            //    //outputValues = this.Age_LoginDownline(userName, password, language, requestInfo, out errCode, out errMsg);
            //    outputValues = this.Agent_Login(userName, out errCode, out errMsg);
            //}
            isSubAcc = false;
            outputValues = this.Agent_Login(userName, out errCode, out errMsg);
            return errCode == 0;
        }

        /// <summary>
        /// Check in a record on ON_USER database.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="subAccId"></param>
        /// <param name="netSessionId"></param>
        /// <param name="serverId"></param>
        /// <param name="ip"></param>
        /// <param name="onUserId"></param>
        public void Acc_Checkin(
            int custId,
            int subAccId,
            string netSessionId,
            int serverId,
            string ip,
            out int onUserId)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("custid", custId);
            parameters.Add("netsessionid", netSessionId);

            // Server ID Setting: agent: 90; master: 91; super: 92
            // In order not to affect the online statistics report (admin site)
            parameters.Add("serverid", serverId);
            parameters.Add("on_userid", 0);
            parameters.Add("ip", ip);
            parameters.Add("subaccid", subAccId);

            Hashtable outputs = ExecSpHashtable("Acc_Checkin", parameters);
            onUserId = (int)outputs["on_userid"];
        }

        

        /// <summary>
        /// Check-out On_User database.
        /// </summary>
        /// <param name="onUserId"></param>
        /// <param name="userName"></param>
        /// <param name="roleId"></param>
        /// <param name="serverId"></param>
        public void Acc_CheckOut(
            int onUserId,
            string userName,
            string nickName,
            byte roleId,
            int serverId)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("on_userid", onUserId);
            parameters.Add("username", userName);
            parameters.Add("roleid", roleId);
            parameters.Add("serverid", serverId);
            parameters.Add("nickname", nickName);

            ExecSp("Acc_CheckOut", parameters);
        }

        #region Checking Permission

        /// <summary>
        /// Check permission of a sub-account.
        /// </summary>
        /// <param name="subAccId"></param>
        /// <param name="pageAccess"></param>
        /// <param name="language"></param>
        /// <param name="userId"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Age_CustSubCheckPageAccess(
            int subAccId,
            string pageAccess,
            string language,
            int userId,
            out int errCode,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("subaccid", subAccId);
            parameters.Add("sppageaccess", pageAccess);
            parameters.Add("language", language);
            parameters.Add("userid", userId);
            parameters.Add("errmsg", string.Empty);
            parameters.Add("errcode", 0);

            Hashtable outputs = ExecSpHashtable("Age_CustSubCheckPageAccess", parameters);

            errMsg = outputs["errmsg"] as string;
            errCode = (int)outputs["errcode"];

            return errCode == 0;
        }

        /// <summary>
        /// Get some extra informations of a customer: [bool DisableHorseracing, bool DisableFI, bool DisableAutoMPT, int SRecommend, MRecommend, ARecommend, int IsAdminCanViewVNIP,Suspend,Closed,AutoSuspend,SyncCasino,DisableCasino,DisableOutright,IsInternal].
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="adminId"></param>
        /// <returns>[bool disablehorseracing, bool disablefi, bool disableautompt, int srecommend, mrecommend, arecommend, int isadmincanviewvnip,suspend,closed,autosuspend,synccasino,disablecasino,disableoutright,isinternal]</returns>
        public DataTable Acc_CustomerSelStatus(int custId, int adminId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("adminid", adminId);

            DataTable result = ExecSpTable("Acc_CustomerSelStatus", parameters);

            return result;
        }

        /// <summary>
        /// Check Show/Hide Consolidated Report
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public bool Age_Rpt_ConsolidatedGroup_isShownMenu(int custId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("isshownmenu", 0);
            Hashtable outputs = ExecSpHashtable("Age_Rpt_ConsolidatedGroup_isShownMenu", parameters);

            return (bool)outputs["isshownmenu"];
        }

        /// <summary>
        /// Check a customer is allow transfer on Total Balance.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="superIds"></param>
        /// <returns></returns>
        public bool Age_Transfer_CheckTotalBalanceTransfer(int custId, string superIds)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("superidstransfertotal", superIds);

            Hashtable outputs = ExecSpHashtable("Age_Transfer_CheckTotalBalanceTransfer", parameters);

            return (bool)outputs["isallowed"];
        }

        /// <summary>
        /// Checking a customer is enable or disabled.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool Acc_CustomerSelCheckEnable(int custId, out string errMsg)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("custid", custId);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);

            Hashtable outputs = ExecSpHashtable("Acc_CustomerSelCheckEnable", parameters);

            errMsg = outputs["errmsg"] as string;
            int errCode = (int)outputs["errcode"];

            return errCode == 0;
        }

        /// <summary>
        /// Check belonging in archive DB.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="currentCustId"></param>
        /// <param name="transId"></param>
        /// <returns></returns>
        public bool Age_CustomerSelCheckBelonging(int custId, int currentCustId, long transId)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("sessioncustid", currentCustId);
            if (custId != 0)
            {
                parameters.Add("custid", custId);
            }

            parameters.Add("transid", transId);
            parameters.Add("roleid", 0);

            Hashtable outputs = ExecSpHashtable("Age_CustomerSelCheckBelonging", parameters);

            return ((byte)outputs["roleid"]) > 0;
        }

        /// <summary>
        /// Check belonging in data warehouse DB.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="currentCustId"></param>
        /// <param name="transId"></param>
        /// <returns></returns>
        public bool Age_CustomerSelCheckBelonging_OLD(int custId, int currentCustId, long transId)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("sessioncustid", currentCustId);
            if (custId != 0)
            {
                parameters.Add("custid", custId);
            }

            parameters.Add("transid", transId);
            parameters.Add("roleid", 0);

            Hashtable outputs = ExecSpHashtable("Age_CustomerSelCheckBelonging_OLD", parameters);

            return (short)outputs["roleid"] > 0;
        }

        public bool Age_CustomerSelCheckBelonging_Multi(string custIds, int currentCustId)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("sessioncustid", currentCustId);
            parameters.Add("custid", custIds);
            parameters.Add("roleid", 0);

            Hashtable outputs = ExecSpHashtable("Age_CustomerSelCheckBelonging_MultiCust", parameters);

            return (byte)outputs["roleid"] > 0;
        }

        #endregion Checking Permission

        #region ViewLogDependencies

        /// <summary>
        /// Check if userName is belong to uplineId.
        /// </summary>
        /// <param name="userName">userName need to check</param>
        /// <param name="uplineId">Id of current login user</param>
        /// <param name="isSubAcc">return if userName is sub account</param>
        /// <returns>Return Id of userName</returns>
        public int Age_CustomerSelCheckBelonging_ByUsername(string userName, int uplineId, out bool isSubAcc)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("uplineid", uplineId);
            parameters.Add("username", userName);
            parameters.Add("custid", 0);
            parameters.Add("issubacc", false);

            Hashtable outputs = ExecSpHashtable("Age_CustomerSelCheckBelonging_ByUsername", parameters);

            isSubAcc = (bool)outputs["issubacc"];
            return (int)outputs["custid"];
        }

        /// <summary>
        /// Check if userName is belong to uplineId.
        /// Author: Jeff
        /// Task: Search&Copy
        /// </summary>
        /// <param name="userName">userName need to check</param>
        /// <param name="uplineId">Id of current login user</param>
        /// <param name="isSubAcc">return if userName is sub account</param>
        /// <returns>Return Id of userName</returns>
        public int Age_CustomerSelCheckBelonging_ByUsername(string userName, int uplineId, out bool isSubAcc, out int roleId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("uplineid", uplineId);
            parameters.Add("username", userName);
            parameters.Add("custid", 0);
            parameters.Add("issubacc", false);
            parameters.Add("roleid", 0);

            Hashtable outputs = ExecSpHashtable("Age_CustomerSelCheckBelonging_ByUsername", parameters);

            isSubAcc = (bool)outputs["issubacc"];
            roleId = (int)outputs["roleid"];
            return (int)outputs["custid"];
        }

        /// <summary>
        /// Check user whether has permission on a function or not.
        /// Author: Lewin
        /// Task: viewlog function
        /// </summary>
        /// <param name="userId">int-userid want to check his permission</param>
        /// <param name="funcId">int-functionid</param>
        /// <returns>bool</returns>
        public bool Age_CustomerSelCheckPermission(int userId, int funcId)
        {
            bool hasPermission = false;

            Hashtable parms = new Hashtable();
            parms.Add("UserId", userId);
            parms.Add("FuncId", funcId);

            Hashtable outputs = ExecSpHashtable("Age_CustomerSelCheckPermission", parms);

            hasPermission = (bool)outputs["Policy"];
            return hasPermission;
        }

        #endregion ViewLogDependencies

        #region Get Request Info

        private static void InsertRequestInfo(Hashtable requestInfo, Hashtable parameters)
        {
            foreach (DictionaryEntry d in requestInfo)
            {
                parameters.Add(d.Key, d.Value);
            }
        }

        #endregion Get Request Info

        /// <summary>
        /// Sub-account login.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="language"></param>
        /// <param name="ip"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private Hashtable Age_LoginDownline_Sub(
            string userName,
            string password,
            string language,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("username", string.Empty);
            parameters.Add("subusername", userName);
            //parameters.Add("userpwd", Encryption.CfsEnCode(password));
            parameters.Add("language", language);
            parameters.Add("roleid", 0);
            parameters.Add("custid", 0);
            parameters.Add("on_userid", 0);
            InsertRequestInfo(requestInfo, parameters);

            Hashtable outputs = ExecSpHashtable("Age_LoginDownline_Sub", parameters);

            errCode = (int)outputs["errcode"];
            errMsg = null;
            if (errCode != 0)
            {
                errMsg = outputs["errmsg"] as string;
            }

            // Temporary remove. Waiting fixing store procedure.
            outputs.Remove("on_userid");

            return outputs;
        }

        /// <summary>
        /// SMA login.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="language"></param>
        /// <param name="ip"></param>
        /// <param name="errCode"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private Hashtable Age_LoginDownline(
            string userName,
            string password,
            string language,
            Hashtable requestInfo,
            out int errCode,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("username", userName);
            //parameters.Add("userpwd", Encryption.CfsEnCode(password));
            parameters.Add("language", language);
            parameters.Add("roleid", 0);
            parameters.Add("custid", 0);
            parameters.Add("on_userid", 0);
            InsertRequestInfo(requestInfo, parameters);

            Hashtable outputs = ExecSpHashtable("Age_LoginDownline", parameters);
            errCode = (int)outputs["errcode"];
            errMsg = null;
            if (errCode != 0)
            {
                errMsg = outputs["errmsg"] as string;
            }

            return outputs;
        }

        private Hashtable Agent_Login(string userName, out int errCode, out string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("username", userName);
            parameters.Add("agentid", 0);
            parameters.Add("agentlevel", 0);
            parameters.Add("currency", 0);
            parameters.Add("ismaster", 0);
            parameters.Add("pageaccess", string.Empty);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);

            Hashtable outputs = ExecSpHashtable("Agent_Login", parameters);
            errCode = (int)outputs["errcode"];
            errMsg = null;
            if (errCode != 0)
            {
                errMsg = outputs["errmsg"] as string;
            }

            return outputs;
        }
    }
}