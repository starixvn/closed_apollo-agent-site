// --------------------------------------------------------------------------
// <copyright file="OutStandingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Reports.ViewModels;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class OutStandingService : ReportServiceBase
    {
        public OutstandingViewModel Total(IEnumerable<OutstandingViewModel> models)
        {
            return new OutstandingViewModel
            {
                UserName = string.Empty,
                GameOutStanding = models.Sum(x => x.GameOutStanding),
                GameShareAmount = models.Sum(x => x.GameShareAmount),
                OutStanding = models.Sum(x => x.OutStanding),
                ShareAmount = models.Sum(x => x.ShareAmount)
            };
        }
    }
}