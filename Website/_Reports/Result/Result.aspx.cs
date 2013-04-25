// <copyright file="Result.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
//
// <summary>
//  Report Result
// </summary>
//
// <history>
//  <change who="Kaden" date="2012.01.17">Created</change>
// </history>
namespace Fanex.Age.UI.Reports.Result
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Resources;
    using System.Text;
    using System.Web.UI;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Reports.Repositories;

    using Apollo.Age.Reports.Services;
    using Apollo.Age.Services;
    using Apollo.Age.Web;
    using Fanex.Age.Utility;

    public partial class Result_Page : Reports_Page
    {
        private readonly ReportService _reportServce;
        private readonly ResultRepository _resultRepository;
        protected DateTime MIN_REPORT_DATE = DateTime.Today;
        protected DateTime MAX_REPORT_DATE = DateTime.Today;

        public Result_Page()
        {
            _reportServce = new ReportService();
            _resultRepository = new ResultRepository();
            MIN_REPORT_DATE = _reportServce.GetBalanceUpDay();
        }

        #region Variable

        private const string NA = "N/A";
        private const string REFUND_HT = "Refund HT";
        private const string REFUND_FT = "Refund FT";
        private const string REFUND = "Refund";
        private const int MAX_LIVE_BALL_COUNT = 30;

        private int _leagueId = 0;
        private int _sportId = 0;
        private bool isOutright = false;
        private int bettype = 0;
        private int _matchId = 0;
        private string raceNo = null;
        private long refNo = 0;
        private bool isChangeSportType = false;

        Result_Template tpl = new Result_Template();

        #endregion Variable

        #region Override

        /// <summary>
        ///
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            //if (_exportToExcel)
            //{
            //    DataTable reportData = (View as Dictionary<string, DataTable>)["ReportData"];
            //    MemoryStream excelStream = null;
            //    if (this.isOutright)
            //    {
            //        var builder = new Fanex.Age.Excel.Reports.Result.OutrightReportBuilder(this._sportId, this._leagueId, this.bettype, reportData, _dateFrom, _dateTo);
            //        excelStream = builder.Build();
            //    }
            //    else
            //    {
            //        var builder = new Fanex.Age.Excel.Reports.Result.ResultReportBuilder(this._sportId, this._leagueId, this.bettype, reportData, _dateFrom, _dateTo);
            //        excelStream = builder.Build();
            //    }

            //    if (excelStream != null)
            //    {
            //        ExportToExcel(excelStream, "Result.xls");
            //        excelStream.Dispose();
            //        return;
            //    }

            //    // Need to render report content only
            //    this.RenderReportContent(this.tpl.ExcelContents);
            //    this.ExportToExcel(Response, this.tpl.ExcelContents.ToString(), "Result");
            //}
            //else
            {
                // Render Report Header and history message
                this.RenderReportHeader(this.tpl);

                // view match result popup
                if (this._matchId > 0)
                {
                    this.tpl.ResultTitle.Visible = false;
                    this.tpl.SportCriteria.Visible = false;
                    this.tpl.HeaderRegion.Visible = false;
                }
                else
                {
                    this.RenderDateRange(this.tpl.HeaderRegion.DateRangeSelect);
                    this.RenderSportCriteria(this.tpl.SportCriteria);
                }

                // Render content
                //if (this.bettype == (int)Sunplus.Agent.Model.BL.Definitions.BetType.Mix_Parlay
                //    || this.bettype == (int)Sunplus.Agent.Model.BL.Definitions.BetType.Combine_Mix_Parlay)
                //{
                //    this.RenderReportContentMixParlay(this.tpl.ExcelContents);
                //}
                //else
                //{
                this.RenderReportContent(this.tpl.ExcelContents);
                //}
                this.RenderHiddenField(this.tpl);
                tpl.ApoRoot = Page.ResolveUrl("~/");

                writer.Write(this.tpl);
            }

            RegisterSortingJavascriptObject();
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            base.ProcessRequest(ref p);

            this._sportId = GetIntRequest("ddlSport", 0);
            this._leagueId = GetIntRequest("ddlLeague", 0);
            this.isOutright = GetBooleanRequest("is_outright", false);
            this.isChangeSportType = GetBooleanRequest("changeSportType", false);
            if (this._leagueId == 0)
            {
                _sortingEnabled = false;
            }

            this._matchId = GetIntRequest("matchid", 0);
            this.bettype = GetIntRequest("bettype", 0);
            this.raceNo = GetStringRequest("race", null);
            this.refNo = GetLongRequest("refnum", 0);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Dictionary<string, DataTable> viewData = new Dictionary<string, DataTable>();

            // Report data
            DataTable reportData = null;

            // View match result popup
            if (this._matchId > 0)
            {
                if (this.raceNo == "0")
                {
                    this.raceNo = null;
                }

                switch (this.bettype)
                {
                    //case 81:
                    //case 82:
                    //case 83:
                    //case 84:
                    //case 85:
                    //case 86:
                    //case 87:
                    //case 88:
                    //case 89:
                    //case 90:
                    //    reportData = _ReportBO.SelectMatchForViewNumberGameResult(this._matchId);
                    //    break;
                    case 9:
                    case 29:
                        // Mix Parlay & System Parlay
                        reportData = _resultRepository.SelectMatchResultParlay(this.refNo);
                        break;
                    default:
                        reportData = _resultRepository.SelectMatchForViewResult(this._matchId, this.raceNo);
                        break;
                }

                viewData.Add("ReportData", reportData);

                return viewData;
            }

            // View match result list
            if (this.isOutright)
            {
                // Report data
                reportData = _resultRepository.GetOutrightResult(_dateFrom, _dateTo);

                // Get default sport id
                if (this._sportId == 0 && reportData.Rows.Count > 0)
                {
                    this._sportId = Convert.ToInt32(reportData.Rows[0]["sporttype"]);
                }
            }
            else
            {
                DataTable sportList = _resultRepository.GetResultSportByDate(_dateFrom);

                // Delete number game if IsShowNumberGame=false
                //if (!UserPermission.IsShowNumberGame)
                //{
                //    DataRow[] numberGame = sportList.Select("sporttype=" + ((int)Sunplus.Agent.Model.BL.Definitions.SportType.NumberGame).ToString());
                //    foreach (DataRow dr in numberGame)
                //    {
                //        sportList.Rows.Remove(dr);
                //    }
                //}

                //if (!UserPermission.IsShowLiveCasino)
                //{
                //    DataRow[] liveCasino = sportList.Select("sporttype=" + ((int)Sunplus.Agent.Model.BL.Definitions.SportType.LiveCasino).ToString());
                //    foreach (DataRow dr in liveCasino)
                //    {
                //        sportList.Rows.Remove(dr);
                //    }
                //}

                viewData.Add("SportList", sportList);

                // Get default sport id
                if ((!this.isChangeSportType && sportList.Rows.Count > 0) || (this._sportId == 0 && sportList.Rows.Count > 0))
                {
                    this._sportId = Convert.ToInt32(sportList.Rows[0]["sporttype"]);
                    this._leagueId = 0;
                }

                ICurrentSession currentSession = (ICurrentSession)Session["Apollo.Age.Web.CurrentSession"];
                DataTable leagueList = _resultRepository.GetResultLeagueByDate(_dateFrom, this._sportId, currentSession.Language);
                viewData.Add("LeagueList", leagueList);

                // Get result data
                //switch (this._sportId)
                //{
                //case (int)Sunplus.Agent.Model.BL.Definitions.SportType.HorseRacing:
                //case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Greyhound:
                //case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Harness:
                //    reportData = _ReportBO.GetResultHRRaces(_dateFrom, this._sportId, this._leagueId);
                //    break;
                //case (int)Sunplus.Agent.Model.BL.Definitions.SportType.NumberGame:
                //    reportData = _ReportBO.GetResultNumberGameByLeague(_dateFrom, this._leagueId);
                //    if (this._leagueId > 0)
                //    {
                //        reportData = SortData(reportData);
                //    }

                //    break;
                //default:
                reportData = _resultRepository.GetResultMatchByLeague(_dateFrom, currentSession.UserId, this._sportId, this._leagueId, currentSession.Language);
                if (this._leagueId > 0)
                {
                    reportData = SortData(reportData);
                }

                //            break;
                //    }
            }

            // Check show LiveCasino on Webconfig
            //if (!UserPermission.IsShowLiveCasino && reportData.Columns.Contains("sporttype"))
            //{
            //    reportData = new DataView(reportData, "sporttype <> 162", string.Empty, DataViewRowState.CurrentRows).ToTable();
            //}

            viewData.Add("ReportData", reportData);
            return viewData;
        }

        #endregion Override

        #region Render Utility

        /// <summary>
        /// Render Report header
        /// </summary>
        /// <param name="headerTemplate"></param>
        private void RenderReportHeader(Result_Template template)
        {
            if (this.bettype == (int)BetTypes.Mix_Parlay)
            {
                this.tpl.ResultTitle.Title = this.tpl.Title = Resources.Lang.lblMixParlay;
            }
            else if (this.bettype == (int)BetTypes.Combine_Mix_Parlay)
            {
                this.tpl.ResultTitle.Title = this.tpl.Title = Resources.Lang.lblSystemParlay;
            }
            else
            {
                template.ResultTitle.Title = template.Title = this.isOutright ? Resources.Lang.lblOutrightResult : Resources.Lang.result;
            }

            template.HeaderRegion.ResultType = this.isOutright ? Resources.Lang.lblNormal : Resources.Lang.lblOutRight;
            template.HeaderRegion.DateRangeSelect.SearchByDateFunction = "Result_SearchByDate()";
            template.HeaderRegion.DateRangeSelect.SearchOneDateFunction = "Result_SearchOneDay(this.value)";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dateRangeSelect"></param>
        protected override void RenderDateRange(Fanex.Age.UI.Components.DateRangeSelect_Control dateRangeSelect)
        {
            if (this.isOutright)
            {
                dateRangeSelect.Initialize(true, _dateFrom, _dateTo);
            }
            else
            {
                dateRangeSelect.Initialize(false, _dateFrom, _dateTo);
            }

            dateRangeSelect.SetDateRange(_reportServce.GetBalanceUpDay(), DateTime.Today);
        }

        /// <summary>
        /// Render report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(Result_ExcelContents_Block excelBlock)
        {
            if (this.isOutright)
            {
                this.RenderOutright(excelBlock);
            }
            else
            {
                DataTable reportData = (View as Dictionary<string, DataTable>)["ReportData"];

                // view match result popup
                if (this._matchId > 0)
                {
                    string title = string.Empty;
                    title += string.Join(null, new string[] { "<strong><span class=\"bl_stype\">", CacheRef.GetSportName(this._sportId), "</span></strong>", " - ", CacheRef.GetLeagueName(this._leagueId) });
                    this.tpl.ResultTitle.Title = this.tpl.Title = title;
                }

                switch (this._sportId)
                {
                    case (int)SportTypes.Soccer:
                        this.RenderSoccer(reportData, excelBlock);
                        break;
                    case (int)SportTypes.BasketBall:
                    case (int)SportTypes.Netball:
                    case (int)SportTypes.FootBall:
                    case (int)SportTypes.Rugby:
                        this.RenderBasketBall(reportData, excelBlock);
                        break;
                    case (int)SportTypes.IceHockey:
                    case (int)SportTypes.FieldHockey:
                        this.RenderHockey(reportData, excelBlock);
                        break;
                    case (int)SportTypes.Tennis:
                        this.RenderTennis(reportData, excelBlock);
                        break;
                    case (int)SportTypes.VolleyBall:
                    case (int)SportTypes.Badminton:
                    case (int)SportTypes.Squash:
                        this.RenderBadminton(reportData, excelBlock);
                        break;
                    case (int)SportTypes.BaseBall:
                        this.RenderBaseBall(reportData, excelBlock);
                        break;
                    case (int)SportTypes.Golf:
                    case (int)SportTypes.Motorsports:
                    case (int)SportTypes.Billard:
                    case (int)SportTypes.Cycling:
                    case (int)SportTypes.Fencing:
                    case (int)SportTypes.Judo:
                    case (int)SportTypes.ModernPentathlon:
                    case (int)SportTypes.Rowing:
                    case (int)SportTypes.Sailing:
                    case (int)SportTypes.Shooting:
                    case (int)SportTypes.Taekwondo:
                    case (int)SportTypes.Triathlon:
                    case (int)SportTypes.Wrestling:
                    case (int)SportTypes.Winter: this.RenderBilliard(reportData, excelBlock);
                        break;
                    //case (int)SportType.HorseRacing:
                    //case (int)SportType.Greyhound:
                    //case (int)SportType.Harness: this.RenderHorseRacing(reportData, excelBlock);
                    //    break;
                    //case (int)SportType.NumberGame: this.RenderNumberGame(reportData, excelBlock);
                    //    break;
                    //case (int)SportType.LiveCasino: this.RenderLiveCasino(reportData, excelBlock);
                    //    break;
                    default: this.RenderOtherSport(reportData, excelBlock);
                        break;
                }
            }
        }

        /// <summary>
        /// Render report content for Mix Parlay, System Parlay
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContentMixParlay(Result_ExcelContents_Block excelBlock)
        {
            DataTable tblMatches = (View as Dictionary<string, DataTable>)["ReportData"];

            List<int> arrLeagueId = new List<int>();
            foreach (DataRow match in tblMatches.Rows)
            {
                int leagueId = Convert.ToInt32(match["LeagueID"]);
                if (!arrLeagueId.Contains(leagueId))
                {
                    arrLeagueId.Add(leagueId);
                }
            }

            excelBlock.Append("<table width=\"100%\">");

            foreach (int Id in arrLeagueId)
            {
                DataRow[] matches = tblMatches.Select(string.Format("LeagueID={0}", Id));
                DataTable reportData = matches.CopyToDataTable();
                this._sportId = Convert.ToInt32(matches[0]["sporttype"]);

                Result.SportType_Template sportTypeBlock = new SportType_Template();
                sportTypeBlock.lblSportName = CacheRef.GetSportName(this._sportId);
                sportTypeBlock.lblLeagueName = CacheRef.GetLeagueName(Id);
                excelBlock.Append(sportTypeBlock);

                excelBlock.Append("<tr><td>");

                switch (this._sportId)
                {
                    case (int)SportTypes.Soccer: this.RenderSoccer(reportData, excelBlock);
                        break;
                    case (int)SportTypes.BasketBall:
                    case (int)SportTypes.Netball:
                    case (int)SportTypes.FootBall:
                    case (int)SportTypes.Rugby: this.RenderBasketBall(reportData, excelBlock);
                        break;
                    case (int)SportTypes.IceHockey:
                    case (int)SportTypes.FieldHockey: this.RenderHockey(reportData, excelBlock);
                        break;
                    case (int)SportTypes.Tennis: this.RenderTennis(reportData, excelBlock);
                        break;
                    case (int)SportTypes.VolleyBall:
                    case (int)SportTypes.Badminton:
                    case (int)SportTypes.Squash: this.RenderBadminton(reportData, excelBlock);
                        break;
                    case (int)SportTypes.BaseBall: this.RenderBaseBall(reportData, excelBlock);
                        break;
                    case (int)SportTypes.Golf:
                    case (int)SportTypes.Motorsports:
                    case (int)SportTypes.Billard:
                    case (int)SportTypes.Cycling:
                    case (int)SportTypes.Fencing:
                    case (int)SportTypes.Judo:
                    case (int)SportTypes.ModernPentathlon:
                    case (int)SportTypes.Rowing:
                    case (int)SportTypes.Sailing:
                    case (int)SportTypes.Shooting:
                    case (int)SportTypes.Taekwondo:
                    case (int)SportTypes.Triathlon:
                    case (int)SportTypes.Wrestling:
                    case (int)SportTypes.Winter: this.RenderBilliard(reportData, excelBlock);
                        break;
                    case (int)SportTypes.HorseRacing:
                    case (int)SportTypes.Greyhound:
                    case (int)SportTypes.Harness: this.RenderHorseRacing(reportData, excelBlock);
                        break;
                    case (int)SportTypes.NumberGame: this.RenderNumberGame(reportData, excelBlock);
                        break;
                    default: this.RenderOtherSport(reportData, excelBlock);
                        break;
                }

                excelBlock.Append("</td></tr>");
            }

            excelBlock.Append("</table>");
        }

        /// <summary>
        /// Render noinfo content
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderNoInfo(Result_ExcelContents_Block excelBlock)
        {
            // Horse racing
            if (this._sportId == 151)
            {
                Result.ResultNoInfoHR_Template template = new Fanex.Age.UI.Reports.Result.ResultNoInfoHR_Template();
                excelBlock.Append(template);
            }
            else
            {
                Result.ResultNoInfo_Template template = new Fanex.Age.UI.Reports.Result.ResultNoInfo_Template();
                excelBlock.Append(template);
            }
        }

        /// <summary>
        /// Render sport and league dropdown list
        /// </summary>
        /// <param name="criteriaBlock"></param>
        private void RenderSportCriteria(Result_SportCriteria_Block criteriaBlock)
        {
            if (this._sportId == 0)
            {
                criteriaBlock.Hide();
            }
            else
            {
                if (this.isOutright)
                {
                    this.RenderOutrightSportCriteria(criteriaBlock);
                }
                else
                {
                    this.RenderNormalSportCriteria(criteriaBlock);
                }
            }
        }

        /// <summary>
        /// Render sport and league dropdown list for outright result
        /// </summary>
        /// <param name="criteriaBlock"></param>
        private void RenderOutrightSportCriteria(Result_SportCriteria_Block criteriaBlock)
        {
            DataTable reportData = (View as Dictionary<string, DataTable>)["ReportData"];
            if (reportData.Rows.Count == 0)
            {
                criteriaBlock.Hide();
            }
            else
            {
                Result_SportCriteria_SportOption_Block sportOption = new Result_SportCriteria_SportOption_Block();
                Result_SportCriteria_LeagueOption_Block leagueOption = new Result_SportCriteria_LeagueOption_Block();
                List<int> sportIdList = new List<int>();
                List<int> leagueIdList = new List<int>();
                criteriaBlock.SportOption.Clear();
                criteriaBlock.LeagueOption.Clear();
                foreach (DataRow dr in reportData.Rows)
                {
                    // Sport dropdownlist
                    int sportId = Convert.ToInt32(dr["sporttype"]);

                    //if (sportId == 180 && !UserPermission.IsShowVirtualSports)
                    //{
                    //    continue;
                    //}

                    if (!sportIdList.Contains(sportId))
                    {
                        sportOption.SportId = sportId.ToString();
                        sportOption.SportName = CacheRef.GetSportName(sportId);
                        sportOption.SelectedOption = this._sportId == sportId ? "selected" : string.Empty;
                        criteriaBlock.SportOption.Append(sportOption);
                        sportIdList.Add(sportId);
                    }
                }

                DataRow[] leagueRows = reportData.Select("sporttype=" + this._sportId.ToString());
                foreach (DataRow dr in leagueRows)
                {
                    // LeagueModel dropdownlist
                    int leagueId = Convert.ToInt32(dr["leagueid"]);

                    if (!leagueIdList.Contains(leagueId))
                    {
                        leagueOption.LeagueId = leagueId.ToString();
                        leagueOption.LeagueName = CacheRef.GetLeagueName(leagueId);
                        leagueOption.SelectedOption = this._leagueId == leagueId ? "selected" : string.Empty;
                        criteriaBlock.LeagueOption.Append(leagueOption);
                        leagueIdList.Add(leagueId);
                    }
                }
            }
        }

        /// <summary>
        /// Render sport and league dropdown list for normal result
        /// </summary>
        /// <param name="criteriaBlock"></param>
        private void RenderNormalSportCriteria(Result_SportCriteria_Block criteriaBlock)
        {
            DataTable sportList = (View as Dictionary<string, DataTable>)["SportList"];
            DataTable leagueList = (View as Dictionary<string, DataTable>)["LeagueList"];
            if (sportList.Rows.Count == 0)
            {
                criteriaBlock.Hide();
            }
            else
            {
                Result_SportCriteria_SportOption_Block sportOption = new Result_SportCriteria_SportOption_Block();
                Result_SportCriteria_LeagueOption_Block leagueOption = new Result_SportCriteria_LeagueOption_Block();
                criteriaBlock.SportOption.Clear();
                criteriaBlock.LeagueOption.Clear();
                foreach (DataRow dr in sportList.Rows)
                {
                    // Sport dropdownlist
                    int sportId = Convert.ToInt32(dr["sporttype"]);

                    //if (sportId == 180 && !UserPermission.IsShowVirtualSports)
                    //{
                    //    continue;
                    //}

                    sportOption.SportId = sportId.ToString();
                    sportOption.SportName = CacheRef.GetSportName(sportId);
                    sportOption.SelectedOption = this._sportId == sportId ? "selected" : string.Empty;
                    criteriaBlock.SportOption.Append(sportOption);
                }

                foreach (DataRow dr in leagueList.Rows)
                {
                    // LeagueModel dropdownlist
                    int leagueId = Convert.ToInt32(dr["leagueid"]);
                    leagueOption.LeagueId = leagueId.ToString();
                    leagueOption.LeagueName = CacheRef.GetLeagueName(leagueId);
                    leagueOption.SelectedOption = this._leagueId == leagueId ? "selected" : string.Empty;
                    criteriaBlock.LeagueOption.Append(leagueOption);
                }
            }
        }

        /// <summary>
        /// Render Hidden Field
        /// </summary>
        /// <param name="template"></param>
        private void RenderHiddenField(Result_Template template)
        {
            template.IsOutright = this.isOutright ? "1" : "0";
        }

        #endregion Render Utility

        #region RenderBySportType

        #region Outright

        /// <summary>
        /// Render outright result
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderOutright(Result_ExcelContents_Block excelBlock)
        {
            // Get report rows
            Result.ResultOutright_Template template = new Fanex.Age.UI.Reports.Result.ResultOutright_Template();
            DataTable reportData = (View as Dictionary<string, DataTable>)["ReportData"];
            string filterString = (this._leagueId > 0) ? "leagueid=" + this._leagueId.ToString() : "sporttype=" + this._sportId.ToString();
            DataRow[] reportRows = reportData.Select(filterString);

            int numRow = reportRows.Length;
            template.RowData.Clear();
            if (numRow > 0)
            {
                int currentLeagueId = 0;
                Result.ResultOutright_RowData_League_Block leagueBlock = new Fanex.Age.UI.Reports.Result.ResultOutright_RowData_League_Block();
                Result.ResultOutright_RowData_Row_Block rowBlock = new Fanex.Age.UI.Reports.Result.ResultOutright_RowData_Row_Block();
                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportRows[i];
                    int leagueid = Convert.ToInt32(dr["leagueid"]);
                    if (currentLeagueId == 0 || currentLeagueId != leagueid)
                    {
                        currentLeagueId = leagueid;
                        leagueBlock.LeagueName = CacheRef.GetLeagueName(leagueid);
                        template.RowData.Append(leagueBlock);
                    }

                    rowBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    rowBlock.TimeValue = dr["eventdate"].ToString();
                    rowBlock.TeamValue = CacheRef.GetTeamName(dr["teamid"]);
                    rowBlock.ResultValue = this.MapEventStatusString(dr["Status"].ToString(), dr.Table.Columns.Contains("IsPending") ? Convert.ToBoolean(dr["IsPending"]) : false, dr.Table.Columns.Contains("IsHTCal") ? Convert.ToBoolean(dr["IsHTCal"]) : false, dr.Table.Columns.Contains("calculate") ? Convert.ToBoolean(dr["calculate"]) : false);
                    template.RowData.Append(rowBlock);
                }

                template.LimitNotFound.Hide();
            }
            else
            {
                template.LimitNotFound.NoInfoRow.NumCols = "3";
            }

            excelBlock.Append(template);
        }

        #endregion Outright

        #region Badminton

        /// <summary>
        /// Render badminton result
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="excelBlock"></param>
        private void RenderBadminton(DataTable reportData, Result_ExcelContents_Block excelBlock)
        {
            // Get report rows
            Result.ResultBadminton_Template template = new Fanex.Age.UI.Reports.Result.ResultBadminton_Template();

            int numRow = reportData.Rows.Count;
            template.RowData.Clear();
            if (numRow > 0)
            {
                int currentLeagueId = 0;
                Result.ResultBadminton_RowData_League_Block leagueBlock = new Fanex.Age.UI.Reports.Result.ResultBadminton_RowData_League_Block();
                Result.ResultBadminton_RowData_Row_Block rowBlock = new Fanex.Age.UI.Reports.Result.ResultBadminton_RowData_Row_Block();

                // Do NOT show league block when Pop up the result of matches
                if (this._matchId > 0)
                {
                    leagueBlock.Visible = false;
                }

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];
                    int leagueid = Convert.ToInt32(dr["leagueid"]);
                    if (currentLeagueId == 0 || currentLeagueId != leagueid)
                    {
                        currentLeagueId = leagueid;
                        leagueBlock.LeagueName = CacheRef.GetLeagueName(leagueid);
                        template.RowData.Append(leagueBlock);
                    }

                    rowBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    rowBlock.KickOffTime = Formater.FullDateFormat2Line(dr["kickofftime"]);
                    rowBlock.EventStatus = this.MapEventStatusString(dr["eventstatus"].ToString(), dr.Table.Columns.Contains("IsPending") ? Convert.ToBoolean(dr["IsPending"]) : false, dr.Table.Columns.Contains("IsHTCal") ? Convert.ToBoolean(dr["IsHTCal"]) : false, dr.Table.Columns.Contains("calculate") ? Convert.ToBoolean(dr["calculate"]) : false);

                    // Home and away team
                    string homeTeamText = string.Empty, awayTeamText = string.Empty;
                    this.BoldTeamByFinalScore(dr, ref homeTeamText, ref awayTeamText);
                    rowBlock.Home = homeTeamText;
                    rowBlock.Away = awayTeamText;

                    // Bet status
                    string[] homeSet = new string[5] { "-", "-", "-", "-", "-" };
                    string[] awaySet = new string[5] { "-", "-", "-", "-", "-" };
                    int totalGameHome = 0, totalGameAway = 0;
                    string betStatus = Convert.IsDBNull(dr["betstatus"]) ? string.Empty : dr["betstatus"].ToString();
                    if (betStatus.IndexOf(",") >= 0)
                    {
                        string[] betStatusList = betStatus.Split(',');
                        for (int j = 0; j < betStatusList.Length && j < 5; j++)
                        {
                            homeSet[j] = betStatusList[j].Trim().Split('-')[0].Trim();
                            awaySet[j] = betStatusList[j].Trim().Split('-')[1].Trim();
                            totalGameHome += Convert.ToInt32(homeSet[j]);
                            totalGameAway += Convert.ToInt32(awaySet[j]);
                        }
                    }

                    rowBlock.S1Away = awaySet[0];
                    rowBlock.S2Away = awaySet[1];
                    rowBlock.S3Away = awaySet[2];
                    rowBlock.S4Away = awaySet[3];
                    rowBlock.S5Away = awaySet[4];

                    rowBlock.S1Home = homeSet[0];
                    rowBlock.S2Home = homeSet[1];
                    rowBlock.S3Home = homeSet[2];
                    rowBlock.S4Home = homeSet[3];
                    rowBlock.S5Home = homeSet[4];

                    rowBlock.TotalPointHome = totalGameHome > 0 ? totalGameHome.ToString() : string.Empty;
                    rowBlock.TotalPointAway = totalGameAway > 0 ? totalGameAway.ToString() : string.Empty;
                    rowBlock.TotalSetHome = Convert.IsDBNull(dr["finalhomescore"]) ? string.Empty : dr["finalhomescore"].ToString();
                    rowBlock.TotalSetAway = Convert.IsDBNull(dr["finalawayscore"]) ? string.Empty : dr["finalawayscore"].ToString();

                    template.RowData.Append(rowBlock);
                }

                template.LimitNotFound.Hide();
            }
            else
            {
                template.LimitNotFound.NoInfoRow.NumCols = "10";
            }

            excelBlock.Append(template);
        }

        #endregion Badminton

        #region SoccerResult

        /// <summary>
        /// Render soccer result
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="excelBlock"></param>
        private void RenderSoccer(DataTable reportData, Result_ExcelContents_Block excelBlock)
        {
            // Get report rows
            Result.ResultSoccer_Template template = new Fanex.Age.UI.Reports.Result.ResultSoccer_Template();
            int numRow = reportData.Rows.Count;
            template.RowData.Clear();
            if (numRow > 0)
            {
                int currentLeagueId = 0;
                Result.ResultSoccer_RowData_League_Block leagueBlock = new Fanex.Age.UI.Reports.Result.ResultSoccer_RowData_League_Block();
                Result.ResultSoccer_RowData_Row_Block rowBlock = this.CreateSoccerRowBlock();

                // Do NOT show league block when Pop up the result of matches
                if (this._matchId > 0)
                {
                    leagueBlock.Visible = false;
                }

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];
                    int leagueid = Convert.ToInt32(dr["leagueid"]);
                    if (currentLeagueId == 0 || currentLeagueId != leagueid)
                    {
                        currentLeagueId = leagueid;
                        leagueBlock.LeagueName = CacheRef.GetLeagueName(leagueid);
                        template.RowData.Append(leagueBlock);
                    }

                    rowBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    rowBlock.KickOffTime = Formater.FullDateFormat2Line(dr["kickofftime"]);
                    rowBlock.EventStatus = this.MapEventStatusString(dr["eventstatus"].ToString()
                        , dr.Table.Columns.Contains("IsPending")
                        ? Convert.ToBoolean(dr["IsPending"]) : false, dr.Table.Columns.Contains("IsHTCal")
                        ? Convert.ToBoolean(dr["IsHTCal"]) : false, dr.Table.Columns.Contains("calculate") ? Convert.ToBoolean(dr["calculate"]) : false);

                    // Home and away team
                    string homeTeamText = string.Empty, awayTeamText = string.Empty;
                    this.BoldTeamByFinalScore(dr, ref homeTeamText, ref awayTeamText);
                    rowBlock.Home = homeTeamText;
                    rowBlock.Away = awayTeamText;

                    // Result score by event status
                    this.RenderSoccerResultScore(dr, rowBlock);

                    template.RowData.Append(rowBlock);
                }

                template.LimitNotFound.Hide();
            }
            else
            {
                template.LimitNotFound.NoInfoRow.NumCols = "5";
            }

            excelBlock.Append(template);
        }

        /// <summary>
        /// Create new soccer row block
        /// if export excel, use (F) and (L) instead image first.gif and last.gif
        /// </summary>
        /// <returns></returns>
        private ResultSoccer_RowData_Row_Block CreateSoccerRowBlock()
        {
            ResultSoccer_RowData_Row_Block rowBlock = new ResultSoccer_RowData_Row_Block();
            if (_exportToExcel)
            {
                rowBlock.LimitFGHome.SetValue("<b>(F)</b>");
                rowBlock.LimitFGAway.SetValue("<b>(F)</b>");
                rowBlock.LimitLGHome.SetValue("<b>(L)</b>");
                rowBlock.LimitLGAway.SetValue("<b>(L)</b>");
            }

            return rowBlock;
        }

        /// <summary>
        /// Show Soccer FGLGLimit
        /// </summary>
        /// <param name="FGLG"></param>
        /// <param name="rowBlock"></param>
        private void ShowSoccerFGLGLimit(string FGLG, Result.ResultSoccer_RowData_Row_Block rowBlock)
        {
            if (_exportToExcel)
            {
                this.ShowSoccerFGLGLimitForExcel(FGLG, rowBlock);
            }
            else
            {
                this.ShowSoccerFGLGLimitForHtml(FGLG, rowBlock);
            }
        }

        /// <summary>
        /// Show Soccer FGLGLimit
        /// </summary>
        /// <param name="FGLG"></param>
        /// <param name="rowBlock"></param>
        private void ShowSoccerFGLGLimitForHtml(string FGLG, Result.ResultSoccer_RowData_Row_Block rowBlock)
        {
            // Event status
            this.HideFGLGElement(rowBlock);

            string[] FGLGArray = FGLG.Split(':');
            if (FGLG.Length > 1 && string.Compare(FGLGArray[0], "FL", true) == 0)
            {
                string firstGoal = FGLGArray[1].Split('-')[0];
                string lastGoal = FGLGArray[1].Split('-')[1];
                if (firstGoal == "1")
                {
                    rowBlock.LimitFGHome.Show();
                }

                if (firstGoal == "2")
                {
                    rowBlock.LimitFGAway.Show();
                }

                if (lastGoal == "1")
                {
                    rowBlock.LimitLGHome.Show();
                }

                if (lastGoal == "2")
                {
                    rowBlock.LimitLGAway.Show();
                }
            }
        }

        /// <summary>
        /// Show Soccer FGLGLimit
        /// </summary>
        /// <param name="FGLG"></param>
        /// <param name="rowBlock"></param>
        private void ShowSoccerFGLGLimitForExcel(string FGLG, Result.ResultSoccer_RowData_Row_Block rowBlock)
        {
            // Event status
            rowBlock.LimitFGHome.SetValue(string.Empty);
            rowBlock.LimitFGAway.SetValue(string.Empty);
            rowBlock.LimitLGHome.SetValue(string.Empty);
            rowBlock.LimitLGAway.SetValue(string.Empty);

            string[] FGLGArray = FGLG.Split(':');
            if (FGLG.Length > 1 && string.Compare(FGLGArray[0], "FL", true) == 0)
            {
                string firstGoal = FGLGArray[1].Split('-')[0];
                string lastGoal = FGLGArray[1].Split('-')[1];
                if (firstGoal == "1")
                {
                    rowBlock.LimitFGHome.SetValue("<b>(F)</b>");
                }

                if (firstGoal == "2")
                {
                    rowBlock.LimitFGAway.SetValue("<b>(F)</b>");
                }

                if (lastGoal == "1")
                {
                    rowBlock.LimitLGHome.SetValue("<b>(L)</b>");
                }

                if (lastGoal == "2")
                {
                    rowBlock.LimitLGAway.SetValue("<b>(L)</b>");
                }
            }
        }

        /// <summary>
        /// Hide FGLG content
        /// </summary>
        /// <param name="rowBlock"></param>
        private void HideFGLGElement(Result.ResultSoccer_RowData_Row_Block rowBlock)
        {
            rowBlock.LimitFGHome.Hide();
            rowBlock.LimitFGAway.Hide();
            rowBlock.LimitLGHome.Hide();
            rowBlock.LimitLGAway.Hide();
        }

        /// <summary>
        /// Render Soccer result content
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="rowBlock"></param>
        private void RenderSoccerResultScore(DataRow dr, Result.ResultSoccer_RowData_Row_Block rowBlock)
        {
            string eventStatus = dr["eventstatus"].ToString();
            if (eventStatus == REFUND)
            {
                rowBlock.HTHomeScore = NA;
                rowBlock.HTAwayScore = NA;
                rowBlock.FTAwayScore = NA;
                rowBlock.FTHomeScore = NA;
                this.HideFGLGElement(rowBlock);
            }
            else if (eventStatus == REFUND_FT)
            {
                string betStatus = dr["betstatus"].ToString().Trim();
                string[] refundArray = betStatus.Split(';');
                if (refundArray.Length == 2 && refundArray[1].ToString() != string.Empty)
                {
                    this.ShowSoccerFGLGLimit(refundArray[1], rowBlock);
                }
                else
                {
                    this.HideFGLGElement(rowBlock);
                }

                rowBlock.HTHomeScore = dr["hthomescore"].ToString();
                rowBlock.HTAwayScore = dr["htawayscore"].ToString();
                rowBlock.FTAwayScore = NA;
                rowBlock.FTHomeScore = NA;
            }
            else if (eventStatus == REFUND_HT)
            {
                rowBlock.HTHomeScore = NA;
                rowBlock.HTAwayScore = NA;
                rowBlock.FTHomeScore = dr["finalhomescore"].ToString();
                rowBlock.FTAwayScore = dr["finalawayscore"].ToString();
                this.HideFGLGElement(rowBlock);
            }
            else
            {
                string betStatus = dr["betstatus"].ToString().Trim();
                if (!string.IsNullOrEmpty(betStatus) && betStatus.Contains(":"))
                {
                    this.ShowSoccerFGLGLimit(betStatus, rowBlock);
                }
                else
                {
                    this.HideFGLGElement(rowBlock);
                }

                rowBlock.HTHomeScore = dr["hthomescore"].ToString();
                rowBlock.HTAwayScore = dr["htawayscore"].ToString();
                rowBlock.FTHomeScore = dr["finalhomescore"].ToString();
                rowBlock.FTAwayScore = dr["finalawayscore"].ToString();
            }
        }

        #endregion SoccerResult

        #region BasketBall

        /// <summary>
        /// Render BasketBall result
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="excelBlock"></param>
        private void RenderBasketBall(DataTable reportData, Result_ExcelContents_Block excelBlock)
        {
            // Get report rows
            Result.ResultBasketBall_Template template = new Fanex.Age.UI.Reports.Result.ResultBasketBall_Template();
            int numRow = reportData.Rows.Count;
            template.RowData.Clear();
            if (numRow > 0)
            {
                int currentLeagueId = 0;
                Result.ResultBasketBall_RowData_League_Block leagueBlock = new Fanex.Age.UI.Reports.Result.ResultBasketBall_RowData_League_Block();
                Result.ResultBasketBall_RowData_Row_Block rowBlock = new Fanex.Age.UI.Reports.Result.ResultBasketBall_RowData_Row_Block();

                // Do NOT show league block when Pop up the result of matches
                if (this._matchId > 0)
                {
                    leagueBlock.Visible = false;
                }

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];
                    int leagueid = Convert.ToInt32(dr["leagueid"]);
                    if (currentLeagueId == 0 || currentLeagueId != leagueid)
                    {
                        currentLeagueId = leagueid;
                        leagueBlock.LeagueName = CacheRef.GetLeagueName(leagueid);
                        template.RowData.Append(leagueBlock);
                    }

                    rowBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    rowBlock.KickOffTime = Formater.FullDateFormat2Line(dr["kickofftime"]);
                    rowBlock.EventStatus = this.MapEventStatusString(dr["eventstatus"].ToString(), dr.Table.Columns.Contains("IsPending") ? Convert.ToBoolean(dr["IsPending"]) : false, dr.Table.Columns.Contains("IsHTCal") ? Convert.ToBoolean(dr["IsHTCal"]) : false, dr.Table.Columns.Contains("calculate") ? Convert.ToBoolean(dr["calculate"]) : false);
                    rowBlock.HTScoreHome = dr["hthomescore"].ToString();
                    rowBlock.HTScoreAway = dr["htawayscore"].ToString();
                    rowBlock.FinalScoreHome = dr["finalhomescore"].ToString();
                    rowBlock.FinalScoreAway = dr["finalawayscore"].ToString();

                    // Home and away team
                    string homeTeamText = string.Empty, awayTeamText = string.Empty;
                    this.BoldTeamByFinalScore(dr, ref homeTeamText, ref awayTeamText);
                    rowBlock.Home = homeTeamText;
                    rowBlock.Away = awayTeamText;

                    // Bet status
                    string eventStatus = dr["eventstatus"].ToString().Trim();

                    int OTHome = 0, OTAway = 0;
                    string[] homeSet = new string[5] { "-", "-", "-", "-", "-" };
                    string[] awaySet = new string[5] { "-", "-", "-", "-", "-" };
                    if (eventStatus != "Refund FT")
                    {
                        string betStatus = Convert.IsDBNull(dr["betstatus"]) ? string.Empty : dr["betstatus"].ToString();
                        if (betStatus.IndexOf(",") >= 0)
                        {
                            string[] betStatusList = betStatus.Split(',');
                            for (int j = 0; j < betStatusList.Length && j < 5; j++)
                            {
                                homeSet[j] = betStatusList[j].Trim().Split('-')[0].Trim();
                                awaySet[j] = betStatusList[j].Trim().Split('-')[1].Trim();

                                if (j == 4)
                                {
                                    OTHome += Convert.ToInt32(homeSet[j]);
                                    OTAway += Convert.ToInt32(awaySet[j]);
                                }
                            }
                        }
                    }

                    rowBlock.Q1Away = awaySet[0];
                    rowBlock.Q2Away = awaySet[1];
                    rowBlock.Q3Away = awaySet[2];
                    rowBlock.Q4Away = awaySet[3];

                    rowBlock.Q1Home = homeSet[0];
                    rowBlock.Q2Home = homeSet[1];
                    rowBlock.Q3Home = homeSet[2];
                    rowBlock.Q4Home = homeSet[3];

                    rowBlock.OTHome = OTHome > 0 ? OTHome.ToString() : string.Empty;
                    rowBlock.OTAway = OTAway > 0 ? OTAway.ToString() : string.Empty;

                    template.RowData.Append(rowBlock);
                }

                template.LimitNotFound.Hide();
            }
            else
            {
                template.LimitNotFound.NoInfoRow.NumCols = "10";
            }

            excelBlock.Append(template);
        }

        #endregion BasketBall

        #region Hockey

        /// <summary>
        /// Render hockey Result
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="excelBlock"></param>
        private void RenderHockey(DataTable reportData, Result_ExcelContents_Block excelBlock)
        {
            // Get report rows
            Result.ResultHockey_Template template = new Fanex.Age.UI.Reports.Result.ResultHockey_Template();

            int numRow = reportData.Rows.Count;
            template.RowData.Clear();
            if (numRow > 0)
            {
                int currentLeagueId = 0;
                Result.ResultHockey_RowData_League_Block leagueBlock = new Fanex.Age.UI.Reports.Result.ResultHockey_RowData_League_Block();
                Result.ResultHockey_RowData_Row_Block rowBlock = new Fanex.Age.UI.Reports.Result.ResultHockey_RowData_Row_Block();

                // Do NOT show league block when Pop up the result of matches
                if (this._matchId > 0)
                {
                    leagueBlock.Visible = false;
                }

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];
                    int leagueid = Convert.ToInt32(dr["leagueid"]);
                    if (currentLeagueId == 0 || currentLeagueId != leagueid)
                    {
                        currentLeagueId = leagueid;
                        leagueBlock.LeagueName = CacheRef.GetLeagueName(leagueid);
                        template.RowData.Append(leagueBlock);
                    }

                    rowBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    rowBlock.KickOffTime = Formater.FullDateFormat2Line(dr["kickofftime"]);
                    rowBlock.EventStatus = this.MapEventStatusString(dr["eventstatus"].ToString(), dr.Table.Columns.Contains("IsPending") ? Convert.ToBoolean(dr["IsPending"]) : false, dr.Table.Columns.Contains("IsHTCal") ? Convert.ToBoolean(dr["IsHTCal"]) : false, dr.Table.Columns.Contains("calculate") ? Convert.ToBoolean(dr["calculate"]) : false);
                    rowBlock.FinalScoreHome = dr["finalhomescore"].ToString();
                    rowBlock.FinalScoreAway = dr["finalawayscore"].ToString();

                    // Home and away team
                    string homeTeamText = string.Empty, awayTeamText = string.Empty;
                    this.BoldTeamByFinalScore(dr, ref homeTeamText, ref awayTeamText);
                    rowBlock.Home = homeTeamText;
                    rowBlock.Away = awayTeamText;

                    // Bet status
                    int OTHome = 0, OTAway = 0;
                    string[] homeSet = new string[4] { "-", "-", "-", "-" };
                    string[] awaySet = new string[4] { "-", "-", "-", "-" };
                    string betStatus = Convert.IsDBNull(dr["betstatus"]) ? string.Empty : dr["betstatus"].ToString();
                    if (betStatus.IndexOf(",") >= 0)
                    {
                        string[] betStatusList = betStatus.Split(',');
                        for (int j = 0; j < betStatusList.Length && j < 4; j++)
                        {
                            homeSet[j] = betStatusList[j].Trim().Split('-')[0].Trim();
                            awaySet[j] = betStatusList[j].Trim().Split('-')[1].Trim();
                            if (j == 3)
                            {
                                OTHome = Convert.ToInt32(homeSet[j]);
                                OTAway = Convert.ToInt32(awaySet[j]);
                            }
                        }
                    }

                    rowBlock.P1Away = awaySet[0];
                    rowBlock.P2Away = awaySet[1];
                    rowBlock.P3Away = awaySet[2];

                    rowBlock.P1Home = homeSet[0];
                    rowBlock.P2Home = homeSet[1];
                    rowBlock.P3Home = homeSet[2];

                    rowBlock.OTHome = OTHome > 0 ? OTHome.ToString() : "-";
                    rowBlock.OTAway = OTAway > 0 ? OTAway.ToString() : "-";

                    template.RowData.Append(rowBlock);
                }

                template.LimitNotFound.Hide();
            }
            else
            {
                template.LimitNotFound.NoInfoRow.NumCols = "8";
            }

            excelBlock.Append(template);
        }

        #endregion Hockey

        #region Tennis

        /// <summary>
        /// Render tennis result
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="excelBlock"></param>
        private void RenderTennis(DataTable reportData, Result_ExcelContents_Block excelBlock)
        {
            // Get report rows
            Result.ResultTennis_Template template = new Fanex.Age.UI.Reports.Result.ResultTennis_Template();

            int numRow = reportData.Rows.Count;
            template.RowData.Clear();
            if (numRow > 0)
            {
                int currentLeagueId = 0;
                var setId = string.Empty;
                var setScore1 = string.Empty;
                var setScore2 = string.Empty;
                DataTable setDetailData = new DataTable();
                string resultSetDetail = string.Empty;
                Result.ResultTennis_RowData_League_Block leagueBlock = new Fanex.Age.UI.Reports.Result.ResultTennis_RowData_League_Block();
                Result.ResultTennis_RowData_Row_Block rowBlock = new Fanex.Age.UI.Reports.Result.ResultTennis_RowData_Row_Block();

                setDetailData = _resultRepository.SelectMatchForViewResult(this._matchId, refNo.ToString());

                if (setDetailData != null && setDetailData.Rows.Count > 0)
                {
                    DataRow dr = setDetailData.Rows[0];
                    resultSetDetail = dr["resultvalue"].ToString();
                }
                else
                {
                    template.SetDetail = null;
                }
                // Do NOT show league block when Pop up the result of matches
                if (this._matchId > 0)
                {
                    leagueBlock.Visible = false;
                }

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];
                    int leagueid = Convert.ToInt32(dr["leagueid"]);
                    if (currentLeagueId == 0 || currentLeagueId != leagueid)
                    {
                        currentLeagueId = leagueid;
                        leagueBlock.LeagueName = CacheRef.GetLeagueName(leagueid);
                        template.RowData.Append(leagueBlock);
                    }

                    rowBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    rowBlock.KickOffTime = Formater.FullDateFormat2Line(dr["kickofftime"]);
                    rowBlock.EventStatus = this.MapEventStatusString(dr["eventstatus"].ToString(), dr.Table.Columns.Contains("IsPending") ? Convert.ToBoolean(dr["IsPending"]) : false, dr.Table.Columns.Contains("IsHTCal") ? Convert.ToBoolean(dr["IsHTCal"]) : false, dr.Table.Columns.Contains("calculate") ? Convert.ToBoolean(dr["calculate"]) : false);

                    // Home and away team
                    string homeTeamText = string.Empty, awayTeamText = string.Empty;
                    this.BoldTeamByFinalScore(dr, ref homeTeamText, ref awayTeamText);
                    rowBlock.Home = homeTeamText;
                    rowBlock.Away = awayTeamText;

                    // Bet status
                    string[] homeSet = new string[5] { "-", "-", "-", "-", "-" };
                    string[] awaySet = new string[5] { "-", "-", "-", "-", "-" };
                    int totalGameHome = 0, totalGameAway = 0;
                    string betStatus = Convert.IsDBNull(dr["betstatus"]) ? string.Empty : dr["betstatus"].ToString();
                    if (betStatus.IndexOf(",") >= 0)
                    {
                        string[] betStatusList = betStatus.Split(',');
                        for (int j = 0; j < betStatusList.Length && j < 5; j++)
                        {
                            homeSet[j] = betStatusList[j].Trim().Split('-')[0].Trim();
                            awaySet[j] = betStatusList[j].Trim().Split('-')[1].Trim();
                            totalGameHome += Convert.ToInt32(homeSet[j]);
                            totalGameAway += Convert.ToInt32(awaySet[j]);
                        }
                    }

                    rowBlock.S1Away = awaySet[0];
                    rowBlock.S2Away = awaySet[1];
                    rowBlock.S3Away = awaySet[2];
                    rowBlock.S4Away = awaySet[3];
                    rowBlock.S5Away = awaySet[4];

                    rowBlock.S1Home = homeSet[0];
                    rowBlock.S2Home = homeSet[1];
                    rowBlock.S3Home = homeSet[2];
                    rowBlock.S4Home = homeSet[3];
                    rowBlock.S5Home = homeSet[4];

                    if (resultSetDetail.IndexOf("*") >= 0)
                    {
                        string[] betStatusList = resultSetDetail.Replace("*", string.Empty).Split(' ');

                        for (int j = 1; j < betStatusList.Length; j++)
                        {
                            setId += string.Format("<td class='{0}'>{1}{2}", "RptHeader", j.ToString(), "</td>");
                            setScore1 += string.Format("{0}{1}{2}", "<td>", betStatusList[j].Trim().Split('-')[0].Trim(), "</td>");
                            setScore2 += string.Format("{0}{1}{2}", "<td>", betStatusList[j].Trim().Split('-')[1].Trim(), "</td>");
                        }

                        //Set detail
                        template.SetDetail.setId = setId;
                        template.SetDetail.setScore1 = setScore1;
                        template.SetDetail.setScore2 = setScore2;
                        template.SetDetail.setName = Resources.BetList.set + ' ' + refNo.ToString() + ' ' + Resources.Lang.lblScore;
                    }

                    rowBlock.TotalGameAway = totalGameAway > 0 ? totalGameAway.ToString() : string.Empty;
                    rowBlock.TotalGameHome = totalGameHome > 0 ? totalGameHome.ToString() : string.Empty;
                    rowBlock.TotalSetAway = Convert.IsDBNull(dr["finalawayscore"]) ? string.Empty : dr["finalawayscore"].ToString();
                    rowBlock.TotalSetHome = Convert.IsDBNull(dr["finalhomescore"]) ? string.Empty : dr["finalhomescore"].ToString();
                    template.RowData.Append(rowBlock);
                }

                template.LimitNotFound.Hide();
            }
            else
            {
                template.LimitNotFound.NoInfoRow.NumCols = "10";
                template.SetDetail = null;
            }

            excelBlock.Append(template);
        }

        #endregion Tennis

        #region BaseBall

        /// <summary>
        /// Render base ball result
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="excelBlock"></param>
        private void RenderBaseBall(DataTable reportData, Result_ExcelContents_Block excelBlock)
        {
            // Get report rows
            Result.ResultBaseBall_Template template = new Fanex.Age.UI.Reports.Result.ResultBaseBall_Template();

            int numRow = reportData.Rows.Count;
            template.RowData.Clear();
            if (numRow > 0)
            {
                int currentLeagueId = 0;
                Result.ResultBaseBall_RowData_League_Block leagueBlock = new Fanex.Age.UI.Reports.Result.ResultBaseBall_RowData_League_Block();
                Result.ResultBaseBall_RowData_Row_Block rowBlock = new Fanex.Age.UI.Reports.Result.ResultBaseBall_RowData_Row_Block();

                // Do NOT show league block when Pop up the result of matches
                if (this._matchId > 0)
                {
                    leagueBlock.Visible = false;
                }

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];
                    int leagueid = Convert.ToInt32(dr["leagueid"]);
                    if (currentLeagueId == 0 || currentLeagueId != leagueid)
                    {
                        currentLeagueId = leagueid;
                        leagueBlock.LeagueName = CacheRef.GetLeagueName(leagueid);
                        template.RowData.Append(leagueBlock);
                    }

                    rowBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    rowBlock.KickOffTime = Formater.FullDateFormat2Line(dr["kickofftime"]);
                    rowBlock.EventStatus = this.MapEventStatusString(dr["eventstatus"].ToString(), dr.Table.Columns.Contains("IsPending") ? Convert.ToBoolean(dr["IsPending"]) : false, dr.Table.Columns.Contains("IsHTCal") ? Convert.ToBoolean(dr["IsHTCal"]) : false, dr.Table.Columns.Contains("calculate") ? Convert.ToBoolean(dr["calculate"]) : false);

                    // Home and away team
                    string homeTeamText = string.Empty, awayTeamText = string.Empty;
                    this.BoldTeamByFinalScore(dr, ref homeTeamText, ref awayTeamText);
                    rowBlock.Home = homeTeamText;
                    rowBlock.Away = awayTeamText;

                    // Bet status
                    string[] homeSet = new string[10] { "-", "-", "-", "-", "-", "-", "-", "-", "-", "-1" };
                    string[] awaySet = new string[10] { "-", "-", "-", "-", "-", "-", "-", "-", "-", "-1" };
                    string betStatus = Convert.IsDBNull(dr["betstatus"]) ? string.Empty : dr["betstatus"].ToString();
                    if (betStatus.IndexOf(",") >= 0)
                    {
                        string[] betStatusList = betStatus.Split(',');
                        for (int j = 0; j < betStatusList.Length && j < 10; j++)
                        {
                            homeSet[j] = betStatusList[j].Trim().Split('-')[0].Trim();
                            awaySet[j] = betStatusList[j].Trim().Split('-')[1].Trim();
                        }
                    }

                    rowBlock.Away1 = awaySet[0];
                    rowBlock.Away2 = awaySet[1];
                    rowBlock.Away3 = awaySet[2];
                    rowBlock.Away4 = awaySet[3];
                    rowBlock.Away5 = awaySet[4];
                    rowBlock.Away6 = awaySet[5];
                    rowBlock.Away7 = awaySet[6];
                    rowBlock.Away8 = awaySet[7];
                    rowBlock.Away9 = awaySet[8];

                    rowBlock.Home1 = homeSet[0];
                    rowBlock.Home2 = homeSet[1];
                    rowBlock.Home3 = homeSet[2];
                    rowBlock.Home4 = homeSet[3];
                    rowBlock.Home5 = homeSet[4];
                    rowBlock.Home6 = homeSet[5];
                    rowBlock.Home7 = homeSet[6];
                    rowBlock.Home8 = homeSet[7];
                    rowBlock.Home9 = homeSet[8];

                    int totalGameHome = Convert.ToInt32(homeSet[9]), totalGameAway = Convert.ToInt32(awaySet[9]);

                    rowBlock.EIAway = totalGameAway >= 0 ? totalGameAway.ToString() : "-";
                    rowBlock.EIHome = totalGameHome >= 0 ? totalGameHome.ToString() : "-";
                    rowBlock.HalfTimeScoreHome = Convert.IsDBNull(dr["hthomescore"]) ? string.Empty : dr["hthomescore"].ToString();
                    rowBlock.HalfTimeScoreAway = Convert.IsDBNull(dr["htawayscore"]) ? string.Empty : dr["htawayscore"].ToString();
                    rowBlock.FinalScoreHome = Convert.IsDBNull(dr["finalhomescore"]) ? string.Empty : dr["finalhomescore"].ToString();
                    rowBlock.FinalScoreAway = Convert.IsDBNull(dr["finalawayscore"]) ? string.Empty : dr["finalawayscore"].ToString();

                    template.RowData.Append(rowBlock);
                }

                template.LimitNotFound.Hide();
            }
            else
            {
                template.LimitNotFound.NoInfoRow.NumCols = "15";
            }

            excelBlock.Append(template);
        }

        #endregion BaseBall

        #region Billiard

        /// <summary>
        /// Render billiard result
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="excelBlock"></param>
        private void RenderBilliard(DataTable reportData, Result_ExcelContents_Block excelBlock)
        {
            // Get report rows
            Result.ResultBilliard_Template template = new Fanex.Age.UI.Reports.Result.ResultBilliard_Template();

            int numRow = reportData.Rows.Count;
            template.RowData.Clear();
            if (numRow > 0)
            {
                int currentLeagueId = 0;
                Result.ResultBilliard_RowData_League_Block leagueBlock = new Fanex.Age.UI.Reports.Result.ResultBilliard_RowData_League_Block();
                Result.ResultBilliard_RowData_Row_Block rowBlock = new Fanex.Age.UI.Reports.Result.ResultBilliard_RowData_Row_Block();

                // Do NOT show league block when Pop up the result of matches
                if (this._matchId > 0)
                {
                    leagueBlock.Visible = false;
                }

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];
                    int leagueid = Convert.ToInt32(dr["leagueid"]);
                    if (currentLeagueId == 0 || currentLeagueId != leagueid)
                    {
                        currentLeagueId = leagueid;
                        leagueBlock.LeagueName = CacheRef.GetLeagueName(leagueid);
                        template.RowData.Append(leagueBlock);
                    }

                    rowBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    rowBlock.KickOffTime = Formater.FullDateFormat2Line(dr["kickofftime"]);
                    rowBlock.EventStatus = this.MapEventStatusString(dr["eventstatus"].ToString(), dr.Table.Columns.Contains("IsPending") ? Convert.ToBoolean(dr["IsPending"]) : false, dr.Table.Columns.Contains("IsHTCal") ? Convert.ToBoolean(dr["IsHTCal"]) : false, dr.Table.Columns.Contains("calculate") ? Convert.ToBoolean(dr["calculate"]) : false);

                    // Home and away team
                    string homeTeamText = string.Empty, awayTeamText = string.Empty;
                    this.BoldTeamByFinalScore(dr, ref homeTeamText, ref awayTeamText);
                    rowBlock.Home = homeTeamText;
                    rowBlock.Away = awayTeamText;

                    rowBlock.FinalScoreHome = Convert.IsDBNull(dr["finalhomescore"]) ? string.Empty : dr["finalhomescore"].ToString();
                    rowBlock.FinalScoreAway = Convert.IsDBNull(dr["finalawayscore"]) ? string.Empty : dr["finalawayscore"].ToString();

                    template.RowData.Append(rowBlock);
                }

                template.LimitNotFound.Hide();
            }
            else
            {
                template.LimitNotFound.NoInfoRow.NumCols = "4";
            }

            excelBlock.Append(template);
        }

        #endregion Billiard

        #region HorseRacing

        /// <summary>
        /// Render horse racing result
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="excelBlock"></param>
        private void RenderHorseRacing(DataTable reportData, Result_ExcelContents_Block excelBlock)
        {
            // Get report rows
            Result.ResultHorseRacing_Template template = new Fanex.Age.UI.Reports.Result.ResultHorseRacing_Template();

            int numRow = reportData.Rows.Count;
            template.RowData.Clear();
            if (numRow > 0)
            {
                int currentLeagueId = 0;
                Result.ResultHorseRacing_RowData_League_Block leagueBlock = new Fanex.Age.UI.Reports.Result.ResultHorseRacing_RowData_League_Block();
                Result.ResultHorseRacing_RowData_Row_Block rowBlock = new Fanex.Age.UI.Reports.Result.ResultHorseRacing_RowData_Row_Block();

                // Do NOT show league block when Pop up the result of matches
                if (this._matchId > 0)
                {
                    leagueBlock.Visible = false;
                }

                int currentRace = 0, raceCounter = 0;
                bool newLeague = false;
                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];
                    if (Convert.ToInt32(dr["place"]) == 0)
                    {
                        continue;
                    }

                    if (Convert.ToInt32(dr["place"]) >= 5)
                    {
                        continue;
                    }

                    int leagueid = Convert.ToInt32(dr["meetingid"]);
                    if (currentLeagueId == 0 || currentLeagueId != leagueid)
                    {
                        currentLeagueId = leagueid;
                        leagueBlock.LeagueName = CacheRef.GetLeagueName(leagueid);
                        template.RowData.Append(leagueBlock);
                        newLeague = true;
                    }
                    else
                    {
                        newLeague = false;
                    }

                    if (newLeague || currentRace != Convert.ToInt32(dr["racename"]))
                    {
                        raceCounter++;
                        currentRace = Convert.ToInt32(dr["racename"]);

                        // Calculate row span
                        DataRow[] rowsTopRace = reportData.Select(string.Format("racename={0} and meetingid={1} and place<5", currentRace, currentLeagueId));

                        rowBlock.STTContent.Show();
                        rowBlock.StatusContent.Show();
                        rowBlock.STTContent.RowSpan = rowsTopRace == null ? "0" : rowsTopRace.Length.ToString();
                        rowBlock.StatusContent.RowSpan = rowsTopRace == null ? "0" : rowsTopRace.Length.ToString();
                        rowBlock.STTContent.STT = currentRace.ToString();
                        this.RenderHorseRacingSCR(reportData, leagueid, currentRace, rowBlock.StatusContent);
                        this.RenderHorseRacingTF(reportData, leagueid, currentRace, rowBlock.StatusContent);
                    }
                    else
                    {
                        rowBlock.STTContent.Hide();
                        rowBlock.StatusContent.Hide();
                    }

                    rowBlock.ColorClass = raceCounter % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    rowBlock.HorseNo = dr["horseno"].ToString();
                    rowBlock.Placing = this.GetHorseRacingPlaceString(dr["place"].ToString());
                    rowBlock.Place = Convert.ToDecimal(dr["placeodds"]) > 0 ? Formater.FormatNumber20(dr["placeodds"], "") : string.Empty; // CurrentUserSession.LangKey
                    rowBlock.Win = Convert.ToDecimal(dr["odds"]) > 0 ? Formater.FormatNumber20(dr["odds"], "") : string.Empty; // CurrentUserSession.LangKey
                    rowBlock.Result = CacheRef.GetHorseTeamName(dr["horseid"]);

                    template.RowData.Append(rowBlock);
                }

                template.LimitNotFound.Hide();
            }
            else
            {
                template.LimitNotFound.NoInfoRow.NumCols = "7";
            }

            excelBlock.Append(template);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="placeNo"></param>
        /// <returns></returns>
        private string GetHorseRacingPlaceString(string placeNo)
        {
            switch (placeNo)
            {
                case "01":
                case "1":
                    return Resources.Lang.No1;
                case "02":
                case "2":
                    return Resources.Lang.No2;
                case "03":
                case "3":
                    return Resources.Lang.No3;
                case "04":
                case "4":
                    return Resources.Lang.No4;
                default:
                    return placeNo + Resources.Lang.noothers;
            }
        }

        /// <summary>
        /// Render horse racing tf
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="meetingId"></param>
        /// <param name="raceName"></param>
        /// <param name="statusBlock"></param>
        private void RenderHorseRacingTF(DataTable reportData, int meetingId, int raceName, Result.ResultHorseRacing_RowData_Row_StatusContent_Block statusBlock)
        {
            string expression = string.Format("meetingid={0} and racename={1} and isfavor=1", meetingId, raceName);
            DataRow[] reportRows = reportData.Select(expression);

            StringBuilder sb = new StringBuilder();
            for (int i = 0, rowCount = reportRows.Length; i < rowCount; i++)
            {
                sb.Append(reportRows[i]["horseno"]);
                if (i < rowCount - 1)
                {
                    sb.Append(", ");
                }
            }

            statusBlock.TFCOntent.TF = sb.ToString();
            if (statusBlock.TFCOntent.TF.Length == 0)
            {
                statusBlock.TFCOntent.Hide();
            }
            else
            {
                statusBlock.TFCOntent.Show();
            }
        }

        /// <summary>
        /// Render horse racing SCR
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="meetingId"></param>
        /// <param name="raceName"></param>
        /// <param name="statusBlock"></param>
        /// <returns></returns>
        private void RenderHorseRacingSCR(DataTable reportData, int meetingId, int raceName, Result.ResultHorseRacing_RowData_Row_StatusContent_Block statusBlock)
        {
            string expression = string.Format("meetingid={0} and racename={1} and isscr=1", meetingId, raceName);
            DataRow[] reportRows = reportData.Select(expression);

            bool isFoddsStatus = false;
            StringBuilder sb = new StringBuilder();
            for (int i = 0, rowCount = reportRows.Length; i < rowCount; i++)
            {
                sb.Append(reportRows[i]["horseno"]);
                if (i < rowCount - 1)
                {
                    sb.Append(", ");
                }

                if (isFoddsStatus == false && reportRows[i]["foddsstatus"].ToString().ToLower() == "completed")
                {
                    isFoddsStatus = true;
                    statusBlock.SCRContent.SCRInfo.EventDate = Formater.FormatDateEn(reportRows[i]["eventdate"]);
                    statusBlock.SCRContent.SCRInfo.LeagueId = meetingId.ToString();
                    statusBlock.SCRContent.SCRInfo.RaceName = raceName.ToString();
                }
            }

            statusBlock.SCRContent.SCR = sb.ToString();
            if (statusBlock.SCRContent.SCR.Length == 0)
            {
                statusBlock.SCRContent.Hide();
            }
            else
            {
                statusBlock.SCRContent.Show();
                if (isFoddsStatus)
                {
                    statusBlock.SCRContent.SCRInfo.Show();
                }
                else
                {
                    statusBlock.SCRContent.SCRInfo.Hide();
                }
            }
        }

        #endregion HorseRacing

        #region OtherSport

        /// <summary>
        /// Render other sport result
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="excelBlock"></param>
        private void RenderOtherSport(DataTable reportData, Result_ExcelContents_Block excelBlock)
        {
            // Get report rows
            Result.ResultOtherSport_Template template = new Fanex.Age.UI.Reports.Result.ResultOtherSport_Template();

            int numRow = reportData.Rows.Count;
            template.RowData.Clear();
            if (numRow > 0)
            {
                int currentLeagueId = 0;
                Result.ResultOtherSport_RowData_League_Block leagueBlock = new Fanex.Age.UI.Reports.Result.ResultOtherSport_RowData_League_Block();
                Result.ResultOtherSport_RowData_Row_Block rowBlock = new Fanex.Age.UI.Reports.Result.ResultOtherSport_RowData_Row_Block();

                // Do NOT show league block when Pop up the result of matches
                if (this._matchId > 0)
                {
                    leagueBlock.Visible = false;
                }

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];
                    int leagueid = Convert.ToInt32(dr["leagueid"]);
                    if (currentLeagueId == 0 || currentLeagueId != leagueid)
                    {
                        currentLeagueId = leagueid;
                        leagueBlock.LeagueName = CacheRef.GetLeagueName(leagueid);
                        template.RowData.Append(leagueBlock);
                    }

                    rowBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                    rowBlock.KickOffTime = Formater.FullDateFormat2Line(dr["kickofftime"]);
                    rowBlock.EventStatus = this.MapEventStatusString(dr["eventstatus"].ToString(), dr.Table.Columns.Contains("IsPending") ? Convert.ToBoolean(dr["IsPending"]) : false, dr.Table.Columns.Contains("IsHTCal") ? Convert.ToBoolean(dr["IsHTCal"]) : false, dr.Table.Columns.Contains("calculate") ? Convert.ToBoolean(dr["calculate"]) : false);
                    rowBlock.FinalScoreHome = Convert.IsDBNull(dr["finalhomescore"]) ? string.Empty : dr["finalhomescore"].ToString();
                    rowBlock.FinalScoreAway = Convert.IsDBNull(dr["finalawayscore"]) ? string.Empty : dr["finalawayscore"].ToString();
                    rowBlock.HTScoreHome = Convert.IsDBNull(dr["hthomescore"]) ? string.Empty : dr["hthomescore"].ToString();
                    rowBlock.HTScoreAway = Convert.IsDBNull(dr["htawayscore"]) ? string.Empty : dr["htawayscore"].ToString();

                    // Home and away team
                    string homeTeamText = string.Empty, awayTeamText = string.Empty;
                    this.BoldTeamByFinalScore(dr, ref homeTeamText, ref awayTeamText);
                    rowBlock.Home = homeTeamText;
                    rowBlock.Away = awayTeamText;

                    template.RowData.Append(rowBlock);
                }

                template.LimitNotFound.Hide();
            }
            else
            {
                template.LimitNotFound.NoInfoRow.NumCols = "5";
            }

            excelBlock.Append(template);
        }

        #endregion OtherSport

        #region NumberGame

        /// <summary>
        /// Render number result
        /// </summary>
        /// <param name="reportData"></param>
        /// <param name="excelBlock"></param>
        private void RenderNumberGame(DataTable reportData, Result_ExcelContents_Block excelBlock)
        {
            // Get report rows
            Result.ResultNumberGame_Template template = new Fanex.Age.UI.Reports.Result.ResultNumberGame_Template();
            int numRow = reportData.Rows.Count;
            template.Content.Clear();
            if (numRow > 0)
            {
                Result.ResultNumberGame_Content_League_Block leagueBlock = new ResultNumberGame_Content_League_Block();
                Result.ResultNumberGame_Content_Match_Block matchBlock = new ResultNumberGame_Content_Match_Block();

                // Do NOT show league block when Pop up the result of matches
                if (this._matchId > 0)
                {
                    leagueBlock.Visible = false;
                }

                var leagueList = reportData.AsEnumerable().Select(row => row.Field<int>("leagueid")).Distinct<int>().OrderBy(leagueid => leagueid);

                // Traverse each league
                foreach (int leagueId in leagueList)
                {
                    leagueBlock.LeagueName = CacheRef.GetLeagueName(leagueId);
                    template.Content.Append(leagueBlock);

                    var matchList = reportData.AsEnumerable().Where(row => row.Field<int>("leagueid") == leagueId).Select(row => row.Field<int>("matchid")).Distinct<int>().OrderBy(matchid => matchid);

                    // Traverse each match
                    int matchCounter = 0;
                    foreach (int matchId in matchList)
                    {
                        var matchDataList = reportData.AsEnumerable().Where(row => row.Field<int>("matchid") == matchId);
                        var seqDataList = matchDataList.OrderBy(row => row.Field<byte?>("seq"));
                        var ballList = seqDataList.Select(row => row.Field<byte?>("number")).ToArray();
                        var ballCount = seqDataList.Where(row => row.Field<byte?>("number") != null && row.Field<byte?>("number").ToString() != string.Empty).Count();
                        var dataRow = matchDataList.First();
                        string status = this.MapEventStatusString(dataRow["eventstatus"].ToString(), dataRow.Table.Columns.Contains("IsPending") ? Convert.ToBoolean(dataRow["IsPending"]) : false, false, false);

                        matchBlock.Ball.Clear();

                        // Show empty if there is no ball
                        if (ballCount == 0)
                        {
                            matchBlock.FirstBall = string.Empty;
                            matchBlock.LastBall = string.Empty;

                            // matchBlock.LastLiveBall = string.Empty;
                            matchBlock.OEFT = string.Empty;
                            matchBlock.warrior = string.Empty;
                        }
                        else
                        {
                            // Count odd ball, even ball, over ball, under ball
                            // over ball: ballNumber>=38, under ball: ball number <38
                            // Render result balls
                            int ballSum = 0, ballOverCount = 0, ballUnderCount = 0, ballCounter = 0;
                            foreach (byte? ballNumber in ballList)
                            {
                                ballCounter++;

                                matchBlock.Ball.Append(ballNumber == null ? string.Empty : this.GenerateNumberGameBall((int)ballNumber));
                                if (_exportToExcel && ballCounter < ballList.Length)
                                {
                                    matchBlock.Ball.Append(" - ");
                                }

                                // Each row does not contains1 over 15 balls
                                if (!_exportToExcel && ballCounter % 15 == 0)
                                {
                                    matchBlock.Ball.Append("<br/>");
                                }

                                ballSum += ballNumber == null ? 0 : (int)ballNumber;
                                if (ballNumber >= 38)
                                {
                                    ballOverCount++;
                                }
                                else
                                {
                                    ballUnderCount++;
                                }
                            }

                            // Render first ball
                            matchBlock.FirstBall = (ballList[0] == null) ? string.Empty : this.GenerateNumberGameBall((int)ballList[0]);

                            // Render last ball
                            if (ballCount > 1)
                            {
                                matchBlock.LastBall = ballList[ballCount - 1] == null ? NA : this.GenerateNumberGameBall((int)ballList[ballCount - 1]);
                            }
                            else
                            {
                                matchBlock.LastBall = NA;
                            }

                            // Render last live ball: if ballCount< MAX_LIVE_BALL_COUNT, last live ball=last ball, else last live ball=ball MAX_LIVE_BALL_COUNT
                            // byte lastLiveBall = ballCount <= MAX_LIVE_BALL_COUNT ? ballList[ballCount - 1] : ballList[MAX_LIVE_BALL_COUNT - 1];
                            // matchBlock.LastLiveBall = this.GenerateNumberGameBall(lastLiveBall);

                            // Render Odd even full time, over under full time
                            if (status.ToLower() == Resources.Lang.running.ToLower())
                            {
                                matchBlock.OEFT = matchBlock.LastBall = string.Empty;
                            }
                            else
                            {
                                matchBlock.OEFT = ballSum % 2 == 1 ? Resources.Lang.lblOdd : Resources.Lang.even;
                            }

                            if (ballList.Length > 2)
                            {
                                matchBlock.warrior = ballList[1] > ballList[2] ? Resources.Lang.warrior2ndBall : Resources.Lang.warrior3rdBall;
                            }
                            else
                            {
                                matchBlock.warrior = null;
                            }
                        }

                        // Match data
                        matchCounter++;
                        matchBlock.ColorClass = matchCounter % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
                        matchBlock.KickOffTime = Formater.FullDateFormat2Line(dataRow["kickofftime"]);
                        matchBlock.EventStatus = status;
                        matchBlock.GameNo = dataRow["matchcode"].ToString();
                        template.Content.Append(matchBlock);
                    }
                }
            }
            else
            {
                Result.ResultNumberGame_Content_NotFound_Block notFoundBlock = new ResultNumberGame_Content_NotFound_Block();
                notFoundBlock.NoInfoRow.NumCols = "8";
                template.Content.Append(notFoundBlock);
            }

            excelBlock.Append(template);
        }

        /// <summary>
        /// If export excel, return the number in string
        /// else return the image
        /// </summary>
        /// <param name="ballNumber"></param>
        /// <returns></returns>
        private string GenerateNumberGameBall(int ballNumber)
        {
            return string.Empty;
        }

        #endregion NumberGame

        #endregion RenderBySportType

        #region Utitlity

        /// <summary>
        /// Bold the name of win team
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="homeScoreText"></param>
        /// <param name="awayScoreText"></param>
        private void BoldTeamByFinalScore(DataRow dr, ref string homeTeamText, ref string awayTeamText)
        {
            // Home and away team
            string homeTeam = CacheRef.GetTeamName(dr["homeid"]) + ((bool)dr["isNeutral"] ? Resources.Lang.IsNeutral : string.Empty);
            string awayTeam = CacheRef.GetTeamName(dr["awayid"]);

            int homeScore = Convert.IsDBNull(dr["finalhomescore"]) ? 0 : Convert.ToInt32(dr["finalhomescore"]);
            int awayScore = Convert.IsDBNull(dr["finalawayscore"]) ? 0 : Convert.ToInt32(dr["finalawayscore"]);

            if (homeScore > awayScore)
            {
                homeTeamText = string.Format("<b>{0}</b>", homeTeam);
                awayTeamText = awayTeam;
            }
            else if (awayScore > homeScore)
            {
                awayTeamText = string.Format("<b>{0}</b>", awayTeam);
                homeTeamText = homeTeam;
            }
            else
            {
                awayTeamText = awayTeam;
                homeTeamText = homeTeam;
            }
        }

        /// <summary>
        /// Return 0X if X less than 10
        /// else return X
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string FormatTwoDigit(int number)
        {
            return number < 10 ? "0" + number.ToString() : number.ToString();
        }

        /// <summary>
        /// Genrate bingo ball image url
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string GenerateBingoBallImageUrl(int number)
        {
            //return url;
            return string.Empty;
        }

        /// <summary>
        /// Get absolute url
        /// </summary>
        /// <param name="relativeUrl">Ex: image/Bingo/Ball/01.jpg</param>
        /// <returns>http://tadmin.com/agent/image/Bingo/Ball/01.jpg</returns>
        private string GetAbsoluteUrl(string relativeUrl)
        {
            string hostName = Request.Headers["Host"];
            string appPath = Request.ApplicationPath;

            string url = /*Protocol + hostName +*/ appPath;

            if (appPath.Length > 1)
            {
                url += '/';
            }

            return url + relativeUrl;
        }

        #endregion Utitlity

        #region Map event status to coresponding language

        // Define list key for eventstatus
        static Dictionary<string, int> _event = new Dictionary<string, int>()
            {
                { "running", Convert.ToInt32("0000000", 2) },
                { "completed", Convert.ToInt32("0000001", 2) },
                { "closed", Convert.ToInt32("0000010", 2) },
                { "postponed", Convert.ToInt32("0000011", 2) },
                { "refund", Convert.ToInt32("0000100", 2) },
                { "refund ft", Convert.ToInt32("0000101", 2) },
                { "refund ht", Convert.ToInt32("0000110", 2) },
                { "won", Convert.ToInt32("0000111", 2) },
                { "internal", Convert.ToInt32("0001000", 2) }
            };

        // Define list key for Result: isPending(1 bit) isHTCal(1 bit) calculate(1 bit) enventStatus(4 bit)
        static Dictionary<int, string> _result = new Dictionary<int, string>()
            {
                { Convert.ToInt32("0000010", 2), "closed" },
                { Convert.ToInt32("0100010", 2), "closed" },
                { Convert.ToInt32("0000000", 2), "running" },
                { Convert.ToInt32("0100000", 2), "running" },
                { Convert.ToInt32("1000000", 2), "pending" },
                { Convert.ToInt32("1010010", 2), "pending" },
                { Convert.ToInt32("1100010", 2), "pending" },
                { Convert.ToInt32("1100000", 2), "pending" },
                { Convert.ToInt32("1010000", 2), "pending" },
                { Convert.ToInt32("0110001", 2), "completed" },
                { Convert.ToInt32("1000010", 2), "pending" },
                { Convert.ToInt32("0000100", 2), "refund" },
                { Convert.ToInt32("0000101", 2), "refundft" },
                { Convert.ToInt32("0010101", 2), "refundft" },
                { Convert.ToInt32("0010110", 2), "refundht" },
                { Convert.ToInt32("0000110", 2), "refundht" },
                { Convert.ToInt32("0010111", 2), "lblWon" },
                { Convert.ToInt32("0000111", 2), "lblWon" },
                { Convert.ToInt32("0010011", 2), "postponed" },
                { Convert.ToInt32("0000011", 2), "postponed" },
                { Convert.ToInt32("0011000", 2), "Internal" },
                { Convert.ToInt32("0001000", 2), "Internal" },
            };

        /// <summary>
        /// Map event status to coresponding language
        /// </summary>
        /// <param name="eventstatus"></param>
        /// <param name="isPending"></param>
        /// <param name="isHTcal"></param>
        /// <param name="calculate"></param>
        /// <returns></returns>
        protected string MapEventStatusString(string eventstatus, bool isPending, bool isHTcal, bool calculate)
        {
            int resultKey = 0x0000000;
            int pending = Convert.ToInt32(isPending);
            int htCal = Convert.ToInt32(isHTcal);
            int cal = Convert.ToInt32(calculate);

            pending <<= 6; // Shift right 6 bit for pending status
            htCal <<= 5; // Shift right 5 bit for haft time calculate status
            cal <<= 4; // Shift right 4 bit for calculate status

            resultKey = pending | htCal | cal;

            if (_event.ContainsKey(eventstatus.ToLowerInvariant()))
            {
                resultKey |= _event[eventstatus.ToLowerInvariant()]; // Or with event status
                if (_result.ContainsKey(resultKey))
                {
                    return this.GetLogResource(_result[resultKey]);
                }
                else
                {
                    // Get status from resource lang, if have.
                    string str = this.GetLogResource(eventstatus);
                    if (str != string.Empty)
                    {
                        return this.GetLogResource(eventstatus.ToLower());
                    }
                }
            }

            return this.UppercaseFirst(eventstatus);
        }

        static object locker = new object();
        static ResourceManager _resourceManager = null;

        public string GetLogResource(string key)
        {
            if (_resourceManager == null)
            {
                lock (locker)
                {
                    if (_resourceManager == null)
                    {
                        _resourceManager = new ResourceManager(typeof(Resources.Lang));
                        _resourceManager.IgnoreCase = true;
                    }
                }
            }

            return _resourceManager.GetString(key) ?? string.Empty;
        }

        public ResourceManager GetLangResourceManager()
        {
            if (_resourceManager == null)
            {
                lock (locker)
                {
                    if (_resourceManager == null)
                    {
                        _resourceManager = new ResourceManager(typeof(Resources.Lang));
                        _resourceManager.IgnoreCase = true;
                    }
                }
            }

            return _resourceManager;
        }

        private string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        #endregion Map event status to coresponding language
    }
}