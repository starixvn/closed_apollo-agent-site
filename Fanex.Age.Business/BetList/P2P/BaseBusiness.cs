/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.Business.BetList.P2P
{
    using System.Web;

    /// <summary>
    /// BaseBusiness
    /// </summary>
    public class BaseBusiness
    {
        /// <summary>
        /// _siteId
        /// </summary>
        public readonly string SiteId = "dd39ffef491f574213f8adba9dd56240";

        /// <summary>
        /// Gets the stored folder path.
        /// </summary>
        /// <returns></returns>
        protected string GetStoredFolderPath()
        {
            ////string dir = @"D:\Work\10.18.200.213\Samurai\Libs\Fanex.Age.Business\BetList\P2P";
            string dir = HttpContext.Current.Server.MapPath("~") + "\\_BetList\\P2P";
            return dir;
        }
    }
}