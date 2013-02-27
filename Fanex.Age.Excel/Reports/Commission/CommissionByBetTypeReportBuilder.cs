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
    public class CommissionByBetTypeReportBuilder
    {
        public IList<CommissionByBetTypeItem> Data { get; private set; }
        public bool IsBold { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public int RoleId { get; private set; }
        public int CustId { get; private set; }

        public CommissionByBetTypeReportBuilder(DataTable data, bool isBold, DateTime fromDate, DateTime toDate, ResourceManager resMan, int roleId, int custId)
        {
            FromDate = fromDate;
            ToDate = toDate;
            IsBold = isBold;
            RoleId = roleId;
            CustId = custId;
            var business = new CommissionByBetTypeBusiness();
            Data = business.ToList(data);
        }

        public MemoryStream Build()
        {
            TemplateReport report = null;
            TemplateSettings settings = new TemplateSettings();
            string filePath = settings.CommissionByBetTypePath();

            report = new CommissionByBetTypeReport(filePath, this);

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
