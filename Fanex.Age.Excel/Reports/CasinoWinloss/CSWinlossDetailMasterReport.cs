namespace Fanex.Age.Excel.Reports.CasinoWinloss
{
    using System.Linq;
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Utilities;

    internal class CSWinlossDetailMasterReport : TemplateReport, IReportBuilder
    {
        private CSWinlossDetailMasterReportBuilder builder;

        public CSWinlossDetailMasterReport(string filePath, CSWinlossDetailMasterReportBuilder builder)
            : base(filePath)
        {
            this.builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = this.builder.Data;
            //if (data == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("CSWinlossDetailMaster");

            // apply Bold on all fonts
            if (this.builder.IsBold)
            {
                BoldAllFonts();
            }

            int rowId = 0;
            int colId = 0;
            hssfWorkbook.SetSheetName(0, Resources.Lang.lblCasinoWinLoss);
            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.lblCasinoWinLoss + " " +
                Formater.ShortDateFormat(this.builder.FromDate) + "-->" + Formater.ShortDateFormat(this.builder.ToDate));
            rowId = 1;
            resultSheet.GetRow(rowId).WriteCells(colId,
                Resources.Lang.lblGameTypeName,
                Resources.Lang.turnover,
                Resources.Lang.agenttotal,
                Resources.Lang.master, "", "",
                Resources.Lang.company);
            rowId = 2;
            resultSheet.GetRow(rowId).WriteCells(colId + 3,
                Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total);

            // content
            rowId = 3;
            int id = 1;
            if (data != null && data.Count > 0)
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

                    resultSheet.GetRow(rowId).WriteCells(colId, item.GameTypeName, item.Turnover, item.AgentTotal,
                        item.MasterWinloss, item.MasterComm, item.MasterTotal,
                        item.Company);

                    id++;
                    rowId++;
                }

                // total
                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetRow(rowId).WriteCells(colId, Resources.Lang.total,
                    data.Sum(p => p.Turnover), data.Sum(p => p.AgentTotal),
                    data.Sum(p => p.MasterWinloss), data.Sum(p => p.MasterComm), data.Sum(p => p.MasterTotal),
                    data.Sum(p => p.Company));
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}