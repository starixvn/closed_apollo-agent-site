using System;
using System.Collections.Generic;


namespace Sunplus.Core.CachingLib
{
    /// <summary>
    /// Use WeakReference.
    /// </summary>
    public class Velocity : ICache
    {
        static Dictionary<string, WeakReference> _cache = new Dictionary<string, WeakReference>();

        public void SetParameters(string host, string bucket, string password)
        {
            throw new NotSupportedException();
        }

        public long DefaultExpiredTime
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public void Add(string key, object value)
        {
            if (!string.IsNullOrEmpty(key)
              && key.Trim().Length > 0
              && value != null)
            {

                if (_cache.ContainsKey(key))
                {
                    _cache[key] = new WeakReference(value);
                }
                else
                {
                    _cache.Add(key, new WeakReference(value));
                }
            }
        }

        public void Add(string key, object value, long milliSeconds)
        {
            throw new NotSupportedException();
        }

        public void Add(string key, object value, TimeSpan timeSpan)
        {
            throw new NotSupportedException();
        }

        public object Get(string key)
        {
            if (!string.IsNullOrEmpty(key)
              && key.Trim().Length > 0
              )
            {
                WeakReference oldRef;
                if (_cache.TryGetValue(key, out oldRef))
                {
                    return oldRef.Target;
                }
            }

            return null;
        }

        public T Get<T>(string key)
        {
            if (!string.IsNullOrEmpty(key)
              && key.Trim().Length > 0
              )
            {
                object value = Get(key);
                if (value != null)
                {
                    return (T)(value);
                }
            }

            return default(T);
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
            Clear();
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
        
        public void Clear()
        {
            _cache.Clear();
        }
    }
}
