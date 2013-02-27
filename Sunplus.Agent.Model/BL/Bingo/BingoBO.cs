/*
 * Created 20100116@Lee - Business class of casino function.
 * Revision:
 * 20110519@Qui - Add BingoInjection
 * 
 */

namespace Sunplus.Agent.Model.BL.Bingo
{
    using LiveCasinoLibrary.SBAgent;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Libraries.ThirdPartyTestData;

    public class BingoBO : Sunplus.Agent.Model.BL.Base.BOBase
    {
        private Customer bingoCustomer = null;
        private Report bingoReport = null;

        public BingoBO(string netSessionId)
            : base(netSessionId)
        {
            BingoInjection.Init();
        }

        public Customer Customer
        {
            get
            {
                return this.bingoCustomer;
            }
        }

        public Report Report
        {
            get
            {
                return this.bingoReport;
            }
        }

        #region Utility
        /// <summary>
        /// Return a string which matching with current language
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string BingoLang(string original)
        {
            switch (Sunplus.Agent.Model.Entities.UserSession.Current.Language)
            {
                case "en-US": return original + "_En";
                case "ja-JP": return original + "_Jp";
                case "zh-TW": return original + "_Ct"; // Traditional
                case "zh-CN": return original + "_Cs"; // Simplified
                case "th-TH": return original + "_Th";
                case "ko-KR": return original + "_Ko";
                case "vi-VN": return original + "_Vi";
            }

            return original;
        }
        #endregion
    }

    internal static class BingoInjection
    {
        static BingoInjection()
        {
            BingoAPICaller bingoApiCaller = new BingoAPICaller();
            APICallerManager apiCallerManager = new APICallerManager(bingoApiCaller);
            apiCallerManager.Register(typeof(LiveCasinoLibrary.SBAgent.Report), "WinLoss*|RealTime*");
        }

        public static void Init()
        {
        }

        private static int GetCustId()
        {
            return UserSession.Current.CustId;
        }

        private static int GetUserId()
        {
            return UserSession.Current.UserId;
        }                
    }
}