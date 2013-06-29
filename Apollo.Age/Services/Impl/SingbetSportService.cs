// --------------------------------------------------------------------------
// <copyright file="SingbetSportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.6.29">Created</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System.Collections.Generic;
    using Apollo.Age.Core;
    using Apollo.Age.Entities;
    using Apollo.Age.Resources;

    public class SingbetSportService
    {
        public static List<SingbetSport> GetSport(bool isOther)
        {
            var sportList = new List<SingbetSport>{
                new SingbetSport{
                    Id = WagerSports.All,
                    Name = Lang.all
                },
                new SingbetSport{
                    Id = WagerSports.Soccer,
                    Name = Lang.soccer
                },
                new SingbetSport{
                    Id = WagerSports.Basketball_Football_Rugy,
                    Name = Lang.lblBkNflRugby
                },
                new SingbetSport{
                    Id = WagerSports.Tennis,
                    Name = Lang.Tennis
                },
                new SingbetSport{
                    Id = WagerSports.Volleyball_Badminton_TableTennis,
                    Name = Lang.lblVbBdmTt
                },
                new SingbetSport{
                    Id = WagerSports.BaseBall,
                    Name = Lang.Baseball
                },
                new SingbetSport{
                    Id = WagerSports.Other,
                    Name = isOther ? Lang.other : Lang.InterEvent
                }
            };

            if (isOther)
            {
                var ouright = new SingbetSport
                {
                    Id = WagerSports.Outright,
                    Name = Lang.lblOutRight
                };
                sportList.Add(ouright);
            }

            return sportList;
        }
    }
}
