// ---------------------------------------------------------------------------------
// <copyright file="SuperPT33.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.01.28">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListHTMLBuilderExt.SuperPTBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SuperPT33 : SuperPT1
    {
        protected int winBetId, placeBetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuperPT33" /> class.
        /// </summary>
        public SuperPT33()
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
        public override SuperPT_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            decimal superPT_Win = 0;
            decimal superPT_Place = 0;

            List<ITicketData> refTicketData = this.GetReferenceData(ticket, ticketData);

            if (refTicketData.Count > 1)
            {
                superPT_Win = refTicketData.FindLast(item => item.BetTypeId == winBetId).SuperPT;
                superPT_Place = refTicketData.FindLast(item => item.BetTypeId == placeBetId).SuperPT;
            }

            this.Template.SuperPT.superPT_setting = string.Join(null, new string[] { "<span class='blue' style='font-weight:normal;'>"
                                                                      , Resources.BetList.win + ": "
                                                                      , "<b>",Formatter.FormatNumber2(superPT_Win*100, null),"%</b>"
                                                                      , "</span><br>"
                                                                      , "<span class='' style='font-weight:normal;'>"
                                                                      , Resources.BetList.place + ": "
                                                                      , "<b>", Formatter.FormatNumber2(superPT_Place*100, null), "%</b>"
                                                                      , "</span>"});

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
