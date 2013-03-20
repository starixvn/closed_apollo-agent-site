namespace Sunplus.Agent.Model.BL.Transfer
{
    using System.Collections;
    using System.Data;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.DL.Transfer;

    public class TransferBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        public TransferBO(string netSessionId)
            : base(netSessionId)
        {
        }

        public int UpdateTransferYesBalance(string custId, double amt, bool isFullTransfer, bool isChecked, string status, string ip, out string errMsg)
        {
            if (!isChecked)
            {
                status = string.Empty;
            }

            TransferDAO dao = new TransferDAO();
            if (CurrentSession.IsAgent)
            {
                return dao.Age_Transfer_Ins_Ag(
                       custId,
                       CurrentSession.Username,
                       CurrentSession.CustId,
                       CurrentSession.SubAccId,
                       ip,
                       -amt,
                       isFullTransfer,
                       AppSettings.AllowedListSuperAccounts,
                       CurrentSession.LangKey,
                       out errMsg);
            }
            else if (CurrentSession.IsMaster)
            {
                return dao.Age_Transfer_Ins_MAg(
                       custId,
                       CurrentSession.CustId,
                       CurrentSession.SubAccId,
                       CurrentSession.CustId,
                       ip,
                       -amt,
                       isFullTransfer,
                       status,
                       AppSettings.AllowedListSuperAccounts,
                       CurrentSession.LangKey,
                       out errMsg);
            }
            else if (CurrentSession.IsSuper)
            {
                return dao.Age_Transfer_Ins_SAg(
                       custId,
                       CurrentSession.CustId,
                       CurrentSession.SubAccId,
                       CurrentSession.CustId,
                       ip,
                       -amt,
                       isFullTransfer,
                       status,
                       AppSettings.AllowedListSuperAccounts,
                       CurrentSession.LangKey,
                       out errMsg);
            }

            errMsg = string.Empty;
            return 0;
        }

        public int TransferYesCleareBalanceForAgent(string toCustIds, bool isChecked, string status, string ip, out string errMsg)
        {
            TransferDAO dao = new TransferDAO();

            if (!isChecked)
            {
                status = string.Empty;
            }

            return dao.Age_Transfer_InsClearBalance_Ag(
                toCustIds, 
                CurrentSession.SubAccId, 
                CurrentSession.UserId, 
                CurrentSession.CustId,
                ip, 
                status, 
                CurrentSession.LangKey, 
                AppSettings.AllowedListSuperAccounts,
                out errMsg); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="pageIndex"></param>
        /// <param name="userName"></param>
        /// <param name="isYesterdayBalance"></param>
        /// <returns></returns>
        /// 
        public DataTable GetDataTransfer(int status, int pageIndex, ref int totalPages, string userName, bool isYesterdayBalance)
        {
            return GetDataTransfer(status, pageIndex, ref totalPages, AppSettings.PageSize, userName, isYesterdayBalance);    
        }

        public DataTable GetDataTransfer(int status, int pageIndex, ref int totalPages, int pagesize, string userName, bool isYesterdayBalance)
        {
            TransferDAO dao = new TransferDAO();

            bool? closed = null, suspended = null, enabled = null;

            if (status == 1)
            {
                closed = false;
            }
            else if (status == 2)
            {
                suspended = true;
            }
            else if (status == 3)
            {
                closed = true;
            }
            else if (status != 0)
            {
                enabled = false;
            }

            if (CurrentSession.IsAgent)
            {
                return dao.Age_Transfer_SelList_Ag(
                    CurrentSession.CustId,
                    closed,
                    suspended,
                    enabled,
                    pageIndex,
                    pagesize,
                    ref totalPages,
                    userName,
                    !isYesterdayBalance,
                    CurrentSession.IsAllowedTotalBalanceTransfer);
            }
            else if (CurrentSession.IsMaster)
            {
                return dao.Age_Transfer_SelList_MAg(
                    CurrentSession.CustId,
                    closed,
                    suspended,
                    enabled,
                    pageIndex,
                    pagesize,
                    ref totalPages,
                    userName,
                    !isYesterdayBalance,
                    CurrentSession.IsAllowedTotalBalanceTransfer);
            }
            else if (CurrentSession.IsSuper)
            {
                return dao.Age_Transfer_SelList_SAg(
                    CurrentSession.CustId,
                    closed,
                    suspended,
                    enabled,
                    pageIndex,
                    pagesize,
                    ref totalPages,
                    userName,
                    !isYesterdayBalance,
                    CurrentSession.IsAllowedTotalBalanceTransfer);
            }

            return null;
        }

        // start Jeff add for Member Info 281011
        public Hashtable TransferSelMemberInfoYesterday(string custId)
        {
            TransferDAO dao = new TransferDAO();

            if (CurrentSession.IsAgent)
            {
                return dao.Age_Transfer_SelMemberInfo_Yesterday(
                    custId,
                    CurrentSession.IsAllowedTotalBalanceTransfer);
            }

            return null;
        }

        // start Jeff add Check for existing transfer 081111
        public DataTable TransferCheckExistingForAgent(string custId)
        {
            TransferDAO dao = new TransferDAO();

            if (CurrentSession.IsAgent)
            {
                return dao.Age_Transfer_CheckExisting(
                    custId);
            }

            return null;
        }

        public string GetOptionTransferCondition(int custId)
        {
            TransferDAO dao = new TransferDAO();
            return dao.Acc_CustomerSelTransferOption(custId);          
        }
    }
}