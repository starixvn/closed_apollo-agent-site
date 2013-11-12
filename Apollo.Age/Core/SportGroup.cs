//-----------------------------------------------------------------------
// <copyright file="SportGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// <summary>This is the SportGroup</summary>
// <history>
//   <change who="Lewis.Le" date="2013.5.8">Created</change>
// </history>
//-----------------------------------------------------------------------

namespace Apollo.Age.Core
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// This class to hard code for sport type Id.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2100:CodeLineMustNotBeLongerThan", Justification = "Reviewed. Suppression is OK here.")]
    public static class SportGroup
    {
        public const string All = "0";
        public const string Soccer = "1";
        public const string BasketballFootballRugy = "2, 3, 26";
        public const string Tennis = "5";
        public const string VolleyballBadmintonTableTennis = "6, 9, 18";
        public const string BaseBall = "8";
        public const string Others = "4, 7, 10, 11, 12, 13, 14, 15, 16, 17, 19, 20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 99";
        public const string Outright = "-1";
    }
}