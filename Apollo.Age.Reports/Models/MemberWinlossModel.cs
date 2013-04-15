// --------------------------------------------------------------------------
// <copyright file="MemberWinlossModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.12">Created</change>
// </history>

namespace Apollo.Age.Reports.Models
{
    public class MemberWinlossModel : AgentWinlossModel
    {
        public decimal WinLoss { get; set; }

        public decimal MemComm { get; set; }

        public decimal MemTotal { get; set; }

    }
}
