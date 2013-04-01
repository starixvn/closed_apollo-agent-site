// --------------------------------------------------------------------------
// <copyright file="OutStandingViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.Reports.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutstandingViewModel
    {
        public string UserName { get; set; }

        public decimal OutStanding { get; set; }

        public decimal ShareAmount { get; set; }

        public decimal GameOutStanding { get; set; }

        public decimal GameShareAmount { get; set; }
    }
}