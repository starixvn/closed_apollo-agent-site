// --------------------------------------------------------------------------
// <copyright file="IAdminRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Admin.Repositories.Interface
{
    using System.Collections.Generic;
    using Apollo.Age.Admin.Criterias;
    using Apollo.Age.DTO.Admin.ExchangeRate;

    public interface IExchangeRateRepository
    {
        IEnumerable<ExchRate> GetAll(ref GetAllExchangeRateCriteria criteria);

        ExchRate GetById(ref GetExchangeRateByIdCriteria criteria);

        void UpdateExchangeRate(ref UpdateExchangeRateCriteria criteria);
    }
}