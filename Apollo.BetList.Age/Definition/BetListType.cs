// ---------------------------------------------------------------------------------
// <copyright file="BetListType.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.18">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Apollo.BetList.Age.Definition
{
    /// <summary>
    /// Bet list type.
    /// </summary>
    public enum BetListType : int
    {
        /// <summary>
        /// None column.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Add Win lost column.
        /// </summary>
        Winlost = 0x1,

        /// <summary>
        /// Add Member information column.
        /// </summary>
        Member = 0x2,

        /// <summary>
        /// Canceled bet list (will not have total row and position taking column, including Win lost).
        /// </summary>
        Canceled = 0x4,

        /// <summary>
        /// Win lost column default.
        /// </summary>
        Default = 0x1,

        /// <summary>
        /// Check hide column on SmartPhone.
        /// </summary>
        SmartPhone = 0x8,

        /// <summary>
        /// Will not have SuperPT, MasterPT, AgentPT.
        /// </summary>
        DailyStatement = 0x10
    }
}
