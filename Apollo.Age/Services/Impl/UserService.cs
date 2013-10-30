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
    using Fanex.Logging;

    public class UserService : QueryService, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository = null)
        {
            _userRepository = userRepository ?? new UserRepository();
        }

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
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public User GetUserByUsername(string username)
        {
            User user = null;
            Check.Argument.IsNotNull(username, "username");

            try
            {
                user = _userRepository.Get(new UserGetByUsernameCriteria
                     {
                         Username = username
                     });
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return user;
        }

        public User GetUserParentInfo(int userId)
        {
            User user = null;
            Check.Argument.IsNotNegative(userId, "userId");

            try
            {
                user = _userRepository.Get(new UserGetUserParentInfoCriteria
                {
                    AgentUserId = userId
                });
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return user;
        }

        public IEnumerable<User> GetUserByAgent(int agentId)
        {
            Check.Argument.IsNotNegative(agentId, "agentId");

            try
            {
                return _userRepository.Fetch(new UserGetUserByAgentCriteria { AgentId = agentId });
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return null;
        }

        /// <summary>
        /// Validates a username and password against credentials stored in NAP database.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="adminId"></param>
        /// <returns></returns>
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
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return -1;
        }

        /// <summary>
        /// Determines whether [has permission on updating] [the specified actor agent id].
        /// </summary>
        /// <param name="actorAgentId">The actor agent id.</param>
        /// <param name="custId">The cust id.</param>
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
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return false;
        }

        /// <summary>
        /// Determines whether [has permission on updating] [the specified actor agent id].
        /// </summary>
        /// <param name="actorAgentId">The actor agent id.</param>
        /// <param name="custId">The cust id.</param>
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
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return false;
        }

        /// <summary>
        /// Writes the log internal login.
        /// </summary>
        /// <param name="agentId">The agent id.</param>
        /// <param name="agentUserId">The agent user id.</param>
        /// <param name="serverId">The server id.</param>
        /// <param name="actionId">The action id.</param>
        /// <param name="doing">The doing.</param>
        /// <param name="loginIp">The login ip.</param>
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
            try
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
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// Internals the get agent by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public User InternalGetAgentByUsername(string username)
        {
            User user = null;
            Check.Argument.IsNotNull(username, "username");

            try
            {
                user = _userRepository.Get(new InternalGetAgentByUsernameCriteria
                {
                    Username = username
                });
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return user;
        }

        /// <summary>
        /// Return OnUserId.
        /// </summary>
        /// <param name="adminId"> Admin user id.</param>
        /// <param name="netSessionId"> Session Id. </param>
        /// <param name="ip"> Requested IP</param>
        /// <returns><c>true</c> if check in successfull; otherwise, <c>false</c>.</returns>
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
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return 0;
        }

        /// <summary>
        /// Return IsKickOut
        /// </summary>
        /// <param name="onUserId"> Authenticated User Id.</param>
        /// <returns></returns>
        public bool InternalReCheckIn(int onUserId)
        {
            Check.Argument.IsNotNegativeOrZero(onUserId, "onuserid");
            try
            {
                var result = _userRepository.InternalReCheckIn(new InternalReCheckinCriteria
                {
                    OnUserId = onUserId,
                    IsKickedOut = 0
                });

                return Convert.ToBoolean(result);
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return false;
        }

        /// <summary>
        /// Return OnUserId.
        /// </summary>
        /// <param name="onUserId">User id of current loginned user.</param>
        /// <returns>
        /// Result of logins.
        /// </returns>
        public int InternalLogout(int onUserId)
        {
            Check.Argument.IsNotNegativeOrZero(onUserId, "onuserid");
            try
            {
                var result = _userRepository.InternalLogout(new InternalLogoutCriteria
                {
                    OnUserId = onUserId
                });

                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return 0;
        }

        public bool AllowLineAccountSSO(string username)
        {
            bool isAllowed = false;

            try
            {
                isAllowed = _userRepository.CheckBelongToUpline(new CheckBelongToUplinesCriteria
                {
                    Username = username,
                    Allowed = false
                });
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            return isAllowed;
        }

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
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return 0;
        }

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
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }

        public void AssignUserPermission(ref UserAssignFunctionCriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                _userRepository.AssignUerPermision(ref criteria);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

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

        public Dictionary<string, byte> ConvertToDictionary(IEnumerable<FunctionPolicy> listUserPermission)
        {
            var userPermission = new Dictionary<string, byte>();

            foreach (var item in listUserPermission)
            {
                userPermission.Add(item.Function.FunctionKey, item.Policy);
            }

            return userPermission;
        }
    }
}