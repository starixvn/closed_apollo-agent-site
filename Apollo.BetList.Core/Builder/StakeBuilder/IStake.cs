// ---------------------------------------------------------------------------------
// <copyright file="IStake.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.StakeBuilder
{
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Stake interface.
    /// </summary>
    public interface IStake
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        Stake_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Stake Template.</returns>
        Stake_Template Render(ITicket ticket);

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Rich Text String.
        /// </returns>
        IRichTextString RenderRTF(ITicket ticket, RTFHelper rtfHelper);
    }
}
