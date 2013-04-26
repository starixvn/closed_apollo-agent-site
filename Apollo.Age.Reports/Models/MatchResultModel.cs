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
    using Apollo.Age.Services;
    using System.Collections.Generic;
    using System.Resources;

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

        public string GoalRecord { get; set; }

        #region Map event status to coresponding language

        // Define list key for eventstatus
        static Dictionary<string, int> _event = new Dictionary<string, int>()
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

        // Define list key for Result: isPending(1 bit) isHTCal(1 bit) calculate(1 bit) enventStatus(4 bit)
        static Dictionary<int, string> _result = new Dictionary<int, string>()
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

        /// <summary>
        /// Map event status to coresponding language
        /// </summary>
        public string EventStatusTranslation
        {
            get
            {
                int resultKey = 0x0000000;
                int pending = Convert.ToInt32(this.IsPending);
                int htCal = Convert.ToInt32(this.IsHTCal);
                int cal = Convert.ToInt32(this.Calculate);

                pending <<= 6;  // Shift right 6 bit for pending status
                htCal <<= 5;    // Shift right 5 bit for haft time calculate status
                cal <<= 4;      // Shift right 4 bit for calculate status

                resultKey = pending | htCal | cal;

                if (_event.ContainsKey(this.EventStatus.ToLowerInvariant()))
                {
                    resultKey |= _event[this.EventStatus.ToLowerInvariant()]; // Or with event status

                    if (_result.ContainsKey(resultKey))
                    {
                        return this.GetResource(_result[resultKey]);
                    }
                    else
                    {
                        // Get status from resource lang, if any
                        string str = this.GetResource(this.EventStatus.ToLower());

                        if (str != string.Empty)
                        {
                            return str;
                        }
                    }
                }

                return UppercaseFirst(this.EventStatus);
            }
        }
        
        #region rem
        //public string MapEventStatus(string eventStatus, bool isPending, bool isHTcal, bool calculate)
        //{
        //    int resultKey = 0x0000000;
        //    int pending = Convert.ToInt32(isPending);
        //    int htCal = Convert.ToInt32(isHTcal);
        //    int cal = Convert.ToInt32(calculate);

        //    pending <<= 6;  // Shift right 6 bit for pending status
        //    htCal <<= 5;    // Shift right 5 bit for haft time calculate status
        //    cal <<= 4;      // Shift right 4 bit for calculate status

        //    resultKey = pending | htCal | cal;

        //    if (_event.ContainsKey(eventStatus.ToLowerInvariant()))
        //    {
        //        resultKey |= _event[eventStatus.ToLowerInvariant()]; // Or with event status

        //        if (_result.ContainsKey(resultKey))
        //        {
        //            return this.GetResource(_result[resultKey]);
        //        }
        //        else
        //        {
        //            // Get status from resource lang, if any
        //            string str = this.GetResource(eventStatus.ToLower());

        //            if (str != string.Empty)
        //            {
        //                return str;
        //            }
        //        }
        //    }

        //    return this.UppercaseFirst(eventStatus);
        //}
        #endregion

        static object locker = new object();
        static ResourceManager _resourceManager = null;

        public string GetResource(string key)
        {
            if (_resourceManager == null)
            {
                lock (locker)
                {
                    if (_resourceManager == null)
                    {
                        _resourceManager = new ResourceManager(typeof(Resources.Lang));
                        _resourceManager.IgnoreCase = true;
                    }
                }
            }

            return _resourceManager.GetString(key) ?? string.Empty;
        }

        static private string UppercaseFirst(string s)
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