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

namespace Fanex.BetList.Age.BetListHTMLBuilderExt.AgentPTBuilder
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Fanex.BetList.Age.BetListHTMLBuilderExt.AgentCommBuilder;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// Build AgentPT.
    /// </summary>
    public class AgentComm1 : IAgentComm
    {
        public AgentComm_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
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
                Template.AgentComm.agentComm_setting = (ticket.CommStatus == "1" ? string.Join(null, new string[] { "2x ", ConvertByBetType.Comm_Percentage(ticket.ADiscount) }) : string.Join(null, new string[] { ConvertByBetType.Comm_Percentage(ticket.ADiscount) })) + "%";
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentPT1" /> class.
        /// </summary>
        public AgentComm1()
        {
            this.Template = new AgentComm_Template();
        }
    }
}