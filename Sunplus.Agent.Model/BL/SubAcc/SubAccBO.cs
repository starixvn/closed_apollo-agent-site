/*
 * Created 20091224@Lee - Business class of sub-account function
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.Model.BL.SubAcc
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Sunplus.Agent.Model.DL.SubAcc;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Base;

    public class SubAccBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        public SubAccBO(string netSessionId)
            : base(netSessionId)
        {
        }

        /// <summary>
        /// Get list sub-accounts information.
        /// </summary>
        /// <param name="count">Number of records</param>
        /// <returns>Return a List of SubAccount if succeeded or raise an exception.</returns>
        public List<AgentUserAccount> GetSubAccountList(int CustId)
        {
            SubAccDAO dao = new SubAccDAO();

            DataTable dataTable = dao.Acc_AgentUser_GetByAgent(CustId);

            List<AgentUserAccount> subAccList = new List<AgentUserAccount>();

            foreach (DataRow dr in dataTable.Rows)
            {
                subAccList.Add(new AgentUserAccount
                {
                    AgentUserId = (int)dr["AgentUserId"],
                    Username = dr["UserName"].ToString(),
                    PageAccess = dr["PageAccess"].ToString(),
                    IsMaster = (bool)dr["IsMaster"],
                    Closed = (bool)dr["Closed"],
                    CreateDate = (DateTime)dr["CreateDate"],
                    Creator = (int)dr["Creator"],
                    LoginCount = 0
                });
            }
            return subAccList;
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
        public int UpdateSubAccountPermission(
            int agentId,
            int agentUserId,
            string pageAccess,
            out string errMsg)
        {
            SubAccDAO dao = new SubAccDAO();

            return dao.Acc_AgentUser_UpdatePermission(
                agentId,
                agentUserId,
                pageAccess,
                out errMsg);
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
            out string errMsg)
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
                out errMsg);
        }
    }
}