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
    using System.Collections;
    using System.Collections.Generic;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Models;
    using Apollo.Age.Repositories.Interface;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// This class for retrieve user.
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        /// <summary>
        /// Gets the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return a user.</returns>
        public override User Get(ICriteria criteria)
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
                        parameters: criteria.GetSpParams());

                return user;
            }
        }

        /// <summary>
        /// Gets the user status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return a user status.</returns>
        public int GetUserId(UserIdByUsernameCriteria criteria)
        {
            int userId = -1;

            if (criteria.IsValid())
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var currentParams = criteria.GetSpParams();
                    objectDb.ExecuteNonQuery(currentParams);

                    if(criteria.ErrCode == 0){
                        userId = criteria.IsAgent ? Converter.ToInt(currentParams["agentid"]) : Converter.ToInt(currentParams["custid"]);
                    }
                }
            }

            return userId;
        }

        /// <summary>
        /// Fetches the specified criteria.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of user.</returns>
        public override IEnumerable<User> Fetch(ICriteria criteria)
        {
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var userList = objectDb.Query<User, Agent, User>(
                        (u, ag) =>
                        {
                            u.Agent = ag;
                            return u;
                        },
                        splitOn: "AgentId",
                        parameterValues: criteria.GetSpParams());

                return userList;
            }
        }

        /// <summary>
        /// Gets the user status.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return a user status.</returns>
        public int GetUserStatus(ICriteria criteria)
        {
            int userStatus = (int)CustomerStatus.Locked;

            if (criteria.IsValid())
            {
                using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    userStatus = Converter.ToInt(objectDb.ExecuteScalar(criteria.GetSpParams()));
                }
            }

            return userStatus;
        }

        /// <summary>
        /// Determines whether [has permission on updating] [the specified criteria].
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        ///   <c>true</c> if [has permission on updating] [the specified criteria]; otherwise, <c>false</c>.
        /// </returns>
        public bool CheckPermission(ICriteria criteria)
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

        /// <summary>
        /// Delete action from command result.
        /// </summary>
        /// <param name="entity">Input a agent.</param>
        /// <returns>Return result.</returns>
        public ICommandResult Delete(User entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Insert action from command result.
        /// </summary>
        /// <param name="entity">Input a agent.</param>
        /// <returns>Return result.</returns>
        public ICommandResult Insert(User entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update action from command result.
        /// </summary>
        /// <param name="entity">Input a agent.</param>
        /// <returns>Return result.</returns>
        public ICommandResult Update(User entity)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Authenticates the nap user.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="adminId">The admin id.</param>
        /// <returns>Return true or false.</returns>
        public bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria, ref int adminId)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.ExecuteScalar(criteria.GetSpParams());
                adminId = Converter.ToInt(objectDb.ExecuteScalar(criteria.GetSpParams()));

                var outParams = objectDb.GetOuputParams();
                int errorCode = Converter.ToInt(outParams["errcode"]);

                return errorCode == 0;
            }
        }

        /// <summary>
        /// Write logs the internal login.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return a number.</returns>
        public int WritelogInternalLogin(InternalLogLoginCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }

        /// <summary>
        /// Internals the check in.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return a user id.</returns>
        public int InternalCheckIn(InternalCheckinCriteria criteria)
        {
            var param = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(param);

                return Convert.ToInt32(param["onuserid"]);
            }
        }

        /// <summary>
        /// Internals the re check in.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return true or false.</returns>
        public byte InternalReCheckIn(InternalReCheckinCriteria criteria)
        {
            var param = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(param);

                return Convert.ToByte(param["iskickedout"]);
            }
        }

        /// <summary>
        /// Internals the logout.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return a user id that customer is checked in.</returns>
        public int InternalLogout(InternalLogoutCriteria criteria)
        {
            var param = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(param);
            }

            return Convert.ToInt32(param["onuserid"]);
        }

        /// <summary>
        /// Check belong with up line.
        /// </summary>
        /// <param name="criteria">Input a criteria.</param>
        /// <returns>Return true or false.</returns>
        public bool CheckBelongToUpline(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    Hashtable param = criteria.GetSpParams();

                    objectDb.ExecuteNonQuery(param);

                    return Converter.ToBool(param["allowed"]);
                }
            }

            return false;
        }

        /// <summary>
        /// Gets all user policy.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of function policy.</returns>
        public IEnumerable<FunctionPolicy> GetAllUserPolicy(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    return objectDb.Query<FunctionPolicy, Function, FunctionGroup, FunctionPolicy>(
                        (fp, f, fg) => 
                        { 
                            fp.Function = f; 
                            f.FunctionGroup = fg; 
                            return fp; 
                        },
                        "FunctionId,FunctionGroupId",
                        criteria.GetSpParams());
                }
            }

            return null;
        }

        /// <summary>
        /// Assigns the user permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void AssignUserPermission(ref UserAssignFunctionCriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    Hashtable param = criteria.GetSpParams();

                    objectDb.ExecuteNonQuery(param);

                    criteria.ErrCode = (int)param["errcode"];
                    criteria.ErrMsg = (string)param["errmsg"];
                }
            }
        }

        /// <summary>
        /// Get time once SSO accessed.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Return time of user logged in.
        /// </returns>
        public DateTime SSOGetTime(SSOGetTimeCriteria criteria)
        {
            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    Hashtable param = criteria.GetSpParams();

                    objectDb.ExecuteNonQuery(param);

                    return (DateTime)param["time"];
                }
            }

            return DateTime.Now;
        }

        /// <summary>
        /// SSO the check in.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Return user Id.
        /// </returns>
        public int SSOCheckIn(SSOCheckInCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                Hashtable param = criteria.GetSpParams();

                db.ExecuteNonQuery(param);

                return Convert.ToInt32(param["onuserid"]);
            }
        }
    }
}