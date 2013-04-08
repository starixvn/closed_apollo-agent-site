using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apollo.Age.Account.ViewModels
{
    public class CustomerListViewModel
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public string Status { get; set; }

        public decimal Handicap { get; set; }

        public decimal OverUnder { get; set; }

        public decimal HandicapLive { get; set; }

        public decimal OverUnderLive { get; set; }

        public string UserGroup { get; set; }

        public decimal CustomerGroup { get; set; }

        public decimal CustomerDiscountOneXTwo { get; set; }

        public decimal CustomerDiscountOther { get; set; }

        public decimal CustDiscountNumberGame { get; set; }

        public decimal AgentGroup { get; set; }

        public decimal AgentDiscountOneXTwo { get; set; }

        public decimal AgentDiscountOther { get; set; }

        public decimal AgentDiscountNumberGame { get; set; }
    }
}
