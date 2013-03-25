// --------------------------------------------------------------------------
// <copyright file="AgentWinlossDTO.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.19">Created</change>
// </history>

namespace Apollo.Age.DTO.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class BaseInfo
    {
        public int ObjectId { get; set; }

        public string UserName { get; set; }

        public decimal MemberBetAmount { get; set; }

        public bool IsAgent { get; set; }

        public decimal GrossComm { get; set; }
    }

    public class AgentWinlossDTO : BaseInfo
    {
        public WinlossComm Agent { get; set; }
    }

    public class MemberWinlossDTO : AgentWinlossDTO
    {
        public WinlossComm Member { get; set; }
    }

    public class WinlossComm
    {
        public decimal Winloss { get; set; }

        public decimal Comm { get; set; }

        public decimal Total { get; set; }
    }
}
