// --------------------------------------------------------------------------
// <copyright file="SportTypes.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.4">Created</change>
// </history>

namespace Apollo.Age.Enumerations
{
    /// <summary>
    /// Sport types.
    /// </summary>
    public enum SportTypes : int
    {
        /// <summary>
        /// The no sport.
        /// </summary>
        None = 0,

        /// <summary>
        /// The soccer sport.
        /// </summary>
        [StringValue("Soccer")]
        Soccer = 1,

        /// <summary>
        /// The basket ball.
        /// </summary>
        [StringValue("BasketBall")]
        BasketBall = 2,

        /// <summary>
        /// The foot ball.
        /// </summary>
        [StringValue("FootBall")]
        FootBall = 3,

        /// <summary>
        /// The ice hockey.
        /// </summary>
        [StringValue("IceHockey")]
        IceHockey = 4,

        /// <summary>
        /// The tennis.
        /// </summary>
        [StringValue("Tennis")]
        Tennis = 5,

        /// <summary>
        /// The volley ball.
        /// </summary>
        [StringValue("VolleyBall")]
        VolleyBall = 6,

        /// <summary>
        /// The snooker sport.
        /// </summary>
        [StringValue("Billard")]
        Billard = 7,

        /// <summary>
        /// The base ball.
        /// </summary>
        [StringValue("BaseBall")]
        BaseBall = 8,

        /// <summary>
        /// The badminton.
        /// </summary>
        [StringValue("Badminton")]
        Badminton = 9,

        /// <summary>
        /// The golf sport.
        /// </summary>
        [StringValue("Golf")]
        Golf = 10,

        /// <summary>
        /// The motorsports.
        /// </summary>
        [StringValue("Motorsports")]
        Motorsports = 11,

        /// <summary>
        /// The swimming.
        /// </summary>
        [StringValue("Swimming")]
        Swimming = 12,

        /// <summary>
        /// The politics.
        /// </summary>
        [StringValue("Politics")]
        Politics = 13,

        /// <summary>
        /// The water polo.
        /// </summary>
        [StringValue("WaterPolo")]
        WaterPolo = 14,

        /// <summary>
        /// The diving.
        /// </summary>
        [StringValue("Diving")]
        Diving = 15,

        /// <summary>
        /// The boxing.
        /// </summary>
        [StringValue("Boxing")]
        Boxing = 16,

        /// <summary>
        /// The archery.
        /// </summary>
        [StringValue("Archery")]
        Archery = 17,

        /// <summary>
        /// The table tennis.
        /// </summary>
        [StringValue("TableTennis")]
        TableTennis = 18,

        /// <summary>
        /// The weightlifting.
        /// </summary>
        [StringValue("Weightlifting")]
        Weightlifting = 19,

        /// <summary>
        /// The canoeing.
        /// </summary>
        [StringValue("Canoeing")]
        Canoeing = 20,

        /// <summary>
        /// The gymnastics.
        /// </summary>
        [StringValue("Gymnastics")]
        Gymnastics = 21,

        /// <summary>
        /// The athletics.
        /// </summary>
        [StringValue("Athletics")]
        Athletics = 22,

        /// <summary>
        /// The equestrian.
        /// </summary>
        [StringValue("Equestrian")]
        Equestrian = 23,

        /// <summary>
        /// The handball.
        /// </summary>
        [StringValue("Handball")]
        Handball = 24,

        /// <summary>
        /// The rugby.
        /// </summary>
        [StringValue("Rugby")]
        Rugby = 26,

        /// <summary>
        /// The cricket.
        /// </summary>
        [StringValue("Cricket")]
        Cricket = 27,

        /// <summary>
        /// The field hockey.
        /// </summary>
        [StringValue("FieldHockey")]
        FieldHockey = 28,

        /// <summary>
        /// The other sports.
        /// </summary>
        [StringValue("OtherSports")]
        OtherSports = 99,

        /// <summary>
        /// The horse racing.
        /// </summary>
        [StringValue("HorseRacing")]
        HorseRacing = 151,

        /// <summary>
        /// The greyhound.
        /// </summary>
        [StringValue("Greyhound")]
        Greyhound = 152,

        /// <summary>
        /// The harness.
        /// </summary>
        [StringValue("Harness")]
        Harness = 153,

        /// <summary>
        /// The finance.
        /// </summary>
        [StringValue("Finance")]
        Finance = 201,

        /// <summary>
        /// The casino.
        /// </summary>
        [StringValue("Casino")]
        Casino = 251,

        /// <summary>
        /// The squash.
        /// </summary>
        [StringValue("Squash")]
        Squash = 30,

        /// <summary>
        /// The winter.
        /// </summary>
        [StringValue("Winter")]
        Winter = 29,

        /// <summary>
        /// The entertainment.
        /// </summary>
        [StringValue("Entertainment")]
        Entertainment = 31,

        /// <summary>
        /// The netball.
        /// </summary>
        [StringValue("Netball")]
        Netball = 32,

        /// <summary>
        /// The cycling.
        /// </summary>
        [StringValue("Cycling")]
        Cycling = 33,

        /// <summary>
        /// The fencing.
        /// </summary>
        [StringValue("Fencing")]
        Fencing = 34,

        /// <summary>
        /// The judo sport.
        /// </summary>
        [StringValue("Judo")]
        Judo = 35,

        /// <summary>
        /// The modern pentathlon.
        /// </summary>
        [StringValue("ModernPentathlon")]
        ModernPentathlon = 36,

        /// <summary>
        /// The rowing.
        /// </summary>
        [StringValue("Rowing")]
        Rowing = 37,

        /// <summary>
        /// The sailing.
        /// </summary>
        [StringValue("Sailing")]
        Sailing = 38,

        /// <summary>
        /// The shooting.
        /// </summary>
        [StringValue("Shooting")]
        Shooting = 39,

        /// <summary>
        /// The taekwondo.
        /// </summary>
        [StringValue("Taekwondo")]
        Taekwondo = 40,

        /// <summary>
        /// The triathlon.
        /// </summary>
        [StringValue("Triathlon")]
        Triathlon = 41,

        /// <summary>
        /// The wrestling.
        /// </summary>
        [StringValue("Wrestling")]
        Wrestling = 42,

        /// <summary>
        /// The number game.
        /// </summary>
        [StringValue("NumberGame")]
        NumberGame = 161,

        /// <summary>
        /// The live casino.
        /// </summary>
        [StringValue("LiveCasino")]
        LiveCasino = 162,

        /// <summary>
        /// The virtual sports.
        /// </summary>
        [StringValue("VirtualSports")]
        VirtualSports = 180,

        /// <summary>
        /// The point to point.
        /// </summary>
        [StringValue("P2P")]
        P2P = 253,

        /// <summary>
        /// The others.
        /// </summary>
        [StringValue("Others")]
        Others = 1000
    }
}