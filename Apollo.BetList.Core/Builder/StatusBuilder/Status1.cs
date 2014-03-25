// ---------------------------------------------------------------------------------
// <copyright file="Status1.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.25">Create</change>
//     <change who="Marc.Bui" date="2013.01.24">Hide result link for Refunded tickets</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Builder.StatusBuilder
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Build Status cell.
    /// </summary>
    public class Status1 : IStatus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Status1" /> class.
        /// </summary>
        public Status1()
        {
            this.Template = new Status_Template();
        }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public Status_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data (Status33 - Win/Place).</param>
        /// <returns>
        /// Status template.
        /// </returns>
        public virtual Status_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            string ticketBetStatus = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();
            string result = string.Empty;

            switch (ticketBetStatus)
            {
                case BetStatus.Running:
                    result = BetList.running;
                    break;

                case BetStatus.Reject:
                    result = BetList.reject;
                    break;

                case BetStatus.Won:
                    result = BetList.won;
                    break;

                case BetStatus.Lose:
                    result = BetList.lose;
                    break;

                case BetStatus.Draw:
                    result = BetList.draw;
                    break;

                case BetStatus.Waiting:
                    result = BetList.waiting;
                    break;

                case BetStatus.Void:
                    result = BetList.voided;
                    break;

                case BetStatus.Refund:
                    result = BetList.refund;
                    break;
            }

            Template.result = result;
            Template.betIp = ticket.IP;

            return Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>
        /// Rich text string.
        /// </returns>
        public virtual IRichTextString RenderRTF(ITicket ticket, List<ITicketData> ticketData, RTFHelper rtfHelper)
        {
            Render(ticket, ticketData);

            rtfHelper.RTFRenderer.AddText(Template.result, rtfHelper.Font(HSSFColor.Black.Index, FontBoldWeight.Normal));
            rtfHelper.RTFRenderer.AddText(string.Join(null, new string[] { "\n", ticket.IP }), rtfHelper.Font(HSSFColor.Blue.Index, FontBoldWeight.Normal));

            IRichTextString rtfStatus = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return rtfStatus;
        }
    }
}