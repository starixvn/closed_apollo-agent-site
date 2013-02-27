namespace Fanex.Age.Excel.BetList
{
    using System.Data;
    using System.IO;
    using NPOI.HSSF.UserModel;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.UI.BetList;

    public class BetListReportBuilder
    {
        public BetListReportBuilder(BetListType betListType, DataTable data, UserTypes userType, string title, DataTable betTransm)
        {
            this.BetListType = betListType;
            this.UserType = userType;
            this.Title = title;
            this.Data = data;
            this.BetTransm = betTransm;
        }

        public BetListType BetListType { get; private set; }

        public UserTypes UserType { get; private set; }

        public DataTable Data { get; private set; }

        public DataTable BetTransm { get; private set; }

        public string Title { get; private set; }

        public MemoryStream Build()
        {
            TemplateReport report = null;
            TemplateSettings settings = new TemplateSettings();
            string filePath = settings.GetBetListPath();

            report = new BetListReport(filePath, this);

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