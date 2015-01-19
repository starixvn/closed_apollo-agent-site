namespace Apollo.Age.CreditBalance.ViewModels
{
    using System;

    /// <summary>
    /// CreditBalanceAgentViewModel class.
    /// </summary>
    public class CreditBalanceViewModel
    {
        public int CustomerId { get; set; }

        public int AgentLevel { get; set; }

        public string Username { get; set; }

        public string BackLink { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }
    }
}