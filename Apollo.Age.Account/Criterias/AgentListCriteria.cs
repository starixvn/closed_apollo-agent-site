namespace Apollo.Age.Account.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public class AgentListCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public string UserName { get; set; }

        public int PageSize { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetByAgent";
        }
    }
}