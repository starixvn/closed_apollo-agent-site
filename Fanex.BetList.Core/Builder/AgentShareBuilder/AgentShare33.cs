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
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// Agent Share.
    /// </summary>
    public class AgentShare33 : AgentShare1
    {
        private int _winBetId, _placeBetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgentShare33" /> class.
        /// </summary>
        public AgentShare33()
        {
            WinBetId = 31;
            PlaceBetId = 32;
        }

        /// <summary>
        /// Gets or sets the win bet identifier.
        /// </summary>
        /// <value>
        /// The win bet identifier.
        /// </value>
        public int WinBetId
        {
            get
            {
                return _winBetId;
            }

            set
            {
                _winBetId = value;
            }
        }

        /// <summary>
        /// Gets or sets the place bet identifier.
        /// </summary>
        /// <value>
        /// The place bet identifier.
        /// </value>
        public int PlaceBetId
        {
            get
            {
                return _placeBetId;
            }

            set
            {
                _placeBetId = value;
            }
        }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">Input a ticket.</param>
        /// <param name="ticketData">List of tickets data.</param>
        /// <returns>Agent share template.</returns>
        public override AgentShare_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            decimal agentPT_Win = 0;
            decimal agentPT_Place = 0;

            List<ITicketData> refTicketData = this.GetReferenceData(ticket, ticketData);

            if (refTicketData.Count > 1)
            {
                agentPT_Win = refTicketData.FindLast(item => item.BetTypeId == WinBetId).AgentPT;
                agentPT_Place = refTicketData.FindLast(item => item.BetTypeId == PlaceBetId).AgentPT;
            }

            this.Template.AgentPT.agentPT_setting = string.Join(
                null,
                new string[] { "<span class='blue' style='font-weight:normal;'>",
                    Apollo.Age.Resources.BetList.win + ": ",
                    "<b>",
                    Formatter.FormatNumber2(agentPT_Win * 100, null),
                    "%</b>",
                    "</span><br>",
                    "<span class='' style='font-weight:normal;'>",
                    Apollo.Age.Resources.BetList.place + ": ",
                    "<b>",
                    Formatter.FormatNumber2(agentPT_Place * 100, null),
                    "%</b>",
                    "</span>" });

            return this.Template;
        }

        /// <summary>
        /// Gets the reference data.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns>List of tickets.</returns>
        protected virtual List<ITicketData> GetReferenceData(ITicket ticket, List<ITicketData> ticketData)
        {
            return ticketData.FindAll(item => item.RefNo.Equals(ticket.TransId.ToString()));
        }
    }
}