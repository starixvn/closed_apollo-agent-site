// --------------------------------------------------------------------------
// <copyright file="IUserService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
//   <change who="Lewis.Le" date="2013.5.3">Added AuthenticateNapUser</change>
//   <change who="Ricky.Nguyen" date="2013.5.10">Added IsKickOut, HasPermissionOnUpdating</change>
// </history>

namespace Apollo.Age.Services.Interface
{
    using System.Collections.Generic;
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;
    using Apollo.Age.Models;

    public interface IUserService
    {
        User GetUserById(int userId);

        User GetUserByUsername(string username);

        User GetUserParentInfo(int userId);

        User InternalGetAgentByUsername(string username);

        IEnumerable<User> GetUserByAgent(int agentId);

        bool AllowLineAccountSSO(string username);

        bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria, ref int adminId);

        int GetUserStatus(int agentId, int agentUserId);

        bool HasPermissionOnUpdating(int actorAgentId, int custId, CustomerType customerType);

        bool CheckDownlineBelongToAgent(int actorAgentId, int custId, CustomerType customerType);

        void WriteLogInternalLogin(
            int agentId, 
            int agentUserId, 
            int serverId, 
            int actionId, 
            string doing, 
            string loginIp, 
            string domainName, 
            string browserName, 
            string platform, 
            string countryCode);

        /// <summary>
        /// Return OnUserId.
        /// </summary>
        /// <param name="adminId">The admin ID.</param>
        /// <param name="netSessionId">The net session ID.</param>
        /// <param name="ip">The IP address.</param>
        /// <returns></returns>
        int InternalCheckIn(int adminId, string netSessionId, string ip);

        /// <summary>
        /// Return IsKickOut.
        /// </summary>
        /// <param name="onlineUserId">Online user ID.</param>
        /// <returns></returns>
        bool InternalReCheckIn(int onlineUserId);

        /// <summary>
        /// Return OnUserId.
        /// </summary>
        /// <param name="onlineUserId">User id of current loginned user.</param>
        /// <returns>Result of login.</returns>
        int InternalLogout(int onlineUserId);

        /// <summary>
        /// Gets the user function policy.
        /// </summary>
        /// <param name="userId">The agent user identifier.</param>
        /// <param name="functionKey">The function key.</param>
        /// <returns>Policy of user on function.</returns>
        byte GetUserPolicy(int userId, string functionKey);

        IEnumerable<FunctionPolicy> GetAllUserPolicy(int userId, short functionGroupId = 0, bool? isActive = null);

        Dictionary<string, byte> GetAllUserPermission(int userId);

        Dictionary<string, byte> ConvertToDictionary(IEnumerable<FunctionPolicy> listUserPermission);

        /// <summary>
        /// Assigns the user permission.
        /// </summary>
        /// <param name="criteria">The criteria assign user permission.</param>   
        void AssignUserPermission(ref UserAssignFunctionCriteria criteria);
    }
}