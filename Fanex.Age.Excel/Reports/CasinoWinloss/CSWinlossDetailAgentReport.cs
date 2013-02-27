namespace Fanex.Age.Excel.Reports.CasinoWinloss
{
    using System.Linq;
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Utilities;

    public class CSWinlossDetailAgentReport : TemplateReport, IReportBuilder
    {
        private CSWinlossDetailAgentReportBuilder builder;

        public CSWinlossDetailAgentReport(string filePath, CSWinlossDetailAgentReportBuilder builder)
            : base(filePath)
        {
            this.builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = this.builder.Data;
            if (data == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("CSWinlossDetailAgent");
            //// apply Bold on all fonts
            if (this.builder.IsBold)
            {
                BoldAllFonts();
            }

            int rowId = 0;
            int colId = 0;

            //// header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.lblCasinoWinLoss + " " +
                Formater.ShortDateFormat(this.builder.FromDate) + "-->" + Formater.ShortDateFormat(this.builder.ToDate));
            rowId = 1;
            resultSheet.GetRow(rowId).WriteCells(colId,
                Resources.Lang.lblGameTypeName,
                Resources.Lang.turnover,
                Resources.Lang.member, "", "",
                Resources.Lang.agent, "", "",
                Resources.Lang.company);
            rowId = 2;
            resultSheet.GetRow(rowId).WriteCells(colId + 2,
                Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total,
                Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total);

            //// content
            rowId = 3;
            int id = 1;
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    if (id % 2 == 0)
                    {
                        resultSheet.CopyRow(templateSheet, 1, rowId);
                    }
                    else
                    {
                        resultSheet.CopyRow(templateSheet, 3, rowId);
                    }

                    resultSheet.GetRow(rowId).WriteCells(colId, item.GameTypeName, item.Turnover,
                        item.MemberWinloss, item.MemberComm, item.MemberTotal,
                        item.AgentWinloss, item.AgentComm, item.AgentTotal,
                        item.Company);

                    id++;
                    rowId++;
                }

                //// total
                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetRow(rowId).WriteCells(colId, Resources.Lang.total,
                    data.Sum(p => p.Turnover),
                    data.Sum(p => p.MemberWinloss), data.Sum(p => p.MemberComm), data.Sum(p => p.MemberTotal),
                    data.Sum(p => p.AgentWinloss), data.Sum(p => p.AgentComm), data.Sum(p => p.AgentTotal),
                    data.Sum(p => p.Company));
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}