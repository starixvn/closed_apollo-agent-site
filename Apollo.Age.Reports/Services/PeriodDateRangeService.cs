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
            var currentDate = DateTime.Parse(AppSettings.GetValue("CurrentDate")); //DateTime.Now;
            List<string> periodRanges = GetAllPeriodRange();
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;                      
            string[] rangeDates = null;
            DateRangeModel periodModel = new DateRangeModel();
            try
            {
                string[] startRangeDates = periodRanges[0].Split('-');
                DateTime startFromDate = DateTime.Parse(startRangeDates[0]);
                DateTime startToDate = DateTime.Parse(startRangeDates[1]);
                var count = 0;
                foreach (var range in periodRanges)
                {
                    
                    rangeDates = range.Split('-');

                    fromDate = DateTime.Parse(rangeDates[0]);                        
                    toDate = DateTime.Parse(rangeDates[1]);                        

                    if (startFromDate > currentDate && startToDate > currentDate)
                    {
                        periodModel.FromDate = startFromDate;
                        periodModel.ToDate = startToDate;

                        return periodModel;
                    }
                    else
                    {
                        if (getLastPeriod)
                        {
                            if (fromDate <= currentDate && currentDate <= toDate)
                            {

                                periodModel.FromDate = DateTime.Parse(periodRanges[count - 1].Split('-')[0]);
                                periodModel.ToDate = DateTime.Parse(periodRanges[count - 1].Split('-')[1]);

                                return periodModel;
                            }
                        }
                        else
                        {
                            if (fromDate <= currentDate && currentDate <= toDate)
                            {
                                periodModel.FromDate = fromDate;
                                periodModel.ToDate = toDate;

                                return periodModel;
                            }
                        }
                    }

                    count++;
                   
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