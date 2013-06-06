// --------------------------------------------------------------------------
// <copyright file="CustomValidateAntiForgeryTokenAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.6.6">Created</change>
// </history>

namespace Apollo.Age.Web.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Helpers;
    using System.Net;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CustomValidateAntiForgeryTokenAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            //  Ajax POSTs and normal form posts have to be treated differently when it comes
            //  to validating the AntiForgeryToken
            if (request.IsAjaxRequest())
            {
                var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                var cookieValue = antiForgeryCookie != null
                    ? antiForgeryCookie.Value
                    : null;
                try
                {
                    AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                }
                catch (HttpAntiForgeryException ex)
                {
                    Logger.Log.Error(ex.Message, ex);
                    var response = filterContext.HttpContext.Response;
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        response.Clear();
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        response.StatusDescription = string.Empty;
                        response.Write(Resources.Lang.UnauthorizedMsg);
                        response.End();
                    }
                }
            }
            else
            {
                new ValidateAntiForgeryTokenAttribute()
                    .OnAuthorization(filterContext);
            }
        }
    }
}
