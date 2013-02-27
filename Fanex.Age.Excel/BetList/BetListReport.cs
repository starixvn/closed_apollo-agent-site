namespace Fanex.Age.Excel.BetList
{
    using NPOI.HSSF.UserModel;
    using Sunplus.Agent.UI.BetList.Factories;

    public class BetListReport : TemplateReport, IReportBuilder
    {
        private BetListReportBuilder builder;

        public BetListReport(string filePath, BetListReportBuilder builder)
            : base(filePath)
        {
            this.builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = this.builder.Data;
            if (data == null)
            {
                return null;
            }

            ExcelFactory.Render(this.builder.BetListType, ref data, this.builder.UserType, this.hssfWorkbook, this.builder.Title, this.builder.BetTransm);
            return this.hssfWorkbook;
        }
    }
}