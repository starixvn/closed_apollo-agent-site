namespace Apollo.Age.Account.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AgentListViewModel
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public Int16 AgentLevel { get; set; }

        public Int32 Status { get; set; }

        public Int32 Handicap { get; set; }

        public Int32 OverUnder { get; set; }

        public Int32 HandicapLive { get; set; }

        public Int32 OverUnderLive { get; set; }

        public decimal GroupA { get; set; }

        public decimal GroupB { get; set; }

        public decimal GroupC { get; set; }

        public decimal GroupD { get; set; }

        public decimal OneXTwo { get; set; }

        public decimal NumberGame { get; set; }

        public decimal Other { get; set; }

        public int NumberOfAgent { get; set; }

        public int NumberOfMember { get; set; }
    }
}