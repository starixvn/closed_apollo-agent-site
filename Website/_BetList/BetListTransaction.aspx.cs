// ---------------------------------------------------------------------------------
// <copyright file="BetList.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.01.12">Create</change>
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
    using Apollo.Age.Web;
    using Fanex.Age.UI.Components;
    using Fanex.Agent.UI._BetList;
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
    using Fanex.BetList.Age.Definition;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Apollo.Age.Resources;

    public partial class BetListTransaction : BetListBase
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
            bool isAdminCanViewVNIP = true;
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
                       .AddNo(index + ((this.PageIndex - 1) * this.PageSize), "<td class='w-order'>", "</td>")
                       .AddMember(ticket, "<td class='l'>", "</td>")
                       .AddTrans(ticket, "<td class='c'>", "</td>")
                       .AddChoice(ticket, ticketHelper, ticketData, "<td class='r bl_evt'><div>", "</div></td>", isShowScoremap)
                       .AddOdds(ticket, ticketData, "<td class='bl_underdog'>", "</td>", funcGetOddsTypeName)
                       .AddStake(ticket, "<td class='bl_underdog'>", "</td>");

                if (isShowWinLost)
                {
                    builder.AddWinLoss(ticket, "<td class='r nowrap'>", "</td>");
                }

                builder.AddStatus(ticket, ticketHelper, ticketData, "<td class='c'>", "</td>", isAdminCanViewVNIP);

                if (isShowAgentPT)
                {
                    builder.AddAgentPT(ticket, ticketData, "<td class='r'>", "</td>");
                    builder.AddAgentComm(ticket, ticketData, false, "<td class='r'>", "</td>");
                }

                builder.Append("</tr>");
            }
            builder.AddFooter(tickets, string.Empty, string.Empty, BetListType.Member);
            content.Append(builder.GetBetList());
            content.Append("</tbody></table>");

            tpl.content = content.ToString();
            tpl.HiddenFields.ApoRoot = Page.ResolveUrl("~/");
            tpl.HiddenFields.MainShareHolderId = mainShareHolderId;
            tpl.HiddenFields.MainShareHolderUsername = mainShareHolderUsername;

            // Paging            
            Paging_Control pagingControl = new Paging_Control(tpl, "pagination", this.PageSize, Request.Url.AbsoluteUri, this.TotalRows, PAGE_SIZES);
            tpl.PagingControl = pagingControl.ToString();

            writer.Write(tpl);
            RegisterJavascriptObject("titleViewComm", BetList.viewCommDetails.ToUpper());
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
            tpl.Filter.valueFromDate = fromDate.ToString("MM/dd/yyyy");
            tpl.Filter.valueToDate = toDate.ToString("MM/dd/yyyy");
            tpl.Filter.valueMaxDate = maxDate.ToString("MM/dd/yyyy");
            tpl.Filter.valueMinDate = minDate.ToString("MM/dd/yyyy");
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
                    FromDate = Formater.ResetStartDate(fromDate),
                    ToDate = Formater.ResetEndDate(toDate),
                    StatusIds = statusIds,
                    MoveBetsDay = moveBetsDay
                };

                if (pageType == "transaction")
                {
                    mainTickets = betListService.GetAllBetsTransactionByDateAndMem(criteria);
                }
                else
                {
                    mainTickets = betListService.GetAllBetsByDateAndMem(criteria);
                }

                // Paging
                this.TotalRows = mainTickets.TicketList.Count();

                currentPage = this.PageSize * (this.PageIndex - 1);

                if (currentPage < this.TotalRows)
                {
                    var newTicketList = mainTickets.TicketList.Skip(currentPage)
                                 .Take(this.PageSize).ToList() as IList<ITicket>;
                    mainTickets.TicketList = newTicketList;
                }
            }
            return mainTickets;
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            var reportService = new ReportService();
            moveBetsDay = reportService.MoveBetsDay();

            pageType = Request["type"];

            if (pageType == "cancel")
            {
                tpl.title = Apollo.Age.Resources.Lang.voidandreject;
                isShowAgentPT = isShowWinLost = false;
                maxDate = DateTime.Now;
                minDate = moveBetsDay;

                statusIds = string.Format("{0},{1}", (int)Apollo.Age.Enumerations.BetStatus.Void, (int)Apollo.Age.Enumerations.BetStatus.Reject);
            }
            else if (pageType == "transaction")
            {
                tpl.title = Apollo.Age.Resources.Lang.transaction;
                isShowWinLost = true;
                maxDate = DateTime.Now;
                minDate = maxDate.AddMonths(-3);

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
            fromDate = GetDateTimeRequest("fromDate", DateTime.Now);
            toDate = GetDateTimeRequest("toDate", DateTime.Now);
            username = this.GetStringRequest("username", CurrentSession.AgentName).Trim();
            userType = GetBooleanRequest("userType", true);
            mainShareHolderId = GetStringRequest("mainShareHolderId", string.Empty);
            mainShareHolderUsername = GetStringRequest("mainShareHolderUsername", string.Empty);

            this.PageIndex = GetIntRequest("pageIndex", this.PageIndex);
            this.PageSize = GetIntRequest("pageSize", this.PageSize);
        }

        public override object ValidateRequest(Hashtable hash)
        {
            if (string.IsNullOrEmpty(Request["type"]))
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

        private string GetStringRequest(string name, string defaultValue)
        {
            if (Request[name] == null)
            {
                return defaultValue;
            }
            return Request[name];
        }
    }
}