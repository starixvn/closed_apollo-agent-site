using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Fanex.Age.DTO.Reports.Result;
using Sunplus.Agent.Utilities;
using System.Web;
using System.Resources;
using NPOI.HSSF.Util;
using Fanex.Age.Utility.NPOI;
using Fanex.Age.UI;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Excel.Reports.Result
{
    public abstract class CellInfo
    {
        public abstract void Write(ICell cell);
    }

    public class StatusInfo : CellInfo
    {
        public IList<HorseRacingRace> SCR_Races;
        public IList<HorseRacingRace> TF_Races;
        private IFont _normalFont;
        private IFont _officialFont;

        public StatusInfo(IWorkbook hssfWorkbook)
        {
            _normalFont = hssfWorkbook.CreateFont();
            _officialFont = hssfWorkbook.CreateFont();
            _officialFont.Color = HSSFColor.RED.index;
        }

        public override void Write(ICell cell)
        {
            RtfFontFormatter fontFormatter = new RtfFontFormatter("Tahoma", 9);
            fontFormatter.Format(_normalFont);
            fontFormatter.Format(_officialFont);

            RtfTextRender render = new RtfTextRender(_normalFont);

            render.AddText(Resources.Lang.official, _officialFont);

            if (SCR_Races != null && SCR_Races.Count > 0)
            {
                string scr = "\n";
                scr += Resources.Lang.scr + ": ";
                foreach (var race in SCR_Races)
                {
                    scr += race.HorseNo;
                    if (SCR_Races.IndexOf(race) < SCR_Races.Count - 1) scr += ", ";
                }
                render.AddText(scr);
                //SportMatch2 race0 = Races[0];
                //if (race0.FOddsStatus.ToString().ToLower() == "completed")
                //{
                //    DateTime eventDate = race0.EventDate;
                //    int leagueId = race0.LeagueId;
                //    int racename = race0.RaceName;
                //    scr += string.Format("{0}, {1}, {2}", Formater.FormatDateEn(eventDate), racename, leagueId);
                //}
            }

            if (TF_Races != null && TF_Races.Count > 0)
            {
                string tf = "\n";
                tf += Resources.Lang.tf + ": ";
                foreach (var race in TF_Races)
                {
                    tf += race.HorseNo;
                    if (TF_Races.IndexOf(race) < TF_Races.Count - 1) tf += ", ";
                }
                render.AddText(tf);
            }

            cell.SetCellValue(render.Render());
        }
    }

    public class ResultHorseRacingReport : BaseResultReport, IReportBuilder
    {
        private ResultReportBuilder _builder;

        public ResultHorseRacingReport(string filePath, ResultReportBuilder builder)
            : base(filePath)
        {
            _builder = builder;
        }

        public HSSFWorkbook Build()
        {
            var leagues = (IList<SportLeague<HorseRacingMatch>>)_builder.Data;
            if (leagues == null) return null;

            ISheet templateSheet = hssfWorkbook.GetSheet("Template");
            ISheet resultSheet = hssfWorkbook.GetSheet("Result");
            int rowId = 0;
            int colId = 0;

            // title
            WriteTitle(resultSheet, ref rowId, ref colId, _builder);

            // header
            resultSheet.GetCell(rowId, colId).SetCellValue(Resources.Lang.lblRace);
            resultSheet.GetCell(rowId, colId + 3).SetCellValue(Resources.Lang.result);
            resultSheet.GetCell(rowId, colId + 4).SetCellValue(Resources.Lang.WinHR);
            resultSheet.GetCell(rowId, colId + 5).SetCellValue(Resources.Lang.Place);
            resultSheet.GetCell(rowId, colId + 6).SetCellValue(Resources.Lang.status);

            rowId++;
            foreach (var league in leagues)
            {
                if (league.Matches.Count == 0)
                    continue;

                // create a LeagueId cell
                resultSheet.CopyRow(templateSheet, 1, rowId);
                string leagueName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetLeagueName(league.LeagueId);
                resultSheet.GetCell(rowId, colId).SetCellValue(leagueName.ToUpper());
                rowId++;

                foreach (var match in league.Matches)
                {
                    // only show maximum 4 first orders
                    var races = match.Races.Where(p => p.Place >= 0 && p.Place <= 4).ToList();
                    if (races.Count == 0) continue;

                    int id = league.Matches.IndexOf(match);
                    if (id % 2 == 0)
                    {
                        for (var i = 0; i < races.Count; i++)
                        {
                            resultSheet.CopyRow(templateSheet, 3, rowId + i);
                        }
                        resultSheet.MergeCells(rowId, colId, rowId + races.Count - 1, colId);
                        resultSheet.MergeCells(rowId, colId + 6, rowId + races.Count - 1, colId + 6);
                    }
                    else
                    {
                        for (var i = 0; i < races.Count; i++)
                        {
                            resultSheet.CopyRow(templateSheet, 5, rowId + i);
                        }
                        resultSheet.MergeCells(rowId, colId, rowId + races.Count - 1, colId);
                        resultSheet.MergeCells(rowId, colId + 6, rowId + races.Count - 1, colId + 6);
                    }

                    // create a RaceNo cell
                    resultSheet.GetCell(rowId, colId).SetCellValue(match.RaceName);
                    foreach (SportMatch2 race in races)
                    {
                        // create a Place cell
                        resultSheet.GetCell(rowId, colId + 1).SetCellValue(Common.GetHorseRacingPlaceString(race.Place));
                        // create a HorseNo cell
                        resultSheet.GetCell(rowId, colId + 2).SetCellValue(race.HorseNo);
                        // create a HorseId cell
                        string horseTeam = Sunplus.Agent.Model.BL.Cache.CacheRef.GetHorseTeamName(race.HorseId);
                        resultSheet.GetCell(rowId, colId + 3).SetCellValue(horseTeam);

                        TplBase page = HttpContext.Current.Handler as TplBase;
                        if (page != null)
                        {
                            // create a Win cell
                            string win = Convert.ToDecimal(race.Odds) > 0 ? Formater.FormatNumber20(race.Odds, page.CurrentUserSession.LangKey) : "";
                            resultSheet.GetCell(rowId, colId + 4).SetCellValue(win);
                            // create a Place cell
                            string place = Convert.ToDecimal(race.PlaceOdds) > 0 ? Formater.FormatNumber20(race.PlaceOdds, page.CurrentUserSession.LangKey) : "";
                            resultSheet.GetCell(rowId, colId + 5).SetCellValue(place);
                        }
                        rowId++;
                    }

                    // create a Status cell
                    var scrList = match.Races.Where(p => p.IsSCR == true).ToList();
                    var tfList = match.Races.Where(p => p.IsFavor == true).ToList();
                    StatusInfo scrInfo = new StatusInfo(hssfWorkbook)
                    {
                        SCR_Races = scrList,
                        TF_Races = tfList
                    };
                    scrInfo.Write(resultSheet.GetCell(rowId - races.Count, colId + 6));
                }
            }

            //for (var i = 0; i < 6; i++)
            //{
            //    resultSheet.AutoSizeColumn(i);
            //}
            hssfWorkbook.RemoveSheetAt(hssfWorkbook.GetSheetIndex(templateSheet));
            return hssfWorkbook;
        }
    }
}
