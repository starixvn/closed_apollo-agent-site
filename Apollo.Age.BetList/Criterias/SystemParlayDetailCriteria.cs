// --------------------------------------------------------------------------
// <copyright file="SystemParlayDetailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.16">System Parlay Detail Criteria</change>
// </history>

namespace Apollo.Age.BetList.Criterias
{
    using System;

    /// <summary>
    /// System parlay detail criteria.
    /// </summary>
    public class SystemParlayDetailCriteria
    {
        public long RefNo { get; set; }

        public int CustId { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string Language { get; set; }
    }
}