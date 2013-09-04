// --------------------------------------------------------------------------
// <copyright file="UnderMaintenanceViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.5.3">Created</change>
//   <change who="Terry.Nguyen" date="2013.7.17">add more property UMDate</change>
//   <change who="Edward.Nguyen" date="2013.9.3">Move from Web to</change>
// </history>

namespace Apollo.UMService.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UnderMaintenanceModel
    {
        public UnderMaintenanceModel()
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
