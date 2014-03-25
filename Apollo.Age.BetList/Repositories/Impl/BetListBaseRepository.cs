// --------------------------------------------------------------------------
// <copyright file="BetListBaseRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2014.2.21">Created</change>
// </history>

namespace Apollo.Age.BetList.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.BetList.Criterias;
    using Apollo.BetList.Core.Entities;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Bet list base repository class.
    /// </summary>
    public class BetListBaseRepository
    {
        /// <summary>
        /// Gets the tickets.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return ticket result.</returns>
        public TicketResult GetTickets(ICriteria criteria)
        {
            var ticketResult = new TicketResult();
            if (criteria.IsValid())
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    var dataSet = db.ExecuteDataSet(criteria.GetSpParams());

                    // Main ticket list.
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    ticketResult.TicketList = this.ReadTicket(mainTable);

                    // Reference ticket.
                    var referenceTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();
                    ticketResult.TicketListMixParlay = this.ReadTicket(referenceTable);
                }
            }

            return ticketResult;
        }

        /// <summary>
        /// Time start date.
        /// </summary>
        /// <param name="dt">Input a date.</param>
        /// <returns>Return a date with full hour.</returns>
        public DateTime ResetStartDate(DateTime dt)
        {
            return dt.AddMilliseconds(-((((((dt.Hour * 60) + dt.Minute) * 60) + dt.Second) * 1000) + dt.Millisecond));
        }

        /// <summary>
        /// Time end date.
        /// </summary>
        /// <param name="dt">Input a date.</param>
        /// <returns>Return a date with full hour.</returns>
        public DateTime ResetEndDate(DateTime dt)
        {
            return ResetStartDate(dt).Add(new TimeSpan(0, 23, 59, 59, 000));
        }

        /// <summary>
        /// Read boolean value from an object.
        /// </summary>
        /// <param name="value">Input value.</param>
        /// <returns>To boolean.</returns>
        public bool ReadBoolean(object value)
        {
            if (value == null || value == Convert.DBNull)
            {
                return false;
            }

            if (value.GetType() == typeof(bool))
            {
                return Convert.ToBoolean(value);
            }

            if (value.GetType() == typeof(int))
            {
                return Convert.ToInt32(value) > 0;
            }

            return false;
        }

        /// <summary>
        /// Parse to Ticket Object.
        /// </summary>
        /// <param name="data">Data Table.</param>
        /// <returns>List Ticket.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public IList<ITicket> ReadTicket(DataTable data)
        {
            var columnCollection = data.Columns;

            // Long
            var containsTransId = columnCollection.Contains("transid");
            var containsBetId = columnCollection.Contains("betid");
            var containsCustId = columnCollection.Contains("custid");

            // string
            var containsRefNo = columnCollection.Contains("refno");
            var containsMatchCode = columnCollection.Contains("matchcode");
            var containsShowTime = columnCollection.Contains("showtime");
            var containsUsername = columnCollection.Contains("username");
            var containsStatus = columnCollection.Contains("status");
            var containsBetStatus = columnCollection.Contains("betstatus");
            var containsBetTeam = columnCollection.Contains("betteam");
            var containsIP = columnCollection.Contains("ip");
            var containsOddsType = columnCollection.Contains("oddstype");
            var containsUGroup = columnCollection.Contains("ugroup");

            // Int
            var containsMatchId = columnCollection.Contains("matchid");
            var containsHomeId = columnCollection.Contains("homeid");
            var containsAwayId = columnCollection.Contains("awayid");
            var containsSportType = columnCollection.Contains("sporttype");
            var containsBetType = columnCollection.Contains("bettype");
            var containsLeagueId = columnCollection.Contains("leagueid");
            var containsLiveHomeScore = columnCollection.Contains("livehomescore");
            var containsLiveAwayScore = columnCollection.Contains("liveawayscore");
            var containsAgentLevel = columnCollection.Contains("agentlevel");
            var containsActualStake = columnCollection.Contains("actual_stake");
            var containsStatusId = columnCollection.Contains("statusid");
            var containsFinalHomeScore = columnCollection.Contains("finalhomescore");
            var containsFinalAwayScore = columnCollection.Contains("finalawayscore");
            var containsFirstHalfHomeScore = columnCollection.Contains("hthomescore");
            var containsFirstHalfAwayScore = columnCollection.Contains("htawayscore");

            // Boolean
            var containsNeutral = columnCollection.Contains("isneutral");
            var containsLiveIndicator = columnCollection.Contains("liveindicator");

            // Money
            var containsOdds = columnCollection.Contains("odds");
            var containsMOdds = columnCollection.Contains("Modds");
            var containsHandicap1 = columnCollection.Contains("hdp1");
            var containsHandicap2 = columnCollection.Contains("hdp2");
            var containsStake = columnCollection.Contains("stake");
            var containsAPositionTaking = columnCollection.Contains("apositiontaking");
            var containsADiscount = columnCollection.Contains("adiscount");
            var containsPlayerDiscount = columnCollection.Contains("playerdiscount");
            var containsAWinlost = columnCollection.Contains("awinlost");
            var containsAComm = columnCollection.Contains("acomm");
            var containWinlost = columnCollection.Contains("winlost");
            var containsPlayercomm = columnCollection.Contains("playercomm");

            // DateTime
            var containsWinlostDate = columnCollection.Contains("winlostdate");
            var containsTransDate = columnCollection.Contains("transdate");

            var result = new List<ITicket>();
            foreach (DataRow dataRow in data.Rows)
            {
                var ticket = new Ticket();

                // Long
                ticket.TransId = containsTransId ? Convert.ToInt64(dataRow["transid"]) : 0;
                ticket.BetId = containsBetId && !Convert.IsDBNull(dataRow["betid"]) ? new long?(Convert.ToInt64(dataRow["betid"])) : null;
                ticket.CustId = containsCustId ? Convert.ToInt64(dataRow["custid"]) : 0;

                // string
                ticket.RefNo = containsRefNo ? Converter.ToString2(dataRow["refno"]) : string.Empty;
                ticket.MatchCode = containsMatchCode ? Converter.ToString2(dataRow["matchcode"]) : string.Empty;
                ticket.ShowTime = containsShowTime ? Converter.ToString2(dataRow["showtime"]) : string.Empty;
                ticket.UserName = containsUsername ? Converter.ToString2(dataRow["username"]) : string.Empty;
                ticket.Status = containsStatus ? Converter.ToString2(dataRow["status"]) : string.Empty;
                ticket.BetStatus = containsBetStatus ? Converter.ToString2(dataRow["betstatus"]) : string.Empty;
                ticket.BetTeam = containsBetTeam ? Converter.ToString2(dataRow["betteam"]) : string.Empty;
                ticket.IP = containsIP ? Converter.ToString2(dataRow["ip"]) : string.Empty;
                ticket.OddsType = containsOddsType ? Converter.ToString2(dataRow["oddstype"]) : "0";
                ticket.UGroup = containsUGroup ? Converter.ToString2(dataRow["ugroup"]) : string.Empty;

                // Int
                ticket.MatchId = containsMatchId ? Converter.ToInt(dataRow["matchid"]) : 0;
                ticket.HomeId = containsHomeId ? Converter.ToInt(dataRow["homeid"]) : 0;
                ticket.AwayId = containsAwayId ? Converter.ToInt(dataRow["awayid"]) : 0;
                ticket.SportTypeId = containsSportType ? Converter.ToInt(dataRow["sporttype"]) : 0;
                ticket.BetTypeId = containsBetType ? Converter.ToInt(dataRow["bettype"]) : 0;
                ticket.LeagueId = containsLeagueId ? Converter.ToInt(dataRow["leagueid"]) : 0;
                ticket.LiveHomeScore = containsLiveHomeScore ? Converter.ToInt(dataRow["livehomescore"]) : 0;
                ticket.LiveAwayScore = containsLiveAwayScore ? Converter.ToInt(dataRow["liveawayscore"]) : 0;
                ticket.AgentLevel = containsAgentLevel ? Converter.ToInt(dataRow["agentlevel"]) : 0;
                ticket.StatusId = containsStatusId ? Converter.ToInt(dataRow["statusid"]) : 0;
                ticket.FinalHomeScore = containsFinalHomeScore ? Converter.ToInt(dataRow["finalhomescore"]) : 0;
                ticket.FinalAwayScore = containsFinalAwayScore ? Converter.ToInt(dataRow["finalawayscore"]) : 0;
                ticket.HalfTimeHomeScore = containsFirstHalfHomeScore ? Converter.ToInt(dataRow["hthomescore"]) : 0;
                ticket.HalfTimeAwayScore = containsFirstHalfAwayScore ? Converter.ToInt(dataRow["htawayscore"]) : 0;

                // Boolean
                ticket.IsLive = containsLiveIndicator ? this.ReadBoolean(dataRow["liveindicator"]) : false;
                ticket.IsNeutral = containsNeutral ? this.ReadBoolean(dataRow["isneutral"]) : false;

                // Money
                ticket.Odds = containsOdds ? Converter.ToDecimal(dataRow["odds"]) : 0;
                ticket.MOdds = containsMOdds ? Converter.ToDecimal(dataRow["modds"]) : 0;
                ticket.Handicap1 = containsHandicap1 ? Converter.ToDecimal(dataRow["hdp1"]) : 0;
                ticket.Handicap2 = containsHandicap2 ? Converter.ToDecimal(dataRow["hdp2"]) : 0;
                ticket.Stake = containsStake ? Converter.ToDecimal(dataRow["stake"]) : 0;
                ticket.ActualStake = containsActualStake ? Converter.ToDecimal(dataRow["actual_stake"]) : 0;

                ticket.APositionTaking = containsAPositionTaking ? Converter.ToDecimal(dataRow["apositiontaking"]) : 0;
                ticket.ADiscount = containsADiscount ? Converter.ToDecimal(dataRow["adiscount"]) : 0;
                ticket.PlayerDiscount = containsPlayerDiscount ? Converter.ToDecimal(dataRow["playerdiscount"]) : 0;

                ticket.AWinlost = containsAWinlost ? Converter.ToDecimal(dataRow["awinlost"]) : 0;
                ticket.AComm = containsAComm ? Converter.ToDecimal(dataRow["acomm"]) : 0;
                ticket.Winlost = containWinlost ? Converter.ToDecimal(dataRow["winlost"]) : 0;
                ticket.PlayerComm = containsPlayercomm ? Converter.ToDecimal(dataRow["playercomm"]) : 0;

                // DateTime
                ticket.WinlostDate = containsWinlostDate ? Converter.ToDateTime(dataRow["winlostdate"]) : DateTime.Now;
                ticket.TransDate = containsTransDate ? Converter.ToDateTime(dataRow["transdate"]) : DateTime.Now;

                result.Add(ticket);
            }

            return result;
        }
    }
}