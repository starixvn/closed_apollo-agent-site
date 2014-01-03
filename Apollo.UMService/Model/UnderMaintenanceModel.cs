// --------------------------------------------------------------------------
// <copyright file="UnderMaintenanceModel.cs" company="Nexcel Solutions Vietnam">
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

    /// <summary>
    /// Under maintenance model.
    /// </summary>
    public class UnderMaintenanceModel
    {
        public UnderMaintenanceModel()
        {
            Status = false;
            var nowHour = DateTime.Today.AddHours(DateTime.Now.Hour);
            Start_time = nowHour.ToShortTimeString();
            End_time = nowHour.AddHours(1).ToShortTimeString();
            UMDate = DateTime.Now.ToString("dd MMMM yyyy");
        }

        public bool Status { get; set; }

        public string Start_time { get; set; }

        public string End_time { get; set; }

        public string UMDate { get; set; }
    }
}