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

    public class AgentWinlossDTO
    {
        public int AgentId { get; set; }

        public string User { get; set; }

        public decimal MemberBetAmmount { get; set; }

        public decimal GrossComm { get; set; }

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

        public decimal Total
        {
            get
            {
                return Winloss + Comm;
            }
        }
    }
}
