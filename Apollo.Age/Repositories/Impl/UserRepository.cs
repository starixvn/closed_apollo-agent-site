// --------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
//   <change who="Anders.Le" date="2013.3.26">Apply generic repository</change>
// </history>

namespace Apollo.Age.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;
    using Fanex.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {       
        public override User Get(ICriteria criteria)
        {
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var user = objectDb.QueryEntity<User, Agent, User>(
                            (u, ag) => {
                                u.Agent = ag;
                                return u;
                            },
                            splitOn: "AgentId",
                            parameters: criteria.GetSpParams()
                        );

                    return user;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICommandResult Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Insert(User entity)
        {
            throw new NotImplementedException();
        }

        public ICommandResult Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
