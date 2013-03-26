/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Age.DTO
{
    using System.Xml.Serialization;

    public enum eHandGroupType : int
    {
        BringIn = 1,
        AddChips = 2,
        Commission = 3,
        LeaveTable = 4
    }

    public enum eHandType : int
    {
        Holdem = 1,
        ThirteenCards = 2
    }

    public enum eHoldemPocketStatus : int
    {
        None = 0,
        Running = 1,
        Waiting = 2,
        Sit_Out = 3
    }

    public enum eHoldemPlayType : int
    {
        Fold = 1,
        Bet = 2,
        Raise = 3,
        Call = 4,
        Check = 5,
        All_In = 6
    }

    public enum eThirteenCardsHandType : int
    {
        [XmlEnum("0")]
        None = 0,

        [XmlEnum("1")]
        HighCard = 1,

        [XmlEnum("2")]
        OnePair = 2,

        [XmlEnum("3")]
        TwoPairs = 3,

        [XmlEnum("4")]
        ThreeOfAKind = 4,

        [XmlEnum("5")]
        Straight = 5,

        [XmlEnum("6")]
        Flush = 6,

        [XmlEnum("7")]
        FullHouse = 7,

        [XmlEnum("8")]
        FourOfAKind = 8,

        [XmlEnum("9")]
        StraightFlush = 9
    }

    public enum eThirteenCardsSpecialHandType : int
    {
        [XmlEnum("0")]
        None = 0,

        [XmlEnum("1")]
        StraightOf13Cards = 1,

        [XmlEnum("2")]
        SixPairs = 2,

        [XmlEnum("3")]
        ThreeStraights = 3,

        [XmlEnum("4")]
        ThreeFlushes = 4
    }

    public enum eReportType : byte
    {
        Daily = 1,
        Monthly = 2,
        Sport = 3
    }
}