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
                    Id = SingbetConstants.All,
                    Name = Lang.all
                },
                new SingbetSport{
                    Id = SingbetConstants.Soccer,
                    Name = Lang.soccer
                },
                new SingbetSport{
                    Id = SingbetConstants.BK_NFL_RUGBY,
                    Name = Lang.lblBkNflRugby
                },
                new SingbetSport{
                    Id = SingbetConstants.Tennis,
                    Name = Lang.Tennis
                },
                new SingbetSport{
                    Id = SingbetConstants.VB_BDM_TT,
                    Name = Lang.lblVbBdmTt
                },
                new SingbetSport{
                    Id = SingbetConstants.BaseBall,
                    Name = Lang.Baseball
                },
                new SingbetSport{
                    Id = SingbetConstants.Other,
                    Name = isOther ? Lang.other : Lang.InterEvent
                },
                new SingbetSport{
                    Id = SingbetConstants.Outright,
                    Name = Lang.lblOutRight
                }
            };

            return sportList;
        }

        public static List<SingbetBettype> GetBettype()
        {
            var bettypeList = new List<SingbetBettype>{
                new SingbetBettype{
                    Id = SingbetConstants.All,
                    Name = Lang.all
                },
                new SingbetBettype{
                    Id = SingbetConstants.Handicap,
                    Name = Lang.lblHandicapFC
                },
                new SingbetBettype{
                    Id = SingbetConstants.Odd_Even,
                    Name = Lang.lblOddEven
                },
                new SingbetBettype{
                    Id = SingbetConstants.Over_Under,
                    Name = Lang.lblOverUnder
                },
                new SingbetBettype{
                    Id = SingbetConstants.Correct_Score,
                    Name = Lang.correctscore
                },
                new SingbetBettype{
                    Id = SingbetConstants.FT_1X2,
                    Name = Lang.lblFT1X2
                },
                new SingbetBettype{
                    Id = SingbetConstants.Total_Goal,
                    Name = Lang.totalgoal
                },
                 new SingbetBettype{
                    Id = SingbetConstants.First_Goal_Last_Goal,
                    Name = Lang.firstgoal + "/" + Lang.lastgoal
                },
                new SingbetBettype{
                    Id = SingbetConstants.HT_FT,
                    Name = Lang.htft
                },
                new SingbetBettype{
                    Id = SingbetConstants.Mix_Parlay,
                    Name = Lang.lblMixParlay
                },
                new SingbetBettype{
                    Id = SingbetConstants.System_Parlay,
                    Name = Lang.lblSystemParlay
                },
                new SingbetBettype{
                    Id = SingbetConstants.Bettype_Outright,
                    Name = Lang.lblOutRight
                },
                new SingbetBettype{
                    Id = SingbetConstants._1st_Handicap,
                    Name = Lang.lbl1stHdp
                },
                new SingbetBettype{
                    Id = SingbetConstants._1st_Odd_Even,
                    Name = Lang.lblOddEven_1st
                },
                new SingbetBettype{
                    Id = SingbetConstants._1st_Over_Under,
                    Name = Lang.lbl1stOU
                },
                new SingbetBettype{
                    Id = SingbetConstants.FirstHalf_1X2,
                    Name = Lang.lbl1x2_1st
                },
                new SingbetBettype{
                    Id = SingbetConstants.OtherBettype,
                    Name = Lang.Others
                },
            };

            return bettypeList;
        }
    }
}
