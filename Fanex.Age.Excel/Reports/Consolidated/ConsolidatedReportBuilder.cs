using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.Age.DTO.Reports.Consolidated;
using Fanex.Age.DTO;
using System.Resources;
using System.IO;
using NPOI.HSSF.UserModel;
using Fanex.Age.Business.Reports.Consolidated;

/*
 * Created 20120221@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Consolidated
{
    public class ConsolidatedReportBuilder
    {
        public IList<ConsolidatedItem> Data { get; private set; }
        public bool IsBold { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public eReportType ReportType { get; set; }
        public int CurrencyId { get; private set; }

        public ConsolidatedReportBuilder(eReportType reportType, int currencyId, DataTable data, bool isBold, DateTime fromDate, DateTime toDate, ResourceManager resMan)
        {
            ReportType = reportType;
            CurrencyId = currencyId;
            IsBold = isBold;
            FromDate = fromDate;
            ToDate = toDate;
            var business = new ConsolidatedBusiness();
            Data = business.ToList(data);
        }

        public MemoryStream Build()
        {
            TemplateSettings settings = new TemplateSettings();
            var report = new ConsolidatedReport(settings.GetConsolidatedPath(), this);
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
