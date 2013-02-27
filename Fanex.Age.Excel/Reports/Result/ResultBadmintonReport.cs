/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Excel.Reports.Result
{
    using System.Collections.Generic;
    using Fanex.Age.DTO.Reports.Result;
    using Fanex.Age.Utility.NPOI;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public class ResultBadmintonReport : BaseResultReport, IReportBuilder
    {
        private ResultReportBuilder builder;

        public ResultBadmintonReport(string filePath, ResultReportBuilder builder)
            : base(filePath)
        {
            this.builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var leagues = (IList<SportLeague<BadmintonMatch>>)this.builder.Data;
            if (leagues == null)
            {
                return null;
            }

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("Result");
            int rowId = 0;
            int colId = 0;

            // title
            this.WriteTitle(resultSheet, ref rowId, ref colId, this.builder);

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.kickofftime);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.match);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.S1);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.S2);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.S3);
            resultSheet.GetCell(rowId, colId + 5).SetCellValue(Resources.Lang.S4);
            resultSheet.GetCell(rowId, colId + 6).SetCellValue(Resources.Lang.S5);
            resultSheet.GetCell(rowId, colId + 7).SetCellValue(Resources.Lang.totalpoint);
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
                    Common.GetFormatText(match, this.hssfWorkbook, out homeRtf, out awayRtf);
                    resultSheet.GetCell(rowId, colId + 1).SetCellValue(homeRtf);
                    resultSheet.GetCell(rowId + 1, colId + 1).SetCellValue(awayRtf);

                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(match.S1Home);
                    resultSheet.GetCell(rowId + 1, colId + 2).SetCellValue(match.S1Away);

                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(match.S2Home);
                    resultSheet.GetCell(rowId + 1, colId + 3).SetCellValue(match.S2Away);

                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(match.S3Home);
                    resultSheet.GetCell(rowId + 1, colId + 4).SetCellValue(match.S3Away);

                    resultSheet.GetCell(rowId, colId + 5).SetCellValue(match.S4Home);
                    resultSheet.GetCell(rowId + 1, colId + 5).SetCellValue(match.S4Away);

                    resultSheet.GetCell(rowId, colId + 6).SetCellValue(match.S5Home);
                    resultSheet.GetCell(rowId + 1, colId + 6).SetCellValue(match.S5Away);

                    resultSheet.GetCell(rowId, colId + 7).SetCellValue(match.TotalGamesHome > 0 ? match.TotalGamesHome.ToString() : string.Empty);
                    resultSheet.GetCell(rowId + 1, colId + 7).SetCellValue(match.TotalGamesAway > 0 ? match.TotalGamesAway.ToString() : string.Empty);

                    resultSheet.GetCell(rowId, colId + 8).SetCellValue(match.FTHomeScore);
                    resultSheet.GetCell(rowId + 1, colId + 8).SetCellValue(match.FTAwayScore);

                    resultSheet.GetCell(rowId, colId + 9).SetCellValue(Common.MapEventStatusString(match));

                    rowId += 2;
                }
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}