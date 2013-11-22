// --------------------------------------------------------------------------
// <copyright file="CheckDownlineBelongToAgentCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.5.7">Created</change>
// </history>
namespace Apollo.Age.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Check down line belong to agent criteria.
    /// </summary>
    public class CheckDownlineBelongToAgentCriteria : CriteriaBase
    {
        public int ActorAgentId { get; set; }

        public int ObjectId { get; set; }

        public int ObjectTypeId { get; set; }

        public bool HasPermission { get; set; }

        /// <summary>
        /// Override Get Store Procedure Name.
        /// </summary>
        /// <returns>Return Store Procedure Name.</returns>
        public override string GetSettingKey()
        {
            return "Age_Agent_CheckDownlineBelongToAgent";
        }

        /// <summary>
        /// Override check is validate parameter.
        /// </summary>
        /// <returns>Return true or false.</returns>
        public override bool IsValid()
        {
            return this.ActorAgentId >= 0 || this.ObjectId >= 0;
        }
    }
}
