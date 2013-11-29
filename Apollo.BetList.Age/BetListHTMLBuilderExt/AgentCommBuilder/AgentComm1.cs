// ---------------------------------------------------------------------------------
// <copyright file="AgentComm1.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.18">Create</change>
//     <change who="Marc.Bui" date="2013.01.28">Update interface to work with Win/Place AgentPT</change>
//     <change who="Marc.Bui" date="2013.02.20">Add % to comm setting</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Age.BetListHTMLBuilderExt.AgentPTBuilder
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Apollo.BetList.Age.BetListHTMLBuilderExt.AgentCommBuilder;
    using Apollo.BetList.Age.ExtTemplate;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Utils;

    /// <summary>
    /// Build AgentPT.
    /// </summary>
    public class AgentComm1 : IAgentComm
    {
        public AgentComm1()
        {
            this.Template = new AgentComm_Template();
        }

        public AgentComm_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="hideDetail">The hide detail.</param>
        /// <returns>Agent commission template.</returns>
        public virtual AgentComm_Template Render(ITicket ticket, List<ITicketData> ticketData, bool hideDetail = false)
        {
            string status = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();

            if (status.Equals(BetStatus.Running) || status.Equals(BetStatus.Waiting))
            {
                Template.AgentCommValue.Visible = false;
            }
            else
            {
                Template.AgentCommValue.agentComm_value = ConvertByBetType.PtValue(ticket.AComm);
            }

            if (ticket.AgentLevel == 0)
            {
                Template.AgentComm.agentComm_setting = Lang.lblNA;
            }
            else
            {
                if (ticket.CommStatus == "1")
                {
                    Template.AgentComm.agentComm_setting = string.Join(null, "2x ", ConvertByBetType.Comm_Percentage(ticket.ADiscount));
                }
                else
                {
                    Template.AgentComm.agentComm_setting = string.Join(null, ConvertByBetType.Comm_Percentage(ticket.ADiscount)) + "%";
                }
            }

            if (hideDetail)
            {
                Template.ViewDetail.Visible = false;
            }
            else
            {
                Template.ViewDetail.transId = ticket.TransId.ToString();
            }

            return Template;
        }
    }
}