// --------------------------------------------------------------------------
// <copyright file="Soccer.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.7.7">Created</change>
// </history>
namespace Apollo.Age.Wagers.Core.BettypeGroup
{
    using System.Collections.Generic;

    /// <summary>
    /// Soccer class.
    /// </summary>
    public static class Soccer
    {
        public const string Straight = "1, 3, 5";
        public const string FirstHalf = "7, 8, 15";
        public const string RunningBall = "1, 3, 7, 8";
        public const string SpecialBet = "4, 7, 10, 11, 12, 13, 14, 15, 16, 17, 19, 20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 99";
        public const string CorrectScore = "4";
        public const string FirstHalfCorrectScore = "30";
        public const string HalfFull = "16";
        public const string Total = "2, 12, 6";
        public const string Parlay = "9, 29";
        public const string OutRight = "10";

        /// <summary>
        /// Get bet type by group bet name.
        /// </summary>
        /// <returns>Dictionary of bet type.</returns>
        public static Dictionary<string, string> ListOfBettype()
        {
            var listOfBettype = new Dictionary<string, string>();

            listOfBettype.Add("straight", Soccer.Straight);
            listOfBettype.Add("firsthalf", Soccer.FirstHalf);
            listOfBettype.Add("runningball", Soccer.RunningBall);
            listOfBettype.Add("specialbet", Soccer.SpecialBet);
            listOfBettype.Add("correctscore", Soccer.CorrectScore);
            listOfBettype.Add("firsthalfcorrectscore", Soccer.FirstHalfCorrectScore);
            listOfBettype.Add("halffull", Soccer.HalfFull);
            listOfBettype.Add("total", Soccer.Total);
            listOfBettype.Add("parlay", Soccer.Parlay);

            return listOfBettype;
        }
    }
}
