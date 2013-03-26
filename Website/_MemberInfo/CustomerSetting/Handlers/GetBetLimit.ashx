<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.CustomerSetting.Handlers.GetBetLimit" %>
namespace Sunplus.Agent.UI._MemberInfo.CustomerSetting.Handlers
{
    using System;
    using System.Web;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using Newtonsoft.Json;
    using Sunplus.Agent.Model.Entities;
    using Newtonsoft.Json.Serialization;
    using Fanex.CustSetting.ViewData;
    using Fanex.CustSetting.Service;
    using Sunplus.Agent.Model.BL;
    using System.Web.SessionState;
    using System.Resources;
    using Fanex.Common;

    public class GetBetLimit : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            IUserSession userSession = UserSession.Current;
            CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(userSession.Language);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(userSession.Language);
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;

            context.Response.ContentType = "text/javascript";
            context.Response.Cache.SetNoStore();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;


            string currentUserName = userSession.Username;
            int custId = userSession.AgentId;
            int roleId = 1;
            switch (userSession.AgentLevel)
            {
                case 0:
                    roleId = (int)Role.Super;
                    break;
                case 1:
                    roleId = (int)Role.MainShareHolder;
                    break;
                case 2:
                    roleId = (int)Role.ShareHolder;
                    break;
                case 3:
                    roleId = (int)Role.MainAgent;
                    break;
            }
            int targetCustId = Converter.ToInt(context.Request["targetCustId"]);
            int targetRoleId = 1;
            //int formType = 0;

            switch (Converter.ToInt(context.Request["agentLevel"]))
            {
                case 0:
                    targetRoleId = (int)Role.Super;
                    break;
                case 1:
                    targetRoleId = (int)Role.MainShareHolder;
                    break;
                case 2:
                    targetRoleId = (int)Role.ShareHolder;
                    break;
                case 3:
                    targetRoleId = (int)Role.MainAgent;
                    break;
            }
            //int.TryParse(context.Request["formType"], out formType);

            SettingContext settingContext = new SettingContext
            {
                CustId = custId,
                TargetCustId = targetCustId,
                ActorCustId = custId,
                RoleId = roleId,
                TargetRoleId = targetRoleId,
                FormSetting = (int)FormSettingType.BetLimit
            };
            ICustomerSettingService customerService = new CustomerSettingService();
            IList<BettingLimit> betLimits = customerService.GetBettingLimitSettingForUpdating(settingContext);

            // Set language
            SetBetLimitLanguage(settingContext, betLimits);

            context.Response.Write("var betLimits = ");
            context.Response.Write(JsonConvert.SerializeObject(betLimits, settings));

            context.Response.Write(";\n");
            context.Response.Write("var settingContext = ");
            context.Response.Write(JsonConvert.SerializeObject(settingContext, settings));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public static void SetBetLimitLanguage(SettingContext settingContext, IList<BettingLimit> betLimits)
        {
            foreach (Fanex.CustSetting.ViewData.BettingLimit bettingLimitItem in betLimits)
            {
                bettingLimitItem.Name = GetResourceData(bettingLimitItem.ResourceKey);
            }
        }

        public static string GetResourceData(string key)
        {
            return _Internal.ResourceManager.GetString(key) ?? string.Empty;
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
}