using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apollo.Age.Entities;
using Apollo.Age.Core;

namespace Apollo.Age.Services.Impl
{
    public class SingbetSportService
    {
        public static List<SingbetSport> GetSport()
        {
            var sportList = new List<SingbetSport>{
                new SingbetSport{
                    Id = SingbetConstants.All,
                    Name = Apollo.Age.Resources.Lang.all
                },
                new SingbetSport{
                    Id = SingbetConstants.Soccer,
                    Name = Apollo.Age.Resources.Lang.soccer
                },
                new SingbetSport{
                    Id = SingbetConstants.BK_NFL_RUGBY ,
                    Name = Apollo.Age.Resources.Lang.lblBkNflRugby
                },
                new SingbetSport{
                    Id = SingbetConstants.Tennis,
                    Name = Apollo.Age.Resources.Lang.Tennis
                },
                new SingbetSport{
                    Id = SingbetConstants.VB_BDM_TT,
                    Name = Apollo.Age.Resources.Lang.lblVbBdmTt
                },
                new SingbetSport{
                    Id = SingbetConstants.BaseBall,
                    Name = Apollo.Age.Resources.Lang.Baseball
                },
                new SingbetSport{
                    Id = SingbetConstants.InterEvent,
                    Name = Apollo.Age.Resources.Lang.InterEvent
                },
                 new SingbetSport{
                    Id = SingbetConstants.Other,
                    Name = Apollo.Age.Resources.Lang.other
                },
                new SingbetSport{
                    Id = SingbetConstants.Outright,
                    Name = Apollo.Age.Resources.Lang.lblOutRight
                }
            };

            return sportList;
        }

        public static List<SingbetBettype> GetBettype()
        {
            var bettypeList = new List<SingbetBettype>{
                new SingbetBettype{
                    Id = SingbetConstants.All,
                    Name = Apollo.Age.Resources.Lang.all
                },
                new SingbetBettype{
                    Id = SingbetConstants.Soccer,
                    Name = Apollo.Age.Resources.Lang.soccer
                },
                new SingbetBettype{
                    Id = SingbetConstants.BK_NFL_RUGBY ,
                    Name = Apollo.Age.Resources.Lang.lblBkNflRugby
                },
                new SingbetBettype{
                    Id = SingbetConstants.Tennis,
                    Name = Apollo.Age.Resources.Lang.Tennis
                },
                new SingbetBettype{
                    Id = SingbetConstants.VB_BDM_TT,
                    Name = Apollo.Age.Resources.Lang.lblVbBdmTt
                },
                new SingbetBettype{
                    Id = SingbetConstants.BaseBall,
                    Name = Apollo.Age.Resources.Lang.Baseball
                },
                new SingbetBettype{
                    Id = SingbetConstants.InterEvent,
                    Name = Apollo.Age.Resources.Lang.InterEvent
                },
                 new SingbetBettype{
                    Id = SingbetConstants.Other,
                    Name = Apollo.Age.Resources.Lang.other
                },
                new SingbetBettype{
                    Id = SingbetConstants.Outright,
                    Name = Apollo.Age.Resources.Lang.lblOutRight
                }
            };

            return bettypeList;
        }
    }
}
