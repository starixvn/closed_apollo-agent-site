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

namespace Fanex.BetList.Core.Builder.StatusBuilder
{
    using System.Collections.Generic;

    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using Fanex.BetList.Core.Utils;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Build Status cell
    /// </summary>
    public class Status1 : IStatus
    {
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
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data (Status33 - Win/Place)</param>
        /// <param name="isShowVNIP">if set to <c>true</c> [is show VNIP].</param>
        /// <returns></returns>
        public virtual Status_Template Render(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, bool isShowVNIP)
        {
            string ticketBetStatus = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();
            Template.result = ticketBetStatus;
            string result = string.Empty;

            switch (ticketBetStatus)
            {
                case BetStatus.Running:
                    result = Apollo.Age.Resources.BetList.running;
                    break;
                case BetStatus.Reject:
                    result = Apollo.Age.Resources.BetList.reject;
                    break;
                case BetStatus.Won:
                    result = Apollo.Age.Resources.BetList.won;
                    break;
                case BetStatus.Lose:
                    result = Apollo.Age.Resources.BetList.lose;
                    break;
                case BetStatus.Draw:
                    result = Apollo.Age.Resources.BetList.draw;
                    break;
                case BetStatus.Waiting:
                    result = Apollo.Age.Resources.BetList.waiting;
                    break;
                case BetStatus.Void:
                    result = Apollo.Age.Resources.BetList.voided;
                    break;
                case BetStatus.Refund:
                    result = Apollo.Age.Resources.BetList.refund;
                    break;
            }

            if (string.Empty != result)
            {
                Template.result = result;
            }

            if (ticketBetStatus.Equals(BetStatus.Running) || ticketBetStatus.Equals(BetStatus.Waiting) || ticketBetStatus.Equals(BetStatus.Void) || ticketBetStatus.Equals(BetStatus.Reject) || ticketBetStatus.Equals(BetStatus.Refund))
            {
                Template.StatusResult.Hide();
            }
            else
            {
                Template.StatusResult.matchId = ticket.MatchId.ToString();
                Template.StatusResult.betType = ticket.BetTypeId.ToString();
                Template.StatusResult.race = string.IsNullOrEmpty(ticket.Race) ? "0" : ticket.Race;
                Template.StatusResult.refNo = ticket.TransId.ToString();
                Template.StatusResult.sportType = ticket.SportTypeId.ToString();
                Template.StatusResult.league = ticket.LeagueId.ToString();
                Template.StatusResult.userName = ticket.UserName;

                Template.StatusResult.winlostDate = Formatter.FullDateFormat(ticket.WinlostDate);
            }

            if (isShowVNIP == false)
            {
                if (ticketHelper.IsVnIp(ticket.IP))
                {
                    Template.ShowIPVN.Hide();
                }
                else
                {
                    Template.ShowIPVN.betIp = ticket.IP;
                }
            }
            else
            {
                Template.ShowIPVN.betIp = ticket.IP;
            }

            return Template;
        }

        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="isShowVNIP">if set to <c>true</c> [is show VNIP].</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns></returns>
        public virtual IRichTextString RenderRTF(ITicket ticket, ITicketHelper ticketHelper, List<ITicketData> ticketData, bool isShowVNIP, RTFHelper rtfHelper)
        {
            Render(ticket, ticketHelper, ticketData, isShowVNIP);

            string status = Template.result;
            rtfHelper.RTFRenderer.AddText(status, rtfHelper.PosFont);

            if (Template.ShowIPVN.Visible)
            {
                status = string.Join(null, new string[] { "\n", Template.ShowIPVN.betIp });
                rtfHelper.RTFRenderer.AddText(status);
            }

            IRichTextString rtfStatus = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return rtfStatus;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Status1" /> class.
        /// </summary>
        public Status1()
        {
            this.Template = new Status_Template();
        }
    }
}