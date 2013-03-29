<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.CustomerSetting.Handlers.UpdateCommissionSetting" %>
namespace Sunplus.Agent.UI._MemberInfo.CustomerSetting.Handlers
{
    using System;
    using System.Web;
    using Fanex.Age.UI;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.BL.Authorization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Collections;
    using Sunplus.Agent.Model.Entities.Permission;
    using System.Data;
    using Fanex.CustSetting.ViewData;
    using System.Collections.Generic;
    using Fanex.CustSetting.Service;
    using Fanex.Age.UI;
    using Apollo.Age.Services.Impl;
    using Fanex.CustSetting.Core;

    public class UpdateCommissionSetting : Fanex.Age.UI.TplHandler
    {
        private CommissionSetting _commissionSetting = null;
        private SettingContext _settingContext = null;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r, true);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;

            string settingContextString = this.Request["settingContext"];
            string commissionSettingString = this.Request["commissionSetting"];

            if (!string.IsNullOrEmpty(settingContextString))
            {
                _settingContext = JsonConvert.DeserializeObject<SettingContext>(settingContextString, settings);
            }

            if (!string.IsNullOrEmpty(commissionSettingString))
            {
                _commissionSetting = JsonConvert.DeserializeObject<CommissionSetting>(commissionSettingString, settings);
            }

        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            if (PermissionService.HasPermissionOnUpdating(CurrentUserSession.AgentId,
                CurrentUserSession.UserId,
                CurrentUserSession.IsMaster))
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {

            if (_commissionSetting != null && _settingContext != null)
            {
                ICustomerSettingService customerService = new CustomerSettingService();
                Fanex.CustSetting.Models.Result result = null;

                result = customerService.UpdateCommissionSetting(_settingContext, _commissionSetting);

                if (result.ErrCode != 0 && result.ErrCode != Constant.WARNING_CODE_DEFAULT)
                {
                    return new Result(ErrorType.Unknown, result.ErrMsg);
                }
                else
                {
                    return new Result(ErrorType.Success, Resources.Lang.updatesuccessfull);
                }
            }
            else
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.invalidvalue);
            }
        }
    }
}