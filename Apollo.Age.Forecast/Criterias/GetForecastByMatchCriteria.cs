// --------------------------------------------------------------------------
// <copyright file="GetForecastByMatchCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.4">Created</change>
// </history>

namespace Apollo.Age.Forecast.Criterias
{
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.Enumerations;
    using Fanex.NAP.Framework.Data;

    public class GetForecastByMatchCriteria : CriteriaBase
    {
        public int AgentId { get; set; }

        public int MatchId { get; set; }

        public int LoggedId { get; set; }

        public int BetType { get; set; }

        public override string GetSettingKey()
        {
            switch (BetType)
            {
                case (int)BetTypes.Handicap:
                case (int)BetTypes._1st_Handicap:
                    // HDP
                    return "Age_Forecast_1_7_GetByMatch";
                case (int)BetTypes.Over_Under:
                case (int)BetTypes._1st_Over_Under:
                    // OU
                    return "Age_Forecast_3_8_GetByMatch";
                case (int)BetTypes.FT_1X2:
                case (int)BetTypes.FirstHalf_1X2:
                    // 1x2
                    return "Age_Forecast_5_15_GetByMatch";
            }

            return string.Empty;
        }

        public override bool IsValid()
        {
            return MatchId > 0 && BetType > 0;
        }
    }
}