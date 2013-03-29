namespace Apollo.Age.Reports.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public class AgentOutStandingCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Rpt_Outstanding_Agent";
        }

        public override bool IsValid()
        {
            return AgentId < 0;
        }
    }
}