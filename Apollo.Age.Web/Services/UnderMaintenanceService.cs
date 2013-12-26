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
                var underMaintenanceInfo = new UnderMaintenanceModel { Status = false };

                // Check downtime from AppSetting.
                if (AppSettings.IsApolloDownTime)
                {
                    underMaintenanceInfo.Status = true;
                    underMaintenanceInfo.Start_time = AppSettings.StartTime;
                    underMaintenanceInfo.End_time = AppSettings.EndTime;

                    return underMaintenanceInfo;
                }

                // Check downtime from library.
                try
                {
                    if (AppSettings.DisabledUnderMaintenanceAPI)
                    {
                        underMaintenanceInfo.Status = false;
                    }
                    else
                    {
                        underMaintenanceInfo = _underMaintenanceBusiness.GetMaintenanceInfo(Sites.Agent, AppSettings.UnderMaintenanceUrl);

                        if (underMaintenanceInfo == null)
                        {
                            return new UnderMaintenanceModel();
                        }
                    }

                    return underMaintenanceInfo;
                }
                catch
                {
                    return new UnderMaintenanceModel { Status = false };
                }
            }
        }
    }
}