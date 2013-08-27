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
        private readonly HttpClient _client;

        public UnderMaintenanceService()
        {
            _underMaintenanceService = new UMBusiness();
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
            _client = new HttpClient();
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
                            maintenanceInfo = new UnderMaintenanceViewModel { Status = false };
                        }
                        finally
                        {
                            _threadMuText.ReleaseMutex();
                        }
                    }
                    else
                    {
                        maintenanceInfo = new UnderMaintenanceViewModel { Status = false };
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
            var umInfo = new UnderMaintenanceViewModel { Status = false };
            var startTime = umInfo.Start_Time;
            var endTime = umInfo.End_Time;

            // Check downtime from AppSetting.
            if (AppSettings.IsApolloDownTime)
            {
                umInfo.Status = true;
                umInfo.Start_Time = AppSettings.StartTime;
                umInfo.End_Time = AppSettings.EndTime;

                return umInfo;
            }

            // Check downtime from library.
            try
            {

                if (AppSettings.DisabledUnderMaintenanceAPI)
                {
                    umInfo.Status = false;
                }
                else
                {
                    HttpResponseMessage response = _client.GetAsync(AppSettings.UnderMaintenanceUrl).Result;
                    umInfo = JsonConvert.DeserializeObject<UnderMaintenanceViewModel>(response.Content.ReadAsStringAsync().Result);
                }

                //if (umInfo.Status)
                //{
                //    umInfo.StartTime = startTime;
                //    umInfo.EndTime = endTime;
                //}

                return umInfo;
            }
            catch (Exception ex)
            {
                HandleException(ex);

                return new UnderMaintenanceViewModel { Status = false };
            }
        }
    }
}