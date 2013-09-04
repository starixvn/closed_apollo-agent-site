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
    using Apollo.Age.Configurations;
    using Apollo.Age.Services.Impl;
    using Apollo.UMService;
    using Apollo.UMService.Model;

    /// <summary>
    /// Under Maintenance Service.
    /// </summary>
    public class UnderMaintenanceService : ServiceBase
    {
        private UMBusiness _underMaintenanceBusiness;

        public UnderMaintenanceService()
        {
            _underMaintenanceBusiness = new UMBusiness();
        }

        public UnderMaintenanceModel MaintenanceInfo
        {
            get
            {
                var underMaintenanceInfo = new UnderMaintenanceModel { status = false };
                var startTime = underMaintenanceInfo.start_time;
                var endTime = underMaintenanceInfo.end_time;

                // Check downtime from AppSetting.
                if (AppSettings.IsApolloDownTime)
                {
                    underMaintenanceInfo.status = true;
                    underMaintenanceInfo.start_time = AppSettings.StartTime;
                    underMaintenanceInfo.end_time = AppSettings.EndTime;

                    return underMaintenanceInfo;
                }

                // Check downtime from library.
                try
                {
                    if (AppSettings.DisabledUnderMaintenanceAPI)
                    {
                        underMaintenanceInfo.status = false;
                    }
                    else
                    {
                        underMaintenanceInfo = _underMaintenanceBusiness.GetMaintainenceInfo(Sites.Agent, AppSettings.UnderMaintenanceUrl);
                    }

                    return underMaintenanceInfo;
                }
                catch (Exception ex)
                {
                    HandleException(ex);

                    return new UnderMaintenanceModel { status = false };
                }
            }
        }
    }
}