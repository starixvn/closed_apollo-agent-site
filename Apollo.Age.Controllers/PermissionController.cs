// --------------------------------------------------------------------------
// <copyright file="PermissionController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.29">Created</change>
// </history>

namespace Apollo.Age.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Apollo.Age.Web;
    using System.Web.Mvc;
    using Sunplus.Agent.Model.Entities;
    using Apollo.Age.Services.Impl;
    using Fanex.Age.UI;

    public class PermissionController : BaseController
    {
        public JsonResult IsKickOut()
        {
            IUserSession userSession = UserSession.Current;
            Result result = new Result(0, string.Empty);
            if (PermissionService.IsKickOut(userSession.AgentId, userSession.UserId))
            {
                result.ErrCode = (int)ErrorType.KickedOut;
            }
            return Json(result);
        }
    }
}