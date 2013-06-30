// --------------------------------------------------------------------------
// <copyright file="WagersCorrectScoreViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.29">Created</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Soccer
{
    /// <summary>
    /// for Correct Score(4), 1st Half Correct Score(30) and Half/Full(16) 
    /// </summary>
    public class WagersCorrectScoreViewModel
    {
        public string ShowTime { get; set; }

        public bool LiveIndicator { get; set; }

        public int leagueid { get; set; }

        public string matchcode { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public short Bettype { get; set; }

        public string UGroup { get; set; }

        public decimal Stake { get; set; }

        public int BetCount { get; set; }
    }
}