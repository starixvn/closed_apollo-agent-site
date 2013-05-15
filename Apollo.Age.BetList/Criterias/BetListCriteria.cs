// --------------------------------------------------------------------------
// <copyright file="BetListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
// </history>

namespace Apollo.Age.BetList.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// BetList Criteria
    /// </summary>
    public class BetListCriteria : CriteriaBase
    {
        public BetListCriteria()
        {
            this.FromDate = DateTime.Today;
            this.ToDate = DateTime.Today;
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public long CustId { get; set; }

        public long AgentId { get; set; }

        public long LoggedId { get; set; }

        public long TransId { get; set; }

        public int MatchId { get; set; }

        public bool IsExportExcel { get; set; }

        public string StatusIds { get; set; }

        public string Bettypes { get; set; }

        public string BetTeam { get; set; }

        public int? LiveIndicator { get; set; }

        public override bool IsValid()
        {
            return this.ToDate >= this.FromDate;
        }
    }
}