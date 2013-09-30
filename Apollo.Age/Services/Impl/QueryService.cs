// --------------------------------------------------------------------------
// <copyright file="QueryService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.1">Created</change>
//   <change who="Anders.Le" date="2013.4.1">Change name from ReportServiceBase to QueryService</change>
//   <change who="Anders" date="2013.4.8">Change function name from GetReportData to GetData</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Common.Helper;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// This class encapsulate dynamic repository to provide 
    /// a flexible way to get data from db by using Stored Procedure
    /// </summary>
    public class QueryService : ServiceBase
    {
        private readonly IDynamicRepository _repository;

        public QueryService(IDynamicRepository repository = null)
        {
            _repository = repository ?? new DynamicRepository();
        }

        public virtual IEnumerable<T>  GetData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {               
                if (criteria.IsValid())
                {
                    return _repository.Fetch<T>(criteria);
                }

                return new List<T>();
            }
            catch (ServiceLayerException ex)
            {
                ex.SettingKey = criteria.GetSettingKey();                
                HandleException(ex);
                return null;
            }
        }
        
    }
}
