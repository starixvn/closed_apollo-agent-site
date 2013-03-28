
namespace Apollo.Age.TotalBets.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Sunplus.Agent.Model.BL.Cache;

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
        
        public string HomeTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.HomeId) + ((IsNeutral) ? "(N)" : string.Empty);
            }
        }

        public string AwayTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.AwayId);
            }
        }
    }
}
