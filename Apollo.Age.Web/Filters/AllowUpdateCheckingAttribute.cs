// --------------------------------------------------------------------------
// <copyright file="AllowUpdateCheckingAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Terry.Nguyen" date="2013.8.1">Created</change>
// </history>

namespace Apollo.Age.Web.Filters
{
    using System;
    using System.Web.Mvc;
    using Fanex.Logging;

    public class AllowUpdateCheckingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                if (SessionManager.CurrentSession != null)
                {
                    if (SessionManager.CurrentSession.IsMaster == false)
                    {
                        CommonChecking.DoUnAuthorized(filterContext);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message, ex);
            }

            base.OnActionExecuted(filterContext);
        }
    }
}