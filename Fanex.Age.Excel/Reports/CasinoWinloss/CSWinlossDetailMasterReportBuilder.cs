using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Age.DTO.Reports.CasinoWinloss;
using System.Resources;
using System.Data;
using Fanex.Age.Business.Reports.CasinoWinloss;
using System.IO;
using NPOI.HSSF.UserModel;

namespace Fanex.Age.Excel.Reports.CasinoWinloss
{
    public class CSWinlossDetailMasterReportBuilder
    {
        public IList<CSWinlossDetailMasterItem> Data { get; private set; }
        public bool IsBold { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }

        public CSWinlossDetailMasterReportBuilder(DataTable data, bool isBold, DateTime fromDate, DateTime toDate, ResourceManager resMan)
        {
            FromDate = fromDate;
            ToDate = toDate;
            IsBold = isBold;
            var business = new CSWinlossDetailMasterBusiness();
            Data = business.ToList(data);
        }

        public MemoryStream Build()
        {
            TemplateReport report = null;
            TemplateSettings settings = new TemplateSettings();
            string filePath = settings.GetCSWinlossDetailMasterPath();

            report = new CSWinlossDetailMasterReport(filePath, this);

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
