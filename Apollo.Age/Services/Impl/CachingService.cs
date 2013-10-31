// --------------------------------------------------------------------------
// <copyright file="CachingService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.4.9">Created</change>
// </history>

namespace Apollo.Age.Services.Impl
{
    using System;
    using Fanex.Caching;

    /// <summary>
    /// Caching service.
    /// </summary>
    public class CachingService
    {
        private static readonly ICache _cacheManager;

        static CachingService()
        {
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        public static object Get(string key)
        {
            try
            {
                return _cacheManager.Get<object>(key);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void Set(string key, object value)
        {
            try
            {
                _cacheManager.Set(key, value);
            }
            catch (Exception)
            {
            }
        }

        public static T Get<T>(string key)
        {
            try
            {
                return _cacheManager.Get<T>(key);
            }
            catch
            {
                return default(T);
            }
        }

        public static void Set<T>(string key, T value)
        {
            _cacheManager.Set<T>(key, value);
        }

        public static void Set(string key, object value, TimeSpan timeSpanExpiration)
        {
            _cacheManager.Set(key, value, timeSpanExpiration);
        }

        public static void Set<T>(string key, T value, TimeSpan timeSpanExpiration)
        {
            _cacheManager.Set<T>(key, value, timeSpanExpiration);
        }

        public static void Set(string key, object value, DateTime absoluteExpiration)
        {
            _cacheManager.Set(key, value, absoluteExpiration);
        }

        public static void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            _cacheManager.Set<T>(key, value, absoluteExpiration);
        }

        public static void Remove(string key)
        {
            _cacheManager.Remove(key);
        }
    }
}