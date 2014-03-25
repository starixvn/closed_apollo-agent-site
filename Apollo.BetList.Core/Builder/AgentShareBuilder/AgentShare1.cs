// --------------------------------------------------------------------------
// <copyright file="AgentShare1.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.14">Agent PT</change>
// </history>

namespace Apollo.BetList.Core.Builder.AgentShareBuilder
{
    using System.Collections.Generic;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using Apollo.BetList.Core.Utils;
    using NPOI.SS.UserModel;
    using NPOI.HSSF.Util;

    /// <summary>
    /// Agent share.
    /// </summary>
    public class AgentShare1 : IAgentShare
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AgentShare1" /> class.
        /// </summary>
        public AgentShare1()
        {
            this.Template = new AgentShare_Template();
        }

        public AgentShare_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns>
        /// Agent share template.
        /// </returns>
        public virtual AgentShare_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            Template.AgentPT.agentPT_setting = string.Join(null, new string[] { ConvertByBetType.Pt(ticket.APositionTaking), "%" });

            string status = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();

            if (status.ToLower().Equals(BetStatus.Won)
                || status.ToLower().Equals(BetStatus.Draw)
                || status.ToLower().Equals(BetStatus.Lose))
            {
                Template.AgentPTValue.agentPT_value = ConvertByBetType.PtValue(ticket.AWinlost);
            }
            else
            {
                Template.AgentPTValue.Visible = false;
            }

            return Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="page">The bet list page.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Rich Text String.
        /// </returns>
        public IRichTextString RenderRTF(ITicket ticket, List<ITicketData> ticketData, BetListPage page, RTFHelper rtfHelper)
        {
            Render(ticket, ticketData);

            string agentPT = string.Join(string.Empty, new string[] { Template.AgentPT.agentPT_setting, "\n" });
            rtfHelper.RTFRenderer.AddText(agentPT);

            string status = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();
            if (status.ToLower().Equals(BetStatus.Won)
                || status.ToLower().Equals(BetStatus.Draw)
                || status.ToLower().Equals(BetStatus.Lose))
            {
                if (ticket.AWinlost < 0)
                {
                    rtfHelper.RTFRenderer.AddText(string.Format(Formatter.DECIMAL_FORMAT20, ticket.AWinlost), rtfHelper.Font(HSSFColor.Red.Index, FontBoldWeight.Normal, false));
                }
                else
                {
                    rtfHelper.RTFRenderer.AddText(string.Format(Formatter.DECIMAL_FORMAT20, ticket.AWinlost));
                }
            }

            IRichTextString rtfAgentShare = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();
            return rtfAgentShare;
        }
    }
}