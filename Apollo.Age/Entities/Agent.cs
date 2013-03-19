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
        public int Level { get; set; }

        /// <summary>
        ///  alias of Agent property
        /// </summary>
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
    }
}
