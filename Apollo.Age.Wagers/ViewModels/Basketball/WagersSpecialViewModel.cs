// --------------------------------------------------------------------------
// <copyright file="WagersSpecialViewModel.cs" company="Nexcel Solutions Vietnam">
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
    public class WagersSpecialViewModel
    {
        public string Showtime { get; set; }

        public bool LiveIndicator { get; set; }

        public int leagueid { get; set; }

        public string matchcode { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string UGroup { get; set; }

        public decimal HomeStakeMoney { get; set; }

        public decimal AwayStakeMoney { get; set; }

        public int HomeCountMoney { get; set; }

        public int AwayCountMoney { get; set; }

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
