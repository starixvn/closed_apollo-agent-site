// --------------------------------------------------------------------------
// <copyright file="PermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.29">Created</change>
// </history>


namespace Apollo.Age.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Repositories.Interface;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.DTO.Accounts;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class PermissionService
    {
        private const string AgentIdSessionKey = "Apollo-AgentId";
        private const string UserIdSessionKey = "Apollo-AgentUserId";

        private static IUserRepository _userRepository;
        static PermissionService()
        {
            _userRepository = new UserRepository();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="agentUserId"></param>
        /// <returns></returns>
        public static bool IsKickOut(int agentId, int agentUserId)
        {
            var currentStatus = _userRepository.GetUserStatus(new AgentGetStatusCriteria
            {
                AgentId = agentId,
                AgentUserId = agentUserId
            });

            if (currentStatus == (int)AgentStatus.Closed)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="agentUserId"></param>
        /// <param name="isMaster"></param>
        /// <returns></returns>
        public static bool HasPermissionOnUpdating(int agentId, int agentUserId, bool isMaster)
        {
            var currentStatus = _userRepository.GetUserStatus(new AgentGetStatusCriteria
            {
                AgentId = agentId,
                AgentUserId = agentUserId
            });

            if (isMaster == true)
            {
                if (currentStatus == (int)AgentStatus.Enable)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
