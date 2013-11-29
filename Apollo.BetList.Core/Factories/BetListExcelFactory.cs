// ---------------------------------------------------------------------------------
// <copyright file="BetListExcelFactory.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Factories
{
    using Fanex.BetList.Core.Builder;

    /// <summary>
    /// Class BetListExcelFactory
    /// </summary>
    public class BetListExcelFactory : IBetListFactory
    {
        /// <summary>
        /// Creates the builder.
        /// </summary>
        /// <returns>BetListExcelBuilder.</returns>
        public BetListExcelBuilder CreateBuilder()
        {
            BetListExcelBuilder builder = new BetListExcelBuilder();
            return builder;
        }
    }
}