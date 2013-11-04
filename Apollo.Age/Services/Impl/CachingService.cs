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
        private static readonly ICache CacheManager;

        static CachingService()
        {
            CacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        public static object Get(string key)
        {
            try
            {
                return CacheManager.Get<object>(key);
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
                CacheManager.Set(key, value);
            }
            catch (Exception)
            {
            }
        }

        public static T Get<T>(string key)
        {
            try
            {
                return CacheManager.Get<T>(key);
            }
            catch
            {
                return default(T);
            }
        }

        public static void Set<T>(string key, T value)
        {
            CacheManager.Set<T>(key, value);
        }

        public static void Set(string key, object value, TimeSpan timeSpanExpiration)
        {
            CacheManager.Set(key, value, timeSpanExpiration);
        }

        public static void Set<T>(string key, T value, TimeSpan timeSpanExpiration)
        {
            CacheManager.Set<T>(key, value, timeSpanExpiration);
        }

        public static void Set(string key, object value, DateTime absoluteExpiration)
        {
            CacheManager.Set(key, value, absoluteExpiration);
        }

        public static void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            CacheManager.Set<T>(key, value, absoluteExpiration);
        }

        public static void Remove(string key)
        {
            CacheManager.Remove(key);
        }
    }
}