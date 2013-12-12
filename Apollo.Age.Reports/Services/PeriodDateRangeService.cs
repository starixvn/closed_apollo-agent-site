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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Services.Impl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class PeriodDateRangeService : QueryService
    {
        /// <summary>
        /// Get list of period date ranges.
        /// </summary>
        /// <returns>List of period date ranges that were formatted.</returns>
        public List<DateRange> PeriodRanges()
        {
            List<DateRanges> data = GetAllPeriodRange();

            var dateRangeNow = new List<DateRange>();

            dateRangeNow = data.Where(x => x.Year == DateTime.Now.Year).Select(y => y.ListOfDateRange).FirstOrDefault();

            return dateRangeNow;
        }

        /// <summary>
        /// Lists the of periods.
        /// </summary>
        /// <returns>List of period date ranges that were formatted.</returns>
        public List<DateRanges> ListOfPeriods()
        {
            return GetAllPeriodRange();   
        }

        /// <summary>
        /// Get period range.
        /// </summary>
        /// <param name="getLastPeriod">Want to get last period or this period.</param>
        /// <returns>
        /// Return period date range.
        /// </returns>
        public DateRange PeriodDates(bool getLastPeriod = false)
        {
            var currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            List<DateRange> periodRanges = PeriodRanges();
            DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateTime toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DateRange periodModel = new DateRange();
            DateTime startFromDate = periodRanges[0].FromDate;
            DateTime startToDate = periodRanges[0].ToDate;
            var count = 0;

            foreach (var range in periodRanges)
            {
                fromDate = range.FromDate;
                toDate = range.ToDate;

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
                            periodModel.FromDate = periodRanges[count - 1].FromDate;
                            periodModel.ToDate = periodRanges[count - 1].ToDate;

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

            return new DateRange
            {
                FromDate = currentDate,
                ToDate = currentDate
            };
        }

        /// <summary>
        /// Gets all period range.
        /// </summary>
        /// <returns>List of all period range.</returns>
        private List<DateRanges> GetAllPeriodRange()
        {
            var listDateRange = new List<DateRanges>();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(HttpContext.Current.Server.MapPath("~/App_Data/PeriodsRange.xml"));
            XmlNodeList periods = xmldoc.SelectNodes("/Periods/Date");
            foreach (XmlNode period in periods)
            {
                XmlNodeList dates = period.ChildNodes;
                var dateRanges = new List<DateRange>();

                foreach (XmlNode date in dates)
                {
                    dateRanges.Add(new DateRange
                    {
                        FromDate = DateTime.Parse(date.Attributes["FromDate"].Value),
                        ToDate = DateTime.Parse(date.Attributes["ToDate"].Value),
                    });
                }

                listDateRange.Add(new DateRanges
                {
                    ListOfDateRange = dateRanges,
                    Year = int.Parse(period.Attributes["Year"].Value)
                });
            }

            return listDateRange;
        }
    }
}