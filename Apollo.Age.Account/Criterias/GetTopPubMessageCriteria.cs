// --------------------------------------------------------------------------
// <copyright file="GetTopPubMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.4.8">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get top public message criteria.
    /// </summary>
    public class GetTopPubMessageCriteria : CriteriaBase
    {      
        public int MessageType { get; set; }

        public string Site1 { get; set; }

        public string Site2 { get; set; }

        public string Site3 { get; set; }

        public string Site4 { get; set; }

        public string Site5 { get; set; }

        [SpParam(SqlParamName = "LatestMsgID")]
        public int LastMessageId { get; set; }

        public bool IsTest { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Acc_Msg_GetTopPubMessage";
        }
    }
}
