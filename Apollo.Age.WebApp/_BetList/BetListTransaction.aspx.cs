// ---------------------------------------------------------------------------------
// <copyright file="BetList.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.01.12">Create</change>
//     <change who="Kaden.Trinh" date="2013.05.31">Change rule: we have to get date from after movebetday 1 day</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.Forecast.Services;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Resources;
    using Apollo.Age.Web;
    using Apollo.Age.WebApp;
    using Apollo.Age.WebApp._BetList;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
    using Fanex.BetList.Age.Definition;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;

    public partial class BetListTransaction : TplPage
    {
        #region Variables

        private string title = string.Empty;
        private BetListTransaction_Template tpl = new BetListTransaction_Template();

        private string username = string.Empty;
        private DateTime fromDate, toDate;
        private int agentId, custId, currentPage;
        private bool userType = false;
        private string pageType = string.Empty;
        private string statusIds = string.Empty;
        private bool isShowAgentPT = true;
        private bool isShowWinLost = true;
        private DateTime moveBetsDay, maxDate, minDate;
        private string mainShareHolderId, mainShareHolderUsername;

        private int pageIndex = 1;
        private int pageSize = 50;
        private List<object> pageSizes = new List<object>(new object[] { 10, 20, 50, 100, 200, 500 });
        private int totalRows;

        #endregion Variables

        public override void RenderContent(HtmlTextWriter writer)
        {
            #region INIT

            this.BoxFilter();
            Dictionary<string, DataTable> results = View as Dictionary<string, DataTable>;

            var ticketResult = View as TicketResult;
            var tickets = ticketResult.TicketList;
            var ticketData = ticketResult.TicketDataList;

            var forecastService = new ForecastService();
            var listBetType = forecastService.GetListScoreMapBetType();
            bool isShowScoremap;

            int index = 0;
           // bool isAdminCanViewVNIP = true;
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            ITicketHelper ticketHelper = new TicketHelper();

            #endregion INIT

            #region RENDER HTML

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();

            StringBuilder content = new StringBuilder();

            Fanex.BetList.Age.ExtTemplate.OpenBetList_Template betlistOpenTag = new Fanex.BetList.Age.ExtTemplate.OpenBetList_Template();
            Header1_Template betlistHeader = new Header1_Template();

            if (!isShowAgentPT)
            {
                betlistHeader.Agent.Hide();
                betlistHeader.Winloss.Hide();
            }

            content.Append(betlistOpenTag);
            content.Append(betlistHeader);
            content.Append("<tbody>");

            foreach (ITicket ticket in tickets)
            {
                index++;
                isShowScoremap = listBetType.Contains(ticket.BetTypeId) && ticket.Status.Equals(Fanex.BetList.Core.Entities.BetStatus.Running);
                ticketHelper.Index = index;

                builder.Append(string.Format("<tr id='{0}' {1}>", ticket.TransId, index % 2 == 0 ? string.Empty : "class='alt'"))
                       .AddMember(ticket, "<td class='l'>", "</td>")
                       .AddTrans(ticket, ticketHelper, "<td class='c'>", "</td>", funcGetOddsTypeName, BetListPage.Report)
                       .AddChoice(ticket, ticketHelper, ticketData, "<td class='r bl_evt'><div>", "</div></td>", isShowScoremap)
                       .AddOdds(ticket, ticketData, "<td class='bl_underdog'>", "</td>", funcGetOddsTypeName)
                       .AddStake(ticket, "<td class='bl_underdog'>", "</td>");

                if (isShowWinLost)
                {
                    builder.AddWinLoss(ticket, "<td class='r nowrap'>", "</td>");
                }

                if (isShowAgentPT)
                {
                    builder.AddAgentPT(ticket, ticketData, "<td class='r'>", "</td>");
                    builder.AddAgentComm(ticket, ticketData, false, "<td class='r'>", "</td>");
                }

                builder.Append("</tr>");
            }
            if (pageType.Equals("transaction"))
            {
                builder.AddFooter(tickets, string.Empty, string.Empty, BetListType.Winlost | BetListType.Member);
            }
            else
            {
                builder.AddFooter(tickets, string.Empty, string.Empty, BetListType.Member);
            }
            content.Append(builder.GetBetList());
            content.Append("</tbody></table>");

            tpl.content = content.ToString();
            tpl.HiddenFields.ApoRoot = Page.ResolveUrl("~/");
            tpl.HiddenFields.MainShareHolderId = mainShareHolderId;
            tpl.HiddenFields.MainShareHolderUsername = mainShareHolderUsername;

            // Paging
            Paging_Control pagingControl = new Paging_Control(tpl, "pagination", this.pageSize, Request.Url.AbsoluteUri, this.totalRows, pageSizes);
            tpl.PagingControl = pagingControl.ToString();

            writer.Write(tpl);
            RegisterJavascriptObject("titleViewComm", BetList.viewCommDetails);
            RegisterJavascriptObject("refNo", BetList.refno);
            RegisterJavascriptObject("titleScoreMap", BetList.scoreMap.ToUpper());

            #endregion RENDER HTML
        }

        private void BoxFilter()
        {
            var selectedUserType = "0";

            if (userType)
            {
                selectedUserType = "1";
            }

            tpl.Filter.User.option_user = HtmlGenerator.ComboBox("userType", selectedUserType, this.CreateUserTypeOption(), "value", "text", null, 72);
            tpl.Filter.valueFromDate = fromDate.ToString("yyyy-M-d");
            tpl.Filter.valueToDate = toDate.ToString("yyyy-M-d");
            tpl.Filter.valueMaxDate = maxDate.ToString("yyyy-M-d");
            tpl.Filter.valueMinDate = minDate.ToString("yyyy-M-d");
            tpl.Filter.User.username = username;
            tpl.HiddenFields.pageType = pageType;
        }

        private DataTable CreateUserTypeOption()
        {
            DataTable dt = new DataTable();
            DataColumn column;
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "value";
            dt.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "text";
            dt.Columns.Add(column);

            DataRow row;

            row = dt.NewRow();
            row["value"] = "1";
            row["text"] = Apollo.Age.Resources.Lang.agent;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["value"] = "0";
            row["text"] = Apollo.Age.Resources.Lang.member;
            dt.Rows.Add(row);

            return dt;
        }

        public override object InvokeData(Hashtable hash)
        {
            var betListService = new BetListService();
            TicketResult mainTickets = new TicketResult();
            int errorCode;
            betListService.GetUserIdByUserName(CurrentSession.AgentId, userType, username, out agentId, out custId, out errorCode);

            if (errorCode == 0)
            {
                var criteria = new BetListCriteria
                {
                    LoggedId = CurrentSession.AgentId,
                    AgentId = agentId,
                    CustId = custId,
                    FromDate = ResetStartDate(fromDate),
                    ToDate = ResetEndDate(toDate),
                    StatusIds = statusIds,
                    MoveBetsDay = moveBetsDay
                };

                if (pageType.Equals("transaction"))
                {
                    mainTickets = betListService.GetTicketResult(criteria);
                }
                else
                {
                    mainTickets = betListService.GetTicketResult(criteria);
                }

                // Paging
                this.totalRows = mainTickets.TicketList.Count();

                currentPage = this.pageSize * (this.pageIndex - 1);

                if (currentPage < this.totalRows)
                {
                    var newTicketList = mainTickets.TicketList.Skip(currentPage)
                                 .Take(this.pageSize).ToList() as IList<ITicket>;
                    mainTickets.TicketList = newTicketList;
                }
            }
            return mainTickets;
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            var reportService = new ReportService();

            pageType = GetStringRequest("type", string.Empty).ToLower();

            if (pageType.Equals("cancel"))
            {
                tpl.title = Apollo.Age.Resources.Lang.voidandreject;
                isShowAgentPT = isShowWinLost = false;
                maxDate = DateTime.Now;
                moveBetsDay = reportService.MoveBetsDay();
                minDate = reportService.MoveBetsDay().AddDays(1);

                statusIds = string.Format("{0},{1}", (int)Apollo.Age.Enumerations.BetStatus.Void, (int)Apollo.Age.Enumerations.BetStatus.Reject);
            }
            else if (pageType.Equals("transaction"))
            {
                tpl.title = Apollo.Age.Resources.Lang.transaction;
                isShowWinLost = true;
                maxDate = DateTime.Now;
                moveBetsDay = reportService.GetBalanceUpDay();
                minDate = moveBetsDay;

                statusIds = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                    (int)Apollo.Age.Enumerations.BetStatus.Void,
                    (int)Apollo.Age.Enumerations.BetStatus.Reject,
                    (int)Apollo.Age.Enumerations.BetStatus.Running,
                    (int)Apollo.Age.Enumerations.BetStatus.Waiting,
                    (int)Apollo.Age.Enumerations.BetStatus.Refund,
                    (int)Apollo.Age.Enumerations.BetStatus.Draw,
                    (int)Apollo.Age.Enumerations.BetStatus.Won,
                    (int)Apollo.Age.Enumerations.BetStatus.Lose);
            }
            fromDate = GetDateTimeRequest("fromDate", maxDate);
            toDate = GetDateTimeRequest("toDate", maxDate);
            username = this.GetStringRequest("username", CurrentSession.AgentName).Trim();
            userType = GetBooleanRequest("userType", true);
            mainShareHolderId = GetStringRequest("mainShareHolderId", string.Empty);
            mainShareHolderUsername = GetStringRequest("mainShareHolderUsername", string.Empty);

            this.pageIndex = GetIntRequest("pageIndex", this.pageIndex);
            this.pageSize = GetIntRequest("pageSize", this.pageSize);
        }

        public override object ValidateRequest(Hashtable hash)
        {
            if (string.IsNullOrEmpty(Request["type"]))
            {
                return new ErrorPage_Page(Apollo.Age.Resources.Lang.strNotAccessPage);
            }

            if (DateTime.Parse(fromDate.ToShortDateString()) < DateTime.Parse(minDate.ToShortDateString())
                || DateTime.Parse(toDate.ToShortDateString()) < DateTime.Parse(minDate.ToShortDateString())
                || DateTime.Parse(toDate.ToShortDateString()) > DateTime.Parse(maxDate.ToShortDateString()))
            {
                return new ErrorPage_Page(Apollo.Age.Resources.Lang.strNotAccessPage);
            }

            return null;
        }

        protected ICurrentSession CurrentSession
        {
            get
            {
                return SessionManager.CurrentSession;
            }
        }

        #region Format Start, End date

        public static DateTime ResetStartDate(DateTime dt)
        {
            return dt.AddMilliseconds(-(((dt.Hour * 60 + dt.Minute) * 60 + dt.Second) * 1000 + dt.Millisecond));
        }

        public static DateTime ResetEndDate(DateTime dt)
        {
            return ResetStartDate(dt).Add(new TimeSpan(23, 59, 59));
        }

        #endregion Format Start, End date
    }
}