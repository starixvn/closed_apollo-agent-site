// --------------------------------------------------------------------------
// <copyright file="Result.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2014.1.16">Created</change>
// </history>
namespace Apollo.BetList.Core.Builder.ResultBuilder
{
    using Apollo.BetList.Core.Entities;
    using NPOI.SS.UserModel;
    using Apollo.BetList.Core.Utils;

    /// <summary>
    /// Result builder class.
    /// </summary>
    public class Result : IResult
    {
        /// <summary>
        /// Renders the RTF.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="page">The bet list page.</param>
        /// <param name="rtfHelper">The RTF helper.</param>
        /// <returns>Rich Text String.</returns>
        public IRichTextString RenderRTF(ITicket ticket, BetListPage page, RTFHelper rtfHelper)
        {
            string ticketBetStatus = string.IsNullOrEmpty(ticket.Status) ? string.Empty : ticket.Status.ToLower();
            if (ticketBetStatus == Apollo.BetList.Core.Entities.BetStatus.Won
                || ticketBetStatus == Apollo.BetList.Core.Entities.BetStatus.Draw
                || ticketBetStatus == Apollo.BetList.Core.Entities.BetStatus.Lose
                || (ticket.StatusId != null && ticket.StatusId.Value > 110 && ticket.StatusId.Value < 114))
            {
                decimal result = ticket.Winlost + ticket.PlayerComm;
                rtfHelper.RTFRenderer.AddText(string.Format(Formatter.DECIMAL_FORMAT20, result));
            }
            else
            {
                rtfHelper.RTFRenderer.AddText("0");
            }

            IRichTextString rtfResult = rtfHelper.RTFRenderer.Render();
            rtfHelper.RTFRenderer.Clear();

            return rtfResult;
        }
    }
}