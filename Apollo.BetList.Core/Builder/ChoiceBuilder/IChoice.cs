// ---------------------------------------------------------------------------------
// <copyright file="IChoice.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.ChoiceBuilder
{
    using System.Collections.Generic;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Choice interface.
    /// </summary>
    public interface IChoice
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        Choice_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        Choice_Template Render(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, bool isShowScoreMap);

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <returns>
        /// Choice Template.
        /// </returns>
        Choice_Template Render(
            ITicket ticket, 
            ITicketHelper ticketHelper, 
            List<ITicketData> ticketData, 
            IList<ITicket> ticketMixParlay, 
            bool isShowScoreMap);

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="ticketMixParlay">The ticket mix parlay.</param>
        /// <param name="isShowScoreMap">If set to <c>true</c> [is show score map].</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// List of rich text.
        /// </returns>
        List<IRichTextString> RenderRTF(
            ITicket ticket, 
            ITicketHelper ticketHelper, 
            List<ITicketData> ticketData, 
            IList<ITicket> ticketMixParlay, 
            bool isShowScoreMap, 
            RTFHelper rtfHelper);
    }
}