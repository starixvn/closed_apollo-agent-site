// <copyright file="HttpRuntimeCache.cs" company="Sunplus Technology Vietnam">
// Copyright (c) Sunplus Technology Vietnam. All rights reserved. 
// </copyright>
//
// <summary>
//  Implement HttpRuntimeCache
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
    using System.Web;
    using System.Web.Caching;

    /// <summary>
    /// Http Runtime cache
    /// </summary>
    public class HttpRuntimeCache : ICache
    {
        #region ICache Members

        public void SetParameters(string host, string bucket, string password)
        {
            throw new NotSupportedException();
        }

        private long _defaultExpiredTime = 60;

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
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, object value)
        {
            if (CheckValidInput(key, value))
            {
                Add(key, value, DefaultExpiredTime);
            }
        }

        /// <summary>
        /// Checks the valid input.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool CheckValidInput(string key, object value)
        {
            return (!string.IsNullOrEmpty(key)
                && key.Trim().Length > 0
                && !ReferenceEquals(value, null));
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="milliSeconds">The milli seconds.</param>
        public void Add(string key, object value, long milliSeconds)
        {
            if (CheckValidInput(key, value))
            {
                HttpRuntime.Cache.Add(key, value, null, DateTime.Now.AddSeconds(milliSeconds * 1000), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="timeSpan">The time span.</param>
        public void Add(string key, object value, TimeSpan timeSpan)
        {
            if (CheckValidInput(key, value))
            {
                HttpRuntime.Cache.Add(key, value, null, DateTime.Now.Add(timeSpan), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
            }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return HttpRuntime.Cache.Get(key);
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
            if (string.IsNullOrEmpty(key) && key.Length <= 0)
                throw new Exception("Key is not null or empty");

            return (T)HttpRuntime.Cache.Get(key);
        }

        /// <summary>
        /// Gets the specified keys.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
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
        /// Gets the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        public List<T> GetList<T>(params string[] keys)
        {
            var entityList = new List<T>();
            foreach (var key in keys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    entityList.AddRange(GetList<T>(key));
                }
            }

            return entityList;
        }

        public List<T> GetList<T>(string key)
        {
            return HttpRuntime.Cache.Get(key) as List<T>;
        }

        /// <summary>
        /// Removes all.
        /// </summary>
        public void RemoveAll()
        {
            HttpRuntime.Close();
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            RemoveAll();
        }
        #endregion
    }
}
