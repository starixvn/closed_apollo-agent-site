// --------------------------------------------------------------------------
// <copyright file="AgentWinlossModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.12">Created</change>
//   <change who="Quinn.Vo" date="2013.7.12">Add Outstanding & ShareAmount</change>
// </history>

namespace Apollo.Age.Reports.Models
{
    using System;

    /// <summary>
    /// Agent win loss model class.
    /// </summary>
    [Serializable]
    public class AgentWinLossModel
    {
        public bool IsAgent { get; set; }

        public int AgentId { get; set; }

        public string AgentName { get; set; }

        /// <summary>
        /// Gets or sets the value of SoccerShare.
        /// </summary>
        /// <value>The SoccerShare.</value> 
        public short SoccerShare { get; set; }

        #region Member
        /// <summary>
        /// Gets or sets BetAmount.
        /// </summary>
        /// <value>The Turnover.</value> 
        public decimal BetAmount { get; set; }

        /// <summary>
        /// Gets or sets BetCount.
        /// </summary>
        /// <value>The BetCount.</value>
        public long BetCount { get; set; }

        /// <summary>
        /// Gets or sets Commission Base.
        /// </summary>
        /// <value>The ActualTO.</value>
        public decimal CommBase { get; set; }

        /// <summary>
        /// Gets or sets Member WinLoss.
        /// </summary>
        /// <value>The Member WinLoss.</value>
        public decimal MemberWinLoss { get; set; }

        /// <summary>
        /// Gets or sets Member Commission.
        /// </summary>
        /// <value>The Member Commission.</value>
        public decimal MemberComm { get; set; }

        /// <summary>
        /// Gets or sets Member Total.
        /// </summary>
        /// <value>The Member Commission.</value>
        public decimal MemberTotal { get; set; }

        #endregion

        #region Agent
        /// <summary>
        /// Gets or sets Agent WinLoss.
        /// </summary>
        /// <value>The AWinLoss.</value>
        public decimal AWinLoss { get; set; }

        /// <summary>
        /// Gets or sets Agent Commission.
        /// </summary>
        /// <value>The ACommission.</value>
        public decimal AComm { get; set; }

        /// <summary>
        /// Gets or sets Agent Total.
        /// </summary>
        /// <value>The ATotal.</value>
        public decimal ATotal { get; set; }
        #endregion

        #region Downline
        /// <summary>
        /// Gets or sets DownLine Effective Stake.
        /// </summary>
        /// <value>The DownLineEffectiveStake.</value>
        public decimal DownLineEffectiveStake { get; set; }

        public decimal DownLineTotal { get; set; }
        #endregion

        #region UpLine
        /// <summary>
        /// Gets or sets UpLine Effective Stake.
        /// </summary>
        /// <value>The UpLineEffectiveStake.</value>
        public decimal UpLineEffectiveStake { get; set; }

        /// <summary>
        /// Gets or sets UpLineWinLoss.
        /// </summary>
        /// <value>The UpLineEffectiveStake.</value>
        public decimal UpLineTotal { get; set; }
        #endregion
    }
}