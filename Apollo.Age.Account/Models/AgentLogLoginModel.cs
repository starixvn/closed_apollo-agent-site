// --------------------------------------------------------------------------
// <copyright file="AgentLogLoginModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.7">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using System;

    /// <summary>
    /// Agent log login model.
    /// </summary>
    public class AgentLogLoginModel
    {
        public long Logid { get; set; }

        public string Ip { get; set; }

        public DateTime LogDate { get; set; }

        public int ActionId { get; set; }

        public string Doing { get; set; }

        public string DomainName { get; set; }

        public string BrowserName { get; set; }

        public string Platform { get; set; }
    }
}