<%@ WebHandler Language="C#" Class="UpdateCommissionSetting" %>

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

public class UpdateCommissionSetting : Fanex.Age.UI.TplHandler
{
    private CommissionSetting commissionSetting = null;
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

        if (this.Request["settingContext"] != null)
        {
            settingContext = JsonConvert.DeserializeObject<SettingContext>(this.Request["settingContext"].ToString(), settings);
        }

        if (this.Request["commissionSetting"] != null)
        {
            commissionSetting = JsonConvert.DeserializeObject<CommissionSetting>(this.Request["commissionSetting"].ToString(), settings);
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
        if (commissionSetting != null && settingContext != null) 
        {
            ICustomerSettingService customerService = new CustomerSettingService();
            Fanex.CustSetting.Models.Result result = null;

            result = customerService.UpdateCommissionSetting(settingContext, commissionSetting);

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