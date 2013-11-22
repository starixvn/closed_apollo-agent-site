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
        [SpParam(SqlParamName = "targetID")]
        public int CustomerId { get; set; }

        [SpParam(SqlParamName = "targetID2")]
        public string SiteIds { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime LastPostTime { get; set; }

        public bool IsAgent { get; set; }

        public string Content { get; set; }

        public string Language { get; set; }

        [SpParam(SqlParamName = "IsTest")]
        public bool IsTest { get; set; }

        public override bool IsValid()
        {
            return this.CustomerId >= 0 && !string.IsNullOrWhiteSpace(SiteIds);
        }

        public override string GetSettingKey()
        {
            return "Age_Msg_GetByCustomer";
        }
    }
}
