// --------------------------------------------------------------------------
// <copyright file="ReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.9">Add movebetday and balanceupday</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Linq;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Services.Impl;
    using Fanex.Caching;

    public class ReportService : QueryService
    {
        private const string BalanceUpdayKey = "Apollo.Age.Reports.Services.SpecialDays";

        protected ICache CacheManager
        {
            get
            {
                return CacheService.CreateInstance(CacheType.MemoryCache);
            }
        }

        private dynamic SpecialDays
        {
            get
            {
                dynamic specialDays = CacheManager.Get<dynamic>(BalanceUpdayKey);
                if (specialDays == null)
                {
                    specialDays = this.GetData<dynamic>(new GetSpecialDaysCriteria()).Single();
                    CacheManager.Set<dynamic>(BalanceUpdayKey, specialDays, DateTime.Now.AddMinutes(5));
                }

                return specialDays;
            }
        }

        public DateTime GetBalanceUpDay()
        {
            return SpecialDays.BalanceUpDay;
        }

        public DateTime MoveBetsDay()
        {
            return SpecialDays.MoveBetsDay;
        }
    }
}
