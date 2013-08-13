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

        public UnderMaintenanceService()
        {
            _underMaintenanceService = new UMBusiness();
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
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
                            maintenanceInfo = new UnderMaintenanceViewModel { IsDownTime = false };
                        }
                        finally
                        {
                            _threadMuText.ReleaseMutex();
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

        //#region Will be Removed
        ///// <summary>
        ///// Checking the day is last day in the month.
        ///// </summary>
        ///// <param name="date">Now date.</param>
        ///// <param name="day">Day need to check.</param>
        ///// <returns>Return boolean.</returns>
        //private bool IsLastDayInMonth(DateTime date, string day)
        //{
        //    if (date.DayOfWeek.ToString().ToLower() != day.ToLower())
        //    {
        //        return false; // It is not monday.
        //    }

        //    // The next monday is...
        //    var oneWeekAfter = date.AddDays(7);

        //    // And is it in same month?, if it is, that means its not last monday.
        //    return oneWeekAfter.Month != date.Month;
        //}

        ///// <summary>
        ///// Get date time at time zone GMT+8.
        ///// </summary>
        ///// <returns>Return date time.</returns>
        //private DateTime TimeInfo()
        //{
        //    return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "Taipei Standard Time");
        //}

        ///// <summary>
        ///// Singbet maintenance info.
        ///// </summary>
        ///// <returns>Return Under Maintenance View Model.</returns>
        //private UnderMaintenanceViewModel SingbetMaintenanceInfo()
        //{
        //    var singbetDowntime = AppSettings.SingbetDownTime;
        //    var day = string.Empty;
        //    var lastDay = string.Empty;
        //    var startTime = string.Empty;
        //    var endTime = string.Empty;
        //    var timeZoneDate = TimeInfo();
        //    if (string.IsNullOrWhiteSpace(singbetDowntime))
        //    {
        //        return new UnderMaintenanceViewModel
        //        {
        //            IsDownTime = false
        //        };
        //    }
        //    else
        //    {
        //        // Every Monday from 11:00 PM to 13:00 PM or Last Monday in a Month from 9:00 AM to 13:00 PM
        //        //Monday@11:00 PM - 13:00 PM | Monday@9:00 AM - 13:00 PM
        //        string[] singbetDowntimeOptions = singbetDowntime.Split("|".ToCharArray());
        //        if (singbetDowntimeOptions.Length <= 0)
        //        {
        //            return new UnderMaintenanceViewModel
        //            {
        //                IsDownTime = false
        //            };
        //        }
        //        else
        //        {
        //            string[] days = singbetDowntimeOptions[0].Split("@".ToCharArray());
        //            string[] lastDays = singbetDowntimeOptions[1].Split("@".ToCharArray());

        //            bool isLastDayInMonth = IsLastDayInMonth(timeZoneDate, lastDays[0].Trim());
        //            if (isLastDayInMonth)
        //            {
        //                string[] times = lastDays[1].Split("-".ToCharArray());
        //                if (times.Length > 0)
        //                {
        //                    startTime = times[0].ToString().Trim();
        //                    endTime = times[1].ToString().Trim();

        //                    DateTime timeStart = Convert.ToDateTime(startTime);
        //                    DateTime timeEnd = Convert.ToDateTime(endTime);

        //                    if (timeStart <= timeZoneDate && timeZoneDate <= timeEnd)
        //                    {
        //                        return new UnderMaintenanceViewModel
        //                        {
        //                            IsDownTime = true,
        //                            StartTime = startTime,
        //                            EndTime = endTime,
        //                            UMDate = timeZoneDate.ToString("dd MMMM yyy")
        //                        };
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (timeZoneDate.DayOfWeek.ToString().ToLower() == days[0].Trim().ToLower())
        //                {
        //                    string[] times = days[1].Split("-".ToCharArray());
        //                    if (times.Length > 0)
        //                    {
        //                        startTime = times[0].ToString().Trim();
        //                        endTime = times[1].ToString().Trim();

        //                        DateTime timeStart = Convert.ToDateTime(startTime);
        //                        DateTime timeEnd = Convert.ToDateTime(endTime);
        //                        if (timeStart < timeZoneDate && timeZoneDate < timeEnd)
        //                        {
        //                            return new UnderMaintenanceViewModel
        //                            {
        //                                IsDownTime = true,
        //                                StartTime = startTime,
        //                                EndTime = endTime,
        //                                UMDate = timeZoneDate.ToString("dd MMMM yyy")
        //                            };
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return new UnderMaintenanceViewModel
        //    {
        //        IsDownTime = false
        //    };
        //}
        //#endregion

        ///// <summary>
        ///// Checking status of Server before open socket.
        ///// </summary>
        ///// <param name="IpAddress">Input Ip address.</param>
        ///// <param name="timeout">Time out.</param>
        ///// <returns>Return true or false.</returns>
        //private static bool GetPingResponse(string IpAddress, int timeout = 3000)
        //{
        //    var ping = new Ping();
        //    IPAddress ipAddress = IPAddress.Parse(IpAddress);

        //    var reply = ping.Send(ipAddress, timeout);
        //    return reply.Status == IPStatus.Success;
        //}

        ///// <summary>
        ///// Checking the status of tcp server.
        ///// </summary>
        ///// <param name="hostURL">Input Ip Address.</param>
        ///// <param name="port">Input port</param>
        ///// <returns>Return true or false.</returns>
        //private static bool PingHost(string hostURL, int port)
        //{
        //    try
        //    {
        //        if (GetPingResponse(hostURL, 180))
        //        {
        //            TcpClient client = new TcpClient(hostURL, port);
        //            if (client.Connected)
        //                return true;
        //            return false;
        //        }
        //        return false;
        //    }
        //    catch
        //    {

        //        return false;
        //    }
        //}

        /// <summary>
        /// Get IBC Maintenance Info.
        /// </summary>
        /// <returns>Return Under Maintenance View Model.</returns>
        private UnderMaintenanceViewModel GetMaintenanceInfo()
        {
            var maintenanceInfo = new UnderMaintenanceViewModel { IsDownTime = false };
            var startTime = maintenanceInfo.StartTime;
            var endTime = maintenanceInfo.EndTime;

            // Check downtime from AppSetting.
            if (AppSettings.IsApolloDownTime)
            {
                maintenanceInfo.IsDownTime = true;
                maintenanceInfo.StartTime = AppSettings.StartTime;
                maintenanceInfo.EndTime = AppSettings.EndTime;

                return maintenanceInfo;
            }

            // Check downtime from library.
            try
            {
                //StringBuilder logTest = new StringBuilder();

                if (AppSettings.DisabledUnderMaintenanceAPI)
                {
                    maintenanceInfo.IsDownTime = false;
                }
                else
                {
                    //string[] arrAddress = AppSettings.UnderMaintenanceUrl.Split(":".ToCharArray());
                    //string serverIP = "10.40.41.112";
                    //int port = 8990;
                    //if (arrAddress.Length > 0)
                    //{
                    //    serverIP = arrAddress[1].Replace("//", "");
                    //    port = Convert.ToInt32(arrAddress[2].Substring(0, arrAddress[2].IndexOf("/")));
                    //}

                    //if (PingHost(serverIP, port))
                    //{
                    //    maintenanceInfo.IsDownTime = _underMaintenanceService.isUnderMaintainence(Sites.Agency, AppSettings.UnderMaintenanceUrl, ref startTime, ref endTime);
                    //}

                    maintenanceInfo.IsDownTime = _underMaintenanceService.isUnderMaintainence(Sites.Agency, AppSettings.UnderMaintenanceUrl, ref startTime, ref endTime);
                }

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