//using Sunplus.Agent.Model.Common;

/*
 * Created 20091224@Lee - Validating definitions
 * Revision:
 * 20091224@Lee - Initializing
 * 
 * !!!CAUTION: THIS IS GLOBAL DEFINITIONS, ANY REVISION MUST BE APPROVED BEFORE PROCEEDING!!!
 * Please refer to <document url> for more details
 * 
 * 
 */

namespace Fanex.Age.UI
{
    public class Validation
    {
        /// <summary>
        /// Return true if password contains least 2 numbers and 2 letters and have length >= 6.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool CheckValidPassword(string password)
        {
            password = password.ToLower();
            if (password.Length < 6)
            {
                return false;
            }

            bool bValid = false;
            int j1 = -1; 
            int j2 = -1; 
            int j3 = -1;

            for (int i = 0; i < 10; i++)
            {
                j1 = password.IndexOf(i.ToString());
                j2 = password.LastIndexOf(i.ToString());
                if (j1 != j2)
                {
                    bValid = true;
                    break;
                }
                else
                {
                    if (j3 == -1)
                    {
                        j3 = j1;
                        continue;
                    }
                    else if (j3 != -1 && j3 != j1)
                    {
                        bValid = true;
                        break;
                    }
                }
            }

            if (bValid)
            {
                string az = "abcdefghijklmnopqrstuvwxyz";
                int count = 0;
                for (int i = 0; i < password.Length; i++)
                {
                    if (az.Contains(password.Substring(i, 1)))
                    {
                        ++count;
                    }

                    if (count == 2)
                    {
                        bValid = true;
                        break;
                    }
                }

                if (count < 2)
                {
                    bValid = false;
                }
            }

            return bValid;
        }

        /// <summary>
        /// Check is valid language string.
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static bool CheckValidLanguage(string culture)
        {
            return true;
           // return Sunplus.Agent.Model.Common.Language.IsValidLanguage(culture);
        }
    }
}
