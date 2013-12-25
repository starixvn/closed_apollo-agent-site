// --------------------------------------------------------------------------
// <copyright file="GetCustomerMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.5">Created</change>
// </history>
namespace Apollo.Age.Account.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get customer message criteria.
    /// </summary>
    public class GetCustomerMessageCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "targetID2")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "targetID")]
        public string SiteIds { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public short? AgentLevel { get; set; }

        public bool IsTest { get; set; }

        public string Content { get; set; }

        public string Language { get; set; }

        public override string GetSettingKey()
        {
            return "Age_Msg_SearchAnnouncement";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
