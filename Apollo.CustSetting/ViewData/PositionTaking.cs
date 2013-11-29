// --------------------------------------------------------------------------
// <copyright file="PositionTaking.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.1">Created</change>
// </history>

namespace Apollo.CustSetting.ViewData
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class name.
    /// </summary>
    public class PositionTaking
    {
        public decimal Preset { get; set; }

        public decimal Min { get; set; }

        public decimal Max { get; set; }

        public decimal Value { get; set; }

        public decimal MinGiven { get; set; }

        public bool IsApplyMember { get; set; }

        public bool IsChangePt { get; set; }

        public PositionTaking Upline { get; set; }
    }
}