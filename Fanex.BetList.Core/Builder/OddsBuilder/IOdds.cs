// ---------------------------------------------------------------------------------
// <copyright file="IOdds.cs" company="Nexcel Solutions Vietnam">
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
    using NPOI.SS.UserModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IOdds
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        Odds_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="FuncGetOddsTypeName">Name of the func get odds type.</param>
        /// <returns>Odds_Template.</returns>
        Odds_Template Render(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById FuncGetOddsTypeName);

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="FuncGetOddsTypeName">Name of the function get odds type.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>IRichTextString</returns>
        IRichTextString RenderRTF(ITicket ticket, List<ITicketData> ticketData, GetCachePropertyById FuncGetOddsTypeName, RTFHelper rtfHelper);
    }
}
