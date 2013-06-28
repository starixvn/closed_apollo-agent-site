using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apollo.Age.Entities;
using Apollo.Age.Core;
using Apollo.Age.Resources;

namespace Apollo.Age.Services.Impl
{
    public class SingbetSportService
    {
        public static List<SingbetSport> GetSport(bool isOther)
        {
            var sportList = new List<SingbetSport>{
                new SingbetSport{
                    Id = SingbetSportConstant.All,
                    Name = Lang.all
                },
                new SingbetSport{
                    Id = SingbetSportConstant.Soccer,
                    Name = Lang.soccer
                },
                new SingbetSport{
                    Id = SingbetSportConstant.BK_NFL_RUGBY,
                    Name = Lang.lblBkNflRugby
                },
                new SingbetSport{
                    Id = SingbetSportConstant.Tennis,
                    Name = Lang.Tennis
                },
                new SingbetSport{
                    Id = SingbetSportConstant.VB_BDM_TT,
                    Name = Lang.lblVbBdmTt
                },
                new SingbetSport{
                    Id = SingbetSportConstant.BaseBall,
                    Name = Lang.Baseball
                },
                new SingbetSport{
                    Id = SingbetSportConstant.Other,
                    Name = isOther ? Lang.other : Lang.InterEvent
                }
            };

            if (isOther)
            {
                var ouright = new SingbetSport
                {
                    Id = SingbetSportConstant.Outright,
                    Name = Lang.lblOutRight
                };
                sportList.Add(ouright);
            }

            return sportList;
        }
               
        public static List<SingbetBettype> GetBettype()
        {
            var bettypeList = new List<SingbetBettype>{
                new SingbetBettype{
                    Id = SingbetSportConstant.All,
                    Name = Lang.all
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.Handicap,
                    Name = Lang.lblHandicapFC
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.Odd_Even,
                    Name = Lang.lblOddEven
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.Over_Under,
                    Name = Lang.lblOverUnder
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.Correct_Score,
                    Name = Lang.correctscore
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.FT_1X2,
                    Name = Lang.lblFT1X2
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.Total_Goal,
                    Name = Lang.totalgoal
                },
                 new SingbetBettype{
                    Id = SingbetSportConstant.First_Goal_Last_Goal,
                    Name = Lang.firstgoal + "/" + Lang.lastgoal
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.HT_FT,
                    Name = Lang.htft
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.Mix_Parlay,
                    Name = Lang.lblMixParlay
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.System_Parlay,
                    Name = Lang.lblSystemParlay
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.Bettype_Outright,
                    Name = Lang.lblOutRight
                },
                new SingbetBettype{
                    Id = SingbetSportConstant._1st_Handicap,
                    Name = Lang.lbl1stHdp
                },
                new SingbetBettype{
                    Id = SingbetSportConstant._1st_Odd_Even,
                    Name = Lang.lbl1stOddEven
                },
                new SingbetBettype{
                    Id = SingbetSportConstant._1st_Over_Under,
                    Name = Lang.lbl1stOU
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.FirstHalf_1X2,
                    Name = Lang.lbl1x2_1st
                },
                new SingbetBettype{
                    Id = SingbetSportConstant.OtherBettype,
                    Name = Lang.Othertypes
                },
            };

            return bettypeList;
        }
    }
}
