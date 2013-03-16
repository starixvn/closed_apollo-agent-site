using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Fanex.Age.Utility;
using System.Web;
using Fanex.Age.Utility.NPOI;
using Fanex.Age.UI;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Common
{
    internal class StatementSuperReport : TemplateReport, IReportBuilder
    {
        private StatementReportBuilder _builder;

        public StatementSuperReport(string filePath, StatementReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = _builder.Data;
            if (data == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("Statement");
            int rowId = 0;
            int colId = 0;
            hssfWorkbook.SetSheetName(0, Resources.Lang.lblStatement);

            ICellStyle negStyle1 = templateSheet.GetCell(5, 0).CellStyle;
            ICellStyle negStyle2 = templateSheet.GetCell(5, 1).CellStyle;

            // title
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.lblStatement.ToUpper() + " " +
                Formater.ShortDateFormat(_builder.FromDate) + "-->" + Formater.ShortDateFormat(_builder.ToDate));
            rowId++;

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.no);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.date);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.remark);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.totalamt);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.total);

            rowId++;
            int id = 1;
            decimal total = 0;
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    ICellStyle negStyle;
                    if (id % 2 == 0)
                    {
                        negStyle = negStyle1;
                        resultSheet.CopyRow(templateSheet, 1, rowId);
                    }
                    else
                    {
                        negStyle = negStyle2;
                        resultSheet.CopyRow(templateSheet, 3, rowId);
                    }

                    // create a No cell
                    resultSheet.GetCell(rowId, colId).SetCellValue(id);

                    // create a Date cell
                    string lowerTransType = item.TransType.ToLower();
                    string dateString = "";
                    if (lowerTransType == "pos" || lowerTransType == "com" || lowerTransType == "bal")
                    {
                        dateString = item.Date.ToString("ddddd") + "\n" + Formater.ShortDateFormat(item.Date);
                    }
                    else
                    {
                        dateString = item.Date.ToString("ddddd") + "\n" + Formater.FullDateFormat(item.Date);
                    }
                    HSSFRichTextString rtf = new HSSFRichTextString(dateString);
                    resultSheet.GetCell(rowId, colId + 1).SetCellValue(rtf);

                    // create a Remark cell
                    string remarkString = item.TransType + "\n - " + item.TransDesc;
                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(remarkString);

                    // create a TotalAmt cell
                    string upperTransDesc = item.TransDesc.ToUpper();
                    if (upperTransDesc == "OPENING BALANCE")
                    {
                        resultSheet.GetCell(rowId, colId + 3).SetCellValue("--");
                        total = item.TotalAmt;
                    }
                    else
                    {
                        resultSheet.GetCell(rowId, colId + 3).SetCellValue(DecorateMoneyNumber(item.TotalAmt));
                        if (item.TotalAmt < 0)
                        {
                            resultSheet.GetCell(rowId, colId + 3).CellStyle = negStyle;
                        }
                        total += item.TotalAmt;
                    }

                    // create a Total cell
                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(DecorateMoneyNumber(total));
                    if (total < 0)
                    {
                        resultSheet.GetCell(rowId, colId + 4).CellStyle = negStyle;
                    }

                    id++;
                    rowId++;
                }
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }

        private string DecorateMoneyNumber(decimal value)
        {
            TplBase page = HttpContext.Current.Handler as TplBase;
            if (page != null)
            {
                decimal numberMoney = Math.Round(value, 2, MidpointRounding.AwayFromZero);
                return Formater.FormatNumber3(numberMoney, page.CurrentUserSession.Language);
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
