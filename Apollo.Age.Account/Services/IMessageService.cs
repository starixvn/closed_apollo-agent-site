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

        bool UpdateReadMessage(int userId, int messsageId, bool isAgent = true);

        bool GetHasPrivateMessage24(int userId, bool isTest = false);

        int CountUnreadPrivateMessage(int userId, string username);

        bool UpdateReadPrivateMessage(int userId, string username);

        IEnumerable<MessageViewModel> GetTopPrivateMessage24(int userId, int latestMessageId = 0, bool isTest = false);

        IEnumerable<MessageViewModel> GetTopPublicMessage(int messageType, int siteId, int latestMessageId = 0, bool isTest = false);
    }
}
