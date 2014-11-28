namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// AgentGetMemberRangeInfoCriteria class.
    /// </summary>
    public class AgentGetMemberRangeInfoCriteria : CriteriaBase
    {
        public int CustId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Customer_GetBalance";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}