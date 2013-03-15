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

            //IUserSession userSession = UserSession.Current;

            string currentUserName = userSession.UserName;
            int custId = userSession.CustId;
            int targetCustId = 0;
            int targetRoleId = 0;
            //int formType = 0;
            int.TryParse(context.Request["targetCustId"], out targetCustId);
            int.TryParse(context.Request["targetRoleId"], out targetRoleId);
            //int.TryParse(context.Request["formType"], out formType);

            SettingContext settingContext = new SettingContext
            {
                CustId = custId,
                TargetCustId = targetCustId,
                ActorCustId = custId,
                RoleId = (int)Role.Agent,
                TargetRoleId = targetRoleId,
                FormSetting = (int)FormSettingType.Commission
            };

            ICustomerSettingService customerService = new CustomerSettingService();
            CommissionSetting commissionSetting = customerService.GetCommissionSettingForUpdating(settingContext);
            if (targetRoleId == (int)Role.Player)
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