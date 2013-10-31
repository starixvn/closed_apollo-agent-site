// --------------------------------------------------------------------------
// <copyright file="ScoreMapBetTypesModel.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Forecast.Models;

    public class ScoreMapBetTypesModel
    {
        public ScoreMapBetTypesModel()
        {
            this.LiveBetTypes = new List<ScoreMapBetTypeModel>();
            this.DeadBallBetTypes = new List<ScoreMapBetTypeModel>();
        }

        public List<ScoreMapBetTypeModel> LiveBetTypes { get; set; }

        public List<ScoreMapBetTypeModel> DeadBallBetTypes { get; set; }
    }
}