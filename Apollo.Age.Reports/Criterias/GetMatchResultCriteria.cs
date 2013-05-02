// -----------------------------------------------------------------------
// <copyright file="GetMatchResultCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Apollo.Age.Reports.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetMatchResultCriteria : CriteriaBase
    {
        [SpParam(SqlParamName="FromDate")]
        public DateTime EventDate { get; set; }

        public int LeagueId { get; set; }

        public string Language { get; set; }

        [SpParam(SqlParamName="SportType")]
        public int SportId { get; set; }

        [SpParam(SqlParamName="IsShowAll")]
        public bool IsTest { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Result_MatchSelByLeague";
        }

        public override bool IsValid()
        {
            return this.EventDate <= DateTime.Now;
        }
    }
}
