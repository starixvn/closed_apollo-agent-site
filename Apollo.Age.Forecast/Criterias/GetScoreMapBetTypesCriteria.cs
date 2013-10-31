// --------------------------------------------------------------------------
// <copyright file="GetScoreMapBetTypesCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.5">Created</change>
// </history>

namespace Apollo.Age.Forecast.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    public class GetScoreMapBetTypesCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Age_ScoreMap_SelBettype";
        }
    }
}