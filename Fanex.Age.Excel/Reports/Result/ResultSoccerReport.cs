using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using Fanex.Age.DTO.Reports.Result;
using Fanex.Age.Utility.NPOI;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Result
{
    public class ResultSoccerReport : BaseResultReport, IReportBuilder
    {
        private ResultReportBuilder _builder;

        public ResultSoccerReport(string filePath, ResultReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var leagues = (IList<SportLeague<SoccerMatch>>)_builder.Data;
            if (leagues == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("Result");
            int rowId = 0;
            int colId = 0;

            hssfWorkbook.SetSheetName(0, Resources.Lang.result);
            // title
            WriteTitle(resultSheet, ref rowId, ref colId, _builder);

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.kickofftime);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.match);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.firsthalfscore);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.finalscore);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.status);

            rowId++;
            foreach (var league in leagues)
            {
                // create a LeagueId cell
                resultSheet.CopyRow(templateSheet, 1, rowId);
                string leagueName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetLeagueName(league.LeagueId);
                resultSheet.GetCell(rowId, colId).SetCellValue(leagueName.ToUpper());
                rowId++;

                foreach (var match in league.Matches)
                {
                    int id = league.Matches.IndexOf(match);
                    if (id % 2 == 0)
                    {
                        resultSheet.CopyRows(templateSheet, 3, rowId, 2);
                    }
                    else
                    {
                        resultSheet.CopyRows(templateSheet, 6, rowId, 2);
                    }

                    // create a KickOffTime cell
                    resultSheet.GetCell(rowId, colId).SetCellValue(match.KickOffTime);

                    IRichTextString homeRtf;
                    IRichTextString awayRtf;
                    Common.GetFormatText(match, hssfWorkbook, out homeRtf, out awayRtf);
                    // create a HomeId cell
                    resultSheet.GetCell(rowId, colId + 1).SetCellValue(homeRtf);
                    // create a AwayId cell
                    resultSheet.GetCell(rowId + 1, colId + 1).SetCellValue(awayRtf);

                    string HTHomeScore = match.HTHomeScore == null ? "" : match.HTHomeScore.ToString();
                    string HTAwayScore = match.HTAwayScore == null ? "" : match.HTAwayScore.ToString();
                    string FTHomeScore = match.FTHomeScore == null ? "" : match.FTHomeScore.ToString();
                    string FTAwayScore = match.FTAwayScore == null ? "" : match.FTAwayScore.ToString();
                    if (match.EventStatus == Common.Refund)
                    {
                        HTHomeScore = Common.NA;
                        HTAwayScore = Common.NA;
                        FTHomeScore = Common.NA;
                        FTAwayScore = Common.NA;
                    }
                    else if (match.EventStatus == Common.RefundFt)
                    {
                        FTHomeScore = Common.NA;
                        FTAwayScore = Common.NA;
                    }
                    else if (match.EventStatus == Common.RefundHt)
                    {
                        HTHomeScore = Common.NA;
                        HTAwayScore = Common.NA;
                    }
                    else
                    {
                    }

                    // create a HTHomeScore cell
                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(HTHomeScore);
                    // create a HTAwayScore cell
                    resultSheet.GetCell(rowId + 1, colId + 2).SetCellValue(HTAwayScore);

                    // create a FTHomeScore cell
                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(FTHomeScore);
                    // create a FTAwayScore cell
                    resultSheet.GetCell(rowId + 1, colId + 3).SetCellValue(FTAwayScore);

                    // create a Status cell
                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(Common.MapEventStatusString(match));

                    rowId += 2;
                }
            }

            //for (var i = 0; i < 5; i++)
            //{
            //    resultSheet.AutoSizeColumn(i);
            //}
            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}
