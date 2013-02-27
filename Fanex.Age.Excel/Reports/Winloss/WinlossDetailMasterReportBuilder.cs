/*
 * Created 20120220@Leo
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

    public class WinlossDetailMasterReportBuilder
    {
        public WinlossDetailMasterReportBuilder(DataTable data, bool isBold, DateTime fromDate, DateTime toDate, ResourceManager resMan)
        {
            this.IsBold = isBold;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            var business = new WinlossDetailMasterBusiness();
            this.Data = business.ToList(data);
        }

        public IList<WinlossDetailMasterItem> Data { get; private set; }

        public bool IsBold { get; private set; }

        public DateTime FromDate { get; private set; }

        public DateTime ToDate { get; private set; }

        public MemoryStream Build()
        {
            TemplateSettings settings = new TemplateSettings();
            var report = new WinlossDetailMasterReport(settings.GetWinlossDetailMasterPath(), this);
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