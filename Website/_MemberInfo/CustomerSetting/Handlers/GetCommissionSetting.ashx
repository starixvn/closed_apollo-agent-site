<%@ WebHandler Language="C#" Class="GetCommissionSetting" %>

using System;
using System.Web;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json;
using Sunplus.Agent.Model.Entities;
using Newtonsoft.Json.Serialization;
using Fanex.CustSetting.ViewData;
using Fanex.CustSetting.Service;

public class GetCommissionSetting : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
       CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        //Thread.CurrentThread.CurrentUICulture = new CultureInfo(UserSession.Current.Language);
        //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(UserSession.Current.Language);
        //Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
        //Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;

        context.Response.ContentType = "text/javascript";
        context.Response.Cache.SetNoStore();

        JsonSerializerSettings settings = new JsonSerializerSettings();
        settings.NullValueHandling = NullValueHandling.Ignore;
        settings.Formatting = Formatting.Indented;

        //IUserSession userSession = UserSession.Current;

        //string currentUserName = userSession.UserName;
        //int custId = userSession.CustId;
        int targetCustId = 0;
        int targetRoleId = 0;
        //int formType = 0;
        int.TryParse(context.Request["targetCustId"], out targetCustId);
        int.TryParse(context.Request["targetRoleId"], out targetRoleId);
        //int.TryParse(context.Request["formType"], out formType);

        SettingContext settingContext = new SettingContext
        {
            CustId = 1,
            TargetCustId = targetCustId,
            RoleId = (int)Role.Agent,
            TargetRoleId = targetRoleId
        };
        ICustomerSettingService customerService = new CustomerSettingService();
        CommissionSetting commissionSetting = null;
        switch (targetRoleId)
        {
            case (int)Role.Agent:
                commissionSetting = customerService.GetCommissionSettingForUpdating(settingContext);
                break;
                
            case (int)Role.Player:
                commissionSetting = customerService.GetCommissionSettingForUpdating(settingContext, context.Request["ugroup"].ToString());
                break;
        }

        // Set language on PT Object

        context.Response.Write("var customerCommission = ");
        context.Response.Write(JsonConvert.SerializeObject(commissionSetting, settings));

        context.Response.Write(";\n");
        context.Response.Write("var settingContext = ");
        context.Response.Write(JsonConvert.SerializeObject(settingContext, settings));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}