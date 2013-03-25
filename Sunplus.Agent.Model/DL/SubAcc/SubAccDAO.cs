/*
 * Created 20091224@Lee - Data access object of sub-account function
 * Revision:
 * ?@? - ...
 *
 */

namespace Sunplus.Agent.Model.DL.SubAcc
{
    using System.Collections;
    using System.Data;
    using Fanex.Age.Utility;

    public class SubAccDAO : Sunplus.Agent.Model.DL.Base.DAOBase
    {
        /// <summary>
        /// Get list sub-accounts information.
        /// </summary>
        /// <param name="custId">Main account id</param>
        /// <returns>Return a DataTable if succeeded, otherwise throw System.Data.SqlClient.SqlException or Sunplus.Agent.Model.DL.DataException.</returns>
        public DataTable Age_AgentUser_GetByAgent(int custId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", custId);

            DataTable result = ExecSpTable("Age_AgentUser_GetByAgent", parameters);

            return result;
        }

        /// <summary>
        /// Get informations of a sub-account.
        /// </summary>
        /// <param name="custId">Main account id</param>
        /// <param name="subAccId">Sub-account id</param>
        /// <returns>Return a DataTable if succeeded, otherwise throw System.Data.SqlClient.SqlException or Sunplus.Agent.Model.DL.DataException.</returns>
        public DataTable Age_CustSubSelInfo(int custId, int subAccId)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("custid", custId);
            parameters.Add("subaccid", subAccId);
            DataTable result = ExecSpTable("Age_CustSubSelInfo", parameters);
            return result;
        }

        /// <summary>
        /// Insert a sub-account.
        /// </summary>
        /// <param name="subAccId">Output sub-account id</param>
        /// <param name="custId">Current main account id</param>
        /// <param name="userName">Sub-account user name</param>
        /// <param name="userPwd">Sub-account password</param>
        /// <param name="firstName">Sub-account first name</param>
        /// <param name="lastName">Sub-account last name</param>
        /// <param name="pageAccess">Sub-account permissions</param>
        /// <param name="loginSubAccId">Current sub-account id</param>
        /// <param name="userId">Login user id</param>
        /// <param name="ip">Login ip address</param>
        /// <param name="errCode">Output error code</param>
        /// <param name="language">Current language</param>
        /// <param name="errMsg">Output error message</param>
        /// <returns>Return true if succeeded, otherwise throw System.Data.SqlClient.SqlException or Sunplus.Agent.Model.DL.DataException.</returns>
        public bool Age_CustSubIns(
            out int subAccId,
            int custId,
            string userName,
            string userPwd,
            string firstName,
            string lastName,
            string pageAccess,
            int loginSubAccId,
            int userId,
            out int errCode,
            string language,
            string ip,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("subaccid", 0);
            parameters.Add("custid", custId); // Current main account id
            parameters.Add("username", userName);
            //parameters.Add("userpwd", Encryption.CfsEnCode(userPwd));
            parameters.Add("firstname", firstName);
            parameters.Add("lastname", lastName);
            parameters.Add("pageaccess", pageAccess);
            parameters.Add("success", 0);
            parameters.Add("loginsubaccid", loginSubAccId); // Current sub-account id
            parameters.Add("language", language);
            parameters.Add("userid", userId);
            parameters.Add("errcode", 0);
            parameters.Add("loginip", ip);
            parameters.Add("errmsg", string.Empty);

            Hashtable outputs = ExecSpHashtable("Age_CustSubIns", parameters);

            subAccId = (int)outputs["subaccid"];
            errCode = (int)outputs["errcode"];
            errMsg = outputs["errmsg"] as string;

            return (bool)outputs["success"];
        }

        /// <summary>
        /// Update a sub-account.
        /// </summary>
        /// <param name="subAccId">Sub-account id</param>
        /// <param name="custId">Main account id</param>
        /// <param name="firstName">Sub-account first name</param>
        /// <param name="lastName">Sub-account last name</param>
        /// <param name="closed"></param>
        /// <param name="loginSubAccId">Current login sub-account id</param>
        /// <param name="userId">Login user id</param>
        /// <param name="language">Current language</param>
        /// <param name="ip">Login ip address</param>
        /// <param name="errMsg">Output error message</param>
        /// <returns>Return error code, otherwise throw System.Data.SqlClient.SqlException or Sunplus.Agent.Model.DL.DataException.</returns>
        public int Age_CustSubUpdInfo(
            int subAccId,
            int custId,
            string firstName,
            string lastName,
            bool closed,
            int loginSubAccId,
            int userId,
            string language,
            string ip,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("subaccid", subAccId);
            parameters.Add("custid", custId);
            parameters.Add("firstname", firstName);
            parameters.Add("lastname", lastName);
            parameters.Add("closed", closed);
            parameters.Add("success", 0);
            parameters.Add("loginsubaccid", loginSubAccId);
            parameters.Add("userid", userId);
            parameters.Add("errcode", 0);
            parameters.Add("language", language);
            parameters.Add("loginip", ip);
            parameters.Add("errmsg", string.Empty);

            Hashtable outputs = ExecSpHashtable("Age_CustSubUpdInfo", parameters);

            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        /// <summary>
        /// Update page access of a sub-account.
        /// </summary>
        /// <param name="custId">Main account id</param>
        /// <param name="subAccId">Sub-account id</param>
        /// <param name="pageAccess">Sub-account permissions</param>
        /// <param name="isAddNew">Indicate in add new case or update case</param>
        /// <param name="loginSubAccId">Current login sub-account id</param>
        /// <param name="userId">Login user id</param>
        /// <param name="language">Current language</param>
        /// <param name="ip">Login ip address</param>
        /// <param name="errMsg">Output error message</param>
        /// <returns>Return error code, otherwise throw System.Data.SqlClient.SqlException or Sunplus.Agent.Model.DL.DataException.</returns>
        public int Acc_AgentUser_UpdatePermission(
            int agentId,
            int agentUserId,
            string PageAccess,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("agentid", agentId);
            parameters.Add("agentuserid", agentUserId);
            parameters.Add("pageaccess", PageAccess);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);

            Hashtable outputs = ExecSpHashtable("Acc_AgentUser_UpdatePermission", parameters);

            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        /// <summary>
        /// Update password of a sub-account.
        /// </summary>
        /// <param name="custId">Main account id</param>
        /// <param name="subAccId">Sub-account id</param>
        /// <param name="userPwd">Sub-account password</param>
        /// <param name="language">Current language</param>
        /// <param name="loginSubAccId">Login sub-account id</param>
        /// <param name="userId">User id</param>
        /// <param name="ip">Login id address</param>
        /// <param name="errMsg">Output error message</param>
        /// <returns>Return error code, otherwise throw System.Data.SqlClient.SqlException or Sunplus.Agent.Model.DL.DataException.</returns>
        public int Age_CustSubPwdUpd(
            int custId,
            int subAccId,
            string userPwd,
            string language,
            int loginSubAccId,
            int userId,
            string ip,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("subaccid", subAccId);
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            //parameters.Add("userpwd", Encryption.CfsEnCode(userPwd));
            parameters.Add("loginsubaccid", loginSubAccId);
            parameters.Add("userid", userId);
            parameters.Add("errcode", 0);
            parameters.Add("loginip", ip);
            parameters.Add("errmsg", string.Empty);

            Hashtable outputs = ExecSpHashtable("Age_CustSubPwdUpd", parameters);

            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }

        /// <summary>
        /// Change password of current sub-account.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        /// <param name="language"></param>
        /// <param name="ip"></param>
        /// <param name="subAccId"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Age_CustSubPwdUpd2(
            int custId,
            string newPassword,
            string oldPassword,
            string language,
            string ip,
            int subAccId,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("subaccid", subAccId);
            parameters.Add("custid", custId);
            parameters.Add("language", language);
            //parameters.Add("userpwd", Encryption.CfsEnCode(newPassword));
            //parameters.Add("oldpwd", Encryption.CfsEnCode(oldPassword));
            parameters.Add("errcode", 0);
            parameters.Add("loginip", ip);
            parameters.Add("errmsg", string.Empty);

            Hashtable outputs = ExecSpHashtable("Age_CustSubPwdUpd2", parameters);

            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }
    }
}