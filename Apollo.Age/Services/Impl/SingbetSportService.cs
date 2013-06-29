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
    }
}
