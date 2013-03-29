// --------------------------------------------------------------------------
// <copyright file="GetTotalGoalCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.TotalBets.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetTotalGoalCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public override string GetSettingKey()
        {
            return "Age_TotalBets_6";
        }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
