// --------------------------------------------------------------------------
// <copyright file="UpdateReadMessageCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.4.5">Created</change>
// </history>

namespace Apollo.Age.Account.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateReadMessageCriteria : CriteriaBase
    {       
        [SpParam(SqlParamName = "TargetId")]
        public int CustomerId { get; set; }

        public bool IsAgent { get; set; }

        public string MessageIds { get; set; }

        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(this.MessageIds);
        }

        public override string GetSettingKey()
        {
            return "Age_Msg_UpdateRead";
        }
    }
}
