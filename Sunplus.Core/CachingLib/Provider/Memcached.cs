using System;
using System.Collections.Generic;
using System.Text;
using Membase;
using Membase.Configuration;

/*
 * Created 20091224@Lee - EnyimMemcached client
 * Revision:
 * 20091224@Lee
 * 
 * 
 */

namespace Sunplus.Core.CachingLib
{
    public class Memcached : ICache
    {
        MembaseClient client = null;
        MembaseClientConfiguration configuration = null;
        static object locker = new object();

        public Memcached()
        {

        }

        #region ICache Members

        public void SetParameters(string host, string bucket, string password)
        {
            if (client == null)
            {
                lock (locker)
                {
                    if (client == null)
                    {
                        configuration = new MembaseClientConfiguration();
                        configuration.Urls.Add(new Uri(host));
                        configuration.Bucket = bucket;
                        configuration.BucketPassword = password;

                        client = new MembaseClient(configuration);
                    }
                }
            }
        }

        private long _defaultExpiredTime = 600;
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

        public void Add(string key, object value)
        {
            client.Store(Enyim.Caching.Memcached.StoreMode.Add, key, value, TimeSpan.FromSeconds(DefaultExpiredTime));
        }

        public void Add(string key, object value, long milliSeconds)
        {
            client.Store(Enyim.Caching.Memcached.StoreMode.Add, key, value, TimeSpan.FromMilliseconds(milliSeconds));
        }

        public void Add(string key, object value, TimeSpan timeSpan)
        {
            client.Store(Enyim.Caching.Memcached.StoreMode.Add, key, value, timeSpan);
        }

        public object Get(string key)
        {
           return client.Get(key);
        }

        public T Get<T>(string key)
        {
            return client.Get<T>(key);
        }

        public IDictionary<string, T> Get<T>(params string[] keys)
        {
            throw new NotSupportedException();
        }

        public List<T> GetList<T>(params string[] keys)
        {
            throw new NotSupportedException();
        }

        public List<T> GetList<T>(string key)
        {
            throw new NotSupportedException();
        }

        public void RemoveAll()
        {
            throw new NotSupportedException();
        }

        public void Remove(string key)
        {
            client.Remove(key);
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
