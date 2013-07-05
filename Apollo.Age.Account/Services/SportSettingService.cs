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
    using System.Web;
    using System.Xml;
    using Apollo.Age.Account.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Services.Impl;
    using Fanex.Common;

    public class SportSettingService : ServiceBase, ISportSettingService
    {
        #region const for read XML PTGrid
        private const string _PATH_XML = "\\CustSettingEngine\\PTGrid.xml";

        private const string _SPORT_TYPE_NODE_XML = "PTGrid/SportType";

        private const string _BET_TYPE_NODE_XML = "BetTypeGroup/BetType";

        private const string _ATTRIBUTE_SPORT_ID = "id";

        private const string _ATTRIBUTE_BETTYPE_ID = "id";

        private const string _ATTRIBUTE_SPORT_NAME = "name";

        private const string _ATTRIBUTE_BETTYPE_NAME = "name";

        private const string _ATTRIBUTE_SPORT_LIVE = "isLive";

        private const string _ATTRIBUTE_RESOURCE_KEY = "resourcekey";

        private const string _MIX_SPORT_PARLAY = "Mix Sports Parlay";
        #endregion

		private static List<SportSetting> _sportList = null;

		public static List<SportSetting> SportList
		{
			get 
			{ 
				return _sportList; 
			}

			set
			{
				_sportList = value;
			}
		}

        public List<SportSetting> GetAllSportType()
        {
            var sportList = new List<SportSetting>();
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(HttpContext.Current.Server.MapPath(_PATH_XML));

            foreach (XmlNode sportTypeNode in xmlDoc.SelectNodes(_SPORT_TYPE_NODE_XML))
            {
                if (sportTypeNode.Attributes[_ATTRIBUTE_SPORT_ID].Value != null
                    && sportTypeNode.Attributes[_ATTRIBUTE_SPORT_NAME].Value != null)
                {
                    var isLive = Converter.ToBool(sportTypeNode.Attributes[_ATTRIBUTE_SPORT_LIVE].Value);
                    var sportId = Converter.To<short>(sportTypeNode.Attributes[_ATTRIBUTE_SPORT_ID].Value);

                    var sport = sportList.Where(t => t.Id == sportId).SingleOrDefault();

                    if (sport == null)
                    {
                        sport = new SportSetting()
                        {
                            Id = sportId,
                            IsMixSportParlay = false,
                            Name = sportTypeNode.Attributes[_ATTRIBUTE_SPORT_NAME].Value.ToString(),
                            ResourceKey = sportTypeNode.Attributes[_ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
                            BetTypeList = new List<BetTypeSetting>()
                        };

                        sportList.Add(sport);
                    }
                    else 
                    {
						// process for sport id=99 & name=Mix Sports Parlay
                        var sportName = sportTypeNode.Attributes[_ATTRIBUTE_SPORT_NAME].Value.ToString();
                        if (sportId == (short)SportTypes.OtherSports && sportName == _MIX_SPORT_PARLAY)
                        {
                            sport = new SportSetting()
                            {
                                Id = sportId,
                                IsMixSportParlay = true,
                                Name = sportName,
                                ResourceKey = sportTypeNode.Attributes[_ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
                                BetTypeList = new List<BetTypeSetting>()
                            };

                            sportList.Add(sport);
                        }
                    }

                    // bet type
                    foreach (XmlNode betTypeNode in sportTypeNode.SelectNodes(@_BET_TYPE_NODE_XML))
                    {
                        if (betTypeNode.Attributes[_ATTRIBUTE_BETTYPE_ID].Value != null
                            && betTypeNode.Attributes[_ATTRIBUTE_BETTYPE_NAME].Value != null)
                        {
                            var betTypeId = Converter.ToInt(betTypeNode.Attributes[_ATTRIBUTE_BETTYPE_ID].Value);
                            var betType = sport.BetTypeList.Where(t => t.Id == betTypeId).SingleOrDefault();

                            if (betType == null)
                            {
                                betType = new BetTypeSetting()
                                {
                                    Id = betTypeId,
                                    Name = betTypeNode.Attributes[_ATTRIBUTE_BETTYPE_NAME].Value.ToString(),
                                    ResourceKey = betTypeNode.Attributes[_ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
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
                                    Name = betTypeNode.Attributes[_ATTRIBUTE_BETTYPE_NAME].Value.ToString(),
                                    ResourceKey = betTypeNode.Attributes[_ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
                                    IsLive = isLive,
                                    IsDead = !isLive
                                };
                            }

                            sport.BetTypeList.Add(betType);
                        }
                    }
                }
            }

            // SportList = sportList;
            return sportList;
        }

        public List<BetTypeSetting> GetBetTypeBySport(short sportId, bool isMixSportParlay)
        {
            var betTypeList = new List<BetTypeSetting>();

            if (SportList == null)
            {
                SportList = GetAllSportType();
            }

            var sport = SportList.Where(t => (t.Id == sportId && t.IsMixSportParlay == isMixSportParlay)).SingleOrDefault();

            if (sport != null)
            {
                betTypeList = sport.BetTypeList;
            }

            return betTypeList.OrderByDescending(t => t.IsLive).ToList();
        }
    }
}
