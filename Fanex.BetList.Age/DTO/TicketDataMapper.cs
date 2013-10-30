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

    /// <summary>
    /// Mapfield definition for ITicketData type and DataRow
    /// </summary>
    public class TicketDataMapper
    {
        public static Dictionary<string, string> dict = new Dictionary<string, string>()         
        { 
            {"RefNo", "refno"}, 
            {"BetTypeId", "bettype"}, 
            {"Status", "status"},
            {"Odds", "odds"},
            {"Stake", "stake"},
            {"BetTeam", "betteam"},
            {"SuperPT", "spositiontaking"},
            {"MasterPT", "mpositiontaking"},
            {"AgentPT", "apositiontaking"}
        };
    }
}
