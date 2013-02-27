<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.PositionTaking.SuperUpdateMaster_Handler" %>
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
    using Fanex.PTEngine.Services;
    using Sunplus.Agent.Model.Entities.Permission;
    using System.Data;

    public class SuperUpdateMaster_Handler : Fanex.Age.UI.TplHandler
    {
        private string customerObject = string.Empty;
        private string parameter = string.Empty;
        private decimal newValueMemberPositionTaking = 0;
        private bool isUpdateAllMember = false;
        private Hashtable updateParameters = new Hashtable();

        private void UpdateStatusCasino(Customer customer, bool isCallAPI)
        {
            if (isCallAPI)
            {
                if (UserPermission.IsShowCasino)
                {

                    string strModifyBy = CurrentUserSession.UserName;

                    if (CurrentUserSession.SubAccId > 0)
                    {
                        strModifyBy = CurrentUserSession.SubUserName;
                    }

                    // Sync data to Casino
                    CasinoLibrary.SBAgent.Customer casino = new CasinoLibrary.SBAgent.Customer();
                    if (customer.StatusSetting.SyncCasino)
                    {
                        // Store data for casino section
                        casino.UpdateCustInfo(customer.CustId, customer.FirstName, customer.LastName, customer.Phone,
                                                customer.MobilePhone, customer.Fax, customer.StatusSetting.Closed,
                                                customer.StatusSetting.Suspended, this.Request.UserHostAddress, CurrentUserSession.CustId, strModifyBy, Sunplus.Agent.Model.BL.Casino.CasinoBO.AGENT_SYSTEM);
                    }
                }
            }
        }

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Fanex.Age.UI.Result r = View as Fanex.Age.UI.Result;
            data.AsResult(r, true);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            if (this.Request["txtCustomerObject"] != null)
            {
                customerObject = this.Request["txtCustomerObject"];
            }

            if (this.Request["memberPositionTaking"] != null)
            {
                string[] memberPositionTaking = this.Request["memberPositionTaking"].Split(';');
                newValueMemberPositionTaking = Convert.ToDecimal(memberPositionTaking[0]);
                isUpdateAllMember = memberPositionTaking[1] == "1" ? true : false;
            }

            if (this.Request["updateParameters"] != null)
            {
                var tmp = this.Request["updateParameters"];
                for (var i = 0; i < tmp.Length; i++)
                {
                    updateParameters.Add(i, tmp[i].ToString());
                }
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
            if (!string.IsNullOrEmpty(customerObject))
            {
                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Formatting.Indented;

                Customer customer = JsonConvert.DeserializeObject<Customer>(customerObject, settings);
                IRequestContext requestContext = new RequestContext()
                {
                    CustId = CurrentUserSession.CustId,
                    RoleId = (int)UserSessionBase.UserTypes.Super,
                    TargetRoleId = (int)UserSessionBase.UserTypes.Master,
                    FormId = FormType.EditPT,
                    SubAccId = CurrentUserSession.SubAccId,
                    AdminId = CurrentUserSession.UserId,
                    IP = Request.UserHostAddress.ToString(),
                    Language = CurrentUserSession.Language
                };

                ICustomerService customerService = new CustomerService();
                Fanex.PTEngine.BusinessObjects.Result result = new Fanex.PTEngine.BusinessObjects.Result();

                if (updateParameters.Count > 0)
                {
                    result = customerService.SuperUpdateMultipleMasters(customer, requestContext, isUpdateAllMember, newValueMemberPositionTaking, updateParameters);
                }
                else
                { 
                    result = customerService.SuperUpdateMaster(customer, requestContext, isUpdateAllMember, newValueMemberPositionTaking);
                }
                
                string message = string.Empty;
                bool isCallAPI = false;
                int error = CommonGenerateHTML.GenerateErrorMsg(result.ErrorCode, result.ErrorMessage, ref message, string.Empty, ref isCallAPI);
                this.UpdateStatusCasino(customer, isCallAPI);

                if (error != 0)
                {
                    return new Fanex.Age.UI.Result(ErrorType.Unknown, message);
                }
                else
                {
                    return new Fanex.Age.UI.Result(ErrorType.Success, parameter);
                }
            }
            
            return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
        }
    }
}