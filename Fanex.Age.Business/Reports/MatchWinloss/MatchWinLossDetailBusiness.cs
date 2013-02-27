namespace Fanex.Age.Business.Reports.MatchWinloss
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Age.DTO.Reports.MatchWinloss;

    public class MatchWinLossDetailBusiness
    {
        private bool isSuper = false;
        private bool isMaster = false;
        private bool isAgent = false;

        public MatchWinLossDetailBusiness(bool isSuper, bool isMaster, bool isAgent)
        {
            this.isSuper = isSuper;
            this.isMaster = isMaster;
            this.isAgent = isAgent;
        }

        public IList<MatchWinLossDetailItem> ToList(DataTable dt)
        {
            bool checkSuper = this.isSuper == true;
            bool checkMaster = this.isSuper == true || this.isMaster == true;
            bool checkAgent = this.isSuper == true || this.isMaster == true || this.isAgent == true;
            if (dt == null)
            {
                return null;
            }

            var result = (from c in dt.AsEnumerable()
                          select new MatchWinLossDetailItem
                          {
                              ShowTime = c.Field<string>("showtime"),
                              IsNeutral = c.Field<bool>("isNeutral"),
                              BetType = c.Field<short>("bettype"),
                              BetGroup = c.Field<int>("betgroup"),
                              LeagueId = c.Field<int>("leagueid"),
                              LiveIndicator = c.Field<int>("liveindicator"),
                              MatchId = c.Field<int>("matchid"),
                              MatchCode = c.Field<string>("matchcode"),
                              HomeId = c.Field<int>("homeid"),
                              AwayId = c.Field<int>("awayid"),
                              Race = c.Field<string>("race"),
                              GrossComm = c.Field<decimal>("GrossComm"),
                              Turnover = c.Field<decimal>("turnover"),
                              MemberWinloss = c.Field<decimal>("MemberWinLost"),
                              MemberComm = c.Field<decimal>("MemberComm"),
                              MemberTotal = c.Field<decimal>("MemberTotal"),
                              AgentWinloss = checkAgent ? c.Field<decimal>("AgentWinLost") : 0,
                              AgentComm = checkAgent ? c.Field<decimal>("AgentComm") : 0,
                              AgentTotal = checkAgent ? c.Field<decimal>("AgentTotal") : 0,
                              MasterWinloss = checkMaster ? c.Field<decimal>("MasterWinLost") : 0,
                              MasterComm = checkMaster ? c.Field<decimal>("MasterComm") : 0,
                              MasterTotal = checkMaster ? c.Field<decimal>("MasterTotal") : 0,
                              SuperWinloss = checkSuper ? c.Field<decimal>("SAgWinLost") : 0,
                              SuperComm = checkSuper ? c.Field<decimal>("SAgComm") : 0,
                              SuperTotal = checkSuper ? c.Field<decimal>("SAgTotal") : 0,
                              CompanyComm = c.Field<decimal>("CompanyComm"),
                              CompanyTotal = c.Field<decimal>("CompanyTotal")
                          }).ToList();

            return result;
        }
    }
}