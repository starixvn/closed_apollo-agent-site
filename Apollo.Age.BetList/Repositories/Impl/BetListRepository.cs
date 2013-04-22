// --------------------------------------------------------------------------
// <copyright file="BetListRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
// </history>

namespace Apollo.Age.BetList.Repositories.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Models;
    using Apollo.Age.BetList.Repositories.Interface;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common;
    using Fanex.Data;

    public class BetListRepository : IBetListRepository
    {
        /// <summary>
        /// Get detail agent PT and agent Comm
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IList<ITicket> GetDetailAgentComm(BetListCriteria criteria)
        {
            var uspKey = "Age_BetList_AgentDetail";
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    var dataSet = db.ExecuteDataSet(criteria.GetSpParams());
                    var ticketResult = new TicketResult();

                    // Main ticket list
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    return this.ReadTicket(mainTable);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get data for BetList
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="uspKey"></param>
        /// <returns></returns>
        public TicketResult GetBetListData(BetListCriteria criteria, string uspKey)
        {
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    var dataSet = db.ExecuteDataSet(criteria.GetSpParams());
                    var ticketResult = new TicketResult();

                    // Main ticket list
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    ticketResult.TicketList = this.ReadTicket(mainTable);

                    // Reference ticket
                    var referenceTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();
                    ticketResult.TicketDataList = this.ReadReferneceTicket(referenceTable);
                    return ticketResult;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get betlist by matchId
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TicketResult GetRunningByBettypeAndMatch(BetListCriteria criteria)
        {
            var uspKey = "Age_BetList_TotalBets_SelRunningByBettypeMatch";
            var uspParams = new
            {
                MatchID = criteria.MatchId,
                LoggedId = criteria.LoggedId,
                Bettype = criteria.Bettypes,
                Liveindicator = criteria.LiveIndicator,
                Betteam = criteria.BetTeam
            };

            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    var dataSet = db.ExecuteDataSet(uspParams);
                    var ticketResult = new TicketResult();

                    // Main ticket list
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    ticketResult.TicketList = this.ReadTicket(mainTable);

                    // Reference ticket
                    var referenceTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();
                    ticketResult.TicketDataList = this.ReadReferneceTicket(referenceTable);
                    return ticketResult;
                }
            }
            catch
            {
                throw;
            }
        }

        public void GetUserIdByUserName(int loggedId, bool isAgent, string username, out int agentId, out int custId, out int errorCode)
        {
            var uspKey = "Age_BetList_GetId";
            Hashtable parameters = new Hashtable();
            parameters.Add("loggedid", loggedId);
            parameters.Add("isagent", isAgent);
            parameters.Add("username", username);
            parameters.Add("agentid", -1);
            parameters.Add("custid", -1);
            parameters.Add("errcode", 0);
            parameters.Add("errmsg", string.Empty);

            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    db.AssignParamValues(parameters);
                    db.ExecuteNonQuery();

                    errorCode = (int)db.GetParameterValue("errcode");

                    if (errorCode > 0)
                    {
                        agentId = custId = -1;
                    }
                    else
                    {
                        errorCode = 0;
                        agentId = (int)db.GetParameterValue("agentid");
                        custId = (int)db.GetParameterValue("custid");
                    }
                }
            }
            catch
            {
                throw;
            }
        }

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
            var containsBetTeam = columnCollection.Contains("betteam");
            var containsIP = columnCollection.Contains("ip");
            var containsOddsType = columnCollection.Contains("oddstype");

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

            // Boolean
            var containsNeutral = columnCollection.Contains("isneutral");
            var containsLiveIndicator = columnCollection.Contains("liveindicator");

            // Money
            var containsOdds = columnCollection.Contains("odds");
            var containsHandicap1 = columnCollection.Contains("hdp1");
            var containsHandicap2 = columnCollection.Contains("hdp2");
            var containsStake = columnCollection.Contains("stake");
            var containsAPositionTaking = columnCollection.Contains("apositiontaking");
            var containsADiscount = columnCollection.Contains("adiscount");

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
                ticket.BetTeam = containsBetTeam ? Converter.ToString2(dataRow["betteam"]) : string.Empty;
                ticket.IP = containsIP ? Converter.ToString2(dataRow["ip"]) : string.Empty;
                ticket.OddsType = containsOddsType ? Converter.ToString2(dataRow["oddstype"]) : "0";

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
                ticket.ActualStake = containsActualStake ? Converter.ToInt(dataRow["actual_stake"]) : 0;

                // Boolean
                ticket.IsLive = containsLiveIndicator ? this.ReadBoolean(dataRow["liveindicator"]) : false;
                ticket.IsNeutral = containsNeutral ? this.ReadBoolean(dataRow["isneutral"]) : false;

                // Money
                ticket.Odds = containsOdds ? Converter.ToDecimal(dataRow["odds"]) : 0;
                ticket.Handicap1 = containsHandicap1 ? Converter.ToDecimal(dataRow["hdp1"]) : 0;
                ticket.Handicap2 = containsHandicap2 ? Converter.ToDecimal(dataRow["hdp2"]) : 0;
                ticket.Stake = containsStake ? Converter.ToDecimal(dataRow["stake"]) : 0;
                ticket.APositionTaking = containsAPositionTaking ? Converter.ToDecimal(dataRow["apositiontaking"]) : 0;
                ticket.ADiscount = containsADiscount ? Converter.ToDecimal(dataRow["adiscount"]) : 0;

                // DateTime
                ticket.WinlostDate = containsWinlostDate ? Converter.ToDateTime(dataRow["winlostdate"]) : DateTime.Now;
                ticket.TransDate = containsTransDate ? Converter.ToDateTime(dataRow["transdate"]) : DateTime.Now;

                result.Add(ticket);
            }

            return result;
        }

        public List<ITicketData> ReadReferneceTicket(DataTable data)
        {
            var result = new List<ITicketData>();

            var containsBetTeam = data.Columns.Contains("betteam");
            var containsStake = data.Columns.Contains("stake");
            foreach (DataRow dataRow in data.Rows)
            {
                var ticket = new TicketData();

                // string
                ticket.BetTeam = containsBetTeam ? dataRow["betteam"].ToString() : string.Empty;
                ticket.RefNo = dataRow["refno"].ToString();
                ticket.Status = dataRow["status"].ToString();

                // Int
                ticket.BetTypeId = Convert.ToInt32(dataRow["bettype"]);

                // Money
                ticket.Stake = containsStake ? Convert.ToDecimal(dataRow["stake"]) : 0;

                // Decimal
                ticket.Odds = Convert.ToDecimal(dataRow["odds"]);

                result.Add(ticket);
            }

            return result;
        }

        /// <summary>
        /// Read boolean value from an int or bool object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool ReadBoolean(object value)
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
    }
}