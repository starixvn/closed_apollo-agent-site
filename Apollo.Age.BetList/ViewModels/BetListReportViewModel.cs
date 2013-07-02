// --------------------------------------------------------------------------
// <copyright file="BetListReportViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
//   <change who="Kaden.Trinh" date="2013.07.01">Add properties SportList and BettypeList</change>
// </history>
namespace Apollo.Age.BetList.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.Entities;

    public class BetListReportViewModel
    {
        public BetListReportViewModel()
        {
            UserType = 1;
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            UserType = 1;
            SportTypes = BetTypes = "0";
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string SportTypes { get; set; }

        public string BetTypes { get; set; }

        public string Username { get; set; }

        public int UserType { get; set; }

        public List<SingbetSport> SportList { get; set; }

        public List<SingbetBettype> BettypeList { get; set; }

        public List<TicketText> TicketList { get; set; }
    }
}