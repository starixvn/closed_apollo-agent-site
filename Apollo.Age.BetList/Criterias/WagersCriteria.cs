// --------------------------------------------------------------------------
// <copyright file="WagersCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Kaden.Trinh" date="2013.04.09">Created and Implement</change>
//   <change who="Terry.Nguyen" date="2013.7.15">Add param IsSuperView</change>
// </history>
namespace Apollo.Age.BetList.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.NAP.Framework.Data;

    public class WagersCriteria : CriteriaBase
    {
        public int LoggedId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string UGroup { get; set; }

        public bool IsDirectMem { get; set; }

        public bool LiveIndicator { get; set; }

        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        public string BetTypes { get; set; }

        public string SportTypes { get; set; }

        public string BetTeam { get; set; }

        public bool IsSuperView { get; set; }

        public override string GetSettingKey()
        {
            return "Age_BetList_SelWagers";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}