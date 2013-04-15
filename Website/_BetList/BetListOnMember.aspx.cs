// ---------------------------------------------------------------------------------
// <copyright file="BetListOnMember.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.04.10">Create</change>
//     <change who="Kaden.Trinh" date="2013.04.10">Implement betlist Outstanding</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Reports;
    using Fanex.Age.UI.Reports.BetList;
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
    using Fanex.BetList.Age.Definition;
    using Fanex.BetList.Age.DTO;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Forecast;
    using Sunplus.Agent.Model.BL.Reports;
    using Sunplus.Agent.Model.Entities;

    public partial class BetListOnMember : WinlossReports_Page
    {
        #region Variables

        private DataTable dtResult = null;
        private BetListBO betList = null;
        private DataTable outDataTable = null;

        private int custid;
        private string username = string.Empty;
        private DateTime dateFrom, dateTo;
        private string statusIds = "0^1"; // running, waiting

        #endregion Variables

        #region Override

        public override void RenderContent(HtmlTextWriter writer)
        {
            #region INIT

            var tpl = new OutstandingDetail_Template();
            tpl.title = Resources.Lang.lblBetLists + " - " + username;

            // Render Content
            DataTable betListData = View as DataTable;
            IList<ITicket> tickets = TicketTransformer.Transform(betListData);
            betListData.Dispose();

            StringBuilder content = new StringBuilder();

            DataTable betTransm = outDataTable == null ? new DataTable() : outDataTable;
            List<ITicketData> ticketData = TicketDataTransformer.Transform(betTransm);
            betTransm.Dispose();

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            TicketHelper ticketHelper = new TicketHelper();
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;

            Header1_Template betlistHeader = new Header1_Template();
            betlistHeader.Member.Hide();

            Fanex.BetList.Age.ExtTemplate.OpenBetList_Template betlistOpenTag = new Fanex.BetList.Age.ExtTemplate.OpenBetList_Template();

            ForecastBO bo = new ForecastBO(UserSession.Current.NetSessionId);
            List<int> lstScoremap = bo.GetListBetTypeScoreMap();

            bool isShowScoremap;
            bool isAdminCanViewVNIP = UserSession.Current.IsAdminCanViewVNIP;

            #endregion INIT

            builder.Append(betlistOpenTag);
            builder.Append(betlistHeader);

            int index = 0;

            foreach (ITicket ticket in tickets)
            {
                if (lstScoremap.Contains(ticket.BetTypeId) && ticket.Status.Equals(Fanex.BetList.Core.Entities.BetStatus.Running))
                {
                    isShowScoremap = true;
                }
                else
                {
                    isShowScoremap = false;
                }

                ticketHelper.Index = index++;
                builder
                    .Append(string.Format("<tr id='r{0}'>", ticket.TransId))
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

            builder.Append("</tbody></table>");

            tpl.content = builder.GetBetList();
            tpl.HiddenFields.ApoRoot = Page.ResolveUrl("~/");
            writer.Write(tpl);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable hash)
        {
            base.ProcessRequest(ref hash);

            if (Request["custid"] != null && Request["username"] != null)
            {
                int.TryParse(Request["custid"], out custid);
                username = Request["username"];
            }

            dateFrom = ReportsBO.MoveBetsDate;
            dateTo = DateTime.Now;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            betList = new BetListBO(CurrentUserSession.NetSessionId);
            dtResult = betList.GetAllBetsByDateAndMem(-1, custid, dateFrom, dateTo, statusIds, out outDataTable);
            return dtResult != null ? dtResult : new DataTable();
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
    }
}