// --------------------------------------------------------------------------
// <copyright file="MatchEnteredViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.15">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using Apollo.Age.Services;

    public class MatchEnteredViewModel
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
}
