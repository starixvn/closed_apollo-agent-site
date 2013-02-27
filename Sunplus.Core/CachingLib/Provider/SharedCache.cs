// <copyright file="SharedCache.cs" company="Sunplus Technology Vietnam">
// Copyright (c) Sunplus Technology Vietnam. All rights reserved. 
// </copyright>
//
// <summary>
//  Implement SharedCache
// </summary>
//
// <history>
//  <change who="Anders" date="2010.01.14">Created</change>
//  <change who="Anders" date="2010.08.20">Implement</change>
//  <change who="Anders" date="2010.10.15">Rename IBC to Sunplus</change>
// </history>

using SCCACHE = SharedCache.WinServiceCommon.Provider.Cache.IndexusDistributionCache;

namespace Sunplus.Core.CachingLib
{
    using System;
    using System.Collections.Generic;    
    using System.Collections;

    public class SharedCache : ICache
    {
        /// <summary>
        /// default 10min cache duration
        /// </summary>
        private long _defaultExpiredTime = 10;

        /// <summary>
        /// unit is minute
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
        /// Add object value to cache
        /// </summary>
        /// <param name="key">string is not null or empty</param>
        /// <param name="value">object must be not null</param>
        public void Add(string key, object value)
        {
            if (!string.IsNullOrEmpty(key)
                && key.Trim().Length > 0
                && !ReferenceEquals(value, null))
            {
                SCCACHE.SharedCache.Add(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="useDefaultExpiredTime"></param>
        public void Add(string key, object value, bool useDefaultExpiredTime)
        {
            if (!string.IsNullOrEmpty(key)
               && key.Trim().Length > 0
               && !ReferenceEquals(value, null))
            {
                if (useDefaultExpiredTime)
                {
                    SCCACHE.SharedCache.Add(key, value, DateTime.Now.AddMinutes(DefaultExpiredTime));
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="milliSeconds">The milli seconds.</param>
        public void Add(string key, object value, long milliSeconds)
        {
            if (!string.IsNullOrEmpty(key)
               && key.Trim().Length > 0
               && !ReferenceEquals(value, null)
                && milliSeconds > 0)
            {
                SCCACHE.SharedCache.Add(key, value, DateTime.Now.AddMilliseconds(milliSeconds));
            }
        }

        /// <summary>
        /// Add object to cache with timeSpan expired
        /// </summary>
        /// <param name="key"><seealso cref="Add"/></param>
        /// <param name="value"><seealso cref="Add"/></param>
        /// <param name="timeSpan"></param>
        public void Add(string key, object value, TimeSpan timeSpan)
        {
            if (!string.IsNullOrEmpty(key)
               && key.Trim().Length > 0
               && !ReferenceEquals(value, null))
            {
                SCCACHE.SharedCache.Add(key, value, DateTime.Now.Add(timeSpan));
            }
        }

        /// <summary>
        /// Get object from cache
        /// </summary>
        /// <param name="key">must be not null or empty</param>
        /// <returns></returns>
        public object Get(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key)
                    && SCCACHE.SharedCache.GetAllKeys().Contains(key))
                {
                    return SCCACHE.SharedCache.Get(key);
                }
            }
            catch
            {

            }
            return null;
        }

        /// <summary>
        /// Get generic object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            return (T)Get(key);
        }

        /// <summary>
        /// Gets the specified keys.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns><![CDATA[ IDictionary<string, T>]]></returns>
        public IDictionary<string, T> Get<T>(params string[] keys)
        {
            IDictionary<string, T> list = new Dictionary<string, T>();
            if (keys != null && keys.Length > 0)
            {
                foreach (var key in keys)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        list.Add(key, Get<T>(key));
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// get list generic object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns> List<T></returns>
        public List<T> GetList<T>(params string[] keys)
        {
            var entityList = new List<T>();
            foreach (var key in keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    entityList.Add(Get<T>(key));
                }
            }

            return entityList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string key)
        {
            var entityList = new List<T>();
            if (!string.IsNullOrEmpty(key)
                && SCCACHE.SharedCache.GetAllKeys().Contains(key))
            {
                entityList = SCCACHE.SharedCache.Get(key) as List<T>;
            }

            return entityList;
        }

        /// <summary>
        /// clear cache
        /// </summary>
        public void RemoveAll()
        {
            List<string> keyList = SCCACHE.SharedCache.GetAllKeys();
            keyList.ForEach(key => Remove(key));
        }

        /// <summary>
        /// Clear cache
        /// </summary>
        public void Clear()
        {
            RemoveAll();
        }

        /// <summary>
        /// Remove object
        /// </summary>
        /// <param name="key">not null or empty key</param>
        public void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key) &&
                SCCACHE.SharedCache.GetAllKeys().Contains(key))
            {
                SCCACHE.SharedCache.Remove(key);
            }
        }
    }
}
