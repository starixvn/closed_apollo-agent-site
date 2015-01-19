namespace Apollo.Age.Reports.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Credit Report Criteria.
    /// </summary>
    public class CreditReportOldCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Rpt_Credit_Old";
        }

        public override bool IsValid()
        {
            return AgentId > 0 || CustId > 0;
        }
    }
}
