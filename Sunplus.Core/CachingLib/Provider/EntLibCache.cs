// <copyright file="EntLibCache.cs" company="Sunplus Technology Vietnam">
// Copyright (c) Sunplus Technology Vietnam. All rights reserved. 
// </copyright>
//
// <summary>
//  Using enterprise library to implement ICahe
// </summary>
//
// <history>
//  <change who="Anders" date="2010.01.14">Created</change>
//  <change who="Anders" date="2010.08.20">Implement</change>
//  <change who="Anders" date="2010.10.15">Rename IBC to Sunplus</change>
// </history>
namespace Sunplus.Core.CachingLib
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.EnterpriseLibrary.Caching;
    using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    /// <summary>
    /// Enterprise Library caching
    /// </summary>
    public class EntLibCache : ICache
    {
        private readonly ICacheManager _cacheManager;

        public EntLibCache()
        {
            _cacheManager = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
        }

        #region ICache Members


        public void SetParameters(string host, string bucket, string password)
        {
            throw new NotSupportedException();
        }

        private long _defaultExpiredTime = 10;
        /// <summary>
        /// default expired time. unit is minute
        /// </summary>
        public long DefaultExpiredTime
        {
            get
            {
                return _defaultExpiredTime;
            }
            set
            {
                _defaultExpiredTime = value;
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key is not null or empty</param>
        /// <param name="value">The value is not null</param>
        public void Add(string key, object value)
        {
            _cacheManager.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="milliSeconds"></param>
        public void Add(string key, object value, long milliSeconds)
        {
            if (!string.IsNullOrEmpty(key)
            && key.Trim().Length > 0
            && value != null)
            {
                _cacheManager.Add(key, value, CacheItemPriority.High, null,
                    new ICacheItemExpiration[] { new AbsoluteTime(DateTime.Now.AddMilliseconds(milliSeconds))});
            }
            
        }

        /// <summary>
        /// <seealso cref="Add(string key, object value, long milliSeconds)"/>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan">time from Now</param>
        public void Add(string key, object value, TimeSpan timeSpan)
        {
            if (!string.IsNullOrEmpty(key)
            && key.Trim().Length > 0
            && value != null)
            {
                _cacheManager.Add(key, value, CacheItemPriority.High, null,
                    new ICacheItemExpiration[] { new AbsoluteTime(timeSpan) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">key must be not null or empty, valid key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (!string.IsNullOrEmpty(key) && _cacheManager.Contains(key))
            {
                return _cacheManager.GetData(key);
            }

            return null;
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            object obj = Get(key);

            return (T)obj;
        }


        /// <summary>
        /// Gets the specified keys.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public IDictionary<string, T> Get<T>(params string[] keys)
        {
            var values = new Dictionary<string, T>();
            foreach (var key in keys)
            {
                values.Add(key, Get<T>(key));
            }

            return values;
        }

        public List<T> GetList<T>(params string[] keys)
        {
            var values = new List<T>();
            foreach (var key in keys)
            {
                values.Add(Get<T>(key));
            }
            return values;
        }

        public List<T> GetList<T>(string key)
        {
            return Get(key) as List<T>;
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        public void RemoveAll()
        {
            _cacheManager.Flush();
        }

        public void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _cacheManager.Remove(key);
            }
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        public void Clear()
        {
            RemoveAll();
        }

        #endregion
    }
}
