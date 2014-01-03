// --------------------------------------------------------------------------
// <copyright file="MessageService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.4.4">Created</change>
//   <change who="Edward.Nguyen" date="2013.4.9">Implement</change>
//   <change who="Anders.Le" date="2013.7.7">Add ctor</change>
// </history>

namespace Apollo.Age.Account.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Apollo.Age.Account.Criterias;
    using Apollo.Age.Account.Models;
    using Apollo.Age.Services.Impl;
    using Fanex.Caching;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Message service.
    /// </summary>
    public class MessageService : QueryService, IMessageService
    {
        private DynamicRepository _dynamicRepository;

        #region ctor

        public MessageService()
            : this(null)
        {
        }

        public MessageService(DynamicRepository dynamicRepository)
        {
            _dynamicRepository = _dynamicRepository ?? new DynamicRepository();
        }

        #endregion ctor

        /// <summary>
        /// Get announcement.
        /// </summary>
        /// <param name="agentId">The agent ID.</param>
        /// <param name="siteIds">List of site ID.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="agentLevel">The agent level.</param>
        /// <param name="totalMessage">Total message.</param>
        /// <param name="isTest">Is test or not.</param>
        /// <param name="content">The message content.</param>
        /// <param name="language">The message language.</param>
        /// <returns>
        /// List of announcement model.
        /// </returns>
        public IEnumerable<AnnouncementModel> GetAnnouncement(
            int agentId,
            string siteIds,
            DateTime? startDate,
            DateTime endDate,
            short? agentLevel = null,
            int totalMessage = 10,
            bool isTest = false,
            string content = "",
            string language = "en-US")
        {
            return this.GetData<AnnouncementModel>(new GetCustomerMessageCriteria
                 {
                     CustomerId = agentId,
                     SiteIds = siteIds,
                     EndDate = endDate,
                     IsTest = isTest,
                     StartDate = startDate,
                     AgentLevel = agentLevel,
                     Content = content,
                     Language = language
                 });
        }

        /// <summary>
        /// Count unread private message.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="isAgent">Agent or member.</param>
        /// <returns>Number of private message.</returns>
        public int CountUnreadPrivateMessage(int userId, bool isAgent = true)
        {
            var criteria = new GetPriMessageCountCriteria
            {
                UserId = userId,
                IsAgent = isAgent
            };

            if (criteria.IsValid())
            {
                return _dynamicRepository.Count(criteria);
            }

            return 0;
        }

        /// <summary>
        /// Update read private message.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="username">The username.</param>
        /// <returns>True : success, false: fail.</returns>
        public bool UpdateReadPrivateMessage(int userId, string username)
        {
            var criteria = new SetPriMessageReadCriteria
            {
                UserId = userId,
                Username = username
            };

            if (criteria.IsValid())
            {
                _dynamicRepository.Count(criteria);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Update read message.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="messsageId">The message ID.</param>
        /// <param name="isAgent">Agent or member.</param>
        /// <returns>True : success, false: fail.</returns>
        public bool UpdateReadMessage(int userId, int messsageId, bool isAgent = true)
        {
            var criteria = new UpdateReadMessageCriteria
            {
                MessageIds = messsageId.ToString(),
                CustomerId = userId, // why we need session here
                IsAgent = isAgent
            };

            _dynamicRepository.Count(criteria);

            return true;
        }

        /// <summary>
        /// Get has private message 24.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="isTest">Is test or not.</param>
        /// <param name="isAgent">Agent or member.</param>
        /// <returns>True : has, false: not.</returns>
        public bool GetHasPrivateMessage24(int userId, bool isTest = false, bool isAgent = true)
        {
            var criteria = new GetHasPriMessage24Criteria
            {
                UserId = userId,
                IsTest = isTest,
                IsAgent = isAgent
            };

            if (criteria.IsValid())
            {
                var privateMessages = this.GetData<MessageModel>(criteria);

                return privateMessages != null && privateMessages.Any();
            }

            return false;
        }

        /// <summary>
        /// Get top public message.
        /// </summary>
        /// <param name="messageType">The message type.</param>
        /// <param name="siteId">The site ID.</param>
        /// <param name="isTest">Is test or not.</param>
        /// <returns>
        /// Message model.
        /// </returns>
        public MessageModel GetTopPublicMessage(int messageType, int siteId, bool isTest = false)
        {
            var criteria = new GetTopPubMessageCriteria
            {
                TargetId = siteId,
                IsTest = isTest
            };

            if (criteria.IsValid())
            {
                var message = new MessageModel();
                var messageList = this.GetData<MessageModel>(criteria);

                if (messageList != null && messageList.Any())
                {
                    message = messageList.FirstOrDefault();
                }

                return message;
            }

            return new MessageModel();
        }

        /// <summary>
        /// Get top private message 24.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="latestMessageId">The latest message ID.</param>
        /// <param name="isTest">Is test or not.</param>
        /// <param name="isAgent">Agent or member.</param>
        /// <returns>Message model.</returns>
        public MessageModel GetTopPrivateMessage24(
            int userId,
            int latestMessageId = 0,
            bool isTest = false,
            bool isAgent = true)
        {
            var criteria = new GetTopPriMessage24Criteria
            {
                UserId = userId,
                LastMessageId = latestMessageId,
                IsTest = isTest,
                IsAgent = isAgent
            };

            if (criteria.IsValid())
            {
                if (this.GetHasPrivateMessage24(userId, isTest))
                {
                    return this.GetData<MessageModel>(criteria).FirstOrDefault();
                }
            }

            return new MessageModel();
        }

        /// <summary>
        /// Get parent sites ID.
        /// </summary>
        /// <param name="siteId">The site ID.</param>
        /// <returns>Parent sites ID string.</returns>
        public string GetParentSitesID(string siteId)
        {
            ArrayList list = new ArrayList();
            int num = 0;

            for (int i = siteId.Length - 1; i >= 0; i--)
            {
                if (siteId[i] != '0')
                {
                    num = i;
                    break;
                }
            }

            for (int j = 1; j <= num; j++)
            {
                string str = string.Empty;
                for (int m = j; m < siteId.Length; m++)
                {
                    str = str + "0";
                }

                list.Add(siteId.Remove(j) + str);
            }

            string str2 = string.Empty;
            for (int k = 0; k < list.Count; k++)
            {
                str2 = str2 + list[k] + ",";
            }

            return str2 + siteId;
        }

        /// <summary>
        /// Get message.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List of message model.</returns>
        public IEnumerable<MessageModel> GetMessage(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                return this.GetData<MessageModel>(criteria);
            }

            return new List<MessageModel>();
        }

        /// <summary>
        /// Gets the top public message level.
        /// </summary>
        /// <param name="siteId">The site identifier.</param>
        /// <param name="agentLevel">The agent level.</param>
        /// <param name="isTest">If set to <c>true</c> [is test].</param>
        /// <returns>
        /// Message level model.
        /// </returns>
        public MessageLevelModel GetTopPublicMessageLevel(int siteId, short agentLevel, bool isTest = false)
        {
            var criteria = new GetTopPubMessageByLevelCriteria
            {
                TargetId = siteId,
                AgentLevel = agentLevel,
                IsTest = isTest
            };

            return this.GetData<MessageLevelModel>(criteria).FirstOrDefault();
        }

        /// <summary>
        /// Get property value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Property value.</returns>
        private static object GetValue(object obj, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);

            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj, null);
            }

            return null;
        }

        /// <summary>
        /// Get property information.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Property information.</returns>
        private static PropertyInfo GetPropertyInfo(object obj, string propertyName)
        {
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (string.Compare(propertyInfo.Name, propertyName, true) == 0)
                {
                    return propertyInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// Check if has property information.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Property information.</returns>
        private static bool HasPropertyInfo(object obj, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);

            return !ReferenceEquals(propertyInfo, null);
        }
    }
}