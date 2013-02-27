using System.Data;
using System.Collections.Generic;
using System.Collections;

using Sunplus.Agent.Model.Entities;
using Sunplus.Agent.Model.Entities.Base;

using Sunplus.Agent.Model.DL.SubAcc;

/*
 * Created 20091224@Lee - Business class of sub-account function
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.BL.SubAcc
{
    public class SubAccBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        public SubAccBO(string netSessionId)
            : base(netSessionId)
        { }

        /// <summary>
        /// Get list sub-accounts information.
        /// </summary>
        /// <param name="count">Number of records</param>
        /// <returns>Return a List of SubAccount if succeeded or raise an exception.</returns>
        public List<SubAccount> GetSubAccountList()
        {
            SubAccDAO dao = new SubAccDAO();

            DataTable dataTable = dao.Age_CustSubSel(CurrentSession.CustId);

            IEnumerable<IEntity> enumerable = dataTable.AsEnumerable(typeof(SubAccount));
            List<SubAccount> subAccList = new List<SubAccount>();

            foreach (IEntity entity in enumerable)
            {
                subAccList.Add(entity as SubAccount);
            }

            return subAccList;
        }
        
        /// <summary>
        /// Get informations of a sub-account.
        /// </summary>
        /// <param name="custId">Main account id</param>
        /// <param name="subAccId">Sub-account id</param>
        /// <returns>Return a SubAccount if succeeded or raise an exception.</returns>
        public SubAccount GetSubAccount(int subAccId)
        {
            SubAccDAO dao = new SubAccDAO();

            DataTable dataTable = dao.Age_CustSubSelInfo(CurrentSession.CustId, subAccId);

            SubAccount subAccount = new SubAccount(dataTable.Rows[0], false);

            return subAccount;
        }

        /// <summary>
        /// Insert a sub-account.
        /// </summary>
        /// <param name="subAccount">Sub-account object. If insert failed, sub-account object will have id = 0</param>
        /// <param name="ip">User ip address</param>
        /// <param name="errMsg">Output error message</param>
        /// <returns>Return true if succeeded or false if failed.</returns>
        public int InsertSubAccount(
            ref SubAccount subAccount,
            string ip,
            out string errMsg)
        {
            SubAccDAO dao = new SubAccDAO();

            int subAccId = 0;
            int errCode = -1;

            bool isSuccess = dao.Age_CustSubIns(
                out subAccId,
                CurrentSession.CustId,
                subAccount.UserName,
                subAccount.PassWord,
                subAccount.FirstName,
                subAccount.LastName,
                subAccount.PageAccess,
                CurrentSession.SubAccId,
                CurrentSession.UserId,
                out errCode,
                CurrentSession.LangKey,
                ip,
                out errMsg
                );

            if (isSuccess)
            {
                subAccount.Id = subAccId;
                errCode = 0;
            }

            return errCode;
        }

        /// <summary>
        /// Update a sub-account.
        /// </summary>
        /// <param name="ip">User ip address</param>
        /// <param name="errMsg">Output error message</param>
        /// <returns>Return error code or raise an exception.</returns>
        public int UpdateSubAccount(
            SubAccount subAccount,
            string ip,
            out string errMsg)
        {
            SubAccDAO dao = new SubAccDAO();

            return dao.Age_CustSubUpdInfo(
                subAccount.Id,
                CurrentSession.CustId,
                subAccount.FirstName,
                subAccount.LastName,
                subAccount.IsClosed,
                CurrentSession.SubAccId,
                CurrentSession.UserId,
                CurrentSession.LangKey,
                ip,
                out errMsg
                );
        }

        /// <summary>
        /// Update page access of a sub-account.
        /// </summary>
        /// <param name="subAccId">Sub-account id</param>
        /// <param name="pageAccessType">Sub-account permission type ([A], [B], [C], [D], [E])</param>
        /// <param name="isAddNew">Indicate in add new case or update case</param>
        /// <param name="ip">User ip address</param>
        /// <param name="errMsg">Output error message</param>
        /// <returns>Return error code or raise an exception.</returns>
        public int UpdateSubAccount(
            int subAccId,
            string pageAccessType,
            bool isAddNew,
            string ip,
            out string errMsg)
        {
            SubAccDAO dao = new SubAccDAO();

            return dao.Age_CustSubUpdPageAccess(
                CurrentSession.CustId,
                subAccId,
                pageAccessType,
                isAddNew,
                CurrentSession.SubAccId,
                CurrentSession.UserId,
                CurrentSession.LangKey,
                ip,
                out errMsg
                );
        }

        /// <summary>
        /// Update password of a sub-account.
        /// </summary>
        /// <param name="subAccId">Sub-account id</param>
        /// <param name="userPwd">Sub-account password</param>
        /// <param name="ip">Login id address</param>
        /// <param name="errMsg">Output error message</param>
        /// <returns></returns>
        public int UpdateSubAccount(
            int subAccId,
            string userPwd,
            string ip,
            out string errMsg
            )
        {
            SubAccDAO dao = new SubAccDAO();

            return dao.Age_CustSubPwdUpd(
                CurrentSession.CustId,
                subAccId,
                userPwd,
                CurrentSession.LangKey,
                CurrentSession.SubAccId,
                CurrentSession.UserId,
                ip,
                out errMsg
                );
        }

        /// <summary>
        /// Change password of current sub-account.
        /// </summary>
        /// <returns></returns>
        public int ChangePassword(string newPassword, string oldPassword, string ip, out string errMsg)
        {
            SubAccDAO dao = new SubAccDAO();

            return dao.Age_CustSubPwdUpd2(
                CurrentSession.CustId,
                newPassword,
                oldPassword,
                CurrentSession.LangKey,
                ip,
                CurrentSession.SubAccId,
                out errMsg);
        }
    }
}