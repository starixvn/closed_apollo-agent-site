namespace Apollo.Age.Reports.Models
{
    using System;

    /// <summary>
    /// Transaction Credit model class.
    /// </summary>
    [Serializable]
    public class TransactionCreditModel
    {
        public int TransId { get; set; }

        public DateTime TransDate { get; set; }

        public string TransType { get; set; }

        public decimal Amount { get; set; }

        public decimal BalanceAmount { get; set; }

        public string TransDesc { get; set; }
    }
}