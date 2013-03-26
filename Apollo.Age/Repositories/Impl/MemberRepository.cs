// --------------------------------------------------------------------------
// <copyright file="MemberRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.25">Created</change>
// </history>

namespace Apollo.Age.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Repositories.Interface;
    using Fanex.NAP.Framework.Data;
    using Apollo.Age.Entities;
    using Fanex.Data;
    using Apollo.Age.Criterias.Accounts;
    using Fanex.Common.Helper;

    public class MemberRepository : IMemberRepository
    {
        public Member Get(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.QueryEntity<Member>(criteria.GetSpParams());
                }
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Member> Fetch(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.Query<Member>(criteria.GetSpParams()).ToList<Member>();
                }
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Member> GetMemberDownline(ref MemberDownlineCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    using (var db = new ObjectDb(criteria.GetSettingKey()))
                    {
                        var param = criteria.GetSpParams();
                        var list = db.Query<Member>(param);

                        // get output parameter
                        criteria.TotalPages = Convert.ToInt32(param["totalpages"]);
                        return list;
                    }
                }

                return new List<Member>();
            }
            catch
            {
                throw;
            }
        }

        public ICommandResult Delete(Member entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(Member entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(Member entity)
        {
            throw new NotImplementedException();
        }

        public int Count(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Member> Fetch(ICriteria criteria, Action<Orderable<Member>> order)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Member> Fetch()
        {
            throw new NotImplementedException();
        }

        public Member Get(int id)
        {
            throw new NotImplementedException();
        }

        public int Length(dynamic paramaters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Member> Query(dynamic parameters)
        {
            throw new NotImplementedException();
        }

        public Member Single(dynamic parameters)
        {
            throw new NotImplementedException();
        }
    }
}
