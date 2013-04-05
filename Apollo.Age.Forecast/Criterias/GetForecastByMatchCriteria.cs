// --------------------------------------------------------------------------
// <copyright file="GetForecastHandicapByMatch.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.4">Created</change>
// </history>


namespace Apollo.Age.Forecast.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetForecastByMatchCriteria : CriteriaBase
    {

        public int AgentId { get; set; }

        public int MatchId { get; set; }

        public int BetType { get; set; }

        public override string GetSettingKey()
        {
            switch (BetType)
            {
                case 1:
                case 7:
                    // HDP
                    return "Age_Forecast_1_7_GetByMatch";
                case 3:
                case 8:
                    // OU
                    return "Age_Forecast_3_8_GetByMatch";
                case 5:
                case 15:
                    // 1x2
                    return "Age_Forecast_5_15_GetByMatch";
                    
            }
            return string.Empty;
        }

        public override bool IsValid()
        {
            return AgentId > 0 && MatchId > 0 && BetType > 0;
        }
    }
}
