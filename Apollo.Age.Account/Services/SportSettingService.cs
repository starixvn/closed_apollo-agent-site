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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using Apollo.Age.Account.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Services.Impl;
    using Fanex.Common;

    /// <summary>
    /// Sport setting service.
    /// </summary>
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

        #endregion const for read XML PTGrid

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

        /// <summary>
        /// Get all sport type from customer setting xml file.
        /// </summary>
        /// <returns>Sport setting list.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public List<SportSetting> GetAllSportType()
        {
            var sportList = new List<SportSetting>();
            var xmlDoc = new XmlDocument();

            xmlDoc.Load(HttpContext.Current.Server.MapPath(_PATH_XML));

            foreach (XmlNode sportTypeNode in xmlDoc.SelectNodes(_SPORT_TYPE_NODE_XML))
            {
                if (sportTypeNode.Attributes[_ATTRIBUTE_SPORT_ID].Value != null
                    && sportTypeNode.Attributes[_ATTRIBUTE_SPORT_NAME].Value != null)
                {
                    var live = Converter.ToBool(sportTypeNode.Attributes[_ATTRIBUTE_SPORT_LIVE].Value);
                    var sportId = Converter.To<short>(sportTypeNode.Attributes[_ATTRIBUTE_SPORT_ID].Value);

                    var sport = sportList.Where(t => t.Id == sportId).SingleOrDefault();

                    if (sport == null)
                    {
                        sport = new SportSetting()
                        {
                            Id = sportId,
                            IsMixSportParlay = false,
                            Name = sportTypeNode.Attributes[_ATTRIBUTE_SPORT_NAME].Value,
                            ResourceKey = sportTypeNode.Attributes[_ATTRIBUTE_RESOURCE_KEY].Value,
                            BetTypeList = new List<BetTypeSetting>()
                        };

                        sportList.Add(sport);
                    }
                    else
                    {
                        // process for sport id=99 & name=Mix Sports Parlay
                        var sportName = sportTypeNode.Attributes[_ATTRIBUTE_SPORT_NAME].Value;
                        if (sportId == (short)SportTypes.OtherSports && sportName == _MIX_SPORT_PARLAY)
                        {
                            sport = new SportSetting()
                            {
                                Id = sportId,
                                IsMixSportParlay = true,
                                Name = sportName,
                                ResourceKey = sportTypeNode.Attributes[_ATTRIBUTE_RESOURCE_KEY].Value,
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
                            int betTypeId = Converter.ToInt(betTypeNode.Attributes[_ATTRIBUTE_BETTYPE_ID].Value);
                            BetTypeSetting betType = sport.BetTypeList.Where(t => t.Id == betTypeId).SingleOrDefault();

                            if (betType == null)
                            {
                                betType = new BetTypeSetting()
                                {
                                    Id = betTypeId,
                                    Name = betTypeNode.Attributes[_ATTRIBUTE_BETTYPE_NAME].Value,
                                    ResourceKey = betTypeNode.Attributes[_ATTRIBUTE_RESOURCE_KEY].Value,
                                    IsLive = live,
                                    IsDead = false
                                };
                            }
                            else
                            {
                                betType.IsDead = live;
                                betType.IsLive = !live;

                                betType = new BetTypeSetting()
                                {
                                    Id = betTypeId,
                                    Name = betTypeNode.Attributes[_ATTRIBUTE_BETTYPE_NAME].Value,
                                    ResourceKey = betTypeNode.Attributes[_ATTRIBUTE_RESOURCE_KEY].Value,
                                    IsLive = live,
                                    IsDead = !live
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

        /// <summary>
        /// Get bet type by sport.
        /// </summary>
        /// <param name="sportId">Input sport id.</param>
        /// <param name="mixSportParlay">Is Mix Sport Parlay or no.</param>
        /// <returns>Bet type setting list.</returns>
        public List<BetTypeSetting> GetBetTypeBySport(short sportId, bool mixSportParlay)
        {
            var betTypeList = new List<BetTypeSetting>();

            if (SportList == null)
            {
                SportList = GetAllSportType();
            }

            SportSetting sport = SportList.Where(t => (t.Id == sportId && t.IsMixSportParlay == mixSportParlay)).SingleOrDefault();

            if (sport != null)
            {
                betTypeList = sport.BetTypeList;
            }

            return betTypeList.OrderByDescending(t => t.IsLive).ToList();
        }
    }
}