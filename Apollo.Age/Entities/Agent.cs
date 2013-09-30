// --------------------------------------------------------------------------
// <copyright file="Agent.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
// </history>

namespace Apollo.Age.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Agent : Customer
    {
        public short Level { get; set; }

        /// <summary>
        /// Gets or sets the parent alias of Agent property.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public Agent Parent
        {
            get
            {
                return this.Agent;
            }

            set
            {
                this.Agent = value;
            }
        }

        public List<Member> MemberList { get; set; }

        public List<Agent> AgentList { get; set; }

        public bool HasMemberDownline { get; set; }

        public bool HasAgentDownline { get; set; }
    }
}
