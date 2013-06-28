namespace Apollo.Age.Wagers.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;

    public class WagerViewModels
    {
        public DateTime KickOffTime { get; set; }
        public int LeagueId { get; set; }
        public string LeagueName
        {
            get;
            set;
        }
    }
}