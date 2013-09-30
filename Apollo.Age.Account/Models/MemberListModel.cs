namespace Apollo.Age.Account.Models
{
    using System;

    public class MemberListModel
    {
        public int MemberId { get; set; }

        public string Username { get; set; }

        public byte Status { get; set; }

        public byte UplineStatus { get; set; }

        public byte BetStatus { get; set; }

        public byte UplineBetStatus { get; set; }

        public int HandicapTakenShare { get; set; }

        public string UserGroup { get; set; }

        public decimal MemberGroup { get; set; }

        public decimal MemberDiscountOneXTwo { get; set; }

        public decimal MemberDiscountOther { get; set; }

        public decimal AgentGroup { get; set; }

        public decimal AgentDiscountOneXTwo { get; set; }

        public decimal AgentDiscountOther { get; set; }

        public decimal CashBalance { get; set; }
    }
}