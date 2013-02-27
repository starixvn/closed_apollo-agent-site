/*
 * Created 20120220@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Excel.Reports.WinlossAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Resources;
    using Fanex.Age.Business.Reports.WinlossAnalysis;
    using Fanex.Age.DTO;
    using Fanex.Age.DTO.Reports.WinlossAnalysis;
    using NPOI.HSSF.UserModel;

    public class WinlossAnalysisAgentReportBuilder
    {
        public WinlossAnalysisAgentReportBuilder(eReportType reportType, DataTable data, bool isBold, DateTime fromDate, DateTime toDate, ResourceManager resMan)
        {
            this.ReportType = reportType;
            this.IsBold = isBold;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            var business = new WinlossAnalysisAgentBusiness();
            this.Data = business.ToList(data);
        }

        public IList<WinlossAnalysisAgentItem> Data { get; private set; }

        public bool IsBold { get; private set; }

        public DateTime FromDate { get; private set; }

        public DateTime ToDate { get; private set; }

        public eReportType ReportType { get; set; }

        public MemoryStream Build()
        {
            TemplateSettings settings = new TemplateSettings();
            var report = new WinlossAnalysisAgentReport(settings.GetWinlossAnalysisAgentPath(), this);
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