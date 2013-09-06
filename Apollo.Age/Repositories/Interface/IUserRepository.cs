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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        byte InternalReCheckIn(InternalReCheckinCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        int GetUserStatus(ICriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        int WritelogInternalLogin(InternalLogLoginCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        int InternalCheckIn(InternalCheckinCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        int InternalLogout(InternalLogoutCriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="adminId"></param>
        /// <returns></returns>
        bool AuthenticateNapUser(AuthenticateNapUserCriteria criteria, ref int adminId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        bool CheckPermission(ICriteria criteria);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        bool CheckBelongToUpline(ICriteria criteria);
    }
}