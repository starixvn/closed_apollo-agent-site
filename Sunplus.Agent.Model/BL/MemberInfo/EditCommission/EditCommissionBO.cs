// -----------------------------------------------------------------------
// <copyright file="EditCommissionBO.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Sunplus.Agent.Model.BL.MemberInfo.EditCommission
{
    using Fanex.PTEngine.BusinessObjects;
    using Sunplus.Agent.Model.DL.MemberInfo.EditCommission;
    using Sunplus.Agent.Model.BL.Base;

    /// <summary>
    /// Edit commission business object
    /// </summary>
    public class EditCommissionBO : BOBase
    {
        public EditCommissionBO(string netSessionId)
            : base(netSessionId)
        {
        }

        /// <summary>
        /// Get customer commission
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        public ICommissionSetting GetCommission(IRequestContext requestContext)
        {
            EditCommissionDAO dao = new EditCommissionDAO();

            return dao.Age_CustomerSelUserComm(requestContext);
        }

        /// <summary>
        /// Update customer commission
        /// </summary>
        /// <param name="commissionSetting"></param>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        public Result UpdateCommission(ICommissionSetting commissionSetting, IRequestContext requestContext)
        {
            EditCommissionDAO dao = new EditCommissionDAO();

            if (CurrentSession.IsSuper)
            {
                return dao.Age_CustomerUpdMasterComm(commissionSetting, requestContext);
            }
            else if (CurrentSession.IsMaster)
            {
                return dao.Age_CustomerUpdAgentComm(commissionSetting, requestContext);
            }
            else if (CurrentSession.IsAgent)
            {
                return dao.Age_CustomerUpdUserComm(commissionSetting, requestContext);
            }

            return null;
        }
    }
}
