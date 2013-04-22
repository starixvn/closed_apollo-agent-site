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
    using System.Text;
    using System.Web.UI;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Models;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Forecast.Services;
    using Apollo.Age.Web;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Reports;
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
    using Fanex.BetList.Age.Definition;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;

    public partial class BetList_Page : WinlossReports_Page
    {
        #region Variables

        private string type = string.Empty;
        private string title = string.Empty;
        private int custId, matchId, live;
        private string bettype, betTeam;
        private DateTime dateFrom = DateTime.Now;
        private DateTime dateTo = DateTime.Now;

        #endregion Variables

        public override void RenderContent(HtmlTextWriter writer)
        {
            #region INIT

            bool isShowWinLost = false,
                 isShowAgentPT = true,
                 isShowMember = true;

            var ticketResult = View as TicketResult;
            var tickets = ticketResult.TicketList;
            var ticketData = ticketResult.TicketDataList;

            #endregion INIT

            #region Customize

            if (type.Equals("MixParlay"))
            {
                title = string.Join(null, new string[]{Resources.Lang.lblMixParlay,
                                                         "&nbsp;|&nbsp; ",
                                                         "<a href=\"BetList.aspx?type=CombMixParlayBetList\">",
                                                         Resources.Lang.lblSystemParlay,
                                                         "</a>"});
            }
            else if (type.Equals("CombMixParlayBetList"))
            {
                title = string.Join(null, new string[]{ "<a href=\"BetList.aspx?type=MixParlay\">",
                                                         Resources.Lang.lblMixParlay,
                                                         "</a>",
                                                         "&nbsp;|&nbsp; ",
                                                         Resources.Lang.lblSystemParlay});
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

            #region RENDER HTML

            var tpl = new BetListTemplate { title = title };
            var content = new StringBuilder();

            content.Append(betlistHeader);
            content.Append("<tbody>");

            var forecastService = new ForecastService();
            var listBetType = forecastService.GetListScoreMapBetType();
            bool isShowScoremap;
            int index = 0;
            bool isAdminCanViewVNIP = CurrentSession.IsAdminCanViewVNIP;

            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            var ticketHelper = new TicketHelper();
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();

            foreach (ITicket ticket in tickets)
            {
                isShowScoremap = listBetType.Contains(ticket.BetTypeId) && ticket.Status.Equals(Fanex.BetList.Core.Entities.BetStatus.Running);

                ticketHelper.Index = ++index;

                builder.Append(string.Format("<tr id='{0}' {1}>", ticket.TransId, index % 2 == 0 ? string.Empty : "class='alt'"))
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

            builder.AddFooter(tickets, string.Empty, string.Empty, BetListType.Member);

            content.Append(builder.GetBetList());
            content.Append("</tbody>");

            tpl.content = content.ToString();
            tpl.HiddenFields.ApoRoot = Page.ResolveUrl("~/");
            writer.Write(tpl);

            RegisterJavascriptObject("type", type);
            RegisterJavascriptObject("fdate", dateFrom.ToShortDateString());
            RegisterJavascriptObject("tdate", dateTo.ToShortDateString());

            #endregion RENDER HTML
        }

        public override object InvokeData(Hashtable hash)
        {
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                LoggedId = CurrentSession.AgentId,
                Bettypes = bettype,
                MatchId = matchId,
                LiveIndicator = live,
                BetTeam = betTeam
            };

            TicketResult mainTickets;

            if (matchId > 0)
            {
                mainTickets = betListService.GetRunningByBettypeAndMatch(criteria);
            }
            else
            {
                mainTickets = betListService.GetRunningByBettype(criteria);
            }

            return mainTickets;
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            base.ProcessRequest(ref hash);

            type = Request["type"];
            bettype = GetStringRequest("bettype", string.Empty);
            betTeam = GetStringRequest("betteam", string.Empty);
            matchId = GetIntRequest("matchid", 0);
            live = GetIntRequest("islive", 0);

            if (type.ToLower().Equals("correctscore"))
            {
                title = CacheRef.GetBettype((int)BetTypes.Correct_Score).ToUpper();
                bettype = string.Format("{0}^{1}", (int)BetTypes.Correct_Score, (int)BetTypes.FirstCorrectScore);
            }
            else if (type.ToLower().Equals("mixparlay"))
            {
                bettype = Convert.ToString((int)BetTypes.Mix_Parlay);
                title = CacheRef.GetBettype(bettype).ToUpper();
            }
            else if (type.ToLower().Equals("combinationmixparlay"))
            {
                bettype = Convert.ToString((int)BetTypes.Combine_Mix_Parlay);
                title = CacheRef.GetBettype(bettype).ToUpper();
            }
            else
            {
                if (Request["leagueName"] != null)
                {
                    title =
                    title = string.Format("{0} - {1}", Request["leagueName"].ToUpper(), CacheRef.GetBettype(bettype).ToUpper());
                }
                else
                {
                    title = CacheRef.GetBettype(bettype).ToUpper();
                }
            }
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

            if (dateFrom < minDate || dateTo < minDate || dateTo > maxDate)
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }

            return null;
        }

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