namespace Sunplus.Agent.Model.DL.Transfer
{
    using System;
    using System.Collections;
    using System.Data;
    using Fanex.Data.DNAide;
    using Sunplus.Agent.Model.DL.Base;

    public class TransferDAO : DAOBase
    {
        public int Age_Transfer_Ins_Ag(string fromCustId, string toUserName, int creatorId, int subAccId, string ip, double amt, bool isFullTransfer, string superIdStransferTotal, string language, out string errMsg)
        {
            // isYes=1 --> Only Yesterday
            // isYes=0 --> Get All
            Hashtable parameters = new Hashtable();
            parameters.Add("superidstransfertotal", superIdStransferTotal);
            parameters.Add("fromcustid", fromCustId);
            parameters.Add("tousername", toUserName);
            parameters.Add("amount", amt);
            parameters.Add("creator", creatorId); // AppSession.UserID : UserID of Admin Login
            parameters.Add("language", language);
            parameters.Add("subaccid", subAccId);
            parameters.Add("ip", ip);

            Hashtable outputs = ExecSpHashtable("Age_Transfer_Ins_Ag", parameters);

            errMsg = outputs["errmsg"] as string;
            return (int)outputs["errcode"];
        }

        public int Age_Transfer_Ins_MAg(string toCustIds, int creatorId, int subAccId, int masterId, string ip, double amt, bool isFullTransfer, string status, string superIdStransferTotal, string language, out string errMsg)
        {
            // isYes=1 --> Only Yesterday
            // isYes=0 --> Get All
            Hashtable parameters = new Hashtable();
            parameters.Add("superidstransfertotal", superIdStransferTotal);
            parameters.Add("tocustids", toCustIds);
            parameters.Add("creator", creatorId); 
            parameters.Add("fulltransfer", isFullTransfer);
            parameters.Add("language", language);
            parameters.Add("subaccid", subAccId);
            parameters.Add("ip", ip);

            parameters.Add("amttrans", amt);
            parameters.Add("masterid", masterId);
            parameters.Add("custstatus", status);

            Hashtable outputs = ExecSpHashtable("Age_Transfer_Ins_MAg", parameters);

            errMsg = outputs["errmsg"] as string;
            return (int)outputs["errcode"];
        }

        public int Age_Transfer_Ins_SAg(string toCustIds, int creatorId, int subAccId,  int superId, string ip, double amt, bool isFullTransfer, string status, string superIdStransferTotal, string language, out string errMsg)
        {
            // isYes=1 --> Only Yesterday
            // isYes=0 --> Get All
            Hashtable parameters = new Hashtable();
            parameters.Add("superidstransfertotal", superIdStransferTotal);
            parameters.Add("tocustids", toCustIds);
            parameters.Add("creator", creatorId); 
            parameters.Add("fulltransfer", isFullTransfer);
            parameters.Add("language", language);
            parameters.Add("subaccid", subAccId);
            parameters.Add("ip", ip);
            parameters.Add("amttrans", amt);
            parameters.Add("sagid", superId);
            parameters.Add("custstatus", status);

            Hashtable outputs = ExecSpHashtable("Age_Transfer_Ins_SAg", parameters);

            errMsg = outputs["errmsg"] as string;
            return (int)outputs["errcode"];
        }

        public int Age_Transfer_InsClearBalance_Ag(string toCustIds, int subAccId, int creatorId, int fromCustId, string ip, string status, string language, string superIdStransferTotal, out string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("fromcustid", fromCustId);
            parameters.Add("creator", creatorId); 
            parameters.Add("tocustids", toCustIds);
            parameters.Add("language", language);
            parameters.Add("subaccid", subAccId);
            parameters.Add("ip", ip);
            parameters.Add("custstatus", status);
            parameters.Add("superidstransfertotal", superIdStransferTotal);

            Hashtable outputs = ExecSpHashtable("Age_Transfer_InsClearBalance_Ag", parameters);

            errMsg = outputs["errmsg"] as string;
            return (int)outputs["errcode"];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="closed"></param>
        /// <param name="suspended"></param>
        /// <param name="enabled"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        /// <param name="userName"></param>
        /// <param name="isYesterdayBalance"></param>
        /// <param name="isAllowedTotalBalTrans"></param>
        /// <returns>Return table. When login is agent</returns>
        public DataTable Age_Transfer_SelList_Ag(
            int custid, 
            bool? closed,
            bool? suspended,
            bool? enabled,
            int pageIndex, 
            int pageSize,
            ref int totalPages,
            string userName,
            bool isYesterdayBalance, 
            bool isAllowedTotalBalTrans)
        {
            // Function list data transfer of Agent
            Hashtable htb = new Hashtable();
            htb.Add("custid", custid);
            htb.Add("pageindex", pageIndex);
            htb.Add("pagesize", pageSize);
            htb.Add("username", userName);
            htb.Add("isyesterdaybalance", isYesterdayBalance);
            htb.Add("isallowtotalbalance", isAllowedTotalBalTrans);

            htb.Add("closed", closed);
            htb.Add("suspended", suspended);
            htb.Add("enabled", enabled);

            DbResult rs = ExecSp("Age_Transfer_SelList_Ag", htb);
            totalPages = (int)rs.Outputs["totalpages"];
            return rs.ResultSet.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="closed"></param>
        /// <param name="suspended"></param>
        /// <param name="enabled"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        /// <param name="userName"></param>
        /// <param name="isYesterdayBalance"></param>
        /// <param name="isAllowedTotalBalTrans"></param>
        /// <returns>Return table. When login is master</returns>
        public DataTable Age_Transfer_SelList_MAg(
            int custid,
            bool? closed,
            bool? suspended,
            bool? enabled,
            int pageIndex,
            int pageSize,
            ref int totalPages,
            string userName,
            bool isYesterdayBalance,
            bool isAllowedTotalBalTrans)
        {
            // Function list data transfer of Agent
            Hashtable htb = new Hashtable();
            htb.Add("masterid", custid);
            htb.Add("pageindex", pageIndex);
            htb.Add("pagesize", pageSize);
            htb.Add("username", userName);
            htb.Add("isyesterdaybalance", isYesterdayBalance);
            htb.Add("isallowtotalbalance", isAllowedTotalBalTrans);

            htb.Add("closed", closed);
            htb.Add("suspended", suspended);
            htb.Add("enabled", enabled);

            DbResult rs = ExecSp("Age_Transfer_SelList_MAg", htb);
            totalPages = (int)rs.Outputs["totalpages"];
            return rs.ResultSet.Tables[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="closed"></param>
        /// <param name="suspended"></param>
        /// <param name="enabled"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        /// <param name="userName"></param>
        /// <param name="isYesterdayBalance"></param>
        /// <param name="isAllowedTotalBalTrans"></param>
        /// <returns>Return table. When login is super</returns>
        public DataTable Age_Transfer_SelList_SAg(
            int custid,
            bool? closed,
            bool? suspended,
            bool? enabled,
            int pageIndex,
            int pageSize,
            ref int totalPages,
            string userName,
            bool isYesterdayBalance,
            bool isAllowedTotalBalTrans)
        {
            // Function list data transfer of Agent
            Hashtable htb = new Hashtable();
            htb.Add("sagid", custid);
            htb.Add("pageindex", pageIndex);
            htb.Add("pagesize", pageSize);
            htb.Add("username", userName);
            htb.Add("isyesterdaybalance", isYesterdayBalance);
            htb.Add("isallowtotalbalance", isAllowedTotalBalTrans);

            htb.Add("closed", closed);
            htb.Add("suspended", suspended);
            htb.Add("enabled", enabled);

            DbResult rs = ExecSp("Age_Transfer_SelList_SAg", htb);
            totalPages = (int)rs.Outputs["totalpages"];
            return rs.ResultSet.Tables[0];
        }

        // start Jeff add for Member Info 281011
        public Hashtable Age_Transfer_SelMemberInfo_Yesterday(string custid, bool isAllowedTotalBalanceTransfer)
        {
            Hashtable htb = new Hashtable();
            htb.Add("custid", custid);
            htb.Add("isallowtotalbalance", isAllowedTotalBalanceTransfer);
            htb.Add("username", string.Empty);
            htb.Add("synccasino", 0);
            htb.Add("disablecasino", 0);
            htb.Add("cashbalance", 0);
            htb.Add("oustanding", 0);
            htb.Add("givencredit", 0);
            htb.Add("betcredit", 0);
            htb.Add("lastlogin", DateTime.Now);
            htb.Add("balance", 0);
            htb.Add("maxtransdate", DateTime.Now);
            htb.Add("futurebetcredit", 0);

            DbResult rs = ExecSp("Age_Transfer_SelMemberInfo_Yesterday", htb);
            return rs.Outputs;
        }

        // start Jeff add Check for existing transfer 081111
        public DataTable Age_Transfer_CheckExisting(string custid)
        {
            Hashtable htb = new Hashtable();
            htb.Add("custid", custid);            
            DbResult rs = ExecSp("Age_Transfer_CheckExisting", htb);
            return rs.ResultSet.Tables[0];
        }

        /// <summary>
        /// Get option transfer Condition
        /// </summary>
        /// <param name="refno"></param>
        /// <param name="language"></param>
        /// <param name="transid"></param>
        /// <param name="betid"></param>
        /// <param name="custId"></param>
        /// <param name="balanceupday"></param>
        /// <param name="movebetday"></param>
        /// <returns></returns>
        public string Acc_CustomerSelTransferOption(int custId)
        {
            // Parameters
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("transferoption", string.Empty);
            DbResult rs = ExecSp("Acc_CustomerSelTransferOption", parameters);
            return (string)rs.Outputs["transferoption"];
        }
    }
}
