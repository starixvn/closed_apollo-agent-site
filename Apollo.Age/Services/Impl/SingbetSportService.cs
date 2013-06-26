using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apollo.Age.Entities;

namespace Apollo.Age.Services.Impl
{
    public class SingbetSportService
    {
        public static List<SingbetSport> GetSport()
        {
            var listSport = new List<SingbetSport>{
                new SingbetSport{
                    Id = "0",
                    Name = Apollo.Age.Resources.Lang.all
                },
                new SingbetSport{
                    Id = "1",
                    Name = Apollo.Age.Resources.Lang.soccer
                },
                new SingbetSport{
                    Id = "2",
                    Name = Apollo.Age.Resources.Lang.lblBkNflRugby
                },
                new SingbetSport{
                    Id = "3",
                    Name = Apollo.Age.Resources.Lang.Tennis
                },
                new SingbetSport{
                    Id = "3",
                    Name = Apollo.Age.Resources.Lang.lblVbBdmTt
                },
                new SingbetSport{
                    Id = "3",
                    Name = Apollo.Age.Resources.Lang.Baseball
                },
                new SingbetSport{
                    Id = "3",
                    Name = Apollo.Age.Resources.Lang.InterEvent
                },
                new SingbetSport{
                    Id = "3",
                    Name = Apollo.Age.Resources.Lang.lblOutRight
                }
            };

            return listSport;
        }
    }
}
