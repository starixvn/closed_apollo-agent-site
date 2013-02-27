<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.PositionTaking.MasterUpdateMinPTAgent_Handler" %>

namespace Sunplus.Agent.UI._MemberInfo.PositionTaking
{
    using System;
    using System.Web;
    using Fanex.Age.UI;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.BL.Authorization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Fanex.PTEngine.BusinessObjects;
    using Fanex.PTEngine.DataAccess;
    using System.Collections;
    using Sunplus.Agent.Model.Entities;
    using Fanex.PTEngine.Services;
    using Sunplus.Agent.Model.Entities.Permission;
    using System.Data;

    public class MasterUpdateMinPTAgent_Handler : Fanex.Age.UI.TplHandler
    {
        private string strCustomer = string.Empty;
        private string parameter = string.Empty;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Fanex.Age.UI.Result r = View as Fanex.Age.UI.Result;
            data.AsResult(r, true);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            if (Request["txtCustomerObject"] != null)
            {
                strCustomer = Request["txtCustomerObject"];
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (!CurrentUserSession.SubPageAccess.CreateAccount || CurrentUserSession.IsInternal)
            {
                return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (!string.IsNullOrEmpty(strCustomer))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Formatting.Indented;

                Customer customer = JsonConvert.DeserializeObject<Customer>(strCustomer, settings);
                IRequestContext requestContext = new RequestContext()
                {
                    CustId = CurrentUserSession.CustId,
                    RoleId = (int)UserSessionBase.UserTypes.Master,
                    TargetRoleId = (int)UserSessionBase.UserTypes.Agent,
                    FormId = FormType.EditMinPT,
                    SubAccId = CurrentUserSession.SubAccId,
                    AdminId = CurrentUserSession.UserId,
                    IP = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    Language = CurrentUserSession.Language
                };

                ICustomerService customerService = new CustomerService();

                Fanex.PTEngine.BusinessObjects.Result result = customerService.MasterUpdateMinPTAgent(customer, requestContext);

                string strMessage = string.Empty;
                bool isCallAPI = false;
                int error = CommonGenerateHTML.GenerateErrorMsg(result.ErrorCode, result.ErrorMessage, ref strMessage, string.Empty, ref isCallAPI);

                if (error != 0)
                {
                    return new Fanex.Age.UI.Result(ErrorType.Unknown, strMessage);
                }
                else
                {
                    return new Fanex.Age.UI.Result(ErrorType.Success, Resources.Lang.updatesuccessfull);
                }
            }

            return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
        }
    }
}