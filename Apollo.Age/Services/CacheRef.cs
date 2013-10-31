// --------------------------------------------------------------------------
// <copyright file="CacheRef.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.30">Clone from Sunplus.Agent.Model.dll</change>
// </history>

namespace Apollo.Age.Services
{
    using System.Threading;
    using RemotingRef;

    /// <summary>
    /// This class is using to retrieve data by remote reference.
    /// </summary>
    public static class CacheRef
    {
        public static string CurrentLanguage
        {
            get
            {
                switch (Thread.CurrentThread.CurrentCulture.Name)
                {
                    case "en-US": return "en";
                    case "zh-TW": return "ch"; // Traditional
                    case "zh-CN": return "cs"; // Simplified
                    case "ja-JP": return "jp";
                    case "th-TH": return "th"; // Thai
                    case "ko-KR": return "ko"; // Korean
                    case "vi-VN": return "vn"; // Vietnamese
                    default: return "en";
                }
            }
        }

        /// <summary>
        /// Get Bet type Name from Bet type ID.
        /// </summary>
        /// <param name="bettypeId">Input a bet type Id.</param>
        /// <returns>Bet type name.</returns>
        public static string GetBettype(object bettypeId)
        {
            return GetBettype(bettypeId, null);
        }

        /// <summary>
        /// Get Bet Type Name from Bet type Id and bet Id.
        /// </summary>
        /// <param name="bettypeId">Input a bet type Id.</param>
        /// <param name="betID">Input a bet Id.</param>
        /// <returns>Bet type name.</returns>
        public static string GetBettype(object bettypeId, object betID)
        {
            // resourceId remove first character
            string resourceId = string.Empty;

            if (betID != null && betID.ToString().Length > 1)
            {
                resourceId = betID.ToString().Substring(1);
            }

            return RefData.GetBetTypeName(bettypeId.ToString(), CurrentLanguage, string.Empty, resourceId);
        }

        /// <summary>
        /// Get Sport Name from Sport Id.
        /// </summary>
        /// <param name="sportId">Input a sport Id.</param>
        /// <returns>Sport Name.</returns>
        public static string GetSportName(object sportId)
        {
            return RefData.GetSportName(sportId.ToString(), CurrentLanguage, string.Empty);
        }

        /// <summary>
        /// Get Team Name from Team Id.
        /// </summary>
        /// <param name="teamId">Input a team Id.</param>
        /// <returns>Team name.</returns>
        public static string GetTeamName(object teamId)
        {
            return RefData.GetTeamName(teamId.ToString(), CurrentLanguage, string.Empty);
        }

        /// <summary>
        /// Get Bet Item Name from Item Id of Roulette Bet Type.
        /// </summary>
        /// <param name="itemId">Input a item Id.</param>
        /// <returns>Bet item name.</returns>
        public static string GetRouletteBetItem(object itemId)
        {
            return RefData.GetRouletteBetItem(itemId.ToString(), CurrentLanguage, string.Empty);
        }

        /// <summary>
        /// Get bet item from bet status id of Casino bet type.
        /// </summary>
        /// <param name="betStatusId">Input a bet status id.</param>
        /// <returns>Bet item name.</returns>
        public static string GetCasinoBetItem(object betStatusId)
        {
            return RefData.GetCasinoBetItem(betStatusId.ToString(), CurrentLanguage, string.Empty);
        }

        /// <summary>
        /// Get league name from league id.
        /// </summary>
        /// <param name="leagueId">Input a league id.</param>
        /// <returns>League name.</returns>
        public static string GetLeagueName(object leagueId)
        {
            return RefData.GetLeagueName(leagueId.ToString(), CurrentLanguage, string.Empty);
        }      

        /// <summary>
        /// Get team name from code id of Horse Racing bet type.
        /// </summary>
        /// <param name="codeId">Input a code id.</param>
        /// <returns>Team name.</returns>
        public static string GetHorseTeamName(object codeId)
        {
            return RefData.GetHorseTeamName(codeId.ToString(), CurrentLanguage);
        }

        /// <summary>
        /// Get odd name from odd id.
        /// </summary>
        /// <param name="oddsTypeId">Input a odd id.</param>
        /// <returns>Return Odd name.</returns>
        public static string GetOddsTypeName(object oddsTypeId)
        {
            return RefData.GetOddsTypeName(oddsTypeId.ToString(), CurrentLanguage);
        }
    }
}