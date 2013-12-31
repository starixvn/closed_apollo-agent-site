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
    using Apollo.Age.Configurations;
    using Apollo.Age.Reports.Models;
    using Apollo.Age.Services.Impl;

    /// <summary>
    /// Period date range service.
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
            List<DateRangesModel> data = GetAllPeriodRange();
            data = data.Where(x => x.Year >= AppSettings.PeriodDateRange.AddYears(-1).Year).ToList();
            var dateRangeNow = new List<DateRange>();

            foreach (var item in data)
            {
                foreach (var range in item.ListOfDateRange)
                {
                    dateRangeNow.Add(new DateRange
                    {
                        FromDate = range.FromDate,
                        ToDate = range.ToDate
                    });
                }
            }

            return dateRangeNow;
        }

        /// <summary>
        /// Lists the of periods.
        /// </summary>
        /// <returns>List of period date ranges that were formatted.</returns>
        public List<DateRangesModel> ListOfPeriods()
        {
            List<DateRangesModel> ranges = GetAllPeriodRange();

            if (ranges.Count > 2)
            {
                return ranges.Where(x => x.Year >= AppSettings.PeriodDateRange.AddYears(-1).Year).ToList();
            }

            return ranges;
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
            var currentDate = new DateTime(AppSettings.PeriodDateRange.Year, AppSettings.PeriodDateRange.Month, AppSettings.PeriodDateRange.Day);
            List<DateRange> periodRanges = PeriodRanges();
            DateTime fromDate = new DateTime(AppSettings.PeriodDateRange.Year, AppSettings.PeriodDateRange.Month, AppSettings.PeriodDateRange.Day);
            DateTime toDate = new DateTime(AppSettings.PeriodDateRange.Year, AppSettings.PeriodDateRange.Month, AppSettings.PeriodDateRange.Day);
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
                            if (count <= 0)
                            {
                                periodModel.FromDate = fromDate;
                                periodModel.ToDate = toDate;
                            }
                            else
                            {
                                periodModel.FromDate = periodRanges[count - 1].FromDate;
                                periodModel.ToDate = periodRanges[count - 1].ToDate;
                            }

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
        private List<DateRangesModel> GetAllPeriodRange()
        {
            var listDateRange = new List<DateRangesModel>();

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

                listDateRange.Add(new DateRangesModel
                {
                    ListOfDateRange = dateRanges,
                    Year = int.Parse(period.Attributes["Year"].Value)
                });
            }

            return listDateRange;
        }
    }
}