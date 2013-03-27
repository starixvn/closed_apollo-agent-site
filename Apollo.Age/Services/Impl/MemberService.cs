// --------------------------------------------------------------------------
// <copyright file="MemberService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.26">Created</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Interface;
    using Fanex.NAP.Framework.Data;

    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _repository;

        public MemberService(IMemberRepository memberRepository = null)
        {
            _repository = memberRepository ?? new MemberRepository();
        }

        public List<Member> GetCustomerSearch(ICriteria criteria)
        {
            return _repository.Fetch(criteria).ToList();
        }

        public Member GetCustomerInfo(AccountInfoCriteria criteria)
        {
            CustomerExistCriteria criteriaExist = new CustomerExistCriteria
            {
                CurrentAgentId = criteria.CurrentAgentId,
                CustId = criteria.CustId,
                IsMember = criteria.IsMember,
                UserName = criteria.UserName
            };

            if (_repository.IsExistValueSearch(criteriaExist))
            {
                return _repository.Get(criteria);
            }
            else
            {
                return null;
            }
        }

        public List<Member> GetMemberDownline(ref MemberDownlineCriteria criteria)
        {
            return _repository.GetMemberDownline(ref criteria).ToList<Member>();
        }
    }
}
