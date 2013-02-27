using System.Data.EntityClient;
using System.Configuration;

namespace Sunplus.Core.Common.IPs
{
    using Fanex.Data;
    using Fanex.Common;
    using Fanex.Caching;

    public static class Utility
    {
        public static string GetEFConnectionString()
        {
            ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);

            string connectionString = cache.Get<string>("IPEntities_ConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                string encryptConnectionString = ConfigurationManager.ConnectionStrings["IPEntities"].ConnectionString;
                var entityBuilder = new EntityConnectionStringBuilder(encryptConnectionString);
                entityBuilder.ProviderConnectionString = Cryptography.DeCryptConnectionString(entityBuilder.ProviderConnectionString);

                connectionString = entityBuilder.ToString();

                cache.Set<string>("IPEntities_ConnectionString", connectionString, System.TimeSpan.FromHours(1));
            }

            return connectionString;
        }
    }
}
