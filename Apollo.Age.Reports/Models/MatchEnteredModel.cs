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

    public class MatchEnteredModel
    {
        public short SportTypeId { get; set; }

        public int YesterdayEntered { get; set; }

        public int YesterdayNonEntered { get; set; }

        public int TodayEntered { get; set; }

        public int TodayNonEntered { get; set; }

        public string SportTypeName
        {
            get
            {
                return CacheRef.GetSportName(this.SportTypeId);
            }
        }
    }

    public class MatchEntered
    {
        public bool IsToday { get; set; }

        public DateTime? EventDate { get; set; }

        public short SportType { get; set; }

        public int Entered { get; set; }

        public int NonEntered { get; set; }
    }
}
