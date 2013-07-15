// --------------------------------------------------------------------------
// <copyright file="OutstandingModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.4.9">Created</change>
// </history>


namespace Apollo.Age.Reports.Models
{
    using System;

    [Serializable]
    public class OutstandingModel
    {
        public bool IsAgent { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public decimal Outstanding { get; set; }

        public decimal ShareAmount { get; set; }

        public decimal GameOutstanding { get; set; }

        public decimal GameShareAmount { get; set; }
    }
}