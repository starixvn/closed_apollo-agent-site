// --------------------------------------------------------------------------
// <copyright file="PermissionUserControlController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.10.2">Created</change>
// </history>

namespace Apollo.Age.Account.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Apollo.Age.Core;
    using Apollo.Age.Criterias;
    using Apollo.Age.Services.Impl;
    using Apollo.Age.Services.Interface;
    using Apollo.Age.Web;
    using Apollo.Age.Web.Web.Attribute;
    using Apollo.Age.Entities;
    using Apollo.Age.Models;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PermissionUserControlController : BaseController
    {
        private readonly IFunctionService _functionService;
        private readonly IUserService _userService;

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
        public PermissionUserControlController(IFunctionService functionService, IUserService userService)
        {
            _functionService = functionService ?? new FunctionService();
            _userService = userService ?? new UserService();
        }

        /// <summary>
        /// Assigns the function to agent user.
        /// </summary>
        /// <param name="agentUserId">The agent user identifier.</param>
        /// <returns>Return view assign permission to agent user.</returns>
        [ApolloAuth(FunctionKey = ApolloFunctionKeys.PermissionControl, Policy = Policy.Query)]
        public ActionResult AssignPermissionControl(int parentAgentId, int agentUserId)
        {
            // Get agent user
            var agentUserInfo = _userService.GetUserById(agentUserId);

            var listFunctionPolicy = new List<FunctionPolicy>();
            var listParentFunctionPolicy = new List<FunctionPolicy>();
            var listFunction = new List<Function>();

            // Get list function which user has permission
            listFunctionPolicy = _userService.GetAllUserPolicy(userId: agentUserId, isActive: true).ToList();

            // Get all function is active
            listFunction = _functionService.GetAll(isActive: true).ToList();

            // Get parent agent and his agent user (is master), if user is shadow will return direct agent
            var parentInfo = _userService.GetUserParentInfo(agentUserId);

            // Get list function which parent user has permission
            listParentFunctionPolicy = _userService.GetAllUserPolicy(userId: parentInfo.Id, isActive: true).ToList();

            // Get permission of current user on this function
            var policy = _userService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.PermissionControl);

            var criteria = new UserAssignFunctionCriteria
            {
                ListFunction = listFunction,
                ListParentFunctionPolicy = listParentFunctionPolicy,
                ListFunctionPolicy = listFunctionPolicy,
                IsMaster = agentUserInfo.IsMaster,
                AgentUserId = agentUserId,
                HasEdit = policy == (byte)Policy.Edit && CurrentSession.UserId != agentUserId && CurrentSession.IsMaster,
                AgentUsername = agentUserInfo.Agent.Username,
                ShadowUsername = agentUserInfo.Username,
                AgentId = agentUserInfo.Agent.Id,
                IsDirectAgent = parentInfo.Agent.Id == CurrentSession.AgentId,
                ParentAgentId = parentAgentId
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
            var agentUserInfo = _userService.GetUserById(agentUserId);

            var listFunctionPolicy = new List<FunctionPolicy>();
            var listParentFunctionPolicy = new List<FunctionPolicy>();
            var listFunction = new List<Function>();

            // Get list function which user has permission
            listFunctionPolicy = _userService.GetAllUserPolicy(userId: agentUserId, isActive: true).ToList();

            // Get all function is active
            listFunction = _functionService.GetAll(isActive: true).ToList();

            // Get list function which parent user has permission
            listParentFunctionPolicy = _userService.GetAllUserPolicy(userId: agentUserInfo.Agent.Id, isActive: true).ToList();

            // Get permission of current user on this function
            var policy = _userService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.ShadowPermission);

            var criteria = new UserAssignFunctionCriteria
            {
                ListFunction = listFunction,
                ListParentFunctionPolicy = listParentFunctionPolicy,
                ListFunctionPolicy = listFunctionPolicy,
                IsMaster = agentUserInfo.IsMaster,
                AgentUserId = agentUserId,
                HasEdit = policy == (byte)Policy.Edit && CurrentSession.UserId != agentUserId && CurrentSession.IsMaster,
                AgentUsername = agentUserInfo.Agent.Username,
                ShadowUsername = agentUserInfo.Username,
                AgentId = agentUserInfo.Agent.Id
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
            var list = _userService.GetUserByAgent(agentId);
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
        public ActionResult AssignPermissionControl(UserAssignFunctionCriteria criteria)
        {
            criteria.ActorAgentId = CurrentSession.AgentId;
            criteria.AdminId = CurrentSession.AdminId;
            criteria.Language = CurrentSession.Language;

            //// Get agent user
            var agentUserInfo = _userService.GetUserById(criteria.AgentUserId);

            if (agentUserInfo.IsMaster)
            {
                // Get permission of current user on this function
                var policyPermissionControl = _userService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.PermissionControl);

                if (policyPermissionControl != (byte)Policy.Edit)
                {
                    return Json(new { ErrCode = -1 });
                }
            }
            else
            {
                // Get permission of current user on this function
                var policyShadowPermission = _userService.GetUserPolicy(CurrentSession.UserId, ApolloFunctionKeys.ShadowPermission);

                if (policyShadowPermission != (byte)Policy.Edit)
                {
                    return Json(new { ErrCode = -1 });
                }
            }

            _userService.AssignUserPermission(ref criteria);

            return Json(new
            {
                ErrCode = criteria.ErrCode,
                ErrMsg = criteria.ErrMsg
            });
        }

        #endregion
    }
}
