// ---------------------------------------------------------------------------------
// <copyright file="BlankFooter.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class BlankFooter : IFooter
    {
        /// <summary>
        /// Renders the specified tickets.
        /// </summary>
        /// <param name="tickets">The tickets.</param>
        /// <param name="betlistType">Type of the bet list.</param>
        /// <returns>
        /// The empty.
        /// </returns>
        public string Render(IList<ITicket> tickets, BetListType betlistType)
        {
            return string.Empty;
        }
    }
}