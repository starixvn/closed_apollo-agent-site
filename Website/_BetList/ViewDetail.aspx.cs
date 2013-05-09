// ---------------------------------------------------------------------------------
// <copyright file="ViewDetail.aspx.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.05.08">View detail Agent Comm</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Sunplus.Agent.UI._BetList
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.UI;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Services;
    using Apollo.Age.Reports.Services;
    using Apollo.Age.Web;
    using Fanex.Age.UI;
    using Fanex.Age.UI.Components;
    using Fanex.Agent.UI._BetList;
    using Fanex.BetList.Age.BetListHTMLBuilderExt;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Factories;
    using Fanex.BetList.Core.TicketHelper;
    using Fanex.BetList.Core.Utils;

    public partial class ViewDetail_Page : TplPage
    {
        #region Define variable

        private Int64 transID = 0;
        private Int64 agentID = 0;
        private DateTime fromDate = DateTime.Now;
        private DateTime toDate = DateTime.Now;
        private bool isReportWinloss = false;
        private DateTime balanceUpDay, moveBetsDay;

        #endregion Define variable

        public override void RenderContent(HtmlTextWriter writer)
        {
            #region INIT

            HeaderViewDetail_Template header = new HeaderViewDetail_Template();
            int index = 0;
            bool isShowAgentPT = true;

            var tickets = View as IList<ITicket>;

            #endregion INIT

            #region RENDER HTML

            ViewDetail_Template tpl = new ViewDetail_Template();
            StringBuilder content = new StringBuilder();

            Fanex.BetList.Age.ExtTemplate.OpenBetList_Template betlistOpenTag = new Fanex.BetList.Age.ExtTemplate.OpenBetList_Template();
            content.Append(betlistOpenTag);

            content.Append(header);

            ITicketHelper ticketHelper = new TicketHelper();
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicket totalTicket = new Ticket();

            if (tickets.Count > 0)
            {
                content.Append("<tbody>");
                foreach (ITicket ticket in tickets)
                {
                    index++;
                    ticketHelper.Index = index;
                    builder.Append(string.Format("<tr {0}>", index % 2 == 0 ? string.Empty : "class='alt'"))
                        .AddMember(ticket, "<td class='l'>", "</td>");

                    if (isShowAgentPT)
                    {
                        builder.AddAgentPT(ticket, null, "<td class='r'>", "</td>");
                        builder.AddAgentComm(ticket, null, true, "<td class='r'>", "</td>");
                    }
                    builder.Append("</tr>");

                    #region Total ticket

                    totalTicket.AWinlost += ticket.AWinlost;
                    totalTicket.APositionTaking += Convert.ToDecimal(ConvertByBetType.Pt(ticket.APositionTaking)) / 100;

                    totalTicket.AComm += ticket.AComm;
                    totalTicket.ADiscount += Convert.ToDecimal(ConvertByBetType.Comm_Percentage(ticket.ADiscount)) / 100;
                    totalTicket.Status = ticket.Status;
                    totalTicket.AgentLevel = ticket.AgentLevel;

                    #endregion Total ticket
                }
                builder.Append("<tfoot><tr class='total'>");
                builder.Append(string.Format("<td class='c'>{0}</td>", Apollo.Age.Resources.Lang.total));

                if (isShowAgentPT)
                {
                    builder.AddAgentPT(totalTicket, null, "<td class='r'>", "</td>");
                    builder.AddAgentComm(totalTicket, null, true, "<td class='r'>", "</td>");
                }

                builder.Append("</tr></tfoot>");
                content.Append(builder.GetBetList());
                content.Append("</tbody></table>");
            }
            else
            {
                builder.Append("<tfoot><tr class='total no-result'>");
                builder.Append(string.Format("<td class='c' colspan='4'>{0}</td>", Apollo.Age.Resources.Lang.noInfo));
                builder.Append("</tr></tfoot>");
                content.Append(builder.GetBetList());
                content.Append("</table>");
            }

            tpl.content = content.ToString();
            writer.Write(tpl);

            #endregion RENDER HTML
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            var reportService = new ReportService();
            balanceUpDay = reportService.GetBalanceUpDay();
            moveBetsDay = reportService.MoveBetsDay();

            Int64.TryParse(Request["transId"], out transID);
            fromDate = GetDateTimeRequest("fromDate", moveBetsDay);
            toDate = GetDateTimeRequest("toDate", DateTime.Now);
            isReportWinloss = GetStringRequest("type", string.Empty) == "report" ? true : false;
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (transID == 0) this.RenderNoInfo();
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            var betListService = new BetListService();
            var criteria = new BetListCriteria
            {
                LoggedId = CurrentSession.AgentId,
                TransId = transID,
                FromDate = Formater.ResetStartDate(fromDate),
                ToDate = Formater.ResetEndDate(toDate),
                BalanceUpDay = balanceUpDay,
                MoveBetsDay = moveBetsDay
            };

            if (isReportWinloss)
            {
                return betListService.GetDetailWinLossAgentComm(criteria);
            }
            return betListService.GetDetailAgentComm(criteria);
        }

        protected ICurrentSession CurrentSession
        {
            get
            {
                ICurrentSession currentSession = (ICurrentSession)Session["Apollo.Age.Web.CurrentSession"];

                return currentSession;
            }
        }

        private object RenderNoInfo()
        {
            return new ErrorPage_Page(Apollo.Age.Resources.Lang.noInfo);
        }
    }
}