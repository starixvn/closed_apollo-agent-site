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
    using System.Resources;
    using Apollo.Age.Services;

    /// <summary>
    /// Match result model.
    /// </summary>
    public class MatchResultModel
    {
        private object _locker = new object();
        private ResourceManager _resourceManager = null;

        // Define list key for eventstatus.
        private Dictionary<string, int> _event = new Dictionary<string, int>()
        {
            { "running", Convert.ToInt32("0000000", 2) },
            { "completed", Convert.ToInt32("0000001", 2) },
            { "closed", Convert.ToInt32("0000010", 2) },
            { "postponed", Convert.ToInt32("0000011", 2) },
            { "refund", Convert.ToInt32("0000100", 2) },
            { "refund ft", Convert.ToInt32("0000101", 2) },
            { "refund ht", Convert.ToInt32("0000110", 2) },
            { "won", Convert.ToInt32("0000111", 2) },
            { "internal", Convert.ToInt32("0001000", 2) }
        };

        // Define list key for Result: isPending(1 bit) isHTCal(1 bit) calculate(1 bit) enventStatus(4 bit).
        private Dictionary<int, string> _result = new Dictionary<int, string>()
        {
            { Convert.ToInt32("0000010", 2), "closed" },
            { Convert.ToInt32("0100010", 2), "closed" },
            { Convert.ToInt32("0000000", 2), "running" },
            { Convert.ToInt32("0100000", 2), "running" },
            { Convert.ToInt32("1000000", 2), "pending" },
            { Convert.ToInt32("1010010", 2), "pending" },
            { Convert.ToInt32("1100010", 2), "pending" },
            { Convert.ToInt32("1100000", 2), "pending" },
            { Convert.ToInt32("1010000", 2), "pending" },
            { Convert.ToInt32("0110001", 2), "completed" },
            { Convert.ToInt32("1000010", 2), "pending" },
            { Convert.ToInt32("0000100", 2), "refund" },
            { Convert.ToInt32("0000101", 2), "refundft" },
            { Convert.ToInt32("0010101", 2), "refundft" },
            { Convert.ToInt32("0010110", 2), "refundht" },
            { Convert.ToInt32("0000110", 2), "refundht" },
            { Convert.ToInt32("0010111", 2), "lblWon" },
            { Convert.ToInt32("0000111", 2), "lblWon" },
            { Convert.ToInt32("0010011", 2), "postponed" },
            { Convert.ToInt32("0000011", 2), "postponed" },
            { Convert.ToInt32("0011000", 2), "Internal" },
            { Convert.ToInt32("0001000", 2), "Internal" },
        };

        public byte SportId { get; set; }

        public string BetStatus { get; set; }

        public int LeagueId { get; set; }

        /// <summary>
        /// Gets the name of the league.
        /// </summary>
        /// <value>
        /// The name of the league.
        /// </value>
        public string LeagueName
        {
            get
            {
                return CacheRef.GetLeagueName(this.LeagueId);
            }
        }

        public string ViewName { get; set; }

        /// <summary>
        /// Gets the name of the sport.
        /// </summary>
        /// <value>
        /// The name of the sport.
        /// </value>
        public string SportName
        {
            get
            {
                return CacheRef.GetSportName(this.SportId);
            }
        }

        public int MatchId { get; set; }

        public string LColor { get; set; }

        public string MatchCode { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime KickoffTime { get; set; }

        public int HomeId { get; set; }

        /// <summary>
        /// Gets the home team.
        /// </summary>
        /// <value>
        /// The home team.
        /// </value>
        public string HomeTeam
        {
            get
            {
                return string.Format("{0} {1}", CacheRef.GetTeamName(this.HomeId), this.IsNeutral ? "(N)" : string.Empty);
            }
        }

        public int AwayId { get; set; }

        /// <summary>
        /// Gets the away team.
        /// </summary>
        /// <value>
        /// The away team.
        /// </value>
        public string AwayTeam
        {
            get
            {
                return CacheRef.GetTeamName(this.AwayId);
            }
        }

        public int? HalfTimeHomeScore { get; set; }

        public int? HalfTimeAwayScore { get; set; }

        public int? FinalHomeScore { get; set; }

        public int? FinalAwayScore { get; set; }

        public string EventStatus { get; set; }

        public string ShowTime { get; set; }

        public int HomeId2 { get; set; }

        public bool IsNeutral { get; set; }

        public string GoalRecord { get; set; }

        public bool IsHTCal { get; set; }

        public bool Calculate { get; set; }

        public bool IsPending { get; set; }

        #region Map event status to responding language

        /// <summary>
        /// Gets map event status to responding language.
        /// </summary>
        /// <value>
        /// The event status translation.
        /// </value>
        public string EventStatusTranslation
        {
            get
            {
                int resultKey = 0x0000000;
                int pending = Convert.ToInt32(this.IsPending);
                int htCal = Convert.ToInt32(this.IsHTCal);
                int cal = Convert.ToInt32(this.Calculate);

                pending <<= 6;  // Shift right 6 bit for pending status.
                htCal <<= 5;    // Shift right 5 bit for haft time calculate status.
                cal <<= 4;      // Shift right 4 bit for calculate status.

                resultKey = pending | htCal | cal;

                if (this._event.ContainsKey(this.EventStatus.ToLowerInvariant()))
                {
                    resultKey |= this._event[this.EventStatus.ToLowerInvariant()]; // Or with event status.

                    if (this._result.ContainsKey(resultKey))
                    {
                        return this.GetResource(this._result[resultKey]);
                    }
                    else
                    {
                        // Get status from resource lang, if any.
                        string str = this.GetResource(this.EventStatus.ToLower());

                        if (str != string.Empty)
                        {
                            return str;
                        }
                    }
                }

                return this.UppercaseFirst(this.EventStatus);
            }
        }

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="key">The key need to get resource value.</param>
        /// <returns>
        /// Return resource value.
        /// </returns>
        public string GetResource(string key)
        {
            if (this._resourceManager == null)
            {
                lock (this._locker)
                {
                    if (this._resourceManager == null)
                    {
                        this._resourceManager = new ResourceManager(typeof(Resources.Lang));
                        this._resourceManager.IgnoreCase = true;
                    }
                }
            }

            return this._resourceManager.GetString(key) ?? string.Empty;
        }

        /// <summary>
        /// Uppercases the first.
        /// </summary>
        /// <param name="s">The string that need to upper case first letter.</param>
        /// <returns>String formatted.</returns>
        private string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        #endregion Map event status to coresponding language
    }
}