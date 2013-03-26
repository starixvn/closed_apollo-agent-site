/*
 * Created 20100116@Lee - Business class of casino function.
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.BL.Casino
{
    using System.Collections;
    using CasinoLibrary.SBAgent;
    using Sunplus.Agent.Model.BL.Authorization;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Libraries.APILogger;
    using Sunplus.Libraries.ThirdPartyTestData;
    using Sunplus.Agent.Model.BL.Base;

    public class CasinoBO : BOBase
    {
        public const int AGENT_SYSTEM = 2;
        public const string CASINO_GROUP1 = "01";
        public const string CASINO_GROUP2 = "02";
        private Customer casinoCustomer = null;
        private Report casinoReport = null;

        public CasinoBO(string netSessionId)
            : base(netSessionId)
        {
            this.casinoCustomer = new Customer();
            this.casinoReport = new Report();

            CasinoInjection.Init();
        }        

        public Customer Customer
        {
            get 
            {
                return this.casinoCustomer;
            }
        }

        public Report Report
        {
            get 
            {
                return this.casinoReport;
            }
        }

        /// <summary>
        /// Return a string which matching with current language
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static string CasinoLang(string original)
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

        public bool IsSynchronized(int custId)
        {
            AuthorizationBO authorization = new AuthorizationBO(NetSessionId);
            Hashtable outputs = new Hashtable();
            authorization.GetCustomerStatus(custId, CurrentSession.AdminId, ref outputs);

            return (bool)outputs["synccasino"];
        }
    }

    internal static class CasinoInjection
    {        
        static CasinoInjection()
        {
            CasinoAPILogger casinoLogger = new CasinoAPILogger(CasinoBO.AGENT_SYSTEM, GetUserId, GetCustId);
            APILogManager apiLogManager = new APILogManager(casinoLogger);
            apiLogManager.Register(typeof(CasinoLibrary.SBAgent.Customer), "Update*|Insert*|AutoInsert*|Move*|Remove*|Reset*|FullSettlementResetMaxTransfer|Rollback*");

            CasinoAPICaller casinoApiCaller = new CasinoAPICaller();
            APICallerManager apiCallerManager = new APICallerManager(casinoApiCaller);
            apiCallerManager.Register(typeof(CasinoLibrary.SBAgent.Report), "WinLoss*|RealTime*");
            apiCallerManager.Register(typeof(CasinoLibrary.SBAgent.Customer), "Get*|Check*");
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