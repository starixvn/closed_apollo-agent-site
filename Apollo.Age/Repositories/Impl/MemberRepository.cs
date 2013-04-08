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
    using Apollo.Age.Entities;
    using Apollo.Age.Repositories.Interface;
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
