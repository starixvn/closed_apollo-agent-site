<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.CustomerSetting.Handlers.GetCommissionSetting" %>
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
    using Sunplus.Agent.Model.BL;
    using System.Web.SessionState;
    using Fanex.Common;

    public class GetCommissionSetting : IHttpHandler, IReadOnlySessionState
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

            ICustomerSettingService customerService = new CustomerSettingService();
            string currentUserName = userSession.Username;
            int custId = userSession.AgentId;
            int roleId = (int)customerService.GetRoleFromAgentLevel(userSession.AgentLevel);
            int targetCustId = Converter.ToInt(context.Request["targetCustId"]);
            int targetRoleId = (int)customerService.GetRoleFromAgentLevel(Converter.ToInt(context.Request["agentLevel"]));
            
            SettingContext settingContext = new SettingContext
            {
                CustId = custId,
                TargetCustId = targetCustId,
                ActorCustId = custId,
                RoleId = roleId,
                TargetRoleId = targetRoleId,
                FormSetting = (int)FormSettingType.Commission
            };

            
            CommissionSetting commissionSetting = customerService.GetCommissionSettingForUpdating(settingContext);
            if (targetRoleId == (int)Role.Member)
            {
                commissionSetting.UserGroup = context.Request.QueryString["ugroup"].ToString();
            }

            // Set language on PT Object

            context.Response.Write("var customerCommission = ");
            context.Response.Write(JsonConvert.SerializeObject(commissionSetting, settings));

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

    }
}