// --------------------------------------------------------------------------
// <copyright file="ScoreMapBetTypeModel.cs" company="Nexcel Solutions Vietnam">
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

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
    public class ScoreMapBetTypesModel
    {
        public ScoreMapBetTypesModel()
        {
            LiveBetTypes = new List<ScoreMapBetTypeModel>();
            DeadBallBetTypes = new List<ScoreMapBetTypeModel>();
        }

        public List<ScoreMapBetTypeModel> LiveBetTypes { get; set; }

        public List<ScoreMapBetTypeModel> DeadBallBetTypes { get; set; }
    }
}