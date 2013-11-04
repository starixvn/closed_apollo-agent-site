// --------------------------------------------------------------------------
// <copyright file="AgentShareModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.17">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    /// <summary>
    /// Agent share model.
    /// </summary>
    public class AgentShareModel
    {
        public string Username { get; set; }

        public short TakenShare { get; set; }

        public short MaxGiven { get; set; }

        public short MinGiven { get; set; }

        public short AgentLevel { get; set; }
    }
}
