// --------------------------------------------------------------------------
// <copyright file="AccountService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.4.8">Created</change>
//   <change who="Lewis.Le" date="2013.4.18">Added GetCommissionToMember, GetCommissionToAgent</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Apollo.Age.Account.Core;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Repositories.Impl;
    using Apollo.Age.Account.Repositories.Interface;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Impl;
    using Fanex.Common;
    using Fanex.Common.Helper;

    public class AccountService : ServiceBase, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAgentRepository _agentRepository;

        public static List<Sport> SportList = null;
        public AccountService()
        {
            _accountRepository = _accountRepository ?? new AccountRepository();
            _agentRepository = _agentRepository ?? new AgentRepository();
        }

        public IEnumerable<AgentListModel> GetAgentList(ref AgentListCriteria criteria)
        {
            return _accountRepository.GetAgentList(ref criteria);
        }

        public IEnumerable<MemberListModel> GetCustomerList(ref CustomerListCriteria criteria)
        {
            return _accountRepository.GetMemberList(ref criteria);
        }

        public List<Agent> GetAgentByUsername(GetAgentByUsernameCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");

            return _accountRepository.GetAgentByUsername(criteria).ToList();
        }

        public List<Member> GetMemberByUsername(GetMemberByUsernameCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");

            return _accountRepository.GetMemberByUsername(criteria).ToList();
        }

        public CustomerInfoModel GetAgentInfo(GetAgentInfoCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _accountRepository.GetCustomerInfo(criteria);
        }

        public CustomerInfoModel GetMemberInfo(GetMemberInfoCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "actorAgentId");

            return _accountRepository.GetCustomerInfo(criteria);
        }

        public List<Member> GetMemberDownline(ref MemberDownlineCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.AgentId, "agentId");

            var memberList = _accountRepository.GetMemberDownline(ref criteria).ToList<Member>();

            return memberList;
        }

        public List<Agent> GetAgentDownlinePath(AgentDownlinePathCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ParentId, "parentId");
            Check.Argument.IsNotNegative(criteria.AgentId, "agentId");

            return _agentRepository.Fetch(criteria).ToList();
        }

        public List<Agent> GetDirectDownline(AgentDownlineCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.AgentId, "agentId");

            return _agentRepository.Fetch(criteria).ToList();
        }

        public List<AgentShareModel> GetAgentShare(AgentShareCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");
            Check.Argument.IsNotNegative(criteria.SportType, "sportType");
            Check.Argument.IsNotNegative(criteria.BetType, "betType");

            return _accountRepository.GetAgentShare(criteria).ToList();
        }

        public List<MemberShareModel> GetMemberShare(MemberShareCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");
            Check.Argument.IsNotNegative(criteria.SportType, "sportType");
            Check.Argument.IsNotNegative(criteria.BetType, "betType");

            return _accountRepository.GetMemberShare(criteria).ToList();
        }

        public IEnumerable<CustomerCommissionModel> GetCommissionToMember(GetCommissionToMemberCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _accountRepository.GetCommissionToMember(criteria);
        }

        public IEnumerable<CustomerCommissionModel> GetCommissionToAgent(GetCommissionToAgentCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _accountRepository.GetCommissionToAgent(criteria);
        }

        public List<Sport> GetAllSportType()
        {
            var sportList = new List<Sport>();
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + Constants.PATH_XML);

            foreach (XmlNode sportTypeNode in xmlDoc.SelectNodes(@Constants.SPORT_TYPE_NODE_XML))
            {
                if (sportTypeNode.Attributes[Constants.ATTRIBUTE_SPORT_ID].Value != null
                    && sportTypeNode.Attributes[Constants.ATTRIBUTE_SPORT_NAME].Value != null)
                {
                    var isLive = Converter.ToBool(sportTypeNode.Attributes[Constants.ATTRIBUTE_SPORT_LIVE].Value);
                    var sportId = Converter.ToInt(sportTypeNode.Attributes[Constants.ATTRIBUTE_SPORT_ID].Value);

                    var sport = sportList.Where(t => t.Id == sportId).SingleOrDefault();

                    if (sport == null)
                    {
                        sport = new Sport()
                        {
                            Id = sportId,
                            Name = sportTypeNode.Attributes[Constants.ATTRIBUTE_SPORT_NAME].Value.ToString(),
                            ResourceKey = sportTypeNode.Attributes[Constants.ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
                            BetTypeList = new List<BetType>()
                        };

                        sportList.Add(sport);
                    }
                    else // process for sport id=99: Mix Sports Parlay
                    {
                        var sportName = sportTypeNode.Attributes[Constants.ATTRIBUTE_SPORT_NAME].Value.ToString();
                        if (sportId == 99 && sportName == Constants.MIX_SPORT_PARLAY)
                        {
                            sport = new Sport()
                            {
                                Id = -sportId,
                                Name = sportName,
                                ResourceKey = sportTypeNode.Attributes[Constants.ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
                                BetTypeList = new List<BetType>()
                            };

                            sportList.Add(sport);
                        }
                    }

                    // bet type
                    foreach (XmlNode betTypeNode in sportTypeNode.SelectNodes(@Constants.BET_TYPE_NODE_XML))
                    {
                        if (betTypeNode.Attributes[Constants.ATTRIBUTE_BETTYPE_ID].Value != null
                            && betTypeNode.Attributes[Constants.ATTRIBUTE_BETTYPE_NAME].Value != null)
                        {
                            var betTypeId = Converter.ToInt(betTypeNode.Attributes[Constants.ATTRIBUTE_BETTYPE_ID].Value);
                            var betType = sport.BetTypeList.Where(t => t.Id == betTypeId).SingleOrDefault();

                            if (betType == null)
                            {
                                betType = new BetType()
                                {
                                    Id = Converter.ToInt(betTypeNode.Attributes[Constants.ATTRIBUTE_BETTYPE_ID].Value),
                                    Name = betTypeNode.Attributes[Constants.ATTRIBUTE_BETTYPE_NAME].Value.ToString(),
                                    ResourceKey = betTypeNode.Attributes[Constants.ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
                                    IsLive = isLive,
                                    IsDead = false
                                };
                            }
                            else
                            {
                                betType.IsDead = isLive;
                                betType.IsLive = !isLive;

                                betType = new BetType()
                                {
                                    Id = Converter.ToInt(betTypeNode.Attributes[Constants.ATTRIBUTE_BETTYPE_ID].Value),
                                    Name = betTypeNode.Attributes[Constants.ATTRIBUTE_BETTYPE_NAME].Value.ToString(),
                                    ResourceKey = betTypeNode.Attributes[Constants.ATTRIBUTE_RESOURCE_KEY].Value.ToString(),
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

        public List<BetType> GetBetTypeBySport(short sportId)
        {
            var betTypeList = new List<BetType>();

            if(SportList == null)
            {
                GetAllSportType();
            }

            var sport = SportList.Where(t => t.Id == sportId).SingleOrDefault();

            if (sport != null)
            {
                betTypeList = sport.BetTypeList;
            }

            return betTypeList.OrderByDescending(t=>t.IsLive).ToList();
        }
    }
}