/*
 * Created 20100827@Lee - Casino API caller class.
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Sunplus.Agent.Model.BL.Casino
{
    using System.Collections.Generic;
    using System.Web;
    using Sunplus.Agent.Model.Services;
    using Sunplus.Libraries.ThirdPartyTestData;
    using System.Configuration;

    public class CasinoAPICaller : IAPICaller
    {
        private static readonly string cookieContainer = "CASINOAPICALLER_COOKIECONTAINER";

        public CasinoAPICaller()
        {
        }

        private System.Net.CookieContainer CookieContainer
        {
            get 
            {
                if (HttpContext.Current.Session[cookieContainer] == null)
                {
                    HttpContext.Current.Session[cookieContainer] = new System.Net.CookieContainer();
                }

                return HttpContext.Current.Session[cookieContainer] as System.Net.CookieContainer;
            }
        }        

        #region IAPICaller Members

        public object Invoke(string apiName, List<object> parametersValue)
        {
            LiveDataService service = new LiveDataService();
            service.CookieContainer = this.CookieContainer;
            return service.InvokeApi(apiName, parametersValue.ToArray());
        }

        public bool IsEnable()
        {
            return ConfigurationManager.AppSettings["UseLiveDb"] == "1";
        }
        
        #endregion
    }
}
