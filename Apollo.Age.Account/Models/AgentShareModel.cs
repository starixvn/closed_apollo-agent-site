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
    public class AgentShareModel
    {
        public string Username { get; set; }

        public decimal TakenShare { get; set; }

        public decimal MaxGiven { get; set; }

        public decimal MinGiven { get; set; }
    }
}
