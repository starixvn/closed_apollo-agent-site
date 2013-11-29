// ---------------------------------------------------------------------------------
// <copyright file="BetListHTMLFactory.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Core.Factories
{
    using Apollo.BetList.Core.Builder;
    using Apollo.BetList.Core.Entities;

    /// <summary>
    /// Class BetListHTMLFactory.
    /// </summary>
    public class BetListHTMLFactory : IBetListFactory
    {
        /// <summary>
        /// Creates the builder.
        /// </summary>
        /// <returns>Bet List HTML Builder.</returns>
        public BetListHTMLBuilder CreateBuilder()
        {
            BetListHTMLBuilder builder = new BetListHTMLBuilder();
            return builder;
        }
    }
}