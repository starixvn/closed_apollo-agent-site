// ---------------------------------------------------------------------------------
// <copyright file="ITrans.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.24">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.TransBuilder
{
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using NPOI.SS.UserModel;

    public interface ITrans
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        Trans_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <param name="betlistPage">The bet list page.</param>
        /// <returns>Transaction Template.</returns>
        Trans_Template Render(ITicket ticket, ITicketHelper ticketHelper, GetCachePropertyById funcGetOddsTypeName, BetListPage betlistPage);

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="page">The bet list page.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <returns>
        /// Rich Text String.
        /// </returns>
        IRichTextString RenderRTF(ITicket ticket, ITicketHelper ticketHelper, BetListPage page, RTFHelper rtfHelper, GetCachePropertyById funcGetOddsTypeName);
    }
}