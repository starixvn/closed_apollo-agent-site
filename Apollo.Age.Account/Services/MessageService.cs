// --------------------------------------------------------------------------
// <copyright file="MessageService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.4">Created</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Account.ViewModels;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Account.Criterias;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MessageService : QueryService
    {
        private const string SiteList = "3000,3300";      

        public IEnumerable<AnnouncementViewModel> GetAnnouncement(int agentId, DateTime lastPostDate, int totalMessage = 10, bool isTest = false)
        {
            return this.GetReportData<AnnouncementViewModel>(new GetCustomerMessageCriteria
                {
                    CustomerId = agentId,
                    SiteIds = SiteList,
                    LastPostTime = lastPostDate,
                    TotalMessage = totalMessage,
                    IsTest = isTest
                });
        }

        public bool UpdateReadMessage(int messsageId)
        {
            throw new NotImplementedException();
        }
    }
}
