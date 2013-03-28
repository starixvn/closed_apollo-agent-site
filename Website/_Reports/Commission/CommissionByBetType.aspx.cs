using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Fanex.Age.UI.Components.ShowHideColumn;
using Resources;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.Utilities;

/*
 * Created 20100529@Jacob - Render CommissionByBetType page
 * Revision:
 * 20111010@Andy - Render P2P info and context menu for show/hide columns
 *
 */

namespace Fanex.Age.UI.Reports.Commission
{
    public partial class CommissionByBetType_Page : Reports_Page, IContextMenuColumns
    {
        private List<string> texts = new List<string>();
        private List<string> cols = new List<string>();

        public CommissionByBetType_Page()
        {
            MIN_REPORT_DATE = Sunplus.Agent.Model.BL.Reports.ReportsBO.BalanceUpDate;
        }

        #region Override

        /// <summary>
        ///
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            Dictionary<String, object> viewData = View as Dictionary<String, object>;
            CommissionByBetType_Template tpl = new CommissionByBetType_Template();

            if (_exportToExcel)
            {
                DataTable reportData = (DataTable)((Dictionary<String, object>)View)["ReportData"];
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.Commission.CommissionByBetTypeReportBuilder(reportData, IsBold, _dateFrom, _dateTo, resMan, _roleId, _custId);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "CommissionByBetType.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = "";
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "CommisionByBetType.xls");
            }
            else
            {
                // Render Report Header and history message
                RenderDateRange(tpl.ReportSearchHeader);
                RenderReportHeader(tpl);

                RenderReportContent(tpl.ExcelContents);
                RenderHiddenField(tpl);

                //Show/hide columns context menu
                this.RegisterCommonScriptObjects("tbl-rpt", new string[] { "rowTitle", "rptHeader", "rptHeader2" }, "mnuColumn", this);
                tpl.ExcelContents.TableClass = this.RenderTableClasses(cols.ToArray());
                tpl.contextMenu = this.RenderContextMenu(texts.ToArray(), cols.ToArray());

                writer.Write(tpl);
            }

            RegisterSortingJavascriptObject();
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            base.ProcessRequest(ref p);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            Dictionary<String, object> viewData = new Dictionary<string, object>();
            // Report data
            DataTable reportData = _ReportBO.GetCommisionByBetType(_custId, _roleId, _dateFrom, _dateTo);
            reportData = SortData(reportData);
            viewData.Add("ReportData", reportData);
            return viewData;
        }

        #endregion Override

        #region Render Utility

        /// <summary>
        /// Render Report header
        /// </summary>
        /// <param name="headerTemplate"></param>
        private void RenderReportHeader(CommissionByBetType_Template template)
        {
            template.ExcelContents.BoldRow = this.BoldRowClass;
        }

        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeaderAndFooter(CommissionByBetType_ExcelContents_Block excelBlock)
        {
            var hasGame = false;
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
            excelBlock.NumColumn = ReportColumnCount.ToString();

            if (_roleId == (int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Super)) excelBlock.CurrentUserType = Resources.Lang.lblCommPTSuper;
            else if (_roleId == (int)(Sunplus.Agent.Model.BL.Definitions.UserTypes.Master)) excelBlock.CurrentUserType = Resources.Lang.lblCommPTMaster;
            else excelBlock.CurrentUserType = Resources.Lang.lblCommPTAgent;

            texts.Add(Lang.Commission1Details);
            texts.Add(Lang.Commission2Details);
            texts.Add(Lang.OneXTwoCommission);
            texts.Add(Lang.OtherBettypes);

            cols.Add("comm1");
            cols.Add("comm2");
            cols.Add("oneXtwo");
            cols.Add("others");

            if (UserPermission.IsShowCasino)
            {
                hasGame = true;
            }
            else
            {
                excelBlock.Casino.Hide();
                excelBlock.TotalContent.Casino.Hide();
            }

            if (UserPermission.IsShowLiveCasino)
            {
                hasGame = true;
            }
            else
            {
                excelBlock.LiveCasino.Hide();
                excelBlock.TotalContent.LiveCasino.Hide();
            }

            if (UserPermission.IsShowNumberGame)
            {
                hasGame = true;
            }
            else
            {
                excelBlock.NumberGame.Hide();
                excelBlock.TotalContent.NumberGame.Hide();
            }

            if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P)
            {
                hasGame = true;
            }
            else
            {
                excelBlock.P2P.Hide();
                excelBlock.TotalContent.P2P.Hide();
            }

            if (hasGame)
            {
                texts.Add(Resources.Lang.GamesDetails);
                cols.Add("_games");
            }
        }

        /// <summary>
        /// Rnder report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(CommissionByBetType_ExcelContents_Block excelBlock)
        {
            Dictionary<String, object> viewData = View as Dictionary<String, object>;
            DataTable reportData = viewData["ReportData"] as DataTable;

            RenderReportContentHeaderAndFooter(excelBlock);

            int numRow = reportData.Rows.Count;
            excelBlock.Content.Clear();
            if (numRow > 0)
            {
                CommissionByBetType_ExcelContents_Content_Block contentRow = new CommissionByBetType_ExcelContents_Content_Block();
                if (!Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P) contentRow.P2P.Hide();
                if (!UserPermission.IsShowCasino) contentRow.Casino.Hide();
                if (!UserPermission.IsShowLiveCasino) contentRow.LiveCasino.Hide();
                if (!UserPermission.IsShowNumberGame) contentRow.NumberGame.Hide();
               

                var sumGame = 0m;

                for (int i = 0; i < numRow; i++)
                {
                    var sum1RowGame = 0m;

                    DataRow dr = reportData.Rows[i];

                    RenderUserNameField(dr, contentRow);

                    // Assign Value

                    #region Assign value

                    contentRow.ColorClass = (i % 2) == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;

                    contentRow.Handicap = Formater.DecFormat(dr["hdp_comm"], 2);
                    contentRow.OverUnder = Formater.DecFormat(dr["ou_comm"], 2);
                    contentRow.FirstHalfHandicap = Formater.DecFormat(dr["hdp1st_comm"], 2);
                    contentRow.FirstHalfOverUnder = Formater.DecFormat(dr["ou1st_comm"], 2);
                    contentRow.OddEven = Formater.DecFormat(dr["oe_comm"], 2);
                    contentRow.CSheet = Formater.DecFormat(dr["csh_comm"], 2);
                    contentRow.DChance = Formater.DecFormat(dr["dc_comm"], 2);
                    contentRow.HNB = Formater.DecFormat(dr["hnb_comm"], 2);
                    contentRow.ANB = Formater.DecFormat(dr["anb_comm"], 2);
                    contentRow.DND = Formater.DecFormat(dr["dnd_comm"], 2);
                    contentRow.DNB = Formater.DecFormat(dr["dnb_comm"], 2);
                    contentRow.BON = Formater.DecFormat(dr["bon_comm"], 2);
                    contentRow.TWinTNill = Formater.DecFormat(dr["wn_comm"], 2);
                    contentRow.ThirdWayHdp = Formater.DecFormat(dr["hdp3_comm"], 2);
                    //contentRow.OneXTwo = Formater.DecFormat(dr["x12_comm"], 2);
                    contentRow.CorrectScore = Formater.DecFormat(dr["cs_comm"], 2);
                    contentRow.TotalGoal = Formater.DecFormat(dr["tg_comm"], 2);
                    contentRow.MixParlay = Formater.DecFormat(dr["mp_comm"], 2);
                    contentRow.CombinationMixParlay = Formater.DecFormat(dr["sp_comm"], 2);
                    contentRow.Outright = Formater.DecFormat(dr["outr_comm"], 2);
                    contentRow.FGLG = Formater.DecFormat(dr["fglg_comm"], 2);
                    contentRow.HTFT = Formater.DecFormat(dr["htft_comm"], 2);
                    contentRow.MoneyLineFullTime = Formater.DecFormat(dr["moneyline_comm"], 2);
                    contentRow.MoneyLineHalfTime = Formater.DecFormat(dr["moneyline1st_comm"], 2);
                    contentRow.Casino.Casino = Formater.DecFormat(dr["Casino_comm"], 2);
                    contentRow.LiveCasino.LiveCasino = Formater.DecFormat(dr["LiveCasino_comm"], 2);
                    contentRow.NumberGame.NumberGame = Formater.DecFormat(dr["NumberGame_comm"], 2);
                    contentRow.P2P.P2P = Formater.DecFormat(dr["P2P_comm"], 2);

                    sum1RowGame = dr.Field<decimal>("Casino_comm") + dr.Field<decimal>("LiveCasino_comm") +
                        dr.Field<decimal>("NumberGame_comm") + dr.Field<decimal>("P2P_comm");

                    contentRow.Total1 = Formater.DecFormat(dr["TotalComm_1"], 2);
                    contentRow.Total2 = Formater.DecFormat(dr["TotalComm_2"], 2);
                    contentRow.Total3 = Formater.DecFormat(sum1RowGame, 2);
                    contentRow.F1X2 = Formater.DecFormat(dr["f_x12_comm"], 2);
                    contentRow._1X2 = Formater.DecFormat(dr["x12_comm"], 2);
                    contentRow.OtherBettypes = Formater.DecFormat(dr["others_comm"], 2);
                    contentRow.FCS = Formater.DecFormat(dr["f_cs_comm"], 2);

                    sumGame += sum1RowGame;

                    #endregion Assign value

                    excelBlock.Content.Append(contentRow);
                }

                #region Assign TotalValue

                excelBlock.TotalContent.Handicap = Formater.DecFormat(reportData.Compute("SUM(hdp_comm)", ""), 2);
                excelBlock.TotalContent.OverUnder = Formater.DecFormat(reportData.Compute("SUM(ou_comm)", ""), 2);
                excelBlock.TotalContent.FirstHalfHandicap = Formater.DecFormat(reportData.Compute("SUM(hdp1st_comm)", ""), 2);
                excelBlock.TotalContent.FirsthalfOverUnder = Formater.DecFormat(reportData.Compute("SUM(ou1st_comm)", ""), 2);
                excelBlock.TotalContent.OddEven = Formater.DecFormat(reportData.Compute("SUM(oe_comm)", ""), 2);
                excelBlock.TotalContent.CSheet = Formater.DecFormat(reportData.Compute("SUM(csh_comm)", ""), 2);
                excelBlock.TotalContent.DChance = Formater.DecFormat(reportData.Compute("SUM(dc_comm)", ""), 2);
                excelBlock.TotalContent.HNB = Formater.DecFormat(reportData.Compute("SUM(hnb_comm)", ""), 2);
                excelBlock.TotalContent.ANB = Formater.DecFormat(reportData.Compute("SUM(anb_comm)", ""), 2);
                excelBlock.TotalContent.DND = Formater.DecFormat(reportData.Compute("SUM(dnd_comm)", ""), 2);
                excelBlock.TotalContent.DNB = Formater.DecFormat(reportData.Compute("SUM(dnb_comm)", ""), 2);
                excelBlock.TotalContent.BON = Formater.DecFormat(reportData.Compute("SUM(bon_comm)", ""), 2);
                excelBlock.TotalContent.TWinTNill = Formater.DecFormat(reportData.Compute("SUM(wn_comm)", ""), 2);
                excelBlock.TotalContent.ThirdWayHdp = Formater.DecFormat(reportData.Compute("SUM(hdp3_comm)", ""), 2);
                excelBlock.TotalContent.Total1 = Formater.DecFormat(reportData.Compute("SUM(TotalComm_1)", ""), 2);

                //excelBlock.TotalContent.OneXTwo = Formater.DecFormat(reportData.Compute("SUM(x12_comm)", ""), 2);
                excelBlock.TotalContent.CorectScore = Formater.DecFormat(reportData.Compute("SUM(cs_comm)", ""), 2);
                excelBlock.TotalContent.TotalGoal = Formater.DecFormat(reportData.Compute("SUM(tg_comm)", ""), 2);
                excelBlock.TotalContent.MixParlay = Formater.DecFormat(reportData.Compute("SUM(mp_comm)", ""), 2);
                excelBlock.TotalContent.CombinationMixParlay = Formater.DecFormat(reportData.Compute("SUM(sp_comm)", ""), 2);
                excelBlock.TotalContent.Outright = Formater.DecFormat(reportData.Compute("SUM(outr_comm)", ""), 2);
                excelBlock.TotalContent.FGLG = Formater.DecFormat(reportData.Compute("SUM(fglg_comm)", ""), 2);
                excelBlock.TotalContent.HTFT = Formater.DecFormat(reportData.Compute("SUM(htft_comm)", ""), 2);
                excelBlock.TotalContent.MoneyLineFullTime = Formater.DecFormat(reportData.Compute("SUM(moneyline_comm)", ""), 2);
                excelBlock.TotalContent.MoneyLineHalfTime = Formater.DecFormat(reportData.Compute("SUM(moneyline1st_comm)", ""), 2);
                excelBlock.TotalContent.Total2 = Formater.DecFormat(reportData.Compute("SUM(TotalComm_2)", ""), 2);
                excelBlock.TotalContent.Casino.Casino = Formater.DecFormat(reportData.Compute("SUM(Casino_comm)", ""), 2);
                excelBlock.TotalContent.LiveCasino.LiveCasino = Formater.DecFormat(reportData.Compute("SUM(LiveCasino_comm)", ""), 2);
                excelBlock.TotalContent.NumberGame.NumberGame = Formater.DecFormat(reportData.Compute("SUM(NumberGame_comm)", ""), 2);
                excelBlock.TotalContent.P2P.P2P = Formater.DecFormat(reportData.Compute("SUM(P2P_comm)", ""), 2);
                excelBlock.TotalContent.Total3 = Formater.DecFormat(sumGame, 2);
                excelBlock.TotalContent.F1X2 = Formater.DecFormat(reportData.Compute("SUM(f_x12_comm)", ""), 2);
                excelBlock.TotalContent._1X2 = Formater.DecFormat(reportData.Compute("SUM(x12_comm)", ""), 2);
                excelBlock.TotalContent.OtherBettypes = Formater.DecFormat(reportData.Compute("SUM(others_comm)", ""), 2);
                excelBlock.TotalContent.FCS = Formater.DecFormat(reportData.Compute("SUM(f_cs_comm)", ""), 2);

                #endregion Assign TotalValue

                excelBlock.NoInfo.Hide();
            }
            else
            {
                excelBlock.TotalContent.Hide();
                excelBlock.NoInfo.NoInfoRow.NumCols = ReportColumnCount.ToString();
            }
        }

        /// <summary>
        /// Render user name field
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="contentRow"></param>
        private void RenderUserNameField(DataRow dr, CommissionByBetType_ExcelContents_Content_Block contentRow)
        {
            // UserName
            if (_roleId == (int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Agent)
            {
                // If user is agent, not show downline link
                contentRow.DownlineLink.SetValue(dr["username"].ToString());
            }
            else
            {
                // show downline link
                contentRow.DownlineLink.UserName = dr["username"].ToString();
                if (_roleId == (int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Master)
                {
                    contentRow.DownlineLink.RoleId = ((int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Agent).ToString();
                    contentRow.DownlineLink.CustId = dr["recommend"].ToString();
                }
                else
                {
                    contentRow.DownlineLink.RoleId = ((int)Sunplus.Agent.Model.BL.Definitions.UserTypes.Master).ToString();
                    contentRow.DownlineLink.CustId = dr["mrecommend"].ToString();
                }
            }
        }

        /// <summary>
        /// Render Hidden Field
        /// </summary>
        /// <param name="template"></param>
        private void RenderHiddenField(CommissionByBetType_Template template)
        {
            template.CustId = _custId.ToString();
            template.RoleId = _roleId.ToString();
        }

        /// <summary>
        /// Get total column count of report
        /// </summary>
        private int _reportColumnCount = 0;

        private int ReportColumnCount
        {
            get
            {
                if (_reportColumnCount == 0)
                {
                    int numCol = 31;
                    if (Sunplus.Agent.Model.BL.Definitions.AppSettings.IsEnableP2P) numCol++;
                    if (UserPermission.IsShowCasino) numCol++;
                    if (UserPermission.IsShowLiveCasino) numCol++;
                    if (UserPermission.IsShowNumberGame) numCol++;
                    _reportColumnCount = numCol;
                }
                return _reportColumnCount;
            }
        }

        #endregion Render Utility

        #region IContextMenuColumns Members

        public string RenderContextMenu(string[] texts, string[] cols)
        {
            return new ContextMenuColumns().RenderCheckbox(texts, cols, new string[] { "15", "11", "2", "1", "6" });
        }

        public void RegisterCommonScriptObjects(string tableId, string[] ctxMenuRegionIds, string ctxMenuContainerId, object implement)
        {
            var encodeStr = string.Concat(CurrentUserSession.UserId, CurrentUserSession.CustId, CurrentUserSession.SubAccId, implement.GetType().BaseType.FullName);

            RegisterJavascriptObject(ContextMenuColumns.TableId, tableId);
            RegisterJavascriptObject(ContextMenuColumns.CookiePrefix, Encryption.PearsonEncode(encodeStr));
            RegisterJavascriptObject(ContextMenuColumns.ContextRegionIds, ctxMenuRegionIds);
            RegisterJavascriptObject(ContextMenuColumns.ContextContainerId, ctxMenuContainerId);
            RegisterJavascriptObject("noHideTotal", true);
            RegisterJavascriptObject("colSpans", new string[] { "15", "11", "2", "1", "6" });
        }

        public string RenderTableClasses(string[] cols)
        {
            var encodeStr = string.Concat(CurrentUserSession.UserId, CurrentUserSession.CustId, CurrentUserSession.SubAccId, this.GetType().BaseType.FullName);
            var cookie = Request.Cookies[Encryption.PearsonEncode(encodeStr)];
            var classes = string.Empty;

            if (cookie != null)
            {
                var values = cookie.Value.Split(',');

                for (var i = 0; i < cols.Length; i++)
                {
                    classes += i >= values.Length || values[i].Equals("1") ? string.Concat(" show", cols[i], " .", cols[i], " ") : string.Empty;
                }
            }
            else
            {
                for (var i = 0; i < cols.Length; i++)
                {
                    if (i == 0 || i == 1 || i == 4)
                    {
                        classes += string.Empty;
                    }
                    else
                    {
                        classes += string.Concat(" show", cols[i], " .", cols[i], " ");
                    }
                }
            }

            return classes;
        }

        public string RenderContextMenu(string[] texts, string[] cols, string[] colspan = null, bool showLcs = false)
        {
            throw new NotImplementedException();
        }

        #endregion IContextMenuColumns Members
    }
}