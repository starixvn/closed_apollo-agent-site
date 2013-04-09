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
    using System.Linq;
    using Apollo.Age.Admin.Repositories.Impl;
    using Apollo.Age.Admin.Repositories.Interface;
    using Apollo.Age.Admin.ViewModels;
    using Apollo.Age.Admin.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Impl;
    using Fanex.Common.Helper;
    using Apollo.Age.DTO.Admin.ExchangeRate;

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
            return _exchangeRateRepo.GetAllExchangeRate(ref criteria);
        }
    }
}