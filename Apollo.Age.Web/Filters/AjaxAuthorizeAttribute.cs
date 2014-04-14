// --------------------------------------------------------------------------
// <copyright file="AjaxAuthorizeAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2014.4.14">Created</change>
// </history>

namespace Apollo.Age.Web.Filters
{
    using System.Web.Mvc;

    /// <summary>
    /// Ajax Authorize Attribute.
    /// </summary>
    public class AjaxAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}