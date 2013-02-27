<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.PositionTaking.SaveNewMember_Handler" %>
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
    using Sunplus.Agent.Model.Security;

    public class SaveNewMember_Handler : Fanex.Age.UI.TplHandler
    {
        private string customerObject = string.Empty;
        private string captchaCode = string.Empty;
        private string parameter = string.Empty;
        private HttpFrequenceRule rule;

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
            if (this.Request["captchaCode"] != null)
            {
                captchaCode = this.Request["captchaCode"];
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            // Check Captcha
            rule = new HttpFrequenceRule("SaveNewMember.ashx", 19, TimeSpan.FromDays(2));
            if (rule.IsViolated)
            {
                if (captchaCode != CurrentUserSession.CaptchaCode)
                {
                    return new Fanex.Age.UI.Result(5, Resources.Lang.errInvalidCaptcha);
                }
                else
                {
                    rule.Reset();
                }
            }
            // End check Captcha
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
                    TargetCustId = 0,
                    RoleId = (int)UserSessionBase.UserTypes.Agent,
                    TargetRoleId = (int)UserSessionBase.UserTypes.Member,
                    FormId = FormType.Add,
                    SubAccId = CurrentUserSession.SubAccId,
                    AdminId = CurrentUserSession.UserId,
                    IP = Sunplus.Agent.Model.Common.AgentIPFormat.GetClientIP(),
                    Language = CurrentUserSession.Language
                };

                int memberId = -1;
                ICustomerService customerService = new CustomerService();

                Fanex.PTEngine.BusinessObjects.Result result = customerService.AddNewMember(customer, requestContext, out memberId);

                string s = char.ConvertFromUtf32(7);

                string[] errCodes = result.ErrorCode.Split(new string[] { s }, StringSplitOptions.RemoveEmptyEntries);
                string[] errMsgs = result.ErrorMessage.Split(new string[] { s }, StringSplitOptions.RemoveEmptyEntries);

                string message = string.Empty;
                for (int i = 0; i < errMsgs.Length; i++)
                {
                    if (i < errMsgs.Length - 1)
                    {
                        message += errMsgs[i] + "<br/>";
                    }
                    else
                    {
                        message += errMsgs[i];
                    }
                }

                if (!string.IsNullOrEmpty(message))
                {
                    return new Fanex.Age.UI.Result(ErrorType.Unknown, message);
                }
                else
                {
                    string isDisableFI = "1";
                    string isDisableHR = UserPermission.IsShowRacing ? "0" : "1";
                    string isDisableBG = UserPermission.IsShowBingo ? "0" : "1";
                    string isDisableLiveCS = UserPermission.IsShowLiveCasino ? "0" : "1";
                    string isDisableCas = "0";

                    Fanex.Data.DNAide.DbResult dr = AccountInfo.Age_CustomerSelStatus(CurrentUserSession.CustId);
                    DataRow dataRow = dr.ResultSet.Tables[0].Rows[0];

                    bool isStatusCS = false;
                    if (UserPermission.IsShowCasino)
                    {
                        Session[Globals.SESSION_CUSTOMER] = DAL.IBCClient.IBCLibProvider.Instance.GetCustInfoIBC(Convert.ToInt32(dr.Outputs["custid"]));
                        isStatusCS = true;
                    }

                    if (!(bool)(dataRow["DisableCasino"]) && !isStatusCS)
                    {
                        isDisableCas = "1";
                    }

                    // Count to check Captcha
                    rule.Hit();
                    parameter = customer.UserName + "," + memberId.ToString() + ","
                                + isDisableFI + "," + isDisableCas + "," + isDisableHR + ","
                                + isDisableBG + "," + isDisableLiveCS;
                    return new Fanex.Age.UI.Result(ErrorType.Success, parameter);
                }
            }
            return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
        }
    }
}