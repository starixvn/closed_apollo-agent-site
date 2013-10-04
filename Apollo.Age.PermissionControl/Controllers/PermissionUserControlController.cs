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
    using System.Linq;
    using System.Web.Mvc;
    using Apollo.Age.PermissionControl.Core;
    using Apollo.Age.PermissionControl.Criterias;
    using Apollo.Age.PermissionControl.Entities;
    using Apollo.Age.PermissionControl.Models;
    using Apollo.Age.PermissionControl.Services;
    using Apollo.Age.PermissionControl.Web.Attribute;
    using Apollo.Age.Resources;
    using Apollo.Age.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PermissionUserControlController : BaseController
    {
        private readonly IFunctionService _functionService;
        private readonly IAgentUserService _agentUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionUserControlController"/> class.
        /// </summary>
        public PermissionUserControlController()
            : this(null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionUserControlController"/> class.
        /// </summary>
        /// <param name="functionService">The function service.</param>
        /// <param name="agentUserService">The agent user service.</param>
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
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.PermissionControl, Policy = Policy.Query)]
        public ActionResult AssignPermissionControl(int agentUserId)
        {
            // Get agent user
            var agentUserInfo = _agentUserService.GetAgentUserInfo(agentUserId);

            var listFunctionPolicy = new List<FunctionPolicy>();
            var listParentFunctionPolicy = new List<FunctionPolicy>();
            var listFunction = new List<Function>();

            // Get list function which user has permission
            listFunctionPolicy = _agentUserService.GetUserFunctionPolicy(agentUserId: agentUserId, isActive: true).ToList();

            // Get all function is active
            listFunction = _functionService.GetAll(isActive: true).ToList();

            // Get parent agent and his agent user (is master), if user is shadow will return direct agent
            var parentInfo = _agentUserService.GetParentInfo(agentUserId, agentUserInfo.IsMaster);

            // Get list function which parent user has permission
            listParentFunctionPolicy = _agentUserService.GetUserFunctionPolicy(agentUserId: parentInfo.Id, isActive: true).ToList();

            // Get permission of current user on this function
            var policy = _agentUserService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.PermissionControl);

            var criteria = new AgentUserAssignFunctionCriteria
            {
                ListFunction = listFunction,
                ListParentFunctionPolicy = listParentFunctionPolicy,
                ListFunctionPolicy = listFunctionPolicy,
                IsMaster = agentUserInfo.IsMaster,
                AgentUserId = agentUserId,
                HasEdit = policy == (byte)Policy.Edit && CurrentSession.UserId != agentUserId,
                AgentUsername = agentUserInfo.Agent.Username,
                ShadowUsername = agentUserInfo.Username
            };

            return View(criteria);
        }

        /// <summary>
        /// Assigns the shadow permission.
        /// </summary>
        /// <param name="agentUserId">The agent user identifier.</param>
        /// <returns>Return view assign permission to agent user.</returns>
        public ActionResult AssignShadowPermission(int agentUserId)
        {
            // Get agent user
            var agentUserInfo = _agentUserService.GetAgentUserInfo(agentUserId);

            var listFunctionPolicy = new List<FunctionPolicy>();
            var listParentFunctionPolicy = new List<FunctionPolicy>();
            var listFunction = new List<Function>();

            // Get list function which user has permission
            listFunctionPolicy = _agentUserService.GetUserFunctionPolicy(agentUserId: agentUserId, isActive: true).ToList();

            // Get all function is active
            listFunction = _functionService.GetAll(isActive: true).ToList();

            // Get parent agent and his agent user (is master), if user is shadow will return direct agent
            var parentInfo = _agentUserService.GetParentInfo(agentUserId, agentUserInfo.IsMaster);

            // Get list function which parent user has permission
            listParentFunctionPolicy = _agentUserService.GetUserFunctionPolicy(agentUserId: parentInfo.Id, isActive: true).ToList();

            // Get permission of current user on this function
            var policy = _agentUserService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.ShadowPermission);

            var criteria = new AgentUserAssignFunctionCriteria
            {
                ListFunction = listFunction,
                ListParentFunctionPolicy = listParentFunctionPolicy,
                ListFunctionPolicy = listFunctionPolicy,
                IsMaster = agentUserInfo.IsMaster,
                AgentUserId = agentUserId,
                HasEdit = policy == (byte)Policy.Edit && CurrentSession.UserId != agentUserId,
                AgentUsername = agentUserInfo.Agent.Username,
                ShadowUsername = agentUserInfo.Username
            };

            return View("AssignPermissionControl", criteria);
        }

        /// <summary>
        /// Agents the user list.
        /// </summary>
        /// <param name="agentId">The agent identifier.</param>
        /// <param name="username">The username.</param>
        /// <returns>Agent user list.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.ShadowPermission, Policy = Policy.Query)]
        public ActionResult AgentUserList(int agentId, string username)
        {
            var list = _agentUserService.GetByAgent(agentId);
            ViewBag.Username = username;

            return View(list);
        }

        #region Ajax

        /// <summary>
        /// Assigns the user permission.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command result when assign function to user..</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.PermissionControl + ";" + ApolloFunctionKeys.ShadowPermission, Policy = Policy.Edit)]
        [HttpPost]
        public ActionResult AssignPermissionControl(AgentUserAssignFunctionCriteria criteria)
        {
            criteria.ActorAgentId = CurrentSession.AgentId;

            //// Get agent user
            var agentUserInfo = _agentUserService.GetAgentUserInfo(criteria.AgentUserId);

            if (agentUserInfo.IsMaster)
            {
                // Get permission of current user on this function
                var policyPermissionControl = _agentUserService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.PermissionControl);

                if (policyPermissionControl != (byte)Policy.Edit)
                {
                    return Json(-1);
                }
            }
            else
            {
                // Get permission of current user on this function
                var policyShadowPermission = _agentUserService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.ShadowPermission);

                if (policyShadowPermission != (byte)Policy.Edit)
                {
                    return Json(-1);
                }
            }

            var command = _agentUserService.AssignUserPermission(criteria);

            return Json(0);
        }

        #endregion
    }
}
