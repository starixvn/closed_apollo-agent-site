// --------------------------------------------------------------------------
// <copyright file="MatchEnteredModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.11">Created</change>
// </history>

namespace Apollo.Age.Reports.Models
{
    using System;

    public class MatchEnteredModel
    {
        public bool IsToday { get; set; }

        public DateTime? EventDate { get; set; }

        public short SportType { get; set; }

        public int Entered { get; set; }

        public int NonEntered { get; set; }
    }
}
