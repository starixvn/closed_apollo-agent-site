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
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.Entities;
    using Fanex.Common;
    using Fanex.NAP.Framework.Data;
    using Apollo.Age.Reports.Models;

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

        [SpParam(Ignored = true)]
        public int PageIndex { get; set; }

        [SpParam(Ignored = true)]
        public int PageSize
        {
            get
            {
                int pageSize = Converter.ToInt(ConfigurationManager.AppSettings["PageSize"], 40);

                return pageSize;
            }
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public DateRangeModel ThisPeriod { get; set; }

        public DateRangeModel LastPeriod { get; set; }

        public string SportTypes { get; set; }

        public string BetTypes { get; set; }

        public string Username { get; set; }

        public int UserType { get; set; }

        public List<SingbetSport> SportList { get; set; }

        public List<SingbetBettype> BettypeList { get; set; }

        public List<TicketText> TicketList { get; set; }

        public bool FromPaging { get; set; }
    }
}