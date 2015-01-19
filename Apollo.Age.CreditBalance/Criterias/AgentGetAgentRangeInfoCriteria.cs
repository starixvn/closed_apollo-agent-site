namespace Apollo.Age.CreditBalance.Criterias
{
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// AgentGetAgentRangeInfoCriteria class.
    /// </summary>
    public class AgentGetAgentRangeInfoCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetBalance";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}