namespace Apollo.Age.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Apollo.Age.Configurations;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Web.Models;
    using Fanex.Caching;
    using Sunplus.UMControllerLibrary;

    public class UnderMaintenanceService : ServiceBase
    {
        private const int CACHETIMEOUTMINUTE = 1; // 1 minute
        private const int WAITINTERVAL = 1000; // 1 second
        private const string MAINTENANCECACHEKEY = "Fanex.Cache.Apollo.Maintenance"; // Cache Key

        private readonly UMBusiness umService;
        private static readonly Mutex mutext = new Mutex();
        private readonly ICache cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);

        public UnderMaintenanceService()
        {
            umService = new UMBusiness();
        }

        public UnderMaintenanceModel UnderMaintenanceInfo
        {
            get
            {
                var maintenanceInfo = this.cacheManager.Get<UnderMaintenanceModel>(MAINTENANCECACHEKEY);
                if (maintenanceInfo == null)
                {
                    if (mutext.WaitOne(WAITINTERVAL))
                    {
                        try
                        {
                            maintenanceInfo = GetMaintenanceInfo();
                            this.cacheManager.Set(MAINTENANCECACHEKEY, maintenanceInfo, DateTime.Now.AddMinutes(CACHETIMEOUTMINUTE));
                        }
                        catch
                        {
                            maintenanceInfo = new UnderMaintenanceModel { IsDownTime = false };
                        }
                        finally
                        {
                            mutext.ReleaseMutex();
                        }
                    }
                    else
                    {
                        maintenanceInfo = new UnderMaintenanceModel { IsDownTime = false };
                    }
                }
                return maintenanceInfo;
            }
        }

        private UnderMaintenanceModel GetMaintenanceInfo()
        {
            var maintenanceInfo = new UnderMaintenanceModel { IsDownTime = false };
            var startTime = maintenanceInfo.StartTime;
            var endTime = maintenanceInfo.EndTime;

            // Check downtime from AppSetting
            if (AppSettings.IsDownTime)
            {
                maintenanceInfo.IsDownTime = true;
                maintenanceInfo.StartTime = AppSettings.StartTime;
                maintenanceInfo.EndTime = AppSettings.EndTime;

                return maintenanceInfo;
            }

            // Check downtime from library
            try
            {
                maintenanceInfo.IsDownTime = umService.isUnderMaintainence(Sites.Agency, AppSettings.UnderMaintenanceUrl, ref startTime, ref endTime);
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
                return new UnderMaintenanceModel { IsDownTime = false };
            }
        }
    }
}