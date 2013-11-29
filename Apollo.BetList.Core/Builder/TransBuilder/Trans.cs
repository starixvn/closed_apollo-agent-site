// --------------------------------------------------------------------------
// <copyright file="Trans.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.11.5">Created</change>
// </history>

namespace Apollo.BetList.Core.Builder.TransBuilder
{
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;

    /// <summary>
    /// Transaction class.
    /// </summary>
    public class Trans : ITrans
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Trans" /> class.
        /// </summary>
        public Trans()
        {
            this.Template = new Trans_Template();
        }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public Trans_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <param name="ticketHelper">The ticket helper.</param>
        /// <param name="funcGetOddsTypeName">Name of the function get odds type.</param>
        /// <param name="betListPage">The bet list page.</param>
        /// <returns>Transaction Template.</returns>
        public virtual Trans_Template Render(ITicket ticket, ITicketHelper ticketHelper, GetCachePropertyById funcGetOddsTypeName, BetListPage betListPage)
        {
            this.Template.oddsName = string.Empty;
            this.Template.transId = ticket.TransId.ToString();
            this.Template.bettypeName = ticketHelper.GetBetTypeNameById(ticket.BetTypeId);

            switch (betListPage)
            {
                case BetListPage.Wager:
                case BetListPage.Forecast:
                case BetListPage.AccountInfo:
                case BetListPage.Cancel:
                case BetListPage.Transaction:
                case BetListPage.Report:
                case BetListPage.Outstanding:
                    this.Template.sportName = string.Format("<br/>{0}&nbsp;", ticketHelper.GetSportNameById(ticket.SportTypeId));
                    break;

                default:
                    this.Template.sportName = string.Empty;
                    break;
            }

            return Template;
        }
    }
}