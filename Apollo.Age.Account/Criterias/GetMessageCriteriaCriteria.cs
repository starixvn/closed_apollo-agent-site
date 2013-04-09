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
    public class GetMessageCriteriaCriteria : CriteriaBase
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
                    switch (this.IsSpecial)
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

        public int TargetId { get; set; }

        public string TargetId2 { get; set; }

        public int Special { get; set; }

        public int Record { get; set; }

        [SpParam(SqlParamName = "edate")]
        public DateTime From { get; set; }

        [SpParam(SqlParamName = "fdate")]
        public DateTime To { get; set; }

        public bool IsTest { get; set; }

        public int MatchId { get; set; }

        [SpParam(Ignored = true)]
        public int IsPublic { get; set; }

        [SpParam(Ignored = true)]
        public int IsSpecial { get; set; }

        public override bool IsValid()
        {
            return TargetId > 0;
        }
    }
}
