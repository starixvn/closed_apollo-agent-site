// --------------------------------------------------------------------------
// <copyright file="ErrorCode.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2013.5.7">Created</change>
// </history>

namespace Apollo.Age.Integration.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// It contains error code returned from authentication service of sungame
    /// </summary>
    public static class ErrorCode
    {
        public const string InvalidRequest = "invalid_request";
        public const string UnauthorizedClient = "unauthorized_client";
        public const string AccessDenied = "access_denied";
        public const string UnsupportedResponseType = "unsupported_response_type";
        public const string ServerError = "server_error";
        public const string TemporarilyUnavailable = "temporarily_unavailable";
        public const string ComingSoon = "coming_soon";
    }
}
