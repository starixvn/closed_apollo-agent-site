/*
* Created 20100226@Terry, Lee - Business functions for change password
* Revision ?@? - ... 
* 
*/

namespace Sunplus.Agent.Model.BL.MemberInfo.ChangePassword
{
    using Sunplus.Agent.Model.DL.MemberInfo.ChangePassword;

    public class ChangePasswordBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        public ChangePasswordBO(string netSessionId)
            : base(netSessionId)
        {
        }

        /// <summary>
        /// Change password of current account.
        /// </summary>
        /// <returns></returns>
        public int ChangePassword(string newPassword, string oldPassword, string ip, out string errMsg)
        {
            ChangePasswordDAO dao = new ChangePasswordDAO();

            return dao.Age_CustomerPwdUpd2(
                CurrentSession.CustId,
                CurrentSession.UserId,
                Sunplus.Agent.Model.BL.Definitions.AppSettings.DisabledCasino,
                newPassword,
                oldPassword,
                CurrentSession.LangKey,
                ip,
                out errMsg);
        }
    }
}