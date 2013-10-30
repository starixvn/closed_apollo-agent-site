// ---------------------------------------------------------------------------------
// <copyright file="AgentPT1.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// Build AgentPT.
    /// </summary>
    public class AgentPT1 : IAgentPT
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentPT1" /> class.
        /// </summary>
        public AgentPT1()
        {
            this.Template = new AgentPT_Template();
        }
        
        public AgentPT_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public virtual AgentPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            Template.AgentPT.agentPT_setting = string.Join(null, new string[] { ConvertByBetType.Pt(ticket.APositionTaking), "%" });

            string status = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();

            if (status.Equals(BetStatus.Running) || status.Equals(BetStatus.Waiting))
            {
                Template.AgentPTValue.Visible = false;
            }
            else
            {
                Template.AgentPTValue.agentPT_value = ConvertByBetType.PtValue(ticket.AWinlost);
            }

            return Template;
        }
    }
}