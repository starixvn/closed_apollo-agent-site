namespace Apollo.Age.Reports.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Credit Report Criteria.
    /// </summary>
    public class CreditReportCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Rpt_Credit";
        }

        public override bool IsValid()
        {
            return AgentId > 0 || CustId > 0;
        }
    }
}
