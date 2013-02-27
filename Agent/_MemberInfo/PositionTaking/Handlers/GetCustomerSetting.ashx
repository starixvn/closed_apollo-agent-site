<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.PositionTaking.GetCustomerSetting_Handler" %>

namespace Sunplus.Agent.UI._MemberInfo.PositionTaking
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

    public class GetCustomerSetting_Handler : IHttpHandler, IReadOnlySessionState
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
            int targetRoleId = 0;
            int formType = 0;
            int.TryParse(context.Request["targetCustId"], out targetCustId);
            int.TryParse(context.Request["targetRoleId"], out targetRoleId);
            int.TryParse(context.Request["formType"], out formType);
            IRequestContext currentContext = new RequestContext();
            ICustomer customer = new Customer();
            customer.PtSetting = new PTGrid();
            ICustomerService customerService = new CustomerService();

            if (userSession.IsSuper)
            {
                currentContext = new RequestContext()
                {
                    UserName = currentUserName.ToUpper(),
                    CustId = custId,
                    TargetCustId = targetCustId,
                    RoleId = (int)UserSessionBase.UserTypes.Super,
                    TargetRoleId = targetRoleId,
                    FormId = FormType.Add,
                    SubAccId = userSession.SubAccId,
                    AdminId = userSession.UserId,
                    SiteName = userSession.Site,
                    Language = userSession.Language,
                    LangKey = userSession.LangKey,

                };

                if (targetCustId > 0 && targetRoleId == (int)UserSessionBase.UserTypes.Master) // Super edit master
                {
                    if (formType == 2)
                    {
                        currentContext.FormId = FormType.EditMinPT;
                        customer = customerService.GetCustomerForSuperEditMinPTMaster(currentContext);
                        customer.BettingLimit = null;
                        customer.StatusSetting = null;
                        customer.CommissionSetting = null;
                        customer.CommissionSetting2 = null;
                    }
                    else
                    {
                        currentContext.FormId = FormType.EditPT;
                        customer = customerService.GetCustomerForSuperEditMaster(currentContext);
                    }
                }
                else if (targetCustId > 0 && targetRoleId == (int)UserSessionBase.UserTypes.Member) // Super edit Member
                {
                    int agentId = 0;
                    int.TryParse(context.Request["agentId"], out agentId);
                    currentContext.AgentId = agentId;
                    currentContext.FormId = FormType.EditPT;
                    customer = customerService.GetCustomerForSuperEditMember(currentContext);
                }
                else
                {
                    customer = customerService.GetCustomerForAddNewMaster(currentContext);
                }
            }
            else if (userSession.IsMaster)
            {
                currentContext = new RequestContext()
                {
                    UserName = currentUserName.ToUpper(),
                    CustId = custId,
                    TargetCustId = targetCustId,
                    RoleId = (int)UserSessionBase.UserTypes.Master,
                    TargetRoleId = targetRoleId,
                    FormId = FormType.Add,
                    SubAccId = userSession.SubAccId,
                    AdminId = userSession.UserId,
                    SiteName = userSession.Site,
                    Language = userSession.Language,
                    LangKey = userSession.LangKey,
                };
                if (targetCustId > 0 && targetRoleId == (int)UserSessionBase.UserTypes.Agent) // Master edit agent
                {
                    if (formType == 2)
                    {
                        currentContext.FormId = FormType.EditMinPT;
                        customer = customerService.GetCustomerForMasterEditMinPTAgent(currentContext);
                        customer.BettingLimit = null;
                        customer.StatusSetting = null;
                        customer.CommissionSetting = null;
                        customer.CommissionSetting2 = null;
                    }
                    else
                    {
                        currentContext.FormId = FormType.EditPT;
                        customer = customerService.GetCustomerForMasterEditAgent(currentContext);
                    }                  
                }
                else if (targetCustId > 0 && targetRoleId == (int)UserSessionBase.UserTypes.Member) // Master edit Member
                {
                    int agentId = 0;
                    int.TryParse(context.Request["agentId"], out agentId);
                    currentContext.AgentId = agentId;
                    currentContext.FormId = FormType.EditPT;
                    customer = customerService.GetCustomerForMasterEditMember(currentContext);
                }
                else
                {
                    customer = customerService.GetCustomerForAddNewAgent(currentContext);
                }
            }
            else if (userSession.IsAgent)
            {
                currentContext = new RequestContext()
                {
                    UserName = currentUserName.ToUpper(),
                    CustId = custId,
                    TargetCustId = targetCustId,
                    RoleId = (int)UserSessionBase.UserTypes.Agent,
                    TargetRoleId = targetRoleId,
                    FormId = FormType.Add,
                    SubAccId = userSession.SubAccId,
                    AdminId = userSession.UserId,
                    CurrencyId = userSession.CurrencyId,
                    SiteName = userSession.Site,
                    Language = userSession.Language,
                    LangKey = userSession.LangKey,
                };
                if (targetCustId > 0 && targetRoleId == (int)UserSessionBase.UserTypes.Member) // Agent edit member
                {
                    currentContext.FormId = FormType.EditPT;
                    customer = customerService.GetCustomerForAgentUpdateMember(currentContext);
                }
                else
                {
                    customer = customerService.GetCustomerForAddNewMember(currentContext);
                }
            }
            // Set language on PT Object
            customer = this.SetLanguage(customer, formType);

            context.Response.Write("var customer = ");
            context.Response.Write(JsonConvert.SerializeObject(customer, settings));

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
        private ICustomer SetLanguage(ICustomer customer, int formType)
        {
            // Set language for SportType Name on BettingLimitItem
            if (formType != 2)
            {
                foreach (BettingLimitItem bettingLimitItem in customer.BettingLimit.BettingLimitItems)
                {
                    bettingLimitItem.Name = this.GetLogResource(bettingLimitItem.ResourceKey);
                }
            }
            // Set language for SportType Name on SportTypes
            string betTypeName = string.Empty;
            string bettypeTitle = string.Empty;
            foreach (Fanex.PTEngine.BusinessObjects.SportType sportTypeItem in customer.PtSetting.SportTypes)
            {
                sportTypeItem.Name = this.GetLogResource(sportTypeItem.ResourceKey);

                // Set language for BetType Name
                foreach (Fanex.PTEngine.BusinessObjects.BetTypeGroupSet betTypeGroupSetItem in sportTypeItem.BetTypeGroupSets)
                {
                    foreach (Fanex.PTEngine.BusinessObjects.BetTypeGroup betTypeGroupItem in betTypeGroupSetItem.BetTypeGroups)
                    {
                        foreach (Fanex.PTEngine.BusinessObjects.BetType betTypeItem in betTypeGroupItem.BetTypes)
                        {
                            betTypeName = bettypeTitle = betTypeItem.Name;
                            this.GetBetTypeName(betTypeItem.Id, ref betTypeName, ref bettypeTitle, betTypeItem.ResourceKey, customer.Language);
                            betTypeItem.Name = betTypeName;
                            betTypeItem.Title = bettypeTitle;
                        }
                    }
                }
            }
            return customer;
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
            switch(bettypeId)
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