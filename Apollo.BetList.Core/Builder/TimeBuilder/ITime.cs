// ---------------------------------------------------------------------------------
// <copyright file="ITime.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.06.26">Create</change>
// </history>
// ---------------------------------------------------------------------------------
namespace Apollo.BetList.Core.Builder.TimeBuilder
{
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using NPOI.SS.UserModel;

    public interface ITime
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        Time_Template Template { get; set; }

        /// <summary>
        /// Render time.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="page">Link from page.</param>
        /// <returns>Time Template.</returns>
        Time_Template Render(ITicket ticket, BetListPage page);

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="page">The bet list page.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Time Template.
        /// </returns>
        IRichTextString RenderRTF(ITicket ticket, BetListPage page, RTFHelper rtfHelper);
    }
}