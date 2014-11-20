namespace Apollo.Age.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class UserIdByUsernameCriteria : CriteriaBase
    {
        public int LoggedId { get; set; }

        public bool IsAgent { get; set; }

        public string Username { get; set; }

        public int AgentId { get; set; }

        public int CustId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_GetId";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Username);
        }
    }
}
