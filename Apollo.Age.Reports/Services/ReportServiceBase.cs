// --------------------------------------------------------------------------
// <copyright file="ReportServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
// </history>


namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;
    using Fanex.Logging;
    using Fanex.Common.Helper;
    

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ReportServiceBase
    {
        private readonly IDynamicRepository _repository;

        public ReportServiceBase(IDynamicRepository repository = null)
        {
            _repository = repository ?? new DynamicRepository();
        }

        /// <summary>
        ///  Get dynamic repository
        /// </summary>
        protected IDynamicRepository Repository
        {
            get { return _repository; }
        }

        protected void HandleException(Exception ex)
        {
            LogService.Error(ex.Message, ex);
        }

        public virtual IEnumerable<T> GetReportData<T>(ICriteria criteria)
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
            catch (Exception ex)
            {
                HandleException(ex);
                return null;
            }
        }
    }
}
