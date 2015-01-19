namespace Apollo.Age.Account.ViewModels
{
    using Apollo.Age.Resources;

    /// <summary>
    /// CreditBalanceAgentViewModel class.
    /// </summary>
    public class DepositViewModel
    {
        public int CustId { get; set; }

        public int AgentId { get; set; }

        public int ParentAgentId { get; set; }

        public int AgentLevel { get; set; }

        public string Username { get; set; }

        public string BackLink { get; set; }

        public decimal MaxAmount { get; set; }

        public bool IsDisable { get; set; }

        public string ArrangeText
        {
            get
            {
                if (AgentLevel <= Enumerations.AgentLevel.MainShareholder && CustId == 0)
                {
                    return string.Empty;
                }
                else
                {
                    if (MaxAmount > 0)
                    {
                        return string.Join(string.Empty, "<= ", MaxAmount.ToString("#,###.##"));
                    }
                    else
                    {
                        IsDisable = true;
                        return string.Format("<span class='red'>{0}</span>", Lang.DepositNotAvailable);
                    }
                }
            }
        }
    }
}