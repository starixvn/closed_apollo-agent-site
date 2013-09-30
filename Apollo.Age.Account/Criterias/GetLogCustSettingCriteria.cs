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
    using System.Diagnostics.CodeAnalysis;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented")]
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

        public override string GetSettingKey()
        {
            string settingKey = string.Empty;

            switch (this.IsAgent)
            {
                case true:
                    settingKey = "ReadLog_RunSpAgent";
                    break;
                default:
                    settingKey = "ReadLog_RunSpCust";
                    break;
            }

            return settingKey;
        }

        public override bool IsValid()
        {
            var checkDate = false;

            checkDate = From <= To;

            return !string.IsNullOrEmpty(this.Username);
        }
    }
}
