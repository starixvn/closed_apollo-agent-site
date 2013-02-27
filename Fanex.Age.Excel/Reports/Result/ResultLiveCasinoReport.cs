using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
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
    public class ResultLiveCasinoReport : BaseResultReport, IReportBuilder
    {
        private ResultReportBuilder _builder;

        public ResultLiveCasinoReport(string filePath, ResultReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var leagues = (IList<SportLeague<LiveCasinoMatch>>)_builder.Data;
            if (leagues == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("Result");
            int rowId = 0;
            int colId = 0;

            // title
            WriteTitle(resultSheet, ref rowId, ref colId, _builder);

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.kickofftime);
            resultSheet.GetCell(rowId, colId + 1).SetCellValue(Resources.Lang.gameTypeAndTableNumber);
            resultSheet.GetCell(rowId, colId + 2).SetCellValue(Resources.Lang.finalscore);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.result);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.status);

            rowId++;
            bool isInsertLeage = true;
            foreach (var league in leagues)
            {
                // create a LeagueId cell
                resultSheet.CopyRow(templateSheet, 1, rowId);
                string leagueName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetLeagueName(league.LeagueId);
                resultSheet.GetCell(rowId, colId).SetCellValue(leagueName.ToUpper());
                isInsertLeage = true;

                foreach (var match in league.Matches)
                {
                    if (isInsertLeage)
                    {
                        if (match.HomeId2 == (int)Sunplus.Agent.Model.BL.Definitions.BetType.Roulette)
                        {
                            resultSheet.GetCell(rowId, colId + 2).SetCellValue(string.Empty);
                        }
                        else
                        {
                            resultSheet.GetCell(rowId, colId + 2).SetCellValue("P - B");
                        }

                        rowId++;
                        isInsertLeage = false;
                    }

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

                    // create a Game Type - Table Number cell
                    resultSheet.GetCell(rowId, colId + 1).SetCellValue(match.ShowTime);

                    string finalScore = "";
                    string results = "";
                    string status = Common.MapEventStatusString(match);

                    if (match.EventStatus.ToLower() == "refund" || match.EventStatus.ToLower() == "void")
                    {
                        finalScore = results = status;
                    }
                    else
                    {
                        if (match.HomeId2 == (int)Sunplus.Agent.Model.BL.Definitions.BetType.Roulette)
                        {
                            finalScore = string.Format("{0}", match.FTHomeScore);
                            results = SetResultsRoulette(match.BetStatus);                            
                        }
                        else
                        {
                            finalScore = string.Format("{0} - {1}", match.FTHomeScore, match.FTAwayScore);
                            results = GetFormattedResults(match.BetStatus);
                        }
                    }

                    // create a Final Score cell
                    resultSheet.GetCell(rowId, colId + 2).SetCellValue(finalScore);                    

                    // create a Results cell
                    resultSheet.GetCell(rowId, colId + 3).SetCellValue(results);

                    // create a Status cell
                    resultSheet.GetCell(rowId, colId + 4).SetCellValue(status);

                    rowId++;
                }
            }

            //for (var i = 0; i < 10; i++)
            //{
            //    resultSheet.AutoSizeColumn(i);
            //}
            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }

        private string GetFormattedResults(string results)
        {
            if (results.Trim().Length < 1) return string.Empty;
            string[] arr = results.Split(',');
            string str = string.Empty, statusName = string.Empty;

            for (int i = 0; i < arr.Length; i++)
            {
                statusName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetCasinoBetItem(arr[i]);

                if (i < arr.Length - 1)
                {
                    if (i > 1)
                    {
                        str += string.Format("{0}, ", statusName);
                    }
                    else
                    {
                        str += string.Format("{0}; ", statusName);
                    }
                }
                else
                {
                    str += statusName;
                }
            }
            if (arr.Length < 3)
            {
                str += string.Format("; -", statusName);
            }
            return str;
        }

        private string SetResultsRoulette(string results)
        {
            if (results.Trim().Length < 1)
            {
                return string.Empty;
            }

            string[] arr = results.Split(',');
            string str = string.Empty, statusName = string.Empty;
            int resultNum = arr.Length;

            for (int i = 0; i < resultNum; i++)
            {
                statusName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetRouletteBetItem(arr[i]);

                if (i == resultNum - 1)
                {
                    str += string.Format("{0}", statusName);
                }
                else if (i == 2)
                {
                    str += string.Format("{0};\n", statusName);
                }
                else
                {
                    str += string.Format("{0}; ", statusName);
                }
            }

            return str;
        }
    }
}
