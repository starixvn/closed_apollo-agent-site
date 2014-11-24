namespace Apollo.Age.Account.Criterias
{
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// AgentTransferMemberCriteria class.
    /// </summary>
    public class AgentTransferMemberCriteria : CriteriaBase
    {
        public int LoggedId { get; set; }

        public int CustId { get; set; }

        public int Amount { get; set; }

        public byte TransType { get; set; }

        public string IP { get; set; }

        public int AdminId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Transfer_AgentToCust";
        }

        public override bool IsValid()
        {
            return CustId > 0;
        }
    }
}