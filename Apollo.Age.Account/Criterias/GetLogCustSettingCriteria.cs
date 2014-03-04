// --------------------------------------------------------------------------
// <copyright file="GetLogCustSettingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.5.16">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get log customer setting criteria.
    /// </summary>
    public class GetLogCustSettingCriteria : CriteriaBase
    {
        public int ActorAgentId { get; set; }

        public string Username { get; set; }

        public int ActionTypeId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRows { get; set; }

        [SpParam(Ignored = true)]
        public bool IsAgent { get; set; }

        [SpParam(Ignored = true)]
        public bool ShowOldValue { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            string settingKey = string.Empty;

            if (this.IsAgent)
            {
                settingKey = "ReadLog_RunSpAgent";
            }
            else
            {
                settingKey = "ReadLog_RunSpCust";
            }

            return settingKey;
        }

        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(this.Username);
        }
    }
}
