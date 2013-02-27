namespace Fanex.Age.Excel.Reports.CasinoWinloss
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Resources;
    using Fanex.Age.Business.Reports.CasinoWinloss;
    using Fanex.Age.DTO.Reports.CasinoWinloss;
    using NPOI.HSSF.UserModel;

    public class CSWinlossSuperReportBuilder
    {
        public IList<CSWinlossSuperItem> Data { get; private set; }

        public bool IsBold { get; private set; }

        public DateTime FromDate { get; private set; }

        public DateTime ToDate { get; private set; }

        public CSWinlossSuperReportBuilder(DataTable data, bool isBold, DateTime fromDate, DateTime toDate, ResourceManager resMan)
        {
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.IsBold = isBold;
            var business = new CSWinlossSuperBusiness();
            this.Data = business.ToList(data);
        }

        public MemoryStream Build()
        {
            TemplateReport report = null;
            TemplateSettings settings = new TemplateSettings();
            string filePath = settings.GetCSWinlossSuperPath();

            report = new CSWinlossSuperReport(filePath, this);

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