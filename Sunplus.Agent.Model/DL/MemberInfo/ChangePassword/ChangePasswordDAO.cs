/*
* Created 20100226@Terry, Lee - Data access functions for change password
* Revision ?@? - ... 
* 
*/

namespace Sunplus.Agent.Model.DL.MemberInfo.ChangePassword
{
    using System.Collections;
    using Sunplus.Agent.Model.DL.Base;
    using Sunplus.Agent.Utilities;

    public class ChangePasswordDAO : DAOBase
    {
        /// <summary>
        /// Change password of current sub-account.
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <param name="isDisabledCasino"></param>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        /// <param name="language"></param>
        /// <param name="ip"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public int Age_CustomerPwdUpd2(
            int custId,
            int userId,
            bool isDisabledCasino,
            string newPassword,
            string oldPassword,
            string language,
            string ip,
            out string errMsg)
        {
            Hashtable parameters = new Hashtable();
            parameters.Add("userid", userId);
            parameters.Add("custid", custId);

            // Use this field to lock upd password when casino function was disabled.
            parameters.Add("discasino", isDisabledCasino);

            parameters.Add("language", language);
            parameters.Add("userpwd", Encryption.CfsEnCode(newPassword));
            parameters.Add("oldpwd", Encryption.CfsEnCode(oldPassword));
            parameters.Add("errcode", 0);
            parameters.Add("loginip", ip);
            parameters.Add("errmsg", string.Empty);

            Hashtable outputs = ExecSpHashtable("Age_CustomerPwdUpd2", parameters);

            errMsg = outputs["errmsg"] as string;

            return (int)outputs["errcode"];
        }
    }
}