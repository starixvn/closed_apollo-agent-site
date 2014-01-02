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
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using Fanex.Logging;

    /// <summary>
    /// Custom validate anti forgery token attribute.
    /// </summary>
    public class CustomValidateAntiForgeryTokenAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Need to review here.")]
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            // Ajax POSTs and normal form posts have to be treated differently when it comes. To validating the AntiForgeryToken.
            if (request.IsAjaxRequest())
            {
                var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                var cookieValue = antiForgeryCookie != null
                    ? antiForgeryCookie.Value
                    : null;

                var isValid = true;
                try
                {
                    if (request.Headers["__RequestVerificationToken"] == null || cookieValue == null)
                    {
                        isValid = false;
                    }
                    else
                    {
                        AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                    }
                }
                catch (HttpAntiForgeryException ex)
                {
                    Logger.Log.Error(ex.Message, ex);
                    isValid = false;
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.Message, ex);
                    isValid = false;
                }
                finally
                {
                    if (!isValid)
                    {
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
            }
            else
            {
                new ValidateAntiForgeryTokenAttribute()
                    .OnAuthorization(filterContext);
            }
        }
    }
}