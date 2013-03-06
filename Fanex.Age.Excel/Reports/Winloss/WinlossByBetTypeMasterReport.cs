/*
 * Created 20120223@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Excel.Reports.Winloss
{
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Utilities;

    internal class WinlossByBetTypeMasterReport : TemplateReport, IReportBuilder
    {
        private WinlossByBetTypeMasterReportBuilder builder;

        public WinlossByBetTypeMasterReport(string filePath, WinlossByBetTypeMasterReportBuilder builder)
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

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("WinlossByBetTypeMaster");

            // apply Bold on all fonts
            if (this.builder.IsBold)
            {
                this.BoldAllFonts();
            }

            int rowId = 0;
            int colId = 0;
            hssfWorkbook.SetSheetName(0, Resources.Lang.lblWinLossByBettype);

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.lblWinLossByBettype + " " +
                Formater.ShortDateFormat(this.builder.FromDate) + "-->" + Formater.ShortDateFormat(this.builder.ToDate));
            rowId = 1;
            resultSheet.GetRow(rowId).WriteCells(
                colId,
                Resources.Lang.bettype,
                Resources.Lang.memberturnover,
                Resources.Lang.grosscomm,
                Resources.Lang.agenttotal,
                Resources.Lang.master,
                string.Empty,
                string.Empty,
                Resources.Lang.company);
            rowId = 2;
            resultSheet.GetRow(rowId).WriteCells(
                colId + 4,
                Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total);

            if (data.Count > 0)
            {
                // content
                rowId = 3;
                int id = 1;
                decimal totalGrossComm = 0;
                decimal totalTurnover = 0;
                decimal totalMasterTotal = 0;
                decimal totalWinloss = 0;
                decimal totalComm = 0;
                decimal totalAgentTotal = 0;
                decimal totalCompany = 0;
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

                    string betTypeName = CommonLibs.GetBetTypeName(item.BetType, item.LiveIndicator);
                    resultSheet.GetRow(rowId).WriteCells(
                        colId,
                        betTypeName,
                        item.Turnover,
                        item.GrossComm,
                        item.AgentTotal,
                        item.Winloss,
                        item.Comm,
                        item.MasterTotal,
                        item.Company);

                    id++;
                    rowId++;

                    totalGrossComm += item.GrossComm;
                    totalTurnover += item.Turnover;
                    totalMasterTotal += item.MasterTotal;
                    totalWinloss += item.Winloss;
                    totalComm += item.Comm;
                    totalAgentTotal += item.AgentTotal;
                    totalCompany += item.Company;
                }

                // total
                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetRow(rowId).WriteCells(
                    colId,
                    Resources.Lang.total,
                    totalTurnover,
                    totalGrossComm,
                    totalAgentTotal,
                    totalWinloss,
                    totalComm,
                    totalMasterTotal,
                    totalCompany);
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}