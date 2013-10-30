// --------------------------------------------------------------------------
// <copyright file="GetSpecialMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.4.8">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetSpecialMessageCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "TargetId")]
        public int CustId { get; set; }

        [SpParam(SqlParamName = "TargetId2")]
        public string SiteId { get; set; }

        public int Special { get; set; }

        public int Record { get; set; }

        [SpParam(SqlParamName = "sdate")]
        public DateTime? From { get; set; }

        [SpParam(SqlParamName = "edate")]
        public DateTime? To { get; set; }

        public bool IsTest { get; set; }

        public int MatchId { get; set; }        

        public override bool IsValid()
        {
            return CustId >= 0;
        }

        public override string GetSettingKey()
        {
            return "Dot_Msg_GetSpecialMessage";
        }
    }
}
