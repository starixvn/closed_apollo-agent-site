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
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.NAP.Framework.Data;
    using Fanex.Logging;

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
            int userStatus = (int)CustomerStatus.Locked;
            try
            {
                if (criteria.IsValid())
                {
                    using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                    {
                        //IDataReader reader = objectDb.ExecuteReader(criteria.GetSpParams());

                        //if (reader.Read())
                        //{
                        //    return reader.ToInt("CurrentStatus");
                        //}
                        //else
                        //{
                        //    return (int)CustomerStatus.Locked;
                        //}                        
                        userStatus = Converter.ToInt(objectDb.ExecuteScalar(criteria.GetSpParams()));
                    }
                }
            }
            catch (Exception ex)
            {
               // LogService.Critical(ex.Message + "IN Repository", ex);
                throw ex;
            }

            return userStatus;
        }

        public bool HasPermissionOnUpdating(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var currentParams = criteria.GetSpParams();
                    objectDb.ExecuteNonQuery(currentParams);

                    return Converter.ToBool(currentParams["haspermission"]);
                }
            }
            return false;
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

        public bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria, ref int adminId)
        {
            try
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    //IDataReader reader = db.ExecuteReader(criteria.GetSpParams());
                    //if (reader.Read())
                    //{
                    //    AdminId = Convert.ToInt32(reader["UserId"]);
                    //}

                    adminId = Converter.ToInt(objectDb.ExecuteScalar(criteria.GetSpParams()));

                    return adminId != 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int WritelogInternalLogin(InternalLogLoginCriteria criteria)
        {
            try
            {
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    return db.ExecuteNonQuery(criteria.GetSpParams());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InternalCheckIn(InternalCheckinCriteria criteria)
        {
            try
            {
                var param = criteria.GetSpParams();
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(param);

                    return Convert.ToInt32(param["onuserid"]);
                }

               // return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public byte InternalReCheckIn(InternalReCheckinCriteria criteria)
        {
            try
            {
                var param = criteria.GetSpParams();
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(param);

                    return Convert.ToByte(param["iskickedout"]);
                }

                //return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int InternalLogout(InternalLogoutCriteria criteria)
        {
            try
            {
                var param = criteria.GetSpParams();
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(param);
                }

                return Convert.ToInt32(param["onuserid"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}