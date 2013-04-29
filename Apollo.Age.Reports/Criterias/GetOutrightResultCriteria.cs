namespace Apollo.Age.Reports.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public class GetOutrightResultCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IsShowAll { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Result_Outright";
        }

        public override bool IsValid()
        {
            return this.FromDate <= ToDate;
        }
    }
}