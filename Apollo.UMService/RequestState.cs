// --------------------------------------------------------------------------
// <copyright file="RequestState.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.9.3">Created</change>
// </history>

namespace Apollo.UMService
{
    using System.IO;
    using System.Net;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RequestState
    {
        // This class stores the State of the request.
        private const int BUFFERSIZE = 1024;

        public RequestState()
        {
            BufferRead = new byte[BUFFERSIZE];
            RequestData = new StringBuilder(string.Empty);
            Request = null;
            StreamResponse = null;
        }

        public StringBuilder RequestData { get; set; }

        public byte[] BufferRead { get; set; }

        public HttpWebRequest Request { get; set; }

        public HttpWebResponse Response { get; set; }

        public Stream StreamResponse { get; set; }
    }
}