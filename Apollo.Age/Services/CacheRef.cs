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

        public static string GetBettype(object bettypeId)
        {
            return GetBettype(bettypeId, null);
        }

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

        public static string GetSportName(object sportId)
        {
            return RefData.GetSportName(sportId.ToString(), CurrentLanguage, string.Empty);
        }

        public static string GetTeamName(object teamId)
        {
            return RefData.GetTeamName(teamId.ToString(), CurrentLanguage, string.Empty);
        }

        public static string GetRouletteBetItem(object itemId)
        {
            return RefData.GetRouletteBetItem(itemId.ToString(), CurrentLanguage, string.Empty);
        }

        public static string GetCasinoBetItem(object betStatusId)
        {
            return RefData.GetCasinoBetItem(betStatusId.ToString(), CurrentLanguage, string.Empty);
        }

        public static string GetLeagueName(object leagueId)
        {
            return RefData.GetLeagueName(leagueId.ToString(), CurrentLanguage, string.Empty);
        }

        public static string GetFncIndexName(object indexId)
        {
            return RefData.GetFncIndexName(indexId.ToString(), CurrentLanguage);
        }

        public static string GetHorseTeamName(object codeId)
        {
            return RefData.GetHorseTeamName(codeId.ToString(), CurrentLanguage);
        }

        public static string GetOddsTypeName(object oddsTypeId)
        {
            return RefData.GetOddsTypeName(oddsTypeId.ToString(), CurrentLanguage);
        }
    }
}