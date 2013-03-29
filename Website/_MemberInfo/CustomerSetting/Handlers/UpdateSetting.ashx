<%@ WebHandler Language="C#" Class="Sunplus.Agent.UI._MemberInfo.CustomerSetting.Handlers.UpdateSetting" %>
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
    using Apollo.Age.Services.Impl;

    public class UpdateSetting : Fanex.Age.UI.TplHandler
    {
        private IList<BettingLimit> _betLimits = null;
        private CommissionSetting _commissionSetting = null;
        private PositionTakingSetting _positionTakingSetting = null;
        private SettingContext _settingContext = null;

        public override void RenderResult(ref JSONData data)
        {
            Result r = View as Result;
            data.AsResult(r, true);
        }

        public override void ProcessRequest(ref Hashtable p)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.Indented;

            string settingContextString = this.Request["settingContext"];
            string betLimitsString = this.Request["betLimits"];
            string commissionSettingString = this.Request["commissionSetting"];
            string positionTakingString = this.Request["positionTakingSetting"];

            if (!string.IsNullOrEmpty(settingContextString))
            {
                _settingContext = JsonConvert.DeserializeObject<SettingContext>(settingContextString, settings);
            }

            if (!string.IsNullOrEmpty(betLimitsString))
            {
                _betLimits = JsonConvert.DeserializeObject<IList<BettingLimit>>(betLimitsString, settings);
            }

            if (!string.IsNullOrEmpty(commissionSettingString))
            {
                _commissionSetting = JsonConvert.DeserializeObject<CommissionSetting>(commissionSettingString, settings);
            }

            if (!string.IsNullOrEmpty(positionTakingString))
            {
                _positionTakingSetting = JsonConvert.DeserializeObject<PositionTakingSetting>(positionTakingString, settings);
            }
        }

        public override object ValidateRequest(Hashtable p)
        {
            if (PermissionService.HasPermissionOnUpdating(CurrentUserSession.AgentId,
                CurrentUserSession.UserId,
                CurrentUserSession.IsMaster))
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            }

            return null;
        }

        public override object InvokeData(Hashtable p)
        {
            if (_betLimits != null && _settingContext != null)
            {
                ICustomerSettingService customerService = new CustomerSettingService();
                Fanex.CustSetting.Models.Result result = null;

                result = customerService.UpdateSetting(_settingContext, _commissionSetting, _betLimits, _positionTakingSetting);

                if (result.ErrCode != 0 && result.ErrCode != Fanex.CustSetting.Core.Constant.WARNING_CODE_DEFAULT)
                {
                    return new Fanex.Age.UI.Result(ErrorType.Unknown, result.ErrMsg);
                }
                else
                {
                    return new Fanex.Age.UI.Result(ErrorType.Success, Resources.Lang.updatesuccessfull);
                }
            }
            else
            {
                return new Result(ErrorType.AccessDenied, Resources.Lang.invalidvalue);
            }
        }
    }
}