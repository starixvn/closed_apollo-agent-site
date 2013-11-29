// ---------------------------------------------------------------------------------
// <copyright file="Discount.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Kaden.Trinh" date="2013.06.26">Create</change>
// </history>
// ---------------------------------------------------------------------------------
namespace Apollo.BetList.Core.Builder.DiscountBuilder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Apollo.BetList.Core.Entities;
    using Apollo.BetList.Core.Templates;
    using Apollo.BetList.Core.Utils;

    /// <summary>
    /// Discount class.
    /// </summary>
    public class Discount : IDiscount
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Discount" /> class.
        /// </summary>
        public Discount()
        {
            this.Template = new Discount_Template();
        }
        
        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>
        /// The template.
        /// </value>
        public Discount_Template Template { get; set; }

        /// <summary>
        /// Renders the specified ticket.
        /// </summary>
        /// <param name="ticket">The ticket need to build.</param>
        /// <param name="page">Link from page.</param>
        /// <returns>Discount template.</returns>
        public virtual Discount_Template Render(ITicket ticket, BetListPage page)
        {
            switch (page)
            {
                case BetListPage.AccountInfo:
                case BetListPage.Wager:
                case BetListPage.Forecast:
                case BetListPage.Outstanding:
                    Template.ugroup = RevertCommGroupName(ticket.UGroup);
                    break;
                case BetListPage.Cancel:
                case BetListPage.Transaction:
                case BetListPage.Report:
                    Template.ugroup = string.Empty;
                    break;
                default:
                    Template.ugroup = Template.discount = string.Empty;
                    break;
            }

            if (ticket.PlayerDiscount == 0)
            {
                Template.discount = "0";
            }
            else
            {
                Template.discount = (ticket.PlayerDiscount * 100).ToString("N2");
            }

            Template.username = ticket.UserName;

            return Template;
        }

        /// <summary>
        /// Reverts the name of the commission group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns>Revert commission group name.</returns>
        private string RevertCommGroupName(string groupName)
        {
            switch (groupName.ToLower())
            {
                case "a":
                    return "D";

                case "b":
                    return "C";

                case "c":
                    return "B";

                case "d":
                    return "A";
            }

            return string.Empty;
        }
    }
}