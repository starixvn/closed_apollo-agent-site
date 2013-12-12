// --------------------------------------------------------------------------
// <copyright file="DateRangeModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.10.8">Created</change>
// </history>

namespace Apollo.Age.Reports.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Date range model class.
    /// </summary>
    public class DateRanges
    {
        public int Year { get; set; }

        public List<DateRange> ListOfDateRange { get; set; }
    }

    /// <summary>
    /// Date range model class.
    /// </summary>
    public class DateRange
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
                
    }
}