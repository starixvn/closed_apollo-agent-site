namespace Apollo.Age.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UnderMaintenanceModel
    {
        public UnderMaintenanceModel()
        {
            IsDownTime = false;
            var nowHour = DateTime.Today.AddHours(DateTime.Now.Hour);
            StartTime = nowHour.ToShortTimeString();
            EndTime = nowHour.AddHours(1).ToShortTimeString();
        }

        public bool IsDownTime { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }
}