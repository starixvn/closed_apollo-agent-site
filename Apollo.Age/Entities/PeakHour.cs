// --------------------------------------------------------------------------
// <copyright file="PeakHour.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.10.15">Created</change>
// </history>

namespace Apollo.Age.Entities
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PeakHour
    {
        public DayOfWeek FromDayOfWeek { get; set; }

        public int FromHour { get; set; }

        public DayOfWeek ToDayOfWeek { get; set; }

        public int ToHour { get; set; }

        public bool IsSameDay 
        {
            get
            {
                return FromDayOfWeek == ToDayOfWeek;
            }
        }
    }
}