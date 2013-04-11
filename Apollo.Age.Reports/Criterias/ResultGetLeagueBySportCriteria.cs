// -----------------------------------------------------------------------
// <copyright file="ResultGetLeagueBySportCriteria.cs" company="">
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
    public class ResultGetLeagueBySportCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Age_Result_LeagueSelByDate";
        }

        public DateTime EventDate { get; set; }

        public byte SportType { get; set; }

        public bool IsShowAll { get; set; }

        public override bool IsValid()
        {
            return this.EventDate <= DateTime.Now;
        }
    }
}
