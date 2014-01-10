// --------------------------------------------------------------------------
// <copyright file="DateRangesModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.12.13">Created</change>
// </history>

namespace Apollo.Age.Reports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Date range model class.
    /// </summary>
    public class DateRangesModel
    {
        public int Year { get; set; }

        public List<DateRange> ListOfDateRange { get; set; }
    }

    /// <summary>
    /// Date range model class.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]
    public class DateRange
    {
        public int PeriodNumber { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}