// --------------------------------------------------------------------------
// <copyright file="LeagueModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
//   <change who="Ricky.Nguyen" date="2013.4.5">StyleCop</change>
// </history>

namespace Apollo.Age.Forecast.Models
{
    using Apollo.Age.Services;

    /// <summary>
    /// League model.
    /// </summary>
    public class LeagueModel
    {
        private string _leagueName;

        public int LeagueId { get; set; }

        public byte SportType { get; set; }

        public string LeagueName
        {
            get
            {
                if (this._leagueName == null)
                {
                    this._leagueName = CacheRef.GetLeagueName(this.LeagueId);
                }

                return this._leagueName;
            }
        }
    }
}