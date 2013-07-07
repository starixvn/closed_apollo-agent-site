// --------------------------------------------------------------------------
// <copyright file="Basketball.cs" company="Nexcel Solutions Vietnam">
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
    /// Basketball class.
    /// </summary>
    public class Basketball
    {
        public const string Straight = "1, 2, 3";
        public const string FirstHalf = "7, 8, 12";
        public const string RunningBall = "1, 3";
        public const string Parlay = "9, 29";
        public const string SpecialBet = "4, 5, 6, 10, 11, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 99";

        /// <summary>
        /// Get bet type by group bet name.
        /// </summary>
        /// <returns>Dictionary of bet type.</returns>
        public static Dictionary<string, string> ListOfBettype()
        {
            var listOfBettype = new Dictionary<string, string>();

            listOfBettype.Add("straight", Basketball.Straight);
            listOfBettype.Add("firsthalf", Basketball.FirstHalf);
            listOfBettype.Add("runningball", Basketball.RunningBall);
            listOfBettype.Add("specialbet", Basketball.SpecialBet);
            listOfBettype.Add("parlay", Basketball.Parlay);

            return listOfBettype;
        }
    }
}
