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
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Account.Repositories.Impl;
    using Apollo.Age.Account.Repositories.Interface;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Impl;
    using Fanex.Common.Helper;
    using Fanex.Common;

    public class AccountService : ServiceBase, IAccountService
    {
        private const string PATH_XML = "\\CustSettingEngine\\PTGrid.xml";
        private const string SPORT_TYPE_NODE_XML = "PTGrid/SportType";
        private const string BET_TYPE_NODE_XML = "BetTypeGroup/BetType";
        private const string ATTRIBUTE_SPORT_ID = "id";
        private const string ATTRIBUTE_BETTYPE_ID = "id";
        private const string ATTRIBUTE_SPORT_NAME = "name";
        private const string ATTRIBUTE_BETTYPE_NAME = "name";
        private const string ATTRIBUTE_SPORT_LIVE = "isLive";

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

        public List<Customer> GetCustomerSuggest(bool isMember, string userName, int agentId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");

            CustomerSearchCriteria criteria = new CustomerSearchCriteria()
            {
                AgentId = agentId,
                IsMember = isMember,
                UserName = userName
            };

            return _accountRepository.GetCustomerSuggest(criteria).ToList();
        }

        public CustomerInfoModel GetCustomerInfo(bool isMember, int custId, string userName, int agentId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");

            CustomerExistCriteria criteriaExist = new CustomerExistCriteria()
            {
                CurrentAgentId = agentId,
                IsMember = isMember,
                CustId = custId,
                UserName = userName
            };

            if (_accountRepository.IsExistValueSearch(criteriaExist))
            {
                AccountInfoCriteria criteria = new AccountInfoCriteria()
                {
                    CurrentAgentId = agentId,
                    IsMember = isMember,
                    CustId = custId,
                    UserName = userName
                };
                return _accountRepository.GetCustomerInfo(criteria);
            }
            else
            {
                return null;
            }
        }

        public List<Member> GetMemberDownline(int agentId, int pageIndex, int pageSize, ref int totalPage)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");

            MemberDownlineCriteria criteria = new MemberDownlineCriteria()
            {
                AgentId = agentId,
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalPages = totalPage
            };

            var memberList = _accountRepository.GetMemberDownline(ref criteria).ToList<Member>();
            totalPage = criteria.TotalPages;

            return memberList;
        }

        public List<Agent> GetAgentDownlinePath(int parentId, int agentId)
        {
            Check.Argument.IsNotNegative(parentId, "parentId");
            Check.Argument.IsNotNegative(agentId, "agentId");

            AgentDownlinePathCriteria criteria = new AgentDownlinePathCriteria()
            {
                ParentId = parentId,
                AgentId = agentId
            };
            return _agentRepository.Fetch(criteria).ToList();
        }

        public List<Agent> GetDirectDownline(int agentId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");

            return _agentRepository.Fetch(new AgentDownlineCriteria
            {
                AgentId = agentId
            }).ToList();
        }

        public List<AgentShareModel> GetAgentShare(int actorAgentId, int agentId, string userName, short sportType, short betType, bool isLive)
        {
            Check.Argument.IsNotNegative(actorAgentId, "actorAgentId");
            Check.Argument.IsNotNegativeOrZero(agentId, "agentId");
            Check.Argument.IsNotNegative(sportType, "sportType");
            Check.Argument.IsNotNegative(betType, "betType");

            var criteria = new AgentShareCriteria() { 
                ActorAgentId = actorAgentId,
                AgentId = agentId,
                Username = userName,
                SportType = sportType,
                BetType = betType,
                IsLive = isLive
            };

            return _accountRepository.GetAgentShare(criteria).ToList();
        }

        public List<MemberShareModel> GetMemberShare(int actorAgentId, int memberId, string userName, short sportType, short betType, bool isLive)
        {
            Check.Argument.IsNotNegative(actorAgentId, "actorAgentId");
            Check.Argument.IsNotNegativeOrZero(memberId, "memberId");
            Check.Argument.IsNotNegative(sportType, "sportType");
            Check.Argument.IsNotNegative(betType, "betType");

            var criteria = new MemberShareCriteria()
            {
                ActorAgentId = actorAgentId,
                CustId = memberId,
                Username = userName,
                SportType = sportType,
                BetType = betType,
                IsLive = isLive
            };

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

            xmlDoc.Load(System.AppDomain.CurrentDomain.BaseDirectory + PATH_XML);

            foreach (XmlNode sportTypeNode in xmlDoc.SelectNodes(@SPORT_TYPE_NODE_XML))
            {
                if (sportTypeNode.Attributes[ATTRIBUTE_SPORT_ID].Value != null 
                    && sportTypeNode.Attributes[ATTRIBUTE_SPORT_NAME].Value != null)
                {
                    var isLive = Converter.ToBool(sportTypeNode.Attributes[ATTRIBUTE_SPORT_LIVE].Value);
                    var sportId = Converter.To<byte>(sportTypeNode.Attributes[ATTRIBUTE_SPORT_ID].Value);

                    var sport = sportList.Where(t => t.Id == sportId).SingleOrDefault();

                    if (sport == null)
                    {
                        sport = new Sport()
                        {
                            Id = sportId,
                            Name = sportTypeNode.Attributes[ATTRIBUTE_SPORT_NAME].Value.ToString(),
                            BetTypeList = new List<BetType>()
                        };

                        sportList.Add(sport);
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
                                betType = new BetType()
                                {
                                    Id = Converter.ToInt(betTypeNode.Attributes[ATTRIBUTE_BETTYPE_ID].Value),
                                    Name = betTypeNode.Attributes[ATTRIBUTE_BETTYPE_NAME].Value.ToString(),
                                    IsLive = isLive,
                                    IsDead = false
                                };
                            }
                            else
                            {
                                //sport.BetTypeList.Where(t => t.Id == betTypeId).SingleOrDefault().IsDead = isLive;
                                betType.IsDead = isLive;
                                betType.IsLive = !isLive;

                                betType = new BetType()
                                {
                                    Id = Converter.ToInt(betTypeNode.Attributes[ATTRIBUTE_BETTYPE_ID].Value),
                                    Name = betTypeNode.Attributes[ATTRIBUTE_BETTYPE_NAME].Value.ToString(),
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

            return betTypeList;
        }
    }
}