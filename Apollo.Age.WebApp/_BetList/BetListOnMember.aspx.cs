// ---------------------------------------------------------------------------------
// <copyright file="BetListOnMember.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.04.10">Create</change>
//     <change who="Kaden.Trinh" date="2013.04.10">Implement betlist Outstanding</change>
//     <change who="Kaden.Trinh" date="2013.04.16">Implement betlist Account Info</change>
//     <change who="Kaden.Trinh" date="2013.05.20">Implement Navigation bar</change>
//     <change who="Kaden.Trinh" date="2013.05.31">Change rule: we have to get date from after movebetday 1 day</change>
//   <change who="Anders.Le" date="2013.6.12">Remove unused variables</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.Forecast.Services;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Resources;
    using Apollo.Age.Web;
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

    public partial class BetListOnMember : TplPage
    {
        #region Variables

        private int custid;
        private string username = string.Empty;
        private DateTime fromDate, toDate, balanceUpDay, moveBetsDay, minDate;
        private string statusIds = string.Empty;
        private string pageType = string.Empty;
        private string mainShareHolderId, mainShareHolderUsername;
        ReportService reportService = new ReportService();

        #endregion Variables

        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            #region INIT

            var tpl = new BetListOnMember_Template();

            if (pageType.Equals("accountinfo"))
            {
                var maxDate = DateTime.Now;
                tpl.HiddenFields.username = username;
                tpl.HiddenFields.custId = custid.ToString();
                tpl.Filter.valueFromDate = fromDate.ToString("yyyy-M-dd");
                tpl.Filter.valueToDate = toDate.ToString("yyyy-M-dd");
                tpl.Filter.valueMaxDate = maxDate.ToShortDateString();
                tpl.Filter.valueMinDate = moveBetsDay.ToShortDateString();
                tpl.HiddenFields.DateTime.Hide();
                tpl.title = Apollo.Age.Resources.BetList.lblMemberBetList + " - " + username;
                tpl.Navigation = string.Empty;
            }
            else
            {
                if (pageType.Equals("report"))
                {
                    tpl.title = string.Format("{0} {1} - {2}", Apollo.Age.Resources.BetList.lblMemberBetList, fromDate.ToString("yyyy-mm-dd"), toDate.ToString("yyyy-mm-dd"));
                }
                else
                {
                    tpl.title = Apollo.Age.Resources.BetList.lblMemberBetList;
                }

                #region Navigation Link

                tpl.Navigation = this.Navigation(custid);

                #endregion Navigation Link

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

            Fanex.BetList.Age.ExtTemplate.OpenBetList_Template betlistOpenTag = new Fanex.BetList.Age.ExtTemplate.OpenBetList_Template();

            var forecastService = new ForecastService();
            var listBetType = forecastService.GetListScoreMapBetType();
            bool isShowScoremap;
           // bool isAdminCanViewVNIP = true;

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
                    .Append(string.Format("<tr id='{0}' {1}>", ticket.TransId, index % 2 == 0 ? string.Empty : "class='alt'"))
                    .AddTrans(ticket, ticketHelper, "<td class='c nonbreak'>", "</td>", funcGetOddsTypeName, BetListPage.Report)
                    .AddChoice(ticket, ticketHelper, ticketData, "<td class='r bl_evt'>", "</td>", isShowScoremap)
                    .AddOdds(ticket, ticketData, "<td class='bl_underdog nonbreak'>", "</td>", funcGetOddsTypeName)
                    .AddStake(ticket, "<td class='bl_underdog'>", "</td>");

                builder.AddWinLoss(ticket, "<td class='r nonbreak'>", "</td>");
                builder.AddAgentPT(ticket, ticketData, "<td class='r'>", "</td>");
                builder.AddAgentComm(ticket, ticketData, false, "<td class='r'>", "</td>");
                builder.Append("</tr>");
            }

            if (pageType.Equals("report") || pageType.Equals("accountinfo"))
            {
                builder.AddFooter(tickets, string.Empty, string.Empty, BetListType.Winlost);
            }
            else
            {
                builder.AddFooter(tickets, string.Empty, string.Empty, BetListType.Member);
            }

            content.Append(builder.GetBetList());
            content.Append("</tbody></table>");

            tpl.content = content.ToString();
            tpl.HiddenFields.pageType = pageType;
            tpl.HiddenFields.ApoRoot = Page.ResolveUrl("~/");
            tpl.HiddenFields.MainShareHolderId = mainShareHolderId;
            tpl.HiddenFields.MainShareHolderUsername = mainShareHolderUsername;

            writer.Write(tpl);
            RegisterJavascriptObject("titleViewComm", BetList.viewCommDetails);
            RegisterJavascriptObject("refNo", BetList.refno);
            RegisterJavascriptObject("titleScoreMap", BetList.scoreMap.ToUpper());
        }

        public override void ProcessRequest(ref System.Collections.Hashtable hash)
        {
            pageType = GetStringRequest("type", string.Empty).ToLower();
            if (pageType.Equals("accountinfo"))
            {
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
            else if (pageType.Equals("report"))
            {
                statusIds = string.Format("{0},{1},{2},{3},{4},{5}",
                       (int)Apollo.Age.Enumerations.BetStatus.Void,
                       (int)Apollo.Age.Enumerations.BetStatus.Reject,
                       (int)Apollo.Age.Enumerations.BetStatus.Refund,
                       (int)Apollo.Age.Enumerations.BetStatus.Draw,
                       (int)Apollo.Age.Enumerations.BetStatus.Won,
                       (int)Apollo.Age.Enumerations.BetStatus.Lose);
            }
            else
            {
                statusIds = string.Format("{0},{1}",
                   (int)Apollo.Age.Enumerations.BetStatus.Running,
                   (int)Apollo.Age.Enumerations.BetStatus.Waiting);
            }

            int.TryParse(Request["custid"], out custid);

            username = GetStringRequest("username", string.Empty);
            balanceUpDay = reportService.GetBalanceUpDay();
            moveBetsDay = reportService.MoveBetsDay();
            minDate = reportService.MoveBetsDay().AddDays(1);
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
                FromDate = ResetStartDate(fromDate),
                ToDate = ResetEndDate(toDate),
                StatusIds = statusIds,
                BalanceUpDay = balanceUpDay,
                MoveBetsDay = moveBetsDay
            };

            if (pageType.Equals("report"))
            {
                mainTickets = betListService.GetWinLossByDateAndMem(criteria);
            }
            else
            {
                mainTickets = betListService.GetTicketResult(criteria);
            }

            return mainTickets;
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            var maxDate = DateTime.Now;

            if (string.IsNullOrEmpty(Request["custid"]) && string.IsNullOrEmpty(Request["username"]))
            {
                return new ErrorPage_Page(Apollo.Age.Resources.Lang.strNotAccessPage);
            }

            if (pageType.Equals("accountinfo") && (DateTime.Parse(fromDate.ToShortDateString()) < DateTime.Parse(minDate.ToShortDateString())
                || DateTime.Parse(toDate.ToShortDateString()) < DateTime.Parse(minDate.ToShortDateString())
                || DateTime.Parse(toDate.ToShortDateString()) > DateTime.Parse(maxDate.ToShortDateString())))
            {
                return new ErrorPage_Page(Apollo.Age.Resources.Lang.strNotAccessPage);
            }

            return null;
        }

        #endregion Override

        protected ICurrentSession CurrentSession
        {
            get
            {
                return SessionManager.CurrentSession;
            }
        }

        /// <summary>
        /// Get Navigation link.
        /// </summary>
        /// <param name="agentId">Agent Id.</param>
        /// <returns></returns>
        private string Navigation(int agentId)
        {
            var betListService = new BetListService();
            var listAgent = betListService.GetAgentUpLineOfMember(CurrentSession.AgentId, agentId) as List<Apollo.Age.Entities.Agent>;
            var navigation = new Navigation_Template();
            navigation.MemberUser.Hide();
            navigation.AgentUser.Hide();
            var link = string.Empty;

            foreach (var item in listAgent)
            {
                var last = listAgent.LastOrDefault();
                if (last != null && !last.Equals(item))
                {
                    var li = new Navigation_AgentUser_Block();
                    li.Level = item.Level.ToString();
                    li.Id = item.Id.ToString();
                    li.Username = item.Username;
                    link += li.ToString();
                }
                else
                {
                    var li = new Navigation_MemberUser_Block();
                    li.Username = item.Username;
                    link += li.ToString();
                }
            }

            navigation.item = link;
            return navigation.ToString();
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