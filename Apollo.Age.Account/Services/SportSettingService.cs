// --------------------------------------------------------------------------
// <copyright file="SportSettingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.5.7">Created</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Apollo.Age.Account.Entities;
    using Apollo.Age.Services.Impl;
    using Fanex.Common;

    public class SportSettingService : ServiceBase, ISportSettingService
    {
        public static List<SportSetting> SportList = null;

        #region const for read XML PTGrid
        private const string PATH_XML = "\\CustSettingEngine\\PTGrid.xml";

        private const string SPORT_TYPE_NODE_XML = "PTGrid/SportType";

        private const string BET_TYPE_NODE_XML = "BetTypeGroup/BetType";

        private const string ATTRIBUTE_SPORT_ID = "id";

        private const string ATTRIBUTE_BETTYPE_ID = "id";

        private const string ATTRIBUTE_SPORT_NAME = "name";

        private const string ATTRIBUTE_BETTYPE_NAME = "name";

        private const string ATTRIBUTE_SPORT_LIVE = "isLive";

        private const string ATTRIBUTE_RESOURCE_KEY = "resourcekey";

        private const string MIX_SPORT_PARLAY = "Mix Sports Parlay";
        #endregion

        public List<SportSetting> GetAllSportType()
        {
            var sportList = new List<SportSetting>();
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + PATH_XML);

            foreach (XmlNode sportTypeNode in xmlDoc.SelectNodes(SPORT_TYPE_NODE_XML))
            {
                if (sportTypeNode.Attributes[ATTRIBUTE_SPORT_ID].Value != null
                    && sportTypeNode.Attributes[ATTRIBUTE_SPORT_NAME].Value != null)
                {
                    var isLive = Converter.ToBool(sportTypeNode.Attributes[ATTRIBUTE_SPORT_LIVE].Value);
                    var sportId = Converter.To<byte>(sportTypeNode.Attributes[ATTRIBUTE_SPORT_ID].Value);

                    var sport = sportList.Where(t => t.Id == sportId).SingleOrDefault();

                    if (sport == null)
                    {
                        sport = new SportSetting()
                        {
                            Id = sportId,
                            IsMixSportParlay = false,
                            Name = sportTypeNode.Attributes[ATTRIBUTE_SPORT_NAME].Value.ToString(),
                            ResourceKey = sportTypeNode.Attributes[ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
                            BetTypeList = new List<BetTypeSetting>()
                        };

                        sportList.Add(sport);
                    }
                    else // process for sport id=99 & name=Mix Sports Parlay
                    {
                        var sportName = sportTypeNode.Attributes[ATTRIBUTE_SPORT_NAME].Value.ToString();
                        if (sportId == 99 && sportName == MIX_SPORT_PARLAY)
                        {
                            sport = new SportSetting()
                            {
                                Id = sportId,
                                IsMixSportParlay = true,
                                Name = sportName,
                                ResourceKey = sportTypeNode.Attributes[ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
                                BetTypeList = new List<BetTypeSetting>()
                            };

                            sportList.Add(sport);
                        }
                    }

                    // bet type
                    foreach (XmlNode betTypeNode in sportTypeNode.SelectNodes(@BET_TYPE_NODE_XML))
                    {
                        if (betTypeNode.Attributes[ATTRIBUTE_BETTYPE_ID].Value != null
                            && betTypeNode.Attributes[ATTRIBUTE_BETTYPE_NAME].Value != null)
                        {
                            var betTypeId = Converter.ToInt(betTypeNode.Attributes[ATTRIBUTE_BETTYPE_ID].Value);
                            var betType = sport.BetTypeList.Where(t => t.Id == betTypeId).SingleOrDefault();

                            if (betType == null)
                            {
                                betType = new BetTypeSetting()
                                {
                                    Id = betTypeId,
                                    Name = betTypeNode.Attributes[ATTRIBUTE_BETTYPE_NAME].Value.ToString(),
                                    ResourceKey = betTypeNode.Attributes[ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
                                    IsLive = isLive,
                                    IsDead = false
                                };
                            }
                            else
                            {
                                betType.IsDead = isLive;
                                betType.IsLive = !isLive;

                                betType = new BetTypeSetting()
                                {
                                    Id = betTypeId,
                                    Name = betTypeNode.Attributes[ATTRIBUTE_BETTYPE_NAME].Value.ToString(),
                                    ResourceKey = betTypeNode.Attributes[ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
                                    IsLive = isLive,
                                    IsDead = !isLive
                                };
                            }

                            sport.BetTypeList.Add(betType);
                        }
                    }
                }
            }

            SportList = sportList;
            return sportList;
        }

        public List<BetTypeSetting> GetBetTypeBySport(short sportId, bool isMixSportParlay)
        {
            var betTypeList = new List<BetTypeSetting>();

            if (SportList == null)
            {
                GetAllSportType();
            }

            var sport = isMixSportParlay
                ? SportList.Where(t => (t.Id == sportId && t.Name == MIX_SPORT_PARLAY)).SingleOrDefault()
                : SportList.Where(t => (t.Id == sportId && t.Name != MIX_SPORT_PARLAY)).SingleOrDefault();

            if (sport != null)
            {
                betTypeList = sport.BetTypeList;
            }

            return betTypeList.OrderByDescending(t => t.IsLive).ToList();
        }
    }
}
