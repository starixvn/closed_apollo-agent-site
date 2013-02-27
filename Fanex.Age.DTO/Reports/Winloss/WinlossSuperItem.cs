/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Winloss
{
    using System;
    using System.Collections.Generic;

    public class WinlossSuperItem
    {
        public string UserName { get; set; }

        public decimal Turnover { get; set; }

        public decimal GrossComm { get; set; }

        public decimal MasterTotal { get; set; }

        public decimal SuperWinloss { get; set; }

        public decimal SuperComm { get; set; }

        public decimal SuperTotal { get; set; }

        public decimal Company { get; set; }
    }
}