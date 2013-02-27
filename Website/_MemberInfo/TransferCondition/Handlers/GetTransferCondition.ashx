<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.TransferCondition.GetTransferCondition_Handler" %>
namespace Sunplus.Agent.UI._MemberInfo.TransferCondition
{
    using System;
    using System.Web;
    using Fanex.PTEngine.BusinessObjects;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Fanex.Data;
    using System.Data;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.BL.MemberInfo.TransferCondition;
    using System.Web.SessionState;
    using Fanex.PTEngine.Services;
    using System.Globalization;
    using System.Threading;
    using System.Resources;

    public class GetTransferCondition_Handler : IHttpHandler, IReadOnlySessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            context.Response.ContentType = "text/javascript";
            context.Response.Cache.SetNoStore();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            int targetCustId = 0;
            int.TryParse(context.Request["targetCustId"], out targetCustId);

            TransferConditionBO bo = new TransferConditionBO(UserSession.Current.NetSessionId);
            string transferCondition = bo.GetTransferCondition(targetCustId);
            IRequestContext currentContext = new RequestContext();
            currentContext = new RequestContext()
            {
                TargetCustId = targetCustId,
            };

            context.Response.Write("var transfer = ");
            context.Response.Write(JsonConvert.SerializeObject(transferCondition, settings));
            context.Response.Write(";\n");
            context.Response.Write("var context = ");
            context.Response.Write(JsonConvert.SerializeObject(currentContext, settings));
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