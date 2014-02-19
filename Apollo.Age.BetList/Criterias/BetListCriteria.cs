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
    using Apollo.BetList.Core.Entities;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Bet List Criteria.
    /// </summary>
    public class BetListCriteria : CriteriaBase
    {
        public BetListCriteria()
        {
            this.FromDate = DateTime.Today;
            this.ToDate = DateTime.Today;
        }

        /// <summary>
        /// Type of report.
        /// </summary>
        public enum ReportType : int
        {
            /// <summary>
            /// The account information.
            /// </summary>
            AccountInfo = 0,

            /// <summary>
            /// The transaction.
            /// </summary>
            Transaction = 1,

            /// <summary>
            /// The outstanding.
            /// </summary>
            Outstanding = 2,

            /// <summary>
            /// The ticket cancelled.
            /// </summary>
            TicketCancelled = 3
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public long CustId { get; set; }

        [SpParam(Ignored = true)]
        public string UserName { get; set; }

        public long AgentId { get; set; }

        public long LoggedId { get; set; }

        public long TransId { get; set; }

        public int MatchId { get; set; }

        public string StatusIds { get; set; }

        public string BetTypes { get; set; }

        public string SportTypes { get; set; }

        public string BetTeam { get; set; }

        public int? LiveIndicator { get; set; }

        [SpParam(Ignored = true)]
        public BetListPage BetListPage { get; set; }

        /// <summary>
        /// Gets the type of the report.
        /// </summary>
        /// <value>
        /// The type of the report.
        /// </value>
        public int RptType
        {
            get
            {
                switch (this.BetListPage)
                {
                    case BetListPage.AccountInfo:
                        return (int)ReportType.AccountInfo;
                    case BetListPage.Transaction:
                        return (int)ReportType.Transaction;
                    case BetListPage.Outstanding:
                        return (int)ReportType.Outstanding;
                    case BetListPage.Cancel:
                        return (int)ReportType.TicketCancelled;
                }

                return (int)ReportType.AccountInfo;
            }
        }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>Return store name.</returns>
        public override string GetSettingKey()
        {
            return "Age_BetList_SelTransaction";
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <returns>Return dynamic parameters.</returns>
        public dynamic GetParameters()
        {
            if (this.BetListPage == BetListPage.Transaction)
            {
                return new
                {
                    todate = this.ToDate,
                    fromdate = this.FromDate,
                    custid = this.CustId,
                    agentid = this.AgentId,
                    loggedid = this.LoggedId,
                    statusids = this.StatusIds,
                    bettypes = !string.IsNullOrEmpty(this.BetTypes) ? this.BetTypes : "0",
                    sporttypes = !string.IsNullOrEmpty(this.SportTypes) ? this.SportTypes : "0",
                    movebetsday = this.MoveBetsDay
                };
            }
            else
            {
                return new
                {
                    todate = this.ToDate,
                    fromdate = this.FromDate,
                    custid = this.CustId,
                    agentid = this.AgentId,
                    loggedid = this.LoggedId,
                    statusids = this.StatusIds,
                    bettypes = !string.IsNullOrEmpty(this.BetTypes) ? this.BetTypes : "0",
                    sporttypes = !string.IsNullOrEmpty(this.SportTypes) ? this.SportTypes : "0",
                    rpttype = this.RptType
                };
            }
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        /// <returns>Is validate or note.</returns>
        public override bool IsValid()
        {
            return this.ToDate >= this.FromDate;
        }
    }
}