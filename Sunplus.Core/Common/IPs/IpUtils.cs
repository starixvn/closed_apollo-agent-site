namespace Sunplus.Core.Common.IPs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Fanex.Caching;
    using Sunplus.Core.Common.IPs.DAL;

    public static class IpUtils
    {
        public const string IpPattern = @"(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)";
        public const string IpVnKey = "Sunplus.Core.Common.IPs.IpVnKey";

        public const string IpLinkFormat = "<a class=\"iplink\" href=\"javascript:OpenIPInfo('{0}');\">{0}</a>";


        /// <summary>
        /// Check valid IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns>return boolean</returns>
        public static bool IsValidIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return false;

            var regex = new Regex(IpPattern);


            return regex.IsMatch(ip, 0);
        }

        public static string FormatIp(string ip)
        {
            return string.Format(IpLinkFormat, ip);
        }

        static List<IPVN> IPVNList = null;

        /// <summary>
        /// get all ip of vn and add to cache
        /// </summary>
        /// <returns>List ipvn</returns>
        public static List<IPVN> GetAllVnIps()
        {
            ICache cache = CacheService.CreateInstance(CacheType.MemoryCache);
            var entityList = cache.Get<List<IPVN>>(IpVnKey);

            if (ReferenceEquals(entityList, null))
            {
                using (var dataContext = new IPEntities(Utility.GetEFConnectionString()))
                {
                    entityList = dataContext.IPVN.Select(s => s).ToList<IPVN>();
                    if (entityList.Count > 0)
                    {
                        cache.Set<List<IPVN>>(IpVnKey, entityList, TimeSpan.FromHours(1));
                    }

                }

            }

            IPVNList = entityList;

            return entityList;
        }

        /// <summary>
        /// Convert IP to number
        /// </summary>
        /// <param name="ip">ip is not empty string</param>
        /// <returns></returns>
        public static double ConvertIpToNumber(string ip)
        {
            double ipNumber = 0;
            if (IsValidIp(ip))
            {
                string[] ipList = ip.Split('.');

                ipNumber = double.Parse(ipList[0]) * 16777216
                    + double.Parse(ipList[1]) * 65536
                    + double.Parse(ipList[2]) * 256
                    + double.Parse(ipList[3]);
            }

            return ipNumber;
        }

        public static bool IsVnIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return false;

            var ipNumber = ConvertIpToNumber(ip);
            var ipvns = IPVNList ?? GetAllVnIps();
            if (ipvns != null)
            {
                var isIpVn = ipvns.Exists((ipvn => (ipvn.IPFrom <= ipNumber && ipNumber <= ipvn.IPTo)));

                return isIpVn;
            }

            return false;
        }



        /// <summary>
        /// Get Location Information From Specific IP
        /// </summary>
        /// <param name="ip">ip input</param>
        /// <returns></returns>
        public static IPTable LocationFromIP(string ip)
        {
            using (var dataContext = new IPEntities(Sunplus.Core.Common.IPs.Utility.GetEFConnectionString()))
            {
                List<IPTable> ips = dataContext.GetIPInfo_Full(ip).ToList();
                if (ips != null && ips.Count > 0)
                {
                    return ips[0];
                }
            }

            return new IPTable();
        }
    }
}
