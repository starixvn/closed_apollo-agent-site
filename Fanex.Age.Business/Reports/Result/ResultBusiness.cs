namespace Fanex.Age.Business.Reports.Result
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.Result;

    public class ResultBusiness
    {
        private int sportTypeId = 0;
        private bool isOutright = false;
        private int leagueId = 0;

        public ResultBusiness(int sportTypeId, int leagueId, bool isOutright)
        {
            this.sportTypeId = sportTypeId;
            this.leagueId = leagueId;
            this.isOutright = isOutright;
        }

        public ResultBusiness(int sportTypeId)
            : this(sportTypeId, 0, false)
        {
        }

        public ResultBusiness(int sportTypeId, int leagueId)
            : this(sportTypeId, leagueId, true)
        {
        }

        public object ToList(DataTable dt)
        {
            if (this.isOutright)
            {
                return this.ToList5(dt);
            }
            else
            {
                switch (this.sportTypeId)
                {
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Soccer:
                        return this.ToList1<SoccerMatch>(dt);
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.BasketBall:
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Netball:
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.FootBall:
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Rugby:
                        return this.ToList1<BasketBallMatch>(dt);
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.IceHockey:
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.FieldHockey:
                        return this.ToList1<HockeyMatch>(dt);
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Tennis:
                        return this.ToList1<TennisMatch>(dt);
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.VolleyBall:
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Badminton:
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Squash:
                        return this.ToList1<BadmintonMatch>(dt);
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.BaseBall:
                        return this.ToList1<BaseBallMatch>(dt);
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Golf:
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Motorsports:
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Billard:
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Winter:
                        return this.ToList1<BilliardMatch>(dt);
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.HorseRacing:
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Greyhound:
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.Harness:
                        return this.ToList2(dt);
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.NumberGame:
                        return this.ToList3(dt);
                    case (int)Sunplus.Agent.Model.BL.Definitions.SportType.LiveCasino:
                        return this.ToList4(dt);
                    default:
                        return this.ToList1<OtherSportMatch>(dt);
                }
            }
        }

        private IList<SportLeague<T>> ToList1<T>(DataTable dt) where T : SportMatch1, new()
        {
            if (dt == null)
            {
                return null;
            }

            var matches = (from c in dt.AsEnumerable()
                           select new T
                           {
                               LeagueId = c.Field<int>("leagueid"),
                               KickOffTime = c.Field<DateTime>("kickofftime"),
                               EventStatus = c.Field<string>("eventstatus"),
                               IsPending = dt.Columns.Contains("IsPending") ? c.Field<bool>("IsPending") : false,
                               IsHTCal = dt.Columns.Contains("IsHTCal") ? c.Field<bool>("IsHTCal") : false,
                               Calculate = dt.Columns.Contains("calculate") ? c.Field<bool>("calculate") : false,
                               HomeId = c.Field<int>("homeid"),
                               AwayId = c.Field<int>("awayid"),
                               HTHomeScore = c["hthomescore"] == DBNull.Value ? (int?)null : c.Field<int?>("hthomescore"),
                               HTAwayScore = c["htawayscore"] == DBNull.Value ? (int?)null : c.Field<int?>("htawayscore"),
                               FTHomeScore = c["finalhomescore"] == DBNull.Value ? (int?)null : c.Field<int?>("finalhomescore"),
                               FTAwayScore = c["finalawayscore"] == DBNull.Value ? (int?)null : c.Field<int?>("finalawayscore"),
                               BetStatus = c.Field<string>("betstatus"),
                               IsNeutral = c.Field<bool>("isneutral")
                           }).ToList();

            return matches.GroupBy(p => p.LeagueId).Select(p => new SportLeague<T> { LeagueId = p.Key, Matches = p.ToList() }).ToList();
        }

        private IList<SportLeague<HorseRacingMatch>> ToList2(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = new List<SportLeague<HorseRacingMatch>>();
            var races = (from c in dt.AsEnumerable()
                         select new HorseRacingRace
                         {
                             LeagueId = c.Field<int>("meetingid"),
                             Place = c.Field<int>("place"),
                             RaceName = c.Field<int>("racename"),
                             HorseNo = c.Field<string>("horseno"),
                             HorseId = c.Field<int>("horseid"),
                             PlaceOdds = c.Field<decimal>("placeodds"),
                             Odds = c.Field<decimal>("odds"),
                             EventDate = c.Field<DateTime>("eventdate"),
                             FOddsStatus = c.Field<string>("foddsstatus"),
                             IsSCR = c.Field<bool>("isscr"),
                             IsFavor = c.Field<bool>("isfavor")
                         }).ToList();

            // get list of LeagueId
            var leagueList = races.Select(p => p.LeagueId).Distinct();
            foreach (var leagueId in leagueList)
            {
                var league = new SportLeague<HorseRacingMatch>()
                {
                    LeagueId = leagueId
                };

                // get list of players in league
                var leagueRaces = races.Where(p => p.LeagueId == leagueId);

                // get list of race in league
                var racenames = leagueRaces.Select(p => p.RaceName).Distinct();
                foreach (var racename in racenames)
                {
                    var match = new HorseRacingMatch()
                    {
                        LeagueId = leagueId,
                        RaceName = racename,
                        Races = leagueRaces.Where(p => p.RaceName == racename).OrderBy(p => p.Place).ToList()
                    };
                    league.Matches.Add(match);
                }

                result.Add(league);
            }

            return result;
        }

        private IList<SportLeague<NumberGameMatch>> ToList3(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var result = new List<SportLeague<NumberGameMatch>>();
            var balls = (from c in dt.AsEnumerable()
                         select new NumberGameBall
                         {
                             LeagueId = c.Field<int>("leagueid"),
                             KickOffTime = c.Field<DateTime>("kickofftime"),
                             MatchId = c.Field<int>("matchid"),
                             MatchCode = c.Field<string>("matchcode"),
                             Seq = c["seq"] == DBNull.Value ? (byte?)null : c.Field<byte?>("seq"),
                             Number = c["number"] == DBNull.Value ? (byte?)null : c.Field<byte?>("number"),
                             EventStatus = c.Field<string>("eventstatus"),
                             IsPending = dt.Columns.Contains("ispending") ? c.Field<bool?>("ispending") : (bool?)null
                         }).ToList();

            // get list of LeagueId
            var leagueList = balls.Select(p => p.LeagueId).Distinct().OrderBy(p => p);
            foreach (var leagueId in leagueList)
            {
                var league = new SportLeague<NumberGameMatch>()
                {
                    LeagueId = leagueId
                };

                // get list of SportMatch3 having LeagueId == leagueId
                var matches = balls.Where(p => p.LeagueId == leagueId);

                // get list of MatchId and order
                var matchList = matches.OrderBy(p => p.MatchId).Select(p => p.MatchId).Distinct();
                foreach (var matchId in matchList)
                {
                    // get list of number ordered by Seq
                    IList<byte?> numbers = matches.Where(p => p.MatchId == matchId && p.Number != null).OrderBy(p => p.Seq).Select(p => p.Number).ToList();
                    var firstMatch = matches.Where(p => p.MatchId == matchId).First();
                    var match = new NumberGameMatch()
                    {
                        LeagueId = leagueId,
                        KickOffTime = firstMatch.KickOffTime,
                        MatchId = matchId,
                        MatchCode = firstMatch.MatchCode,
                        Seq = firstMatch.Seq,
                        EventStatus = firstMatch.EventStatus,
                        IsPending = firstMatch.IsPending,
                        BallList = numbers
                    };
                    league.Matches.Add(match);
                }

                result.Add(league);
            }

            return result;
        }

        private IList<SportLeague<LiveCasinoMatch>> ToList4(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var matches = (from c in dt.AsEnumerable()
                           select new LiveCasinoMatch
                           {
                               LeagueId = c.Field<int>("leagueid"),
                               KickOffTime = c.Field<DateTime>("kickofftime"),
                               EventStatus = c.Field<string>("eventstatus"),
                               IsPending = dt.Columns.Contains("IsPending") ? c.Field<bool>("IsPending") : false,
                               IsHTCal = dt.Columns.Contains("IsHTCal") ? c.Field<bool>("IsHTCal") : false,
                               Calculate = dt.Columns.Contains("calculate") ? c.Field<bool>("calculate") : false,
                               HomeId = c.Field<int>("homeid"),
                               HomeId2 = c.Field<int>("homeid2"),
                               AwayId = c.Field<int>("awayid"),
                               HTHomeScore = c["hthomescore"] == DBNull.Value ? (int?)null : c.Field<int?>("hthomescore"),
                               HTAwayScore = c["htawayscore"] == DBNull.Value ? (int?)null : c.Field<int?>("htawayscore"),
                               FTHomeScore = c["finalhomescore"] == DBNull.Value ? (int?)null : c.Field<int?>("finalhomescore"),
                               FTAwayScore = c["finalawayscore"] == DBNull.Value ? (int?)null : c.Field<int?>("finalawayscore"),
                               BetStatus = c.Field<string>("betstatus"),
                               ShowTime = c.Field<string>("showtime")
                           }).ToList();

            return matches.GroupBy(p => p.LeagueId).Select(p => new SportLeague<LiveCasinoMatch> { LeagueId = p.Key, Matches = p.ToList() }).ToList();
        }

        private IList<SportLeague<OutrightMatch>> ToList5(DataTable dt)
        {
            if (dt == null)
            {
                return null;
            }

            var matches = from c in dt.AsEnumerable()
                          select new OutrightMatch
                          {
                              LeagueId = c.Field<int>("leagueid"),
                              SportType = c.Field<byte>("sporttype"),
                              TeamId = c.Field<int>("teamid"),
                              EventDate = c.Field<DateTime>("eventdate"),
                              IsPending = dt.Columns.Contains("IsPending") ? c.Field<bool>("IsPending") : false,
                              IsHTCal = dt.Columns.Contains("IsHTCal") ? c.Field<bool>("IsHTCal") : false,
                              Calculate = dt.Columns.Contains("calculate") ? c.Field<bool>("calculate") : false,
                              Status = c.Field<string>("Status")
                          };

            if (this.sportTypeId != 0)
            {
                matches = matches.Where(p => p.SportType == this.sportTypeId);
            }

            if (this.leagueId != 0)
            {
                matches = matches.Where(p => p.LeagueId == this.leagueId);
            }

            return matches.GroupBy(p => p.LeagueId).Select(p => new SportLeague<OutrightMatch> { LeagueId = p.Key, Matches = p.ToList() }).ToList();
        }
    }
}