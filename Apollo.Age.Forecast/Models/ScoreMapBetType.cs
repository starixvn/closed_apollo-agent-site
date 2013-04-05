// -----------------------------------------------------------------------
// <copyright file="ScoreMapBetType.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Apollo.Age.Forecast.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScoreMapBetType
    {
        public int TypeId { get; set; }
        public bool IsLive { get; set; }
        public bool IsDead { get; set; }
        public bool Id1st { get; set; }
        public string BetTypeName 
        {
            get
            {
                return CacheRef.GetBettype(this.TypeId);
            }
        }
    }
}
