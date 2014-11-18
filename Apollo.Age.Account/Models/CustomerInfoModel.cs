// --------------------------------------------------------------------------
// <copyright file="CustomerInfoModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.10">Created</change>
// </history>

namespace Apollo.Age.Account.Models
{
    using System.Collections.Generic;
    using Apollo.Age.Entities;

    /// <summary>
    /// Customer information model.
    /// </summary>
    public class CustomerInfoModel
    {
        public int ActorAgentId { get; set; }

        public string ActorAgentUsername { get; set; }

        public bool IsMember { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public short Level { get; set; }

        public bool HasAgentDownline { get; set; }

        public bool HasMemberDownline { get; set; }

        public string Group { get; set; }

        public int ParentId { get; set; }

        public string ParentName { get; set; }

        public bool IsMaster { get; set; }

        public bool IsSuper { get; set; }

        public byte Status { get; set; }

        public byte UplineStatus { get; set; }

        public byte BetStatus { get; set; }

        public byte UplineBetStatus { get; set; }

        public decimal SportCredit { get; set; }

        public decimal CashBalance { get; set; }

        public decimal UserBalance
        {
            get
            {
                if (IsMember)
                {
                    return CashBalance;
                }

                return SportCredit;
            }
        }

        public List<Agent> AgentPathList { get; set; }

        public decimal SportCredit { get; set; }
    }
}