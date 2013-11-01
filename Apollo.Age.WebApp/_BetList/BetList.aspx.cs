// ---------------------------------------------------------------------------------
// <copyright file="BetList.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//      <change who="Kaden.Trinh" date="2013.01.12">Create</change>
//      <change who="Kaden.Trinh" date="2013.06.26">Change for new requirement</change>
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
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Resources;
    using Apollo.Age.TotalBets.Services;
    using Apollo.Age.Web;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
    using Fanex.BetList.Age.Definition;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;

    /// <summary>
    /// Bet list page.
    /// </summary>
    public partial class BetList_Page : TplPage
    {
        #region Variables

        private string type = string.Empty;
        private string title = string.Empty;
        private int matchId, live;
        private string bettype, betTeam;
        private string mainShareHolderUsername;
        private BetListTemplate tpl = new BetListTemplate();
        private TotalBetsService totalBetsService = new TotalBetsService();
        private IEnumerable<Agent> mainShareHolders;
        private int loggedId, mainShareHolderId;
        private bool isForecast = false;

        #endregion Variables

        protected ICurrentSession CurrentSession
        {
            get
            {
                return SessionManager.CurrentSession;
            }
        }

        public override void RenderContent(HtmlTextWriter writer)
        {
            bool isShowAgentPT = true;

            var ticketResult = View as TicketResult;
            var tickets = ticketResult.TicketList;
            var ticketData = ticketResult.TicketDataList;

            // Customize
            Header1_Template betlistHeader = new Header1_Template();
            betlistHeader.headerTrans = BetList.ticket;
            betlistHeader.headerMember = BetList.rebate;

            if (!isShowAgentPT)
            {
                betlistHeader.Agent.Hide();
            }

            betlistHeader.Winloss.Hide();

            // Render HTML
            var content = new StringBuilder();

            content.Append(betlistHeader);
            content.Append("<tbody>");

            int index = 0;
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            var ticketHelper = new TicketHelper();
            var factory = new BetListHTMLFactory();
            var builder = factory.CreateBuilder();

            foreach (ITicket ticket in tickets)
            {
                ticketHelper.Index = ++index;
                ticket.AgentLevel = Convert.ToInt32(CurrentSession.AgentLevel);

                builder.Append(string.Format("<tr id='{0}' {1}>", ticket.TransId, index % 2 == 0 ? string.Empty : "class='alt'"));

                builder.AddTime(ticket, "<td class='c'>", "</td>", BetListPage.Forecast);
                builder.AddDiscount(ticket, "<td class='c'>", "</td>", BetListPage.Forecast);

                builder.AddTrans(ticket, ticketHelper, "<td class='c'>", "</td>", funcGetOddsTypeName, BetListPage.Forecast)
                       .AddChoice(ticket, ticketHelper, ticketData, "<td class='bl_evt'>", "</td>", false)
                       .AddStake(ticket, "<td class='bl_underdog'>", "</td>");

                builder.Append("</tr>");
            }

            builder.AddFooter(tickets, string.Empty, string.Empty, BetListType.Member);
            content.Append(builder.GetBetList());
            content.Append("</tbody>");

            tpl.title = title;
            tpl.content = content.ToString();
            tpl.HiddenFields.MainShareHolderId = mainShareHolderId.ToString();
            tpl.HiddenFields.MainShareHolderUsername = mainShareHolderUsername;
            tpl.HiddenFields.ApoRoot = Page.ResolveUrl("~/");

            if (CurrentSession.IsSuper && !isForecast && mainShareHolders.Count() > 0)
            {
                this.BoxMainShareholder();
            }
            else
            {
                tpl.MainShareHolder.Hide();
            }

            writer.Write(tpl);
            RegisterJavascriptObject("titleViewComm", BetList.viewCommDetails);
            RegisterJavascriptObject("refNo", BetList.refno);
            RegisterJavascriptObject("titleScoreMap", BetList.scoreMap.ToUpper());
        }

        public override object InvokeData(Hashtable hash)
        {
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                LoggedId = loggedId,
                AgentId = mainShareHolderId,
                BetTypes = bettype,
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
            type = GetStringRequest("type", string.Empty).ToLower();
            bettype = GetStringRequest("bettype", string.Empty);
            betTeam = GetStringRequest("betteam", string.Empty);
            matchId = GetIntRequest("matchid", 0);
            mainShareHolderId = GetIntRequest("mainShareHolderId", 0);
            mainShareHolderUsername = GetStringRequest("mainShareHolderUsername", string.Empty);
            live = GetIntRequest("islive", 0);
            isForecast = GetBooleanRequest("isForecast", false);

            if (type.Equals("correctscore"))
            {
                title = CacheRef.GetBettype((int)BetTypes.Correct_Score).ToUpper();
                bettype = string.Format("{0},{1}", (int)BetTypes.Correct_Score, (int)BetTypes.FirstCorrectScore);
            }
            else if (type.Equals("mixparlay"))
            {
                bettype = Convert.ToString((int)BetTypes.MixParlay);
                title = CacheRef.GetBettype(bettype).ToUpper();
            }
            else if (type.Equals("combmixparlaybetlist"))
            {
                bettype = Convert.ToString((int)BetTypes.Combine_Mix_Parlay);
                title = Apollo.Age.Resources.Lang.lblSystemParlay.ToUpper();
            }
            else
            {
                if (Request["leagueName"] != null)
                {
                    title = string.Format("{0} - {1}", Request["leagueName"].ToUpper(), CacheRef.GetBettype(bettype).ToUpper());
                }
                else
                {
                    title = CacheRef.GetBettype(bettype).ToUpper();
                }
            }

            if (CurrentSession.IsSuper)
            {
                mainShareHolders = totalBetsService.GetData<Agent>(new GetAllMainShareHolderCriteria { Bettypes = bettype });
                if (mainShareHolderId == 0 && mainShareHolders.Count() > 0)
                {
                    mainShareHolderId = mainShareHolderId = mainShareHolders.ElementAt(0).Id;
                }
                else
                {
                    mainShareHolderId = Convert.ToInt32(mainShareHolderId);
                }

                loggedId = CurrentSession.AgentId;
            }
            else
            {
                loggedId = mainShareHolderId = CurrentSession.AgentId;
            }
        }

        public override object ValidateRequest(Hashtable p)
        {
            if (!string.IsNullOrEmpty(Request["custid"]))
            {
                if (int.Parse(Request["custid"].ToString()) <= 0)
                {
                    return new ErrorPage_Page(Apollo.Age.Resources.Lang.strNotAccessPage);
                }
            }

            if (!string.IsNullOrEmpty(Request["bettype"]))
            {
                if (int.Parse(Request["bettype"].ToString()) < 0)
                {
                    return new ErrorPage_Page(Apollo.Age.Resources.Lang.strNotAccessPage);
                }
            }

            return null;
        }

        #region Box Main Shareholder

        private DataTable CreateMainShareholderData()
        {
            var mainShareHolders = totalBetsService.GetData<Agent>(new GetAllMainShareHolderCriteria { Bettypes = bettype });

            DataTable dataTable = new DataTable();
            DataColumn column;
            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "value";
            dataTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "text";
            dataTable.Columns.Add(column);

            DataRow row;

            foreach (var mainShareHolder in mainShareHolders)
            {
                row = dataTable.NewRow();
                row["value"] = mainShareHolder.Id.ToString();
                row["text"] = mainShareHolder.Username;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        private void BoxMainShareholder()
        {
            tpl.MainShareHolder.option_agent = HtmlGenerator.ComboBox(
                "ddlMainShareHolder", 
                mainShareHolderId.ToString(), 
                this.CreateMainShareholderData(), 
                "value", 
                "text", 
                null, 
                280);
        }

        #endregion Box Main Shareholder
    }
}