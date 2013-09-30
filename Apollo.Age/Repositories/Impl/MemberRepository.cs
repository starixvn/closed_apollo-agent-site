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

    /// <summary>
    /// This class for retrieve member.
    /// </summary>
    public class MemberRepository : RepositoryBase<Member>, IMemberRepository
    {
        /// <summary>
        /// Query member from criteria.
        /// </summary>
        /// <param name="criteria">Input a criteria.</param>
        /// <returns>Return list of member.</returns>
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

        /// <summary>
        /// Delete action from command result.
        /// </summary>
        /// <param name="entity">Input a agent.</param>
        /// <returns>Return result.</returns>
        public ICommandResult Delete(Member entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert action from command result.
        /// </summary>
        /// <param name="entity">Input a agent.</param>
        /// <returns>Return result.</returns>
        public ICommandResult Insert(Member entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update action from command result.
        /// </summary>
        /// <param name="entity">Input a agent.</param>
        /// <returns>Return result.</returns>
        public ICommandResult Update(Member entity)
        {
            throw new NotImplementedException();
        }     
    }
}
