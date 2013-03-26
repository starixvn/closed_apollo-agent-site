/*
 * Created 20100205@Lee - Maintenance configuration.
 * Dependencies: LoadBalancer.dll-->LoadBalancerLibrary
 * Revision:
 * 20120606@Kaden - Change library from LoadBalancer.dll to UMControllerLibrary.dll
 *
 */

namespace Sunplus.Agent.Model.Services
{
    using System;
    using Fanex.Age.Utility;
    using Sunplus.Agent.Model.BL.Definitions;
    using Sunplus.UMControllerLibrary;

    public class MaintenanceService
    {
        /// <summary>
        /// Get downtime information.
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static bool GetDownTime(out string startTime, out string endTime)
        {
            // Check manually UM first, in AppSetting.ini
            if (AppSettings.GetValue("IsUM") == "1")
            {
                startTime = AppSettings.GetValue("UM_Start");
                endTime = AppSettings.GetValue("UM_End");
                return true;
            }

            startTime = endTime = null;
            string url = AppSettings.LBUrl;
            bool isUM = false;

            if (string.IsNullOrEmpty(url))
            {
                return false;
            }

            try
            {
                UMBusiness business = new UMBusiness();
                isUM = business.isUnderMaintainence(Sites.Agency, url, ref startTime, ref endTime);
            }
            catch (Exception ex)
            {
                Logs.Write("LoadBalancerLibrary Error : " + ex.Message);
            }

            return isUM;
        }
    }
}