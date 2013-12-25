// --------------------------------------------------------------------------
// <copyright file="IMessageService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders" date="2013.4.7">Created</change>
//   <change who="Edward.Nguyen" date="2013.4.9">Implement</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System;
    using System.Collections.Generic;
    using Apollo.Age.Account.Models;
    using Fanex.NAP.Framework.Data;

    public interface IMessageService
    {
        IEnumerable<AnnouncementModel> GetAnnouncement(
            int agentId,
            string siteIds,
            DateTime? startDate,
            DateTime endDate,
            short? agentLevel,
            int totalMessage = 10,
            bool isTest = false,
            string content = "",
            string language = "en-US");

        bool GetHasPrivateMessage24(int userId, bool isTest = false, bool isAgent = true);

        bool UpdateReadPrivateMessage(int userId, string username);

        MessageModel GetTopPrivateMessage24(int userId, int latestMessageId = 0, bool isTest = false, bool isAgent = true);

        MessageModel GetTopPublicMessage(int messageType, int siteId, bool isTest = false);

        MessageLevelModel GetTopPublicMessageLevel(int siteId, short agentLevel, bool isTest = false);

        IEnumerable<MessageModel> GetMessage(ICriteria criteria);
    }
}