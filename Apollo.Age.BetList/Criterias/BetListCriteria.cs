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
    using Fanex.BetList.Core.Entities;
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

        public string StatusIds { get; set; }

        public string BetTypes { get; set; }

        public string SportTypes { get; set; }

        public string BetTeam { get; set; }

        public int? LiveIndicator { get; set; }

        [SpParam(Ignored = true)]
        public BetListPage BetListPage { get; set; }

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

        public override string GetSettingKey()
        {
            string settingKey = string.Empty;

            switch (this.BetListPage)
            {
                case BetListPage.Outstanding:
                case BetListPage.AccountInfo:
                case BetListPage.Cancel:
                    settingKey = "Age_BetList_SelByDateMemStatus";
                    break;
                case BetListPage.Transaction:
                    settingKey = "Age_BetList_SelTransaction";
                    break;
            }

            return settingKey;
        }

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


        public override bool IsValid()
        {
            return this.ToDate >= this.FromDate;
        }
    }

    public enum ReportType : int
    {
        AccountInfo = 0,
        Transaction = 1,
        Outstanding = 2,
        TicketCancelled = 3
    }
}