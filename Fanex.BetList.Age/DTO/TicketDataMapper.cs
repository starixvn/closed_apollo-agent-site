// ---------------------------------------------------------------------------------
// <copyright file="TicketDataMapper.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.27">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.DTO
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Map field definition for ITicketData type and DataRow.
    /// </summary>
    public class TicketDataMapper
    {
        /// <summary>
        /// The dictionary.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        public static Dictionary<string, string> Dict = new Dictionary<string, string>()
        { 
            { "RefNo", "refno" }, 
            { "BetTypeId", "bettype" }, 
            { "Status", "status" },
            { "Odds", "odds" },
            { "Stake", "stake" },
            { "BetTeam", "betteam" },
            { "SuperPT", "spositiontaking" },
            { "MasterPT", "mpositiontaking" },
            { "AgentPT", "apositiontaking" }
        };
    }
}
