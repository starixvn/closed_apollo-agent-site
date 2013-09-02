
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
            status = false;
            var nowHour = DateTime.Today.AddHours(DateTime.Now.Hour);
            start_time = nowHour.ToShortTimeString();
            end_time = nowHour.AddHours(1).ToShortTimeString();
            UMDate = DateTime.Now.ToString("dd MMMM yyyy");

        }

        public bool status { get; set; }

        public string start_time { get; set; }

        public string end_time { get; set; }

        public string UMDate { get; set; }
    }
}