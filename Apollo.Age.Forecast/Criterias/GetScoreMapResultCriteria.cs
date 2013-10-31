// --------------------------------------------------------------------------
// <copyright file="GetScoreMapResultCriteria.cs" company="Nexcel Solutions Vietnam">
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

    public class GetScoreMapResultCriteria : CriteriaBase
    {
        public string MatchIds { get; set; }

        public int LoggedId { get; set; }

        public int AgentId { get; set; }

        [SpParam(SqlParamName = "bettypes_live")]
        public string LiveBetTypes { get; set; }

        [SpParam(SqlParamName = "bettypes_dead")]
        public string DeadBetTypes { get; set; }

        public override string GetSettingKey()
        {
            return "Age_ScoreMap_Result";
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(MatchIds);
        }
    }
}