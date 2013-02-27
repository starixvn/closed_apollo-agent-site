/*
 * Created 20120223@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Commission
{
    using System;
    using System.Collections.Generic;

    public class HRCommissionItem
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal WinComm { get; set; }

        public decimal PlaceComm { get; set; }

        public decimal WinPlaceComm { get; set; }

        public decimal TotalComm { get; set; }
    }
}