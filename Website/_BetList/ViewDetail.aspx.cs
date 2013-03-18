using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Age.UI.Components;
using Fanex.BetList.Age.BetListCmd;
using Fanex.BetList.Age.BetListHTMLBuilderExt;
using Fanex.BetList.Age.Definition;
using Fanex.BetList.Age.DTO;
using Fanex.BetList.Age.ExtTemplate;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;
using Fanex.BetList.Core.Factories;
using Fanex.BetList.Core.TicketHelper;
using Fanex.BetList.Core.Utils;
using Sunplus.Agent.Model.BL.BetList;
using Sunplus.Agent.Model.BL.Reports;
using Sunplus.Agent.Model.Entities;
using Fanex.Agent.UI._BetList;

namespace Sunplus.Agent.UI._BetList
{
    public partial class ViewDetail_Page : Fanex.Age.UI.Reports.WinlossReports_Page
    {
        #region Define variable

        private Int64 transID = 0;
        private Int64 agentID = 0;
        private DateTime fromDate = DateTime.Now;
        private DateTime toDate = DateTime.Now;
        private bool isSportsbook = false;
        IBetListCmd command = null;

        #endregion Define variable

        public override void RenderContent(HtmlTextWriter writer)
        {
            #region INIT

            bool isShowAgentPT = true;

            DataTable betListData = View as DataTable;
            IList<ITicket> tickets = new List<ITicket>();

            if (betListData != null)
            {
                tickets = TicketTransformer.Transform(betListData);
                betListData.Dispose();
            }

            #endregion INIT

            HeaderViewDetail_Template header = new HeaderViewDetail_Template();

            int index = 0;

            #region RENDER HTML

            ViewDetail_Template tpl = new ViewDetail_Template();
            StringBuilder content = new StringBuilder();

            Fanex.BetList.Age.ExtTemplate.OpenBetList_Template betlistOpenTag = new Fanex.BetList.Age.ExtTemplate.OpenBetList_Template();
            content.Append(betlistOpenTag);

            content.Append(header);
            content.Append("<tbody>");

            ITicketHelper ticketHelper = new TicketHelper();
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();
            ITicket totalTicket = new Ticket();
            foreach (ITicket ticket in tickets)
            {
                index++;
                ticketHelper.Index = index;

                builder.Append("<tr>")
                    .AddMember(ticket, "<td class='c' style='vertical-align:middle'>", "</td>")
                    .AddNo(ticket.AgentLevel, "<td class='c' style='vertical-align:middle'>", "</td>");

                if (isShowAgentPT)
                {
                    builder.AddAgentPT(ticket, null, "<td class='r'>", "</td>");
                    builder.AddAgentComm(ticket, null, true, "<td class='r'>", "</td>");
                }
                builder.Append("</tr>");

                #region Total ticket

                totalTicket.Comm += ticket.Comm;
                totalTicket.TComm += ticket.TComm;
                totalTicket.TPositionTaking += ticket.TPositionTaking;
                totalTicket.AgentWinlost += ticket.AgentWinlost;
                totalTicket.Status = ticket.Status;

                #endregion Total ticket
            }

            builder.Append("<tr>");
            builder.Append(string.Format("<td class='r underdog' style='vertical-align:middle' colspan='2'>{0}</td>", Resources.Lang.total));

            if (isShowAgentPT)
            {
                builder.AddAgentPT(totalTicket, null, "<td class='r'>", "</td>");
                builder.AddAgentComm(totalTicket, null, true, "<td class='r'>", "</td>");
            }

            builder.Append("</tr>");
            content.Append(builder.GetBetList());
            content.Append("</tbody></table>");

            tpl.content = content.ToString();
            writer.Write(tpl);

            #endregion RENDER HTML
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            Int64.TryParse(Request["transId"], out transID);
            Int64.TryParse(Request["agentId"], out agentID);
            DateTime.TryParse(Request["fromDate"], out fromDate);
            DateTime.TryParse(Request["toDate"], out toDate);
            bool.TryParse(Request["isSportsbook"], out isSportsbook);
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (transID == 0) this.RenderNoInfo();
            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            try
            {
                BetListBO bo = new BetListBO(CurrentUserSession.NetSessionId);
                return bo.GetAgentPTComm(transID, agentID, fromDate, toDate, ReportsBO.MoveBetsDate, true, false);
            }
            catch (Exception ex) { 
                //writelog
            }

            return null;
        }

        private object RenderNoInfo()
        {
            return new ErrorPage_Page(Resources.Lang.noInfo);
        }
    }
}