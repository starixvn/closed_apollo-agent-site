// --------------------------------------------------------------------------
// <copyright file="IUserRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.3.18">Created</change>
//   <change who="Lewis.Le" date="2013.5.3">Added AuthenticateNapUser</change>
// </history>

namespace Apollo.Age.Repositories.Interface
{
    using Apollo.Age.Criterias;
    using Apollo.Age.Entities;
    using Fanex.NAP.Framework.Data;

    public interface IUserRepository : IRepository<User>
    {
        byte InternalReCheckIn(InternalReCheckinCriteria criteria);
        int GetUserStatus(ICriteria criteria);        
        int WritelogInternalLogin(InternalLogLoginCriteria criteria);
        int InternalCheckIn(InternalCheckinCriteria criteria);
        int InternalLogout(InternalLogoutCriteria criteria);       
        bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria, ref int adminId);
        bool CheckPermission(ICriteria criteria);
        bool CheckBelongToUpline(ICriteria criteria);
    }
}