namespace Fanex.Age.Excel.Reports.CasinoWinloss
{
    using System.Linq;
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Utilities;

    internal class CSWinlossMasterReport : TemplateReport, IReportBuilder
    {
        private CSWinlossMasterReportBuilder _builder;

        public CSWinlossMasterReport(string filePath, CSWinlossMasterReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = _builder.Data;
            if (data == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("CSWinlossMaster");

            // apply Bold on all fonts
            if (_builder.IsBold)
            {
                BoldAllFonts();
            }

            int rowId = 0;
            int colId = 0;

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.agentRealTimeWinloss + " " +
                Formater.ShortDateFormat(_builder.FromDate) + "-->" + Formater.ShortDateFormat(_builder.ToDate));
            rowId = 1;
            resultSheet.GetRow(rowId).WriteCells(colId,
                Resources.Lang.username,
                Resources.Lang.turnover,
                Resources.Lang.lblRollingTurnOver,
                Resources.Lang.member, "", "",
                Resources.Lang.agent, "", "",
                Resources.Lang.master, "", "",
                Resources.Lang.company);
            rowId = 2;
            resultSheet.GetRow(rowId).WriteCells(colId + 3,
                Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total,
                Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total,
                Resources.Lang.win,
                Resources.Lang.com,
                Resources.Lang.total);

            // content
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

                    resultSheet.GetRow(rowId).WriteCells(colId, item.Username, item.Turnover, item.RollingTurnover,
                        item.MemberWinloss, item.MemberComm, item.MemberTotal,
                        item.AgentWinloss, item.AgentComm, item.AgentTotal,
                        item.MasterWinloss, item.MasterComm, item.MasterTotal,
                        item.Company);

                    id++;
                    rowId++;
                }

                // total
                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetRow(rowId).WriteCells(colId, Resources.Lang.total,
                    data.Sum(p => p.Turnover), data.Sum(p => p.RollingTurnover),
                    data.Sum(p => p.MemberWinloss), data.Sum(p => p.MemberComm), data.Sum(p => p.MemberTotal),
                    data.Sum(p => p.AgentWinloss), data.Sum(p => p.AgentComm), data.Sum(p => p.AgentTotal),
                    data.Sum(p => p.MasterWinloss), data.Sum(p => p.MasterComm), data.Sum(p => p.MasterTotal),
                    data.Sum(p => p.Company));
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}