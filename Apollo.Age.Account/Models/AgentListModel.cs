namespace Apollo.Age.Account.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AgentListModel
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public Int16 AgentLevel { get; set; }

        public byte Status { get; set; }

        public byte UplineStatus { get; set; }

        public byte BetStatus { get; set; }

        public byte UplineBetStatus { get; set; }

        public Int32 HandicapTakenShare { get; set; }

        public Int32 HandicapGivenShare { get; set; }

        public decimal GroupA { get; set; }

        public decimal GroupB { get; set; }

        public decimal GroupC { get; set; }

        public decimal GroupD { get; set; }

        public decimal OneXTwo { get; set; }

        public decimal Other { get; set; }

        public int NumberOfAgent { get; set; }

        public int NumberOfMember { get; set; }
    }
}