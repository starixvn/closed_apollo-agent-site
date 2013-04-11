// --------------------------------------------------------------------------
// <copyright file="MatchResultModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.11">Created</change>
// </history>


namespace Apollo.Age.Reports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchResultModel 
    {
        public byte SportId{ get; set;}

		 public string BetStatus{ get; set;}

		 public int LeagueId{ get; set;}

		 public int MatchId{ get; set;}

		 public string LColor{ get; set;}

		 public string MatchCode{ get; set;}

		 public DateTime EvenDate{ get; set;}

		 public DateTime KickoffTime{ get; set;}

		 public int HomeId{ get; set;}

		 public int AwayId{ get; set;}

		 public int HalfTimeHomeScore{ get; set;}

		 public int HalfTimeAwayScore{ get; set;}

		 public int FinalHomeScore{ get; set;}

		 public int FinalAwayScore{ get; set;}

		 public string EventStatus{ get; set;}		 

		 public bool ShowTime{ get; set;}

		 public string IsNeutral{ get; set;}

		 public int HomeId2{ get; set;}

		 public bool IsHTCal{ get; set;}

		 public bool Calculate{ get; set;}

		 public bool IsPending{ get; set;}

	}
}