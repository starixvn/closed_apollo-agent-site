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
    using Fanex.Logging;

    public class GetBetLimit : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //// Get Data
                IUserSession userSession = UserSession.Current;
                ICustomerSettingService customerService = new CustomerSettingService();
                string currentUserName = userSession.Username;
                int custId = userSession.AgentId;
                int roleId = (int)customerService.GetRoleFromAgentLevel(userSession.AgentLevel);
                int targetRoleId, targetCustId = Converter.ToInt(context.Request["targetCustId"]);
                string agentLevel = context.Request["agentLevel"];
                if (agentLevel != null)
                {
                    targetRoleId = (int)customerService.GetRoleFromAgentLevel(Converter.ToInt(agentLevel));
                }
                else
                {
                    targetRoleId = (int)Role.Member;
                }
                SettingContext settingContext = new SettingContext
                {
                    CustId = custId,
                    TargetCustId = targetCustId,
                    ActorCustId = custId,
                    RoleId = roleId,
                    TargetRoleId = targetRoleId,
                    FormSetting = (int)FormSettingType.BetLimit
                };
                IList<BettingLimit> betLimits = customerService.GetBettingLimitSettingForUpdating(settingContext);

                // Format Language
                CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(userSession.Language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(userSession.Language);
                Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
                Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;
                SetBetLimitLanguage(settingContext, betLimits);

                //// Format Json
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Formatting.Indented;

                //// Response
                context.Response.ContentType = "text/javascript";
                context.Response.Cache.SetNoStore();
                //// Response Bet Limit Object
                context.Response.Write("var betLimits = ");
                context.Response.Write(JsonConvert.SerializeObject(betLimits, settings));
                context.Response.Write(";\n");
                //// Return Setting Context Object
                context.Response.Write("var settingContext = ");
                context.Response.Write(JsonConvert.SerializeObject(settingContext, settings));
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }
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