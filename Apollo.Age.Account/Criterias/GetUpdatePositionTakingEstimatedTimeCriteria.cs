// --------------------------------------------------------------------------
// <copyright file="GetUpdatePositionTakingEstimatedTimeCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.9.20">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get update position taking estimated time criteria.
    /// </summary>
    public class GetUpdatePositionTakingEstimatedTimeCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public short NumOfChange { get; set; }

        public override bool IsValid()
        {
            return AgentId > 0;
        }

        public override string GetSettingKey()
        {
            return "Age_Agent_GetUpdatePositionTakingEstimatedTime";
        }
    }
}
