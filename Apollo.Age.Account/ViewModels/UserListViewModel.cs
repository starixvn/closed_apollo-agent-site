namespace Apollo.Age.Account.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class UserListViewModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int UserLevel { get; set; }

        public string Status { get; set; }

        public string Group { get; set; }

        public CommissionViewModel Commission { get; set; }

        public SportTypeCommissionViewModel MemberCommission { get; set; }

        public SportTypeCommissionViewModel AgentCommission { get; set; }

        public Share SoccerShare { get; set; }
    }

    public class CommissionViewModel
    {
        public SportTypeCommissionViewModel SportTypeCommission { get; set; }

        public GroupCommissionViewModel GroupCommission { get; set; }
    }

    public class GroupCommissionViewModel
    {
        public decimal GroupA { get; set; }

        public decimal GroupB { get; set; }

        public decimal GroupC { get; set; }

        public decimal GroupD { get; set; }
    }

    public class SportTypeCommissionViewModel
    {
        public decimal Group { get; set; }

        public decimal OneXTwo { get; set; }

        public decimal Other { get; set; }

        public decimal NumberGame { get; set; }
    }

    public class Share
    {
        public decimal Handicap { get; set; }

        public decimal OverUnder { get; set; }

        public decimal HandicapLive { get; set; }

        public decimal OverUnderLive { get; set; }
    }
}