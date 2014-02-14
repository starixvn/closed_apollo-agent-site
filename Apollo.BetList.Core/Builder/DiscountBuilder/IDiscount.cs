// ---------------------------------------------------------------------------------
// <copyright file="IDiscount.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.06.26">Create</change>
// </history>
// ---------------------------------------------------------------------------------
namespace Apollo.BetList.Core.Builder.DiscountBuilder
{
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Discount interface.
    /// </summary>
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
        /// <param name="page">Link from page.</param>
        /// <returns>Discount template.</returns>
        Discount_Template Render(ITicket ticket, BetListPage page);

        /// <summary>
        /// Adds the discount.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="page">The bet list page.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Bet List Excel Builder.
        /// </returns>
        IRichTextString RenderRTF(ITicket ticket, BetListPage page, RTFHelper rtfHelper);
    }
}