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
    /// TODO: Update summary.
    /// </summary>
    public class MessageService : QueryService, IMessageService
    {
        private DynamicRepository _dyanamicRepository;

        private ICache _cache;

        #region ctor
        public MessageService(ICache cache, DynamicRepository dynamicRepository)
        {
            _cache = cache ?? CacheService.CreateInstance(CacheType.MemoryCache);
            _dyanamicRepository = _dyanamicRepository ?? new DynamicRepository();
        }

        public MessageService()
            : this(null, null)
        {
        }
       
        #endregion       

        public IEnumerable<AnnouncementModel> GetAnnouncement(
            int agentId,
            string siteIds,
            DateTime lastPostDate,
            int totalMessage = 10,
            bool isTest = false,
            bool isAgent = true,
            DateTime? startTime = null,
            string content = "",
            string langue = "en-US")
        {
            return this.GetData<AnnouncementModel>(new GetCustomerMessageCriteria
                {
                    CustomerId = agentId,
                    SiteIds = siteIds,
                    LastPostTime = lastPostDate,
                    IsTest = isTest,
                    IsAgent = isAgent,
                    StartDate = startTime,
                    Content = content,
                    Language = langue
                });
        }

        public int CountUnreadPrivateMessage(int userId, bool isAgent = true)
        {
            var criteria = new GetPriMessageCountCriteria
            {
                UserId = userId,
                IsAgent = isAgent
            };

            if (criteria.IsValid())
            {
                return _dyanamicRepository.Count(criteria);
            }

            return 0;
        }

        public bool UpdateReadPrivateMessage(int userId, string username)
        {
            var criteria = new SetPriMessageReadCriteria
            {
                UserId = userId,
                Username = username
            };

            if (criteria.IsValid())
            {
                _dyanamicRepository.Count(criteria);

                return true;
            }

            return false;
        }

        public bool UpdateReadMessage(int userId, int messsageId, bool isAgent = true)
        {
            var criteria = new UpdateReadMessageCriteria
            {
                MessageIds = messsageId.ToString(),
                CustomerId = userId, // why we need session here
                IsAgent = isAgent
            };

            _dyanamicRepository.Count(criteria);

            return true;
        }

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

        public MessageModel GetTopPublicMessage(int messageType, int siteId, int latestMessageId = 0, bool isTest = false)
        {
            char[] separator = new char[] { ',' };
            string[] arraySite = GetParentSitesID(siteId.ToString()).Split(separator);

            var criteria = new GetTopPubMessageCriteria
            {
                MessageType = messageType,
                Site1 = arraySite[0],
                Site2 = arraySite.Length > 1 ? arraySite[1] : string.Empty,
                Site3 = arraySite.Length > 2 ? arraySite[2] : string.Empty,
                LastMessageId = latestMessageId,
                IsTest = isTest
            };

            if (criteria.IsValid())
            {
                //string key = messageType.ToString() + "_" + siteId;
                MessageModel message = new MessageModel();
                string key = string.Join("_", messageType.ToString(), siteId);

                message = _cache.Get<MessageModel>(key);

                if (message == null)
                {
                    var messageList = this.GetData<MessageModel>(criteria);

                    if (messageList != null && messageList.Any())
                    {
                        message = messageList.FirstOrDefault();
                        _cache.Set<MessageModel>(key, message);
                    }
                }              

                return message;
            }

            return new MessageModel();
        }

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

        public IEnumerable<MessageModel> GetMessage(ICriteria criteria)
        {
            if (criteria.IsValid())
            {
                return this.GetData<MessageModel>(criteria);
            }

            return new List<MessageModel>();
        }

        private static object GetValue(object obj, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(obj, null);
            }

            return null;
        }

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

        private static bool HasPropertyInfo(object obj, string propertyName)
        {
            var propertyInfo = GetPropertyInfo(obj, propertyName);

            return !ReferenceEquals(propertyInfo, null);
        }
    }
}
