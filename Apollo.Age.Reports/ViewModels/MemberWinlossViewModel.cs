// --------------------------------------------------------------------------
// <copyright file="MemberWinlossViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.12">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Reports.Models;

    public class MemberWinlossViewModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public List<MemberWinlossModel> MemberWinLossList { get; set; }
    }
}
