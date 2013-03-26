/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO.Reports.Common
{
    using System;

    public class StatementItem
    {
        public DateTime Date { get; set; }

        public string TransType { get; set; }

        public string TransDesc { get; set; }

        public decimal TotalAmt { get; set; }

        public long RowNum { get; set; }
    }
}