// --------------------------------------------------------------------------
// <copyright file="AdminService.cs" company="Nexcel Solutions Vietnam">
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
    using Apollo.Age.Admin.Repositories.Impl;
    using Apollo.Age.Admin.Repositories.Interface;
    using Apollo.Age.DTO.Admin.ExchangeRate;
    using Apollo.Age.Services.Impl;

    public class ExchangeRateService : ServiceBase, IExchangeRateService
    {
        private readonly IExchangeRateRepository _exchangeRateRepo;

        public ExchangeRateService() : this(null) { }

        public ExchangeRateService(IExchangeRateRepository exchangeRateRepo)
        {
            _exchangeRateRepo = exchangeRateRepo ?? new ExchangeRateRepository();
        }

        public IEnumerable<ExchRate> GetAll(ref GetAllExchangeRateCriteria criteria)
        {
            return _exchangeRateRepo.GetAll(ref criteria);
        }

        public ExchRate GetById(ref GetExchangeRateByIdCriteria criteria)
        {
            return _exchangeRateRepo.GetById(ref criteria);
        }

        public void UpdateExchangeRate(ref UpdateExchangeRateCriteria criteria)
        {
            _exchangeRateRepo.UpdateExchangeRate(ref criteria);
        }
    }
}