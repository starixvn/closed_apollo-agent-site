/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Winloss
{
    using System;

    [Serializable]
    public class WinlossReport
    {
        public decimal TurnOver { get; set; }

        public decimal WinLost { get; set; }

        public decimal GrossComm { get; set; }

        public decimal Commision { get; set; }

        public decimal Company { get; set; }
    }
}