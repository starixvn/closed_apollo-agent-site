// --------------------------------------------------------------------------
// <copyright file="WagersStraightViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.6.29">Created</change>
// </history>

using Apollo.Age.Services;
namespace Apollo.Age.Wagers.ViewModels.Soccer
{
    public class WagersStraightViewModel
    {
        public string Showtime { get; set; }

        public bool LiveIndicator { get; set; }

        public int LeagueId { get; set; }

        public string LeagueName {
            get {
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        
        }

        public string matchcode { get; set; }

        public int HomeId { get; set; }

        public string HomeName
        {
            get {
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

        public string UGroup { get; set; }

        public decimal HomeStakeHDP { get; set; }

        public decimal AwayStakeHDP { get; set; }

        public decimal HomeStakeOU { get; set; }

        public decimal AwayStakeOU { get; set; }

        public decimal Stake1 { get; set; }

        public decimal StakeX { get; set; }

        public decimal Stake2 { get; set; }

        public int HomeCountHDP { get; set; }

        public int AwayCountHDP { get; set; }

        public int HomeCountOU { get; set; }

        public int AwayCountOU { get; set; }

        public int Count1 { get; set; }

        public int CountX { get; set; }

        public int Count2 { get; set; }
    }
}