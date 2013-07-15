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

    [Serializable]
    public class AgentWinLossModel
    {
        public bool IsAgent { get; set; }

        public int AgentId { get; set; }

        public string AgentName { get; set; }

        public short SoccerShare { get; set; }

        public decimal Turnover { get; set; }

        public decimal ActualTO { get; set; }

        public decimal GrossTotal { get; set; }

        public decimal AWinLoss { get; set; }

        public decimal AComm { get; set; }

        public decimal ATotal { get; set; }

        public decimal UplineTotal { get; set; }

        public decimal Outstanding { get; set; }

        public decimal OutstandingShare { get; set; }
    }
}