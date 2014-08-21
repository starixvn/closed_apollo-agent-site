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
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.BetList.Criterias;
    using Apollo.Age.BetList.Repositories.Interface;
    using Apollo.Age.Entities;
    using Apollo.BetList.Core.Entities;
    using Fanex.Common;
    using Fanex.Data;

    /// <summary>
    /// BetList Repository.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class BetListRepository : BetListBaseRepository, IBetListRepository
    {
        /// <summary>
        /// Get detail agent PT and agent commission.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>List Ticket.</returns>
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
        /// Get detail win loss agent PT and agent commission.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>List Ticket.</returns>
        public IList<ITicket> GetDetailWinLossAgentComm(BetListCriteria criteria)
        {
            try
            {
                DataTable mainTable = this.GetDetailWinLossNewOrOld("Age_BetList_AgentDetail", criteria);

                if (mainTable == null || mainTable.Rows.Count <= 0)
                {
                    mainTable = this.GetDetailWinLossNewOrOld("Age_BetList_AgentDetail_Old", criteria);
                }

                // Main ticket list.
                var ticketList = this.ReadTicket(mainTable);

                return ticketList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get detail ticket mix-parlay.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>List Ticket.</returns>
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
        /// Get bet list running on Total Bets menu.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>Ticket data.</returns>
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

                    // Main ticket list.
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    ticketResult.TicketList = this.ReadTicket(mainTable);

                    // Reference ticket.
                    var referenceTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();
                    ticketResult.TicketListMixParlay = this.ReadTicket(referenceTable);
                    return ticketResult;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get bet list by matchId.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>Ticket data.</returns>
        public TicketResult GetRunningByBettypeAndMatch(BetListCriteria criteria)
        {
            var uspKey = "Age_BetList_TotalBets_SelRunningByBettypeMatch";
            var uspParams = new
            {
                MatchID = criteria.MatchId,
                LoggedId = criteria.LoggedId,
                AgentId = criteria.AgentId,
                Bettypes = criteria.BetTypes
            };

            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    var dataSet = db.ExecuteDataSet(uspParams);
                    var ticketResult = new TicketResult();

                    // Main ticket list.
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();
                    ticketResult.TicketList = this.ReadTicket(mainTable);

                    // Reference ticket.
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
        /// Get all bets by date and member from Win loss listOutstanding.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>Ticket data.</returns>
        public TicketResult GetWinLossByDateAndMem(BetListCriteria criteria)
        {
            try
            {
                DataTable mainTable, referenceTable;
                var ticketResult = new TicketResult();
                var preBalanceUpDay = criteria.BalanceUpDay.AddDays(-1);
                if (criteria.FromDate < this.ResetStartDate(criteria.BalanceUpDay))
                {
                    if (criteria.ToDate <= this.ResetEndDate(preBalanceUpDay))
                    {
                        mainTable = this.GetWinLossFromOld("Age_BetList_SelWinloss_Old", criteria, out referenceTable);
                    }
                    else
                    {
                        // Separate 2 parts: dateFrom --> preBalanceUpDate, balanceUpDate --> dateTo.
                        DataTable referenceOldTable = null;
                        var mainOldTable = this.GetWinLossFromOld("Age_BetList_SelWinloss_Old", criteria, out referenceOldTable);

                        DataTable referenceNewTable = null;
                        var mainNewTable = this.GetWinLossFromNew("Age_BetList_SelWinloss", criteria, out referenceNewTable);

                        // Merge two table bettrans.
                        mainOldTable.Merge(mainNewTable);
                        mainTable = mainOldTable;

                        // Merge table reference.
                        referenceOldTable.Merge(referenceNewTable);
                        referenceTable = referenceOldTable;
                    }
                }
                else
                {
                    mainTable = this.GetWinLossFromNew("Age_BetList_SelWinloss", criteria, out referenceTable);
                }

                // Main ticket list.
                ticketResult.TicketList = this.ReadTicket(mainTable);

                // Reference ticket.
                ticketResult.TicketListMixParlay = this.ReadTicket(referenceTable);

                return ticketResult;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// BetList: Transaction, Cancel.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>Ticket data.</returns>
        public TicketResult GetTicketResult(BetListCriteria criteria)
        {
            try
            {
                var ticketResult = new TicketResult();
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var dataSet = db.ExecuteDataSet(criteria.GetParameters());

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
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the ticket result.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="isIncludeOldData">Include old data from Archive database or not.</param>
        /// <returns>Ticket result data.</returns>
        public TicketResult GetTicketResult(BetListCriteria criteria, bool isIncludeOldData = false)
        {
            try
            {
                var ticketResult = new TicketResult();
                var balanceUpDay = criteria.BalanceUpDay;
                var preBalanceUpDay = balanceUpDay.AddDays(-1);
                if (criteria.IsValid())
                {
                    if (isIncludeOldData)
                    {
                        if (criteria.FromDate < this.ResetStartDate(balanceUpDay))
                        {
                            if (criteria.ToDate <= this.ResetEndDate(preBalanceUpDay))
                            {
                                // Get data from Archive database only.
                                using (var db = new ObjectDb("Age_BetList_SelByDateMemStatus_Old"))
                                {
                                    var uspParams = new
                                    {
                                        loggedid = criteria.LoggedId,
                                        agentid = criteria.AgentId,
                                        custid = criteria.CustId,
                                        fromdate = criteria.FromDate,
                                        todate = criteria.ToDate > this.ResetEndDate(preBalanceUpDay) ? this.ResetEndDate(preBalanceUpDay) : criteria.ToDate,
                                        statusids = criteria.StatusIds,
                                        sporttypes = string.IsNullOrWhiteSpace(criteria.SportTypes) ? "0" : criteria.SportTypes,
                                        bettypes = string.IsNullOrWhiteSpace(criteria.BetTypes) ? "0" : criteria.BetTypes,
                                        rpttype = criteria.RptType,
                                    };

                                    var dataSet = db.ExecuteDataSet(uspParams);

                                    if (dataSet.Tables.Count > 1)
                                    {
                                        ticketResult.TicketList = this.ReadTicket(dataSet.Tables[0]);
                                        ticketResult.TicketListMixParlay = this.ReadTicket(dataSet.Tables[1]);
                                    }
                                    else
                                    {
                                        ticketResult.TicketList = this.ReadTicket(dataSet.Tables[0]);
                                        ticketResult.TicketListMixParlay = new List<ITicket>();
                                    }
                                }
                            }
                            else
                            {
                                // Get data from main database and archive database.
                                var oldDatatable = new DataTable();
                                var newDatatable = new DataTable();
                                var oldReferenceDatatable = new DataTable();
                                var newReferenceDatatable = new DataTable();

                                // Get data from Archive database only.
                                using (var db = new ObjectDb("Age_BetList_SelByDateMemStatus_Old"))
                                {
                                    var uspParams = new
                                    {
                                        loggedid = criteria.LoggedId,
                                        agentid = criteria.AgentId,
                                        custid = criteria.CustId,
                                        fromdate = criteria.FromDate,
                                        todate = criteria.ToDate > this.ResetEndDate(preBalanceUpDay) ? this.ResetEndDate(preBalanceUpDay) : criteria.ToDate,
                                        statusids = criteria.StatusIds,
                                        sporttypes = string.IsNullOrWhiteSpace(criteria.SportTypes) ? "0" : criteria.SportTypes,
                                        bettypes = string.IsNullOrWhiteSpace(criteria.BetTypes) ? "0" : criteria.BetTypes,
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

                                // Get data from Archive database only.
                                using (var db = new ObjectDb("Age_BetList_SelByDateMemStatus"))
                                {
                                    var uspParams = new
                                    {
                                        loggedid = criteria.LoggedId,
                                        agentid = criteria.AgentId,
                                        custid = criteria.CustId,
                                        fromdate = criteria.FromDate < this.ResetStartDate(balanceUpDay) ? this.ResetStartDate(balanceUpDay) : criteria.FromDate,
                                        todate = criteria.ToDate,
                                        statusids = criteria.StatusIds,
                                        sporttypes = string.IsNullOrWhiteSpace(criteria.SportTypes) ? "0" : criteria.SportTypes,
                                        bettypes = string.IsNullOrWhiteSpace(criteria.BetTypes) ? "0" : criteria.BetTypes,
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
                                ticketResult.TicketListMixParlay = this.ReadTicket(newReferenceDatatable);
                            }
                        }
                        else
                        {
                            // Get data from main database only.
                            using (var db = new ObjectDb("Age_BetList_SelByDateMemStatus"))
                            {
                                var uspParams = new
                                {
                                    loggedid = criteria.LoggedId,
                                    agentid = criteria.AgentId,
                                    custid = criteria.CustId,
                                    fromdate = criteria.FromDate < this.ResetStartDate(balanceUpDay) ? this.ResetStartDate(balanceUpDay) : criteria.FromDate,
                                    todate = criteria.ToDate,
                                    statusids = criteria.StatusIds,
                                    sporttypes = string.IsNullOrWhiteSpace(criteria.SportTypes) ? "0" : criteria.SportTypes,
                                    bettypes = string.IsNullOrWhiteSpace(criteria.BetTypes) ? "0" : criteria.BetTypes,
                                    rpttype = criteria.RptType,
                                    movebetsday = criteria.MoveBetsDay
                                };

                                var dataSet = db.ExecuteDataSet(uspParams);

                                if (dataSet.Tables.Count > 1)
                                {
                                    ticketResult.TicketList = this.ReadTicket(dataSet.Tables[0]);
                                    ticketResult.TicketListMixParlay = this.ReadTicket(dataSet.Tables[1]);
                                }
                                else
                                {
                                    ticketResult.TicketList = this.ReadTicket(dataSet.Tables[0]);
                                    ticketResult.TicketListMixParlay = new List<ITicket>();
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
        /// Get data bet list on Wagers page.
        /// </summary>
        /// <param name="criteria">The wagers criteria.</param>
        /// <returns>Return a ticket object.</returns>
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

                        // Main ticket list.
                        var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();

                        ticketResult.TicketList = this.ReadTicket(mainTable);
                        var referenceTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();
                        ticketResult.TicketListMixParlay = this.ReadTicket(referenceTable);
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
        /// Get Combination Mix Parlay Detail.
        /// </summary>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>System Parlay data.</returns>
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
        /// Get User Id By Username.
        /// </summary>
        /// <param name="loggedId">Logged id.</param>
        /// <param name="isAgent">Agent choice.</param>
        /// <param name="username">User name.</param>
        /// <param name="agentId">Output agent id.</param>
        /// <param name="custId">Output customer id.</param>
        /// <param name="errorCode">Output error code.</param>
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
        /// Get Agent up line of member.
        /// </summary>
        /// <param name="parentId">Logged id.</param>
        /// <param name="custId">Customer id.</param>
        /// <returns>List of Agent.</returns>
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
        /// Gets the account information ticket.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The ticket result.
        /// </returns>
        public TicketResult GetAccountInfoTicket(AccountInfoBetlistCriteria criteria)
        {
            var ticketResult = new TicketResult();

            criteria.FromDate = this.ResetStartDate(criteria.FromDate);
            criteria.Todate = this.ResetEndDate(criteria.Todate);

            ticketResult = this.GetTickets(criteria);

            return ticketResult;
        }

        /// <summary>
        /// Gets the out standing ticket.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The ticket result.</returns>
        public TicketResult GetOutStandingTicket(OutstandingCriteria criteria)
        {
            var ticketResult = new TicketResult();

            ticketResult = this.GetTickets(criteria);

            return ticketResult;
        }

        /// <summary>
        /// Gets the cancelled ticket.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The ticket result.
        /// </returns>
        public TicketResult GetCancelledTicket(CancelledTicketCriteria criteria)
        {
            var ticketResult = new TicketResult();

            ticketResult = this.GetTickets(criteria);

            return ticketResult;
        }

        /// <summary>
        /// Gets the transaction ticket.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// The ticket result.
        /// </returns>
        public TicketResult GetTransactionTicket(TransactionCriteria criteria)
        {
            var ticketResult = new TicketResult();

            ticketResult = this.GetTickets(criteria);

            return ticketResult;
        }

        /// <summary>
        /// Parse data table to list TicketData Object.
        /// </summary>
        /// <param name="data">Data table.</param>
        /// <returns>List Ticket Data.</returns>
        private List<ITicketData> ReadReferneceTicket(DataTable data)
        {
            var result = new List<ITicketData>();

            var containsBetTeam = data.Columns.Contains("betteam");
            var containsStake = data.Columns.Contains("stake");
            foreach (DataRow dataRow in data.Rows)
            {
                var ticket = new TicketData();

                ticket.BetTeam = containsBetTeam ? dataRow["betteam"].ToString() : string.Empty;
                ticket.RefNo = dataRow["refno"].ToString();
                ticket.Status = dataRow["status"].ToString();

                ticket.BetTypeId = Convert.ToInt32(dataRow["bettype"]);

                ticket.Stake = containsStake ? Convert.ToDecimal(dataRow["stake"]) : 0;

                ticket.Odds = Convert.ToDecimal(dataRow["odds"]);

                result.Add(ticket);
            }

            return result;
        }

        /// <summary>
        /// Get detail Win loss from new or archive.
        /// </summary>
        /// <param name="uspKey">Store name.</param>
        /// <param name="criteria">Criteria name.</param>
        /// <returns>Return to table.</returns>
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

                    // Main ticket list.
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
        /// Get bet list Win loss from new or archive.
        /// </summary>
        /// <param name="uspKey">Store name.</param>
        /// <param name="criteria">Criteria name.</param>
        /// <param name="referenceTable">Out reference table.</param>
        /// <returns>Return to table.</returns>
        private DataTable GetWinLossFromNewOrOld(string uspKey, BetListCriteria criteria, out DataTable referenceTable)
        {
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    var dataSet = db.ExecuteDataSet(criteria.GetSpParams());

                    // Main ticket list.
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();

                    // Reference ticket.
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
        /// Get bet list Win loss from new or archive.
        /// </summary>
        /// <param name="uspKey">Store name.</param>
        /// <param name="criteria">Criteria name.</param>
        /// <param name="referenceTable">Out reference table.</param>
        /// <returns>Return to table.</returns>
        private DataTable GetWinLossFromNew(string uspKey, BetListCriteria criteria, out DataTable referenceTable)
        {
            try
            {
                using (var db = new ObjectDb(uspKey))
                {
                    var fromDate = criteria.FromDate < this.ResetStartDate(criteria.BalanceUpDay)
                        ? this.ResetStartDate(criteria.BalanceUpDay)
                        : criteria.FromDate;

                    db.AddInParameter("loggedid", DbType.Int32, criteria.LoggedId);
                    db.AddInParameter("agentid", DbType.Int32, criteria.AgentId);
                    db.AddInParameter("custid", DbType.Int32, criteria.CustId);
                    db.AddInParameter("fromdate", DbType.DateTime, fromDate);
                    db.AddInParameter("todate", DbType.DateTime, criteria.ToDate);
                    db.AddInParameter("statusids", DbType.AnsiString, criteria.StatusIds);
                    db.AddInParameter("movebetsday", DbType.DateTime, criteria.MoveBetsDay);
                    db.AddInParameter("sporttypes", DbType.String, criteria.SportTypes);
                    db.AddInParameter("bettypes", DbType.String, criteria.BetTypes);

                    var dataSet = db.ExecuteDataSet();

                    // Main ticket list.
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();

                    // Reference ticket.
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
        /// Get bet list Win loss from new or archive.
        /// </summary>
        /// <param name="uspKey">Store name.</param>
        /// <param name="criteria">Criteria name.</param>
        /// <param name="referenceTable">Out reference table.</param>
        /// <returns>Return to table.</returns>
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
                    db.AddInParameter(
                        "todate",
                        DbType.DateTime,
                        criteria.ToDate > this.ResetEndDate(criteria.BalanceUpDay.AddDays(-1)) ? this.ResetEndDate(criteria.BalanceUpDay.AddDays(-1)) : criteria.ToDate);
                    db.AddInParameter("statusids", DbType.AnsiString, criteria.StatusIds);
                    db.AddInParameter("sporttypes", DbType.String, criteria.SportTypes);
                    db.AddInParameter("bettypes", DbType.String, criteria.BetTypes);

                    var dataSet = db.ExecuteDataSet();

                    // Main ticket list.
                    var mainTable = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable();

                    // Reference ticket.
                    referenceTable = dataSet.Tables.Count > 1 ? dataSet.Tables[1] : new DataTable();

                    return mainTable;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}