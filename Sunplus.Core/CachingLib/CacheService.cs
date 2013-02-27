namespace Sunplus.Core.CachingLib
{
    using System.Collections;

    public static class CacheService
    {
        private static Hashtable _cacheStore;
       
        // Lock synchronization object
        private static readonly object SyncLock = new object();

        /// <summary>
        /// Create Cache Instance
        /// </summary>
        /// <param name="type">type: CacheType</param>
        /// <returns></returns>

        public static ICache CreateInstance(CacheType type)
        {
            /* 
             * Support multithreaded applications through
             * 'Double checked locking' pattern which (once the instance exists) avoids locking each
             * time the method is invoked
             */
            ICache cachInstance = null;
            lock (SyncLock)
            {
                if (ReferenceEquals(_cacheStore, null))
                {
                    _cacheStore = new Hashtable();
                }
                else
                {
                    if (_cacheStore.ContainsKey(type.ToString()))
                    {
                        return _cacheStore[type.ToString()] as ICache;
                    }
                }

                switch (type)
                {
                    case CacheType.Memcached:
                        cachInstance = new Memcached();
                        break;
                    case CacheType.HttpRuntime:
                        cachInstance = new HttpRuntimeCache();
                        break;
                    case CacheType.EntLib:
                        cachInstance = new EntLibCache();
                        break;
                    case CacheType.Velocity:
                        cachInstance = new Velocity();
                        break;
                    default:
                        cachInstance = new HttpRuntimeCache();
                        break;
                }

                _cacheStore.Add(type.ToString(), cachInstance);
            }
            
            
            return cachInstance;
        }

        /// <summary>
        /// Create instance with expired time
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expiredTime">time unit: second</param>
        /// <returns></returns>
        public static ICache CreateInstance(CacheType type, long expiredTime)
        {
            ICache cachInstance = CreateInstance(type);
            cachInstance.DefaultExpiredTime = expiredTime;

            return cachInstance;
        }


        /// <summary>
        /// Create SharedCache Instance
        /// </summary>
        /// <returns></returns>
        public static ICache CreateInstance()
        {
            return CreateInstance(CacheType.HttpRuntime);
        }

        /// <summary>
        /// Dispose cache instance
        /// </summary>
        /// <param name="type"></param>
        public static void DisposeInstance(CacheType type)
        {
            lock (SyncLock)
            {
                if (ReferenceEquals(_cacheStore, null)) return;
                if (_cacheStore.ContainsKey(type.ToString()))
                {
                    _cacheStore.Remove(type.ToString());
                }
            }
        }

        /// <summary>
        /// Dispose shared cache instance
        /// </summary>
        public static void DisposeInstance()
        {
            DisposeInstance(CacheType.HttpRuntime);
        }
    }

    /// <summary>
    /// Memcached = 1, HttpRuntime = 2, EntLib = 3, Velocity = 4
    /// </summary>
    public enum CacheType
    {
        Memcached = 1, HttpRuntime = 2, EntLib = 3, Velocity = 4
    }
}
