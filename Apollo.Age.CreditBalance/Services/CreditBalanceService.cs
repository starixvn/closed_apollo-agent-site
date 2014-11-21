namespace Apollo.Age.CreditBalance.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.CreditBalance.Criterias;
    using Apollo.Age.CreditBalance.Entities;
    using Apollo.Age.Entities;
    using Apollo.Age.Services.Impl;

    /// <summary>
    /// CreditBalanceService class.
    /// </summary>
    public class CreditBalanceService : ServiceBase, ICreditBalanceService
    {
        public AgentBalanceInfo AgentGetAgentInfo(AgentGetAgentRangeInfoCriteria criteria)
        {
            return GetListData<AgentBalanceInfo>(criteria).FirstOrDefault();
        }

        public MemberBalanceInfo AgentGetMemberInfo(AgentGetMemberRangeInfoCriteria criteria)
        {
            return GetListData<MemberBalanceInfo>(criteria).FirstOrDefault();
        }

        public Result AgentTransferAgent(AgentTransferAgentCriteria criteria)
        {
            return Execute(criteria);
        }

        public Result AgentTransferMember(AgentTransferMemberCriteria criteria)
        {
            return Execute(criteria);
        }
    }
}