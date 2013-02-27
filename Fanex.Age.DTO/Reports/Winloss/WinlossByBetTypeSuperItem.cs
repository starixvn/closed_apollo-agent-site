/*
 * Created 20120223@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Winloss
{
    using System;
    using System.Collections.Generic;

    public class WinlossByBetTypeSuperItem
    {
        public int BetType { get; set; }

        public int LiveIndicator { get; set; }

        public decimal GrossComm { get; set; }

        public decimal Turnover { get; set; }

        public decimal Winloss { get; set; }

        public decimal Comm { get; set; }

        public decimal MasterTotal { get; set; }

        public decimal SuperTotal { get; set; }

        public decimal Company { get; set; }
    }
}