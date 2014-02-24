// --------------------------------------------------------------------------
// <copyright file="TransactionCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2014.2.24">Created</change>
// </history>

namespace Apollo.Age.BetList.Criterias
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Transaction Criteria class.
    /// </summary>
    public class TransactionCriteria : CriteriaBase
    {
        public int LoggedId { get; set; }

        public int AgentId { get; set; }

        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime Todate { get; set; }

        public string Sporttypes { get; set; }

        public string Bettypes { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelTransaction";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}