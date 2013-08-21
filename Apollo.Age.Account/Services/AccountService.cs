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
    using System;

    public class AccountService : ServiceBase, IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAgentRepository _agentRepository;

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

        /// <summary>
        /// Get agent down line list by user name.
        /// </summary>
        /// <param name="criteria">Input GetAgentByUsernameCriteria.</param>
        /// <returns>Agent down line list.</returns>
        public List<Agent> GetAgentByUsername(GetAgentByUsernameCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");

            return _accountRepository.GetAgentByUsername(criteria).ToList();
        }

        /// <summary>
        /// Get member down line list by user name.
        /// </summary>
        /// <param name="criteria">Input GetMemberByUsernameCriteria.</param>
        /// <returns>Member down line list.</returns>
        public List<Member> GetMemberByUsername(GetMemberByUsernameCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");

            return _accountRepository.GetMemberByUsername(criteria).ToList();
        }

        /// <summary>
        /// Get full info of a agent.
        /// </summary>
        /// <param name="criteria">Input GetAgentAllInfoCriteria.</param>
        /// <returns>CustomerInfoModel (info of agent).</returns>
        public CustomerInfoModel GetAgentInfo(GetAgentAllInfoCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _accountRepository.GetCustomerInfo(criteria);
        }

        /// <summary>
        /// Check current agent is child direct agent.
        /// </summary>
        /// <param name="logginAgentId">Current Loggin agent.</param>
        /// <param name="checkingAgentId">Agent need to check.</param>
        /// <param name="isSupper">If supper, then full permission.</param>
        /// <returns>Checking agent is valid direct downline or not.</returns>
        public bool IsDirectAgent(int logginAgentId, int checkingAgentId, bool isSupper = false)
        {
            if (isSupper)
            {
                return isSupper;
            }

            try
            {
                var agentInfo = _accountRepository.GetCustomerInfo(new GetAgentInfoCriteria
                    {
                        AgentId = checkingAgentId
                    });
                if (agentInfo != null && agentInfo.ParentId == logginAgentId)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return false;
        }

        /// <summary>
        /// Get info of a member.
        /// </summary>
        /// <param name="criteria">Input GetMemberInfoCriteria.</param>
        /// <returns>CustomerInfoModel (info of member).</returns>
        public CustomerInfoModel GetMemberInfo(GetMemberInfoCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "actorAgentId");

            return _accountRepository.GetCustomerInfo(criteria);
        }

        /// <summary>
        /// Get member direct down line list.
        /// </summary>
        /// <param name="criteria">Input MemberDownLineCriteria.</param>
        /// <returns>Member list.</returns>
        public List<Member> GetMemberDownline(ref MemberDownlineCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.AgentId, "agentId");

            var memberList = _accountRepository.GetMemberDownline(ref criteria).ToList<Member>();

            return memberList;
        }

        /// <summary>
        /// Get agent down line path list.
        /// </summary>
        /// <param name="criteria">Input AgentDownLinePathCriteria.</param>
        /// <returns>Agent list.</returns>
        public List<Agent> GetAgentDownlinePath(AgentDownlinePathCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ParentId, "parentId");
            Check.Argument.IsNotNegative(criteria.AgentId, "agentId");

            return _agentRepository.Fetch(criteria).ToList();
        }

        /// <summary>
        /// Get agent direct down line list.
        /// </summary>
        /// <param name="criteria">Input AgentDownLineCriteria.</param>
        /// <returns>Agent list.</returns>
        public List<Agent> GetDirectDownline(AgentDownlineCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.AgentId, "agentId");

            return _agentRepository.Fetch(criteria).ToList();
        }

        /// <summary>
        /// Get agent share model list.
        /// </summary>
        /// <param name="criteria">Input AgentShareCriteria.</param>
        /// <returns>AgentShareModel list.</returns>
        public List<AgentShareModel> GetAgentShare(AgentShareCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");
            Check.Argument.IsNotNegative(criteria.SportType, "sportType");
            Check.Argument.IsNotNegative(criteria.BetType, "betType");

            return _accountRepository.GetAgentShare(criteria).ToList();
        }

        /// <summary>
        /// Get member share model list.
        /// </summary>
        /// <param name="criteria">Input MemberShareCriteria.</param>
        /// <returns>MemberShareCriteria list.</returns>
        public List<MemberShareModel> GetMemberShare(MemberShareCriteria criteria)
        {
            Check.Argument.IsNotNegative(criteria.ActorAgentId, "actorAgentId");
            Check.Argument.IsNotNegative(criteria.SportType, "sportType");
            Check.Argument.IsNotNegative(criteria.BetType, "betType");

            return _accountRepository.GetMemberShare(criteria).ToList();
        }

        /// <summary>
        /// Get Commission to member list.
        /// </summary>
        /// <param name="criteria">Input GetCommissionToMemberCriteria.</param>
        /// <returns>CustomerCommissionModel list.</returns>
        public IEnumerable<CustomerCommissionModel> GetCommissionToMember(GetCommissionToMemberCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _accountRepository.GetCommissionToMember(criteria);
        }

        /// <summary>
        /// Get Commission to agent list.
        /// </summary>
        /// <param name="criteria">Input GetCommissionToAgentCriteria.</param>
        /// <returns>CustomerCommissionModel list.</returns>
        public IEnumerable<CustomerCommissionModel> GetCommissionToAgent(GetCommissionToAgentCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _accountRepository.GetCommissionToAgent(criteria);
        }

        public IEnumerable<AgentLogLoginModel> GetAgentLogLogin(GetLoginLogCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            return _accountRepository.GetAgentLogLogin(criteria);
        }
    }
}