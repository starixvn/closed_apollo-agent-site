namespace Apollo.Age.Reports.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public class MemberOutstandingCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Rpt_Outstanding_Member";
        }

        public override bool IsValid()
        {
            return AgentId >= 0;
        }
    }
}