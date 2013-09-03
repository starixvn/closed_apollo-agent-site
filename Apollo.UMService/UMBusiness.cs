// --------------------------------------------------------------------------
// <copyright file="UMService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.9.3">Created</change>
// </history>

namespace Apollo.UMService
{
    using System;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Web.Helpers;
    using Apollo.UMService.Model;
    using Fanex.Caching;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UMBusiness
    {
        private const int WaitInterval = 1000; // 1 second.
        private const string MaintenanceCacheKey = "Fanex.Cache.Apollo.Maintenance"; // Cache Key.        
        private readonly ICache _cacheManager;
        private HttpWebRequest _request;
        private string _umUrlFormat = "{0}?site={1}";
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private const int BUFFER_SIZE = 1024;
        private const int DefaultTimeout = 2 * 60 * 1000; // 2 minutes timeout 

        public UMBusiness()
        {
            _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);
        }

        public UnderMaintenanceModel GetMaintainenceInfo(string siteName, string umCtrlUrl)
        {
            var umInfo = new UnderMaintenanceModel();
            umInfo = _cacheManager.Get<UnderMaintenanceModel>(MaintenanceCacheKey);

            if (umInfo == null)
            {
                umCtrlUrl = string.Format(_umUrlFormat, umCtrlUrl, siteName);

                umInfo = this.GetMaintainenceInfo(umCtrlUrl);
                //_cacheManager.Set<UnderMaintenanceModel>(MaintenanceCacheKey, umInfo, TimeSpan.FromMinutes(0)); //TimeSpan.FromMinutes(0));
            }

            return umInfo;
        }

        private UnderMaintenanceModel GetMaintainenceInfo(string umCtrlUrl)
        {
            var umInfo = new UnderMaintenanceModel();

            _request = (HttpWebRequest)WebRequest.Create(umCtrlUrl);

            // Create an instance of the RequestState and assign the previous myHttpWebRequest 
            // object to its request field.  
            RequestState myRequestState = new RequestState();
            myRequestState.Request = _request;

            // Start the asynchronous request.
            IAsyncResult result =
              (IAsyncResult)_request.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);

            // this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), _request, DefaultTimeout, true);

            // The response came in the allowed time. The work processing will happen in the  
            // callback function.
            allDone.WaitOne();

            // Release the HttpWebResponse resource.
            myRequestState.Response.Close();

            return umInfo;
        }

        // Abort the request if the timer fires. 
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

        private void RespCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                // State of request is asynchronous.
                RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
                HttpWebRequest myHttpWebRequest = myRequestState.Request;
                myRequestState.Response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);

                // Read the response into a Stream object.
                Stream responseStream = myRequestState.Response.GetResponseStream();
                myRequestState.StreamResponse = responseStream;

                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                return;
            }
            catch (WebException e)
            {
            }

            allDone.Set();
        }

        private void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {

                RequestState myRequestState = (RequestState)asyncResult.AsyncState;
                Stream responseStream = myRequestState.StreamResponse;
                int read = responseStream.EndRead(asyncResult);
                // Read the HTML page and then print it to the console. 
                if (read > 0)
                {
                    myRequestState.RequestData.Append(Encoding.ASCII.GetString(myRequestState.BufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                    return;
                }
                else
                {
                    if (myRequestState.RequestData.Length > 1)
                    {
                        var umInfo = Json.Decode<UnderMaintenanceModel>(myRequestState.RequestData.ToString());
                        _cacheManager.Set<UnderMaintenanceModel>(MaintenanceCacheKey, umInfo, TimeSpan.FromSeconds(10));
                    }

                    responseStream.Close();
                }
            }
            catch (WebException e)
            {
            }

            allDone.Set();
        }
    }
}
