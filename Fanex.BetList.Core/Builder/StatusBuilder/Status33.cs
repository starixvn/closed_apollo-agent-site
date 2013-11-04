// ---------------------------------------------------------------------------------
// <copyright file="Status33.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.26">Create</change>
//     <change who="Marc.Bui" date="2013.01.25">Update export excel</change>
//     <change who="Marc.Bui" date="2013.02.01">Change statuses to show</change>
//     <change who="Marc.Bui" date="2013.02.27">Add GetReferenceData for Odds 1233</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.StatusBuilder
{
    using System.Collections.Generic;
    using Fanex.BetList.Core.Entities;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    /// <summary>
    /// Win/Place.
    /// </summary>
    public class Status33 : Status1
    {
        protected int winBetId, placeBetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Status33" /> class.
        /// </summary>
        public Status33()
        {
            winBetId = 31;
            placeBetId = 32;
        }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <param name="isShowVNIP">If set to <c>true</c> [is show VNIP].</param>
        /// <returns></returns>
        public override Templates.Status_Template Render(ITicket ticket, List<ITicketData> ticketData)
        {
            base.Render(ticket, ticketData);

            // Custom Status generated from base class if ticketData has matched items for the current ticket
            if (ticketData != null && ticketData.Count > 0)
            {
                List<ITicketData> foundTicketData = this.GetReferenceData(ticket, ticketData);

                if (foundTicketData != null && foundTicketData.Count > 1)
                {
                    string winStatus = foundTicketData.Find(item => item.BetTypeId == winBetId).Status.ToLower();
                    string placeStatus = foundTicketData.Find(item => item.BetTypeId == placeBetId).Status.ToLower();

                    string contextWinStatus = GetContextStatus(winStatus);
                    string contextPlaceStatus = GetContextStatus(placeStatus);

                    string winStatusMarkup = string.Join(null, new string[] { "<span class='blue' style='font-weight:normal;'>", Apollo.Age.Resources.BetList.win, ": ", "<b>", contextWinStatus, "</b>", "</span>" });
                    string placeStatusMarkup = string.Join(null, new string[] { "<span class='' style='font-weight:normal;'>", Apollo.Age.Resources.BetList.place, ": ", "<b>", contextPlaceStatus, "</b>", "</span>" });

                    string ticketBetStatus = ticket.Status.ToLower();

                    switch (ticketBetStatus)
                    {
                        case BetStatus.Running:
                        case BetStatus.Waiting:
                        case BetStatus.Void:
                        case BetStatus.Reject:
                        case BetStatus.Refund:
                            break;
                        default:
                            this.Template.result = string.Join(null, new string[] { winStatusMarkup, "<br/>", placeStatusMarkup });
                            break;
                    }
                }
            }

            return Template;
        }

        public override IRichTextString RenderRTF(ITicket ticket, List<ITicketData> ticketData, bool isShowVNIP, RTFHelper rtfHelper)
        {
            Render(ticket, ticketData);

            // Status != Win/Lose
            if (this.Template.result.Length < 80)
            {
                rtfHelper.RTFRenderer.AddText(this.Template.result, rtfHelper.PosFont);
            }
            else
            {
                int breakPos = this.Template.result.IndexOf("<br/>");

                string status_WinString = this.Template.result.Substring(0, breakPos);
                string status_PlaceString = this.Template.result.Replace(status_WinString, string.Empty);

                string status_Win = ExtractStringBetweenTag("b", status_WinString);
                string status_Place = ExtractStringBetweenTag("b", status_PlaceString);

                rtfHelper.RTFRenderer.AddText(string.Join(null, new string[] { Apollo.Age.Resources.BetList.win, ": ", status_Win, "\n", Apollo.Age.Resources.BetList.place, ": ", status_Place }), rtfHelper.PosFont);
            }

            var rtfStatus = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return rtfStatus;
        }

        /// <summary>
        /// Gets the reference data.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketData">The ticket data.</param>
        /// <returns>List of tickets.</returns>
        protected virtual List<ITicketData> GetReferenceData(ITicket ticket, List<ITicketData> ticketData)
        {
            return ticketData.FindAll(item => item.RefNo.Equals(ticket.TransId.ToString()));
        }

        /// <summary>
        /// Extract string value between tags from a source string.
        /// </summary>
        /// <param name="tag">Tag name.</param>
        /// <param name="source">Source string.</param>
        /// <returns></returns>
        private string ExtractStringBetweenTag(string tag, string source)
        {
            string startTag = string.Join(null, new string[] { "<", tag, ">" });
            string endTag = string.Join(null, new string[] { "</", tag, ">" });

            int startIndex = source.IndexOf(startTag) + startTag.Length;
            int endIndex = source.IndexOf(endTag, startIndex);
            return source.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// Gets the context status.
        /// </summary>
        /// <param name="betStatus">The bet status.</param>
        /// <returns>String status.</returns>
        private string GetContextStatus(string betStatus)
        {
            switch (betStatus)
            {
                case BetStatus.Draw:
                    return Apollo.Age.Resources.BetList.draw;
                case BetStatus.Lose:
                    return Apollo.Age.Resources.BetList.lose;
                case BetStatus.Refund:
                    return Apollo.Age.Resources.BetList.refund;
                case BetStatus.Reject:
                    return Apollo.Age.Resources.BetList.reject;
                case BetStatus.Running:
                    return Apollo.Age.Resources.BetList.running;
                case BetStatus.Void:
                    return Apollo.Age.Resources.BetList.voided;
                case BetStatus.Waiting:
                    return Apollo.Age.Resources.BetList.waiting;
                case BetStatus.Won:
                    return Apollo.Age.Resources.BetList.won;
                default:
                    return string.Empty;
            }
        }
    }
}