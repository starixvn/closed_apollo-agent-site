namespace Apollo.Age.Wagers.Services
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Apollo.Age.Wagers.Core;
    using Wagers.Entities;

    public static class MenuService
    {
        public static List<WagerSport> GenerateSportMenu()
        {
            var sportList = new List<WagerSport>{
                new WagerSport{
                    Id = WagersSporttypes.SC,
                    Name = Lang.lblSC,
                    Url = "~/wagers/Soccer"
                },
                new WagerSport{
                    Id =  WagersSporttypes.BK_NFL_Rugby,
                    Name = Lang.lblBkNflRugby,
                    Url = "~/wagers/BkNFLRugby"
                },
                new WagerSport{
                    Id = WagersSporttypes.TN,
                    Name = Lang.lblTN,
                    Url = "~/wagers/Tennis"
                },
                new WagerSport{
                    Id = WagersSporttypes.VB,
                    Name = Lang.lblVbBdmTt,
                    Url = "~/wagers/VolleyballBadminton"
                },
                new WagerSport{
                    Id = WagersSporttypes.BS,
                    Name = Lang.lblBs,
                    Url = "~/wagers/BaseBall"
                },
                new WagerSport{
                    Id =WagersSporttypes.Other,
                    Name = Lang.other,
                    Url = "~/wagers/Other"
                },
            };

            return sportList;
        }

        public static List<WagerSport> GenerateSoccerBettypesMenu()
        {
            var BettypleList = new List<WagerSport>{
                new WagerSport{
                    Id = WagersBettypes.Straight,
                    Name = Lang.lblStraight,                    
                },
                new WagerSport{
                  Id = WagersBettypes.FirstHalf,
                    Name = Lang.lbl1sthalf,    
                },
                new WagerSport{
                 Id = WagersBettypes.RB,
                    Name = Lang.lblRB,    
                },
                new WagerSport{
                Id = WagersBettypes.SpecialBet,
                    Name = Lang.lblSpecialbet,    
                },
                new WagerSport{
                   Id = WagersBettypes.CorrectScore,
                    Name = Lang.correctscore,    
                },
                new WagerSport{
                  Id = WagersBettypes.FirstHalfCorrectScore,
                    Name = Lang.lbl1stHalfCorrectScore,    
                },
                new WagerSport{
                  Id = WagersBettypes.Total,
                    Name = Lang.total,    
                },
                 new WagerSport{
                  Id = WagersBettypes.HalfFull,
                    Name = Lang.lblHalfFull,    
                },
                new WagerSport{
                  Id = WagersBettypes.Parlay,
                    Name = Lang.lblParlay,    
                },
            };

            return BettypleList;
        }

        public static List<WagerSport> Generate_Tennis_BettypesMenu()
        {
            var BettypleList = new List<WagerSport>{
                new WagerSport{
                    Id = WagersBettypes.Straight,
                    Name = Lang.lblStraight,                    
                },               
                new WagerSport{
                 Id = WagersBettypes.RB,
                    Name = Lang.lblRB,    
                },               
               new WagerSport{
                   Id = WagersBettypes.CorrectScore,
                    Name = Lang.correctscore,    
                },                 
                new WagerSport{
                  Id = WagersBettypes.Parlay,
                    Name = Lang.lblParlay,    
                },
            };

            return BettypleList;
        }

        public static List<WagerSport> Generate_Volleyball_BettypesMenu()
        {
            var BettypleList = new List<WagerSport>{
                new WagerSport{
                    Id = WagersBettypes.Straight,
                    Name = Lang.lblStraight,                    
                },               
                new WagerSport{
                 Id = WagersBettypes.RB,
                    Name = Lang.lblRB,    
                },               
               new WagerSport{
                   Id = WagersBettypes.CorrectScore,
                    Name = Lang.correctscore,    
                },                 
                new WagerSport{
                  Id = WagersBettypes.Parlay,
                    Name = Lang.lblParlay,    
                },
            };

            return BettypleList;
        }

        public static List<WagerSport> Generate_Baseball_BettypesMenu()
        {
            var BettypleList = new List<WagerSport>{
                new WagerSport{
                    Id = WagersBettypes.Straight,
                    Name = Lang.lblStraight,                    
                },       
                 new WagerSport{
                  Id = WagersBettypes.FirstHalf,
                    Name = Lang.lbl1sthalf,    
                },
                new WagerSport{
                 Id = WagersBettypes.RB,
                    Name = Lang.lblRB,    
                },               
               new WagerSport{
                   Id = WagersBettypes.CorrectScore,
                    Name = Lang.correctscore,    
                },  
                new WagerSport{
                  Id = WagersBettypes.Total,
                    Name = Lang.total,    
                },
                new WagerSport{
                  Id = WagersBettypes.Parlay,
                    Name = Lang.lblParlay,    
                },
            };

            return BettypleList;
        }

        public static List<WagerSport> Generate_BK_NFL_Rugby_BettypesMenu()
        {
            var BettypleList = new List<WagerSport>{
                new WagerSport{
                    Id = WagersBettypes.Straight,
                    Name = Lang.lblStraight,                    
                },               
                new WagerSport{
                 Id = WagersBettypes.RB,
                    Name = Lang.lblRB,    
                },               
                new WagerSport{
                  Id = WagersBettypes.Total,
                    Name = Lang.total,    
                },
                 new WagerSport{
                  Id = WagersBettypes.HalfFull,
                    Name = Lang.lblHalfFull,    
                },
                new WagerSport{
                  Id = WagersBettypes.Parlay,
                    Name = Lang.lblParlay,    
                },
            };

            return BettypleList;
        }

        public static List<WagerSport> Generate_Other_BettypesMenu()
        {
            var BettypleList = new List<WagerSport>{
                new WagerSport{
                    Id = WagersBettypes.Straight,
                    Name = Lang.lblStraight,                    
                },    
                 new WagerSport{
                  Id = WagersBettypes.FirstHalf,
                    Name = Lang.lbl1sthalf,    
                },
                new WagerSport{
                 Id = WagersBettypes.RB,
                    Name = Lang.lblRB,    
                },       
                 new WagerSport{
                   Id = WagersBettypes.CorrectScore,
                    Name = Lang.correctscore,    
                },  
                 new WagerSport{
                   Id = WagersBettypes.FirstHalfCorrectScore,
                    Name = Lang.lbl1stHalfCorrectScore,    
                },  
                new WagerSport{
                  Id = WagersBettypes.HalfFull,
                    Name = Lang.lblHalfFull,    
                },
                 new WagerSport{
                  Id = WagersBettypes.Total,
                    Name = Lang.total,    
                },
                new WagerSport{
                  Id = WagersBettypes.Parlay,
                    Name = Lang.lblParlay,    
                },
            };

            return BettypleList;
        }

    }
}