// --------------------------------------------------------------------------
// <copyright file="LeagueViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.5">StyleCop</change>
// </history>

namespace Apollo.Age.Forecast.ViewModels
{
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LeagueViewModel
    {
        public int LeagueId { get; set; }

        public byte SportType { get; set; }

        private string _leagueName;
        public string LeagueName
        {
            get
            {
                if (_leagueName == null)
                {
                    _leagueName = CacheRef.GetLeagueName(this.LeagueId);
                }
                return _leagueName;
            }
        }
    }
}