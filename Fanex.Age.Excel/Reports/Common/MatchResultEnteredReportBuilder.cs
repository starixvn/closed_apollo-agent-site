using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Resources;
using System.IO;
using NPOI.HSSF.UserModel;
using Fanex.Age.DTO.Reports.Common;
using Fanex.Age.Business.Reports.Common;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Common
{
    public class MatchResultEnteredReportBuilder
    {
        public IList<MatchResultEnteredItem> Data { get; private set; }
        public bool IsBold { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }

        public MatchResultEnteredReportBuilder(DateTime fromDate, DateTime toDate, bool isBold, DataTable data, ResourceManager resMan)
        {
            FromDate = fromDate;
            ToDate = toDate;
            IsBold = isBold;
            var business = new MatchResultEnteredBusiness();
            Data = business.ToList(data);
        }

        public MemoryStream Build()
        {
            TemplateReport report = null;
            TemplateSettings settings = new TemplateSettings();
            string filePath = settings.GetMatchResultEntered();

            report = new MatchResultEnteredReport(filePath, this);

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
