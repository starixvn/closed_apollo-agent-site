// --------------------------------------------------------------------------
// <copyright file="MatchEnteredDTO.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.11">Created</change>
// </history>

namespace Apollo.Age.Reports.DTO
{
    using System;

    public class MatchEnteredDTO
    {
        public int IsToday { get; set; }

        public DateTime EventDate { get; set; }

        public byte SportType { get; set; }

        public int Entered { get; set; }

        public int NonEntered { get; set; }
    }
}
