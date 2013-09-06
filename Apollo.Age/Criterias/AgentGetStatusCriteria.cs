// --------------------------------------------------------------------------
// <copyright file="AgentGetStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.29">Created</change>
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
    public class AgentGetStatusCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int AgentUserId { get; set; }

        /// <summary>
        /// Override Is Validate Parameter.
        /// </summary>
        /// <returns>Return true or false.</returns>
        public override bool IsValid()
        {
            return AgentId >= 0 && AgentUserId >= 0;
        }

        /// <summary>
        /// Override Get Setting Key.
        /// </summary>
        /// <returns>Return store procedure name.</returns>
        public override string GetSettingKey()
        {
            return "AgentAdmin_Age_Agent_GetStatus";
        }
    }
}