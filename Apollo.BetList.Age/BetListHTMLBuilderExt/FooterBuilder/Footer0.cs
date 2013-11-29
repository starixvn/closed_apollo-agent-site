// ---------------------------------------------------------------------------------
// <copyright file="Footer0.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.BetList.Age.Definition;
    using Apollo.BetList.Age.ExtTemplate;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Build Footer with no total info.
    /// </summary>
    public class Footer0 : IFooter
    {
        private Footer0_Template _template;

        /// <summary>
        /// Initializes a new instance of the <see cref="Footer0" /> class.
        /// </summary>
        public Footer0()
        {
            _template = new Footer0_Template();
        }

        /// <summary>
        /// Renders the specified tickets.
        /// </summary>
        /// <param name="tickets">The tickets.</param>
        /// <param name="betlistType">Type of the bet list.</param>
        /// <returns>String template.</returns>
        public string Render(IList<ITicket> tickets, BetListType betlistType)
        {
            int totalColumns = 10;

            if ((betlistType & BetListType.Canceled) == BetListType.Canceled)
            {
                totalColumns = totalColumns - 4;
            }
            else
            {
                if ((betlistType & BetListType.Member) != BetListType.Member)
                {
                    totalColumns = totalColumns - 1;
                }

                if ((betlistType & BetListType.Winlost) != BetListType.Winlost)
                {
                    totalColumns = totalColumns - 1;
                }
            }

            _template.colSpan = totalColumns.ToString();
            return _template;
        }
    }
}