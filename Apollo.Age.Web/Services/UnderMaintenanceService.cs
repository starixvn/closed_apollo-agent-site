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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Apollo.Age.Configurations;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web.ViewModels;
    using Fanex.Caching;
    using Sunplus.UMControllerLibrary;

    public class UnderMaintenanceService : ServiceBase
    {        
        private const int WaitInterval = 1000; // 1 second
        private const string MaintenanceCacheKey = "Fanex.Cache.Apollo.Maintenance"; // Cache Key

        private readonly UMBusiness umService;
        private static readonly Mutex mutext = new Mutex();
        private readonly ICache _cacheManager;

        public UnderMaintenanceService()
        {
            umService = new UMBusiness();
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        public UnderMaintenanceViewModel MaintenanceInfo
        {
            get
            {
                var maintenanceInfo = _cacheManager.Get<UnderMaintenanceViewModel>(MaintenanceCacheKey);                    
                if (maintenanceInfo == null)
                {
                    if (mutext.WaitOne(WaitInterval))
                    {
                        try
                        {
                            maintenanceInfo = GetMaintenanceInfo();
                            _cacheManager.Set<UnderMaintenanceViewModel>(MaintenanceCacheKey, maintenanceInfo, TimeSpan.FromMinutes(1));                                
                        }
                        catch
                        {
                            maintenanceInfo = new UnderMaintenanceViewModel { IsDownTime = false };
                        }
                        finally
                        {
                            mutext.ReleaseMutex();
                        }
                    }
                    else
                    {
                        maintenanceInfo = new UnderMaintenanceViewModel { IsDownTime = false };
                    }
                }
                return maintenanceInfo;
            }
        }

        private UnderMaintenanceViewModel GetMaintenanceInfo()
        {
            var maintenanceInfo = new UnderMaintenanceViewModel { IsDownTime = false };
            var startTime = maintenanceInfo.StartTime;
            var endTime = maintenanceInfo.EndTime;

            // Check downtime from AppSetting
            if (AppSettings.IsApolloDownTime)
            {
                maintenanceInfo.IsDownTime = true;
                maintenanceInfo.StartTime = AppSettings.StartTime;
                maintenanceInfo.EndTime = AppSettings.EndTime;

                return maintenanceInfo;
            }

            // Check downtime from library
            try
            {
                maintenanceInfo.IsDownTime = false; // umService.isUnderMaintainence(Sites.Agency, AppSettings.UnderMaintenanceUrl, ref startTime, ref endTime);
                if (maintenanceInfo.IsDownTime)
                {
                    maintenanceInfo.StartTime = startTime;
                    maintenanceInfo.EndTime = endTime;
                }

                return maintenanceInfo;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return new UnderMaintenanceViewModel { IsDownTime = false };
            }
        }
    }
}