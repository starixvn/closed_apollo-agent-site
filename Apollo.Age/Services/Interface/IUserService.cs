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
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Apollo.Age.Enumerations;

    public interface IUserService
    {
        User GetUserById(int userId);
        
        User GetUserByUsername(string username);

        User AgentLoginByUsername(string username);

        /// <summary>
        /// Validates a username and password against credentials stored in NAP database
        /// </summary>
        bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria, ref int AdminId);

        bool IsKickOut(int agentId, int agentUserId);

        bool HasPermissionOnUpdating(int actorAgentId, int custId, CustomerType customerType);

        void WriteLogInternalLogin(int agentId, int agentUserId, int serverId, int actionId, string doing, string loginIp, string domainName, string browserName, string platform, string countryCode);
    }
}