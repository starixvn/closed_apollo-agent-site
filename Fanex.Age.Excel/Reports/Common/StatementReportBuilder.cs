using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Resources;
using System.IO;
using Fanex.Age.DTO.Reports.Common;
using Fanex.Age.Business.Reports.Common;
using NPOI.HSSF.UserModel;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Common
{
    public class StatementReportBuilder
    {
        public bool IsAgentSite { get; private set; }
        public IList<StatementItem> Data { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }

        public StatementReportBuilder(bool isAgentSite, DataTable data, DateTime fromDate, DateTime toDate)
        {
            IsAgentSite = isAgentSite;
            FromDate = fromDate;
            ToDate = toDate;
            var business = new StatementBusiness(IsAgentSite);
            Data = business.ToList(data);
        }

        public MemoryStream Build()
        {
            TemplateReport report = null;
            TemplateSettings settings = new TemplateSettings();
            string filePath = settings.GetStatementPath(IsAgentSite);

            if (IsAgentSite)
                report = new StatementAgentReport(filePath, this);
            else
                report = new StatementSuperReport(filePath, this);

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
