// --------------------------------------------------------------------------
// <copyright file="GetLoginLogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.7">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get login log criteria.
    /// </summary>
    public class GetLoginLogCriteria : CriteriaBase
    {
        public int AgentId 
        {
            get; 
            set; 
        }

        public int AgentUserId 
        {
            get; 
            set; 
        }

        public override string GetSettingKey()
        {
            return "ReadLog_Login";
        }

        public override bool IsValid()
        {
            return AgentId > 0 || AgentUserId > 0;
        }
    }
}
