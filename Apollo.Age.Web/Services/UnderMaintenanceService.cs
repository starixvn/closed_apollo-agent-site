// --------------------------------------------------------------------------
// <copyright file="UnderMaintenanceService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry" date="2013.04.23">Created</change>
//   <change who="Anders.Le" date="2013.6.12">Add Fanex.Caching</change>
// </history>

namespace Apollo.Age.Web.Services
{
    using System;
    using System.Threading;
    using Apollo.Age.Configurations;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web.ViewModels;
    using Fanex.Caching;
    using Sunplus.UMControllerLibrary;
    using System.Text;
    using System.Net.NetworkInformation;
    using System.Net;
    using System.Net.Sockets;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;
    using System.IO;
    using System.Web.Helpers;

    /// <summary>
    /// Under Maintenance Service.
    /// </summary>
    public class UnderMaintenanceService : ServiceBase
    {
        private const int WaitInterval = 1000; // 1 second.
        private const string MaintenanceCacheKey = "Fanex.Cache.Apollo.Maintenance"; // Cache Key.

        private readonly UMBusiness _underMaintenanceService;
        private readonly Mutex _threadMuText = new Mutex();
        private readonly ICache _cacheManager;
       //private readonly HttpClient _client;
        private HttpWebRequest request;

        public UnderMaintenanceService()
        {
            _underMaintenanceService = new UMBusiness();
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
           // _client = new HttpClient();
            
        }

        public UnderMaintenanceViewModel MaintenanceInfo
        {
            get
            {
                var maintenanceInfo = _cacheManager.Get<UnderMaintenanceViewModel>(MaintenanceCacheKey);
                if (maintenanceInfo == null)
                {
                    if (_threadMuText.WaitOne(WaitInterval))
                    {
                        try
                        {
                            maintenanceInfo = GetMaintenanceInfo();
                            _cacheManager.Set<UnderMaintenanceViewModel>(MaintenanceCacheKey, maintenanceInfo, TimeSpan.FromMinutes(1)); //TimeSpan.FromMinutes(0));
                        }
                        catch
                        {
                            maintenanceInfo = new UnderMaintenanceViewModel { status = false };
                        }
                        finally
                        {
                            _threadMuText.ReleaseMutex();
                        }
                    }
                    else
                    {
                        maintenanceInfo = new UnderMaintenanceViewModel { status = false };
                    }
                }

                return maintenanceInfo;
            }
        }

        /// <summary>
        /// Get IBC Maintenance Info.
        /// </summary>
        /// <returns>Return Under Maintenance View Model.</returns>
        private UnderMaintenanceViewModel GetMaintenanceInfo()
        {
            var umInfo = new UnderMaintenanceViewModel { status = false };
            var startTime = umInfo.start_time;
            var endTime = umInfo.end_time;

            // Check downtime from AppSetting.
            if (AppSettings.IsApolloDownTime)
            {
                umInfo.status = true;
                umInfo.start_time = AppSettings.StartTime;
                umInfo.end_time = AppSettings.EndTime;

                return umInfo;
            }

            // Check downtime from library.
            try
            {

                if (AppSettings.DisabledUnderMaintenanceAPI)
                {
                    umInfo.status = false;
                }
                else
                {
                    request = (HttpWebRequest)WebRequest.Create(AppSettings.UnderMaintenanceUrl);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Method = WebRequestMethods.Http.Get; // it is required                    
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {

                        Stream dataStream = response.GetResponseStream();
                        // read response stream and parse to JsonResponseModel
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            string jsonString = reader.ReadToEnd();
                            if (!string.IsNullOrWhiteSpace(jsonString))
                            {
                                umInfo = Json.Decode<UnderMaintenanceViewModel>(jsonString);
                            }

                            //Fanex.Logging.LogService.Error(jsonString);
                        }
                    }
                                        
                }

                return umInfo;
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new UnderMaintenanceViewModel { status = false };
            }
        }
    }
}