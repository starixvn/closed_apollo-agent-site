// -----------------------------------------------------------------------
// <copyright file="AgentWinlossCriteria.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Apollo.Age.Criterias.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;
    using System.Collections;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgentWinlossCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int AgentId { get; set; }

        public int ViewedBy { get; set; }

        public override bool IsValid()
        {
            return base.IsValid();
        }
    }
}
