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

    public class ResultBaseBallReport : BaseResultReport, IReportBuilder
    {
        private ResultReportBuilder builder;

        public ResultBaseBallReport(string filePath, ResultReportBuilder builder)
            : base(filePath)
        {
            this.builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var leagues = (IList<SportLeague<BaseBallMatch>>)this.builder.Data;
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
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.events);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue("1");
            resultSheet.GetCell(rowId, colId + 3).SetCellValue("2");
            resultSheet.GetCell(rowId, colId + 4).SetCellValue("3");
            resultSheet.GetCell(rowId, colId + 5).SetCellValue("4");
            resultSheet.GetCell(rowId, colId + 6).SetCellValue("5");
            resultSheet.GetCell(rowId, colId + 7).SetCellValue("6");
            resultSheet.GetCell(rowId, colId + 8).SetCellValue("7");
            resultSheet.GetCell(rowId, colId + 9).SetCellValue("8");
            resultSheet.GetCell(rowId, colId + 10).SetCellValue("9");
            resultSheet.GetCell(rowId, colId + 11).SetCellValue("E.I");
            resultSheet.GetCell(rowId, colId + 12).SetCellValue(Resources.Lang.lblHTScore);
            resultSheet.GetCell(rowId, colId + 13).SetCellValue(Resources.Lang.finalscore);
            resultSheet.GetCell(rowId, colId + 14).SetCellValue(Resources.Lang.status);

            rowId++;
            foreach (var league in leagues)
            {
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

                    resultSheet.GetCell(rowId, colId).SetCellValue(match.KickOffTime);

                    IRichTextString homeRtf;
                    IRichTextString awayRtf;
                    Common.GetFormatText(match, this.hssfWorkbook, out homeRtf, out awayRtf);
                    resultSheet.GetCell(rowId, colId + 1).SetCellValue(homeRtf);
                    resultSheet.GetCell(rowId + 1, colId + 1).SetCellValue(awayRtf);

                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(match.Home1);
                    resultSheet.GetCell(rowId + 1, colId + 2).SetCellValue(match.Away1);

                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(match.Home2);
                    resultSheet.GetCell(rowId + 1, colId + 3).SetCellValue(match.Away2);

                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(match.Home3);
                    resultSheet.GetCell(rowId + 1, colId + 4).SetCellValue(match.Away3);

                    resultSheet.GetCell(rowId, colId + 5).SetCellValue(match.Home4);
                    resultSheet.GetCell(rowId + 1, colId + 5).SetCellValue(match.Away4);

                    resultSheet.GetCell(rowId, colId + 6).SetCellValue(match.Home5);
                    resultSheet.GetCell(rowId + 1, colId + 6).SetCellValue(match.Away5);

                    resultSheet.GetCell(rowId, colId + 7).SetCellValue(match.Home6);
                    resultSheet.GetCell(rowId + 1, colId + 7).SetCellValue(match.Away6);

                    resultSheet.GetCell(rowId, colId + 8).SetCellValue(match.Home7);
                    resultSheet.GetCell(rowId + 1, colId + 8).SetCellValue(match.Away7);

                    resultSheet.GetCell(rowId, colId + 9).SetCellValue(match.Home8);
                    resultSheet.GetCell(rowId + 1, colId + 9).SetCellValue(match.Away8);

                    resultSheet.GetCell(rowId, colId + 10).SetCellValue(match.Home9);
                    resultSheet.GetCell(rowId + 1, colId + 10).SetCellValue(match.Away9);

                    resultSheet.GetCell(rowId, colId + 11).SetCellValue(match.EIHome > 0 ? match.EIHome.ToString() : "-");
                    resultSheet.GetCell(rowId + 1, colId + 11).SetCellValue(match.EIAway > 0 ? match.EIAway.ToString() : "-");

                    resultSheet.GetCell(rowId, colId + 12).SetCellValue(match.HTHomeScore);
                    resultSheet.GetCell(rowId + 1, colId + 12).SetCellValue(match.HTAwayScore);

                    resultSheet.GetCell(rowId, colId + 13).SetCellValue(match.FTHomeScore);
                    resultSheet.GetCell(rowId + 1, colId + 13).SetCellValue(match.FTAwayScore);

                    resultSheet.GetCell(rowId, colId + 14).SetCellValue(Common.MapEventStatusString(match));

                    rowId += 2;
                }
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}