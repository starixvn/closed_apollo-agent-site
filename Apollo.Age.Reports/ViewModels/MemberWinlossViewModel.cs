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

    /// <summary>
    /// Member win loss view model class.
    /// </summary>
    public class MemberWinLossViewModel
    {
        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public string SportTypeIds { get; set; }

        public string BetTypeIds { get; set; }

        public List<SingbetSport> SportList { get; set; }

        public List<SingbetBettype> BettypeList { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public DateRange ThisPeriod { get; set; }

        public DateRange LastPeriod { get; set; }

        public List<MemberWinLossModel> MemberWinLossList { get; set; }

        public List<Agent> DownlinePath { get; set; }

        /// <summary>
        /// Gets the name of the sport.
        /// </summary>
        /// <value>
        /// The name of the sport.
        /// </value>
        public string SportName
        {
            get
            {
                if (SportList != null)
                {
                    var sport = SportList.Where(t => t.Id == this.SportTypeIds).SingleOrDefault();

                    if (sport != null)
                    {
                        return sport.Name;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the name of the bet type.
        /// </summary>
        /// <value>
        /// The name of the bet type.
        /// </value>
        public string BettypeName
        {
            get
            {
                if (BettypeList != null)
                {
                    var bettype = BettypeList.Where(t => t.Id == this.BetTypeIds).SingleOrDefault();

                    if (bettype != null)
                    {
                        return bettype.Name;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
