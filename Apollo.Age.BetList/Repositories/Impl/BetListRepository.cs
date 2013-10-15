// --------------------------------------------------------------------------
// <copyright file="BetListRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
//   <change who="Kaden.Trinh" date="2013.05.20">Add more function get agent upline of member</change>
// </history>

namespace Apollo.Age.BetList.Repositories.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Repositories.Interface;
    using Apollo.Age.Entities;
    using Fanex.BetList.Core.Entities;
    using Fanex.Common;
    using Fanex.Data;

    /// <summary>
    /// BetList Repository
    /// </summary>
    public class BetListRepository : IBetListRepository
    {
        /// <summary>
        /// Get detail agent PT and agent commission
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>List Ticket</returns>
        public IList<ITicket> GetDetailAgentComm(BetListCriteria criteria)
        {
            var uspKey = "Age_BetList_AgentDetail";

            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    var dataSet = db.ExecuteDataSet(new
                    {
                        criteria.TransId,
                        criteria.LoggedId
                    });
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
        /// Get detail win loss agent PT and agent commission
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>List Ticket</returns>
        public IList<ITicket> GetDetailWinLossAgentComm(BetListCriteria criteria)
        {
            try
            {
                DataTable mainTable;
                var preBalanceUpDay = criteria.BalanceUpDay.AddDays(-1);
                if (criteria.FromDate < criteria.BalanceUpDay)
                {
                    if (criteria.ToDate < preBalanceUpDay)
                    {
                        mainTable = this.GetDetailWinLossNewOrOld("Age_BetList_AgentDetail_Old", criteria);
                    }
                    else
                    {
                        // Separate 2 parts: dateFrom --> preBalanceUpDate, balanceUpDate --> dateTo
                        var mainOldTable = this.GetDetailWinLossNewOrOld("Age_BetList_AgentDetail_Old", criteria);

                        var mainNewTable = this.GetDetailWinLossNewOrOld("Age_BetList_AgentDetail", criteria);

                        // Merge two table bettrans
                        mainOldTable.Merge(mainNewTable);
                        mainTable = mainOldTable;
                    }
                }
                else
                {
                    mainTable = this.GetDetailWinLossNewOrOld("Age_BetList_AgentDetail", criteria);
                }

                // Main ticket list
                var ticketList = this.ReadTicket(mainTable);

                return ticketList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get detail ticket mix-parlay
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>List Ticket</returns>
        public IList<ITicket> GetMixParlayDetailTicket(BetListCriteria criteria)
        {
            var uspKey = "Age_BetList_SelParlayDetail";
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    var dataSet = db.ExecuteDataSet(criteria.GetSpParams());
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
        /// Get bet list running on Total Bets menu
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetRunningByBettype(BetListCriteria criteria)
        {
            var uspKey = "Age_BetList_TotalBets_SelRunningByBettype";
            var uspParams = new
            {
                AgentId = criteria.AgentId,
                criteria.LoggedId,
                Bettypes = criteria.BetTypes
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

        /// <summary>
        /// Get bet list by matchId
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetRunningByBettypeAndMatch(BetListCriteria criteria)
        {
            var uspKey = "Age_BetList_TotalBets_SelRunningByBettypeMatch";
            var uspParams = new
            {
                MatchID = criteria.MatchId,
                LoggedId = criteria.LoggedId,
                AgentId = criteria.AgentId,
                Bettype = criteria.BetTypes,
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

        /// <summary>
        /// Get all bets by date and member from Win loss listOutstanding
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetWinLossByDateAndMem(BetListCriteria criteria)
        {
            try
            {
                DataTable mainTable, referenceTable;
                var ticketResult = new TicketResult();
                var preBalanceUpDay = criteria.BalanceUpDay.AddDays(-1);
                if (criteria.FromDate < criteria.BalanceUpDay)
                {
                    if (criteria.ToDate < preBalanceUpDay)
                    {
                        mainTable = this.GetWinLossFromOld("Age_BetList_SelWinloss_Old", criteria, out referenceTable);
                    }
                    else
                    {
                        // Separate 2 parts: dateFrom --> preBalanceUpDate, balanceUpDate --> dateTo
                        DataTable referenceOldTable = null;
                        var mainOldTable = this.GetWinLossFromOld("Age_BetList_SelWinloss_Old", criteria, out referenceOldTable);

                        DataTable referenceNewTable = null;
                        var mainNewTable = this.GetWinLossFromNew("Age_BetList_SelWinloss", criteria, out referenceNewTable);

                        // Merge two table bettrans
                        mainOldTable.Merge(mainNewTable);
                        mainTable = mainOldTable;

                        // Merge table reference
                        referenceOldTable.Merge(referenceNewTable);
                        referenceTable = referenceOldTable;
                    }
                }
                else
                {
                    mainTable = this.GetWinLossFromNew("Age_BetList_SelWinloss", criteria, out referenceTable);
                }

                // Main ticket list
                ticketResult.TicketList = this.ReadTicket(mainTable);

                // Reference ticket
                ticketResult.TicketDataList = this.ReadReferneceTicket(referenceTable);

                return ticketResult;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// BetList: Transaction, Cancel
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Ticket data</returns>
        public TicketResult GetTicketResult(BetListCriteria criteria)
        {
            try
            {
                var ticketResult = new TicketResult();
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        //var param = criteria.GetSpParams();
                        var dataSet = db.ExecuteDataSet(criteria.GetParameters());

                        // Main ticket list
                        var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                        ticketResult.TicketList = this.ReadTicket(mainTable);

                        // Reference ticket
                        var referenceTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();
                        ticketResult.TicketDataList = this.ReadReferneceTicket(referenceTable);
                    }
                }
                return ticketResult;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the ticket result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="isIncludeOldData">Include old data from Archive database or not</param>
        /// <returns>Ticket result data.</returns>
        public TicketResult GetTicketResult(BetListCriteria criteria, bool isIncludeOldData = false)
        {
            try
            {
                var ticketResult = new TicketResult();
                var balanceUpDay = criteria.BalanceUpDay;

                if (criteria.IsValid())
                {
                    if (isIncludeOldData)
                    {
                        if (criteria.FromDate < balanceUpDay)
                        {
                            if (criteria.ToDate < balanceUpDay.AddDays(-1))
                            {
                                using (var db = new ObjectDb("Age_BetList_SelByDateMemStatus_Old")) // Get data from Archive database only.
                                {
                                    var uspParams = new
                                    {
                                        loggedid = criteria.LoggedId,
                                        agentid = criteria.AgentId,
                                        custid = criteria.CustId,
                                        fromdate = criteria.FromDate,
                                        todate = criteria.BalanceUpDay.AddDays(-1),
                                        statusids = criteria.StatusIds,
                                        sporttypes = criteria.SportTypes,
                                        bettypes = criteria.BetTypes,
                                        rpttype = criteria.RptType,
                                    };

                                    var dataSet = db.ExecuteDataSet(uspParams);

                                    if (dataSet.Tables.Count > 1)
                                    {
                                        ticketResult.TicketList = this.ReadTicket(dataSet.Tables[0]);
                                        ticketResult.TicketDataList = this.ReadReferneceTicket(dataSet.Tables[1]);
                                    }
                                    else
                                    {
                                        ticketResult.TicketList = this.ReadTicket(dataSet.Tables[0]);
                                        ticketResult.TicketDataList = new List<ITicketData>();
                                    }
                                }
                            }
                            else // Get data from main database and archive database.
                            {
                                var oldDatatable = new DataTable();
                                var newDatatable = new DataTable();
                                var oldReferenceDatatable = new DataTable();
                                var newReferenceDatatable = new DataTable();

                                using (var db = new ObjectDb("Age_BetList_SelByDateMemStatus_Old")) // Get data from Archive database only.
                                {
                                    var uspParams = new
                                    {
                                        loggedid = criteria.LoggedId,
                                        agentid = criteria.AgentId,
                                        custid = criteria.CustId,
                                        fromdate = criteria.FromDate,
                                        todate = criteria.BalanceUpDay.AddDays(-1),
                                        statusids = criteria.StatusIds,
                                        sporttypes = criteria.SportTypes,
                                        bettypes = criteria.BetTypes,
                                        rpttype = criteria.RptType,
                                    };

                                    var dataSet = db.ExecuteDataSet(uspParams);

                                    if (dataSet.Tables.Count > 1)
                                    {
                                        oldDatatable = dataSet.Tables[0];
                                        oldReferenceDatatable = dataSet.Tables[1];
                                    }
                                    else
                                    {
                                        oldDatatable = dataSet.Tables[0];
                                        oldReferenceDatatable = new DataTable();
                                    }
                                }

                                using (var db = new ObjectDb("Age_BetList_SelByDateMemStatus")) // Get data from Archive database only.
                                {
                                    var uspParams = new
                                    {
                                        loggedid = criteria.LoggedId,
                                        agentid = criteria.AgentId,
                                        custid = criteria.CustId,
                                        fromdate = criteria.BalanceUpDay,
                                        todate = criteria.ToDate,
                                        statusids = criteria.StatusIds,
                                        sporttypes = criteria.SportTypes,
                                        bettypes = criteria.BetTypes,
                                        rpttype = criteria.RptType,
                                        movebetsday = criteria.MoveBetsDay
                                    };

                                    var dataSet = db.ExecuteDataSet(uspParams);

                                    if (dataSet.Tables.Count > 1)
                                    {
                                        newDatatable = dataSet.Tables[0];
                                        newReferenceDatatable = dataSet.Tables[1];
                                    }
                                    else
                                    {
                                        newDatatable = dataSet.Tables[0];
                                        newReferenceDatatable = new DataTable();
                                    }
                                }

                                // Merge new and old data into a data table.
                                newDatatable.Merge(oldDatatable);
                                newReferenceDatatable.Merge(oldReferenceDatatable);

                                ticketResult.TicketList = this.ReadTicket(newDatatable);
                                ticketResult.TicketDataList = this.ReadReferneceTicket(newReferenceDatatable);
                            }
                        }
                        else
                        {
                            using (var db = new ObjectDb("Age_BetList_SelByDateMemStatus")) // Get data from main database only.
                            {
                                var uspParams = new
                                {
                                    loggedid = criteria.LoggedId,
                                    agentid = criteria.AgentId,
                                    custid = criteria.CustId,
                                    fromdate = criteria.BalanceUpDay,
                                    todate = criteria.ToDate,
                                    statusids = criteria.StatusIds,
                                    sporttypes = criteria.SportTypes,
                                    bettypes = criteria.BetTypes,
                                    rpttype = criteria.RptType,
                                    movebetsday = criteria.MoveBetsDay
                                };

                                var dataSet = db.ExecuteDataSet(uspParams);

                                if (dataSet.Tables.Count > 1)
                                {
                                    ticketResult.TicketList = this.ReadTicket(dataSet.Tables[0]);
                                    ticketResult.TicketDataList = this.ReadReferneceTicket(dataSet.Tables[1]);
                                }
                                else
                                {
                                    ticketResult.TicketList = this.ReadTicket(dataSet.Tables[0]);
                                    ticketResult.TicketDataList = new List<ITicketData>();
                                }
                            }
                        }
                    }
                    else
                    {
                        return GetTicketResult(criteria);
                    }
                }

                return ticketResult;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get data betlist on Wagers page
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public TicketResult WagersTicket(WagersCriteria criteria)
        {
            try
            {
                var ticketResult = new TicketResult();
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var param = criteria.GetSpParams();
                        var dataSet = db.ExecuteDataSet(param);

                        // Main ticket list
                        var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                        ticketResult.TicketList = this.ReadTicket(mainTable);
                    }
                }
                return ticketResult;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Combination Mix Parlay Detail
        /// </summary>
        /// <param name="criteria">Criteria name</param>
        /// <returns>SystemParlay data</returns>
        public SystemParlayData GetCombinationMixParlayDetail(SystemParlayDetailCriteria criteria)
        {
            var uspKey = "Age_Betlist_SelSystemParlay_Details";
            var uspParams = new Hashtable
            {
                { "refno", criteria.RefNo },
                { "custid", criteria.CustId },
                { "movebetsday", criteria.MoveBetsDay },
                { "balanceupday", criteria.BalanceUpDay },
                { "language", criteria.Language },
                { "outstanding", 0 }
            };

            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    var dataSet = db.ExecuteDataSet(uspParams);
                    var ticketData = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    var serialData = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();

                    var systemParlayData = new SystemParlayData();

                    systemParlayData.TicketList = new List<ISystemParlayTicket>();
                    foreach (DataRow row in ticketData.Rows)
                    {
                        ISystemParlayTicket ticket = new SystemParlayTicket();
                        ticket.WinLostDate = Converter.ToDateTime(row["winlostdate"]);
                        ticket.TransId = Convert.ToInt64(row["transid"]);
                        ticket.Serial = Convert.ToInt64(row["serial"]);
                        ticket.StatusId = Converter.ToInt(row["statusid"]);
                        ticket.Status = Converter.ToString2(row["status"]);
                        ticket.MOdds = Converter.ToDecimal(row["modds"]);
                        ticket.Stake = Converter.ToDecimal(row["stake"]);
                        ticket.WinLost = Converter.ToDecimal(row["winlost"]);
                        ticket.PlayerComm = Converter.ToDecimal(row["playercomm"]);

                        systemParlayData.TicketList.Add(ticket);
                    }

                    systemParlayData.SerialList = new List<ISystemParlaySerial>();
                    foreach (DataRow row in serialData.Rows)
                    {
                        ISystemParlaySerial serial = new SystemParlaySerial();
                        serial.Serial = Convert.ToInt64(row["serial"]);
                        serial.HomeId = Converter.ToInt(row["homeid"]);
                        serial.AwayId = Converter.ToInt(row["awayid"]);

                        systemParlayData.SerialList.Add(serial);
                    }

                    systemParlayData.Outstanding = Converter.ToDecimal(uspParams["outstanding"]);

                    return systemParlayData;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get UserId By Username
        /// </summary>
        /// <param name="loggedId">Logged id</param>
        /// <param name="isAgent">Agent choice</param>
        /// <param name="username">User name</param>
        /// <param name="agentId">Output agent id</param>
        /// <param name="custId">Output customer id</param>
        /// <param name="errorCode">Output error code</param>
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

        /// <summary>
        /// Get Agent up line of member
        /// </summary>
        /// <param name="parentId">Logged id</param>
        /// <param name="custId">customer id</param>
        /// <returns></returns>
        public IEnumerable<Agent> GetAgentUpLineOfMember(int parentId, int custId)
        {
            var uspKey = "Age_Agent_GetCustomerDownlinePath";
            var uspParams = new
            {
                parentId,
                custId
            };
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    return db.Query<Agent>(uspParams);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Parse to Ticket Objet
        /// </summary>
        /// <param name="data">Data Table</param>
        /// <returns>List Ticket</returns>
        private IList<ITicket> ReadTicket(DataTable data)
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
                ticket.HaftTimeHomeScore = containsFirstHalfHomeScore ? Converter.ToInt(dataRow["hthomescore"]) : 0;
                ticket.HaftTimeAwayScore = containsFirstHalfAwayScore ? Converter.ToInt(dataRow["htawayscore"]) : 0;

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

        /// <summary>
        /// Parse data table to list TicketData Object
        /// </summary>
        /// <param name="data">Data table</param>
        /// <returns>List TicketData</returns>
        private List<ITicketData>
            ReadReferneceTicket(DataTable data)
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
        /// Get detail Win loss from new or archive
        /// </summary>
        /// <param name="uspKey">Store name</param>
        /// <param name="criteria">Criteria name</param>
        /// <returns>Return to table</returns>
        private DataTable GetDetailWinLossNewOrOld(string uspKey, BetListCriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    var dataSet = db.ExecuteDataSet(new
                    {
                        criteria.TransId,
                        criteria.LoggedId
                    });

                    // Main ticket list
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    return mainTable;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get bet list Win loss from new or archive
        /// </summary>
        /// <param name="uspKey">Store name</param>
        /// <param name="criteria">Criteria name</param>
        /// <param name="referenceTable">Out reference table</param>
        /// <returns>Return to table</returns>
        private DataTable GetWinLossFromNewOrOld(string uspKey, BetListCriteria criteria, out DataTable referenceTable)
        {
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    var dataSet = db.ExecuteDataSet(criteria.GetSpParams());

                    // Main ticket list
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();

                    // Reference ticket
                    referenceTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();

                    return mainTable;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get bet list Win loss from new or archive
        /// </summary>
        /// <param name="uspKey">Store name</param>
        /// <param name="criteria">Criteria name</param>
        /// <param name="referenceTable">Out reference table</param>
        /// <returns>Return to table</returns>
        private DataTable GetWinLossFromNew(string uspKey, BetListCriteria criteria, out DataTable referenceTable)
        {
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    db.AddInParameter("loggedid", DbType.Int32, criteria.LoggedId);
                    db.AddInParameter("agentid", DbType.Int32, criteria.AgentId);
                    db.AddInParameter("custid", DbType.Int32, criteria.CustId);
                    db.AddInParameter("fromdate", DbType.DateTime, criteria.FromDate);
                    db.AddInParameter("todate", DbType.DateTime, criteria.ToDate);
                    db.AddInParameter("statusids", DbType.AnsiString, criteria.StatusIds);
                    db.AddInParameter("movebetsday", DbType.DateTime, criteria.MoveBetsDay);
                    db.AddInParameter("sporttypes", DbType.String, criteria.SportTypes);
                    db.AddInParameter("bettypes", DbType.String, criteria.BetTypes);

                    var dataSet = db.ExecuteDataSet();

                    // Main ticket list
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();

                    // Reference ticket
                    referenceTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();

                    return mainTable;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get bet list Win loss from new or archive
        /// </summary>
        /// <param name="uspKey">Store name</param>
        /// <param name="criteria">Criteria name</param>
        /// <param name="referenceTable">Out reference table</param>
        /// <returns>Return to table</returns>
        private DataTable GetWinLossFromOld(string uspKey, BetListCriteria criteria, out DataTable referenceTable)
        {
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    db.AddInParameter("loggedid", DbType.Int32, criteria.LoggedId);
                    db.AddInParameter("agentid", DbType.Int32, criteria.AgentId);
                    db.AddInParameter("custid", DbType.Int32, criteria.CustId);
                    db.AddInParameter("fromdate", DbType.DateTime, criteria.FromDate);
                    db.AddInParameter("todate", DbType.DateTime, criteria.ToDate);
                    db.AddInParameter("statusids", DbType.AnsiString, criteria.StatusIds);
                    db.AddInParameter("sporttypes", DbType.String, criteria.SportTypes);
                    db.AddInParameter("bettypes", DbType.String, criteria.BetTypes);

                    var dataSet = db.ExecuteDataSet();

                    // Main ticket list
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();

                    // Reference ticket
                    referenceTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();

                    return mainTable;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Read boolean value from an object
        /// </summary>
        /// <param name="value">Input value</param>
        /// <returns>To boolean</returns>
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