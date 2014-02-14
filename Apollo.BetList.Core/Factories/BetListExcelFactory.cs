// ---------------------------------------------------------------------------------
// <copyright file="BetListExcelFactory.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Class Bet List Excel Factory.
    /// </summary>
    public class BetListExcelFactory : IBetListFactory
    {
        /// <summary>
        /// Creates the builder.
        /// </summary>
        /// <returns>Bet List Excel Builder.</returns>
        public BetListExcelBuilder CreateBuilder()
        {
            BetListExcelBuilder builder = new BetListExcelBuilder();
            return builder;
        }
    }
}