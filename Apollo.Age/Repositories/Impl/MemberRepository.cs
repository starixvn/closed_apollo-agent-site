// --------------------------------------------------------------------------
// <copyright file="MemberRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Quinn.Vo" date="2013.3.25">Created</change>
//   <change who="Anders.Le" date="2013.3.26">Apply generic repository</change>
// </history>

namespace Apollo.Age.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Interface;
    using Fanex.Common.Helper;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    public class MemberRepository : RepositoryBase<Member>, IMemberRepository
    {
        public override Member Get(ICriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.QueryEntity<Member, Agent, Member>(
                        (f, w) =>
                        {
                            f.Agent = w;
                            return f;
                        },
                        "ParentId",
                        criteria.GetSpParams());
                }
            }
            catch
            {
                throw;
            }
        }

        public bool IsExistValueSearch(ICriteria criteria)
        {
            bool result = false;
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    result = (bool)db.ExecuteScalar(criteria.GetSpParams());
                }
            }
            catch
            {
                throw;
            }

            return result;
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
    }
}
