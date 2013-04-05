// --------------------------------------------------------------------------
// <copyright file="LeagueViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
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
    public class LeagueViewModel
    {
        public int LeagueId { get; set; }
        public byte SportType { get; set; }
        public string LeagueName
        {
            get
            {
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        }
    }
}
