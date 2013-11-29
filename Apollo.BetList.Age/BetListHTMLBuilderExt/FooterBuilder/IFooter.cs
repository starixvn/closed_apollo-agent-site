// ---------------------------------------------------------------------------------
// <copyright file="IFooter.cs" company="Nexcel Solutions Vietnam">
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

    public interface IFooter
    {
        string Render(IList<ITicket> tickets, BetListType betlistType);
    }
}