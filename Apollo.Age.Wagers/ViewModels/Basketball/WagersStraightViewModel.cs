// --------------------------------------------------------------------------
// <copyright file="WagersStraightViewModel.cs" company="Nexcel Solutions Vietnam">
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
    public class WagersStraightViewModel
    {
        public string Showtime { get; set; }

        public bool LiveIndicator { get; set; }

        public int leagueid { get; set; }

        public string matchcode { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string UGroup { get; set; }

        public decimal HomeStakeHDP { get; set; }

        public decimal AwayStakeHDP { get; set; }

        public decimal HomeStakeOE { get; set; }

        public decimal AwayStakeOE { get; set; }

        public decimal HomeStakeOU { get; set; }

        public decimal AwayStakeOU { get; set; }

        public int HomeCountHDP { get; set; }

        public int AwayCountHDP { get; set; }

        public int HomeCountOE { get; set; }

        public int AwayCountOE { get; set; }

        public int HomeCountOU { get; set; }

        public int AwayCountOU { get; set; }

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
    }
}
