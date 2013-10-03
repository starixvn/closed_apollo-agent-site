// --------------------------------------------------------------------------
// <copyright file="PermissionUserControlController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.PermissionControl.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Apollo.Age.PermissionControl.Entities;
    using Apollo.Age.PermissionControl.Services;
    using Apollo.Age.Web;
    using System.Linq;
    using Apollo.Age.PermissionControl.Criterias;
    using Fanex.NAP.Framework.Data;
    using Apollo.Age.PermissionControl.Models;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PermissionUserControlController : BaseController
    {
        private readonly IFunctionService _functionService;
        private readonly IAgentUserService _agentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionUserController"/> class.
        /// </summary>
        public PermissionUserControlController()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionUserController"/> class.
        /// </summary>
        /// <param name="functionService">The function service.</param>
        public PermissionUserControlController(IFunctionService functionService, IAgentUserService agentUserService)
        {
            _functionService = functionService ?? new FunctionService();
            _agentUserService = agentUserService ?? new AgentUserService();
        }

        /// <summary>
        /// Assigns the function to agent user.
        /// </summary>
        /// <param name="agentUserId">The agent user identifier.</param>
        /// <returns>Return view assign permission to agent user.</returns>
        public ActionResult AssignFunction(int agentUserId)
        {
            var agentUserInfo = _agentUserService.GetAgentUserInfo(agentUserId);

            var listFunctionPolicy = new List<FunctionPolicy>();
            var listParentFunctionPolicy = new List<FunctionPolicy>();
            var listFunction = new List<Function>();

            listFunctionPolicy = _agentUserService.GetUserFunctionPolicy(agentUserId: agentUserId, isActive: true).ToList();
            listFunction = _functionService.GetAll(isActive: true).ToList();

            var parentInfo = _agentUserService.GetParentInfo(agentUserId, agentUserInfo.IsMaster);
            listParentFunctionPolicy = _agentUserService.GetUserFunctionPolicy(agentUserId: parentInfo.Id, isActive: true).ToList();

            var criteria = new AgentUserAssignFunctionCriteria
            {
                ListFunction = listFunction,
                ListParentFunctionPolicy = listParentFunctionPolicy,
                ListFunctionPolicy = listFunctionPolicy,
                IsMaster = agentUserInfo.IsMaster,
                AgentUserId = agentUserId
            };

            return View(criteria);
        }

        #region Ajax

        [HttpPost]
        public ActionResult AssignUserPermission(AgentUserAssignFunctionCriteria criteria)
        {
            criteria.ActorAgentId = CurrentSession.AgentId;

            var command = _agentUserService.AssignUserPermission(criteria);

            return Json(command);
        }

        #endregion
    }
}
