namespace Apollo.Age.Criterias.Reports.OutStandings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public class OutStandingCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Rpt_Outstanding";
        }

        public override bool IsValid()
        {
            return AgentId < 0;
        }
    }
}