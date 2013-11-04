// ---------------------------------------------------------------------------------
// <copyright file="ITicketHelper.cs" company="Nexcel Solutions Vietnam">
//     Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------
// <history>
//     <change who="Marc.Bui" date="2012.11.16">Create</change>
//     <change who="Marc.Bui" date="2012.12.19">Remove GetMixParlayDetail</change>
//     <change who="Marc.Bui" date="2012.12.26">Add awayId to GetHorseTeamNameById 
//                                              in case some teams use awayId to 
//                                              resolve sport name</change>
// </history>
// ---------------------------------------------------------------------------------

namespace Fanex.BetList.Core.Entities
{
    public delegate string GetCachePropertyById(object id);
    
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITicketHelper
    {
        #region METHODS TO RESOLVE NAMES FROM IDS

        string GetLeagueNameById(object leagueId);
        string GetTeamNameById(object teamId);
        string GetBetTypeNameById(object bettypeId);
        string GetBetTypeNameById(object bettypeId, object betID);
        string GetSportNameById(object sportId);
        string GetCasinoBetTeamNameById(object betteamId);
        string GetHorseTeamNameById(object homeId, object awayId);
        string GetRouletteBetItemNameById(object itemId);

        #endregion

        /// <summary>
        /// Check if the input IP string is from Vietnam
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        bool IsVnIp(string ip);
        int Index { get; set; }
        string CurrentLanguage { get; set; }
        
        /// <summary>
        /// Determine if the current render show Odds values for each ticket (Ex: in MixParlay sub-tickets)
        /// </summary>
        bool IsShowOddsWithChoice { get; set; }
    }
}