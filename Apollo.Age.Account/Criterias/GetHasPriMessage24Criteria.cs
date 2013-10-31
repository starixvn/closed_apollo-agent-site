// --------------------------------------------------------------------------
// <copyright file="GetHasPriMessage24Criteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Get has private message criteria.
    /// </summary>
    public class GetHasPriMessage24Criteria : CriteriaBase
    {    
        [SpParam(SqlParamName = "TargetId")]
        public int UserId { get; set; }

        public bool IsAgent { get; set; }

        public bool IsTest { get; set; }

        public override bool IsValid()
        {
            return UserId > 0;
        }

        public override string GetSettingKey()
        {
            return "Dot_Msg_HasPriMessage24";
        }
    }
}
