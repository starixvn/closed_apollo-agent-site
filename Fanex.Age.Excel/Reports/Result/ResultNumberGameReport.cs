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
    public class ResultNumberGameReport : BaseResultReport, IReportBuilder
    {
        private ResultReportBuilder _builder;

        public ResultNumberGameReport(string filePath, ResultReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var leagues = (IList<SportLeague<NumberGameMatch>>)_builder.Data;
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
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.result);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.warrior);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.lblOEFT);
            resultSheet.GetCell(rowId, colId + 5).SetCellValue(Resources.Lang.lbl1stBall);
            resultSheet.GetCell(rowId, colId + 6).SetCellValue(Resources.Lang.lastball);
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
                        resultSheet.CopyRow(templateSheet, 3, rowId);
                    }
                    else
                    {
                        resultSheet.CopyRow(templateSheet, 5, rowId);
                    }

                    // create a KickOffTime cell
                    resultSheet.GetCell(rowId, colId).SetCellValue(match.KickOffTime);
                    // create a Match cell
                    resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.numbergameno + " " + match.MatchCode);
                    // create a Results cell
                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(string.Join(" - ", match.BallList));

                    string firstball = "";
                    string lastball = "";
                    string oeft = "";
                    string warrior = "";
                    string status = Common.MapEventStatusString(match);
                    if (match.BallList.Count > 0)
                    {
                        firstball = match.FirstBall == null ? "" : match.FirstBall.ToString();

                        if (match.BallList.Count > 2)
                        {
                            warrior = match.BallList[1] > match.BallList[2] ? Resources.Lang.warrior2ndBall : Resources.Lang.warrior3rdBall;
                        }

                        lastball = match.LastBall == null ? Common.NA : match.LastBall.ToString();

                        if (status == Resources.Lang.running.ToLower())
                        {
                            lastball = "";
                        }
                        else
                        {
                            oeft = match.BallSum % 2 == 1 ? Resources.Lang.lblOdd : Resources.Lang.even;
                        }
                    }

                    // create a Warrior cell
                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(warrior);
                    // create a OEFT cell
                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(oeft);
                    // create a FirstBall cell
                    resultSheet.GetCell(rowId, colId + 5).SetCellValue(firstball);
                    // create a LastBall cell
                    resultSheet.GetCell(rowId, colId + 6).SetCellValue(lastball);
                    // create a Status cell
                    resultSheet.GetCell(rowId, colId + 7).SetCellValue(status);

                    rowId += 1;
                }
            }

            //for (var i = 0; i < 8; i++)
            //{
            //    resultSheet.AutoSizeColumn(i);
            //}
            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}
