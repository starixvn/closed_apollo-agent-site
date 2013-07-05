// --------------------------------------------------------------------------
// <copyright file="TotalBetsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.3.31">Created</change>
// </history>

namespace Apollo.Age.TotalBets.Criterias
{
    using Apollo.Age.Enumerations;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get Total bet Criteria.
    /// </summary>
    public class TotalBetsCriteria : CriteriaBase
    {
        private int _agentId;

        public int AgentId
        {
            get { return _agentId; }
            set { _agentId = value; }
        }

        public int LoggedId { get; set; }

        public int BetType { get; set; }

        /// <summary>
        /// Get Setting Key for Get Data.
        /// </summary>
        /// <returns>Setting key.</returns>
        public override string GetSettingKey()
        {
            switch (BetType)
            {
                case (int)BetTypes.Handicap: //Handicap
                    return "Age_TotalBets_1_3_7_8";
                case (int)BetTypes.Total_Goal: //Total Goal
                    return "Age_TotalBets_6";
                case (int)BetTypes.Money_Line: // Money Line
                    return "Age_TotalBets_20_21";
                case (int)BetTypes.Odd_Even: // Odd/Even
                    return "Age_TotalBets_2_5_12_15_123";
                case (int)BetTypes.HT_FT: // HTFT
                    return "Age_TotalBets_16";
                case (int)BetTypes.Draw_No_Bet: // Home/Draw/Away/No Bet
                    return "Age_TotalBets_25_121_122";
                case (int)BetTypes.First_Goal_Last_Goal: // First Goal/Last Goal
                    return "Age_TotalBets_14";
                case (int)BetTypes.Outright: // Out Right
                    return "Age_TotalBets_10";
            }

            return string.Empty;
        }

        /// <summary>
        /// Can run store procedure.
        /// </summary>
        /// <returns>Can run or not.</returns>
        public override bool IsValid()
        {
            return AgentId > 0;
        }
    }
}
