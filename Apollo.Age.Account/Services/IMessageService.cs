// --------------------------------------------------------------------------
// <copyright file="IMessageService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.7">Created</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Account.ViewModels;

    public interface IMessageService
    {
        IEnumerable<AnnouncementViewModel> GetAnnouncement(int agentId, DateTime lastPostDate, int totalMessage = 10, bool isTest = false);
        bool UpdateReadMessage(int messsageId);
        int CountUnreadPrivateMessage(int userId, string username);
        bool UpdateReadPrivateMessage(int userId, string username);
        IEnumerable<dynamic> GetTopPrivateMessage(int userId, bool isTest = false, int latestMessageId = 0);
        
    }
}
