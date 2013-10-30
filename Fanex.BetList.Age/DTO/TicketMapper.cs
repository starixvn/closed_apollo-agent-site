// ---------------------------------------------------------------------------------
// <copyright file="TicketMapper.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.12.18">Create</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Age.DTO
{
    using System.Collections.Generic;

    /// <summary>
    /// Mapfield definition for ITicket type and DataRow
    /// </summary>
    public class TicketMapper
    {
        public static Dictionary<string, string> dict = new Dictionary<string, string>()
        {
            {"SportTypeId", "sporttype"},
            {"UserName", "username"},
            {"TransId", "transid"},
            {"BetId", "betid"},
            {"BetTypeId", "bettype"},
            {"Handicap1", "hdp1"},
            {"Handicap2", "hdp2"},
            {"MasterOdds", "modds"},
            {"OddsType", "oddstype"},
            {"Odds", "odds"},
            {"Stake", "stake"},
            {"ActualStake", "actual_stake"},
            {"BetTeam", "betteam"},
            {"Status", "status"},
            {"StatusId", "statusid"},
            {"RefNo", "refno"},
            {"TransDate", "transdate"},
            {"WinlostDate", "winlostdate"},
            {"Winlost", "winlost"},
            {"AWinlost", "awinlost"},
            {"MasterWinlost", "mwinlost"},
            {"SuperWinlost", "swinlost"},
            {"PlayerComm", "playercomm"},
            {"Comm", "comm"},
            {"AComm", "acomm"},
            {"AgentLevel", "agentlevel"},
            {"SuperComm", "scomm"},
            {"APositionTaking", "apositiontaking"},
            {"MasterPositionTaking", "mpositiontaking"},
            {"SuperPositionTaking", "spositiontaking"},
            {"ADiscount", "adiscount"},
            {"MasterDiscount", "mdiscount"},
            {"SuperDiscount", "sdiscount"},
            {"IsLive", "liveindicator"},
            {"LiveHomeScore", "livehomescore"},
            {"LiveAwayScore", "liveawayscore"},
            {"IP", "ip"},
            {"LeagueId", "leagueid"},
            {"MatchId", "matchid"},
            {"MatchCode", "matchcode"},
            {"Race", "race"},
            {"HomeId", "homeid"},
            {"AwayId", "awayid"},
            {"EventStatus", "betstatus"},
            {"IsNeutral", "isneutral"},
            {"CustId", "custid"},
            {"ShowTime", "showtime"},
            {"BetCheck", "betcheck"},
            {"CommStatus", "comstatus"},
        };
    }
}