// --------------------------------------------------------------------------
// <copyright file="AgentWinlossViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.12">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System.Collections.Generic;
    using Apollo.Age.Reports.Models;
using System;

    /// <summary>
    /// Credit report view model.
    /// </summary>
    public class CreditReportViewModel
    {
        public bool IsAgent { get; set; }

        public int AgentId { get; set; }

        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public List<TransactionCreditModel> TransactionList { get; set; }
    }
}
