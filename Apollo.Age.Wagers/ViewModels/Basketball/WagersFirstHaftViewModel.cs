// --------------------------------------------------------------------------
// <copyright file="WagersFirstHaftViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.6.30">Created</change>
// </history>

using Apollo.Age.Services;
namespace Apollo.Age.Wagers.ViewModels.Basketball
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WagersFirstHaftViewModel
    {
        public string Showtime { get; set; }

        public bool LiveIndicator { get; set; }

        public int leagueid { get; set; }

        public string matchcode { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string UGroup { get; set; }

        public decimal HomeStakeHTHDP { get; set; }

        public decimal AwayStakeHTHDP { get; set; }

        public decimal HomeStakeHTOU { get; set; }

        public decimal AwayStakeHTOU { get; set; }

        public decimal HomeStakeHTOE { get; set; }

        public decimal AwayStakeHTOE { get; set; }

        public int HomeCountHTHDP { get; set; }

        public int AwayCountHTHDP { get; set; }

        public int HomeCountHTOU { get; set; }

        public int AwayCountHTOU { get; set; }

        public int HomeCountHTOE { get; set; }

        public int AwayCountHTOE { get; set; }

        public string HomeName
        {
            get
            {
                return CacheRef.GetTeamName(this.HomeId);
            }
        }

        public string AwayName
        {
            get
            {
                return CacheRef.GetTeamName(this.AwayId);
            }
        }

        public string LeagueName
        {
            get
            {
                return CacheRef.GetTeamName(this.leagueid);
            }
        }
    }
}
