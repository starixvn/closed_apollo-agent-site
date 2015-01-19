namespace Apollo.Age.CreditBalance.Services
{
    using System.Collections.Generic;
    using Apollo.Age.CreditBalance.Criterias;
    using Apollo.Age.CreditBalance.Entities;
    using Apollo.Age.Entities;

    public interface ICreditBalanceService
    {
        AgentBalanceInfo AgentGetAgentInfo(AgentGetAgentRangeInfoCriteria criteria);

        MemberBalanceInfo AgentGetMemberInfo(AgentGetMemberRangeInfoCriteria criteria);

        Result AgentTransferAgent(AgentTransferAgentCriteria criteria);

        Result AgentTransferMember(AgentTransferMemberCriteria criteria);
    }
}