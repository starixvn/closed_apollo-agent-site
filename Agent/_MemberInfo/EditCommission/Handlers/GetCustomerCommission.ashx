<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.EditCommission.GetMemberCommission_Handler" %>
namespace Sunplus.Agent.UI._MemberInfo.EditCommission
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
    using Sunplus.Agent.Model.BL.MemberInfo.EditCommission;

    public class GetMemberCommission_Handler : IHttpHandler, IReadOnlySessionState
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

            IRequestContext currentContext = new RequestContext()
            {
                UserName = currentUserName.ToUpper(),
                CustId = custId,
                TargetCustId = targetCustId,
                RoleId = CurrentUserSession.RoleId,
                TargetRoleId = CurrentUserSession.RoleId - 1,
                FormId = FormType.Add,
                SubAccId = CurrentUserSession.SubAccId,
                AdminId = CurrentUserSession.UserId,
                SiteName = CurrentUserSession.Site,
                Language = CurrentUserSession.Language,
                LangKey = CurrentUserSession.LangKey
            };

            EditCommissionBO bo = new EditCommissionBO(CurrentUserSession.NetSessionId);
            ICommissionSetting commission = bo.GetCommission(currentContext);

            Hashtable view = new Hashtable();
            view.Add("commission", commission);
            view.Add("context", currentContext);
            context.Response.Write(SerializeToJs(view));
        }

        public static string SerializeToJs(Hashtable view)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            return SerializeToJs(view, settings);
        }

        public static string SerializeToJs(Hashtable view, JsonSerializerSettings settings)
        {
            string result = string.Empty;

            foreach (var key in view.Keys)
            {
                var value = view[key];
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