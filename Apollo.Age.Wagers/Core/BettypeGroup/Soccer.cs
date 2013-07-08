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
        public const string RunningBall = "1, 2, 3, 5, 7, 8, 12, 15, 20";
        public const string SpecialBet = "11, 13, 14, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 121, 122, 123, 124, 125, 1301, 1302, 1303, 1304, 1305, 1306, 1307, 1308, 1309, 1310, 1311, 1312, 1313, 1314, 1315, 1316, 1317, 1318, 1319, 1320, 1321, 1322, 1323, 1324, 1325, 1326, 1327, 1328, 1329, 1330, 1331, 1332, 1333, 1334";
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
