namespace Apollo.Age.Wagers.Services
{
    using System.Collections.Generic;
    using Apollo.Age.Resources;
    using Apollo.Age.Wagers.Core;
    using Wagers.Entities;

    public static class SportMenuService
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
                    Url = "wagers/BkNFLRugby"
                },
                new WagerSport{
                    Id = WagersSporttypes.TN,
                    Name = Lang.lblTN,
                    Url = "wagers/Tennis"
                },
                new WagerSport{
                    Id = WagersSporttypes.VB,
                    Name = Lang.lblVbBdmTt,
                    Url = "wagers/VolleyballBadminton"
                },
                new WagerSport{
                    Id = WagersSporttypes.BS,
                    Name = Lang.lblBs,
                    Url = "wagers/BaseBall"
                },
                new WagerSport{
                    Id =WagersSporttypes.Other,
                    Name = Lang.other,
                    Url = "wagers/Other"
                },
            };

            return sportList;
        }
    }
}