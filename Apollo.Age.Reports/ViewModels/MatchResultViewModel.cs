// --------------------------------------------------------------------------
// <copyright file="MatchResultViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.10">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System;

    /// <summary>
    /// Match result view model class.
    /// </summary>
    public class MatchResultViewModel
    {
        public DateTime BalanceUpDay { get; set; }

        public DateTime EventDate { get; set; }
    }
}