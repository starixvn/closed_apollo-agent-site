/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Excel.Reports.Result
{
    using System;
    using System.Data;
    using System.IO;
    using Fanex.Age.Business.Reports.Result;
    using NPOI.HSSF.UserModel;

    public class OutrightReportBuilder : BaseResultReportBuilder
    {
        public OutrightReportBuilder(int sportTypeId, int leagueId, int betType, DataTable data, DateTime fromDate, DateTime toDate)
        {
            this.SportTypeId = sportTypeId;
            this.LeagueId = leagueId;
            this.BetType = betType;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.IsOutright = true;
            var business = new ResultBusiness(sportTypeId, leagueId);
            this.Data = business.ToList(data);
        }

        public object Data { get; private set; }

        public MemoryStream Build()
        {
            TemplateSettings settings = new TemplateSettings();
            string filePath = settings.GetOutrightTemplatePath(SportTypeId);
            var report = new OutrightReport(filePath, this);
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
                }

                report.Dispose();
            }

            return null;
        }
    }
}