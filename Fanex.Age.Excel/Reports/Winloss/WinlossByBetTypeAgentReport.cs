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

    internal class WinlossByBetTypeAgentReport : TemplateReport, IReportBuilder
    {
        private WinlossByBetTypeAgentReportBuilder builder;

        public WinlossByBetTypeAgentReport(string filePath, WinlossByBetTypeAgentReportBuilder builder)
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
            ISheet resultSheet = hssfWorkbook.GetSheet("WinlossByBetTypeAgent");

            // apply Bold on all fonts
            if (this.builder.IsBold)
            {
                this.BoldAllFonts();
            }

            int rowId = 0;
            int colId = 0;

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.lblWinLossByBettype + " " +
                Formater.ShortDateFormat(this.builder.FromDate) + "-->" + Formater.ShortDateFormat(this.builder.ToDate));
            rowId = 1;
            resultSheet.GetRow(rowId).WriteCells(
                colId,
                Resources.Lang.bettype,
                Resources.Lang.turnover,
                Resources.Lang.grosscomm,
                Resources.Lang.member,
                string.Empty,
                string.Empty,
                Resources.Lang.agent,
                string.Empty,
                string.Empty,
                Resources.Lang.company);

            rowId = 2;
            resultSheet.GetRow(rowId).WriteCells(
                colId + 3,
                Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total,
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
                decimal totalMemberWinloss = 0;
                decimal totalMemberComm = 0;
                decimal totalMemberTotal = 0;
                decimal totalAgentWinloss = 0;
                decimal totalAgentComm = 0;
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
                        item.MemberWinloss,
                        item.MemberComm,
                        item.MemberTotal,
                        item.AgentWinloss,
                        item.AgentComm,
                        item.AgentTotal,
                        item.Company);

                    id++;
                    rowId++;

                    totalGrossComm += item.GrossComm;
                    totalTurnover += item.Turnover;
                    totalMemberWinloss += item.MemberWinloss;
                    totalMemberComm += item.MemberComm;
                    totalMemberTotal += item.MemberTotal;
                    totalAgentWinloss += item.AgentWinloss;
                    totalAgentComm += item.AgentComm;
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
                    totalMemberWinloss,
                    totalMemberComm,
                    totalMemberTotal,
                    totalAgentWinloss,
                    totalAgentComm, 
                    totalAgentTotal,
                    totalCompany);
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}