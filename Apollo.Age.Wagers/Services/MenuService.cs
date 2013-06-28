namespace Apollo.Age.Wagers.Services
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Apollo.Age.Wagers.Core;
    using Wagers.Entities;

    public static class MenuService
    {
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

        public static List<WagerSport> GenerateTennisBettypesMenu()
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

        public static List<WagerSport> GenerateVolleyballBettypesMenu()
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

        public static List<WagerSport> GenerateBaseballBettypesMenu()
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

        public static List<WagerSport> GenerateBKNFLRugbyBettypesMenu()
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

        public static List<WagerSport> GenerateOtherBettypesMenu()
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