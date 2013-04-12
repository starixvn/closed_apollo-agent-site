// ---------------------------------------------------------------------------------
// <copyright file="BetList.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.01.12">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.Age.UI.Reports.BetList
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
    using Fanex.BetList.Age.BetListCmd;
    using Fanex.BetList.Age.BetListExcelBuilderExt;
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
    using Fanex.BetList.Age.DTO;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using NPOI.SS.UserModel;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public partial class BetList : TplPage
    {
        #region Variables

        private DataTable dtResult = null;
        private BetListBO betList = null;
        private DataTable outDataTable = null;

        private string title = string.Empty;
        private CancelBetList_Template tpl = new CancelBetList_Template();
        private IBetListCmd command = null;

        private string username = string.Empty;
        private DateTime dateFrom, dateTo;
        private int agentId, custId;
        private bool userType = false;
        private string statusIds = string.Empty;

        #endregion Variables

        public override void RenderContent(HtmlTextWriter writer)
        {
            #region INIT

            this.BoxFilter();
            Dictionary<string, DataTable> results = View as Dictionary<string, DataTable>;
            UserTypes userType = UserTypes.Unknown;

            DataTable betListData = View as DataTable;
            IList<ITicket> tickets = TicketTransformer.Transform(betListData);
            betListData.Dispose();

            DataTable betTransm = outDataTable == null ? new DataTable() : outDataTable;
            List<ITicketData> ticketData = TicketDataTransformer.Transform(betTransm);
            betTransm.Dispose();

            int index = 0;
            bool isAdminCanViewVNIP = UserSession.Current.IsAdminCanViewVNIP;
            GetCachePropertyById funcGetOddsTypeName = CacheRef.GetOddsTypeName;
            ITicketHelper ticketHelper = new TicketHelper();

            #endregion INIT

            #region RENDER HTML

            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();

            StringBuilder content = new StringBuilder();

            Fanex.BetList.Age.ExtTemplate.OpenBetList_Template betlistOpenTag = new Fanex.BetList.Age.ExtTemplate.OpenBetList_Template();
            Header1_Template betlistHeader = new Header1_Template();
            betlistHeader.Winloss.Hide();

            content.Append(betlistOpenTag);
            content.Append(betlistHeader);
            content.Append("<tbody>");

            foreach (ITicket ticket in tickets)
            {
                index++;
                ticketHelper.Index = index;
                builder.Append(string.Format("<tr id='{0}'>", ticket.TransId))
                       .AddNo(index, "<td class='w-order'>", "</td>")
                       .AddMember(ticket, "<td class='c'>", "</td>")
                       .AddTrans(ticket, "<td class='c'>", "</td>")
                       .AddChoice(ticket, ticketHelper, ticketData, "<td class='r bl_evt'><div>", "</div></td>", false)
                       .AddOdds(ticket, ticketData, "<td class='bl_underdog'>", "</td>", funcGetOddsTypeName)
                       .AddStake(ticket, "<td class='bl_underdog'>", "</td>")
                       .AddStatus(ticket, ticketHelper, ticketData, "<td class='c'>", "</td>", isAdminCanViewVNIP)
                       .Append("</tr>");
            }

            content.Append(builder.GetBetList());
            content.Append("</tbody></table>");

            tpl.content = content.ToString();
            tpl.HiddenFields.ApoRoot = Page.ResolveUrl("~/");
            writer.Write(tpl);

            #endregion RENDER HTML
        }

        private void BoxFilter()
        {
            tpl.Filter.valueFromDate = dateFrom.ToShortDateString();
            tpl.Filter.valueToDate = dateTo.ToShortDateString();
            tpl.Filter.valueMinDate = DateTime.Now.AddMonths(-3).ToShortDateString();
            tpl.Filter.valueMaxDate = DateTime.Now.AddMonths(1).ToShortDateString();
            tpl.Filter.username = username;
        }

        public override object InvokeData(Hashtable hash)
        {
            betList = new BetListBO(CurrentUserSession.NetSessionId);
            betList.GetUserIdByUserName(CurrentUserSession.CustId, userType, username, out agentId, out custId);
            dtResult = betList.GetAllBetsByDateAndMem(agentId, custId, dateFrom, dateTo, statusIds, out outDataTable);
            return dtResult != null ? dtResult : new DataTable();
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            statusIds = string.Format("{0}^{1}", (int)Apollo.Age.Enumerations.BetStatus.Void, (int)Apollo.Age.Enumerations.BetStatus.Reject);
            dateFrom = GetDateTimeRequest("fromDate", DateTime.Now.AddDays(-3));
            dateTo = GetDateTimeRequest("toDate", DateTime.Now);
            username = this.GetStringRequest("username", UserSession.Current.AgentName);
            userType = GetBooleanRequest("userType", true);
        }

        private string GetStringRequest(string name, string defaultValue)
        {
            if (Request[name] == null)
            {
                return defaultValue;
            }
            return Request[name];
        }

        public override object ValidateRequest(Hashtable hash)
        {
            return null;
        }
    }
}