// --------------------------------------------------------------------------
// <copyright file="MemberWinlossViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.12">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Services.Impl;

    public class MemberWinLossViewModel
    {
        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public string Sporttypes { get; set; }

        public string Bettypes { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<MemberWinLossModel> MemberWinLossList { get; set; }

        public List<Agent> DownlinePath { get; set; }

        public string SportName
        {
            get
            {
                var sportList = SingbetSportService.GetSport(false);
                var sport = sportList.Where(t => t.Id == this.Sporttypes).SingleOrDefault();

                if (sport != null)
                {
                    return sport.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string BettypeName
        {
            get
            {
                var bettypeList = ReportService.GetBettype();
                var bettype = bettypeList.Where(t => t.Id == this.Bettypes).SingleOrDefault();

                if (bettype != null)
                {
                    return bettype.Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
