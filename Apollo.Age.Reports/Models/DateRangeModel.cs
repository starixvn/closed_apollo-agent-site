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

    /// <summary>
    /// Date range model class.
    /// </summary>
    public class DateRangeModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}