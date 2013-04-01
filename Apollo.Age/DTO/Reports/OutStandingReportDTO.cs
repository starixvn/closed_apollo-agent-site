namespace Apollo.Age.DTO.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class OutStandingReportDTO
    {
        public string UserName { get; set; }

        public decimal OutStanding { get; set; }

        public decimal ShareAmount { get; set; }

        public decimal GameOutStanding { get; set; }

        public decimal GameShareAmount { get; set; }
    }
}