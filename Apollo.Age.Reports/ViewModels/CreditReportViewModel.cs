namespace Apollo.Age.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Reports.Models;

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

        public Dictionary<string, byte> UserPermission { get; set; }
    }
}
