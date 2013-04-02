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
        public int al { get; set; }
        public int hl { get; set; }
        public int dl { get; set; }
        public int ht_al { get; set; }
        public int ht_hl { get; set; }
        public int ht_dl { get; set; }
    }
}
