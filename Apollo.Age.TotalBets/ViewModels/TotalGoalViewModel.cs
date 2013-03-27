
namespace Apollo.Age.TotalBets.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class TotalGoalViewModel
    {
        public int MatchId { get; set; }
        public int HomeId { get; set; }
        public int AwayId { get; set; }
        public decimal Total01 { get; set; }
        public decimal Total23 { get; set; }
        public decimal Total46 { get; set; }
        public decimal Total7Over { get; set; }
        public bool IsNeutral { get; set; }
    }
}
