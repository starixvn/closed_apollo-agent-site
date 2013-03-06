using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Sunplus.Agent.Utilities;
using Fanex.Age.Utility.NPOI;
using Sunplus.Agent.Model.Entities.Permission;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Common
{
    internal class MatchResultEnteredReport : TemplateReport, IReportBuilder
    {
        private MatchResultEnteredReportBuilder _builder;

        public MatchResultEnteredReport(string filePath, MatchResultEnteredReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = _builder.Data;
            if (data == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("MatchResultEntered");
            // apply Bold on all fonts
            if (_builder.IsBold)
            {
                BoldAllFonts();
            }

            int rowId = 0;
            int colId = 0;
            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.date);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Formater.ShortDateFormat(_builder.FromDate));
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Formater.ShortDateFormat(_builder.ToDate));
            rowId = 1;
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.result);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.entered);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.nonentered);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.entered);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.nonentered);

            rowId = 2;
            int id = 1;
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    if (item.SportType == (int)Sunplus.Agent.Model.BL.Definitions.SportType.VirtualSports && !UserPermission.IsShowVirtualSports)
                    {
                        continue;
                    }

                    if (id % 2 == 0)
                    {
                        resultSheet.CopyRow(templateSheet, 1, rowId);
                    }
                    else
                    {
                        resultSheet.CopyRow(templateSheet, 3, rowId);
                    }

                    resultSheet.GetCell(rowId, colId).SetCellValue(item.SportName);
                    resultSheet.GetCell(rowId, colId + 1).SetCellValue(item.YesterdayEntered);
                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(item.YesterdayNonEntered);
                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(item.TodayEntered);
                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(item.TodayNonEntered);

                    id++;
                    rowId++;
                }
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}
