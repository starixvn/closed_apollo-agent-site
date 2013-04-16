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
    using System.Text;
    using System.Web.UI;
    using Fanex.Age.UI.Components;
    using Fanex.Age.UI.Reports;
    using Fanex.Age.UI.Reports.BetList;
    using Fanex.Age.Utility;
    using Fanex.BetList.Age.BetListCmd;
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
    using Fanex.BetList.Age.Definition;
    using Fanex.BetList.Age.DTO;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Sunplus.Agent.Model.BL.BetList;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.Agent.Model.BL.Reports;
    using Sunplus.Agent.Model.Entities;

    public partial class BetListTransaction : WinlossReports_Page
    {
        #region Variables

        private DataTable dtResult = null;
        private BetListBO betList = null;
        private DataTable outDataTable = null;

        private string title = string.Empty;
        private BetListTransaction_Template tpl = new BetListTransaction_Template();
        private IBetListCmd command = null;

        private string username = string.Empty;
        private DateTime dateFrom, dateTo;
        private int agentId, custId;
        private bool userType = false;
        private string pageType = string.Empty;
        private string statusIds = string.Empty;
        private bool isShowAgentPT = true;
        private bool isShowWinLost = true;

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
                ticketHelper.Index = index;
                builder.Append(string.Format("<tr id='{0}'>", ticket.TransId))
                       .AddNo(index, "<td class='w-order'>", "</td>")
                       .AddMember(ticket, "<td class='c'>", "</td>")
                       .AddTrans(ticket, "<td class='c'>", "</td>")
                       .AddChoice(ticket, ticketHelper, ticketData, "<td class='r bl_evt'><div>", "</div></td>", false)
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
            writer.Write(tpl);

            #endregion RENDER HTML
        }

        private void BoxFilter()
        {
            var selectedUserType = "0";
            var maxDate = DateTime.Now;

            if (userType)
            {
                selectedUserType = "1";
            }
            tpl.Filter.User.option_user = HtmlGenerator.ComboBox("userType", selectedUserType, this.CreateUserTypeOption(), "value", "text", null, 72);
            tpl.Filter.valueFromDate = ReportsBO.MoveBetsDate.ToShortDateString();

            tpl.Filter.valueToDate = dateTo.ToShortDateString();
            tpl.Filter.valueMaxDate = maxDate.ToShortDateString();
            tpl.Filter.valueMinDate = maxDate.AddDays(1 - maxDate.Day).AddMonths(-3).ToShortDateString();
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
            row["text"] = Resources.Lang.agent;
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["value"] = "0";
            row["text"] = Resources.Lang.member;
            dt.Rows.Add(row);

            return dt;
        }

        public override object InvokeData(Hashtable hash)
        {
            int errorCode;
            betList = new BetListBO(CurrentUserSession.NetSessionId);
            betList.GetUserIdByUserName(CurrentUserSession.CustId, userType, username, out agentId, out custId, out errorCode);
            if (errorCode == 0)
            {
                if (pageType == "transaction")
                {
                    dtResult = betList.GetAllBetsTransactionByDateAndMem(agentId, custId, dateFrom, dateTo, ReportsBO.MoveBetsDate, statusIds, out outDataTable);
                }
                else
                {
                    dtResult = betList.GetAllBetsByDateAndMem(agentId, custId, dateFrom, dateTo, statusIds, out outDataTable);
                }
            }
            return dtResult == null ? new DataTable() : dtResult;
        }

        public override void ProcessRequest(ref Hashtable hash)
        {
            pageType = Request["type"];

            if (pageType == "cancel")
            {
                tpl.title = Resources.Lang.voidandreject;
                isShowAgentPT = isShowWinLost = false;
                statusIds = string.Format("{0}^{1}", (int)Apollo.Age.Enumerations.BetStatus.Void, (int)Apollo.Age.Enumerations.BetStatus.Reject);
            }
            else if (pageType == "transaction")
            {
                tpl.title = Resources.Lang.transaction;
                isShowWinLost = true;
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

            dateFrom = GetDateTimeRequest("fromDate", ReportsBO.MoveBetsDate);
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
            if (string.IsNullOrEmpty(Request["type"]))
            {
                return new ErrorPage_Page(Resources.Lang.strNotAccessPage);
            }
            return null;
        }
    }
}