

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Result
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NPOI.HSSF.UserModel;
    using Fanex.Age.DTO.Reports.Result;
    using NPOI.SS.UserModel;
    using Fanex.Age.Utility.NPOI;
    public class ResultHockeyReport : BaseResultReport, IReportBuilder
    {
        private ResultReportBuilder builder;

        public ResultHockeyReport(string filePath, ResultReportBuilder builder)
            : base(filePath)
        {
            this.builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var leagues = (IList<SportLeague<HockeyMatch>>)this.builder.Data;
            if (leagues == null)
            {
                return null;
            }

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("Result");
            int rowId = 0;
            int colId = 0;

            hssfWorkbook.SetSheetName(0, Resources.Lang.result);
            // title
            this.WriteTitle(resultSheet, ref rowId, ref colId, this.builder);

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.kickofftime);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.match);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.P1);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.P2);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.P3);
            resultSheet.GetCell(rowId, colId + 5).SetCellValue(Resources.Lang.OT);
            resultSheet.GetCell(rowId, colId + 6).SetCellValue(Resources.Lang.finalscore);
            resultSheet.GetCell(rowId, colId + 7).SetCellValue(Resources.Lang.status);

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

                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(match.P1Home);
                    resultSheet.GetCell(rowId + 1, colId + 2).SetCellValue(match.P1Away);

                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(match.P2Home);
                    resultSheet.GetCell(rowId + 1, colId + 3).SetCellValue(match.P2Away);

                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(match.P3Home);
                    resultSheet.GetCell(rowId + 1, colId + 4).SetCellValue(match.P3Away);

                    resultSheet.GetCell(rowId, colId + 5).SetCellValue(match.OTHome > 0 ? match.OTHome.ToString() : "-");
                    resultSheet.GetCell(rowId + 1, colId + 5).SetCellValue(match.OTAway > 0 ? match.OTAway.ToString() : "-");

                    resultSheet.GetCell(rowId, colId + 6).SetCellValue(match.FTHomeScore != null ? match.FTHomeScore.ToString() : "-");
                    resultSheet.GetCell(rowId + 1, colId + 6).SetCellValue(match.FTAwayScore != null ? match.FTAwayScore.ToString() : "-");

                    resultSheet.GetCell(rowId, colId + 7).SetCellValue(Common.MapEventStatusString(match));

                    rowId += 2;
                }
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}
