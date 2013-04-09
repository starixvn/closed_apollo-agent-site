// --------------------------------------------------------------------------
// <copyright file="AdminRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Admin.Repositories.Impl
{
    using System.Collections.Generic;
    using Apollo.Age.Admin.Criterias;
    using Apollo.Age.Admin.Repositories.Interface;
    using Apollo.Age.DTO.Admin.ExchangeRate;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using System.Collections;
    using System;

    public class ExchangeRateRepository : IExchangeRateRepository
    {
        public IEnumerable<ExchRate> GetAll(ref GetAllExchangeRateCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        return db.Query<ExchRate>(criteria.GetSpParams());
                    }
                }

                return new List<ExchRate>();
            }
            catch
            {
                throw;
            }
        }

        public ExchRate GetById(ref GetExchangeRateByIdCriteria criteria)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateExchangeRate(ref UpdateExchangeRateCriteria criteria)
        {
            Hashtable parameters = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(parameters);
            }

            criteria.ErrCode = Convert.ToInt32(parameters["errcode"]);
            criteria.ErrMsg = parameters["errmsg"].ToString();
        }
    }
}