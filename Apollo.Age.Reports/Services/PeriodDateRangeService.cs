// --------------------------------------------------------------------------
// <copyright file="PeriodDateRangeService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.10.8">Created</change>
// </history>
namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Configurations;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Services.Impl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PeriodDateRangeService : QueryService
    {
        /// <summary>
        /// Get period range.
        /// </summary>
        /// <param name="getLastPeriod">Want to get last period or this period.</param>
        /// <returns>
        /// Return period date range.
        /// </returns>
        public DateRangeModel PeriodRange(bool getLastPeriod = false)
        {
            var currentDate = DateTime.Now;
            List<string> periodRanges = GetAllPeriodRange();
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            string[] rangeDates = null;
            DateRangeModel periodModel = new DateRangeModel();

            try
            {
                foreach (var range in periodRanges)
                {
                    rangeDates = range.Split('-');

                    fromDate = new DateTime(DateTime.Now.Year, Convert.ToInt32(rangeDates[0].Split('/')[0]), Convert.ToInt32(rangeDates[0].Split('/')[1]));
                    toDate = new DateTime(DateTime.Now.Year, Convert.ToInt32(rangeDates[1].Split('/')[0]), Convert.ToInt32(rangeDates[1].Split('/')[1]));
                    if (getLastPeriod)
                    {
                        if (fromDate.Month == DateTime.Now.AddMonths(-1).Month && fromDate.Day <= DateTime.Now.Day)
                        {
                            periodModel.FromDate = fromDate;
                            periodModel.ToDate = toDate;

                            return periodModel;
                        }
                    }
                    else
                    {
                        if (fromDate.Month == DateTime.Now.Month && fromDate.Day <= DateTime.Now.Day)
                        {
                            periodModel.FromDate = fromDate;
                            periodModel.ToDate = toDate;

                            return periodModel;
                        }
                    }
                }

                return new DateRangeModel();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return new DateRangeModel();
            }
        }                

        /// <summary>
        /// Gets all period range.
        /// </summary>
        /// <returns>List of all period range.</returns>
        private List<string> GetAllPeriodRange()
        {
            if (!string.IsNullOrWhiteSpace(AppSettings.PeriodDateRange))
            {
                string periodDateRange = AppSettings.PeriodDateRange;
                return periodDateRange.Split(';').ToList();
            }

            return new List<string>();
        }
    }
}