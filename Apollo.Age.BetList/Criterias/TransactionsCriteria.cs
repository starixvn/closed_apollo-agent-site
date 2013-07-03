// --------------------------------------------------------------------------
// <copyright file="TransactionsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.07.03">Created and Implement</change>
// </history>
namespace Apollo.Age.BetList.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    public class TransactionsCriteria : CriteriaBase
    {
        public TransactionsCriteria()
        {
            this.FromDate = DateTime.Today;
            this.ToDate = DateTime.Today;
        }

        public int AgentId { get; set; }

        public int LoggedId { get; set; }

        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public string StatusIds { get; set; }

        public string BetTypes { get; set; }

        public string SportTypes { get; set; }

        [SpParam(Ignored = true)]
        public bool IsTransactions { get; set; }

        public override string GetSettingKey()
        {
            string settingKey = string.Empty;

            switch (this.IsTransactions)
            {
                case false:
                    settingKey = "Age_BetList_SelByDateMemStatus";
                    break;
                default:
                    settingKey = "Age_BetList_SelTransaction";
                    break;
            }

            return settingKey;
        }

        public override bool IsValid()
        {
            return this.ToDate >= this.FromDate;
        }
    }
}