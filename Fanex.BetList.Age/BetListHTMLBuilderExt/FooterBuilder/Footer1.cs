// ---------------------------------------------------------------------------------
// <copyright file="Footer1.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.18">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.BetListHTMLBuilderExt.FooterBuilder
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.BetList.Age.Definition;
    using Fanex.BetList.Age.ExtTemplate;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.Utils;

    /// <summary>
    /// WinLost.
    /// </summary>
    public class Footer1 : IFooter
    {
        private Footer1_Template Template;

        /// <summary>
        /// Initializes a new instance of the <see cref="Footer1" /> class.
        /// </summary>
        public Footer1()
        {
            Template = new Footer1_Template();
        }

        /// <summary>
        /// Renders the specified tickets.
        /// </summary>
        /// <param name="tickets">The tickets.</param>
        /// <param name="betlistType">Type of the betlist.</param>
        /// <param name="userType">Type of the user.</param>
        /// <returns></returns>
        public string Render(IList<ITicket> tickets, BetListType betlistType)
        {
            int statusColumns = 4;
            int subTotalColumns = 5;

            if ((betlistType & BetListType.Canceled) == BetListType.Canceled)
            {
                Template.Status = null;
                Template.subTotalColSpan = subTotalColumns.ToString();
            }
            else
            {
                if ((betlistType & BetListType.Member) != BetListType.Member)
                {
                    subTotalColumns = subTotalColumns - 1;
                }

                Template.subTotalColSpan = subTotalColumns.ToString();

                if ((betlistType & BetListType.DailyStatement) == BetListType.DailyStatement)
                {
                    statusColumns = statusColumns - 3;
                }

                Template.Status.statusColSpan = statusColumns.ToString();
            }

            IList<ITicket> filteredTickets = tickets.Where(ticket => ticket.Status.ToLower() != BetStatus.Running).ToList();

            decimal sumWinLoss = filteredTickets.Sum(ticket => ticket.Winlost);
            decimal sumPlayerComm = filteredTickets.Sum(ticket => ticket.PlayerComm);

            Template.Winloss.subTotalWinloss = ConvertByBetType.Winloss(sumWinLoss);
            Template.Winloss.subTotalCom = ConvertByBetType.Comm(sumPlayerComm);
            Template.Winloss.total = ConvertByBetType.Winloss(sumWinLoss + sumPlayerComm);

            return Template;
        }
    }
}