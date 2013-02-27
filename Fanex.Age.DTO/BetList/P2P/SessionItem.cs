// -----------------------------------------------------------------------
// <copyright file="SessionItem.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Age.DTO.BetList.P2P
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SessionItem
    {
        public DateTime Date { get; set; }

        public IdName Table { get; set; }

        public IdName Tournament { get; set; }

        public string GamingId { get; set; }

        public string GameMode { get; set; }

        public SessionCurrency TableCurrency { get; set; }

        public double? XRate { get; set; }

        public SessionCurrency UserCurrency { get; set; }
    }
}