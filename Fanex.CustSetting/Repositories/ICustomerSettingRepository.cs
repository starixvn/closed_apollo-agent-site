// --------------------------------------------------------------------------
// <copyright file="ICustomerSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.5">Created</change>
// </history>

namespace Fanex.CustSetting.Repositories
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustSetting.Models;
    using Fanex.CustSetting.ViewData;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "EX1003:MethodMustBeDocumented", Justification = "Reviewed.")]
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