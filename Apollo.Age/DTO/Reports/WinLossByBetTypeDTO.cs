// --------------------------------------------------------------------------
// <copyright file="WinLossByBetTypeDTO.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.19">Created</change>
// </history>


namespace Apollo.Age.DTO.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WinLossByBetTypeDTO
    {
        public string BetType { get; set; }

        public decimal MemberBetAmmount { get; set; }

        public decimal WinLoss { get; set; }

        public decimal Comm { get; set; }

        public decimal Total { get { return WinLoss + Comm; } }
    }
}
