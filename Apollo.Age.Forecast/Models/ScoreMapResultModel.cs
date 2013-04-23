// --------------------------------------------------------------------------
// <copyright file="ScoreMapResultModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.5">Created</change>
// </history>

namespace Apollo.Age.Forecast.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.Services;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ScoreMapResultModel
    {
        public ScoreMapResultModel()
        {
            ScoreMapItems = new List<ScoreMapItem>();
            ScoreMapMatches = new List<ScoreMapMatch>();
        }

        public List<ScoreMapItem> ScoreMapItems { get; set; }

        public List<ScoreMapMatch> ScoreMapMatches { get; set; }
    }

    public class ScoreMapItem
    {
        public int MatchId { get; set; }

        public int Home { get; set; }

        public int Away { get; set; }

        public decimal Total { get; set; }
    }

    public class ScoreMapMatch
    {
        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public string HomeTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.HomeId);
            }
        }

        public string AwayTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.AwayId);
            }
        }

        private string _matchName;

        public string MatchName
        {
            get
            {
                if (_matchName == null)
                {
                    _matchName = HomeTeam + "-vs-" + AwayTeam;
                }
                return _matchName;
            }
        }
    }
}