using System;

/// <summary>
/// All bettype denfinitions
/// </summary>
public enum BetType
{
    Handicap = 1,
    OddEven = 2,
    OverUnder = 3,
    CorrectScore = 4,
    FT1x2 = 5,
    TotalGoal = 6,
    _1stHandicap = 7,
    _1stOverUnder = 8,
    MixParlay = 9,
    Outright = 10,
    TotalCorners = 11,
    _1stOddEven = 12,
    CleanStreet = 13,
    FirstLastGoal = 14,
    _1st1x2 = 15,
    HTFT = 16,
    _2ndHDP = 17,
    _2ndOverUnder = 18,
    Substitutes = 19,
    MoneyLine = 20,
    NextGoal = 22,
    NextCorner = 23,
    SystemParlay= 29,
    Win = 31,
    Place = 32,
    WinPlace = 33,
    FO_Win = 41,
    FO_Place = 42,
    FO_WinPlace = 43,
    FI5MinOU=51,
    FI15MinOU=52,
    FI30MinOU=53,
    FI45MinOU=54,
    FI60MinOU=55,
    FIEndOfDayOU=56,
    FI5MinOE=61,
    FI15MinOE=62,
    FI30MinOE=63,
    FI45MinOE=64,
    FI60MinOE=65,
    FIEndOfDayOE=66,
    Casino = 71,
    P2P = 72,
    NG1stBallOU = 81,
    NGLastBallOU = 82,
    NG1stBallOE = 83,
    NGLastBallOE = 84,
    NGOUFT = 85,
    NGOEFT = 86,
    NGHighLow = 87,
}

/// <summary>
/// All sporttype denfinitions
/// </summary>
public enum SportType
{
    Soccer = 1,
    BasketBall = 2,
    FootBall = 3,
    IceHockey = 4,
    Tennis = 5,
    VolleyBall = 6,
    Billard = 7,
    BaseBall = 8,
    Badminton = 9,
    Golf = 10,
    Motorsports = 11,
    Swimming = 12,
    Politics = 13,
    WaterPolo = 14,
    Diving = 15,
    Boxing = 16,
    Archery = 17,
    TableTennis = 18,
    Weightlifting = 19,
    Canoeing = 20,
    Gymnastics = 21,
    Athletics = 22,
    Equestrian = 23,
    Handball = 24,
    Rugby = 26,
    Cricket = 27,
    FieldHockey = 28,
    OtherSports = 19999,
    HorseRacing = 151,
    Greyhound = 152,
    Harness = 153,
    Finance = 201,
    Casino = 251,
    Squash = 30,
    Winter = 29,
    Entertainment = 31,
    NumberGame = 161,
    NetBall = 32
}

/// <summary>
/// All Betlist status definitions
/// </summary>
public struct BetListStatus
{
    public const string Running = "running";
    public const string Waiting = "waiting";
    public const string Refund = "refund";
    public const string Void = "void";
    public const string Won = "won";
    public const string Lose = "lose";
    public const string Draw = "draw";
    public const string Reject = "reject";
}

public struct Language
{
    public static string English = "en-US";
    public static string Chinese_Traditional = "zh-TW";
    public static string Chinese_Simplified = "zh-CN";
    public static string Japanese = "ja-JP";
    public static string Thai = "th-TH";
    public static string Korean = "ko-KR";
}
