// --------------------------------------------------------------------------
// <copyright file="OutRightViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.1">Created</change>
// </history>


namespace Apollo.Age.TotalBets.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutRightViewModel
    {
        public int leagueid { get; set; }
        public string matchcode { get; set; }
        public int orid { get; set; }
        public int teamid { get; set; }
        public decimal balance { get; set; }
        public int BetCount { get; set; }
        public decimal totalwinlost { get; set; }
        public bool IsNeutral { get; set; }
    }
}
