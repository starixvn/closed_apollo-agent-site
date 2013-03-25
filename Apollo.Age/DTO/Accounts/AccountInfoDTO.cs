// --------------------------------------------------------------------------
// <copyright file="AcountInfoDTO.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.22">Created</change>
// </history>

namespace Apollo.Age.DTO.Accounts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AccountInfoDTO : CustomerDownline
    {
        public bool IsMember { get; set; }

        public string ParentName { get; set; }

        public bool HasMemberDownline { get; set; }

        public string UserGroup { get; set; }
    }

    public class CustomerDownline
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public int ParentId { get; set; }

        public bool HasAgentDownline { get; set; }

        public bool Enable { get; set; }
    }

    public class AgentDownlinePath
    {
        public int AgentId { get; set; }

        public string UserName { get; set; }

        public bool Enable { get; set; }
    }

    public class CustomerSearching
    {
        public string UserName { get; set; }
    }
}
