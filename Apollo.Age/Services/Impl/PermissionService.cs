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
    using Apollo.Age.Criterias.Accounts;
    using Apollo.Age.DTO.Accounts;
    using Apollo.Age.Repositories.Impl;
    using Apollo.Age.Repositories.Interface;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class PermissionService
    {
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
            try
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
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
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
            try
            {
                var currentStatus = _userRepository.GetUserStatus(new AgentGetStatusCriteria
                {
                    AgentId = agentId,
                    AgentUserId = agentUserId
                });

                if (isMaster == true && currentStatus == (int)AgentStatus.Enable)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }
            return false;
        }
    }
}