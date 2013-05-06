// --------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
//   <change who="Anders.Le" date="2013.3.26">Apply generic repository</change>
//   <change who="Lewis.Le" date="2013.5.3">Added AuthenticateNapUser</change>
// </history>

namespace Apollo.Age.Repositories.Impl
{
    using System;
    using System.Data;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Repositories.Interface;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;

    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public override User Get(ICriteria criteria)
        {
            try
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var user = objectDb.QueryEntity<User, Agent, User>(
                            (u, ag) =>
                            {
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

        public int GetUserStatus(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    IDataReader reader = objectDb.ExecuteReader(criteria.GetSpParams());
                    if (reader.Read())
                    {
                        return reader.ToInt("CurrentStatus");
                    }
                    else
                    {
                        return (int)CustomerStatus.Locked;
                    }
                }
            }
            return (int)CustomerStatus.Locked;
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

        public bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    IDataReader reader = db.ExecuteReader(criteria.GetSpParams());

                    return reader.Read();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}