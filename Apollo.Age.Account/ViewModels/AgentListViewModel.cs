using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apollo.Age.Account.ViewModels
{
    public class AgentListViewModel
    {
        public int AgentId { get; set; }

        public string Username { get; set; }

        public int AgentLevel { get; set; }

        public string Status { get; set; }

        public decimal Handicap { get; set; }

        public decimal OverUnder { get; set; }

        public decimal HandicapLive { get; set; }

        public decimal OverUnderLive { get; set; }

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
