// --------------------------------------------------------------------------
// <copyright file="WagersSpecialViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.30">Created</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Basketball
{
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SpecialViewModel : WagerViewModel
    {
        public int MatchId { get; set; }

        public int Bettype { get; set; }

        public decimal Stake { get; set; }

        public long Count { get; set; }

        public byte Type { get; set; }

        public string BetTeam { get; set; }

        public string BetName
        {
            get
            {
                return CacheRef.GetBettype(this.Bettype);
            }
        }
    }
}
