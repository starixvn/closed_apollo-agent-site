// --------------------------------------------------------------------------
// <copyright file="AgentTreeViewNode.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.7.18">Created</change>
// </history>

namespace Apollo.Age.Account.Entities
{
    using System.Collections.Generic;

    /// <summary>
    /// Agent tree view node.
    /// </summary>
    public class AgentTreeViewNode
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public bool HasAgentDownline { get; set; }

        public List<AgentTreeViewNode> Children { get; set; }

        public bool Expanded { get; set; }

        public string Classes { get; set; }
    }
}
