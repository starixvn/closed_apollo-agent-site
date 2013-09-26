// --------------------------------------------------------------------------
// <copyright file="BetTypes.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.4">Created</change>
// </history>

namespace Apollo.Age.Enumerations
{
    /// <summary>
    /// Hard code bet types.
    /// </summary>
    public enum BetTypes : int
    {
        /// <summary>
        /// Hard code for none bet type.
        /// </summary>
        None = 0,

        /// <summary>
        /// Hard code for handicap bet type.
        /// </summary>
        Handicap = 1,

        /// <summary>
        /// Hard code for odd even.
        /// </summary>
        Odd_Even = 2,

        /// <summary>
        /// Hard code for over under.
        /// </summary>
        Over_Under = 3,

        /// <summary>
        /// Hard code for correct score.
        /// </summary>
        Correct_Score = 4,

        /// <summary>
        /// Hard code for full time 1 x 2.
        /// </summary>
        FT_1X2 = 5,

        /// <summary>
        /// Hard code for total goal.
        /// </summary>
        Total_Goal = 6,

        /// <summary>
        /// Hard code for first handicap.
        /// </summary>
        _1st_Handicap = 7,

        /// <summary>
        /// Hard code for first over under.
        /// </summary>
        _1st_Over_Under = 8,

        /// <summary>
        /// Hard code for mix parlay.
        /// </summary>
        MixParlay = 9,

        /// <summary>
        /// Hard code for outright.
        /// </summary>
        Outright = 10,

        /// <summary>
        /// Hard code for total corners.
        /// </summary>
        Total_Corners = 11,

        /// <summary>
        /// Hard code for first odd even.
        /// </summary>
        _1st_Odd_Even = 12,

        /// <summary>
        /// Hard code for clean sheet.
        /// </summary>
        Clean_Sheet = 13,

        /// <summary>
        /// Hard code for first goal last goal.
        /// </summary>
        First_Goal_Last_Goal = 14,

        /// <summary>
        /// Hard code for first half 1 x 2.
        /// </summary>
        FirstHalf_1X2 = 15,

        /// <summary>
        /// Hard code for haft time and full time.
        /// </summary>
        HT_FT = 16,

        /// <summary>
        /// Hard code for second handicap.
        /// </summary>
        _2nd_HDP = 17,

        /// <summary>
        /// HHard code for second over under.
        /// </summary>
        _2nd_Over_Under = 18,

        /// <summary>
        /// Hard code for substitutes.
        /// </summary>
        Substitutes = 19,

        /// <summary>
        /// Hard code for money line.
        /// </summary>
        Money_Line = 20,

        /// <summary>
        /// Hard code for first money line.
        /// </summary>
        _1st_Money_Line = 21,

        /// <summary>
        /// Hard code for next goal.
        /// </summary>
        Next_Goal = 22,

        /// <summary>
        /// Hard code for next corner.
        /// </summary>
        Next_Corner = 23,

        /// <summary>
        /// Hard code for double change.
        /// </summary>
        Double_Change = 24,

        /// <summary>
        /// Hard code for draw no bet.
        /// </summary>
        Draw_No_Bet = 25,

        /// <summary>
        /// Hard code for both one neither.
        /// </summary>
        Both_One_Neither = 26,

        /// <summary>
        /// Hard code for to win to NILL.
        /// </summary>
        To_Win_To_Nill = 27,

        /// <summary>
        /// Hard code for 3 way handicap.
        /// </summary>
        _3_Way_Handicap = 28,

        /// <summary>
        /// Hard code for combine mix parlay.
        /// </summary>
        Combine_Mix_Parlay = 29,

        /// <summary>
        /// Hard code for win.
        /// </summary>
        Win = 31,

        /// <summary>
        /// Hard code for place.
        /// </summary>
        Place = 32,

        /// <summary>
        /// Hard code for win and place.
        /// </summary>
        Win_Place = 33,

        /// <summary>
        /// Hard code for win UK tote.
        /// </summary>
        Win_UK_Tote = 41,

        /// <summary>
        /// Hard code for place UK tote.
        /// </summary>
        Place_UK_Tote = 42,

        /// <summary>
        /// Hard code for win and place UK tote.
        /// </summary>
        Win_Place_UK_Tote = 43,

        /// <summary>
        /// Hard code for 5 minutes over under.
        /// </summary>
        _5_min_O_U = 51,

        /// <summary>
        /// Hard code for 15 minutes over under.
        /// </summary>
        _15_min_O_U = 52,

        /// <summary>
        /// Hard code for 30 minutes over under.
        /// </summary>
        _30_min_O_U = 53,

        /// <summary>
        /// Hard code for 45 minutes over under.
        /// </summary>
        _45_min_O_U = 54,

        /// <summary>
        /// Hard code for 60 minutes over under.
        /// </summary>
        _60_min_O_U = 55,

        /// <summary>
        /// Hard code for end of day over under.
        /// </summary>
        End_of_day_O_U = 56,

        /// <summary>
        /// Hard code for 5 minutes odd even.
        /// </summary>
        _5_min_OE = 61,

        /// <summary>
        /// Hard code for 15 minutes odd even.
        /// </summary>
        _15_min_OE = 62,

        /// <summary>
        /// Hard code for 30 minutes odd even.
        /// </summary>
        _30_min_OE = 63,

        /// <summary>
        /// Hard code for 45 minutes odd even.
        /// </summary>
        _45_min_OE = 64,

        /// <summary>
        /// Hard code for 60 minutes odd even.
        /// </summary>
        _60_min_OE = 65,

        /// <summary>
        /// Hard code for end of day odd even.
        /// </summary>
        End_of_day_OE = 66,

        /// <summary>
        /// Hard code for casino games.
        /// </summary>
        Casino_Games = 71,

        /// <summary>
        /// Hard code for P2P games.
        /// </summary>
        P2P_Games = 72,

        /// <summary>
        /// Hard code for home no bet.
        /// </summary>
        Home_No_Bet = 121,

        /// <summary>
        /// Hard code for away no bet.
        /// </summary>
        Away_No_Bet = 122,

        /// <summary>
        /// Hard code for draw no draw.
        /// </summary>
        Draw_NoDraw = 123,

        /// <summary>
        /// Hard code for P2P.
        /// </summary>
        P2P = 1000,

        /// <summary>
        /// Hard code for first ball over under.
        /// </summary>
        _1st_Ball_OU = 81,

        /// <summary>
        /// Hard code for last ball over under.
        /// </summary>
        Last_Ball_OU = 82,

        /// <summary>
        /// Hard code for first ball odd even.
        /// </summary>
        _1st_Ball_OE = 83,

        /// <summary>
        /// Hard code for last ball odd even.
        /// </summary>
        Last_Ball_OE = 84,

        /// <summary>
        /// Hard code for N over under.
        /// </summary>
        N_Over_Under = 85,

        /// <summary>
        /// Hard code for N odd even.
        /// </summary>
        N_Odd_Even = 86,

        /// <summary>
        /// Hard code for next high low.
        /// </summary>
        Next_High_Low = 87,

        /// <summary>
        /// Hard code for warrior.
        /// </summary>
        Warrior = 88,

        /// <summary>
        /// Hard code for next combo.
        /// </summary>
        Next_Combo = 89,

        /// <summary>
        /// Hard code for number wheel.
        /// </summary>
        NumberWheel = 90,

        /// <summary>
        /// Hard code for baccarat.
        /// </summary>
        Baccarat = 1101,

        /// <summary>
        /// Hard code for roulette.
        /// </summary>
        Roulette = 1102,

        /// <summary>
        /// Hard code for SICBO.
        /// </summary>
        Sicbo = 1103,

        /// <summary>
        /// Hard code for Virtual Soccer handicap.
        /// </summary>
        VS_Handicap = 1201,

        /// <summary>
        /// Hard code for Virtual Soccer over under.
        /// </summary>
        VS_OverUnder = 1203,

        /// <summary>
        /// Hard code for Virtual Soccer correct score.
        /// </summary>
        VS_CorrectScore = 1204,

        /// <summary>
        /// Hard code for Virtual Soccer 1 x 2.
        /// </summary>
        VS_1X2 = 1205,

        /// <summary>
        /// Hard code for Virtual Soccer total goal.
        /// </summary>
        VS_TotalGoal = 1206,

        /// <summary>
        /// Hard code for Virtual Soccer double change.
        /// </summary>
        VS_DoubleChange = 1224,

        /// <summary>
        /// Hard code for Virtual Horses Racing win.
        /// </summary>
        VHR_Win = 1231,

        /// <summary>
        /// Hard code for Virtual Horses Racing place.
        /// </summary>
        VHR_Place = 1232,

        /// <summary>
        /// Hard code for Virtual Horses Racing win place.
        /// </summary>
        VHR_WinPlace = 1233,

        /// <summary>
        /// Hard code for Virtual Horses Racing forecast.
        /// </summary>
        VHR_Forecast = 1237,

        /// <summary>
        /// Hard code for Virtual Horses Racing TRICAST.
        /// </summary>
        VHR_Tricast = 1238,

        /// <summary>
        /// Hard code for Virtual Tennis player win.
        /// </summary>
        VT_PlayerWin = 1220,

        /// <summary>
        /// Hard code for Virtual Tennis score bet.
        /// </summary>
        VT_ScoreBet = 1235,

        /// <summary>
        /// Hard code for Virtual Tennis total points.
        /// </summary>
        VT_TotalPoints = 1236,

        /// <summary>
        /// Hard code for match winner.
        /// </summary>
        MatchWinner = 1301,

        /// <summary>
        /// Hard code for match correct score.
        /// </summary>
        MatchCorrectScore = 1302,

        /// <summary>
        /// Hard code for set handicap.
        /// </summary>
        SetHandicap = 1303,

        /// <summary>
        /// Hard code for total sets.
        /// </summary>
        TotalSets = 1304,

        /// <summary>
        /// Hard code for match total games odd even.
        /// </summary>
        MatchTotalGamesOddEven = 1305,

        /// <summary>
        /// Hard code for match total games 2 ways.
        /// </summary>
        MatchTotalGames2Way = 1306,

        /// <summary>
        /// Hard code for match total games  3 ways.
        /// </summary>
        MatchTotalGames3Way = 1307,

        /// <summary>
        /// Hard code for match handicap games 2 ways.
        /// </summary>
        MatchHandicapGames2Way = 1308,

        /// <summary>
        /// Hard code for match handicap games 3 ways.
        /// </summary>
        MatchHandicapGames3Way = 1309,

        /// <summary>
        /// Hard code for player x match total games 2 ways.
        /// </summary>
        PlayerXMatchTotalGames2Way = 1310,

        /// <summary>
        /// Hard code for set x winner.
        /// </summary>
        SetXWinner = 1311,

        /// <summary>
        /// Hard code for set x total games 2 ways.
        /// </summary>
        SetXTotalGames2Way = 1312,

        /// <summary>
        /// Hard code for set x total games 3 ways.
        /// </summary>
        SetXTotalGames3Way = 1313,

        /// <summary>
        /// Hard code for set x will there be a tiebreak.
        /// </summary>
        SetXWillThereBeATiebreak = 1314,

        /// <summary>
        /// Hard code for set x player to win on a tiebreak.
        /// </summary>
        SetXPlayerToWinOnATiebreak = 1315,

        /// <summary>
        /// Hard code for set x game handicap.
        /// </summary>
        SetXGameHandicap = 1316,

        /// <summary>
        /// Hard code for set x correct score.
        /// </summary>
        SetXCorrectScore = 1317,

        /// <summary>
        /// Hard code for set x total games odd even.
        /// </summary>
        SetXTotalGamesOddEven = 1318,

        /// <summary>
        /// Hard code for set x race to y games.
        /// </summary>
        SetXRaceToYGames = 1319,

        /// <summary>
        /// Hard code for set x score after 2 games.
        /// </summary>
        SetXScoreAfter2Games = 1320,

        /// <summary>
        /// Hard code for set x score after 4 games.
        /// </summary>
        SetXScoreAfter4Games = 1321,

        /// <summary>
        /// Hard code for set x score after 6 games.
        /// </summary>
        SetXScoreAfter6Games = 1322,

        /// <summary>
        /// Hard code for set x total deuce game 2 ways.
        /// </summary>
        SetXTotalDeuceGame2Way = 1323,

        /// <summary>
        /// Hard code for set x game y winner.
        /// </summary>
        SetXGameYWinner = 1324,

        /// <summary>
        /// Hard code for set x game y correct score.
        /// </summary>
        SetXGameYCorrectScore = 1325,

        /// <summary>
        /// Hard code for set x game y point handicap.
        /// </summary>
        SetXGameYPointHandicap = 1326,

        /// <summary>
        /// Hard code for set x game y deuce betting.
        /// </summary>
        SetXGameYDeuceBetting = 1327,

        /// <summary>
        /// Hard code for set x game y total points exact.
        /// </summary>
        SetXGameYTotalPointsExact = 1328,

        /// <summary>
        /// Hard code for set x game y total points 2 ways.
        /// </summary>
        SetXGameYTotalPoints2Way = 1329,

        /// <summary>
        /// Hard code for set x game y total points 3 ways.
        /// </summary>
        SetXGameYTotalPoints3Way = 1330,

        /// <summary>
        /// Hard code for set x game y total points odd even.
        /// </summary>
        SetXGameYTotalPointsOddEven = 1331,

        /// <summary>
        /// Hard code for set x game y total point z winner.
        /// </summary>
        SetXGameYTotalPointZWinner = 1332,

        /// <summary>
        /// Hard code for set x game y and game y add1 winner.
        /// </summary>
        SetXGameYAndGameYAdd1Winner = 1333,

        /// <summary>
        /// Hard code for set x game y and game y add 1 point.
        /// </summary>
        SetXGameYAndGameYAdd1Points = 1334,

        /// <summary>
        /// Hard code for others.
        /// Max 19000 for Betting.
        /// </summary>
        Others = 19001,

        /// <summary>
        /// Hard code for first correct score.
        /// Bet type from 19001 to 32767 for Transfer.
        /// </summary>
        FirstCorrectScore = 30
    }
}