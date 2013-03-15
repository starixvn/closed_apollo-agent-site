// ---------------------------------------------------------------------------------
// <copyright file="Odds71.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2013.02.21">Create</change>
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
    /// Casino
    /// </summary>
    public class Odds71 : IOdds
    {
        public Odds_Template Template { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Odds1" /> class.
        /// </summary>
        public Odds71()
        {
            this.Template = new Odds_Template();
        }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="FuncGetOddsTypeName">Name of the func get odds type.</param>
        /// <returns>Odds_Template.</returns>
        public Odds_Template Render(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById FuncGetOddsTypeName)
        {
            //This bettype never has an odds
            this.Template.odds = "-";
            this.Template.oddsType = string.Empty;            

            return this.Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="FuncGetOddsTypeName">Name of the func get odds type.</param>
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

            var rtfOdds = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();
            return rtfOdds;
        }
    }
}