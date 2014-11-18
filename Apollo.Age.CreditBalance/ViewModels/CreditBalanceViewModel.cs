namespace Apollo.Age.CreditBalance.ViewModels
{
    using System;

    /// <summary>
    /// CreditBalanceAgentViewModel class.
    /// </summary>
    public class CreditBalanceViewModel
    {
        public int CustomerId { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }
    }
}