// --------------------------------------------------------------------------
// <copyright file="UMBusiness.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.9.24">Created</change>
// </history>

namespace Apollo.UMService
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Web.Helpers;
    using Apollo.UMService.Model;
    using Fanex.Caching;

    /// <summary>
    /// Under maintenance class.
    /// </summary>
    public class UMBusiness
    {
        private const string MaintenanceCacheKey = "Fanex.Cache.Apollo.Maintenance"; // Cache Key.
        private const int _BUFFER_SIZE = 1024;
        private const int _DefaultTimeout = 30000; // 0.5 minutes timeout
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private readonly ICache _cacheManager;
        private HttpWebRequest _request;
        private string _underMaintenanceUrlFormat = "{0}?site={1}";

        public UMBusiness()
        {
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        /// <summary>
        /// Gets the maintenance information.
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <param name="underMaintenanceCtrlUrl">The um control URL.</param>
        /// <returns>Return a under maintenance info.</returns>
        public UnderMaintenanceModel GetMaintenanceInfo(string siteName, string underMaintenanceCtrlUrl)
        {
            var underMaintenanceInfo = new UnderMaintenanceModel();
            underMaintenanceInfo = _cacheManager.Get<UnderMaintenanceModel>(MaintenanceCacheKey);

            if (underMaintenanceInfo == null)
            {
                underMaintenanceCtrlUrl = string.Format(_underMaintenanceUrlFormat, underMaintenanceCtrlUrl, siteName);

                this.GetMaintenanceInfo(underMaintenanceCtrlUrl);
            }

            return underMaintenanceInfo;
        }

        /// <summary>
        /// Gets the maintenance information.
        /// </summary>
        /// <param name="underMaintenanceCtrlUrl">The um control URL.</param>
        private void GetMaintenanceInfo(string underMaintenanceCtrlUrl)
        {
            var underMaintenanceInfo = new UnderMaintenanceModel();

            _request = (HttpWebRequest)WebRequest.Create(underMaintenanceCtrlUrl);

            // Create an instance of the RequestState and assign the previous myHttpWebRequest
            // object to its request field.
            RequestState requestState = new RequestState();
            requestState.Request = _request;

            // Start the asynchronous request.
            IAsyncResult result =
              (IAsyncResult)_request.BeginGetResponse(new AsyncCallback(RespCallback), requestState);

            // this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), _request, _DefaultTimeout, true);

            // The response came in the allowed time. The work processing will happen in the
            // callback function.
            allDone.WaitOne();

            // Release the HttpWebResponse resource.
            if (requestState.Response != null)
            {
                requestState.Response.Close();
            }
        }

        /// <summary>
        /// Timeouts the callback.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="timedOut">Set time out or not.</param>
        private void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }

        /// <summary>
        /// Response the callback.
        /// </summary>
        /// <param name="asynchronousResult">The asynchronous result.</param>
        private void RespCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                // State of request is asynchronous.
                RequestState requestState = (RequestState)asynchronousResult.AsyncState;
                HttpWebRequest httpWebRequest = requestState.Request;
                requestState.Response = (HttpWebResponse)httpWebRequest.EndGetResponse(asynchronousResult);

                // Read the response into a Stream object.
                Stream responseStream = requestState.Response.GetResponseStream();
                requestState.StreamResponse = responseStream;

                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = responseStream.BeginRead(requestState.BufferRead, 0, _BUFFER_SIZE, new AsyncCallback(ReadCallBack), requestState);
                return;
            }
            catch
            {
                allDone.Set();
            }
        }

        /// <summary>
        /// Reads the call back.
        /// </summary>
        /// <param name="asyncResult">The asynchronous result.</param>
        private void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {
                RequestState requestState = (RequestState)asyncResult.AsyncState;
                Stream responseStream = requestState.StreamResponse;
                int read = responseStream.EndRead(asyncResult);

                // Read the HTML page and then cache to memory cache
                if (read > 0)
                {
                    requestState.RequestData.Append(Encoding.ASCII.GetString(requestState.BufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(requestState.BufferRead, 0, _BUFFER_SIZE, new AsyncCallback(ReadCallBack), requestState);

                    return;
                }
                else
                {
                    if (requestState.RequestData.Length > 1)
                    {
                        var underMaintenanceInfo = Json.Decode<UnderMaintenanceModel>(requestState.RequestData.ToString());
                        _cacheManager.Set<UnderMaintenanceModel>(MaintenanceCacheKey, underMaintenanceInfo, TimeSpan.FromSeconds(5));

                        // LogService.Info(umInfo.status.ToString() + " - " + umInfo.start_time + " - " + umInfo.end_time);
                    }

                    responseStream.Close();

                    allDone.Set();
                }
            }
            catch
            {
                allDone.Set();
            }
        }
    }
}