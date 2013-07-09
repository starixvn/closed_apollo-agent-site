// ---------------------------------------------------------------------------------
// <copyright file="Trans1.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.24">Create</change>
//     <change who="Marc.Bui" date="2013.01.24">Remove BetSlip from Trans.Time</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Builder.TransBuilder
{
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Templates;
    using Fanex.BetList.Core.Utils;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public class Trans : ITrans
    {
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public Trans_Template Template { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trans" /> class.
        /// </summary>
        public Trans()
        {
            this.Template = new Trans_Template();
        }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket.</param>
        /// <returns></returns>
        public virtual Trans_Template Render(ITicket ticket, ITicketHelper ticketHelper, GetCachePropertyById FuncGetOddsTypeName, BetListPage betListPage)
        {
            this.Template.oddsName = string.Empty;
            this.Template.transId = ticket.TransId.ToString();
            this.Template.bettypeName = ticketHelper.GetBetTypeNameById(ticket.BetTypeId);

            switch (betListPage)
            {
                case BetListPage.Wager:
                case BetListPage.Forecast:
                case BetListPage.Outstanding:
                    this.Template.sportName = string.Empty;
                    break;
                case BetListPage.AccountInfo:
                case BetListPage.Cancel:
                case BetListPage.Transaction:
                case BetListPage.Report:
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