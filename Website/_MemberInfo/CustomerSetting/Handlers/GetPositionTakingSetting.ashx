<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.CustomerSetting.Handlers.GetPositionTakingSetting" %>
namespace Sunplus.Agent.UI._MemberInfo.CustomerSetting.Handlers
{
    using System;
    using System.Web;
    using System.Globalization;
    using System.Threading;
    using Newtonsoft.Json;
    using Sunplus.Agent.Model.Entities;
    using Newtonsoft.Json.Serialization;
    using Fanex.CustSetting.ViewData;
    using Fanex.CustSetting.Service;
    using System.Web.SessionState;
    using System.Resources;
    using Fanex.Common;
    using Fanex.Logging;
    using Fanex.Age.Utility;
    
    public class GetPositionTakingSetting : IHttpHandler, IReadOnlySessionState
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
                int targetCustId = Converter.ToInt(context.Request["targetCustId"]);
                int targetRoleId;
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
                    FormSetting = (int)FormSettingType.PositionTaking,
                    Language = userSession.Language
                };
                PositionTakingSetting positionTakingSetting = customerService.GetPositionTakingSettingForUpdating(settingContext);

                //// Format Language
                CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(userSession.Language);
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(userSession.Language);
                Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
                Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;
                SetPTLanguage(settingContext, positionTakingSetting);

                //// Format Json
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Formatting.Indented;

                //// Response
                context.Response.Write("var positionTakingSetting = ");
                context.Response.Write(JsonConvert.SerializeObject(positionTakingSetting, settings));

                context.Response.Write(";\n");
                context.Response.Write("var settingContext = ");
                context.Response.Write(JsonConvert.SerializeObject(settingContext, settings));
            }
            catch (Exception ex)
            {
                Logs.Error(ex.Message, ex);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region Set Language
        /// <summary>
        /// Set language fot PT
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public void SetPTLanguage(SettingContext settingContext, PositionTakingSetting positionTakingSetting)
        {
            // Set language for SportType Name on SportTypes
            string betTypeName = string.Empty;
            string bettypeTitle = string.Empty;
            foreach (Fanex.CustSetting.ViewData.SportType sportTypeItem in positionTakingSetting.SportTypes)
            {
                sportTypeItem.Name = this.GetResourceData(sportTypeItem.ResourceKey);
                foreach (Fanex.CustSetting.ViewData.BetTypeGroup betTypeGroupItem in sportTypeItem.BetTypeGroups)
                {
                    foreach (Fanex.CustSetting.ViewData.BetType betTypeItem in betTypeGroupItem.BetTypes)
                    {
                        betTypeName = bettypeTitle = betTypeItem.Name;
                        this.GetBetTypeName(betTypeItem.Id, ref betTypeName, ref bettypeTitle, betTypeItem.ResourceKey, settingContext.Language);
                        betTypeItem.Name = betTypeName;
                        betTypeItem.Title = bettypeTitle;
                    }
                }
            }
        }

        /// <summary>
        /// Get Resource from key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetResourceData(string key)
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
            bettypeTitle = betTypeName = this.GetResourceData(resourceKey);
            if (!this.IsTranslationBetTypeName(bettypeId, language))
            {
                betTypeName = oldBetTypeName;
            }
        }

        /// <summary>
        /// This function is check bettype name too long. We don't need translation. Set default English
        /// </summary>
        /// <returns></returns>
        private bool IsTranslationBetTypeName(int bettypeId, string language)
        {
            switch (bettypeId)
            {
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType._1st_Handicap:
                    if (language == "J")
                    {
                        return false;
                    }
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType._1st_Over_Under:
                    if (language == "J")
                    {
                        return false;
                    }
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType._1st_Ball_OU:
                    if (language == "TH")
                    {
                        return false;
                    }
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Correct_Score:
                    if (language == "VI")
                    {
                        return false;
                    }
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Total_Goal:
                    if (language == "VI")
                    {
                        return false;
                    }
                    break;
                case (int)Sunplus.Agent.Model.BL.Definitions.BetType.Mix_Parlay:
                    if (language == "VI")
                    {
                        return false;
                    }
                    break;
                default:
                    return true;
            }

            return true;
        }
        #endregion

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