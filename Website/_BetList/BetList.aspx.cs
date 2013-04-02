// ---------------------------------------------------------------------------------
// <copyright file="BetList.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Jacob.Phan" date="2010.05.29">Render BetList page</change>
//     <change who="Thai" date="2011.03.08">Render BetList page</change>
//     <change who="Marc.Bui" date="2012.12.19">Apply CoreBetList</change>
//     <change who="Marc.Bui" date="2012.12.19">Rollback old Betlist</change>
//     <change who="Marc.Bui" date="2013.02.26">Apply CoreBetList</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Reports;
    using Fanex.BetList.Age.BetListCmd;
    using Fanex.BetList.Age.BetListExcelBuilderExt;
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
    using Fanex.BetList.Age.Definition;
    using Fanex.BetList.Age.DTO;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Model.BL.Forecast;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public partial class BetList_Page : WinlossReports_Page
    {
        string type = string.Empty;
        string title = string.Empty;
        int custId;
        int bettype;
        DateTime dateFrom, dateTo;

        bool exportExcel = false;
        IBetListCmd command = null;

        public override void RenderContent(HtmlTextWriter writer)
        {
            #region INIT

            BetListType betlistType = this.setBetListType();

            bool isShowWinLost = true,
                 isShowAgentPT = true,
                 isShowMember = true;

            if ((betlistType & BetListType.Canceled) == BetListType.Canceled)
            {
                isShowWinLost = isShowAgentPT = false;
            }
            else
            {
                if ((betlistType & BetListType.Winlost) != BetListType.Winlost)
                {
                    isShowWinLost = false;
                }

                if ((betlistType & BetListType.Member) != BetListType.Member)
                {
                    isShowMember = false;
                }

                if ((betlistType & BetListType.DailyStatement) == BetListType.DailyStatement)
                {
                    isShowAgentPT = false;
                }
            }

            DataTable betListData = View as DataTable;
            IList<ITicket> tickets = new List<ITicket>();

            if (betListData != null)
            {
                tickets = TicketTransformer.Transform(betListData);
                betListData.Dispose();
            }

            DataTable commandData = command.OutDataTable();
            List<ITicketData> ticketData = new List<ITicketData>();

            if (commandData != null)
            {
                ticketData = TicketDataTransformer.Transform(commandData);
                commandData.Dispose();
            }

            #endregion INIT

            #region Customize

            string reportTitle = string.Empty;

            if (type.Equals("MixParlay"))
            {
                if (exportExcel)
                {
                    reportTitle = Resources.Lang.lblMixParlay;
                }
                else
                {
                    reportTitle = string.Join(null, new string[]{Resources.Lang.lblMixParlay,
                                                         "&nbsp;|&nbsp; ",
                                                         "<a href=\"BetList.aspx?type=CombMixParlayBetList\">",
                                                         Resources.Lang.lblSystemParlay,
                                                         "</a>"});
                }
            }
            else if (type.Equals("CombMixParlayBetList"))
            {
                if (exportExcel) reportTitle = Resources.Lang.lblSystemParlay;
                else
                {
                    reportTitle = string.Join(null, new string[]{ "<a href=\"BetList.aspx?type=MixParlay\">",
                                                         Resources.Lang.lblMixParlay,
                                                         "</a>",
                                                         "&nbsp;|&nbsp; ",
                                                         Resources.Lang.lblSystemParlay});
                }
            }
            else
            {
                reportTitle = string.IsNullOrEmpty(command.Title) ? Resources.Lang.BetList : command.Title;
            }

            short excelColumns = 11;
            Header1_Template betlistHeader = new Header1_Template();

            if (!isShowAgentPT)
            {
                excelColumns--;
                betlistHeader.Agent.Hide();
            }

            if (!isShowMember)
            {
                excelColumns--;
                betlistHeader.Member.Hide();
            }

            if (!isShowWinLost)
            {
                excelColumns--;
                betlistHeader.Winloss.Hide();
            }

            #endregion Customize

            int index = 0;
            bool isAdminCanViewVNIP = UserSession.Current.IsAdminCanViewVNIP;

            if (exportExcel)
            {
                #region EXPORT EXCEL

                reportTitle = reportTitle.Replace("&nbsp;", " ").Replace("<a href=\"BetList.aspx?type=CombinationMixParlay\">", string.Empty).Replace("</a>", string.Empty);

                GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
                ITicketHelper ticketHelper = new TicketHelper();
                BetListExcelFactory factory = new BetListExcelFactory();
                BetListExcelBuilder builder = factory.CreateBuilder();

                builder.BuildWorksheet("BetList", excelColumns);
                builder.AddCaption(reportTitle, excelColumns);
                builder.AddHeader(isShowWinLost, isShowAgentPT, isShowMember);

                foreach (ITicket ticket in tickets)
                {
                    index++;
                    ticketHelper.Index = index;

                    builder
                        .AddNo(index);
                    if (isShowMember)
                    {
                        builder.AddMember(ticket, ticketHelper);
                    }

                    builder.AddTrans(ticket, ticketHelper);

                    builder
                        .AddChoice(ticket, ticketHelper, ticketData)
                        .AddOdds(ticket, ticketData, funcGetOddsTypeName)
                        .AddStake(ticket);

                    if (isShowWinLost)
                    {
                        builder.AddWinLoss(ticket);
                    }

                    builder
                        .AddStatus(ticket, ticketHelper, ticketData, isAdminCanViewVNIP);

                    if (isShowAgentPT)
                    {
                        builder.AddAgentPT(ticket, ticketData);
                    }
                }

                if (tickets.Count == 0 || (betlistType & BetListType.Winlost) == BetListType.Winlost)
                {
                    builder.AddFooter(tickets, isShowMember, excelColumns);
                }

                MemoryStream ms = new MemoryStream();
                IWorkbook resultWorkbook = builder.GetBetList();
                resultWorkbook.Write(ms);

                ExportToExcel(ms, "NewBetList.xls");
                ms.Dispose();
                return;

                #endregion EXPORT EXCEL
            }
            else
            {
                #region RENDER HTML

                var tpl = new BetListTemplate { title = reportTitle };
                var content = new StringBuilder();

                content.Append(betlistHeader);
                content.Append("<tbody>");

                var forecastBO = new ForecastBO(UserSession.Current.NetSessionId);
                List<int> lstScoremap = forecastBO.GetListBetTypeScoreMap();
                bool isShowScoremap;
                bool deviceMode = ((BetListType)bettype & BetListType.SmartPhone) != BetListType.SmartPhone;

                GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
                var ticketHelper = new TicketHelper();
                var factory = new BetListHTMLFactory();
                var builder = factory.CreateBuilder();

                foreach (ITicket ticket in tickets)
                {
                    isShowScoremap = lstScoremap.Contains(ticket.BetTypeId)
                        && ticket.Status.Equals(BetStatus.Running) && deviceMode;

                    ticketHelper.Index = ++index;

                    builder.Append(string.Format("<tr id='{0}'>", ticket.TransId))
                           .AddNo(index, "<td class='c'>", "</td>");

                    if (isShowMember)
                    {
                        builder.AddMember(ticket, "<td class='c'>", "</td>");
                    }

                    builder.AddTrans(ticket, "<td class='c'>", "</td>")
                           .AddChoice(ticket, ticketHelper, ticketData, "<td class='bl_evt'>", "</td>", isShowScoremap)
                           .AddOdds(ticket, ticketData, "<td class='bl_underdog'>", "</td>", funcGetOddsTypeName)
                           .AddStake(ticket, "<td class='bl_underdog'>", "</td>");

                    if (isShowWinLost)
                    {
                        builder.AddWinLoss(ticket, "<td class='r nowrap'>", "</td>");
                    }

                    builder.AddStatus(ticket, ticketHelper, ticketData, "<td class='c'>", "</td>", isAdminCanViewVNIP);
                    builder.AddAgentPT(ticket, ticketData, "<td class='r'>", "</td>");
                    builder.AddAgentComm(ticket, ticketData, false, "<td class='r'>", "</td>");

                    builder.Append("</tr>");
                }

                builder.AddFooter(tickets, string.Empty, string.Empty, (BetListType)betlistType);

                content.Append(builder.GetBetList());
                content.Append("</tbody>");

                tpl.content = content.ToString();
                writer.Write(tpl);

                RegisterJavascriptObject("type", type);
                RegisterJavascriptObject("fdate", dateFrom.ToShortDateString());
                RegisterJavascriptObject("tdate", dateTo.ToShortDateString());
                RegisterJavascriptObject("isHistoryReport", base._isHistoryReport == true ? 1 : 0);
                RenderFilterElementNames();

                #endregion RENDER HTML
            }
        }

        public override object InvokeData(Hashtable hash)
        {
            DataTable dtResult = null;

            command = BetListFactory.CreateBetList(type, CurrentUserSession.NetSessionId, _permission);
            if (command != null)
            {
                //MixParlay list
                if (type.Equals("MixParlay"))
                {
                    hash.Add("bettype", 9);
                }
                //Combination MP list
                else if (type.Equals("CombMixParlayBetList"))
                {
                    hash.Add("bettype", 29);
                }
                else
                {
                    hash.Add("custid", custId);
                    hash.Add("fdate", dateFrom);
                    hash.Add("tdate", dateTo);
                    hash.Add("bettype", bettype);
                }

                command.SetParams(hash);
                dtResult = command.Execute() as DataTable;
            }
            return dtResult;
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            base.ProcessRequest(ref hash);

            type = Request["type"];

            if (!type.Equals("MixParlay") && !type.Equals("CombinationMixParlay"))
            {
                if (Request["fdate"] == null || Request["tdate"] == null)
                {
                    hash.Add("DateIsNull", true);
                }

                int.TryParse(Request["custid"] + string.Empty, out custId);
                int.TryParse(Request["bettype"] + string.Empty, out bettype);
                dateFrom = Cast.AsDateTime(Request["fdate"]);
                dateTo = Cast.AsDateTime(Request["tdate"]);

                hash.Add("language", Request["lang"] + string.Empty);
                hash.Add("username", Request["username"] + string.Empty);
                hash.Add("race", Request["race"] + string.Empty);
                hash.Add("matchid", Request["matchid"] != null ? int.Parse(Request["matchid"]) : 0);
                hash.Add("leagueid", Request["leagueid"] != null ? int.Parse(Request["leagueid"]) : 0);
                hash.Add("islive", Request["islive"] != null ? int.Parse(Request["islive"]) : 0);
                hash.Add("transtype", Request["transtype"] != null ? int.Parse(Request["transtype"]) : 0);
                hash.Add("betteam", Request["betteam"] + string.Empty);
            }
            else
            {
                dateFrom = DateTime.Now;
                dateTo = DateTime.Now;
            }

            exportExcel = Request["exporttoexcel.x"] != null ? true : false;
        }

        public override object ValidateRequest(Hashtable p)
        {
            if (Request["custid"] != null)
            {
                if (int.Parse(Request["custid"].ToString()) <= 0)
                {
                    return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
                }
            }

            if (Request["bettype"] != null)
            {
                if (int.Parse(Request["bettype"].ToString()) < 0)
                {
                    return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
                }
            }

            DateTime minDate = new DateTime(2000, 1, 1);
            DateTime maxDate = DateTime.Now.AddDays(30);

            if (dateFrom < minDate || dateTo < minDate || dateTo > maxDate) return new ErrorPage_Page(Resources.Lang.strNotAccessPage);

            return null;
        }

        private BetListType setBetListType()
        {
            BetListType betListType = command.BetListType;

            if (Request["racing"] != null && Request["racing"].ToString().Equals("1"))
            {
                betListType |= BetListType.Winlost;
            }
            return betListType;
        }

        public new void ExportToExcel(HttpResponse Response, string data, string filename)
        {
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + filename.Replace(" ", "") + ".xls");
            Response.Charset = "utf-8";
            Response.ContentType = "application/vnd.xls";

            const String excelHtmlHeader = @"<html>
                    <head>
                        <META http-EQUIV='Content-Type' CONTENT='text/html; charset=unicode-utf8'>
                        <style>
                            body{font-family:Tahoma; font-size:12px; color:#000000; background:#ffffff;}
                            td,table{border-collapse:collapse;border-color:#7C98A4; border-style:Solid; border-width:thin;}
                            .league{color: #000000; background: #aacccc;}
                            td{vertical-align:top;text-align:right;font-size:12px;}
                            font, div, span{font-size:12px;text-decoration:none;}
                            .bl_underdog{font-weight:bold}
                            .bl_footertotal{font-weight:bold}
                            .bl_match{white-space: nowrap; width: 200px}
                            .bl_favorite{color: red;font-weight:bold}
                            a{font-size:12px;text-decoration: none; color: #000}
                            .r,td.r,td.div{text-align:right;font-size:12px;}
                            .l,td.l{text-align:left;font-size:12px;}
                            .c,td.c{text-align:center;font-size:12px;}
                            .bl_footertotal{background-color:#aaccee}
                            thead tr th{background-color:#aaccee; font-size:12px;  border-right: thin solid #7C98A4;}
                            .endTbl{border-color:#ffffff;}
                            .div_result{color: rgb(27, 164, 237);}
                            .strike_through{text-decoration: line-through;}
                        </style>
                    </head>
                    <body>
                    ";
            const String excelHtmlFooter = @"
                        <table class='endTbl'>
                            <tr>
                                <td></td>
                            </tr>
                        </table>
                    </body>
                </html>";

            Response.Write(excelHtmlHeader);
            Response.Write(data);
            Response.Write(excelHtmlFooter);

            Response.End();
        }
    }
}