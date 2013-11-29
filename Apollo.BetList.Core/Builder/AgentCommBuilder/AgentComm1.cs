// --------------------------------------------------------------------------
// <copyright file="AgentComm1.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.14">Get Agent Comm</change>
// </history>

namespace Apollo.BetList.Core.Builder.AgentCommBuilder
{
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using Apollo.BetList.Core.Utils;

    /// <summary>
    /// Agent commission.
    /// </summary>
    public class AgentComm1 : IAgentComm
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentComm1" /> class.
        /// </summary>
        public AgentComm1()
        {
            this.Template = new AgentComm_Template();
        }

        public AgentComm_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Agent commission template.</returns>
        public AgentComm_Template Render(ITicket ticket)
        {
            string status = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();

            if (status.ToLower().Equals(BetStatus.Won)
                || status.ToLower().Equals(BetStatus.Draw)
                || status.ToLower().Equals(BetStatus.Lose))
            {
                Template.AgentCommValue.agentComm_value = ConvertByBetType.PtValue(ticket.AComm);
            }
            else
            {
                Template.AgentCommValue.Visible = false;
            }

            if (ticket.AgentLevel == 0)
            {
                Template.AgentComm.agentComm_setting = Apollo.Age.Resources.BetList.lblNA;
            }
            else
            {
                if (ticket.CommStatus == "1")
                {
                    Template.AgentComm.agentComm_setting = string.Join(null, new string[] { "2x ", ConvertByBetType.Comm_Percentage(ticket.ADiscount) });
                }
                else
                {
                    Template.AgentComm.agentComm_setting = string.Join(null, new string[] { ConvertByBetType.Comm_Percentage(ticket.ADiscount) }) + "%";
                }                
            }

            return Template;
        }
    }
}