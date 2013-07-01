using System.Collections.Generic;

namespace Apollo.Age.Wagers.Core.BettypeGroup
{
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

        public static Dictionary<string, string> ListOfBettype()
        {
            var listOfBettype = new Dictionary<string, string>();

            listOfBettype.Add("Straight", Soccer.Straight);
            listOfBettype.Add("FirstHalf", Soccer.FirstHalf);
            listOfBettype.Add("RunningBall", Soccer.RunningBall);
            listOfBettype.Add("SpecialBet", Soccer.SpecialBet);
            listOfBettype.Add("CorrectScore", Soccer.CorrectScore);
            listOfBettype.Add("FirstHalfCorrectScore", Soccer.FirstHalfCorrectScore);
            listOfBettype.Add("HalfFull", Soccer.HalfFull);
            listOfBettype.Add("Total", Soccer.Total);
            listOfBettype.Add("Parlay", Soccer.Parlay);

            return listOfBettype;
        }
    }

    public class Basketball
    {
        public const string Straight = "1, 2, 3";
        public const string FirstHalf = "7, 8, 12";
        public const string RunningBall = "1, 3";
        public const string Parlay = "9, 29";
        public const string SpecialBet = "4, 5, 6, 10, 11, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 99";

        public static Dictionary<string, string> ListOfBettype()
        {
            var listOfBettype = new Dictionary<string, string>();

            listOfBettype.Add("Straight", Basketball.Straight);
            listOfBettype.Add("FirstHalf", Basketball.FirstHalf);
            listOfBettype.Add("RunningBall", Basketball.RunningBall);
            listOfBettype.Add("SpecialBet", Basketball.SpecialBet);
            listOfBettype.Add("Parlay", Basketball.Parlay);

            return listOfBettype;
        }
    }
}