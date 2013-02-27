/*
 * Created 20120223@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.CasinoWinloss
{
    using System;
    using System.Collections.Generic;

    public class CSWinlossDetailSuperItem
    {
        public string GameTypeName { get; set; }

        public decimal Turnover { get; set; }

        public decimal MasterTotal { get; set; }

        public decimal SuperWinloss { get; set; }

        public decimal SuperComm { get; set; }

        public decimal SuperTotal { get; set; }

        public decimal Company { get; set; }
    }
}