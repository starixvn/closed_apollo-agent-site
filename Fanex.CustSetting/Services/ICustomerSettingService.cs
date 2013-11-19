// --------------------------------------------------------------------------
// <copyright file="ICustomerSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.8">Created</change>
// </history>

namespace Fanex.CustSetting.Service
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustSetting.Models;
    using Fanex.CustSetting.ViewData;

    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "EX1003:MethodMustBeDocumented", Justification = "Reviewed.")]
    public interface ICustomerSettingService
    {
        PositionTakingSetting GetPositionTakingSettingForUpdating(SettingContext settingContext);

        IList<BettingLimit> GetBettingLimitSettingForUpdating(SettingContext settingContext, ref int maxBetLimit);

        CommissionSetting GetCommissionSettingForUpdating(SettingContext settingContext, string userGroup = "");

        Result UpdatePositionTakingSetting(SettingContext settingContext, PositionTakingSetting positionTakingSetting);

        Result UpdateBettingLimitSetting(SettingContext settingContext, IList<BettingLimit> bettingLimits);

        Result UpdateCommissionSetting(SettingContext settingContext, CommissionSetting commissionSetting);

        Result UpdateSetting(
            SettingContext settingContext, 
            CommissionSetting commissionSetting, 
            IList<BettingLimit> bettingLimits, 
            PositionTakingSetting positionTakingSetting);

        MemberInfo GetMemberInfo(int memberId);
    }
}