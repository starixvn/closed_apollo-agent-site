// --------------------------------------------------------------------------
// <copyright file="AgentShare33.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.05.14">Agent PT</change>
// </history>
namespace Fanex.BetList.Core.Builder.AgentShareBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// Agent Share
    /// </summary>
    public class AgentShare33 : AgentShare1
    {
        protected int winBetId, placeBetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentShare33" /> class.
        /// </summary>
        public AgentShare33()
        {
            winBetId = 31;
            placeBetId = 32;
        }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData"></param>
        /// <returns></returns>
        public override AgentShare_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            decimal agentPT_Win = 0;
            decimal agentPT_Place = 0;

            List<ITicketData> refTicketData = this.GetReferenceData(ticket, ticketData);

            if (refTicketData.Count > 1)
            {
                agentPT_Win = refTicketData.FindLast(item => item.BetTypeId == winBetId).AgentPT;
                agentPT_Place = refTicketData.FindLast(item => item.BetTypeId == placeBetId).AgentPT;
            }

            this.Template.AgentPT.agentPT_setting = string.Join(null, new string[]{ "<span class='blue' style='font-weight:normal;'>"
                                                                      ,Apollo.Age.Resources.BetList.win + ": "
                                                                      ,"<b>",Formatter.FormatNumber2(agentPT_Win*100, null),"%</b>"
                                                                      ,"</span><br>"
                                                                      ,"<span class='' style='font-weight:normal;'>"
                                                                      ,Apollo.Age.Resources.BetList.place + ": "
                                                                      ,"<b>",Formatter.FormatNumber2(agentPT_Place*100, null),"%</b>"
                                                                      ,"</span>"});

            return this.Template;
        }

        /// <summary>
        /// Gets the reference data.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns></returns>
        protected virtual List<ITicketData> GetReferenceData(ITicket ticket, List<ITicketData> ticketData)
        {
            return ticketData.FindAll(item => item.RefNo.Equals(ticket.TransId.ToString()));
        }
    }
}