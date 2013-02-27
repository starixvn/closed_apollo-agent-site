using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Sunplus.Agent.Utilities;
using Fanex.Age.Utility.NPOI;

namespace Fanex.Age.Excel.Reports.CasinoWinloss
{
    class CSWinlossSuperReport : TemplateReport, IReportBuilder
    {
        private CSWinlossSuperReportBuilder builder;

        public CSWinlossSuperReport(string filePath, CSWinlossSuperReportBuilder builder)
            : base(filePath)
        {
            this.builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = this.builder.Data;
            if (data == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("CSWinlossSuper");
            // apply Bold on all fonts
            if (this.builder.IsBold)
            {
                BoldAllFonts();
            }

            int rowId = 0;
            int colId = 0;

            //// header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.masterRealTimeWinloss + " " +
                Formater.ShortDateFormat(builder.FromDate) + "-->" + Formater.ShortDateFormat(builder.ToDate));
            rowId = 1;
            resultSheet.GetRow(rowId).WriteCells(colId,
                Resources.Lang.username,
                Resources.Lang.turnover,
                Resources.Lang.lblRollingTurnOver,
                Resources.Lang.member, "", "",
                Resources.Lang.agent, "", "",
                Resources.Lang.master, "", "",
                Resources.Lang.super, "", "",
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

                    resultSheet.GetRow(rowId).WriteCells(colId, item.Username, item.Turnover, item.RollingTurnover,
                        item.MemberWinloss, item.MemberComm, item.MemberTotal,
                        item.AgentWinloss, item.AgentComm, item.AgentTotal,
                        item.MasterWinloss, item.MasterComm, item.MasterTotal,
                        item.SuperWinloss, item.SuperComm, item.SuperTotal,
                        item.Company);

                    id++;
                    rowId++;
                }

                //// total
                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetRow(rowId).WriteCells(colId, Resources.Lang.total,
                    data.Sum(p => p.Turnover), data.Sum(p => p.RollingTurnover),
                    data.Sum(p => p.MemberWinloss), data.Sum(p => p.MemberComm), data.Sum(p => p.MemberTotal),
                    data.Sum(p => p.AgentWinloss), data.Sum(p => p.AgentComm), data.Sum(p => p.AgentTotal),
                    data.Sum(p => p.MasterWinloss), data.Sum(p => p.MasterComm), data.Sum(p => p.MasterTotal),
                    data.Sum(p => p.SuperWinloss), data.Sum(p => p.SuperComm), data.Sum(p => p.SuperTotal),
                    data.Sum(p => p.Company));
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}
