// --------------------------------------------------------------------------
// <copyright file="Constant.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.5.8">Created</change>
//   <change who="Ricky.Nguyen" date="2013.5.13">Update Const</change>
// </history>

namespace Apollo.Age.Web
{
    public static class WebConstants
    {
        public const string APOLLO_CURRENT_SESSION_KEY = "Apollo.Age.Web.CurrentSession";

        public const string AccountStoppedUrl = "~/Internal/AccountStopped";
        
        public const string UnauthorizedUrl = "~/Internal/Unauthorized";

        public const string InternalSignInUrl = "~/Internal/SignIn";
        
        public const int UN_AUTHORIZED_CODE = 401;
    }
}