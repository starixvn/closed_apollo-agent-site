namespace Sunplus.Agent.Model.BL.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;

    using Fanex.Caching;
    using RemotingRef;

    using Sunplus.Agent.Model.Services;
    using System.Configuration;

    public class CacheRef
    {
        private static Type refDataType = typeof(RefData);
        private static Dictionary<string, MethodInfo> methodInfos = new Dictionary<string, MethodInfo>();
        private static ICache velocityCache = CacheService.CreateInstance(CacheType.MemoryCache);

        public static string CurrentTWLanguage
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

        private static bool IsWebService
        {
            get
            {
                return "1".Equals(ConfigurationManager.AppSettings["UseLiveDb"]);
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

            if (IsWebService)
            {
                return Invoke("GetBetTypeName", new object[] { bettypeId.ToString(), CurrentTWLanguage, string.Empty, resourceId});
            }

            return RefData.GetBetTypeName(bettypeId.ToString(), CurrentTWLanguage, string.Empty, resourceId);
        }

        public static string GetSportName(object sportId)
        {
            if (IsWebService)
            {
                return Invoke("GetSportName", new object[] { sportId.ToString(), CurrentTWLanguage, string.Empty });
            }

            return RefData.GetSportName(sportId.ToString(), CurrentTWLanguage, string.Empty);
        }

        public static string GetTeamName(object teamId)
        {
            if (IsWebService)
            {
                return Invoke("GetTeamName", new object[] { teamId.ToString(), CurrentTWLanguage, string.Empty });
            }

            return RefData.GetTeamName(teamId.ToString(), CurrentTWLanguage, string.Empty);
        }

        public static string GetRouletteBetItem(object itemId)
        {
            if (IsWebService)
            {
                return Invoke("GetRouletteBetItem", new object[] { itemId.ToString(), CurrentTWLanguage, string.Empty });
            }

            return RefData.GetRouletteBetItem(itemId.ToString(), CurrentTWLanguage, string.Empty);
        }

        public static string GetCasinoBetItem(object betStatusId)
        {
            if (IsWebService)
            {
                return Invoke("GetCasinoBetItem", new object[] { betStatusId.ToString(), CurrentTWLanguage, string.Empty });
            }

            return RefData.GetCasinoBetItem(betStatusId.ToString(), CurrentTWLanguage, string.Empty);
        }

        public static string GetLeagueName(object leagueId)
        {
            if (IsWebService)
            {
                return Invoke("GetLeagueName", new object[] { leagueId.ToString(), CurrentTWLanguage, string.Empty });
            }

            return RefData.GetLeagueName(leagueId.ToString(), CurrentTWLanguage, string.Empty);
        }

        public static string GetFncIndexName(object indexId)
        {
            if (IsWebService)
            {
                return Invoke("GetFncIndexName", new object[] { indexId.ToString(), CurrentTWLanguage });
            }

            return RefData.GetFncIndexName(indexId.ToString(), CurrentTWLanguage);
        }

        public static string GetHorseTeamName(object codeId)
        {
            if (IsWebService)
            {
                return Invoke("GetHorseTeamName", new object[] { codeId.ToString(), CurrentTWLanguage });
            }

            return RefData.GetHorseTeamName(codeId.ToString(), CurrentTWLanguage);
        }

        public static string GetOddsTypeName(object oddsTypeId)
        {
            if (IsWebService)
            {
                return Invoke("GetOddsTypeName", new object[] { oddsTypeId.ToString(), CurrentTWLanguage });
            }

            return RefData.GetOddsTypeName(oddsTypeId.ToString(), CurrentTWLanguage);
        }
        
        private static string Invoke(string functionName, object[] parameters)
        {
            int length = (parameters == null) ? 0 : parameters.Length;
            string[] ps = new string[length + 2];
            if (length > 0)
            {
                parameters.CopyTo(ps, 0);
            }

            ps[length] = functionName;
            ps[length + 1] = Thread.CurrentThread.CurrentCulture.Name;

            string key = string.Join("|", ps);
            string value = velocityCache.Get<string>(key);

            if (string.IsNullOrEmpty(value))
            {
                LiveDataService service = new LiveDataService();
                value = service.InvokeRemoteRef(functionName, parameters);
                velocityCache.Set<string>(key, value);
            }

            return value;
        }
    }
}