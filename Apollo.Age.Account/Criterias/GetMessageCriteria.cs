// --------------------------------------------------------------------------
// <copyright file="GetMessageCriteriaCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.4.8">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GetMessageCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            string settingKey = string.Empty;

            switch (this.IsPublic)
            {
                case 0:
                    settingKey = "Dot_Msg_GetPrivateMessage";
                    break;

                case 1:
                    settingKey = "Dot_Msg_GetPublicMessage";
                    break;

                default:
                    switch (this.Special)
                    {
                        case 0:
                            settingKey = "Dot_Msg_GetNotSpecialMessage";
                            break;

                        case 1:
                            settingKey = "Dot_Msg_GetSpecialMessage";
                            break;

                    }
                    settingKey = (string.IsNullOrEmpty(settingKey) ? "Dot_Msg_GetMessage" : settingKey);
                    break;

            }

            return settingKey;
        }

        [SpParam(SqlParamName = "TargetId")]
        public int CustId { get; set; }

        [SpParam(SqlParamName = "TargetId2")]
        public string SiteId { get; set; }

        public bool IsAgent { get; set; }

        public int Special { get; set; }

        public int Record { get; set; }

        [SpParam(SqlParamName = "sdate")]
        public DateTime? From { get; set; }

        [SpParam(SqlParamName = "edate")]
        public DateTime? To { get; set; }

        public bool IsTest { get; set; }

        public int MatchId { get; set; }

        [SpParam(Ignored = true)]
        public int IsPublic { get; set; }

        [SpParam(Ignored = true)]
        public string PageQuery { get; set; }

        [SpParam(Ignored = true)]
        public int Extra { get; set; }

        public override bool IsValid()
        {
            return CustId >= 0;
        }
    }
}
