// --------------------------------------------------------------------------
// <copyright file="MatchEnteredViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.11">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Reports.Models;

    public class MatchEnteredViewModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<MatchEnteredModel> matchEnteredList { get; set; }
    }
}
