// --------------------------------------------------------------------------
// <copyright file="IWinLoss.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.11.1">Created</change>
// </history>

namespace Apollo.BetList.Age.BetListHTMLBuilderExt.WinLostBuilder
{
    using Apollo.BetList.Age.ExtTemplate;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IWinLoss
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        WinLoss_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns>Win loss template.</returns>
        WinLoss_Template Render(ITicket ticket);
    }
}