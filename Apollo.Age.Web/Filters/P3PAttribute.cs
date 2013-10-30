// --------------------------------------------------------------------------
// <copyright file="P3PAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Lewis.Le" date="2013.5.22">Created</change>
// </history>

namespace Apollo.Age.Web.Filters
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Privacy Preferences Project (P3P) serve a compact policy (a "p3p" HTTP header) for all requests
    /// P3P provides a standard way for Web sites to communicate about their practices around the collection, 
    /// use, and distribution of personal information. It's a machine-readable privacy policy that can be 
    /// automatically fetched and viewed by users, and it can be tailored to fit your company's specific policies.
    /// </summary>
    /// <remarks>
    /// More info http://www.oreillynet.com/lpt/a/1554
    /// ref http://stackoverflow.com/questions/389456/cookie-blocked-not-saved-in-iframe-in-internet-explorer.
    /// </remarks>
    public class P3PAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// On Action Executing add a compact policy "p3p" HTTP header.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

            base.OnActionExecuting(filterContext);
        }
    }
}