// --------------------------------------------------------------------------
// <copyright file="Forecast1X2ViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.2">Created</change>
// </history>


namespace Apollo.Age.Forecast.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Forecast1X2ViewModel
    {
        public int MatchID { get; set; }
        public int homeid { get; set; }
        public int awayid { get; set; }
        public bool liveindicator { get; set; }
        public short livehomescore { get; set; }
        public short liveawayscore { get; set; }
        public string ShowTime { get; set; }
        public string MatchCode { get; set; }
        public bool isNeutral { get; set; }
        public decimal al { get; set; }
        public decimal hl { get; set; }
        public decimal dl { get; set; }
        public decimal ht_al { get; set; }
        public decimal ht_hl { get; set; }
        public decimal ht_dl { get; set; }

        public string HomeTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.homeid) + ((isNeutral) ? "(N)" : string.Empty);
            }
        }

        public string AwayTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.awayid);
            }
        }
    }
}
