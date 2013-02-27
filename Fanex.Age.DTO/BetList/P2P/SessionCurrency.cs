// -----------------------------------------------------------------------
// <copyright file="SessionCurrency.cs" company="">
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
    public class SessionCurrency
    {
        public string CurrencyName { get; set; }

        public double? WinLoss { get; set; }

        public double? Comm { get; set; }

        public double? GrossComm { get; set; }
    }
}