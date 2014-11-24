namespace Apollo.Age.Account.Criterias
{
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// AgentTransferAgentCriteria class.
    /// </summary>
    public class AgentTransferAgentCriteria : CriteriaBase
    {
        public int LoggedId { get; set; }

        public int AgentId { get; set; }

        public int Amount { get; set; }

        public byte TransType { get; set; }

        public string IP { get; set; }

        public int AdminId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Transfer_AgentToAgent";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}