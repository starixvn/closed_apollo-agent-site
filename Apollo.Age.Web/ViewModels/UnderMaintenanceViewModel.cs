
// --------------------------------------------------------------------------
// <copyright file="UnderMaintenanceViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.3">Created</change>
//   <change who="Terry.Nguyen" date="2013.7.17">add more property UMDate</change>
// </history>

namespace Apollo.Age.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UnderMaintenanceViewModel
    {
        public UnderMaintenanceViewModel()
        {
            Status = false;
            var nowHour = DateTime.Today.AddHours(DateTime.Now.Hour);
            StartTime = nowHour.ToShortTimeString();
            EndTime = nowHour.AddHours(1).ToShortTimeString();
            UMDate = DateTime.Now.ToString("dd MMMM yyyy");
        }

        public bool Status { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string UMDate { get; set; }
    }
}