using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using Fanex.Age.DTO.Reports.Result;
using NPOI.SS.UserModel;
using Fanex.Age.Utility.NPOI;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Result
{
    public class ResultTennisReport : BaseResultReport, IReportBuilder
    {
        private ResultReportBuilder _builder;

        public ResultTennisReport(string filePath, ResultReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var leagues = (IList<SportLeague<TennisMatch>>)_builder.Data;
            if (leagues == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("Result");
            int rowId = 0;
            int colId = 0;

            // title
            WriteTitle(resultSheet, ref rowId, ref colId, _builder);

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.kickofftime);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.match);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.S1);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.S2);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.S3);
            resultSheet.GetCell(rowId, colId + 5).SetCellValue(Resources.Lang.S4);
            resultSheet.GetCell(rowId, colId + 6).SetCellValue(Resources.Lang.S5);
            resultSheet.GetCell(rowId, colId + 7).SetCellValue(Resources.Lang.totalgame);
            resultSheet.GetCell(rowId, colId + 8).SetCellValue(Resources.Lang.totalset);
            resultSheet.GetCell(rowId, colId + 9).SetCellValue(Resources.Lang.status);

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

                    // create a S1Home cell
                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(match.S1Home);
                    // create a S1Away cell
                    resultSheet.GetCell(rowId + 1, colId + 2).SetCellValue(match.S1Away);

                    // create a S2Home cell
                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(match.S2Home);
                    // create a S2Away cell
                    resultSheet.GetCell(rowId + 1, colId + 3).SetCellValue(match.S2Away);

                    // create a S3Home cell
                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(match.S3Home);
                    // create a S3Away cell
                    resultSheet.GetCell(rowId + 1, colId + 4).SetCellValue(match.S3Away);

                    // create a S4Home cell
                    resultSheet.GetCell(rowId, colId + 5).SetCellValue(match.S4Home);
                    // create a S4Away cell
                    resultSheet.GetCell(rowId + 1, colId + 5).SetCellValue(match.S4Away);

                    // create a S5Home cell
                    resultSheet.GetCell(rowId, colId + 6).SetCellValue(match.S5Home);
                    // create a S5Away cell
                    resultSheet.GetCell(rowId + 1, colId + 6).SetCellValue(match.S5Away);

                    // create a OTHome cell
                    resultSheet.GetCell(rowId, colId + 7).SetCellValue(match.TotalGamesHome > 0 ? match.TotalGamesHome.ToString() : "");
                    // create a OTAway cell
                    resultSheet.GetCell(rowId + 1, colId + 7).SetCellValue(match.TotalGamesAway > 0 ? match.TotalGamesAway.ToString() : "");

                    // create a FTHomeScore cell
                    resultSheet.GetCell(rowId, colId + 8).SetCellValue(match.FTHomeScore);
                    // create a FTAwayScore cell
                    resultSheet.GetCell(rowId + 1, colId + 8).SetCellValue(match.FTAwayScore);

                    // create a Status cell
                    resultSheet.GetCell(rowId, colId + 9).SetCellValue(Common.MapEventStatusString(match));

                    rowId += 2;
                }
            }

            //for (var i = 0; i < 10; i++)
            //{
            //    resultSheet.AutoSizeColumn(i);
            //}
            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}
