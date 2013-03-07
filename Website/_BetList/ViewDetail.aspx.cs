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
using Sunplus.Agent.Model.BL.BetList;
using Sunplus.Agent.Model.BL.Reports;
using Sunplus.Agent.Model.Entities;

namespace Sunplus.Agent.UI._BetList
{
    public partial class ViewDetail_Page : Fanex.Age.UI.TplPage
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

            BetListType betlistType = command.BetListType;

            bool isShowAgentPT = true;

            if ((betlistType & BetListType.Canceled) == BetListType.Canceled)
            {
                isShowAgentPT = false;
            }
            else if ((betlistType & BetListType.DailyStatement) == BetListType.DailyStatement)
            {
                isShowAgentPT = false;
            }

            DataTable betListData = View as DataTable;
            IList<ITicket> tickets = new List<ITicket>();

            if (betListData != null)
            {
                tickets = TicketTransformer.Transform(betListData);
                betListData.Dispose();
            }

            DataTable commandData = command.OutDataTable();
            List<ITicketData> ticketData = new List<ITicketData>();

            if (commandData != null)
            {
                ticketData = TicketDataTransformer.Transform(commandData);
                commandData.Dispose();
            }

            #endregion INIT

            Header1_Template betlistHeader = new Header1_Template();

            int index = 0;

            #region RENDER HTML

            BetListTemplate tpl = new BetListTemplate();
            StringBuilder content = new StringBuilder();

            tpl.title = "View detail";

            Fanex.BetList.Age.ExtTemplate.OpenBetList_Template betlistOpenTag = new Fanex.BetList.Age.ExtTemplate.OpenBetList_Template();
            content.Append(betlistOpenTag);

            content.Append(betlistHeader);
            content.Append("<tbody>");

            ITicketHelper ticketHelper = new TicketHelper();
            BetListHTMLFactory factory = new BetListHTMLFactory();
            BetListHTMLBuilder builder = factory.CreateBuilder();

            foreach (ITicket ticket in tickets)
            {
                index++;
                ticketHelper.Index = index;

                builder.Append("<tr>")
                    .AddMember(ticket, "<td class='c'>", "</td>")
                    .AddNo(index, "<td class='c'>", "</td>");

                if (isShowAgentPT)
                {
                    builder.AddAgentPT(ticket, ticketData, "<td class='r'>", "</td>");
                    builder.AddAgentComm(ticket, ticketData, "<td class='r'>", "</td>");
                }
                builder.Append("</tr>");
            }

            builder.AddFooter(tickets, string.Empty, string.Empty, (Fanex.BetList.Age.Definition.BetListType)betlistType);

            content.Append(builder.GetBetList());
            content.Append("</tbody></table>");

            tpl.content = content.ToString();
            tpl.ToString(writer);

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
            BetListBO bo = new BetListBO(CurrentUserSession.NetSessionId);
            if (isSportsbook)
            {
                return bo.GetAgentPTComm(transID, agentID, fromDate, toDate, ReportsBO.MoveBetsDate, true, false);
            }
            else
            {
                return bo.GetAgentPTComm(transID, agentID, fromDate, toDate, ReportsBO.MoveBetsDate, false, true);
            }
        }

        private object RenderNoInfo()
        {
            return new ErrorPage_Page(Resources.Lang.noInfo);
        }
    }
}