// -----------------------------------------------------------------------
// <copyright file="MatchWinLossReportBuilder.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Age.Excel.Reports.MatchWinloss
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using Fanex.Age.DTO.Reports.MatchWinloss;
    using Fanex.Age.Business.Reports.MatchWinloss;
    using System.IO;
    using NPOI.HSSF.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLossReportBuilder
    {
        public IList<MatchWinLossItem> Data { get; private set; }
        public bool IsBold { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }

        public MatchWinLossReportBuilder(DataTable data, bool isBold, DateTime fromDate, DateTime toDate)
        {
            IsBold = isBold;
            FromDate = fromDate;
            ToDate = toDate;
            var business = new MatchWinLossBusiness();
            Data = business.ToList(data);
        }

        public MemoryStream Build()
        {
            TemplateSettings settings = new TemplateSettings();
            var report = new MatchWinLossReport(settings.GetMatchWinLossPath(), this);
            if (report != null)
            {
                if (report is IReportBuilder)
                {
                    MemoryStream mem = new MemoryStream();
                    HSSFWorkbook hssfWorkbook = ((IReportBuilder)report).Build();
                    if (hssfWorkbook != null)
                    {
                        hssfWorkbook.Write(mem);
                        return mem;
                    }
                    report.Dispose();
                }
            }

            return null;
        }
    }
}
