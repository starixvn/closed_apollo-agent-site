// --------------------------------------------------------------------------
// <copyright file="TotalBetCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.31">Created</change>
// </history>

namespace Apollo.Age.TotalBets.Criterias
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Fanex.NAP.Framework.Data;


    public class TotalBetsCriteria : CriteriaBase
    {
        private string _settingKey;
        private int _agentId;

        public override string GetSettingKey()
        {
            switch (BetType)
            {
                case 1: //Handicap
                    return "Age_TotalBets_1_3_7_8";
                case 6: //Total Goal
                    return "Age_TotalBets_6";
                case 20: // Money Line
                    return "Age_TotalBets_20_21";
                case 2: // Odd/Even
                    return "Age_TotalBets_2_5_12_15_123";

            }

            return _settingKey;
        }

        public int AgentId
        {
            get { return _agentId; }
            set { _agentId = value; }
        }

        public int BetType { get; set; }

        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
