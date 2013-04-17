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
    using Apollo.Age.Services;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MatchResultModel
    {
        public byte SportId { get; set; }

        public string BetStatus { get; set; }

        public int LeagueId { get; set; }

        public string LeagueName
        {
            get
            {
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        }

        public int MatchId { get; set; }

        public string LColor { get; set; }

        public string MatchCode { get; set; }

        public DateTime EvenDate { get; set; }

        public DateTime KickoffTime { get; set; }

        public int HomeId { get; set; }

        public string HomeTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.HomeId) + ((this.IsNeutral) ? "(N)" : string.Empty);
            }
        }

        public int AwayId { get; set; }

        public string AwayTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.AwayId) + ((this.IsNeutral) ? "(N)" : string.Empty);
            }
        }

        public int HalfTimeHomeScore { get; set; }

        public int HalfTimeAwayScore { get; set; }

        public int FinalHomeScore { get; set; }

        public int FinalAwayScore { get; set; }

        public string EventStatus { get; set; }

        public bool ShowTime { get; set; }

        public bool IsNeutral { get; set; }

        public int HomeId2 { get; set; }

        public bool IsHTCal { get; set; }

        public bool Calculate { get; set; }

        public bool IsPending { get; set; }

        #region for soccer
        // for soccer
        public bool IsHomeFirstGoal { get; internal set; }

        public bool IsHomeLastGoal { get; internal set; }

        public bool IsAwayFirstGoal { get; internal set; }

        public bool IsAwayLastGoal { get; internal set; }

        // Base on [BetStatus] to get First/Last Goal
        // Example [BetStatus] = "FL:1-2"
        public void SetFirstAndLastGoal()
        {
            if (!string.IsNullOrEmpty(this.BetStatus) && this.BetStatus.Contains(":"))
            {
                var valueArr = this.BetStatus.Split(':');
                if (string.Compare(valueArr[0], "FL", true) == 0)
                {
                    var firstGoal = valueArr[1].Split('-')[0];
                    var lastGoal = valueArr[1].Split('-')[1];
                    if (firstGoal == "1")
                    {
                        IsHomeFirstGoal = true;
                    }

                    if (firstGoal == "2")
                    {
                        IsAwayFirstGoal = true;
                    }

                    if (lastGoal == "1")
                    {
                        IsHomeLastGoal = true;
                    }

                    if (lastGoal == "2")
                    {
                        IsAwayLastGoal = true;
                    }
                }
            }
        }
        #endregion
    }
}