// --------------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.7">Created</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Configurations;
    using Apollo.Age.Entities;
    using Fanex.Common;

    /// <summary>
    /// Service base.
    /// </summary>
    public class ServiceBase
    {
        /// <summary>
        /// Revert Commission Group Name.
        /// </summary>
        /// <param name="commGroupName"> Input commission group name normally (a,b,c,d).</param>
        /// <returns>Return commission group name that need to revert(d,c,b,a).</returns>
        protected string RevertCommGroupName(string commGroupName)
        {
            switch (commGroupName.ToLower())
            {
                case "a":
                    return "D";

                case "b":
                    return "C";

                case "c":
                    return "B";

                case "d":
                    return "A";

                default:
                    return "All";
            }
        }

        /// <summary>
        /// Get Peak hours for prevent action, format peak hour in config file: DAYINWEEK-HOUR.
        /// </summary>
        /// <returns>Peak Hours list.</returns>
        protected IList<PeakHour> GetPeakHours()
        {
            IList<PeakHour> peakHours = new List<PeakHour>();
            var peakHoursString = AppSettings.PeakHours;
            if (!string.IsNullOrEmpty(peakHoursString))
            {
                var peakHourArray = peakHoursString.Split(';');
                foreach (string peakHourSetting in peakHourArray)
                {
                    if (!string.IsNullOrEmpty(peakHourSetting))
                    {
                        var peakHourItem = peakHourSetting.Split('>');
                        var peakHour = new PeakHour
                        {
                            FromDayOfWeek = (DayOfWeek)Converter.ToInt(peakHourItem[0].Split(':')[0]),
                            FromHour = Converter.ToInt(peakHourItem[0].Split(':')[1]),
                            ToDayOfWeek = (DayOfWeek)Converter.ToInt(peakHourItem[1].Split(':')[0]),
                            ToHour = Converter.ToInt(peakHourItem[1].Split(':')[1])
                        };

                        peakHours.Add(peakHour);
                    }
                }
            }

            return peakHours;
        }
    }
}