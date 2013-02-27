using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Fanex.Age.UI.Reports.Components;
using Sunplus.Agent.Model.Entities.Permission;
using Sunplus.Agent.Utilities;

/*
 * Created 20100529@Jacob - Render WinlossByBetTypeSuper page
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.UI.Reports.Winloss
{
    public partial class WinlossByBetTypeSuper_Page : Reports_Page
    {
        public WinlossByBetTypeSuper_Page()
        {
            MIN_REPORT_DATE = Sunplus.Agent.Model.BL.Reports.ReportsBO.BalanceUpDate;
        }

        #region Variable

        #endregion Variable

        #region Override

        /// <summary>
        ///
        /// </summary>
        /// <param name="writer"></param>
        public override void RenderContent(HtmlTextWriter writer)
        {
            WinlossByBetTypeSuper_Template tpl = new WinlossByBetTypeSuper_Template();
            tpl.LocalControl.Hide();

            RegisterJavascriptObject("role", CurrentUserSession.RoleId);

            if (_exportToExcel)
            {
                DataTable reportData = (DataTable)View;
                var resMan = GetLangResourceManager();
                var builder = new Fanex.Age.Excel.Reports.Winloss.WinlossByBetTypeSuperReportBuilder(reportData, IsBold, _dateFrom, _dateTo, resMan);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "WinlossByBetType.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = "";
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "WinlossByBetType");
            }
            else
            {
                // Render Report Header and history message
                RenderDateRange(tpl.ReportSearchHeader);
                RenderReportHeader(tpl);
                RenderHistoryMessage(tpl.HistoryMessage);

                RenderReportContent(tpl.ExcelContents);
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
            DataTable reportData = _ReportBO.GetWinlossByBetType(_dateFrom, _dateTo);
            reportData = SortData(reportData);
            return reportData;
        }

        #endregion Override

        #region Render Utility

        /// <summary>
        /// Render Report header
        /// </summary>
        /// <param name="headerTemplate"></param>
        private void RenderReportHeader(WinlossByBetTypeSuper_Template template)
        {
            template.ExcelContents.BoldRow = this.BoldRowClass;
        }

        /// <summary>
        /// Render History message
        /// </summary>
        /// <param name="messageTemplate"></param>
        private void RenderHistoryMessage(HistoryMessage_Template messageTemplate)
        {
            messageTemplate.ContentMessage.Hide();
            messageTemplate.OptionMsg.Hide();
            messageTemplate.GrossCommMsg.Hide();
        }

        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeader(WinlossByBetTypeSuper_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }

        /// <summary>
        /// Rnder report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(WinlossByBetTypeSuper_ExcelContents_Block excelBlock)
        {
            DataTable reportData = View as DataTable;
            // Render date from and date to
            RenderReportContentHeader(excelBlock);

            excelBlock.Content.Clear();
            int numRow = reportData.Rows.Count;
            if (numRow > 0)
            {
                WinlossByBetTypeSuper_ExcelContents_Content_Block rowBlock = new WinlossByBetTypeSuper_ExcelContents_Content_Block();
                WinlossByBetTypeSuper_LocalControl_CasinoLink_Block casinoLink = new WinlossByBetTypeSuper_LocalControl_CasinoLink_Block();
                WinlossByBetTypeSuper_LocalControl_BingoLink_Block bingoLink = new WinlossByBetTypeSuper_LocalControl_BingoLink_Block();

                for (int i = 0; i < numRow; i++)
                {
                    DataRow dr = reportData.Rows[i];
                    int betType = Convert.ToInt32(dr["bettype"]);
                    string betTypeName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetBettype(dr["bettype"]);

                    switch (betType)
                    {
                        case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Casino_Games:  // 71
                            {
                                casinoLink.BetTypeName = betTypeName;
                                casinoLink.CasinoType = "Casino";
                                rowBlock.BetTypeName = casinoLink.ToString();
                            }
                            break;
                        case (int)Sunplus.Agent.Model.BL.Definitions.BetType.P2P: // 1000
                            {
                                //casinoLink.BetTypeName = Resources.Lang.P2P;
                                //casinoLink.CasinoType = "P2P";
                                rowBlock.BetTypeName = Resources.Lang.P2P;
                            }
                            break;
                        case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Bingo: // 73
                            {
                                bingoLink.BetTypeName = betTypeName;
                                rowBlock.BetTypeName = bingoLink.ToString();
                            }
                            break;
                        case (int)(Sunplus.Agent.Model.BL.Definitions.BetType.N_Over_Under): // Number game Over/Under
                            rowBlock.BetTypeName = Resources.Lang.next + " " + betTypeName + " (" + Resources.Lang.numbergame + ")";
                            break;
                        case (int)(Sunplus.Agent.Model.BL.Definitions.BetType.N_Odd_Even): // Number game Odd/Even
                            string strNextCombo = (int)dr["liveindicator"] == 1 ? Resources.Lang.next + " " : Resources.Lang.ft + ". ";
                            rowBlock.BetTypeName = strNextCombo + betTypeName + " (" + Resources.Lang.numbergame + ")";
                            break;
                        case (int)(Sunplus.Agent.Model.BL.Definitions.BetType.NumberWheel): // Number game - NumberWheel
                            string typeNumberWheel = (int)dr["liveindicator"] == 1 ? Resources.Lang.LiveGame : Resources.Lang.PreGame;
                            rowBlock.BetTypeName = betTypeName + " (" + typeNumberWheel + ")";
                            break;
                        // For Live Casino
                        case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Baccarat: // 1101
                        case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Roulette: // 1102
                        case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Sicbo: // 1103
                            {
                                if (UserPermission.IsShowLiveCasino)
                                {
                                    rowBlock.BetTypeName = betTypeName;
                                    break;
                                }
                                else
                                    continue;
                            }
                        default:
                            rowBlock.BetTypeName = betTypeName;
                            break;
                    }

                    rowBlock.ColorClass = (i % 2 == 0) ? EVEN_ROW_CLASS : ODD_ROW_CLASS; ;

                    rowBlock.GrossComm = Formater.DecFormat(dr["GrossComm"], 2);
                    rowBlock.Turnover = Formater.DecFormat(dr["turnover"], 2);
                    rowBlock.Total = Formater.DecFormat(dr["MAgTotal"], 2);
                    rowBlock.Winloss = Formater.DecFormat(dr["SAgWinlost"], 2);
                    rowBlock.Comm = Formater.DecFormat(dr["comm"], 2);
                    rowBlock.TotalSuper = Formater.DecFormat(dr["SAgTotal"], 2);
                    rowBlock.Company = Formater.DecFormat(dr["Company"], 2);

                    // Add row
                    excelBlock.Content.Append(rowBlock);
                }

                // Total row
                excelBlock.TotalContent.TotalGrossComm = Formater.DecFormat(reportData.Compute("SUM(GrossComm)", ""), 2);
                excelBlock.TotalContent.TotalTurnover = Formater.DecFormat(reportData.Compute("SUM(turnover)", ""), 2);
                excelBlock.TotalContent.TotalTotal = Formater.DecFormat(reportData.Compute("SUM(MAgTotal)", ""), 2);
                excelBlock.TotalContent.TotalWinloss = Formater.DecFormat(reportData.Compute("SUM(SAgWinlost)", ""), 2);
                excelBlock.TotalContent.TotalCom = Formater.DecFormat(reportData.Compute("SUM(comm)", ""), 2);
                excelBlock.TotalContent.TotalTotalSuper = Formater.DecFormat(reportData.Compute("SUM(SAgTotal)", ""), 2);
                excelBlock.TotalContent.TotalCompany = Formater.DecFormat(reportData.Compute("SUM(Company)", ""), 2);

                // Hide noinfo row
                excelBlock.NoInfo.Hide();
            }
            else
            {
                excelBlock.TotalContent.Hide();
                excelBlock.NoInfo.NoInfoRow.NumCols = "8";
            }
        }

        #endregion Render Utility
    }
}