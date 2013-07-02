// --------------------------------------------------------------------------
// <copyright file="ReportService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.9">Add movebetday and balanceupday</change>
// </history>

namespace Apollo.Age.Reports.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Entities;
    using Apollo.Age.Reports.Core;
    using Apollo.Age.Reports.Criterias;
    using Apollo.Age.Resources;
    using Apollo.Age.Services.Impl;
    using Fanex.Caching;

    public class ReportService : QueryService
    {
        private const string BalanceUpdayKey = "Apollo.Age.Reports.Services.SpecialDays";

        protected ICache CacheManager
        {
            get
            {
                return CacheService.CreateInstance(CacheType.MemoryCache);
            }
        }

        private dynamic SpecialDays
        {
            get
            {
                dynamic specialDays = CacheManager.Get<dynamic>(BalanceUpdayKey);
                if (specialDays == null)
                {
                    specialDays = this.GetData<dynamic>(new GetSpecialDaysCriteria()).Single();
                    CacheManager.Set<dynamic>(BalanceUpdayKey, specialDays, DateTime.Now.AddMinutes(5));
                }

                return specialDays;
            }
        }

        public DateTime GetBalanceUpDay()
        {
            return SpecialDays.BalanceUpDay;
        }

        public DateTime MoveBetsDay()
        {
            return SpecialDays.MoveBetsDay;
        }

		private static List<SingbetBettype> SingbetBettypeList { get; set; }

        public List<SingbetBettype> GetBettype()
        {
			if (SingbetBettypeList == null)
			{
				var bettypeList = new List<SingbetBettype>{
					new SingbetBettype{
						Id = SingbetBettypeConstant.All,
						Name = Lang.all
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.Handicap,
						Name = Lang.lblHandicapFC
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.Odd_Even,
						Name = Lang.lblOddEven
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.Over_Under,
						Name = Lang.lblOverUnder
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.Correct_Score,
						Name = Lang.correctscore
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.FT_1X2,
						Name = Lang.lblFT1X2
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.Total_Goal,
						Name = Lang.totalgoal
					},
					 new SingbetBettype{
						Id = SingbetBettypeConstant.First_Goal_Last_Goal,
						Name = Lang.firstgoal + "/" + Lang.lastgoal
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.HT_FT,
						Name = Lang.htft
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.Mix_Parlay,
						Name = Lang.lblMixParlay
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.System_Parlay,
						Name = Lang.lblSystemParlay
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.Bettype_Outright,
						Name = Lang.lblOutRight
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant._1st_Handicap,
						Name = Lang.lbl1stHdp
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant._1st_Odd_Even,
						Name = Lang.lbl1stOddEven
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant._1st_Over_Under,
						Name = Lang.lbl1stOU
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.FirstHalf_1X2,
						Name = Lang.lbl1x2_1st
					},
					new SingbetBettype{
						Id = SingbetBettypeConstant.OtherBettype,
						Name = Lang.Othertypes
					},
				};

				SingbetBettypeList = bettypeList;
			}

            return SingbetBettypeList;
        }
    }
}
