// --------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="???" date="???">Created</change>
//   <change who="Lewis.Le" date="2013.5.3">Added AuthenticateNapUser</change>
//   <change who="Ricky.Nguyen" date="2013.5.10">Added IsKickOut, HasPermissionOnUpdating</change>
//   <change who="Terry.Nguyen" date="2013.8.5">Added AllowLineAccountSSO for checking sso in one line</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Models;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Interface;
    using Fanex.Common;
    using Fanex.Common.Helper;

    /// <summary>
    /// User service.
    /// </summary>
    public class UserService : QueryService, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository = null)
        {
            _userRepository = userRepository ?? new UserRepository();
        }

        /// <summary>
        /// Get user by ID.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>User object.</returns>
        public User GetUserById(int userId)
        {
            Check.Argument.IsNotNegative(userId, "userId");

            try
            {
                var criteria = new UserGetByIdCriteria
                {
                    AgentUserId = userId
                };

                return _userRepository.Get(criteria);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>User object.</returns>
        public User GetUserByUsername(string username)
        {
            Check.Argument.IsNotNull(username, "username");

            User user = _userRepository.Get(new UserGetByUsernameCriteria
            {
                Username = username
            });

            return user;
        }

        /// <summary>
        /// Get user parent information.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>User object.</returns>
        public User GetUserParentInfo(int userId)
        {
            Check.Argument.IsNotNegative(userId, "userId");

            User user = _userRepository.Get(new UserGetUserParentInfoCriteria
            {
                AgentUserId = userId
            });

            return user;
        }

        /// <summary>
        /// Get user by agent.
        /// </summary>
        /// <param name="agentId">The agent ID.</param>
        /// <returns>List of user.</returns>
        public IEnumerable<User> GetUserByAgent(int agentId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");

            return _userRepository.Fetch(new UserGetUserByAgentCriteria { AgentId = agentId });
        }

        /// <summary>
        /// Validates a username and password against credentials stored in NAP database.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="adminId">The admin ID.</param>
        /// <returns>True : success, false : fail.</returns>
        public bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria, ref int adminId)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            criteria.Password = Cryptography.CfsEnCode(criteria.Password);

            return _userRepository.AuthenticateNapUser(criteria, ref adminId);
        }

        /// <summary>
        /// Gets the user status.
        /// </summary>
        /// <param name="agentId">The agent id.</param>
        /// <param name="agentUserId">The agent user id.</param>
        /// <returns> User status.</returns>
        public int GetUserStatus(int agentId, int agentUserId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");
            Check.Argument.IsNotNegative(agentUserId, "agentUserId");

            try
            {
                var currentStatus = _userRepository.GetUserStatus(new AgentGetStatusCriteria
                {
                    AgentId = agentId,
                    AgentUserId = agentUserId
                });

                return currentStatus;
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Determines whether [has permission on updating] [the specified actor agent id].
        /// </summary>
        /// <param name="actorAgentId">The actor agent id.</param>
        /// <param name="custId">The customer id.</param>
        /// <param name="customerType">Type of the customer.</param>
        /// <returns>
        ///   <c>true</c> if [has permission on updating] [the specified actor agent id]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPermissionOnUpdating(int actorAgentId, int custId, CustomerType customerType)
        {
            try
            {
                return _userRepository.CheckPermission(new CheckDownlineBelongToAgentCriteria
                {
                    ActorAgentId = actorAgentId,
                    ObjectId = custId,
                    ObjectTypeId = (int)customerType
                });
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether [has permission on updating] [the specified actor agent id].
        /// </summary>
        /// <param name="actorAgentId">The actor agent id.</param>
        /// <param name="custId">The customer id.</param>
        /// <param name="customerType">Type of the customer.</param>
        /// <returns>
        ///   <c>true</c> if [has permission on updating] [the specified actor agent id]; otherwise, <c>false</c>.
        /// </returns>
        public bool CheckDownlineBelongToAgent(int actorAgentId, int custId, CustomerType customerType)
        {
            try
            {
                return _userRepository.CheckPermission(new CheckDownlineBelongToAgentCriteria
                {
                    ActorAgentId = actorAgentId,
                    ObjectId = custId,
                    ObjectTypeId = (int)customerType
                });
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes the log internal login.
        /// </summary>
        /// <param name="agentId">The agent id.</param>
        /// <param name="agentUserId">The agent user id.</param>
        /// <param name="serverId">The server id.</param>
        /// <param name="actionId">The action id.</param>
        /// <param name="doing">The doing.</param>
        /// <param name="loginIp">The login IP address.</param>
        /// <param name="domainName">Name of the domain.</param>
        /// <param name="browserName">Name of the browser.</param>
        /// <param name="platform">The platform.</param>
        /// <param name="countryCode">The country code.</param>
        public void WriteLogInternalLogin(
            int agentId,
            int agentUserId,
            int serverId,
            int actionId,
            string doing,
            string loginIp,
            string domainName,
            string browserName,
            string platform,
            string countryCode)
        {
            var result = _userRepository.WritelogInternalLogin(new InternalLogLoginCriteria
            {
                AgentId = agentId,
                AgentUserId = agentUserId,
                ServerId = serverId,
                ActionId = actionId,
                Doing = doing,
                LoginIp = loginIp,
                Platform = platform,
                CountryCode = countryCode,
                BrowserName = browserName,
                DomainName = domainName
            });
        }

        /// <summary>
        /// Internals the get agent by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>User object.</returns>
        public User InternalGetAgentByUsername(string username)
        {
            Check.Argument.IsNotNull(username, "username");

            User user = _userRepository.Get(new InternalGetAgentByUsernameCriteria
            {
                Username = username
            });

            return user;
        }

        /// <summary>
        /// Return OnUserId.
        /// </summary>
        /// <param name="adminId">Admin user id.</param>
        /// <param name="netSessionId">Session Id. </param>
        /// <param name="ip">Requested IP.</param>
        /// <returns><c>true</c> if check in successful; otherwise, <c>false</c>.</returns>
        public int InternalCheckIn(int adminId, string netSessionId, string ip)
        {
            Check.Argument.IsNotNegativeOrZero(adminId, "adminid");

            try
            {
                var result = _userRepository.InternalCheckIn(new InternalCheckinCriteria
                {
                    UserId = adminId,
                    OnUserId = 0,
                    NetSessionId = netSessionId,
                    Ip = ip
                });
                return result;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Return IsKickOut.
        /// </summary>
        /// <param name="onlineUserId">Authenticated User Id.</param>
        /// <returns>True : success, false: fail.</returns>
        public bool InternalReCheckIn(int onlineUserId)
        {
            Check.Argument.IsNotNegativeOrZero(onlineUserId, "onuserid");

            try
            {
                var result = _userRepository.InternalReCheckIn(new InternalReCheckinCriteria
                {
                    OnUserId = onlineUserId,
                    IsKickedOut = 0
                });

                return Convert.ToBoolean(result);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Return OnUserId.
        /// </summary>
        /// <param name="onlineUserId">User id of current login user.</param>
        /// <returns>
        /// Result of logins.
        /// </returns>
        public int InternalLogout(int onlineUserId)
        {
            Check.Argument.IsNotNegativeOrZero(onlineUserId, "onuserid");

            try
            {
                var result = _userRepository.InternalLogout(new InternalLogoutCriteria
                {
                    OnUserId = onlineUserId
                });

                return Convert.ToInt32(result);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Allow line account SSO.
        /// </summary>
        /// <param name="username">Login username.</param>
        /// <returns>True: allowed, false: not allowed.</returns>
        public bool AllowLineAccountSSO(string username)
        {
            bool isAllowed = false;

            isAllowed = _userRepository.CheckBelongToUpline(new CheckBelongToUplinesCriteria
            {
                Username = username,
                Allowed = false
            });

            return isAllowed;
        }

        /// <summary>
        /// Get user policy.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="functionKey">The function key.</param>
        /// <returns>User policy.</returns>
        public byte GetUserPolicy(int userId, string functionKey)
        {
            try
            {
                var criteria = new UserGetUserFunctionPolicyCriteria
                {
                    AgentUserId = userId,
                    FunctionKey = functionKey
                };

                return (byte)_userRepository.Count(criteria);
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Get all user policy.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="functionGroupId">The function group ID.</param>
        /// <param name="isActive">Is active or not.</param>
        /// <returns>List of policy.</returns>
        public IEnumerable<FunctionPolicy> GetAllUserPolicy(int userId, short functionGroupId = 0, bool? isActive = null)
        {
            try
            {
                var criteria = new UserGetAllUserPolicyCriteria
                {
                    AgentUserId = userId,
                    FunctionGroupId = functionGroupId,
                    IsActive = isActive
                };

                return _userRepository.GetAllUserPolicy(criteria);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Assign user permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        public void AssignUserPermission(ref UserAssignFunctionCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            _userRepository.AssignUserPermission(ref criteria);
        }

        /// <summary>
        /// Get all user permission.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <returns>Dictionary of permission.</returns>
        public Dictionary<string, byte> GetAllUserPermission(int userId)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");

            var userPermission = new Dictionary<string, byte>();
            var listFunctionPolicy = this.GetAllUserPolicy(userId: userId, isActive: true);

            foreach (var item in listFunctionPolicy)
            {
                userPermission.Add(item.Function.FunctionKey, item.Policy);
            }

            return userPermission;
        }

        /// <summary>
        /// Convert to dictionary.
        /// </summary>
        /// <param name="listUserPermission">List of user permission.</param>
        /// <returns>Dictionary of policy.</returns>
        public Dictionary<string, byte> ConvertToDictionary(IEnumerable<FunctionPolicy> listUserPermission)
        {
            var userPermission = new Dictionary<string, byte>();

            foreach (var item in listUserPermission)
            {
                userPermission.Add(item.Function.FunctionKey, item.Policy);
            }

            return userPermission;
        }

        /// <summary>
        /// SSO the get time.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return SSO time.</returns>
        public DateTime SSOGetTime(SSOGetTimeCriteria criteria)
        {
            return _userRepository.SSOGetTime(criteria);
        }

        /// <summary>
        /// SSO the check in.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Return on user Id.</returns>
        public int SSOCheckIn(SSOCheckInCriteria criteria)
        {
            return _userRepository.SSOCheckIn(criteria);
        }
    }
}