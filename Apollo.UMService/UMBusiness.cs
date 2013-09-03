// --------------------------------------------------------------------------
// <copyright file="UMService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.9.3">Created</change>
// </history>

namespace Apollo.UMService
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Web.Helpers;
    using Apollo.UMService.Model;
    using Fanex.Caching;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UMBusiness
    {
        private const int WaitInterval = 1000; // 1 second.
        private const string MaintenanceCacheKey = "Fanex.Cache.Apollo.Maintenance"; // Cache Key.
        private readonly Mutex _threadMuText = new Mutex();
        private readonly ICache _cacheManager;
        private HttpWebRequest _request;
        private string _umUrlFormat = "{0}?site={1}";

        public UMBusiness()
        {
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        public UnderMaintenanceModel GetMaintainenceInfo(string siteName, string umCtrlUrl)
        {
            var umInfo = new UnderMaintenanceModel();
            umInfo = _cacheManager.Get<UnderMaintenanceModel>(MaintenanceCacheKey);

            if (umInfo == null)
            {
                if (_threadMuText.WaitOne(WaitInterval))
                {
                    try
                    {
                        umCtrlUrl = string.Format(_umUrlFormat, umCtrlUrl, siteName);

                        umInfo = this.GetMaintainenceInfo(umCtrlUrl);
                        _cacheManager.Set<UnderMaintenanceModel>(MaintenanceCacheKey, umInfo, TimeSpan.FromMinutes(1)); //TimeSpan.FromMinutes(0));
                    }
                    catch
                    {
                        umInfo = new UnderMaintenanceModel { status = false };
                    }
                    finally
                    {
                        _threadMuText.ReleaseMutex();
                    }
                }
                else
                {
                    umInfo = new UnderMaintenanceModel { status = false };
                }
            }

            return umInfo;
        }

        private UnderMaintenanceModel GetMaintainenceInfo(string umCtrlUrl)
        {
            var umInfo = new UnderMaintenanceModel();

            _request = (HttpWebRequest)WebRequest.Create(umCtrlUrl);
            _request.Credentials = CredentialCache.DefaultCredentials;
            _request.Method = WebRequestMethods.Http.Get; // it is required    

            HttpWebResponse response = (HttpWebResponse)_request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream dataStream = response.GetResponseStream();
                // read response stream and parse to JsonResponseModel

                using (StreamReader reader = new StreamReader(dataStream))
                {
                    string jsonString = reader.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(jsonString))
                    {
                        umInfo = Json.Decode<UnderMaintenanceModel>(jsonString);
                    }
                }
            }

            return umInfo;
        }
    }
}
