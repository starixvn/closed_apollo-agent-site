<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.BetSetting.GetCustomerBetSetting_Handler" %>
namespace Sunplus.Agent.UI._MemberInfo.BetSetting
{
    using System;
    using System.Web;
    using Fanex.PTEngine.BusinessObjects;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Fanex.Data;
    using System.Data;
    using Sunplus.Agent.Model.Entities;
    using System.Web.SessionState;
    using Fanex.PTEngine.Services;
    using System.Globalization;
    using System.Threading;
    using System.Resources;
    
    public class GetCustomerBetSetting_Handler : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            // use english date time format
            CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(UserSession.Current.Language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(UserSession.Current.Language);
            System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;

            context.Response.ContentType = "text/javascript";
            context.Response.Cache.SetNoStore();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            IUserSession userSession = UserSession.Current;

            string currentUserName = userSession.UserName;
            int custId = userSession.CustId;
            int targetCustId = 0;
            int.TryParse(context.Request["targetCustId"], out targetCustId);

            IRequestContext currentContext = new RequestContext();
            IBettingLimit<BettingLimitItem> bettingLimit = new BettingLimit();

            IBettingLimitService bettingLimitService = new BettingLimitService();

            currentContext = new RequestContext()
            {
                FormId = FormType.EditPT,
                UserName = currentUserName.ToUpper(),
                CustId = custId,
                TargetCustId = targetCustId,
                RoleId = (int)UserSessionBase.UserTypes.Super,
                SubAccId = userSession.SubAccId,
                AdminId = userSession.UserId,
                SiteName = userSession.Site,
                Language = userSession.Language,
                LangKey = userSession.LangKey,

            };

            bettingLimit = bettingLimitService.GetBettingForEdit(currentContext);

            // Set language
            bettingLimit = this.SetLanguage(bettingLimit);

            context.Response.Write("var betting = ");
            context.Response.Write(JsonConvert.SerializeObject(bettingLimit, settings));

            context.Response.Write(";\n");
            context.Response.Write("var context = ");
            context.Response.Write(JsonConvert.SerializeObject(currentContext, settings));
        }

        #region Set Language
        /// <summary>
        /// Set language fot PT
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        private IBettingLimit<BettingLimitItem> SetLanguage(IBettingLimit<BettingLimitItem> bettingLimit)
        {
            // Set language for SportType Name on BettingLimitItem
            foreach (BettingLimitItem bettingLimitItem in bettingLimit.BettingLimitItems)
            {
                bettingLimitItem.Name = this.GetLogResource(bettingLimitItem.ResourceKey);
            }
            return bettingLimit;
        }

        /// <summary>
        /// Get Resource from key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetLogResource(string key)
        {
            return _Internal.ResourceManager.GetString(key) ?? string.Empty;
        }

        /// <summary>
        /// Get BetType Name from Resource
        /// </summary>
        /// <param name="bettypeId"></param>
        /// <param name="betTypeName"></param>
        /// <returns></returns>
        private void GetBetTypeName(int bettypeId, ref string betTypeName, ref string bettypeTitle, string resourceKey, string language)
        {
            string oldBetTypeName = betTypeName;
            bettypeTitle = betTypeName = this.GetLogResource(resourceKey);

        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }

    static class _Internal
    {
        public static ResourceManager ResourceManager = null;

        static _Internal()
        {
            ResourceManager = new ResourceManager(typeof(Resources.Lang));
            ResourceManager.IgnoreCase = true;
        }
    }

}