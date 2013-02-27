/*
 * Created 20120220@Leo
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

    internal class WinlossDetailMasterReport : TemplateReport, IReportBuilder
    {
        private WinlossDetailMasterReportBuilder builder;

        public WinlossDetailMasterReport(string filePath, WinlossDetailMasterReportBuilder builder)
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
            ISheet resultSheet = hssfWorkbook.GetSheet("WinlossDetailMaster");
            //// apply Bold on all fonts
            if (this.builder.IsBold)
            {
                for (short i = 0; i < hssfWorkbook.NumberOfFonts; i++)
                {
                    var font = hssfWorkbook.GetFontAt(i);
                    font.Boldweight = (short)FontBoldWeight.BOLD;
                }
            }

            int rowId = 0;
            int colId = 0;

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.agentwinlostdetail + " " +
                Formater.ShortDateFormat(this.builder.FromDate) + "-->" + Formater.ShortDateFormat(this.builder.ToDate));
            rowId++;
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.username);
            resultSheet.GetRow(rowId).WriteCells(
                colId + 1,
                Resources.Lang.turnover,
                Resources.Lang.grosscomm,
                Resources.Lang.member);
            resultSheet.GetCell(rowId, colId + 6).SetCellValue(Resources.Lang.agent);
            resultSheet.GetCell(rowId, colId + 9).SetCellValue(Resources.Lang.master);
            resultSheet.GetCell(rowId, colId + 12).SetCellValue(Resources.Lang.company);
            rowId++;
            resultSheet.GetRow(rowId).WriteCells(
                colId + 3,
                Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total,
                Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total,
                Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total);

            if (data.Count > 0)
            {
                rowId = 3;
                int id = 1;
                decimal turnoverTotal = 0;
                decimal grosscomTotal = 0;
                decimal memberwinlossTotal = 0;
                decimal membercommTotal = 0;
                decimal memberTotal = 0;
                decimal agentwinlossTotal = 0;
                decimal agentcommTotal = 0;
                decimal agentTotal = 0;
                decimal masterwinlossTotal = 0;
                decimal mastercommTotal = 0;
                decimal masterTotal = 0;
                decimal companyTotal = 0;

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

                    resultSheet.GetRow(rowId).WriteCells(
                        colId, 
                        item.UserName,
                        item.Turnover,
                        item.GrossComm,
                        item.MemberWinloss,
                        item.MemberComm,
                        item.MemberTotal,
                        item.AgentWinloss,
                        item.AgentComm,
                        item.AgentTotal,
                        item.MasterWinloss,
                        item.MasterComm,
                        item.MasterTotal,
                        item.Company);

                    turnoverTotal += item.Turnover;
                    grosscomTotal += item.GrossComm;
                    memberwinlossTotal += item.MemberWinloss;
                    membercommTotal += item.MemberComm;
                    memberTotal += item.MemberTotal;
                    agentwinlossTotal += item.AgentWinloss;
                    agentcommTotal += item.AgentComm;
                    agentTotal += item.AgentTotal;
                    masterwinlossTotal += item.MasterWinloss;
                    mastercommTotal += item.MasterComm;
                    masterTotal += item.MasterTotal;
                    companyTotal += item.Company;

                    id++;
                    rowId++;
                }

                // total
                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetRow(rowId).WriteCells(
                    colId,
                    Resources.Lang.total,
                    turnoverTotal,
                    grosscomTotal,
                    memberwinlossTotal,
                    membercommTotal,
                    memberTotal,
                    agentwinlossTotal,
                    agentcommTotal,
                    agentTotal,
                    masterwinlossTotal,
                    mastercommTotal,
                    masterTotal,
                    companyTotal);
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}