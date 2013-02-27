using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using Fanex.Age.DTO.Reports.MatchWinloss;
using System.Data;
using Fanex.Age.Business.Reports.MatchWinloss;
using System.IO;
using NPOI.HSSF.UserModel;

namespace Fanex.Age.Excel.Reports.MatchWinloss
{
    public class MatchWinLossDetailReportBuilder
    {
        public IList<MatchWinLossDetailItem> Data { get; private set; }
        public bool IsBold { get; private set; }
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }
        public bool IsSuper { get; private set; }
        public bool IsMaster { get; private set; }
        public bool IsAgent { get; private set; }

        public MatchWinLossDetailReportBuilder(DataTable data, bool isBold, DateTime fromDate, DateTime toDate, ResourceManager resMan,
            bool isSuper, bool isMaster, bool isAgent)
        {
            IsBold = isBold;
            FromDate = fromDate;
            ToDate = toDate;
            IsSuper = isSuper;
            IsMaster = isMaster;
            IsAgent = isAgent;
            var business = new MatchWinLossDetailBusiness(isSuper, isMaster, isAgent);
            Data = business.ToList(data);
        }

        public MemoryStream Build()
        {
            TemplateSettings settings = new TemplateSettings();
            var report = new MatchWinLossDetailReport(settings.GetMatchWinLossDetailPath(IsSuper, IsMaster, IsAgent), this);
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
