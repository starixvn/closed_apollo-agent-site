// <copyright file="EntLibLogger.cs" company="Sunplus Technology Vietnam">
// Copyright (c) Sunplus Technology Vietnam. All rights reserved. 
// </copyright>
//
// <summary>
//  Implement EntLibLoggingService
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
   
    public interface ICache
    {
        /// <summary>
        /// time unit: second
        /// </summary>
        long DefaultExpiredTime { get; set; }

        void SetParameters(string host, string bucket, string password);

        void Add(string key, object value);
        void Add(string key, object value, long milliSeconds);
        void Add(string key, object value, TimeSpan timeSpan);

        object Get(string key);
        T Get<T>(string key);
        IDictionary<string, T> Get<T>(params string[] keys);
        List<T> GetList<T>(params string[] keys);
        List<T> GetList<T>(string key);

        void RemoveAll();
        void Remove(string key);
        void Clear();
    }
}
