/*
 * Created 20091224@Lee - Common definitions
 * Revision:
 * 20091224@Lee - Initializing
 * 
 * !!!CAUTION: THIS IS GLOBAL DEFINITIONS, ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
 * 
 * 
 */

namespace Sunplus.Agent.Model.Common
{
    public class Language
    {
        private static string[] languages = new string[] { "en-US", "zh-TW", "ja-JP", "th-TH", "zh-CN", "ko-KR", "vi-VN" };

        // E,J,CT,CS,TH,KO.
        private static string[] languagesKeys = new string[] { "E", "CT", "J", "TH", "CS", "KO", "VI" };

        /// <summary>
        /// Check is valid language string.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static bool IsValidLanguage(string culture)
        {
            int c = languages.Length;
            for (int i = 0; i < c; i++)
            {
                if (languages[i] == culture)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get language key from culture.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string LangKeyFromCulture(string culture)
        {
            int c = languages.Length;
            for (int i = 0; i < c; i++)
            {
                if (languages[i] == culture)
                {
                    return languagesKeys[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Get culture from language key.
        /// </summary>
        /// <param name="langKey"></param>
        /// <returns></returns>
        public static string CultureFromLangKey(string langKey)
        {
            int c = languagesKeys.Length;
            for (int i = 0; i < c; i++)
            {
                if (languagesKeys[i] == langKey)
                {
                    return languages[i];
                }
            }

            return null;
        }
    }
}