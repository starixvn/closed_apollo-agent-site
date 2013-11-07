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
    using System.Diagnostics.CodeAnalysis;
    using Apollo.Age.Core;
    using Apollo.Age.Entities;
    using Apollo.Age.Resources;

    /// <summary>
    /// SINGBET sport service.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
    public class SingbetSportService
    {
        /// <summary>
        /// Get SINGBET sport list.
        /// </summary>
        /// <param name="isOther">Is other sport or not.</param>
        /// <returns>List of SINGBET sport.</returns>
        public static List<SingbetSport> GetSport(bool isOther)
        {
            var sportList = new List<SingbetSport>
            {
                new SingbetSport
                {
                    Id = SportGroup.All,
                    Name = Lang.all
                },
                new SingbetSport
                {
                    Id = SportGroup.Soccer,
                    Name = Lang.soccer
                },
                new SingbetSport
                {
                    Id = SportGroup.BasketballFootballRugy,
                    Name = Lang.lblBkNflRugby
                },
                new SingbetSport
                {
                    Id = SportGroup.Tennis,
                    Name = Lang.Tennis
                },
                new SingbetSport
                {
                    Id = SportGroup.VolleyballBadmintonTableTennis,
                    Name = Lang.lblVbBdmTt
                },
                new SingbetSport
                {
                    Id = SportGroup.BaseBall,
                    Name = Lang.Baseball
                },
                new SingbetSport
                {
                    Id = SportGroup.Others,
                    Name = isOther ? Lang.other : Lang.InterEvent
                },
                new SingbetSport
                {
                    Id = "-99",
                    Name = Lang.lblParlay
                }
            };

            if (isOther)
            {
                sportList.Add(new SingbetSport
                    {
                        Id = SportGroup.Outright,
                        Name = Lang.lblOutRight
                    });
            }

            return sportList;
        }
    }
}