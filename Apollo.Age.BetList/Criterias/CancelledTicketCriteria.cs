// --------------------------------------------------------------------------
// <copyright file="CancelledTicketCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Cancelled tickets bet list criteria class.
    /// </summary>
    public class CancelledTicketCriteria : CriteriaBase
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
            return "Age_BetList_SelCancelled";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}