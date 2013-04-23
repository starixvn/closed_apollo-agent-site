/*
 * Created 20100529@Jacob - Render FCAH page
 * Revision:
 * ?@? - ... 
 * 
 */
namespace Fanex.Age.UI.Reports.MatchWinloss
{
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Reports.Common;
    using Fanex.Age.UI.Reports.Components;
    using Sunplus.Agent.Utilities;

    public partial class MatchWinlossDetail_Page : WinlossReports_Page
    {
        string leagueIds = string.Empty;
        string sportIds = string.Empty;
        string leagueFull = string.Empty;
        string filterLeagueId = "0";
        string filterRace = "0";

        DataTable reportData = null;

        protected override void InitCheckBox()
        {
            base.InitCheckBox();

            _filterCollection.Remove(Filter_Control.SHOWCASINO);
            _filterCollection.Remove(Filter_Control.SHOW_P2P);
        }

        protected new string RenderSearchPanel(bool isShowed)
        {
            WinlossSearchView_Control searchControl = new WinlossSearchView_Control(_filterControl, _dateFrom, _dateTo, isShowed);
            searchControl.IsHistoryReport = _isHistoryReport;
            searchControl.UserType = _userType;
            searchControl.BoldExportExcel = this.BoldChecked;

            searchControl._template.HistoryMessage.GrossCommMsg.Hide();
            searchControl._template.HistoryMessage.OptionMsg.Hide();
            searchControl._template.HistoryMessage.ContentMessage.SetValue(string.Empty);

            //searchControl._template.DateRangeSelect.SetDateRange(MIN_REPORT_DATE, MAX_REPORT_DATE);

            ButtonBoxLeague_Template tplButtonBoxLeague = new ButtonBoxLeague_Template();
            searchControl._template.DateRangeSelect._template.AdditionalContent.SetValue(tplButtonBoxLeague);
            return searchControl.ToString();
        }

        private string BoxLeagues()
        {
            SelLeague_Template tplSelLeague = new SelLeague_Template();
            tplSelLeague.v_leagueIds = leagueIds;
            tplSelLeague.v_leagueFull = leagueFull;

            string[] arrLeagues = leagueIds.Split(',');
            string[] arrLeagueFull = leagueFull.Split(',');
            if (arrLeagueFull.Length > 0 && !string.IsNullOrEmpty(leagueFull))
            {
                StringBuilder strGenLeague = new StringBuilder();
                StringBuilder strLeagueSel = new StringBuilder();

                if (arrLeagueFull.Length > 1)
                {
                    SelLeague_ContentLeague_ColLeague_Block ColLeague = new SelLeague_ContentLeague_ColLeague_Block();
                    ColLeague.League = "All";
                    ColLeague.LeagueName = "<b>" + Resources.Lang.all + "</b>";
                    ColLeague.isChecked = (arrLeagues.Length == arrLeagueFull.Length ? "checked" : string.Empty);
                    ColLeague.checkAll = "onclick='CheckSelAllLeague()'";
                    strLeagueSel.Append(ColLeague);
                }

                for (int i = 0; i < arrLeagueFull.Length; i++)
                {
                    SelLeague_ContentLeague_ColLeague_Block ColLeague = new SelLeague_ContentLeague_ColLeague_Block();
                    ColLeague.League = arrLeagueFull[i].ToString();
                    ColLeague.LeagueName = CacheRef.GetLeagueName(arrLeagueFull[i]);

                    bool isChecked = false;
                    for (int j = 0; j < arrLeagues.Length; j++)
                    {
                        if (arrLeagueFull[i] == arrLeagues[j])
                        {
                            isChecked = true;
                            break;
                        }
                    }
                    ColLeague.isChecked = isChecked ? "checked" : "";

                    ColLeague.checkAll = "onclick=isAllLeague('chL" + arrLeagueFull[i].ToString() + "')";
                    strLeagueSel.Append(ColLeague);
                }

                tplSelLeague.ContentLeague.ColLeague.SetValue(strLeagueSel.ToString());
            }
            else
            {
                tplSelLeague.ContentLeague.ColLeague.SetValue("<div class='no_info'>" + Resources.Lang.noInfo + "</div>");
            }

            return tplSelLeague.ToString();
        }

        public MatchWinlossDetail_Page()
        {
            MIN_REPORT_DATE = Sunplus.Agent.Model.BL.Reports.ReportsBO.BalanceUpDate;
        }

        #region Variable
        #endregion

        #region Override
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            MatchWinlossDetail_Template tpl = new MatchWinlossDetail_Template();

            if (_exportToExcel)
            {

                DataTable reportData = (DataTable)View;
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.MatchWinloss.MatchWinLossDetailReportBuilder(reportData, IsBold, _dateFrom, _dateTo, resMan,
                    CurrentUserSession.IsSuper, CurrentUserSession.IsMaster, CurrentUserSession.IsAgent);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "MatchWinlossDetail.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = "";
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "MatchWinlossDetail");
            }
            else
            {
                // Render Report Header and history message
                RegisterJavascriptObject("FilterLeague", (string.IsNullOrEmpty(Request["fdate"]) || string.IsNullOrEmpty(Request["tdate"])) ? "" : Request["fdate"] + Request["tdate"] + sportIds);
                RegisterJavascriptObject("selectLeague", Resources.Lang.selectLeague);
                RegisterJavascriptObject("hideLeague", Resources.Lang.hideLeague);
                RenderReportHeader(tpl);
                RenderReportContent(tpl.ExcelContents);
                tpl.BoxLeague = BoxLeagues();
                writer.Write(tpl);
            }
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            base.ProcessRequest(ref p);
            leagueIds = string.IsNullOrEmpty(Request["leagueIds"]) ? "" : Request["leagueIds"];
            leagueFull = string.IsNullOrEmpty(Request["leagueFull"]) ? "" : Request["leagueFull"];
            sportIds += string.IsNullOrEmpty(Request["chk_showsb"]) || Request["chk_showsb"].ToString() != "on" ? "0" : "1";
            sportIds += string.IsNullOrEmpty(Request["chk_showrb"]) || Request["chk_showrb"].ToString() != "on" ? "0" : "1";
            sportIds += string.IsNullOrEmpty(Request["chk_showng"]) || Request["chk_showng"].ToString() != "on" ? "0" : "1";
            sportIds += string.IsNullOrEmpty(Request["isChanged"]) ? "" : Request["isChanged"];
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            return _ReportBO.GetMatchWinlossDetail(_dateFrom, _dateTo, _permission, leagueIds);

        }
        #endregion

        #region Render Utility

        /// <summary>
        /// Render Report header
        /// </summary>
        /// <param name="headerTemplate"></param>
        private void RenderReportHeader(MatchWinlossDetail_Template tpl)
        {
            tpl.SearchPanel = RenderSearchPanel(true);
            RenderFilterElementNames();
            tpl.ExcelContents.BoldRow = this.BoldRowClass;
        }
        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeader(MatchWinlossDetail_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }
        /// <summary>
        /// Rnder report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(MatchWinlossDetail_ExcelContents_Block excelBlock)
        {
            reportData = View as DataTable;

            HideRedundantColumn(excelBlock);

            // Render date from and date to
            RenderReportContentHeader(excelBlock);

            excelBlock.Content.Clear();
            int rowCount = reportData.Rows.Count;
            if (rowCount > 0)
            {
                excelBlock.NoInfo.Hide();
                MatchWinlossDetail_ExcelContents_Content_MatchContent_Block matchBlock = new MatchWinlossDetail_ExcelContents_Content_MatchContent_Block();
                MatchWinlossDetail_ExcelContents_Content_BetTypeRow_Block betTypeBlock = new MatchWinlossDetail_ExcelContents_Content_BetTypeRow_Block();
                matchBlock.NumCols = ReportNumColumn.ToString();
                string currentTitle = string.Empty;
                int currentMatchId = 0;
                DataRow drParlay = null, drCombineMixParlay = null;

                for (int i = 0; i < rowCount; i++)
                {
                    DataRow dr = reportData.Rows[i];
                    int betType = Convert.ToInt32(dr["bettype"]);

                    if (Convert.ToInt32(dr["betgroup"]) == 3 && betType == 9) // fro parlay only
                    {
                        drParlay = dr;
                        continue;
                    }

                    if (Convert.ToInt32(dr["betgroup"]) == 3 && betType == 29) // Combination Mix parlay
                    {
                        drCombineMixParlay = dr;
                        continue;
                    }

                    String leagueName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetLeagueName(dr["leagueid"]);

                    // Reset block value
                    matchBlock.MatchName.Show();
                    matchBlock.Show();

                    String betTypeName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetBettype(dr["bettype"]);

                    RenderMatchRow(dr, betType, ref currentTitle, ref currentMatchId, i, rowCount, leagueName, betTypeName, matchBlock, excelBlock);

                    RenderBetTypeRow(dr, currentMatchId, betTypeName, i, betTypeBlock);

                    excelBlock.Content.Append(betTypeBlock);
                }

                if (rowCount > 1 && filterLeagueId != "0")
                {
                    if (filterRace != "0") // is Horse Racing
                    {
                        MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block totalBetTypeBlock = new MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block();
                        RenderTotalBetType("leagueid=" + filterLeagueId + " and race='" + filterRace + "'", totalBetTypeBlock, excelBlock);
                    }
                    else if (currentMatchId != 0)
                    {
                        MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block totalBetTypeBlock = new MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block();
                        RenderTotalBetType("matchid=" + currentMatchId, totalBetTypeBlock, excelBlock);
                    }
                }

                // CaculateTotalValue
                #region CaculateTotalValue
                decimal turnOverTotal = 0, maxComTotal = 0, winMemTotal = 0, comMemTotal = 0, totalMemTotal = 0, winAgtTotal = 0, comAgtTotal = 0, totalAgtTotal = 0, comCompanyTotal = 0, companyTotal = 0;
                decimal winMasTotal = 0, comMasTotal = 0, totalMasTotal = 0, winSupTotal = 0, comSupTotal = 0, totalSupTotal = 0;
                turnOverTotal = GetDBDecimalValue(reportData.Compute("SUM(turnover)", "betgroup<>3"));
                maxComTotal = GetDBDecimalValue(reportData.Compute("SUM(GrossComm)", "betgroup<>3"));
                winMemTotal = GetDBDecimalValue(reportData.Compute("SUM(MemberWinLost)", "betgroup<>3"));
                comMemTotal = GetDBDecimalValue(reportData.Compute("SUM(MemberComm)", "betgroup<>3"));
                totalMemTotal = GetDBDecimalValue(reportData.Compute("SUM(MemberTotal)", "betgroup<>3"));
                winAgtTotal = GetDBDecimalValue(reportData.Compute("SUM(AgentWinLost)", "betgroup<>3"));
                comAgtTotal = GetDBDecimalValue(reportData.Compute("SUM(AgentComm)", "betgroup<>3"));
                totalAgtTotal = GetDBDecimalValue(reportData.Compute("SUM(AgentTotal)", "betgroup<>3"));
                comCompanyTotal = GetDBDecimalValue(reportData.Compute("SUM(CompanyComm)", "betgroup<>3"));
                companyTotal = GetDBDecimalValue(reportData.Compute("SUM(CompanyTotal)", "betgroup<>3"));
                if (CurrentUserSession.IsSuper)
                {
                    winSupTotal = GetDBDecimalValue(reportData.Compute("SUM(SAgWinLost)", "betgroup<>3"));
                    comSupTotal = GetDBDecimalValue(reportData.Compute("SUM(SAgComm)", "betgroup<>3"));
                    totalSupTotal = GetDBDecimalValue(reportData.Compute("SUM(SAgTotal)", "betgroup<>3"));
                }
                if (CurrentUserSession.IsSuper || CurrentUserSession.IsMaster)
                {
                    winMasTotal = GetDBDecimalValue(reportData.Compute("SUM(MasterWinLost)", "betgroup<>3"));
                    comMasTotal = GetDBDecimalValue(reportData.Compute("SUM(MasterComm)", "betgroup<>3"));
                    totalMasTotal = GetDBDecimalValue(reportData.Compute("SUM(MasterTotal)", "betgroup<>3"));
                }
                #endregion

                #region TotalWithNoParlay
                excelBlock.LimitFooter.TurnOverTotal = Formater.DecFormat(turnOverTotal, 2);
                excelBlock.LimitFooter.MaxCommTotal = Formater.DecFormat(maxComTotal, 2);
                excelBlock.LimitFooter.WinlossMemTotal = Formater.DecFormat(winMemTotal, 2);
                excelBlock.LimitFooter.CommMemTotal = Formater.DecFormat(comMemTotal, 2);
                excelBlock.LimitFooter.TotalMemTotal = Formater.DecFormat(totalMemTotal, 2);
                excelBlock.LimitFooter.WinlossAgtTotal = Formater.DecFormat(winAgtTotal, 2);
                excelBlock.LimitFooter.CommAgtTotal = Formater.DecFormat(comAgtTotal, 2);
                excelBlock.LimitFooter.TotalAgtTotal = Formater.DecFormat(totalAgtTotal, 2);
                excelBlock.LimitFooter.Master.WinlossMasTotal = Formater.DecFormat(winMasTotal, 2);
                excelBlock.LimitFooter.Master.CommMasTotal = Formater.DecFormat(comMasTotal, 2);
                excelBlock.LimitFooter.Master.TotalMasTotal = Formater.DecFormat(totalMasTotal, 2);
                excelBlock.LimitFooter.Super.WinlossSupTotal = Formater.DecFormat(winSupTotal, 2);
                excelBlock.LimitFooter.Super.CommSupTotal = Formater.DecFormat(comSupTotal, 2);
                excelBlock.LimitFooter.Super.TotalSupTotal = Formater.DecFormat(totalSupTotal, 2);
                excelBlock.LimitFooter.CommComTotal = Formater.DecFormat(comCompanyTotal, 2);
                excelBlock.LimitFooter.TotalComTotal = Formater.DecFormat(companyTotal, 2);
                #endregion

                // Render parlay and total all row
                RenderParlayAndTotalAll(drParlay, drCombineMixParlay, turnOverTotal, maxComTotal, winMemTotal, comMemTotal, totalMemTotal, winAgtTotal, comAgtTotal, totalAgtTotal, comCompanyTotal, companyTotal, winMasTotal, comMasTotal, totalMasTotal, winSupTotal, comSupTotal, totalSupTotal, excelBlock);
            }
            else
            {
                excelBlock.LimitFooter.Hide();
            }
            excelBlock.NumCols = excelBlock.NoInfo.NoInfoRow.NumCols = ReportNumColumn.ToString();

        }

        /// <summary>
        /// Render parlay and total all row
        /// </summary>
        /// <param name="drParlay"></param>
        /// <param name="turnOverTotal"></param>
        /// <param name="maxComTotal"></param>
        /// <param name="winMemTotal"></param>
        /// <param name="comMemTotal"></param>
        /// <param name="totalMemTotal"></param>
        /// <param name="winAgtTotal"></param>
        /// <param name="comAgtTotal"></param>
        /// <param name="totalAgtTotal"></param>
        /// <param name="comCompanyTotal"></param>
        /// <param name="companyTotal"></param>
        /// <param name="winMasTotal"></param>
        /// <param name="comMasTotal"></param>
        /// <param name="totalMasTotal"></param>
        /// <param name="winSupTotal"></param>
        /// <param name="comSupTotal"></param>
        /// <param name="totalSupTotal"></param>
        /// <param name="excelBlock"></param>
        private void RenderParlayAndTotalAll(DataRow drParlay, DataRow drCombineMixParlay, decimal turnOverTotal, decimal maxComTotal, decimal winMemTotal, decimal comMemTotal, decimal totalMemTotal, decimal winAgtTotal, decimal comAgtTotal, decimal totalAgtTotal, decimal comCompanyTotal, decimal companyTotal, decimal winMasTotal, decimal comMasTotal, decimal totalMasTotal, decimal winSupTotal, decimal comSupTotal, decimal totalSupTotal, MatchWinlossDetail_ExcelContents_Block excelBlock)
        {

            if (drParlay != null || drCombineMixParlay != null)
            {
                string RowParlay = string.Empty, RowSP = string.Empty;

                #region get value Parlay Total
                decimal turnOverParlay = 0, maxComParlay = 0, winMemParlay = 0, comMemParlay = 0, totalMemParlay = 0, winAgtParlay = 0, comAgtParlay = 0, totalAgtParlay = 0, comCompanyParlay = 0, companyParlay = 0, winSupParlay = 0, comSupParlay = 0, totalParlay = 0, winMasParlay = 0, comMasParlay = 0, totalMasParlay = 0;
                if (drParlay != null)
                {
                    turnOverParlay = Convert.ToDecimal(drParlay["turnover"]);
                    maxComParlay = Convert.ToDecimal(drParlay["GrossComm"]);
                    winMemParlay = Convert.ToDecimal(drParlay["MemberWinLost"]);
                    comMemParlay = Convert.ToDecimal(drParlay["MemberComm"]);
                    totalMemParlay = Convert.ToDecimal(drParlay["MemberTotal"]);
                    winAgtParlay = Convert.ToDecimal(drParlay["AgentWinLost"]);
                    comAgtParlay = Convert.ToDecimal(drParlay["AgentComm"]);
                    totalAgtParlay = Convert.ToDecimal(drParlay["AgentTotal"]);
                    comCompanyParlay = Convert.ToDecimal(drParlay["CompanyComm"]);
                    companyParlay = Convert.ToDecimal(drParlay["CompanyTotal"]);
                    if (CurrentUserSession.IsSuper)
                    {
                        winSupParlay = Convert.ToDecimal(drParlay["SAgWinLost"]);
                        comSupParlay = Convert.ToDecimal(drParlay["SAgComm"]);
                        totalParlay = Convert.ToDecimal(drParlay["SAgTotal"]);
                    }
                    if (CurrentUserSession.IsSuper || CurrentUserSession.IsMaster)
                    {
                        winMasParlay = Convert.ToDecimal(drParlay["MasterWinLost"]);
                        comMasParlay = Convert.ToDecimal(drParlay["MasterComm"]);
                        totalMasParlay = Convert.ToDecimal(drParlay["MasterTotal"]);
                    }
                    RowParlay = RenderParlay(Convert.ToInt16(drParlay["bettype"]), turnOverParlay, maxComParlay, winMemParlay, comMemParlay, totalMemParlay, winAgtParlay, comAgtParlay, totalAgtParlay, comCompanyParlay, companyParlay, winSupParlay, comSupParlay, totalParlay, winMasParlay, comMasParlay, totalMasParlay);
                }
                #endregion

                #region get value Combine Mix Parlay Total
                decimal turnOverSP = 0, maxComSP = 0, winMemSP = 0, comMemSP = 0, totalMemSP = 0, winAgtSP = 0, comAgtSP = 0, totalAgtSP = 0, comCompanySP = 0, companySP = 0, winSupSP = 0, comSupSP = 0, totalSP = 0, winMasSP = 0, comMasSP = 0, totalMasSP = 0;
                if (drCombineMixParlay != null)
                {
                    turnOverSP = Convert.ToDecimal(drCombineMixParlay["turnover"]);
                    maxComSP = Convert.ToDecimal(drCombineMixParlay["GrossComm"]);
                    winMemSP = Convert.ToDecimal(drCombineMixParlay["MemberWinLost"]);
                    comMemSP = Convert.ToDecimal(drCombineMixParlay["MemberComm"]);
                    totalMemSP = Convert.ToDecimal(drCombineMixParlay["MemberTotal"]);
                    winAgtSP = Convert.ToDecimal(drCombineMixParlay["AgentWinLost"]);
                    comAgtSP = Convert.ToDecimal(drCombineMixParlay["AgentComm"]);
                    totalAgtSP = Convert.ToDecimal(drCombineMixParlay["AgentTotal"]);
                    comCompanySP = Convert.ToDecimal(drCombineMixParlay["CompanyComm"]);
                    companySP = Convert.ToDecimal(drCombineMixParlay["CompanyTotal"]);
                    if (CurrentUserSession.IsSuper)
                    {
                        winSupSP = Convert.ToDecimal(drCombineMixParlay["SAgWinLost"]);
                        comSupSP = Convert.ToDecimal(drCombineMixParlay["SAgComm"]);
                        totalSP = Convert.ToDecimal(drCombineMixParlay["SAgTotal"]);
                    }
                    if (CurrentUserSession.IsSuper || CurrentUserSession.IsMaster)
                    {
                        winMasSP = Convert.ToDecimal(drCombineMixParlay["MasterWinLost"]);
                        comMasSP = Convert.ToDecimal(drCombineMixParlay["MasterComm"]);
                        totalMasSP = Convert.ToDecimal(drCombineMixParlay["MasterTotal"]);
                    }
                    RowSP = RenderParlay(Convert.ToInt16(drCombineMixParlay["bettype"]), turnOverSP, maxComSP, winMemSP, comMemSP, totalMemSP, winAgtSP, comAgtSP, totalAgtSP, comCompanySP, companySP, winSupSP, comSupSP, totalSP, winMasSP, comMasSP, totalMasSP);
                }
                #endregion

                excelBlock.LimitFooter.Parlay.SetValue(RowParlay + RowSP);

                #region Assign TotalAll
                excelBlock.LimitFooter.TotalAll.TurnoverTotalAll = Formater.DecFormat(turnOverTotal + turnOverParlay + turnOverSP, 2);
                excelBlock.LimitFooter.TotalAll.MaxCommTotalAll = Formater.DecFormat(maxComTotal + maxComParlay + maxComSP, 2);
                excelBlock.LimitFooter.TotalAll.WinlossTotalAll = Formater.DecFormat(winMemTotal + winMemParlay + winMemSP, 2);
                excelBlock.LimitFooter.TotalAll.CommTotalAll = Formater.DecFormat(comMemTotal + comMemParlay + comMemSP, 2);
                excelBlock.LimitFooter.TotalAll.TotalTotalAll = Formater.DecFormat(totalMemTotal + totalMemParlay + totalMemSP, 2);
                excelBlock.LimitFooter.TotalAll.WinlossAgtTotalAll = Formater.DecFormat(winAgtTotal + winAgtParlay + winAgtSP, 2);
                excelBlock.LimitFooter.TotalAll.CommAgtTotalAll = Formater.DecFormat(comAgtTotal + comAgtParlay + comAgtSP, 2);
                excelBlock.LimitFooter.TotalAll.TotalAgtTotalAll = Formater.DecFormat(totalAgtTotal + totalAgtParlay + totalAgtSP, 2);
                excelBlock.LimitFooter.TotalAll.CommComTotalAll = Formater.DecFormat(comCompanyTotal + comCompanyParlay + comCompanySP, 2);
                excelBlock.LimitFooter.TotalAll.TotalComTotalAll = Formater.DecFormat(companyTotal + companyParlay + companySP, 2);
                excelBlock.LimitFooter.TotalAll.Master.WinlossMasTotalAll = Formater.DecFormat(winMasTotal + winMasParlay + winMasSP, 2);
                excelBlock.LimitFooter.TotalAll.Master.CommMasTotalAll = Formater.DecFormat(comMasTotal + comMasParlay + comMasSP, 2);
                excelBlock.LimitFooter.TotalAll.Master.TotalMasTotalAll = Formater.DecFormat(totalMasTotal + totalMasParlay + totalMasSP, 2);
                excelBlock.LimitFooter.TotalAll.Super.WinlossSupTotalAll = Formater.DecFormat(winSupTotal + winSupParlay + winSupSP, 2);
                excelBlock.LimitFooter.TotalAll.Super.CommSupTotalAll = Formater.DecFormat(comSupTotal + comSupParlay + comSupSP, 2);
                excelBlock.LimitFooter.TotalAll.Super.TotalSupTotalAll = Formater.DecFormat(totalSupTotal + totalParlay + totalSP, 2);
                #endregion
            }
            else
            {
                excelBlock.LimitFooter.Parlay.Hide();
                excelBlock.LimitFooter.TotalAll.Hide();
            }
        }

        private string RenderParlay(int betType, decimal turnOverParlay, decimal maxComParlay, decimal winMemParlay, decimal comMemParlay, decimal totalMemParlay, decimal winAgtParlay, decimal comAgtParlay, decimal totalAgtParlay, decimal comCompanyParlay, decimal companyParlay, decimal winSupParlay, decimal comSupParlay, decimal totalParlay, decimal winMasParlay, decimal comMasParlay, decimal totalMasParlay)
        {
            MatchWinlossDetail_ExcelContents_LimitFooter_Parlay_Block tpl = new MatchWinlossDetail_ExcelContents_LimitFooter_Parlay_Block();

            if (CurrentUserSession.IsAgent)
            {
                tpl.Master.Hide();
            }
            if (CurrentUserSession.IsAgent || CurrentUserSession.IsMaster)
            {
                tpl.Super.Hide();
            }

            switch (betType)
            {
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Mix_Parlay: // 9
                    tpl.BetTypeName = Resources.Lang.lblMixParlay;
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Combine_Mix_Parlay: // 29
                    tpl.BetTypeName = Resources.Lang.lblSystemParlay;
                    break;
            }

            tpl.ColorOver = Fanex.Age.UI.Common.RowBgOver;

            tpl.MPDateFrom = _dateFrom.ToShortDateString();
            tpl.MPDateTo = _dateTo.ToShortDateString();
            tpl.MPBetType = betType.ToString();
            tpl.MPType = Fanex.Age.UI.Common.BetListByMixParlayOfMatch;

            tpl.TurnoverParlay = Formater.DecFormat(turnOverParlay, 2);
            tpl.MaxcommParlay = Formater.DecFormat(maxComParlay, 2);
            tpl.WinlossMemParlay = Formater.DecFormat(winMemParlay, 2);
            tpl.CommMemParlay = Formater.DecFormat(comMemParlay, 2);
            tpl.TotalMemParlay = Formater.DecFormat(totalMemParlay, 2);
            tpl.WinlossAgtParlay = Formater.DecFormat(winAgtParlay, 2);
            tpl.CommAgtParlay = Formater.DecFormat(comAgtParlay, 2);
            tpl.TotalAgtParlay = Formater.DecFormat(totalAgtParlay, 2);
            tpl.CommComParlay = Formater.DecFormat(comCompanyParlay, 2);
            tpl.TotalComParlay = Formater.DecFormat(companyParlay, 2);
            tpl.Master.WinlossMasParlay = Formater.DecFormat(winMasParlay, 2);
            tpl.Master.CommMasParlay = Formater.DecFormat(comMasParlay, 2);
            tpl.Master.TotalMasParlay = Formater.DecFormat(totalMasParlay, 2);
            tpl.Super.WinlossSupParlay = Formater.DecFormat(winSupParlay, 2);
            tpl.Super.CommSupParlay = Formater.DecFormat(comSupParlay, 2);
            tpl.Super.TotalSupParlay = Formater.DecFormat(totalParlay, 2);
            return tpl;
        }

        /// <summary>
        /// Render bet type row
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="currentMatchId"></param>
        /// <param name="betTypeName"></param>
        /// <param name="betTypeBlock"></param>
        private void RenderBetTypeRow(DataRow dr, int currentMatchId, String betTypeName, int rowIndex, MatchWinlossDetail_ExcelContents_Content_BetTypeRow_Block betTypeBlock)
        {
            #region betTypeBlock
            HideRedundantColumn(betTypeBlock);
            betTypeBlock.ColorClass = rowIndex % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;
            betTypeBlock.LeagueId = dr["leagueid"].ToString();
            betTypeBlock.MatchId = currentMatchId.ToString();
            betTypeBlock.BetVewType = Fanex.Age.UI.Common.WinLossByBetTypeMatch;
            betTypeBlock.Race = Convert.IsDBNull(dr["race"]) || String.IsNullOrEmpty(dr["race"].ToString()) ? "0" : dr["race"].ToString();

            int betType = Convert.ToInt32(dr["bettype"]);
            betTypeBlock.BetTypeId = betType.ToString();
            string strTypeNameNG = string.Empty;
            switch (betType)
            {
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.N_Over_Under: //85
                    strTypeNameNG = Resources.Lang.next + " ";
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.N_Odd_Even: // 86
                    if ((int)dr["liveindicator"] == 1)
                        strTypeNameNG = Resources.Lang.next + " ";
                    else
                        strTypeNameNG = Resources.Lang.ft + ". ";
                    break;
            }
            betTypeBlock.BetTypeName = strTypeNameNG + betTypeName;

            betTypeBlock.islive = dr["liveindicator"].ToString();
            betTypeBlock.TurnOver = Formater.DecFormat(dr["turnover"], 2);
            betTypeBlock.MaxComm = Formater.DecFormat(dr["GrossComm"], 2);
            betTypeBlock.WinlossMem = Formater.DecFormat(dr["MemberWinLost"], 2);
            betTypeBlock.CommMem = Formater.DecFormat(dr["MemberComm"], 2);
            betTypeBlock.TotalMem = Formater.DecFormat(dr["MemberTotal"], 2);
            betTypeBlock.WinlossAg = Formater.DecFormat(dr["AgentWinLost"], 2);
            betTypeBlock.CommAg = Formater.DecFormat(dr["AgentComm"], 2);
            betTypeBlock.TotalAg = Formater.DecFormat(dr["AgentTotal"], 2);
            if (CurrentUserSession.IsSuper)
            {
                betTypeBlock.Super.WinlossSAg = Formater.DecFormat(dr["SAgWinLost"], 2);
                betTypeBlock.Super.CommSAg = Formater.DecFormat(dr["SAgComm"], 2);
                betTypeBlock.Super.TotalSAg = Formater.DecFormat(dr["SAgTotal"], 2);
            }
            if (CurrentUserSession.IsSuper || CurrentUserSession.IsMaster)
            {
                betTypeBlock.Master.WinlossMAg = Formater.DecFormat(dr["MasterWinLost"], 2);
                betTypeBlock.Master.CommMAg = Formater.DecFormat(dr["MasterComm"], 2);
                betTypeBlock.Master.TotalMAg = Formater.DecFormat(dr["MasterTotal"], 2);
            }
            betTypeBlock.CommComp = Formater.DecFormat(dr["CompanyComm"], 2);
            betTypeBlock.TotalComp = Formater.DecFormat(dr["CompanyTotal"], 2);
            #endregion
        }
        /// <summary>
        /// Render Total Bet Type Row
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="currentMatchId"></param>
        /// <param name="betTypeBlock"></param>
        private void RenderTotalBetType(string strFilter, MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block totalBetTypeBlock, MatchWinlossDetail_ExcelContents_Block excelBlock)
        {
            #region betTypeBlock
            HideRedundantColumnTotalRow(totalBetTypeBlock);
            totalBetTypeBlock.ColorClass = EVEN_ROW_CLASS;
            totalBetTypeBlock.TurnOver = Formater.DecFormat((decimal)reportData.Compute("SUM(turnover)", strFilter), 2);
            totalBetTypeBlock.MaxComm = Formater.DecFormat((decimal)reportData.Compute("SUM(GrossComm)", strFilter), 2);
            totalBetTypeBlock.WinlossMem = Formater.DecFormat((decimal)reportData.Compute("SUM(MemberWinLost)", strFilter), 2);
            totalBetTypeBlock.CommMem = Formater.DecFormat((decimal)reportData.Compute("SUM(MemberComm)", strFilter), 2);
            totalBetTypeBlock.TotalMem = Formater.DecFormat((decimal)reportData.Compute("SUM(MemberTotal)", strFilter), 2);
            totalBetTypeBlock.WinlossAg = Formater.DecFormat((decimal)reportData.Compute("SUM(AgentWinLost)", strFilter), 2);
            totalBetTypeBlock.CommAg = Formater.DecFormat((decimal)reportData.Compute("SUM(AgentComm)", strFilter), 2);
            totalBetTypeBlock.TotalAg = Formater.DecFormat((decimal)reportData.Compute("SUM(AgentTotal)", strFilter), 2);
            if (CurrentUserSession.IsSuper)
            {
                totalBetTypeBlock.Super.WinlossSAg = Formater.DecFormat((decimal)reportData.Compute("SUM(SAgWinLost)", strFilter), 2);
                totalBetTypeBlock.Super.CommSAg = Formater.DecFormat((decimal)reportData.Compute("SUM(SAgComm)", strFilter), 2);
                totalBetTypeBlock.Super.TotalSAg = Formater.DecFormat((decimal)reportData.Compute("SUM(SAgTotal)", strFilter), 2);
            }
            if (CurrentUserSession.IsSuper || CurrentUserSession.IsMaster)
            {
                totalBetTypeBlock.Master.WinlossMAg = Formater.DecFormat((decimal)reportData.Compute("SUM(MasterWinLost)", strFilter), 2);
                totalBetTypeBlock.Master.CommMAg = Formater.DecFormat((decimal)reportData.Compute("SUM(MasterComm)", strFilter), 2);
                totalBetTypeBlock.Master.TotalMAg = Formater.DecFormat((decimal)reportData.Compute("SUM(MasterTotal)", strFilter), 2);
            }
            totalBetTypeBlock.CommComp = Formater.DecFormat((decimal)reportData.Compute("SUM(CompanyComm)", strFilter), 2);
            totalBetTypeBlock.TotalComp = Formater.DecFormat((decimal)reportData.Compute("SUM(CompanyTotal)", strFilter), 2);

            excelBlock.Content.Append(totalBetTypeBlock);
            #endregion
        }
        /// <summary>
        /// Render match row, show match title
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="betType"></param>
        /// <param name="currentTitle"></param>
        /// <param name="currentMatchId"></param>
        /// <param name="rowCount"></param>
        /// <param name="leagueName"></param>
        /// <param name="betTypeName"></param>
        /// <param name="matchBlock"></param>
        /// <param name="excelBlock"></param>
        private void RenderMatchRow(DataRow dr, int betType, ref String currentTitle, ref int currentMatchId, int RowIndex, int rowCount, String leagueName, String betTypeName, MatchWinlossDetail_ExcelContents_Content_MatchContent_Block matchBlock, MatchWinlossDetail_ExcelContents_Block excelBlock)
        {
            bool isInsertTotal = false;
            int lastMatchId = currentMatchId;
            int matchId = (int)dr["matchid"];

            if (betType == (int)Sunplus.Agent.Model.BL.Definitions.BetType.Outright) // 10
            {
                matchBlock.MatchName.Hide();
                matchBlock.League = betTypeName;
                if (rowCount == 2) matchBlock.Hide(); // Outright header invisible when have 2 records
            }
            else if ((betType > 30 && betType < 34) || (betType > 40 && betType < 44)) // Racing
            {
                String title = Resources.Lang.hr + " - " + leagueName + "-" + dr["race"].ToString();
                if (currentTitle != title)
                {
                    isInsertTotal = true;
                    currentTitle = title;
                    //currentMatchId = -1;
                    matchBlock.MatchName.MatchName = Resources.Lang.lblRace + dr["race"].ToString();
                    matchBlock.League = Resources.Lang.hr + " - " + leagueName;
                }
            }
            else if ((betType > 50 && betType < 57) || (betType > 60 && betType < 67)) // Finance
            {
                string leagueNameTitle = Resources.Lang.Financial;
                leagueName = _exportToExcel ? leagueName : Formater.TruncateWithTip(leagueName, 23).Trim();
                if (!String.IsNullOrEmpty(leagueName)) leagueNameTitle += "-" + leagueName;

                //string indexName = CacheRef.GetTeamName(r["homeid"]);
                string indexName = CacheRef.GetFncIndexName(dr["homeid"]);
                String title = leagueName + indexName;

                if (currentTitle != title)
                {
                    isInsertTotal = true;
                    currentTitle = title;
                    currentMatchId = matchId;
                    matchBlock.MatchName.MatchName = indexName;
                    matchBlock.League = leagueNameTitle;
                }
            }
            else if (betType > 80 && betType < 121) // Number Game
            {
                string leagueNameTitle = Resources.Lang.numbergame;
                leagueName = _exportToExcel ? leagueName : Formater.TruncateWithTip(leagueName, 23).Trim();
                if (!String.IsNullOrEmpty(leagueName)) leagueNameTitle += "-" + leagueName;

                string gameId = Resources.Lang.numbergameno + " " + dr["matchcode"].ToString();
                String title = leagueName + gameId;

                if (currentTitle != title)
                {
                    isInsertTotal = true;
                    currentTitle = title;
                    currentMatchId = matchId;
                    matchBlock.MatchName.MatchName = gameId;
                    matchBlock.League = leagueNameTitle;
                }
            }
            else if (betType > 1100 && betType < 1104) // For Live Casino
            {
                string leagueNameTitle = Resources.Lang.LiveCasino;
                String title = betTypeName + " " + dr["showtime"].ToString();

                if (currentTitle != title)
                {
                    isInsertTotal = true;
                    currentTitle = title;
                    currentMatchId = matchId;
                    matchBlock.MatchName.MatchName = title;
                    matchBlock.League = leagueNameTitle;
                }
            }
            else
            {
                if (currentMatchId == 0 || currentMatchId != matchId) // new match
                {
                    isInsertTotal = true;
                    currentMatchId = matchId;
                    String homeName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["homeid"]) + ((bool)dr["isNeutral"] ? Resources.Lang.IsNeutral : string.Empty);
                    String awayName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["awayid"]);
                    matchBlock.MatchName.MatchName = homeName + "-vs-" + awayName;

                    leagueName = _exportToExcel ? leagueName : Formater.TruncateWithTip(leagueName, 33);
                    currentTitle = matchBlock.ToString();
                    matchBlock.League = leagueName;
                }
            }

            #region Title & Sub Total
            if (isInsertTotal)
            {
                if (filterLeagueId != "0")
                {
                    if (filterRace != "0")
                    {
                        MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block totalBetTypeBlock = new MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block();
                        RenderTotalBetType("leagueid=" + filterLeagueId + " and race='" + filterRace + "'", totalBetTypeBlock, excelBlock);
                    }
                    else if (lastMatchId != 0)
                    {
                        MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block totalBetTypeBlock = new MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block();
                        RenderTotalBetType("matchid=" + lastMatchId, totalBetTypeBlock, excelBlock);
                    }
                }
                excelBlock.Content.Append(matchBlock);
            }
            //Cal Total for Last Row
            if ((int)dr["leagueid"] == 0 && filterLeagueId != "0")
            {
                if (filterRace != "0")
                {
                    MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block totalBetTypeBlock = new MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block();
                    RenderTotalBetType("leagueid=" + filterLeagueId + " and race='" + filterRace + "'", totalBetTypeBlock, excelBlock);
                }
                else if (lastMatchId != 0)
                {
                    MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block totalBetTypeBlock = new MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block();
                    RenderTotalBetType("matchid=" + lastMatchId, totalBetTypeBlock, excelBlock);
                }
            }
            filterLeagueId = dr["leagueid"].ToString();
            filterRace = string.IsNullOrEmpty(dr["race"].ToString()) ? "0" : dr["race"].ToString();
            #endregion Title & Sub Total
        }

        #endregion

        #region Utility



        private bool AllowRenderBet(String sportTypeName)
        {
            if (string.Compare(sportTypeName, Resources.Lang.hr) == 0)
            {
                if (!Sunplus.Agent.Model.Entities.Permission.UserPermission.IsShowRacing)
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// Get report column count
        /// </summary>
        private int ReportNumColumn
        {
            get
            {
                if (CurrentUserSession.IsAgent) return 11;
                else if (CurrentUserSession.IsMaster) return 14;
                else return 17;
            }
        }

        /// <summary>
        /// Hide redundant coliumn
        /// If logged user is master, does not show super column 
        /// If looged user is agent, does not show master column and super column
        /// </summary>
        /// <param name="excelBlock"></param>

        private void HideRedundantColumn(MatchWinlossDetail_ExcelContents_Block excelBlock)
        {
            excelBlock.Master1.Show();
            excelBlock.Master2.Show();
            excelBlock.Super1.Show();
            excelBlock.Super2.Show();
            excelBlock.LimitFooter.Master.Show();
            excelBlock.LimitFooter.Super.Show();
            excelBlock.LimitFooter.Parlay.Master.Show();
            excelBlock.LimitFooter.Parlay.Super.Show();
            excelBlock.LimitFooter.TotalAll.Master.Show();
            excelBlock.LimitFooter.TotalAll.Super.Show();
            if (CurrentUserSession.IsAgent)
            {
                excelBlock.Master1.Hide();
                excelBlock.Master2.Hide();
                excelBlock.LimitFooter.Master.Hide();
                excelBlock.LimitFooter.Parlay.Master.Hide();
                excelBlock.LimitFooter.TotalAll.Master.Hide();
            }
            if (CurrentUserSession.IsAgent || CurrentUserSession.IsMaster)
            {
                excelBlock.Super1.Hide();
                excelBlock.Super2.Hide();
                excelBlock.LimitFooter.Super.Hide();
                excelBlock.LimitFooter.Parlay.Super.Hide();
                excelBlock.LimitFooter.TotalAll.Super.Hide();
            }
        }
        /// <summary>
        /// Hide redundant coliumn
        /// If logged user is master, does not show super column 
        /// If looged user is agent, does not show master column and super column
        /// </summary>
        /// <param name="contentRow"></param>
        private void HideRedundantColumn(MatchWinlossDetail_ExcelContents_Content_BetTypeRow_Block rowBlock)
        {
            rowBlock.Master.Show();
            rowBlock.Super.Show();
            if (CurrentUserSession.IsAgent)
            {
                rowBlock.Master.Hide();
            }
            if (CurrentUserSession.IsAgent || CurrentUserSession.IsMaster)
            {
                rowBlock.Super.Hide();
            }
        }

        /// <summary>
        /// Hide redundant coliumn for TOTAL row
        /// If logged user is master, does not show super column for TOTAL row
        /// If looged user is agent, does not show master column and super column for TOTAL row
        /// </summary>
        /// <param name="contentRow"></param>
        private void HideRedundantColumnTotalRow(MatchWinlossDetail_ExcelContents_Content_TotalBetTypeRow_Block rowBlock)
        {
            rowBlock.Master.Show();
            rowBlock.Super.Show();
            if (CurrentUserSession.IsAgent)
            {
                rowBlock.Master.Hide();
            }
            if (CurrentUserSession.IsAgent || CurrentUserSession.IsMaster)
            {
                rowBlock.Super.Hide();
            }
        }
        #endregion
    }
}