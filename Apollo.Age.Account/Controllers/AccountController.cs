// --------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Ricky.Nguyen" date="2013.3.29">Created</change>
// </history>


namespace Apollo.Age.Account.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Apollo.Age.Web;
    using System.Web.Mvc;
    using Sunplus.Agent.Model.Entities;
    using Fanex.Age.UI;
    using Apollo.Age.Services.Impl;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AccountController : BaseController
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
