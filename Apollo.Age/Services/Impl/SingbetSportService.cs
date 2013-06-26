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
            var listSport = new List<SingbetSport>{
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
                    Id = SingbetConstants.Other,
                    Name = Apollo.Age.Resources.Lang.other
                },
                new SingbetSport{
                    Id = SingbetConstants.Outright,
                    Name = Apollo.Age.Resources.Lang.lblOutRight
                }
            };

            return listSport;
        }
    }
}
