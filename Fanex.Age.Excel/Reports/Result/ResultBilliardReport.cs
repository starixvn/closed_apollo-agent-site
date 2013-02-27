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

    public class ResultBilliardReport : BaseResultReport, IReportBuilder
    {
        private ResultReportBuilder builder;

        public ResultBilliardReport(string filePath, ResultReportBuilder builder)
            : base(filePath)
        {
            this.builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var leagues = (IList<SportLeague<BilliardMatch>>)this.builder.Data;
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
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.finalscore);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.status);

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
                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(match.FTHomeScore);
                    resultSheet.GetCell(rowId + 1, colId + 2).SetCellValue(match.FTAwayScore);

                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(Common.MapEventStatusString(match));

                    rowId += 2;
                }
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}