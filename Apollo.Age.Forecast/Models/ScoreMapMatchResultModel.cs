// --------------------------------------------------------------------------
// <copyright file="ScoreMapMatchResultModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.5">Created</change>
// </history>

namespace Apollo.Age.Forecast.Models
{
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.Forecast.Core;

    public class ScoreMapMatchResultModel
    {
        public ScoreMapMatchResultModel()
        {
            this.ScoreMatrix = new decimal[Constants.ForecastResultTableDimension, Constants.ForecastResultTableDimension];
        }

        public int MatchId { get; set; }

        public string HomeName { get; set; }

        public string AwayName { get; set; }

        public decimal[,] ScoreMatrix { get; set; }

        public bool HasData { get; set; }
    }
}