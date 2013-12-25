// --------------------------------------------------------------------------
// <copyright file="GetTopPriMessage24Criteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get top private message criteria.
    /// </summary>
    public class GetTopPriMessage24Criteria : CriteriaBase
    {
        [SpParam(SqlParamName = "TargetId")]
        public int UserId { get; set; }

        public bool IsAgent { get; set; }

        [SpParam(SqlParamName = "LatestMsgID")]
        public int LastMessageId { get; set; }

        public bool IsTest { get; set; }

        public override bool IsValid()
        {
            return UserId > 0;
        }

        public override string GetSettingKey()
        {
            return "Age_Msg_GetTopPriMessage24";
        }
    }
}
