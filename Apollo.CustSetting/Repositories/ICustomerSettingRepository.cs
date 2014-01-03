// --------------------------------------------------------------------------
// <copyright file="ICustomerSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>

namespace Apollo.CustSetting.Repositories
{
    using System.Collections.Generic;
    using Apollo.CustSetting.Models;
    using Apollo.CustSetting.ViewData;
    using Fanex.NAP.Framework.Data;

    public interface ICustomerSettingRepository
    {
        Result UpdateSetting(ICriteria criteria, ICriteria logCriteria);

        Dictionary<string, PositionTaking> GetCustomerPositionTakingSettingForUpdating(ICriteria criteria);

        Dictionary<string, PositionTaking> GetAgentPositionTakingSettingForUpdating(ICriteria criteria);

        IList<BettingLimit> GetBettingLimitSettingForUpdating(SettingContext settingContext, ICriteria criteria, ref int maxBetLimit);

        IList<CommissionItem> GetAgentCommissionSetingForUpdating(ICriteria criteria);

        IList<CommissionItem> GetCustomerCommissionSetingForUpdating(ICriteria criteria);

        MemberInfo GetMemberInfo(ICriteria criteria);
    }
}