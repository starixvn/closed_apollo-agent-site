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

    public class UpdateSetting : Fanex.Age.UI.TplHandler
    {
        private IList<BettingLimit> betLimits = null;
        private CommissionSetting commissionSetting = null;
        private PositionTakingSetting positionTakingSetting = null;
        private SettingContext settingContext = null;

        public override void RenderResult(ref Fanex.Age.UI.JSONData data)
        {
            Fanex.Age.UI.Result r = View as Fanex.Age.UI.Result;
            data.AsResult(r, true);
        }

        public override void ProcessRequest(ref System.Collections.Hashtable p)
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
                settingContext = JsonConvert.DeserializeObject<SettingContext>(settingContextString, settings);
            }

            if (!string.IsNullOrEmpty(betLimitsString))
            {
                betLimits = JsonConvert.DeserializeObject<IList<BettingLimit>>(betLimitsString, settings);
            }

            if (!string.IsNullOrEmpty(commissionSettingString))
            {
                commissionSetting = JsonConvert.DeserializeObject<CommissionSetting>(commissionSettingString, settings);
            }

            if (!string.IsNullOrEmpty(positionTakingString))
            {
                positionTakingSetting = JsonConvert.DeserializeObject<PositionTakingSetting>(positionTakingString, settings);
            }
        }

        public override object ValidateRequest(System.Collections.Hashtable p)
        {
            //if (!CurrentUserSession.SubPageAccess.CreateAccount || CurrentUserSession.IsInternal)
            //{
            //    return new Fanex.Age.UI.Result(ErrorType.AccessDenied, Resources.Lang.lblNotAuthorized);
            //}

            return null;
        }

        public override object InvokeData(System.Collections.Hashtable p)
        {
            if (betLimits != null && settingContext != null)
            {
                ICustomerSettingService customerService = new CustomerSettingService();
                Fanex.CustSetting.Models.Result result = null;

                result = customerService.UpdateSetting(settingContext, commissionSetting, betLimits, positionTakingSetting);

                if (result.ErrCode != 0 && result.ErrCode != Fanex.CustSetting.Core.Constant.WARNING_CODE_DEFUALT)
                {
                    return new Fanex.Age.UI.Result(ErrorType.Unknown, result.ErrMsg);
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