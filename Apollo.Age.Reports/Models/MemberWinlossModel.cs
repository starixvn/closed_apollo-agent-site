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
    using System;

    [Serializable]
    public class MemberWinLossModel : AgentWinLossModel
    {
        public int MemberId { get; set; }

        public string MemberName { get; set; }

        public decimal WinLoss { get; set; }

        public decimal MemComm { get; set; }

        public decimal MemTotal { get; set; }

        public decimal Outstanding { get; set; }

        public decimal OutstandingShare { get; set; }

        public decimal ActualTOAfter { get; set; }
    }
}
