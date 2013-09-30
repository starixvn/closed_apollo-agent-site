// ---------------------------------------------------------------------------------
// <copyright file="IDiscount.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.06.26">Create</change>
// </history>
// ---------------------------------------------------------------------------------
namespace Fanex.BetList.Core.Builder.DiscountBuilder
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using NPOI.SS.UserModel;

    public interface IDiscount
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        Discount_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="page">Link from page</param>
        /// <returns></returns>
        Discount_Template Render(ITicket ticket, BetListPage page);
    }
}