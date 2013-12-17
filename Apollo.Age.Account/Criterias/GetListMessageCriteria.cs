// --------------------------------------------------------------------------
// <copyright file="GetListMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.12.17">Created</change>
// </history>
namespace Apollo.Age.Account.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get top public message by level criteria.
    /// </summary>
    public class GetListMessageCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "targetID2")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "targetID")]
        public string SiteIds { get; set; }

        [SpParam(SqlParamName = "Sdate")]
        public DateTime StartDate { get; set; }

        [SpParam(SqlParamName = "Edate")]
        public DateTime EndDate { get; set; }

        public bool IsSpecial { get; set; }

        public byte MsgType { get; set; }

        public bool IsTest { get; set; }

        public int Record { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Msg_GetListAnnouncement";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}