// -----------------------------------------------------------------------
// <copyright file="MatchWinLossReport.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Age.Excel.Reports.MatchWinloss
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Model.BL.Cache;
    using Sunplus.Agent.Utilities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchWinLossReport : TemplateReport, IReportBuilder
    {
        private MatchWinLossReportBuilder _builder;

        public MatchWinLossReport(string filePath, MatchWinLossReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var data = _builder.Data;
            if (data == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("MatchWinLoss");

            // apply Bold on all fonts
            if (_builder.IsBold)
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
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.matchwinloss + " " + Formater.ShortDateFormat(_builder.FromDate) + "-->" + Formater.ShortDateFormat(_builder.ToDate));

            rowId = 1;
            var values = new List<object>{Resources.Lang.match,
                Resources.Lang.bettype,
                Resources.Lang.grosscomm,
                Resources.Lang.com,
                Resources.Lang.positiontaking,
                Resources.Lang.total};
            resultSheet.GetRow(rowId).WriteCells(colId, values);

            // all content
            if (data.Count > 0)
            {
                rowId = 2;
                int id = 1;

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

                    string eventName = string.Empty;
                    if ((item.BetTypeId > 50 && item.BetTypeId < 57) || (item.BetTypeId > 60 && item.BetTypeId < 67)) // Finance
                    {
                        eventName = CacheRef.GetFncIndexName(item.HomeId);
                    }
                    else if (item.BetTypeId > 80 && item.BetTypeId < 121) // Number Game
                    {
                        eventName = Resources.Lang.numbergameno + " " + item.MatchCode;
                    }
                    else if (item.BetTypeId > 1100 && item.BetTypeId < 1104) // For Live Casino
                    {
                        eventName = Resources.Lang.LiveCasino + ": " + CacheRef.GetBettype(item.BetTypeId) + " " + item.ShowTime;
                    }
                    else
                    {
                        eventName = CacheRef.GetTeamName(item.HomeId) + (item.IsNeutral ? Resources.Lang.IsNeutral : string.Empty) + " -vs- " + CacheRef.GetTeamName(item.AwayId);
                    }

                    string strTypeNameNG = string.Empty;
                    switch (item.BetTypeId)
                    {
                        case (int)Sunplus.Agent.Model.BL.Definitions.BetType.N_Over_Under: //85
                            strTypeNameNG = Resources.Lang.next + " ";
                            break;

                        case (int)Sunplus.Agent.Model.BL.Definitions.BetType.N_Odd_Even: // 86
                            if (item.LiveIndicator == 1)
                            {
                                strTypeNameNG = Resources.Lang.next + " ";
                            }
                            else
                            {
                                strTypeNameNG = Resources.Lang.ft + ". ";
                            }

                            break;
                    }

                    string betTypeName = strTypeNameNG + CacheRef.GetBettype(item.BetTypeId);

                    resultSheet.GetRow(rowId).WriteCells(colId,
                        eventName,
                        betTypeName,
                        item.GrossComm,
                        item.Comm,
                        item.PositionTaking,
                        item.Total);

                    id++;
                    rowId++;
                }
            }

            // total
            resultSheet.CopyRow(templateSheet, 5, rowId);
            resultSheet.GetRow(rowId).WriteCells(colId,
                Resources.Lang.total,
                "",
                data.Sum(p => p.GrossComm),
                data.Sum(p => p.Comm),
                data.Sum(p => p.PositionTaking),
                data.Sum(p => p.Total));

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}