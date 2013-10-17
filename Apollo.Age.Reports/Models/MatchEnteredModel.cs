// --------------------------------------------------------------------------
// <copyright file="MatchEnteredModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.11">Created</change>
// </history>

namespace Apollo.Age.Reports.Models
{
    using System;
    using Apollo.Age.Services;

    /// <summary>
    /// Match entered model class.
    /// </summary>
    public class MatchEnteredModel
    {
        public short SportTypeId { get; set; }

        public int YesterdayEntered { get; set; }

        public int YesterdayNonEntered { get; set; }

        public int TodayEntered { get; set; }

        public int TodayNonEntered { get; set; }

        /// <summary>
        /// Gets the name of the sport type.
        /// </summary>
        /// <value>
        /// The name of the sport type.
        /// </value>
        public string SportTypeName
        {
            get
            {
                return CacheRef.GetSportName(this.SportTypeId);
            }
        }
    }

    /// <summary>
    /// Match entered class.
    /// </summary>
    public class MatchEntered
    {
        public bool IsToday { get; set; }

        public DateTime? EventDate { get; set; }

        public short SportType { get; set; }

        public int Entered { get; set; }

        public int NonEntered { get; set; }
    }
}
