/*
 * Created 20120223@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Excel.Reports.Winloss
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Resources;
    using Fanex.Age.Business.Reports.Winloss;
    using Fanex.Age.DTO.Reports.Winloss;
    using NPOI.HSSF.UserModel;

    public class WinlossByBetTypeMasterReportBuilder
    {
        public WinlossByBetTypeMasterReportBuilder(DataTable data, bool isBold, DateTime fromDate, DateTime toDate, ResourceManager resMan)
        {
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.IsBold = isBold;
            var business = new WinlossByBetTypeMasterBusiness();
            this.Data = business.ToList(data);
        }

        public IList<WinlossByBetTypeMasterItem> Data { get; private set; }

        public bool IsBold { get; private set; }

        public DateTime FromDate { get; private set; }

        public DateTime ToDate { get; private set; }

        public MemoryStream Build()
        {
            TemplateReport report = null;
            TemplateSettings settings = new TemplateSettings();
            string filePath = settings.GetWinlossByBetTypeMasterPath();

            report = new WinlossByBetTypeMasterReport(filePath, this);

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