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
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Helpers;
    using Apollo.UMService.Model;
    using Fanex.Caching;

    /// <summary>
    /// Under maintenance class.
    /// </summary>
    public class UMBusiness
    {
        private const string MaintenanceCacheKey = "Fanex.Cache.Apollo.Maintenance"; // Cache Key.

        private const int _DefaultTimeout = 30000; // 0.5 minutes timeout

        private readonly ICache _cacheManager;

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

                var task = MakeAsyncRequest(underMaintenanceCtrlUrl, "text/json");

                underMaintenanceInfo = Json.Decode<UnderMaintenanceModel>(task.Result);

                underMaintenanceInfo = underMaintenanceInfo != null ? underMaintenanceInfo : new UnderMaintenanceModel { Status = false };

                this._cacheManager.Set<UnderMaintenanceModel>(MaintenanceCacheKey, underMaintenanceInfo, 5);
            }

            return underMaintenanceInfo;
        }

        /// <summary>
        /// Makes the asynchronous request.
        /// </summary>
        /// <param name="url">The URL of maintenance service.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>Return task asynchronous request.</returns>
        public Task<string> MakeAsyncRequest(string url, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = WebRequestMethods.Http.Get;
            request.Timeout = _DefaultTimeout;
            request.Proxy = null;

            Task<WebResponse> task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
                (object)null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
        }

        /// <summary>
        /// Reads the stream from response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns>Return UM content.</returns>
        private string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(responseStream))
                {
                    ////Need to return this response 
                    string strContent = sr.ReadToEnd();

                    return strContent;
                }
            }
        }
    }
}