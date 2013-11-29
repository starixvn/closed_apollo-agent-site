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

namespace Apollo.BetList.Core.Entities
{
    public delegate string GetCachePropertyById(object id);

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ITicketHelper
    {
        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        int Index { get; set; }

        /// <summary>
        /// Gets or sets the current language.
        /// </summary>
        /// <value>
        /// The current language.
        /// </value>
        string CurrentLanguage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is show odds with choice].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is show odds with choice]; otherwise, <c>false</c>.
        /// </value>
        bool IsShowOddsWithChoice { get; set; }

        /// <summary>
        /// Check if the input IP string is from Vietnam.
        /// </summary>
        /// <param name="ip">Input The IP.</param>
        /// <returns>True if VN IP.</returns>
        bool IsVnIp(string ip);

        #region METHODS TO RESOLVE NAMES FROM IDS

        string GetLeagueNameById(object leagueId);

        string GetTeamNameById(object teamId);

        string GetBetTypeNameById(object bettypeId);

        string GetBetTypeNameById(object bettypeId, object betID);

        string GetSportNameById(object sportId);

        string GetCasinoBetTeamNameById(object betteamId);

        string GetHorseTeamNameById(object homeId, object awayId);

        string GetRouletteBetItemNameById(object itemId);

        #endregion METHODS TO RESOLVE NAMES FROM IDS
    }
}