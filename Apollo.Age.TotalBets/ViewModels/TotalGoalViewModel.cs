// --------------------------------------------------------------------------
// <copyright file="TotalGoalViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.3.29">Implement</change>
// </history>

namespace Apollo.Age.TotalBets.ViewModels
{

    using Apollo.Age.Services;
    public class TotalGoalViewModel
    {
        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public decimal Total01 { get; set; }

        public decimal Total23 { get; set; }

        public decimal Total46 { get; set; }

        public decimal Total7Over { get; set; }

        public bool IsNeutral { get; set; }

        public bool Closed { get; set; }

        public string HomeTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.HomeId) + ((IsNeutral) ? "(N)" : string.Empty);
            }
        }

        public string AwayTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.AwayId);
            }
        }
    }
}