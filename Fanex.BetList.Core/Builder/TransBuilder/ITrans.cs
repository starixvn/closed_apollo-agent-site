// ---------------------------------------------------------------------------------
// <copyright file="ITrans.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.24">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.TransBuilder
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
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
        /// <param name="ticket"></param>
        /// <param name="ticketHelper"></param>
        /// <param name="FuncGetOddsTypeName"></param>
        /// <param name="betlistPage"></param>
        /// <returns></returns>
        Trans_Template Render(ITicket ticket, ITicketHelper ticketHelper, GetCachePropertyById FuncGetOddsTypeName, BetListPage betlistPage);
    }
}