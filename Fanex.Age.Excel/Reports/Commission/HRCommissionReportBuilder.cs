using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Age.DTO.Reports.Commission;
using System.Resources;
using System.Data;
using Fanex.Age.Business.Reports.Commission;
using System.IO;
using NPOI.HSSF.UserModel;

namespace Fanex.Age.Excel.Reports.Commission
{
    public class HRCommissionReportBuilder
    {
        public IList<HRCommissionItem> Data { get; private set; }
        public bool IsBold { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }

        public HRCommissionReportBuilder(DataTable data, bool isBold, DateTime fromDate, DateTime toDate, ResourceManager resMan)
        {
            FromDate = fromDate;
            ToDate = toDate;
            IsBold = isBold;
            var business = new HRCommissionBusiness();
            Data = business.ToList(data);
        }

        public MemoryStream Build()
        {
            TemplateReport report = null;
            TemplateSettings settings = new TemplateSettings();
            string filePath = settings.GetHRCommissionPath();

            report = new HRCommissionReport(filePath, this);

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
