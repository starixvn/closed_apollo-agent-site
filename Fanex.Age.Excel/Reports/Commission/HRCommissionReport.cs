using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Sunplus.Agent.Utilities;
using Fanex.Age.Utility.NPOI;

namespace Fanex.Age.Excel.Reports.Commission
{
    class HRCommissionReport : TemplateReport, IReportBuilder
    {
        private HRCommissionReportBuilder _builder;

        public HRCommissionReport(string filePath, HRCommissionReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = _builder.Data;
            if (data == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("HRCommission");
            // apply Bold on all fonts
            if (_builder.IsBold)
            {
                BoldAllFonts();
            }

            int rowId = 0;
            int colId = 0;

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.lblcommssionhorseracing + " " +
                Formater.ShortDateFormat(_builder.FromDate) + "-->" + Formater.ShortDateFormat(_builder.ToDate));
            rowId = 1;
            resultSheet.GetRow(rowId).WriteCells(colId,
                Resources.Lang.username,
                Resources.Lang.tote);
            rowId = 2;
            resultSheet.GetRow(rowId).WriteCells(colId + 1,
                Resources.Lang.WinHR,
                Resources.Lang.Place,
                Resources.Lang.eway,
                Resources.Lang.total);

            // content
            rowId = 3;
            int id = 1;
            if (data.Count > 0)
            {
                decimal totalWinComm = 0;
                decimal totalPlaceComm = 0;
                decimal totalWinPlaceComm = 0;
                decimal totalTotalComm = 0;
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

                    resultSheet.GetRow(rowId).WriteCells(colId, item.Username, item.WinComm, item.PlaceComm, item.WinPlaceComm, item.TotalComm);

                    id++;
                    rowId++;

                    totalWinComm += item.WinComm;
                    totalPlaceComm += item.PlaceComm;
                    totalWinPlaceComm += item.WinPlaceComm;
                    totalTotalComm += item.TotalComm;
                }

                // total
                resultSheet.CopyRow(templateSheet, 5, rowId);
                resultSheet.GetRow(rowId).WriteCells(colId, Resources.Lang.total,
                    totalWinComm, totalPlaceComm, totalWinPlaceComm, totalTotalComm);
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}
