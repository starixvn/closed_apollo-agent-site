// ---------------------------------------------------------------------------------
// <copyright file="BetListOnMember.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.04.10">Create</change>
//     <change who="Kaden.Trinh" date="2013.04.10">Implement betlist Outstanding</change>
//     <change who="Kaden.Trinh" date="2013.04.16">Implement betlist Account Info</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Models;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.Forecast.Services;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Web;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Reports.BetList;
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
    using Fanex.BetList.Age.Definition;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;

    public partial class BetListOnMember : TplPage
    {
        #region Variables

        private DataTable dtResult = null;
        private DataTable outDataTable = null;

        private int custid;
        private string username = string.Empty;
        private DateTime fromDate, toDate, balanceUpDay, moveBetsDay;
        private string statusIds = string.Empty;
        private string pageType = string.Empty;
        private string mainShareHolderId, mainShareHolderUsername;

        #endregion Variables

        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            #region INIT

            var tpl = new BetListOnMember_Template();
            tpl.title = Resources.Lang.lblBetLists + " - " + username;
            if (pageType == "accountinfo")
            {
                var maxDate = DateTime.Now;
                tpl.HiddenFields.username = username;
                tpl.HiddenFields.custId = custid.ToString();
                tpl.Filter.valueFromDate = fromDate.ToShortDateString();
                tpl.Filter.valueToDate = toDate.ToShortDateString();
                tpl.Filter.valueMaxDate = maxDate.ToShortDateString();
                tpl.Filter.valueMinDate = moveBetsDay.ToShortDateString();
                tpl.HiddenFields.pageType = pageType;
                tpl.HiddenFields.DateTime.Hide();
            }
            else
            {
                tpl.HiddenFields.DateTime.valueFromDate = fromDate.ToShortDateString();
                tpl.HiddenFields.DateTime.valueToDate = toDate.ToShortDateString();
                tpl.Filter.Hide();
            }

            // Render Content
            var ticketResult = View as TicketResult;
            var tickets = ticketResult.TicketList;
            var ticketData = ticketResult.TicketDataList;

            StringBuilder content = new StringBuilder();
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            TicketHelper ticketHelper = new TicketHelper();
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;

            Header1_Template betlistHeader = new Header1_Template();
            betlistHeader.Member.Hide();

            Fanex.BetList.Age.ExtTemplate.OpenBetList_Template betlistOpenTag = new Fanex.BetList.Age.ExtTemplate.OpenBetList_Template();

            var forecastService = new ForecastService();
            var listBetType = forecastService.GetListScoreMapBetType();
            bool isShowScoremap;
            bool isAdminCanViewVNIP = CurrentSession.IsAdminCanViewVNIP;

            #endregion INIT

            content.Append(betlistOpenTag);
            content.Append(betlistHeader);
            content.Append("<tbody>");
            int index = 0;

            foreach (ITicket ticket in tickets)
            {
                isShowScoremap = listBetType.Contains(ticket.BetTypeId) && ticket.Status.Equals(Fanex.BetList.Core.Entities.BetStatus.Running);

                ticketHelper.Index = index++;
                builder
                    .Append(string.Format("<tr id='r{0}' {1}>", ticket.TransId, index % 2 == 0 ? string.Empty : "class='alt'"))
                    .AddNo(index, "<td class='w-order'>", "</td>");

                builder
                    .AddTrans(ticket, "<td class='c nonbreak'>", "</td>")
                    .AddChoice(ticket, ticketHelper, ticketData, "<td class='r bl_evt'>", "</td>", isShowScoremap)
                    .AddOdds(ticket, ticketData, "<td class='bl_underdog nonbreak'>", "</td>", funcGetOddsTypeName)
                    .AddStake(ticket, "<td class='bl_underdog'>", "</td>");

                builder.AddWinLoss(ticket, "<td class='r nonbreak'>", "</td>");
                builder.AddStatus(ticket, ticketHelper, ticketData, "<td class='c'>", "</td>", isAdminCanViewVNIP);
                builder.AddAgentPT(ticket, ticketData, "<td class='r'>", "</td>");
                builder.AddAgentComm(ticket, ticketData, false, "<td class='r'>", "</td>");
                builder.Append("</tr>");
            }
            builder.AddFooter(tickets, string.Empty, string.Empty, BetListType.Member);
            content.Append(builder.GetBetList());
            content.Append("</tbody></table>");

            tpl.content = content.ToString();
            tpl.HiddenFields.pageType = pageType;
            tpl.HiddenFields.ApoRoot = Page.ResolveUrl("~/");
            tpl.HiddenFields.MainShareHolderId = mainShareHolderId;
            tpl.HiddenFields.MainShareHolderUsername = mainShareHolderUsername;
            writer.Write(tpl);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable hash)
        {
            pageType = Request["type"];

            if (pageType == "accountinfo")
            {
                statusIds = string.Format("{0}^{1}^{2}^{3}^{4}^{5}^{6}^{7}",
                   (int)Apollo.Age.Enumerations.BetStatus.Void,
                   (int)Apollo.Age.Enumerations.BetStatus.Reject,
                   (int)Apollo.Age.Enumerations.BetStatus.Running,
                   (int)Apollo.Age.Enumerations.BetStatus.Waiting,
                   (int)Apollo.Age.Enumerations.BetStatus.Refund,
                   (int)Apollo.Age.Enumerations.BetStatus.Draw,
                   (int)Apollo.Age.Enumerations.BetStatus.Won,
                   (int)Apollo.Age.Enumerations.BetStatus.Lose);
            }
            else if (pageType == "report")
            {
                statusIds = string.Format("{0}^{1}^{2}^{3}^{4}^{5}",
                       (int)Apollo.Age.Enumerations.BetStatus.Void,
                       (int)Apollo.Age.Enumerations.BetStatus.Reject,
                       (int)Apollo.Age.Enumerations.BetStatus.Refund,
                       (int)Apollo.Age.Enumerations.BetStatus.Draw,
                       (int)Apollo.Age.Enumerations.BetStatus.Won,
                       (int)Apollo.Age.Enumerations.BetStatus.Lose);
            }
            else
            {
                statusIds = string.Format("{0}^{1}",
                   (int)Apollo.Age.Enumerations.BetStatus.Running,
                   (int)Apollo.Age.Enumerations.BetStatus.Waiting);
            }

            if (Request["custid"] != null && Request["username"] != null)
            {
                int.TryParse(Request["custid"], out custid);
                username = Request["username"];
            }

            var reportService = new ReportService();
            balanceUpDay = reportService.GetBalanceUpDay();
            moveBetsDay = reportService.MoveBetsDay();
            fromDate = GetDateTimeRequest("fromDate", moveBetsDay);
            toDate = GetDateTimeRequest("toDate", DateTime.Now);
            mainShareHolderId = GetStringRequest("mainShareHolderId", string.Empty);
            mainShareHolderUsername = GetStringRequest("mainShareHolderUsername", string.Empty);
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            TicketResult mainTickets = new TicketResult();
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                AgentId = -1,
                LoggedId = CurrentSession.AgentId,
                CustId = custid,
                FromDate = fromDate,
                ToDate = toDate,
                StatusIds = statusIds,
                BalanceUpDay = balanceUpDay,
                MoveBetsDay = moveBetsDay
            };
            if (pageType == "report")
            {
                mainTickets = betListService.GetWinLossByDateAndMem(criteria);
            }
            else
            {
                mainTickets = betListService.GetAllBetsByDateAndMem(criteria);
            }
            return mainTickets;
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (string.IsNullOrEmpty(Request["custid"]) && string.IsNullOrEmpty(Request["username"]))
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }
            return null;
        }

        #endregion Override

        protected ICurrentSession CurrentSession
        {
            get
            {
                ICurrentSession currentSession = (ICurrentSession)Session["Apollo.Age.Web.CurrentSession"];

                return currentSession;
            }
            set
            {
                if (value is ICurrentSession)
                {
                    Session["Apollo.Age.Web.CurrentSession"] = value;
                }
            }
        }
    }
}