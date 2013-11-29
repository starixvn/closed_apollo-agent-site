// ---------------------------------------------------------------------------------
// <copyright file="Footer1.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.18">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Age.BetListHTMLBuilderExt.FooterBuilder
{
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.BetList.Age.Definition;
    using Apollo.BetList.Age.ExtTemplate;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Utils;

    /// <summary>
    /// Win Lost class.
    /// </summary>
    public class Footer1 : IFooter
    {
        private Footer1_Template _template;

        /// <summary>
        /// Initializes a new instance of the <see cref="Footer1" /> class.
        /// </summary>
        public Footer1()
        {
            _template = new Footer1_Template();
        }

        /// <summary>
        /// Renders the specified tickets.
        /// </summary>
        /// <param name="tickets">The tickets.</param>
        /// <param name="betlistType">Type of the bet list.</param>
        /// <returns>
        /// String template.
        /// </returns>
        public string Render(IList<ITicket> tickets, BetListType betlistType)
        {
            int statusColumns = 4;
            int subTotalColumns = 5;

            if ((betlistType & BetListType.Canceled) == BetListType.Canceled)
            {
                _template.Status = null;
                _template.subTotalColSpan = subTotalColumns.ToString();
            }
            else
            {
                if ((betlistType & BetListType.Member) != BetListType.Member)
                {
                    subTotalColumns = subTotalColumns - 1;
                }

                _template.subTotalColSpan = subTotalColumns.ToString();

                if ((betlistType & BetListType.DailyStatement) == BetListType.DailyStatement)
                {
                    statusColumns = statusColumns - 3;
                }

                _template.Status.statusColSpan = statusColumns.ToString();
            }

            IList<ITicket> filteredTickets = tickets.Where(ticket => ticket.Status.ToLower() != BetStatus.Running).ToList();

            decimal sumWinLoss = filteredTickets.Sum(ticket => ticket.Winlost);
            decimal sumPlayerComm = filteredTickets.Sum(ticket => ticket.PlayerComm);

            _template.Winloss.subTotalWinloss = ConvertByBetType.Winloss(sumWinLoss);
            _template.Winloss.subTotalCom = ConvertByBetType.Comm(sumPlayerComm);
            _template.Winloss.total = ConvertByBetType.Winloss(sumWinLoss + sumPlayerComm);

            return _template;
        }
    }
}