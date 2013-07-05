// --------------------------------------------------------------------------
// <copyright file="AgentListModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.21">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using System;

    public class AgentListModel
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public short AgentLevel { get; set; }

        public byte Status { get; set; }

        public byte UplineStatus { get; set; }

        public byte BetStatus { get; set; }

        public byte UplineBetStatus { get; set; }

        public int HandicapTakenShare { get; set; }

        public int HandicapGivenShare { get; set; }

        public decimal GroupA { get; set; }

        public decimal GroupB { get; set; }

        public decimal GroupC { get; set; }

        public decimal GroupD { get; set; }

        public decimal OneXTwo { get; set; }

        public decimal Other { get; set; }

        public int NumberOfAgent { get; set; }

        public int NumberOfMember { get; set; }
    }
}