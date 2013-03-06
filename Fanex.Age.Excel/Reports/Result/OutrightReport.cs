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

    public class OutrightReport : BaseResultReport, IReportBuilder
    {
        private OutrightReportBuilder builder;

        public OutrightReport(string filePath, OutrightReportBuilder builder)
            : base(filePath)
        {
            this.builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var leagues = (IList<SportLeague<OutrightMatch>>)this.builder.Data;
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
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.time);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.lblTeam);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.result);

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
                        resultSheet.CopyRow(templateSheet, 3, rowId);
                    }
                    else
                    {
                        resultSheet.CopyRow(templateSheet, 5, rowId);
                    }

                    resultSheet.GetCell(rowId, colId).SetCellValue(match.EventDate.ToString());
                    string teamName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(match.TeamId);
                    resultSheet.GetCell(rowId, colId + 1).SetCellValue(teamName);
                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(Common.MapEventStatusString(match));

                    rowId += 1;
                }
            }

            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return this.hssfWorkbook;
        }
    }
}