using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Fanex.Age.UI.Reports.Components;
using Sunplus.Agent.Utilities;

/*
 * Created 20100529@Jacob - Render MatchWinloss page
 * Revision: Pending, this file relates with fire Report/
 * ?@? - ...
 *
 */

namespace Fanex.Age.UI.Reports.MatchWinloss
{
    public partial class MatchWinloss_Page : Reports_Page
    {
        public MatchWinloss_Page()
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
            MatchWinloss_Template tpl = new MatchWinloss_Template();

            if (_exportToExcel)
            {
                DataTable reportData = (DataTable)View;
                var builder = new Fanex.Age.Excel.Reports.MatchWinloss.MatchWinLossDetailReportBuilder(reportData, IsBold, _dateFrom, _dateTo, GetLangResourceManager(), CurrentUserSession.IsSuper, CurrentUserSession.IsMaster, CurrentUserSession.IsAgent);
                var excelStream = builder.Build();
                if (excelStream != null)
                {
                    ExportToExcel(excelStream, "MatchWinloss.xls");
                    excelStream.Dispose();
                    return;
                }

                // Need to render report content only
                tpl.ExcelContents.BoldRow = "";
                RenderReportContent(tpl.ExcelContents);
                ExportToExcel(Response, tpl.ExcelContents.ToString(), "MatchWinloss");
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
            DataTable reportData = _ReportBO.GetMatchWinloss(_dateFrom, _dateTo);
            reportData.Columns.Add("Total", typeof(decimal), "GrossComm+commission+positiontaking");
            reportData = SortData(reportData);
            return reportData;
        }

        #endregion Override

        #region Render Utility

        /// <summary>
        /// Render Report header
        /// </summary>
        /// <param name="headerTemplate"></param>
        private void RenderReportHeader(MatchWinloss_Template template)
        {
            template.ExcelContents.BoldRow = this.BoldRowClass;
        }

        /// <summary>
        /// Render report content header
        /// Assign DateFrom, DateTo, Title
        /// </summary>
        /// <param name="excelBlock"></param>
        private void RenderReportContentHeader(MatchWinloss_ExcelContents_Block excelBlock)
        {
            // Render date from and date to
            excelBlock.DateFrom = Formater.ShortDateFormat(_dateFrom);
            excelBlock.DateTo = Formater.ShortDateFormat(_dateTo);
        }

        /// <summary>
        /// Render History message
        /// </summary>
        /// <param name="messageTemplate"></param>
        private void RenderHistoryMessage(HistoryMessage_Template messageTemplate)
        {
            string message = Resources.Lang.ViewHistoryReport1.Replace("{viewhistory}", "<a href ='#' onclick='SearchByDate(1);'>" + Resources.Lang.ViewHistoryReport2 + "</a>");
            messageTemplate.ContentMessage.Hide();
            messageTemplate.OptionMsg.Hide();
            messageTemplate.GrossCommMsg.Hide();
        }

        /// <summary>
        /// Rnder report content
        /// </summary>
        /// <param name="template"></param>
        private void RenderReportContent(MatchWinloss_ExcelContents_Block excelBlock)
        {
            DataTable reportData = View as DataTable;

            // Render date from and date to
            RenderReportContentHeader(excelBlock);
            excelBlock.Content.Clear();
            int rowCount = reportData.Rows.Count;
            if (rowCount > 0)
            {
                excelBlock.NoInfo.Hide();

                MatchWinloss_ExcelContents_Content_Block matchBlock = new MatchWinloss_ExcelContents_Content_Block();
                for (int i = 0; i < rowCount; i++)
                {
                    DataRow dr = reportData.Rows[i];

                    if (reportData.Columns.Contains("closed") && Convert.ToInt32(dr["closed"]) != 0)
                    {
                        matchBlock.ColorClass = CLOSED_ROW_CLASS;
                    }
                    else matchBlock.ColorClass = i % 2 == 0 ? EVEN_ROW_CLASS : ODD_ROW_CLASS;

                    // Assign value
                    int betType = Convert.ToInt32(dr["bettype"]);
                    String eventName = "";
                    //if (bettype == 51 || bettype == 52)
                    if ((betType > 50 && betType < 57) || (betType > 60 && betType < 67)) // Finance
                    {
                        eventName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetFncIndexName(dr["homeid"]);
                    }
                    else if (betType > 80 && betType < 121) // Number Game
                    {
                        eventName = Resources.Lang.numbergameno + " " + dr["matchcode"];
                    }
                    else if (betType > 1100 && betType < 1104) // For Live Casino
                    {
                        eventName = Resources.Lang.LiveCasino + ": " + CacheRef.GetBettype(dr["bettype"]) + " " + dr["showtime"].ToString();
                    }
                    else
                    {
                        eventName = Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["homeid"]) + (Convert.ToBoolean(dr["isNeutral"]) ? Resources.Lang.IsNeutral : string.Empty) + "&nbsp;-vs-&nbsp;" + Sunplus.Agent.Model.BL.Cache.CacheRef.GetTeamName(dr["awayid"]);
                    }

                    matchBlock.EventName = eventName;
                    matchBlock.MatchId = dr["matchid"].ToString();

                    matchBlock.BetType = betType.ToString();
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
                    matchBlock.TypeName = strTypeNameNG + Sunplus.Agent.Model.BL.Cache.CacheRef.GetBettype(betType);

                    matchBlock.islive = dr["liveindicator"].ToString();
                    matchBlock.ViewType = Fanex.Age.UI.Common.WinLossByBetTypeMatch;
                    matchBlock.PositionTaking = Formater.DecFormat(dr["positiontaking"], 2);
                    matchBlock.GrossComm = Formater.DecFormat(dr["GrossComm"], 2);
                    matchBlock.Commission = Formater.DecFormat(dr["commission"], 2);
                    matchBlock.TotalValue = Formater.DecFormat(dr["Total"], 2);

                    excelBlock.Content.Append(matchBlock);
                }
                // Assign total value
                excelBlock.TotalContent.Commission = Formater.DecFormat(reportData.Compute("SUM(commission)", ""), 2);
                excelBlock.TotalContent.GrossComm = Formater.DecFormat(reportData.Compute("SUM(GrossComm)", ""), 2);
                excelBlock.TotalContent.PositionTaking = Formater.DecFormat(reportData.Compute("SUM(positiontaking)", ""), 2);
                excelBlock.TotalContent.Total = Formater.DecFormat(reportData.Compute("SUM(Total)", ""), 2);
            }
            else
            {
                excelBlock.TotalContent.Hide();
                excelBlock.NoInfo.NoInfoRow.NumCols = "6";
            }
        }

        #endregion Render Utility
    }
}