<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.VirtualSports.GetCustomerSetting_Handler" %>

namespace Sunplus.Agent.UI._MemberInfo.VirtualSports
{
    using System;
    using System.Web;
    using Sunplus.Agent.Model.Entities;
    using System.Globalization;
    using System.Threading;
    using System.Web.SessionState;
    using System.Collections;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Fanex.PTEngine.BusinessObjects;
    using Fanex.PTVirtualSports.Services;

    /// <summary>
    /// Summary description for GetCustomerSetting
    /// </summary>
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

            IUserSession CurrentUserSession = UserSession.Current;
            string currentUserName = CurrentUserSession.UserName;
            int custId = CurrentUserSession.CustId;
            int targetCustId = 0;
            int targetRoleId = 0;
            int.TryParse(context.Request["targetCustId"], out targetCustId);
            int.TryParse(context.Request["targetRoleId"], out targetRoleId);

            IRequestContext currentContext = new RequestContext
            {
                CustId = CurrentUserSession.CustId,
                TargetCustId = targetCustId,
                TargetRoleId = targetRoleId,
                FormId = FormType.EditPT,
                SubAccId = CurrentUserSession.SubAccId,
                AdminId = CurrentUserSession.AdminId,
                Language = CurrentUserSession.LangKey,
                LangKey = CurrentUserSession.LangKey,
                IP = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP()
            };
            IUserSession userSession = UserSession.Current;

            if (userSession.IsSuper)
            {
                currentContext.RoleId = (int)UserSessionBase.UserTypes.Super;
            }
            else if (userSession.IsMaster)
            {
                currentContext.RoleId = (int)UserSessionBase.UserTypes.Master;
            }
            else if (userSession.IsAgent)
            {
                currentContext.RoleId = (int)UserSessionBase.UserTypes.Agent;
            }

            CustomerService customerService = new CustomerService();
            //Hashtable otherSetting = new Hashtable();
            var customer = customerService.GetCustomer(currentContext);

            Hashtable ht = new Hashtable();
            ht.Add("customer", customer);
            ht.Add("context", currentContext);
            context.Response.Write(SerializeToJs(ht));
        }

        public static string SerializeToJs(Hashtable tb)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return SerializeToJs(tb, settings);
        }

        public static string SerializeToJs(Hashtable tb, JsonSerializerSettings settings)
        {
            string result = string.Empty;

            foreach (var key in tb.Keys)
            {
                var value = tb[key];
                result += "var " + key + " = " + JsonConvert.SerializeObject(value, settings) + ";\n";
            }

            return result;
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