// --------------------------------------------------------------------------
// <copyright file="UserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="???" date="???">Created</change>
//   <change who="Lewis.Le" date="2013.5.3">Added AuthenticateNapUser</change>
//   <change who="Ricky.Nguyen" date="2013.5.10">Added IsKickOut, HasPermissionOnUpdating</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Services.Interface;
    using Fanex.Common;
    using Fanex.Common.Helper;
    using Fanex.Logging;

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository = null)
        {
            _userRepository = userRepository ?? new UserRepository();
        }

        public User GetUserById(int userId)
        {
            Check.Argument.IsNotNegativeOrZero(userId, "userId");
            throw new NotImplementedException();
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

        public bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria,  ref int AdminId)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            criteria.Password = Cryptography.CfsEnCode(criteria.Password);

            return _userRepository.AuthenticateNapUser(criteria,ref AdminId);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="agentUserId"></param>
        /// <returns></returns>
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
        ///
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="agentUserId"></param>
        /// <param name="isMaster"></param>
        /// <returns></returns>
        public bool HasPermissionOnUpdating(int actorAgentId, int custId, CustomerType customerType)
        {
            try
            {
                return _userRepository.HasPermissionOnUpdating(new CheckUpdatePermissionCriteria
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

       
        public void WriteLogInternalLogin(int agentId, int agentUserId, int serverId, int actionId, string doing, string loginIp, string domainName, string browserName, string platform, string countryCode)
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


        public int InternalCheckIn(int adminId, string netSessionId, string ip)
        {
            Check.Argument.IsNotNegativeOrZero(adminId, "adminid");
            try
            {
                var result = _userRepository.InternalCheckIn(new InternalCheckinCriteria { 
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

        public bool InternalReCheckIn(int onUserId)
        {
            Check.Argument.IsNotNegativeOrZero(onUserId, "adminid");
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

        public int InternalLogout(int onUserId)
        {
            Check.Argument.IsNotNegativeOrZero(onUserId, "adminid");
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
    }
}