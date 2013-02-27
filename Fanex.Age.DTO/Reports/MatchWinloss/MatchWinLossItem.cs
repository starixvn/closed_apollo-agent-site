// -----------------------------------------------------------------------
// <copyright file="MatchWinLossItem.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Age.DTO.Reports.MatchWinloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLossItem
    {
        public short BetTypeId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public int MatchId { get; set; }

        public string MatchCode { get; set; }

        public string ShowTime { get; set; }

        public decimal GrossComm { get; set; }

        public decimal Comm { get; set; }

        public decimal PositionTaking { get; set; }

        public decimal Total { get; set; }

        public int LiveIndicator { get; set; }

        public bool IsNeutral { get; set; }
    }
}
