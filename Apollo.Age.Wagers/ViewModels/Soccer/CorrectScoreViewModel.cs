// --------------------------------------------------------------------------
// <copyright file="CorrectScoreViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.29">Created</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Soccer
{
    /// <summary>
    /// For Correct Score(4), 1st Half Correct Score(30) and Half/Full(16).
    /// </summary>
    public class CorrectScoreViewModel : WagerViewModel
    {
        public short Bettype { get; set; }                

        public decimal Stake { get; set; }

        public int BetCount { get; set; }
    }
}