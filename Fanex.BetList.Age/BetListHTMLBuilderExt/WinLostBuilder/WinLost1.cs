// ---------------------------------------------------------------------------------
// <copyright file="WinLost1.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.18">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListHTMLBuilderExt.WinLostBuilder
{
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// Build WinLoss column.
    /// </summary>
    public class WinLost1 : IWinLoss
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinLost1" /> class.
        /// </summary>
        public WinLost1()
        {
            this.Template = new WinLoss_Template();
        }
        
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public WinLoss_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <returns>Win loss template.</returns>
        public WinLoss_Template Render(ITicket ticket)
        {
            if (ticket.Status.Equals(BetStatus.Running) || ticket.Status.Equals(BetStatus.Waiting))
            {
                Template.winloss = "-";
                Template.comm = string.Empty;
            }
            else
            {
                Template.winloss = ConvertByBetType.Winloss(ticket.Winlost);
                Template.comm = ConvertByBetType.Winloss(ticket.PlayerComm);
            }

            return this.Template;
        }
    }
}