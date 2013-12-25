// --------------------------------------------------------------------------
// <copyright file="GetTopPubMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.4.8">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get top public message criteria.
    /// </summary>
    public class GetTopPubMessageCriteria : CriteriaBase
    {
        public int TargetId { get; set; }

        public bool IsTest { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Msg_GetTopPubMessage";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
