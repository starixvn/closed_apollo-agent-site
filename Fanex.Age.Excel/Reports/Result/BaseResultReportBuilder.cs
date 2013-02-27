namespace Fanex.Age.Excel.Reports.Result
{
    using System;

    public class BaseResultReportBuilder
    {
        public int SportTypeId { get; protected set; }

        public int LeagueId { get; protected set; }

        public int BetType { get; protected set; }

        public DateTime FromDate { get; protected set; }

        public DateTime ToDate { get; protected set; }

        public bool IsOutright { get; protected set; }
    }
}