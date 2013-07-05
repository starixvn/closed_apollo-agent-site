// --------------------------------------------------------------------------
// <copyright file="WagerViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.30">Created</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels
{
    using Apollo.Age.Services;

    /// <summary>
    /// Wager view model.
    /// </summary>
    public class WagerViewModel
    {
        public string ShowTime { get; set; }

        public bool LiveIndicator { get; set; }

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        }

        public int HomeId { get; set; }

        public string HomeName
        {
            get
            {
                return CacheRef.GetTeamName(this.HomeId);
            }
        }

        public int AwayId { get; set; }

        public string AwayName
        {
            get
            {
                return CacheRef.GetTeamName(this.AwayId);
            }
        }

        public string CommGroup
        {
            get
            {
                return this.UGroup;
            }
        }

        public string UGroup { get; set; }

        public int MatchId { get; set; }
    }
}