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

    /// <summary>
    /// Score map result model.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class ScoreMapResultModel
    {
        public ScoreMapResultModel()
        {
            this.ScoreMapItems = new List<ScoreMapItem>();
            this.ScoreMapMatches = new List<ScoreMapMatch>();
        }

        public List<ScoreMapItem> ScoreMapItems { get; set; }

        public List<ScoreMapMatch> ScoreMapMatches { get; set; }
    }

    /// <summary>
    /// Score map item model.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class ScoreMapItem
    {
        public int MatchId { get; set; }

        public int Home { get; set; }

        public int Away { get; set; }

        public decimal Total { get; set; }
    }

    /// <summary>
    /// Score map match model.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class ScoreMapMatch
    {
        private string _matchName;

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

        public string MatchName
        {
            get
            {
                if (this._matchName == null)
                {
                    this._matchName = this.HomeTeam + "-vs-" + this.AwayTeam;
                }

                return this._matchName;
            }
        }
    }
}