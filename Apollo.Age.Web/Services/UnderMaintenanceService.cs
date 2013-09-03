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
        private UMBusiness _umBusiness;

        public UnderMaintenanceService()
        {
            _umBusiness = new UMBusiness();
        }

        public UnderMaintenanceModel MaintenanceInfo
        {
            get
            {
                var umInfo = new UnderMaintenanceModel { status = false };
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
                        umInfo = _umBusiness.GetMaintainenceInfo(Sites.Agent, AppSettings.UnderMaintenanceUrl);
                    }

                    return umInfo;
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