// --------------------------------------------------------------------------
// <copyright file="GetRunningLeagueCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
// </history>


namespace Apollo.Age.Forecast.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetRunningLeagueCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "AgID")]
        public int AgentId { get; set; }

        public string BetTypes { get; set; }

        public override string GetSettingKey()
        {
            return "Age_League_GetRunning";
        }

        public override bool IsValid()
        {
            return AgentId > 0 && !string.IsNullOrWhiteSpace(BetTypes);
        }
    }
}
