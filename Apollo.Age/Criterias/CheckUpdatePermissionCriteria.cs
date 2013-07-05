// --------------------------------------------------------------------------
// <copyright file="CheckUpdatePermissionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.5.7">Created</change>
// </history>
namespace Apollo.Age.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CheckUpdatePermissionCriteria : CriteriaBase
    {
        public int ActorAgentId { get; set; }

        public int ObjectId { get; set; }

        public int ObjectTypeId { get; set; }

        public bool HasPermission { get; set; }

        public override string GetSettingKey()
        {
            return "AgentAdmin_Age_Agent_CheckUpdatePermission";
        }

        public override bool IsValid()
        {
            return this.ActorAgentId >= 0 || this.ObjectId >= 0;
        }
    }
}
