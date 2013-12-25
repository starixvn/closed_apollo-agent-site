// --------------------------------------------------------------------------
// <copyright file="GetTopPubMessageByLevelCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.12.16">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get top public message by level criteria.
    /// </summary>
    public class GetTopPubMessageByLevelCriteria : CriteriaBase
    {
        public int TargetId { get; set; }

        public short AgentLevel { get; set; }

        public bool IsTest { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Msg_GetTopPubLevelMessage";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}