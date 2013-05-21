// --------------------------------------------------------------------------
// <copyright file="ResultRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.24">Created</change>
// </history>

namespace Apollo.Age.Reports.Repositories
{
    using System;
    using System.Data;
    using Fanex.Data;

    /// <summary>
    ///  move code from Sunplus.Agent.Model, it's temporarily use
    /// </summary>
    public class ResultRepository
    {
        public DataTable GetResultMatchByLeague1(DateTime date, int custId, int sportId, int leagueId, string language)
        {
            var dataTable = new DataTable();
            using (ObjectDb db = new ObjectDb("Age_Result_MatchSelByLeague"))
            {
                DataSet ds = db.ExecuteDataSet(new { fromdate = date, sporttype = sportId, custId = custId, leagueid = leagueId, language = language });

                if (ds != null && ds.Tables.Count > 0)
                {
                    dataTable = ds.Tables[0];
                }
            }

            return dataTable;
        }

        public DataTable GetResultSportByDate1(DateTime date)
        {
            var dataTable = new DataTable();
            using (ObjectDb db = new ObjectDb("Age_Result_SportSelByDate"))
            {
                DataSet ds = db.ExecuteDataSet(new { eventdate = date });

                if (ds != null && ds.Tables.Count > 0)
                {
                    dataTable = ds.Tables[0];
                }
            }

            return dataTable;
        }

        public DataTable GetOutrightResult1(DateTime fromDate, DateTime toDate)
        {
            var dataTable = new DataTable();
            using (ObjectDb db = new ObjectDb("Age_Result_Outright"))
            {
                DataSet ds = db.ExecuteDataSet(new { fromDate = fromDate, toDate = toDate });

                if (ds != null && ds.Tables.Count > 0)
                {
                    dataTable = ds.Tables[0];
                }
            }

            return dataTable;
        }

        public DataTable GetResultLeagueByDate1(DateTime date, int sportId, string language)
        {
            var dataTable = new DataTable();
            using (ObjectDb db = new ObjectDb("Age_Result_LeagueSelByDate"))
            {
                DataSet ds = db.ExecuteDataSet(new { eventdate = date, sporttype = sportId, language = language });

                if (ds != null && ds.Tables.Count > 0)
                {
                    dataTable = ds.Tables[0];
                }
            }

            return dataTable;
        }

        public DataTable SelectMatchForViewResult1(int matchId, string raceno)
        {
            var dataTable = new DataTable();
            using (ObjectDb db = new ObjectDb("Age_BetList_SelMatchResult"))
            {
                DataSet ds = db.ExecuteDataSet(new { matchid = matchId, raceno = raceno });

                if (ds != null && ds.Tables.Count > 0)
                {
                    dataTable = ds.Tables[0];
                }
            }

            return dataTable;
        }

        public DataTable SelectMatchResultParlay1(long refno)
        {
            var dataTable = new DataTable();
            using (ObjectDb db = new ObjectDb("Age_BetList_SelMatchResultParlay"))
            {
                DataSet ds = db.ExecuteDataSet(new { refno = refno });

                if (ds != null && ds.Tables.Count > 0)
                {
                    dataTable = ds.Tables[0];
                }
            }

            return dataTable;
        }
    }
}
