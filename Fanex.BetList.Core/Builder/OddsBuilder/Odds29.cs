// ---------------------------------------------------------------------------------
// <copyright file="Odds29.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.OddsBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using Fanex.BetList.Core.Utils;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Combination Mix Parlay.
    /// </summary>
    public class Odds29 : IOdds
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Odds29" /> class.
        /// </summary>
        public Odds29()
        {
            this.Template = new Odds_Template();
        }

        public Odds_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="FuncGetOddsTypeName">Name of the function get odds type.</param>
        /// <returns>Odds_Template.</returns>
        public Odds_Template Render(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById FuncGetOddsTypeName)
        {
            string _status = ticket.Status.ToLower();
            if (_status.Equals(BetStatus.Running))
            {
                this.Template.odds = "-";
            }
            else
            {
                this.Template.odds = ConvertByBetType.Odds(ticket.MOdds);
            }

            this.Template.oddsType = FuncGetOddsTypeName(ConvertByBetType.OddsType(ticket.BetTypeId, ticket.OddsType));

            return this.Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="FuncGetOddsTypeName">Name of the function get odds type.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// IRichTextString
        /// </returns>
        public IRichTextString RenderRTF(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById FuncGetOddsTypeName, RTFHelper rtfHelper)
        {
            Render(ticket, ticketData, FuncGetOddsTypeName);
            var rtfOdds = BuildRTFOdds(rtfHelper);

            return rtfOdds;
        }

        /// <summary>
        /// Builds the RTF odds.
        /// </summary>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns></returns>
        protected virtual IRichTextString BuildRTFOdds(RTFHelper rtfHelper)
        {
            string odds = Template.odds;

            var rtfNumberRenderer = new RTFNumber(rtfHelper.RTFRenderer, rtfHelper.PosFont, rtfHelper.NegFont);

            rtfNumberRenderer.Render(odds);
            if (!string.IsNullOrEmpty(Template.oddsType))
            {
                odds = "\n" + Template.oddsType;
                rtfHelper.RTFRenderer.AddText(odds, rtfHelper.PosFont);
            }

            var rtfOdds = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();
            return rtfOdds;
        }
    }
}