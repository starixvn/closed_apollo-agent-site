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

        public ICache UserCacheManager
        {
            get 
            {
                return CacheService.CreateInstance(CacheType.MemCache);
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

        public List<SingbetBettype> GetBettype()
        {
            var bettypeList = new List<SingbetBettype>
                {
                    new SingbetBettype
                    {
                        Id = BettypeGroup.All,
                        Name = Lang.all
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.Handicap,
                        Name = Lang.lblHandicapFC
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.OddEven,
                        Name = Lang.lblOddEven
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.OverUnder,
                        Name = Lang.lblOverUnder
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.CorrectScore,
                        Name = Lang.correctscore
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FT1X2,
                        Name = Lang.lblFT1X2
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.TotalGoal,
                        Name = Lang.totalgoal
                    },
                     new SingbetBettype
                     {
                        Id = BettypeGroup.FirstGoalLastGoal,
                        Name = Lang.firstgoal + "/" + Lang.lastgoal
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.HTFT,
                        Name = Lang.htft
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.MixParlay,
                        Name = Lang.lblMixParlay
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.SystemParlay,
                        Name = Lang.lblSystemParlay
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.Outright,
                        Name = Lang.lblOutRight
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FirstHandicap,
                        Name = Lang.lbl1stHdp
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FirstOddEven,
                        Name = Lang.lbl1stOddEven
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FirstOverUnder,
                        Name = Lang.lbl1stOU
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.FirstHalf1X2,
                        Name = Lang.lbl1x2_1st
                    },
                    new SingbetBettype
                    {
                        Id = BettypeGroup.OtherBettype,
                        Name = Lang.Othertypes
                    },
                };

            return bettypeList;
        }

        public IEnumerable<Agent> AgentDownlinePath(int parentId, int agentId)
        {
            IEnumerable<Agent> models = GetData<Agent>(new AgentDownlinePathCriteria
            {
                ParentId = parentId,
                AgentId = agentId
            });

            return models;
        }
    }
}