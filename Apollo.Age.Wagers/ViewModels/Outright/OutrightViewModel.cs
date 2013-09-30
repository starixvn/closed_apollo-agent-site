// --------------------------------------------------------------------------
// <copyright file="OutrightViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.7.2">Created</change>
//   <change who="Terry.Nguyen" date="2013.7.18">Add Stake Before</change>
// </history>

namespace Apollo.Age.Wagers.ViewModels.Outright
{
    using System;
    using Apollo.Age.Services;

    /// <summary>
    /// Outright view model.
    /// </summary>
    public class OutrightViewModel : WagerViewModel
    {
        public DateTime EventDate { get; set; }
        
        public int TeamId { get; set; }

        public string TeamName 
        {
            get
            {
                return CacheRef.GetTeamName(this.TeamId);
            }
        }

        public int Orid { get; set; }

        public short Bettype { get; set; }

        public decimal Stake { get; set; }

        public decimal StakeBefore { get; set; }

        public string BetTeam { get; set; }

        public int BetCount { get; set; }

        public string Status { get; set; }
    }
}