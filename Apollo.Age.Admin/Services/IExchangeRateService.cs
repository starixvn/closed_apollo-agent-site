// --------------------------------------------------------------------------
// <copyright file="IAdminService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Admin.Services
{
    using System.Collections.Generic;
    using Apollo.Age.Admin.Criterias;
    using Apollo.Age.DTO.Admin.ExchangeRate;

    public interface IExchangeRateService
    {
        IEnumerable<ExchRate> GetAll(ref GetAllExchangeRateCriteria criteria);

        ExchRate GetById(ref GetExchangeRateByIdCriteria criteria);

        void UpdateExchangeRate(ref UpdateExchangeRateCriteria criteria);
    }
}